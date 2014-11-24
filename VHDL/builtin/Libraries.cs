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
    using LibraryDeclarativeRegion = VHDL.LibraryDeclarativeRegion;
    using VhdlFile = VHDL.VhdlFile;

    /// <summary>
    /// Builtin library wrapper.
    /// </summary>
    public class Libraries
    {

        /// IEEE library. 
        public static readonly LibraryDeclarativeRegion IEEE = new LibraryDeclarativeRegion("ieee");
        /// STD library. 
        public static readonly LibraryDeclarativeRegion STD = new LibraryDeclarativeRegion("std");

        static Libraries()
        {
            VhdlFile ieeeFile = new VhdlFile("ieee");
            ieeeFile.Elements.Add(StdLogic1164.PACKAGE);
            ieeeFile.Elements.Add(StdLogicArith.PACKAGE);
            ieeeFile.Elements.Add(StdLogicSigned.PACKAGE);
            ieeeFile.Elements.Add(StdLogicUnsigned.PACKAGE);
            ieeeFile.Elements.Add(NumericStd.PACKAGE);
            IEEE.Files.Add(ieeeFile);

            VhdlFile stdFile = new VhdlFile("std");
            stdFile.Elements.Add(Standard.PACKAGE);
            stdFile.Elements.Add(TextIO.PACKAGE);
            STD.Files.Add(stdFile);
        }

        //Prevent instantiation.
        private Libraries()
        {
        }
    }

}