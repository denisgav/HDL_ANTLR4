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
using System.Linq;
using System.Text;
using VHDL.expression;
using VHDL.Object;
using VHDL.literal;
using System.Globalization;
using VHDLCompiler.CodeTemplates.Helpers;

namespace VHDLCompiler.CodeGenerator
{
    public static class VHDLExpressionTypeGenerator
    {
        public static string GetExpressionType(Expression expression, VHDLCompilerInterface compiler)
        {
            if (expression is Aggregate)
                throw new NotImplementedException();

            if (expression is Abs)
                throw new NotImplementedException();

            if (expression is Plus)
                return GetPlusExpresstionType(expression as Plus, compiler);

            if (expression is Minus)
                return GetMinusExpresstionType(expression as Minus, compiler);

            if (expression is Not)
                return GetNotExpresstionType(expression as Not, compiler);

            if (expression is TypeConversion)
                throw new NotImplementedException();

            if (expression is FunctionCall)
                throw new NotImplementedException();

            if (expression is Aggregate)
                throw new NotImplementedException();

            if (expression is QualifiedExpression)
                throw new NotImplementedException();

            if (expression is QualifiedExpressionAllocator)
                throw new NotImplementedException();

            if (expression is Parentheses)
                return GetParenthesesExpresstionType(expression as Parentheses, compiler);

            if (expression is SubtypeIndicationAllocator)
                throw new NotImplementedException();

            if (expression is Mod)
                return GetModExpresstionType(expression as Mod, compiler);

            if (expression is Rem)
                throw new NotImplementedException();

            if (expression is Divide)
                return GetDivideExpresstionType(expression as Divide, compiler);

            if (expression is Multiply)
                return GetMultiplyExpresstionType(expression as Multiply, compiler);

            if (expression is Pow)
                throw new NotImplementedException();

            if (expression is Ror)
                throw new NotImplementedException();

            if (expression is Sla)
                throw new NotImplementedException();

            if (expression is Sll)
                throw new NotImplementedException();

            if (expression is Rol)
                throw new NotImplementedException();

            if (expression is Add)
                return GetPlusExpresstionType(expression as Add, compiler);

            if (expression is Concatenate)
                throw new NotImplementedException();

            if (expression is Subtract)
                return GetMinusExpresstionType(expression as Subtract, compiler);

            if (expression is Nand)
                throw new NotImplementedException();

            if (expression is Xnor)
                throw new NotImplementedException();

            if (expression is Nor)
                throw new NotImplementedException();

            if (expression is And)
                throw new NotImplementedException();

            if (expression is Or)
                throw new NotImplementedException();

            if (expression is Equals)
                return GetEqualsExpresstionType(expression as Equals, compiler);

            if (expression is NotEquals)
                return GetNotEqualsExpresstionType(expression as NotEquals, compiler);

            if (expression is GreaterThan)
                return GetGreaterThanExpresstionType(expression as GreaterThan, compiler);

            if (expression is GreaterEquals)
                return GetGreaterEqualsExpresstionType(expression as GreaterEquals, compiler);

            if (expression is LessEquals)
                return GetLessEqualsExpresstionType(expression as LessEquals, compiler);

            if (expression is LessThan)
                return GetLessThanExpresstionType(expression as LessThan, compiler);

            if (expression is RecordElement)
                return GetRecordExpresstionType(expression as RecordElement, compiler);

            if (expression is IntegerLiteral)
                return GetDecimalLiteralExpressionType(expression as IntegerLiteral, compiler);

            if (expression is RealLiteral)
                return GetRealLiteralExpresstionType(expression as RealLiteral, compiler);

            if (expression is CharacterLiteral)
                throw new NotImplementedException();

            if (expression is PhysicalLiteral)
            {
                return GetPhysicalLiteralExpresstionType(expression as PhysicalLiteral, compiler);
            }

            if (expression is VHDL.type.EnumerationType.IdentifierEnumerationLiteral)
            {
                return GetIdentifierEnumerationLiteralExpresstionType(expression as VHDL.type.EnumerationType.IdentifierEnumerationLiteral, compiler);
            }

            throw new NotImplementedException();
        }

        public static string GetIdentifierEnumerationLiteralExpresstionType(VHDL.type.EnumerationType.IdentifierEnumerationLiteral expression, VHDLCompilerInterface compiler)
        {
            return compiler.TypeDictionary[expression.Type];
        }

        public static string GetRecordExpresstionType(RecordElement expression, VHDLCompilerInterface compiler)
        {           
            return compiler.TypeDictionary[expression.Type];            
        }


        public static string GetParenthesesExpresstionType(Parentheses expression, VHDLCompilerInterface compiler)
        {
            return (GetExpressionType(expression.Expression, compiler));
        }

        public static string GetDecimalLiteralExpressionType(IntegerLiteral expression, VHDLCompilerInterface compiler)
        {
            return compiler.TypeDictionary[expression.Type];            
        }

        public static string GetRealLiteralExpresstionType(RealLiteral expression, VHDLCompilerInterface compiler)
        {
            return compiler.TypeDictionary[expression.Type];            
        }

        public static string GetPhysicalLiteralExpresstionType(PhysicalLiteral expression, VHDLCompilerInterface compiler)
        {
            return compiler.TypeDictionary[expression.Type];            
        }

        public static string GetPlusExpresstionType(Plus expression, VHDLCompilerInterface compiler)
        {
            string destination = GetExpressionType(expression.Expression, compiler);
            return destination;
        }

        public static string GetMinusExpresstionType(Minus expression, VHDLCompilerInterface compiler)
        {
            string destination = GetExpressionType(expression.Expression, compiler);
            return destination;
        }

        public static string GetNotExpresstionType(Not expression, VHDLCompilerInterface compiler)
        {
            string destination = GetExpressionType(expression.Expression, compiler);
            return destination;
        }

        public static string GetModExpresstionType(Mod expression, VHDLCompilerInterface compiler)
        {
            string left = GetExpressionType(expression.Left, compiler);
            return left;
        }

        public static string GetDivideExpresstionType(Divide expression, VHDLCompilerInterface compiler)
        {
            string left = GetExpressionType(expression.Left, compiler);
            return left;
        }

        public static string GetMultiplyExpresstionType(Multiply expression, VHDLCompilerInterface compiler)
        {
            string left = GetExpressionType(expression.Left, compiler);
            return left;
        }

        public static string GetPlusExpresstionType(Add expression, VHDLCompilerInterface compiler)
        {
            string left = GetExpressionType(expression.Left, compiler);
            return left;
        }

        public static string GetMinusExpresstionType(Subtract expression, VHDLCompilerInterface compiler)
        {
            string left = GetExpressionType(expression.Left, compiler);
            return left;
        }

        public static string GetLessThanExpresstionType(LessThan expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }

        public static string GetLessEqualsExpresstionType(LessEquals expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }

        public static string GetGreaterThanExpresstionType(GreaterThan expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }

        public static string GetGreaterEqualsExpresstionType(GreaterEquals expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }

        public static string GetNotEqualsExpresstionType(NotEquals expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }

        public static string GetEqualsExpresstionType(Equals expression, VHDLCompilerInterface compiler)
        {
            return "bool";
        }
        
        
    }
}
