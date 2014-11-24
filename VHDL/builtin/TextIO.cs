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

    using FileDeclaration = VHDL.declaration.FileDeclaration;
    using FunctionDeclaration = VHDL.declaration.FunctionDeclaration;
    using Subtype = VHDL.declaration.Subtype;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using StringLiteral = VHDL.literal.StringLiteral;
    using FileObject = VHDL.Object.FileObject;
    using AccessType = VHDL.type.AccessType;
    using EnumerationType = VHDL.type.EnumerationType;
    using FileType = VHDL.type.FileType;

    /// <summary>
    /// TEXTIO package wrapper.
    /// </summary>
    public class TextIO
    {

        /// LINE type. 
        public static readonly AccessType LINE = new AccessType("LINE", Standard.STRING);
        /// TEXT type. 
        public static readonly FileType TEXT = new FileType("TEXT", Standard.STRING);
        /// SIDE type. 
        public static readonly EnumerationType SIDE = new EnumerationType("SIDE", "RIGHT", "LEFT");
        /// WIDTH subtype. 
        public static readonly Subtype WIDTH = new Subtype("WIDTH", Standard.NATURAL);
        /// INPUT file. 
        public static readonly FileObject INPUT = new FileObject("INPUT", TEXT, Standard.FILE_OPEN_KIND_READ_MODE, new StringLiteral("STD_INPUT"));
        /// OUTPUT file. 
        public static readonly FileObject OUTPUT = new FileObject("OUTPUT", TEXT, Standard.FILE_OPEN_KIND_WRITE_MODE, new StringLiteral("STD_OUTPUT"));
        /// ENDFILE function. 
        public static readonly FunctionDeclaration ENDFILE = new FunctionDeclaration("ENDFILE", Standard.BOOLEAN, new FileObject("F", TEXT));

        /// TEXTIO package. 
        public static readonly PackageDeclaration PACKAGE = new PackageDeclaration("textio");

        static TextIO()
        {
            PACKAGE.Declarations.Add(LINE);
            PACKAGE.Declarations.Add(TEXT);
            PACKAGE.Declarations.Add(SIDE);
            PACKAGE.Declarations.Add(WIDTH);
            PACKAGE.Declarations.Add(new FileDeclaration(INPUT));
            PACKAGE.Declarations.Add(new FileDeclaration(OUTPUT));
            PACKAGE.Declarations.Add(ENDFILE);
        }

        //Prevent instantiation
        private TextIO()
        {
        }
    }

}