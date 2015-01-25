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
    public static class VHDLOperandGenerator
    {
        public static string GetOperand(Expression expression, VHDLCompilerInterface compiler, bool GenerateGetOperandFunction = true)
        {
            if (expression is Aggregate)
                throw new NotImplementedException();

            if (expression is Abs)
                throw new NotImplementedException();

            if (expression is Plus)
                return GetPlusOperand(expression as Plus, compiler);

            if (expression is Minus)
                return GetMinusOperand(expression as Minus, compiler);

            if (expression is Not)
                return GetNotOperand(expression as Not, compiler);

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
                return GetParenthesesOperand(expression as Parentheses, compiler);

            if (expression is SubtypeIndicationAllocator)
                throw new NotImplementedException();

            if (expression is Mod)
                return GetModOperand(expression as Mod, compiler);

            if (expression is Rem)
                throw new NotImplementedException();

            if (expression is Divide)
                return GetDivideOperand(expression as Divide, compiler);

            if (expression is Multiply)
                return GetMultiplyOperand(expression as Multiply, compiler);

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
                return GetPlusOperand(expression as Add, compiler);

            if (expression is Concatenate)
                throw new NotImplementedException();

            if (expression is Subtract)
                return GetMinusOperand(expression as Subtract, compiler);

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
                return GetEqualsOperand(expression as Equals, compiler);

            if (expression is NotEquals)
                return GetNotEqualsOperand(expression as NotEquals, compiler);

            if (expression is GreaterThan)
                return GetGreaterThanOperand(expression as GreaterThan, compiler);

            if (expression is GreaterEquals)
                return GetGreaterEqualsOperand(expression as GreaterEquals, compiler);

            if (expression is LessEquals)
                return GetLessEqualsOperand(expression as LessEquals, compiler);

            if (expression is LessThan)
                return GetLessThanOperand(expression as LessThan, compiler);

            if (expression is RecordElement)
                return GetRecordOperand(expression as RecordElement, compiler, GenerateGetOperandFunction);

            if (expression is IntegerLiteral)
                return GetDecimalLiteralOperand(expression as IntegerLiteral, compiler);

            if (expression is RealLiteral)
                return GetRealLiteralOperand(expression as RealLiteral, compiler);

            if (expression is CharacterLiteral)
                throw new NotImplementedException();

            if (expression is PhysicalLiteral)
            {
                return GetPhysicalLiteralOperand(expression as PhysicalLiteral, compiler);
            }

            if (expression is VHDL.type.EnumerationType.IdentifierEnumerationLiteral)
            {
                return GetIdentifierEnumerationLiteralOperand(expression as VHDL.type.EnumerationType.IdentifierEnumerationLiteral, compiler);
            }

            if (expression is VhdlObject)
            {
                return GetObjectOperand(expression as VhdlObject, compiler, GenerateGetOperandFunction);
            }

            throw new NotImplementedException();
        }

        public static string GetIdentifierEnumerationLiteralOperand(VHDL.type.EnumerationType.IdentifierEnumerationLiteral expression, VHDLCompilerInterface compiler)
        {
            return compiler.LiteralDictionary[expression.getLiteral()];
        }

        public static string GetObjectOperand(VHDL.Object.VhdlObject expression, VHDLCompilerInterface compiler, bool GenerateGetOperandFunction = true)
        {
            string valueProviderName = compiler.ObjectDictionary[expression];
            if (string.IsNullOrEmpty(valueProviderName))
            {
                return expression.Identifier;
            }
            else
            {
                if (GenerateGetOperandFunction)
                {
                    valueProviderName = GetValueFunctionCall(valueProviderName);
                }
                return valueProviderName;
            }
        }

        public static string GetVariableOperand(VHDL.Object.Variable expression, VHDLCompilerInterface compiler)
        {
            return compiler.ObjectDictionary[expression];
        }

        public static string GetConstantOperand(VHDL.Object.Constant expression, VHDLCompilerInterface compiler)
        {
            return compiler.ObjectDictionary[expression];
        }

        public static string GetRecordOperand(RecordElement expression, VHDLCompilerInterface compiler, bool GenerateGetOperandFunction = true)
        {
            Name prefix = expression.getPrefix();
            if (prefix is VHDL.Object.VhdlObject)
            {
                string valueProviderName = compiler.ObjectDictionary[prefix as VHDL.Object.VhdlObject];
                if (string.IsNullOrEmpty(valueProviderName))
                {
                    return expression.getElement();
                }
                else
                {
                    if (GenerateGetOperandFunction)
                    {
                        valueProviderName = GetValueFunctionCall(valueProviderName);
                    }
                    return valueProviderName;
                }
            }

            throw new NotImplementedException();
        }

        public static string GetValueFunctionCall(string target)
        {
            FunctionCallTemplate template = new FunctionCallTemplate(target, "GetValue");
            return template.TransformText();
        }

        public static string GetParenthesesOperand(Parentheses expression, VHDLCompilerInterface compiler)
        {
            return (GetOperand(expression.Expression, compiler));
        }

        public static string GetDecimalLiteralOperand(IntegerLiteral expression, VHDLCompilerInterface compiler)
        {
            string value = expression.IntegerValue.ToString(CultureInfo.InvariantCulture);
            NewStatementTemplate template = new NewStatementTemplate("VHDLIntegerValue", value);
            return template.TransformText();
        }

        public static string GetRealLiteralOperand(RealLiteral expression, VHDLCompilerInterface compiler)
        {
            string value = expression.RealValue.ToString(CultureInfo.InvariantCulture);
            NewStatementTemplate template = new NewStatementTemplate("VHDLFloatingPointValue", value);
            return template.TransformText();
        }

        public static string GetPhysicalLiteralOperand(PhysicalLiteral expression, VHDLCompilerInterface compiler)
        {
            string unitName = string.Format("\"{0}\"", expression.Unit);
            Int64 val = (expression.Value as IntegerLiteral).IntegerValue;
            string physType = compiler.TypeDictionary[expression.GetPhysicalType()];
            NewStatementTemplate template = new NewStatementTemplate(physType, val, unitName);
            return template.TransformText();
        }

        public static string GetPlusOperand(Plus expression, VHDLCompilerInterface compiler)
        {
            string destination = GetOperand(expression.Expression, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(destination, "UnaryPlus");
            return template.TransformText();
        }

        public static string GetMinusOperand(Minus expression, VHDLCompilerInterface compiler)
        {
            string destination = GetOperand(expression.Expression, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(destination, "UnaryMinus");
            return template.TransformText();
        }

        public static string GetNotOperand(Not expression, VHDLCompilerInterface compiler)
        {
            string destination = GetOperand(expression.Expression, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(destination, "Not");
            return template.TransformText();
        }

        public static string GetModOperand(Mod expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Mod", right);
            return template.TransformText();
        }

        public static string GetDivideOperand(Divide expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Divide", right);
            return template.TransformText();
        }

        public static string GetMultiplyOperand(Multiply expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Multiply", right);
            return template.TransformText();
        }

        public static string GetPlusOperand(Add expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Plus", right);
            return template.TransformText();
        }

        public static string GetMinusOperand(Subtract expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Minus", right);
            return template.TransformText();
        }

        public static string GetLessThanOperand(LessThan expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "LessThan", right);
            return template.TransformText();
        }

        public static string GetLessEqualsOperand(LessEquals expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "LessEquals", right);
            return template.TransformText();
        }

        public static string GetGreaterThanOperand(GreaterThan expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "GreaterThan", right);
            return template.TransformText();
        }

        public static string GetGreaterEqualsOperand(GreaterEquals expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "GreaterEquals", right);
            return template.TransformText();
        }

        public static string GetNotEqualsOperand(NotEquals expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "NotEquals", right);
            return template.TransformText();
        }

        public static string GetEqualsOperand(Equals expression, VHDLCompilerInterface compiler)
        {
            string left = GetOperand(expression.Left, compiler);
            string right = GetOperand(expression.Right, compiler);
            FunctionCallTemplate template = new FunctionCallTemplate(left, "Equals", right);
            return template.TransformText();
        }
    }
}
