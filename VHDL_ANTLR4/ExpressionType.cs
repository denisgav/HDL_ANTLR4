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

namespace VHDL.parser.antlr
{

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
    using NotEquals = VHDL.expression.NotEquals;
    using Or = VHDL.expression.Or;
    using Rem = VHDL.expression.Rem;
    using Rol = VHDL.expression.Rol;
    using Ror = VHDL.expression.Ror;
    using Sla = VHDL.expression.Sla;
    using Sll = VHDL.expression.Sll;
    using Sra = VHDL.expression.Sra;
    using Srl = VHDL.expression.Srl;
    using Subtract = VHDL.expression.Subtract;
    using Xnor = VHDL.expression.Xnor;
    using Xor = VHDL.expression.Xor;

    /// <summary>
    /// Expression type.
    /// </summary>
    internal enum ExpressionType
    {
        AND,
        OR,
        NAND,
        NOR,
        XOR,
        XNOR,
        EQ,
        NEQ,
        LT,
        LE,
        GT,
        GE,
        SLL,
        SRL,
        SLA,
        SRA,
        ROL,
        ROR,
        ADD,
        SUB,
        CONCAT,
        MUL,
        DIV,
        MOD,
        REM
    }

    internal class ExpressionTypeCreator
    {
        public static Expression create(ExpressionType type, Expression l, Expression r)
        {
            switch (type)
            {
                case ExpressionType.AND:
                    return new And(l, r);
                case ExpressionType.OR:
                    return new Or(l, r);
                case ExpressionType.NAND:
                    return new Nand(l, r);
                case ExpressionType.NOR:
                    return new Nor(l, r);
                case ExpressionType.XOR:
                    return new Xor(l, r);
                case ExpressionType.XNOR:
                    return new Xnor(l, r);
                case ExpressionType.EQ:
                    return new Equals(l, r);
                case ExpressionType.NEQ:
                    return new NotEquals(l, r);
                case ExpressionType.LT:
                    return new LessThan(l, r);
                case ExpressionType.LE:
                    return new LessEquals(l, r);
                case ExpressionType.GT:
                    return new GreaterThan(l, r);
                case ExpressionType.GE:
                    return new GreaterEquals(l, r);
                case ExpressionType.SLL:
                    return new Sll(l, r);
                case ExpressionType.SRL:
                    return new Srl(l, r);
                case ExpressionType.SLA:
                    return new Sla(l, r);
                case ExpressionType.SRA:
                    return new Sra(l, r);
                case ExpressionType.ROL:
                    return new Rol(l, r);
                case ExpressionType.ROR:
                    return new Ror(l, r);
                case ExpressionType.ADD:
                    return new Add(l, r);
                case ExpressionType.SUB:
                    return new Subtract(l, r);
                case ExpressionType.CONCAT:
                    return new Concatenate(l, r);
                case ExpressionType.MUL:
                    return new Multiply(l, r);
                case ExpressionType.DIV:
                    return new Divide(l, r);
                case ExpressionType.MOD:
                    return new Mod(l, r);
                case ExpressionType.REM:
                    return new Rem(l, r);
                default:
                    return null;
            }
        }
    }
}