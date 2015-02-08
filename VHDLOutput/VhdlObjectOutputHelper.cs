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
using VHDL.expression;
using VHDL.expression.name;
using VHDL.Object;

namespace VHDL.output
{
    /// <summary>
    /// VHDL output helper class.
    /// </summary>
    internal class VhdlObjectOutputHelper
    {

        private VhdlObjectOutputHelper()
        {
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
    }

}