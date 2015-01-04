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

    using Abs = VHDL.expression.Abs;
    using Add = VHDL.expression.Add;
    using And = VHDL.expression.And;
    using Concatenate = VHDL.expression.Concatenate;
    using Divide = VHDL.expression.Divide;
    using Equals = VHDL.expression.Equals;
    using Expression = VHDL.expression.Expression;
    using GreaterEquals = VHDL.expression.GreaterEquals;
    using GreaterThan = VHDL.expression.GreaterThan;
    using LessEquals = VHDL.expression.LessEquals;
    using LessThan = VHDL.expression.LessThan;
    using Mod = VHDL.expression.Mod;
    using Multiply = VHDL.expression.Multiply;
    using Nand = VHDL.expression.Nand;
    using Nor = VHDL.expression.Nor;
    using Not = VHDL.expression.Not;
    using NotEquals = VHDL.expression.NotEquals;
    using Or = VHDL.expression.Or;
    using Pow = VHDL.expression.Pow;
    using Rem = VHDL.expression.Rem;
    using Rol = VHDL.expression.Rol;
    using Ror = VHDL.expression.Ror;
    using Sla = VHDL.expression.Sla;
    using Sll = VHDL.expression.Sll;
    using Sra = VHDL.expression.Sra;
    using Subtract = VHDL.expression.Subtract;
    using Xnor = VHDL.expression.Xnor;
    using Xor = VHDL.expression.Xor;

    public abstract class TokenCreator
    {
        public static Expression create(Expression left, Expression right, TokenType type)
        {
            switch (type)
            {
                case TokenType.ABS:
                    return new Abs(left);
                case TokenType.AND:
                    return new And(left, right);
                case TokenType.MOD:
                    return new Mod(left, right);
                case TokenType.NAND:
                    return new Nand(left, right);
                case TokenType.NOR:
                    return new Nor(left, right);
                case TokenType.NOT:
                    return new Not(left);
                case TokenType.OR:
                    return new Or(left, right);
                case TokenType.REM:
                    return new Rem(left, right);
                case TokenType.ROL:
                    return new Rol(left, right);
                case TokenType.ROR:
                    return new Ror(left, right);
                case TokenType.SLA:
                    return new Sla(left, right);
                case TokenType.SLL:
                    return new Sll(left, right);
                case TokenType.SRA:
                    return new Sra(left, right);
                case TokenType.SRL:
                    return new Sla(left, right);
                case TokenType.XNOR:
                    return new Xnor(left, right);
                case TokenType.XOR:
                    return new Xor(left, right);
                case TokenType.AMPERSAND:
                    return new Concatenate(left, right);
                case TokenType.MUL:
                    return new Multiply(left, right);
                case TokenType.DOUBLESTAR:
                    return new Pow(left, right);
                case TokenType.LT:
                    return new LessThan(left, right);
                case TokenType.LE:
                    return new LessEquals(left, right);
                case TokenType.GT:
                    return new GreaterThan(left, right);
                case TokenType.GE:
                    return new GreaterEquals(left, right);
                case TokenType.NEQ:
                    return new NotEquals(left, right);
                case TokenType.DBLQUOTE:
                    return null;
                case TokenType.DIV:
                    return new Divide(left, right);
                case TokenType.PLUS:
                    return new Add(left, right);
                case TokenType.MINUS:
                    return new Subtract(left, right);
                case TokenType.EQ:
                    return new Equals(left, right);
                case TokenType.LPAREN:
                    return null;
                case TokenType.RPAREN:
                    return null;
                case TokenType.PLACEHOLDER:
                    return null;
                case TokenType.BINARY_BIT_STRING_LITERAL:
                    return null;
                case TokenType.HEX_BIT_STRING_LITERAL:
                    return null;
                case TokenType.OCTAL_BIT_STRING_LITERAL:
                    return null;
                case TokenType.STRING_LITERAL:
                    return null;
                case TokenType.CHARACTER_LITERAL:
                    return null;
                case TokenType.DECIMAL_LITERAL:
                    return null;
                case TokenType.BASED_LITERAL:
                    return null;
                case TokenType.WHITESPACE:
                    return null;
                case TokenType.EOF:
                    return null;
                case TokenType.ERROR:
                    return null;
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Expression  token type.
    /// </summary>
    public enum TokenType
    {
        ABS,
        AND,
        MOD,
        NAND,
        NOR,
        NOT,
        OR,
        REM,
        ROL,
        ROR,
        SLA,
        SLL,
        SRA,
        SRL,
        XNOR,
        XOR,
        AMPERSAND,
        MUL,
        DOUBLESTAR,
        LT,
        LE,
        GT,
        GE,
        NEQ,
        DBLQUOTE,
        DIV,
        PLUS,
        MINUS,
        EQ,
        LPAREN,
        RPAREN,
        PLACEHOLDER,
        BINARY_BIT_STRING_LITERAL,
        HEX_BIT_STRING_LITERAL,
        OCTAL_BIT_STRING_LITERAL,
        STRING_LITERAL,
        CHARACTER_LITERAL,
        DECIMAL_LITERAL,
        BASED_LITERAL,
        WHITESPACE,
        EOF,
        ERROR
    }

}