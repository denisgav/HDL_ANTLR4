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
    using UseClause = VHDL.libraryunit.UseClause;
    using Expression = VHDL.expression.Expression;
    using Subtract = VHDL.expression.Subtract;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using DecimalLiteral = VHDL.literal.DecBasedInteger;
    using Constant = VHDL.Object.Constant;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using UnconstrainedArray = VHDL.type.UnconstrainedArray;

    /// <summary>
    /// NUMERIC_STD wrapper class.
    /// </summary>
    public class NumericStd
    {

        /// Use clause for all declarations in this package. 
        public static readonly UseClause USE_CLAUSE = new UseClause("ieee.numeric_std.all");
        /// UNSIGNED type. 
        public static readonly UnconstrainedArray UNSIGNED = new UnconstrainedArray("unsigned", Standard.NATURAL, StdLogic1164.STD_LOGIC);

        /// <summary>
        /// Creates an UNSIGNED(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_UNSIGNED(int width)
        {
            return Create_UNSIGNED(new Range(width - 1, Range.RangeDirection.DOWNTO, 0));
        }

        /// <summary>
        /// Creates an UNSIGNED(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_UNSIGNED(Expression width)
        {
            Expression from = new Subtract(width, new DecimalLiteral(1));
            Expression to = new DecimalLiteral(0);
            return Create_UNSIGNED(new Range(from, Range.RangeDirection.DOWNTO, to));
        }

        /// <summary>
        /// Creates an UNSIGNED(range) subtype indication.
        /// </summary>
        /// <param name="range">the range</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_UNSIGNED(Range range)
        {
            return new IndexSubtypeIndication(UNSIGNED, range);
        }
        /// SIGNED type. 
        public static readonly UnconstrainedArray SIGNED = new UnconstrainedArray("signed", Standard.NATURAL, StdLogic1164.STD_LOGIC);

        /// <summary>
        /// Creates a SIGNED(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_SIGNED(int width)
        {
            return Create_SIGNED(new Range(width - 1, Range.RangeDirection.DOWNTO, 0));
        }

        /// <summary>
        /// Creates a SIGNED(width - 1 DOWNTO 0) subtype indication.
        /// </summary>
        /// <param name="width">the width</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_SIGNED(Expression width)
        {
            Expression from = new Subtract(width, new DecimalLiteral(1));
            Expression to = new DecimalLiteral(0);
            return Create_SIGNED(new Range(from, Range.RangeDirection.DOWNTO, to));
        }

        /// <summary>
        /// Creates a SIGNED(range) subtype indication.
        /// </summary>
        /// <param name="range">the range</param>
        /// <returns>the subtype indication</returns>
        public static SubtypeIndication Create_SIGNED(Range range)
        {
            return new IndexSubtypeIndication(SIGNED, range);
        }
        //    *
        //     * SHIFT_LEFT function.
        //     
        //TODO: add overloaded versions
        public static readonly FunctionDeclaration SHIFT_LEFT = new FunctionDeclaration("SHIFT_LEFT", UNSIGNED, new Constant("ARG", UNSIGNED), new Constant("COUNT", Standard.NATURAL));
        //    *
        //     * SHIFT_RIGHT function.
        //     
        //TODO: add overloaded versions
        public static readonly FunctionDeclaration SHIFT_RIGHT = new FunctionDeclaration("SHIFT_RIGHT", UNSIGNED, new Constant("ARG", UNSIGNED), new Constant("COUNT", Standard.NATURAL));
        //    *
        //     * ROTATE_LEFT function.
        //     
        //TODO: add overloaded versions
        public static readonly FunctionDeclaration ROTATE_LEFT = new FunctionDeclaration("ROTATE_LEFT", UNSIGNED, new Constant("ARG", UNSIGNED), new Constant("COUNT", Standard.NATURAL));
        //    *
        //     * ROTATE_RIGHT function.
        //     
        //TODO: add overloaded versions
        public static readonly FunctionDeclaration ROTATE_RIGHT = new FunctionDeclaration("ROTATE_RIGHT", UNSIGNED, new Constant("ARG", UNSIGNED), new Constant("COUNT", Standard.NATURAL));
        //    *
        //     * RESIZE function.
        //     
        //TODO: add overloaded version
        public static readonly FunctionDeclaration RESIZE = new FunctionDeclaration("RESIZE", UNSIGNED, new Constant("ARG", UNSIGNED), new Constant("NEW_SIZE", Standard.NATURAL));
        //    *
        //     * TO_INTEGER function.
        //     
        //TODO: add overloaded version
        public static readonly FunctionDeclaration TO_INTEGER = new FunctionDeclaration("TO_INTEGER", Standard.NATURAL, new Constant("ARG", UNSIGNED));
        //    *
        //     * TO_UNSIGNED function.
        //     
        //TODO: add overloaded version
        public static readonly FunctionDeclaration TO_UNSIGNED = new FunctionDeclaration("TO_UNSIGNED", UNSIGNED, new Constant("ARG", Standard.NATURAL), new Constant("SIZE", Standard.NATURAL));
        //    *
        //     * TO_SIGNED function.
        //     
        //TODO: add overloaded version
        public static readonly FunctionDeclaration TO_SIGNED = new FunctionDeclaration("TO_SIGNED", SIGNED, new Constant("ARG", Standard.INTEGER), new Constant("SIZE", Standard.NATURAL));
        /// NUMERIC_STD package. 
        public static readonly PackageDeclaration PACKAGE = new PackageDeclaration("numeric_std");

        static NumericStd()
        {
            PACKAGE.Declarations.Add(UNSIGNED);
            PACKAGE.Declarations.Add(SIGNED);
            PACKAGE.Declarations.Add(SHIFT_LEFT);
            PACKAGE.Declarations.Add(SHIFT_RIGHT);
            PACKAGE.Declarations.Add(ROTATE_LEFT);
            PACKAGE.Declarations.Add(ROTATE_RIGHT);
            PACKAGE.Declarations.Add(RESIZE);
            PACKAGE.Declarations.Add(TO_INTEGER);
            PACKAGE.Declarations.Add(TO_SIGNED);
            PACKAGE.Declarations.Add(TO_UNSIGNED);
        }

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private NumericStd()
        {
        }
    }

}