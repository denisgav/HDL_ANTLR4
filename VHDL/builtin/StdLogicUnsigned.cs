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

namespace VHDL.builtin
{

    using UseClause = VHDL.libraryunit.UseClause;
    using FunctionDeclaration = VHDL.declaration.FunctionDeclaration;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using Constant = VHDL.Object.Constant;

    /// <summary>
    /// STD_LOGIC_UNSIGNED package wrapper.
    /// </summary>
    public class StdLogicUnsigned
    {

        /// Use clause for all declarations in this package. 
        public static readonly UseClause USE_CLAUSE = new UseClause("ieee.std_logic_unsigned.all");
        //public static final FunctionDeclaration SHL_SLV_SLV_SLV = new FunctionDeclaration("SHL", StdLogic1164.STD_LOGIC_VECTOR, new Constant("ARG", StdLogic1164.STD_LOGIC_VECTOR), new Constant("COUNT", StdLogic1164.STD_LOGIC_VECTOR));
        //public static final FunctionDeclaration SHR_SLV_SLV_SLV = new FunctionDeclaration("SHR", StdLogic1164.STD_LOGIC_VECTOR, new Constant("ARG", StdLogic1164.STD_LOGIC_VECTOR), new Constant("COUNT", StdLogic1164.STD_LOGIC_VECTOR));

        /// CONV_INTEGER function. 
        public static readonly FunctionDeclaration CONV_INTEGER_SLV_INTEGER = new FunctionDeclaration("CONV_INTEGER", Standard.INTEGER, new Constant("ARG", StdLogic1164.STD_LOGIC_VECTOR));

        /// STD_LOGIC_UNSIGNED package. 
        public static readonly PackageDeclaration PACKAGE = new PackageDeclaration("std_logic_unsigned");

        static StdLogicUnsigned()
        {
            //PACKAGE.getDeclarations().add(SHL_SLV_SLV_SLV);
            //PACKAGE.getDeclarations().add(SHR_SLV_SLV_SLV);
            PACKAGE.Declarations.Add(CONV_INTEGER_SLV_INTEGER);
        }

        //Prevent instantiation.
        private StdLogicUnsigned()
        {
        }
    }

}