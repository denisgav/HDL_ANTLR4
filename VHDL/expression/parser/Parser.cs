//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;


namespace VHDL.expression.parser
{

    using Abs = VHDL.expression.Abs;
    using Expression = VHDL.expression.Expression;
    using Minus = VHDL.expression.Minus;
    using Not = VHDL.expression.Not;
    using Parentheses = VHDL.expression.Parentheses;
    using Plus = VHDL.expression.Plus;
    using BasedLiteral = VHDL.literal.BasedLiteral;
    using CharacterLiteral = VHDL.literal.CharacterLiteral;
    using DecBasedInteger = VHDL.literal.DecBasedInteger;
    using BinaryStringLiteral = VHDL.literal.BinaryStringLiteral;
    using OctalStringLiteral = VHDL.literal.OctalStringLiteral;
    using HexStringLiteral = VHDL.literal.HexStringLiteral;
    using StringLiteral = VHDL.literal.StringLiteral;

    /// <summary>
    /// Expression parser parser.
    /// </summary>
    internal class Parser
    {

        private readonly Lexer input;
        private readonly List<Expression> parameters;
        private TokenType lookahead;
        private static readonly TokenType[][] binaryHierarchy;
        private const int SIMPLE_EXPRESSION_LEVEL = 3;

        static Parser()
        {
            binaryHierarchy = new TokenType[][] 
            { 
                new TokenType[] { TokenType.AND, TokenType.OR, TokenType.NAND, TokenType.NOR, TokenType.XOR, TokenType.XNOR },
                new TokenType[] { TokenType.EQ, TokenType.NEQ, TokenType.LT, TokenType.LE, TokenType.GT, TokenType.GE },
                new TokenType[] { TokenType.SLL, TokenType.SRL, TokenType.SLA, TokenType.SRA, TokenType.ROL, TokenType.ROR },
                new TokenType[] { TokenType.PLUS, TokenType.MINUS, TokenType.AMPERSAND },
                new TokenType[] { TokenType.MUL, TokenType.DIV, TokenType.MOD, TokenType.REM },
                new TokenType[] { TokenType.DOUBLESTAR } 
            };
        }

        public Parser(Lexer input, List<Expression> parameters)
        {
            this.input = input;
            this.parameters = parameters;

            lookahead = input.nextToken();
            if (lookahead == TokenType.ERROR)
            {
                throw new ArgumentException("lexical error");
            }
        }

        private string accept(TokenType t)
        {
            if (lookahead != t)
            {
                throw new ArgumentException("illegal token " + lookahead.ToString() + " (expecting " + t.ToString() + ")");
            }

            string text = input.getTokenText();

            lookahead = input.nextToken();
            if (lookahead == TokenType.ERROR)
            {
                throw new ArgumentException("lexical error");
            }
            return text;
        }

        private TokenType getBinaryType(int level)
        {
            foreach (TokenType token in binaryHierarchy[level])
            {
                if (token == lookahead)
                {
                    return token;
                }
            }

            return TokenType.NOR;
        }

        public virtual Expression getExpression()
        {
            Expression e = expression(0);

            if (lookahead != TokenType.EOF)
            {
                throw new ArgumentException("extraneous output at end of expression template (" + lookahead.ToString() + ")");
            }

            return e;
        }

        private Expression expression(int level)
        {
            if (level < binaryHierarchy.Length)
            {
                Expression ret;

                if (level == SIMPLE_EXPRESSION_LEVEL)
                {
                    if (lookahead == TokenType.PLUS)
                    {
                        accept(TokenType.PLUS);
                        ret = new Plus(expression(level + 1));
                    }
                    else if (lookahead == TokenType.MINUS)
                    {
                        accept(TokenType.MINUS);
                        ret = new Minus(expression(level + 1));
                    }
                    else
                    {
                        ret = expression(level + 1);
                    }
                }
                else
                {
                    ret = expression(level + 1);
                }

                for (; ; )
                {
                    TokenType type = getBinaryType(level);

                    if (type != null)
                    {
                        accept(type);
                        ret = TokenCreator.create(ret, expression(level + 1), type);
                    }
                    else
                    {
                        return ret;
                    }
                }
            }
            else
            {
                return factor();
            }
        }

        private Expression factor()
        {
            if (lookahead == TokenType.ABS)
            {
                accept(TokenType.ABS);
                return new Abs(primary());
            }
            else if (lookahead == TokenType.NOT)
            {
                accept(TokenType.NOT);
                return new Not(primary());
            }
            else
            {
                return primary();
            }
        }

        private Expression primary()
        {
            string text;

            switch (lookahead)
            {
                case TokenType.PLACEHOLDER:
                    text = accept(TokenType.PLACEHOLDER);
                    int index;
                    try
                    {
                        index = Convert.ToInt32(text.Substring(1)) - 1;
                    }
                    catch (FormatException ex)
                    {
                        throw new ArgumentException("illegal placeholder " + text);
                    }
                    if (index < 0 || index > parameters.Count - 1)
                    {
                        throw new ArgumentException("illegal placeholder " + text);
                    }
                    return parameters[index];

                case TokenType.LPAREN:
                    accept(TokenType.LPAREN);
                    Expression expr = new Parentheses(expression(0));
                    accept(TokenType.RPAREN);
                    return expr;

                case TokenType.DECIMAL_LITERAL:
                    text = accept(TokenType.DECIMAL_LITERAL);
                    return new DecBasedInteger(text);

                case TokenType.BASED_LITERAL:
                    text = accept(TokenType.BASED_LITERAL);
                    return new BasedLiteral(text);

                case TokenType.STRING_LITERAL:
                    text = accept(TokenType.STRING_LITERAL);
                    return new StringLiteral(text.Substring(1, text.Length - 1));

                case TokenType.CHARACTER_LITERAL:
                    text = accept(TokenType.CHARACTER_LITERAL);
                    return new CharacterLiteral(text[1]);

                case TokenType.BINARY_BIT_STRING_LITERAL:
                    text = accept(TokenType.BINARY_BIT_STRING_LITERAL);
                    return new BinaryStringLiteral(text.Substring(2, text.Length - 1));

                case TokenType.HEX_BIT_STRING_LITERAL:
                    text = accept(TokenType.HEX_BIT_STRING_LITERAL);
                    return new HexStringLiteral(text.Substring(2, text.Length - 1));

                case TokenType.OCTAL_BIT_STRING_LITERAL:
                    text = accept(TokenType.OCTAL_BIT_STRING_LITERAL);
                    return new OctalStringLiteral(text.Substring(2, text.Length - 1));

                default:
                    string errorToken = lookahead.ToString();
                    accept(lookahead);
                    throw new ArgumentException("illegal primary " + errorToken);
            }
        }
    }

}