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
    using UseClause = VHDL.libraryunit.UseClause;
    using FunctionDeclaration = VHDL.declaration.FunctionDeclaration;
    using Subtype = VHDL.declaration.Subtype;
    using Expression = VHDL.expression.Expression;
    using Subtract = VHDL.expression.Subtract;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using DecimalLiteral = VHDL.literal.DecBasedInteger;
    using EnumerationLiteral = VHDL.literal.EnumerationLiteral;
    using Constant = VHDL.Object.Constant;
    using Signal = VHDL.Object.Signal;
    using EnumerationType = VHDL.type.EnumerationType;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using RangeSubtypeIndication = VHDL.type.RangeSubtypeIndication;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using UnconstrainedArray = VHDL.type.UnconstrainedArray;

    /// <summary>
    /// STDLOGIC1164 package wrapper.
    /// </summary>
    public class StdLogic1164
    {

        /// Use clause for all declarations in this package. 
        public static readonly UseClause USE_CLAUSE = new UseClause("ieee.std_logic_1164.all");

        /// STD_ULOGIC type. 
        public static readonly EnumerationType STD_ULOGIC = new EnumerationType("std_ulogic", 'U', 'X', '0', '1', 'Z', 'W', 'L', 'H', '-');
        /// 'U' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_U = STD_ULOGIC.Literals[0];
        /// 'X' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_X = STD_ULOGIC.Literals[1];
        /// '0' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_0 = STD_ULOGIC.Literals[2];
        /// '1' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_1 = STD_ULOGIC.Literals[3];
        /// 'Z' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_Z = STD_ULOGIC.Literals[4];
        /// 'W' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_W = STD_ULOGIC.Literals[5];
        /// 'L' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_L = STD_ULOGIC.Literals[6];
        /// 'H' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_H = STD_ULOGIC.Literals[7];
        /// '-' literal. 
        public static readonly EnumerationLiteral STD_ULOGIC_DONT_CARE = STD_ULOGIC.Literals[8];
        /// STD_ULOGIC_VECTOR type. 
        public static readonly UnconstrainedArray STD_ULOGIC_VECTOR = new UnconstrainedArray("std_ulogic_vector", Standard.NATURAL, STD_ULOGIC);

        /// <summary>
        /// Creates a STD_ULOGIC_VECTOR(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param nathe subtype indicationme="width">the width</param>
        /// <returns></returns>
        public static SubtypeIndication Create_STD_ULOGIC_VECTOR(int width)
        {
            return Create_STD_ULOGIC_VECTOR(new Range(width - 1, Range.RangeDirection.DOWNTO, 0));
        }

        /// <summary>
        /// Creates a STD_ULOGIC_VECTOR(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_STD_ULOGIC_VECTOR(Expression width)
        {
            Expression from = new Subtract(width, new DecimalLiteral(1));
            Expression to = new DecimalLiteral(0);
            return Create_STD_ULOGIC_VECTOR(new Range(from, Range.RangeDirection.DOWNTO, to));
        }

        /// <summary>
        /// Creates a STD_ULOGIC_VECTOR(range) subtype indication.
        /// </summary>
        /// <param name="range">the range</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_STD_ULOGIC_VECTOR(Range range)
        {
            return new IndexSubtypeIndication(STD_ULOGIC_VECTOR, range);
        }

        //TODO: add resolve function
        /// STD_LOGIC type. 
        public static readonly Subtype STD_LOGIC = new Subtype("std_logic", STD_ULOGIC);
        /// 'U' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_U = STD_ULOGIC_U;
        /// 'X' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_X = STD_ULOGIC_X;
        /// '0' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_0 = STD_ULOGIC_0;
        /// '1' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_1 = STD_ULOGIC_1;
        /// 'Z' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_Z = STD_ULOGIC_Z;
        /// 'W' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_W = STD_ULOGIC_W;
        /// 'L' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_L = STD_ULOGIC_L;
        /// 'H' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_H = STD_ULOGIC_H;
        /// '-' literal. 
        public static readonly EnumerationLiteral STD_LOGIC_DONT_CARE = STD_ULOGIC_DONT_CARE;
        /// STD_LOGIC_VECTOR type. 
        public static readonly UnconstrainedArray STD_LOGIC_VECTOR = new UnconstrainedArray("std_logic_vector", Standard.NATURAL, STD_LOGIC);

        /// <summary>
        /// Creates an STD_LOGIC_VECTOR(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_STD_LOGIC_VECTOR(int width)
        {
            return Create_STD_LOGIC_VECTOR(new Range(width - 1, Range.RangeDirection.DOWNTO, 0));
        }

        /// <summary>
        /// Creates an STD_LOGIC_VECTOR(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_STD_LOGIC_VECTOR(Expression width)
        {
            Expression from = new Subtract(width, new DecimalLiteral(1));
            Expression to = new DecimalLiteral(0);
            return Create_STD_LOGIC_VECTOR(new Range(from, Range.RangeDirection.DOWNTO, to));
        }

        /// <summary>
        /// Creates an STD_LOGIC_VECTOR(range) subtype indication.
        /// </summary>
        /// <param name="range">the range</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_STD_LOGIC_VECTOR(Range range)
        {
            return new IndexSubtypeIndication(STD_LOGIC_VECTOR, range);
        }

        /// X01 subtype. 
        //TODO: add resolve function
        public static readonly Subtype X01 = new Subtype("X01", new RangeSubtypeIndication(STD_ULOGIC, new Range(STD_ULOGIC_X, Range.RangeDirection.TO, STD_LOGIC_1)));

        /// X01Z subtype. 
        //TODO: add resolve function
        public static readonly Subtype X01Z = new Subtype("X01Z", new RangeSubtypeIndication(STD_ULOGIC, new Range(STD_ULOGIC_X, Range.RangeDirection.TO, STD_LOGIC_Z)));

        /// UX01 subtype. 
        //TODO: add resolve function
        public static readonly Subtype UX01 = new Subtype("UX01", new RangeSubtypeIndication(STD_ULOGIC, new Range(STD_ULOGIC_U, Range.RangeDirection.TO, STD_LOGIC_1)));

        /// UX01Z subtype. 
        //TODO: add resolve function
        public static readonly Subtype UX01Z = new Subtype("UX01Z", new RangeSubtypeIndication(STD_ULOGIC, new Range(STD_ULOGIC_U, Range.RangeDirection.TO, STD_LOGIC_Z)));

        /// TO_BIT function. 
        //TODO: add overloaded versions
        public static readonly FunctionDeclaration TO_BIT = new FunctionDeclaration("TO_BIT", Standard.BIT, new Constant("s", STD_ULOGIC), new Constant("xmap", Standard.BIT, Standard.BIT_0));
        /// TO_BITVECTOR function. 
        public static readonly FunctionDeclaration TO_BITVECTOR = new FunctionDeclaration("TO_BITVECTOR", Standard.BIT_VECTOR, new Constant("s", STD_ULOGIC_VECTOR), new Constant("xmap", Standard.BIT, Standard.BIT_0));
        /// TO_STDULOGIC function. 
        public static readonly FunctionDeclaration TO_STDULOGIC = new FunctionDeclaration("TO_STDULOGIC", STD_ULOGIC, new Constant("b", Standard.BIT));
        /// TO_STDULOGICVECTOR function. 
        public static readonly FunctionDeclaration TO_STDLOGICVECTOR = new FunctionDeclaration("TO_STDLOGICVECTOR", STD_LOGIC_VECTOR, new Constant("s", STD_ULOGIC_VECTOR));
        /// TO_X01 function. 
        public static readonly FunctionDeclaration TO_X01 = new FunctionDeclaration("TO_X01", X01, new Constant("s", STD_ULOGIC));
        /// TO_X01Z function. 
        public static readonly FunctionDeclaration TO_X01Z = new FunctionDeclaration("TO_X01Z", X01Z, new Constant("s", STD_ULOGIC));
        /// TO_UX01 function. 
        public static readonly FunctionDeclaration TO_UX01 = new FunctionDeclaration("TO_UX01", UX01, new Constant("s", STD_ULOGIC));
        /// RISING_EDGE function. 
        public static readonly FunctionDeclaration RISING_EDGE = new FunctionDeclaration("RISING_EDGE", Standard.BOOLEAN, new Signal("s", STD_ULOGIC));
        /// FALLING_EDGE function. 
        public static readonly FunctionDeclaration FALLING_EDGE = new FunctionDeclaration("FALLING_EDGE", Standard.BOOLEAN, new Signal("s", STD_ULOGIC));
        /// IS_X function. 
        public static readonly FunctionDeclaration IS_X = new FunctionDeclaration("IS_X", Standard.BOOLEAN, new Constant("s", STD_ULOGIC));

        /// STD_LOGIC_1164 package. 
        public static readonly PackageDeclaration PACKAGE = new PackageDeclaration("std_logic_1164");

        static StdLogic1164()
        {
            PACKAGE.Declarations.Add(STD_ULOGIC);
            PACKAGE.Declarations.Add(STD_ULOGIC_VECTOR);
            PACKAGE.Declarations.Add(STD_LOGIC);
            PACKAGE.Declarations.Add(STD_LOGIC_VECTOR);
            PACKAGE.Declarations.Add(X01);
            PACKAGE.Declarations.Add(X01Z);
            PACKAGE.Declarations.Add(UX01);
            PACKAGE.Declarations.Add(UX01Z);
            PACKAGE.Declarations.Add(TO_BIT);
            PACKAGE.Declarations.Add(TO_BITVECTOR);
            PACKAGE.Declarations.Add(TO_STDLOGICVECTOR);
            PACKAGE.Declarations.Add(TO_STDULOGIC);
            PACKAGE.Declarations.Add(TO_UX01);
            PACKAGE.Declarations.Add(TO_X01);
            PACKAGE.Declarations.Add(TO_X01Z);
            PACKAGE.Declarations.Add(RISING_EDGE);
            PACKAGE.Declarations.Add(FALLING_EDGE);
            PACKAGE.Declarations.Add(IS_X);
        }

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private StdLogic1164()
        {
        }
    }

}