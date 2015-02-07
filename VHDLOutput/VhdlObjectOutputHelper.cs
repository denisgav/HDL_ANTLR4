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

namespace VHDL.output
{

    using VHDL.expression;
    using VHDL.Object;
    using System;

    /// <summary>
    /// VHDL output helper class.
    /// </summary>
    internal class VhdlObjectOutputHelper
    {

        private VhdlObjectOutputHelper()
        {
        }

        public static void name(Name name, VhdlWriter writer, OutputModule output)
        {
            if (name is SelectedName)
            {
                recordElement((SelectedName)name, writer, output);
            }
            else if (name is IndexedName)
            {
                arrayElement((IndexedName)name, writer, output);
            }
            else if (name is Slice)
            {
                slice((Slice)name, writer, output);
            }
            else if (name is AttributeExpression)
            {
                attributeExpression((AttributeExpression)name, writer, output);
            }
            else if (name is VhdlObject)
            {
                writer.AppendIdentifier((VhdlObject)name);
            }
            else if (name is FunctionCall)
            {
                output.getExpressionVisitor().visit((FunctionCall)name);
            }
        }

        public static void interfaceSuffix(VhdlObject @object, VhdlWriter writer, OutputModule output)
        {
            if (@object is Signal)
            {
                signalInterfaceSuffix((Signal)@object, writer, output);
            }
            else if (@object is Constant)
            {
                constantInterfaceSuffix((Constant)@object, writer, output);
            }
            else if (@object is Variable)
            {
                variableInterfaceSuffix((Variable)@object, writer, output);
            }
            else if (@object is FileObject)
            {
                fileInterfaceSuffix((FileObject)@object, writer, output);
            }
            else
            {
                throw new ArgumentException("Unknown interface element.");
            }
        }

        //TODO: write BUS keyword
        public static void signalInterfaceSuffix(Signal signal, VhdlWriter writer, OutputModule output)
        {
            output.writeSubtypeIndication(signal.Type);

            if (signal.Kind == Signal.KindEnum.BUS)
            {
                writer.Append(' ').Append(signal.Kind.ToString());
            }
            else if (signal.Kind == Signal.KindEnum.REGISTER)
            {
                throw new Exception("Signal kind register isn't allowed in an interface declaration");
            }

            if (signal.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(signal.DefaultValue);
            }
        }

        //TODO: add mode
        public static void variableInterfaceSuffix(Variable variable, VhdlWriter writer, OutputModule output)
        {
            output.writeSubtypeIndication(variable.Type);
            if (variable.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(variable.DefaultValue);
            }
        }

        public static void constantInterfaceSuffix(Constant constant, VhdlWriter writer, OutputModule output)
        {
            output.writeSubtypeIndication(constant.Type);
            if (constant.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(constant.DefaultValue);
            }
        }

        public static void fileInterfaceSuffix(FileObject file, VhdlWriter writer, OutputModule output)
        {
            output.writeSubtypeIndication(file.Type);
        }

        public static void slice(Slice slice, VhdlWriter writer, OutputModule output)
        {
            output.writeExpression(slice.Prefix);
            writer.Append('(');
            for (int i = 0; i < slice.Ranges.Count; i++)
            {
                DiscreteRange r = slice.Ranges[i];
                output.writeDiscreteRange(r);
                if(i < slice.Ranges.Count - 1)
                    writer.Append(", ");
            }
            writer.Append(')');
        }

        public static void arrayElement(IndexedName arrayElement, VhdlWriter writer, OutputModule output)
        {
            output.writeExpression(arrayElement.Prefix);
            writer.Append('(');
            bool first = true;
            foreach (Expression index in arrayElement.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }
                output.writeExpression(index);
            }
            writer.Append(')');
        }

        public static void recordElement(SelectedName recordElement, VhdlWriter writer, OutputModule output)
        {
            output.writeExpression(recordElement.getPrefix());
            writer.Append('.');
            writer.Append(recordElement.getElement());
        }

        public static void attributeExpression(AttributeExpression expression, VhdlWriter writer, OutputModule output)
        {
            output.writeExpression(expression.Prefix);
            writer.Append('\'');
            writer.AppendIdentifier(expression.Attribute);
            if (expression.Parameters.Count != 0)
            {
                writer.Append('(');
                for(int i=0; i<expression.Parameters.Count; i++)
                {
                    Expression p = expression.Parameters[i];
                    output.writeExpression(p);
                    if (i != expression.Parameters.Count - 1)
                    {
                        writer.Append(", ");
                    }
                }
                writer.Append(')');
            }
        }
    }

}