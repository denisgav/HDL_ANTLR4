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

namespace VHDL.expression.parser
{
    /// <summary>
    /// Expression parser lexer.
    /// </summary>
    internal class Lexer
    {

        private const char EOF = (char)0;
        private readonly string input;
        private int currentIndex;
        private System.Text.StringBuilder tokenText;

        public Lexer(string input)
        {
            this.input = input.ToLower();
        }

        private char consume()
        {
            if (currentIndex < input.Length)
            {
                char c = input[currentIndex++];
                tokenText.Append(c);
                return c;
            }
            else
            {
                return EOF;
            }
        }

        public virtual TokenType nextToken()
        {
            TokenType token;

            while ((token = nextTokenWithWhitespace()) == TokenType.WHITESPACE)
            {
                //do nothing
            }

            return token;
        }

        private TokenType nextTokenWithWhitespace()
        {
            tokenText = new System.Text.StringBuilder();

            switch (consume())
            {
                case 'a':
                    switch (consume())
                    {
                        case 'b':
                            return keywordOrError(TokenType.ABS, "s");

                        case 'n':
                            return keywordOrError(TokenType.AND, "d");
                    }
                    break;

                case 'b':
                    if (consume() != '\"')
                    {
                        return TokenType.ERROR;
                    }
                    based_integer();
                    if (consume() != '\"')
                    {
                        return TokenType.ERROR;
                    }
                    return TokenType.BINARY_BIT_STRING_LITERAL;

                case 'm':
                    return keywordOrError(TokenType.MOD, "od");

                case 'n':
                    switch (consume())
                    {
                        case 'a':
                            return keywordOrError(TokenType.NAND, "nd");

                        case 'o':
                            switch (consume())
                            {
                                case 'r':
                                    return TokenType.NOR;

                                case 't':
                                    return TokenType.NOT;
                            }
                            break;
                    }
                    break;

                case 'o':
                    switch (consume())
                    {
                        case 'r':
                            return TokenType.ABS;

                        case '\"':
                            based_integer();
                            if (consume() != '\"')
                            {
                                return TokenType.ERROR;
                            }
                            return TokenType.OCTAL_BIT_STRING_LITERAL;
                    }
                    break;

                case 'r':
                    switch (consume())
                    {
                        case 'e':
                            return keywordOrError(TokenType.REM, "m");

                        case 'o':
                            switch (consume())
                            {
                                case 'l':
                                    return TokenType.ROL;

                                case 'r':
                                    return TokenType.ROR;
                            }
                            break;
                    }
                    break;

                case 's':
                    switch (consume())
                    {
                        case 'l':
                            switch (consume())
                            {
                                case 'a':
                                    return TokenType.SLA;
                                case 'l':
                                    return TokenType.SLL;
                            }
                            break;
                        case 'r':
                            switch (consume())
                            {
                                case 'a':
                                    return TokenType.SRA;
                                case 'l':
                                    return TokenType.SRL;
                            }
                            break;
                    }
                    break;

                case 'x':
                    switch (consume())
                    {
                        case 'n':
                            return keywordOrError(TokenType.XNOR, "or");
                        case 'o':
                            return keywordOrError(TokenType.XOR, "r");
                        case '\"':
                            based_integer();
                            if (consume() != '\"')
                            {
                                return TokenType.ERROR;
                            }
                            return TokenType.HEX_BIT_STRING_LITERAL;
                    }
                    break;

                case '*':
                    if (lookahead() == '*')
                    {
                        consume();
                        return TokenType.DOUBLESTAR;
                    }
                    else
                    {
                        return TokenType.MUL;
                    }

                case '<':
                    if (lookahead() == '=')
                    {
                        consume();
                        return TokenType.LE;
                    }
                    else
                    {
                        return TokenType.LT;
                    }

                case '>':
                    if (lookahead() == '=')
                    {
                        consume();
                        return TokenType.GE;
                    }
                    else
                    {
                        return TokenType.GT;
                    }

                case '!':
                    if (consume() == '=')
                    {
                        return TokenType.NEQ;
                    }
                    return TokenType.ERROR;

                case '/':
                    return TokenType.DIV;

                case '+':
                    return TokenType.PLUS;

                case '-':
                    return TokenType.MINUS;

                case '=':
                    return TokenType.EQ;

                case '(':
                    return TokenType.LPAREN;

                case ')':
                    return TokenType.RPAREN;

                case '%':
                    integer();
                    return TokenType.PLACEHOLDER;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return decimal_or_based_literal();

                case '"':
                    while (lookahead() != '\"')
                    {
                        if (consume() == EOF)
                        {
                            return TokenType.ERROR;
                        }
                    }
                    consume();

                    return TokenType.STRING_LITERAL;

                case '\'':
                    consume();
                    if (consume() == '\'')
                    {
                        return TokenType.CHARACTER_LITERAL;
                    }
                    else
                    {
                        return TokenType.ERROR;
                    }

                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    return TokenType.WHITESPACE;

                case EOF:
                    return TokenType.EOF;
            }

            return TokenType.ERROR;
        }

        private TokenType decimal_or_based_literal()
        {
            TokenType type = TokenType.DECIMAL_LITERAL;
            integer();
            if (lookahead() == '.')
            {
                consume();
                if (!isDigit(lookahead()))
                {
                    return TokenType.ERROR;
                }
                integer();
            }
            else if (lookahead() == '#')
            {
                type = TokenType.BASED_LITERAL;
                consume();
                based_integer();
                if (lookahead() == '.')
                {
                    consume();
                    based_integer();
                }
                if (consume() != '#')
                {
                    return TokenType.ERROR;
                }
            }
            if (lookahead() == 'e')
            {
                consume();
                if (lookahead() == '-' || lookahead() == '+')
                {
                    consume();
                }
                if (!isDigit(lookahead()))
                {
                    return TokenType.ERROR;
                }
                integer();
            }
            return type;
        }

        private void based_integer()
        {
            bool first = true;

            char c = lookahead();

            while (isDigit(c) || (c >= 'a' && c <= 'f') || (!first && c == '_'))
            {
                first = false;
                consume();
                c = lookahead();
            }
        }

        private void integer()
        {
            while (isDigit(lookahead()) || lookahead() == '_')
            {
                consume();
            }
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private char lookahead()
        {
            if (currentIndex < input.Length)
            {
                return input[currentIndex];
            }
            else
            {
                return EOF;
            }
        }

        private TokenType keywordOrError(TokenType type, string text)
        {
            foreach (char c in text.ToCharArray())
            {
                if (c != consume())
                {
                    return TokenType.ERROR;
                }
            }

            return type;
        }

        public virtual string getTokenText()
        {
            return tokenText.ToString();
        }
    }

}