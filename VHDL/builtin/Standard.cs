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

    using Range = VHDL.Range;
    using FunctionDeclaration = VHDL.declaration.FunctionDeclaration;
    using Subtype = VHDL.declaration.Subtype;
    using Expression = VHDL.expression.Expression;
    using Subtract = VHDL.expression.Subtract;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using DecimalLiteral = VHDL.literal.DecBasedInteger;
    using EnumerationLiteral = VHDL.literal.EnumerationLiteral;
    using EnumerationType = VHDL.type.EnumerationType;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using IntegerType = VHDL.type.IntegerType;
    using RealType = VHDL.type.RealType;
    using PhysicalType = VHDL.type.PhysicalType;
    using RangeSubtypeIndication = VHDL.type.RangeSubtypeIndication;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using UnconstrainedArray = VHDL.type.UnconstrainedArray;

    /// <summary>
    /// STANDARD package wrapper class.
    /// </summary>
    public class Standard
    {

        /// BOOLEAN type. 
        public static readonly EnumerationType BOOLEAN = new EnumerationType("BOOLEAN", "FALSE", "TRUE");
        /// FALSE literal. 
        public static readonly EnumerationLiteral BOOLEAN_FALSE = BOOLEAN.Literals[0];
        /// TRUE literal. 
        public static readonly EnumerationLiteral BOOLEAN_TRUE = BOOLEAN.Literals[1];
        /// BIT type. 
        public static readonly EnumerationType BIT = new EnumerationType("BIT", '0', '1');
        /// '0' literal. 
        public static readonly EnumerationLiteral BIT_0 = BIT.Literals[0];
        /// '1' literal. 
        public static readonly EnumerationLiteral BIT_1 = BIT.Literals[1];
        /// CHARACTER type. 
        //TODO: add missing enumeration literals
        //TODO: add characters
        public static readonly EnumerationType CHARACTER = new EnumerationType("CHARACTER");
        /// SEVERITY_LEVEL type. 
        public static readonly EnumerationType SEVERITY_LEVEL = new EnumerationType("SEVERITY_LEVEL", "NOTE", "WARNING", "ERROR", "FAILURE");
        /// NOTE literal. 
        public static readonly EnumerationLiteral SEVERITY_LEVEL_NOTE = SEVERITY_LEVEL.Literals[0];
        /// WARNING literal. 
        public static readonly EnumerationLiteral SEVERITY_LEVEL_WARNING = SEVERITY_LEVEL.Literals[1];
        /// ERROR literal. 
        public static readonly EnumerationLiteral SEVERITY_LEVEL_ERROR = SEVERITY_LEVEL.Literals[2];
        /// FAILURE literal. 
        public static readonly EnumerationLiteral SEVERITY_LEVEL_FAILURE = SEVERITY_LEVEL.Literals[3];

        private static readonly Range INTEGER_RANGE = new Range(int.MinValue, Range.RangeDirection.TO, int.MaxValue);
        private static readonly Range REAL_RANGE = new Range(double.MinValue, Range.RangeDirection.TO, double.MaxValue);

        /// INTEGER type. 
        public static readonly IntegerType INTEGER = new IntegerType("INTEGER", INTEGER_RANGE);
        /// REAL type. 
        //TODO: fix range and type
        public static readonly RealType REAL = new RealType("REAL", REAL_RANGE);
        /// TIME type. 
        //TODO: replace with correct type
        public static readonly PhysicalType TIME = new PhysicalType("TIME", INTEGER_RANGE, "fs");

        /// DELAY_LENGTH type. 
        //TODO: use correct range
        public static readonly Subtype DELAY_LENGTH = new Subtype("DELAY_LENGTH", new RangeSubtypeIndication(TIME, new Range(0, Range.RangeDirection.TO, 1000)));
        /// NOW function. 
        //TODO: set pure
        public static readonly FunctionDeclaration NOW = new FunctionDeclaration("NOW", DELAY_LENGTH);
        //TODO: replace Integer.MAX_VALUE by "INTEGER'HIGH"
        private static readonly Range NATURAL_RANGE = new Range(0, Range.RangeDirection.TO, int.MaxValue);
        /// NATURAL type. 
        public static readonly Subtype NATURAL = new Subtype("NATURAL", new RangeSubtypeIndication(INTEGER, NATURAL_RANGE));
        //TODO: replace Integer.MAX_VALUE by "INTEGER'HIGH"
        private static readonly Range POSITIVE_RANGE = new Range(1, Range.RangeDirection.TO, int.MaxValue);
        /// POSITIVE type. 
        public static readonly Subtype POSITIVE = new Subtype("POSITIVE", new RangeSubtypeIndication(INTEGER, POSITIVE_RANGE));
        /// STRING type. 
        public static readonly UnconstrainedArray STRING = new UnconstrainedArray("STRING", CHARACTER, POSITIVE);
        /// BIT_VECTOR type. 
        public static readonly UnconstrainedArray BIT_VECTOR = new UnconstrainedArray("BIT_VECTOR", BIT, NATURAL);

        /// <summary>
        /// Creates a BIT_VECTOR(width -1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_BIT_VECTOR(int width)
        {
            return Create_BIT_VECTOR(new Range(width - 1, Range.RangeDirection.DOWNTO, 0));
        }

        /// <summary>
        /// Creates a BIT_VECTOR(width -1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_BIT_VECTOR(Expression width)
        {
            Expression from = new Subtract(width, new DecimalLiteral(1));
            Expression to = new DecimalLiteral(0);
            return Create_BIT_VECTOR(new Range(from, Range.RangeDirection.DOWNTO, to));
        }

        /// <summary>
        /// Creates a BIT_VECTOR(range) subtype indication.
        /// </summary>
        /// <param name="range">range the range</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_BIT_VECTOR(Range range)
        {
            return new IndexSubtypeIndication(BIT_VECTOR, range);
        }
        /// FILE_OPEN_KIND type. 
        public static readonly EnumerationType FILE_OPEN_KIND = new EnumerationType("FILE_OPEN_KIND", "READ_MODE", "WRITE_MODE", "APPEND_MODE");
        /// READ_MODE literal. 
        public static readonly EnumerationLiteral FILE_OPEN_KIND_READ_MODE = FILE_OPEN_KIND.Literals[0];
        /// WRITE_MODE literal. 
        public static readonly EnumerationLiteral FILE_OPEN_KIND_WRITE_MODE = FILE_OPEN_KIND.Literals[1];
        /// APPEND_MODE literal. 
        public static readonly EnumerationLiteral FILE_OPEN_KIND_APPEND_MODE = FILE_OPEN_KIND.Literals[2];
        /// FILE_OPEN_STATUS type. 
        public static readonly EnumerationType FILE_OPEN_STATUS = new EnumerationType("FILE_OPEN_STATUS", "OPEN_OK", "STATUS_ERROR", "NAME_ERROR", "MODE_ERROR");
        /// OPEN_OK literal. 
        public static readonly EnumerationLiteral FILE_OPEN_STATUS_OPEN_OK = FILE_OPEN_STATUS.Literals[0];
        /// STATUS_ERROR literal. 
        public static readonly EnumerationLiteral FILE_OPEN_STATUS_STATUS_ERROR = FILE_OPEN_STATUS.Literals[1];
        /// NAME_ERROR literal. 
        public static readonly EnumerationLiteral FILE_OPEN_STATUS_NAME_ERROR = FILE_OPEN_STATUS.Literals[2];
        /// MODE_ERROR literal. 
        public static readonly EnumerationLiteral FILE_OPEN_STATUS_MODE_ERROR = FILE_OPEN_STATUS.Literals[3];
        //TODO: add FOREIGN attribute
        /// STANDARD package. 
        public static readonly PackageDeclaration PACKAGE = new PackageDeclaration("standard");

        static Standard()
        {
            PACKAGE.Declarations.Add(BOOLEAN);
            PACKAGE.Declarations.Add(BIT);
            PACKAGE.Declarations.Add(CHARACTER);
            PACKAGE.Declarations.Add(SEVERITY_LEVEL);
            PACKAGE.Declarations.Add(INTEGER);
            PACKAGE.Declarations.Add(REAL);
            PACKAGE.Declarations.Add(TIME);
            PACKAGE.Declarations.Add(DELAY_LENGTH);
            PACKAGE.Declarations.Add(NOW);
            PACKAGE.Declarations.Add(NATURAL);
            PACKAGE.Declarations.Add(POSITIVE);
            PACKAGE.Declarations.Add(STRING);
            PACKAGE.Declarations.Add(BIT_VECTOR);
            PACKAGE.Declarations.Add(FILE_OPEN_KIND);
            PACKAGE.Declarations.Add(FILE_OPEN_STATUS);

            CHARACTER.AddLiterals("NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL", "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI", "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB", "CAN", "EM", "SUB", "ESC", "FSP", "GSP", "RSP", "USP", "DEL", "C128", "C129", "C130", "C131", "C132", "C133", "C134", "C135", "C136", "C137", "C138", "C139", "C140", "C141", "C142", "C143", "C144", "C145", "C146", "C147", "C148", "C149", "C150", "C151", "C152", "C153", "C154", "C155", "C156", "C157", "C158", "C159");
            CHARACTER.AddLiterals(' ', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4', '5', '6', '7', 	'8', '9', ':', ';', '<', '=', '>', '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~');

            TIME.createUnit("fs");
            TIME.createUnit("ps", 1000, "fs");
            TIME.createUnit("ns", 1000, "ps");
            TIME.createUnit("us", 1000, "ns");
            TIME.createUnit("ms", 1000, "us");
            TIME.createUnit("sec", 1000, "ms");
            TIME.createUnit("min", 60, "sec");
            TIME.createUnit("hr", 60, "min");
        }

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private Standard()
        {
        }
    }

}