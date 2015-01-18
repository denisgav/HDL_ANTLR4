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
    /// <summary>
    /// Customizable VHDL code format.
    /// </summary>
    public class CustomCodeFormat : IVhdlCodeFormat
    {

        private string lineSeparator;
        private string indentationString;
        private bool align;
        private bool repeatLabels;
        private bool upperCaseKeywords;

        /// <summary>
        /// Creates a custom code format.
        /// </summary>
        public CustomCodeFormat()
        {
            lineSeparator = DEFAULTVhdlCodeFormat.DEFAULT.LineSeparator;
            indentationString = DEFAULTVhdlCodeFormat.DEFAULT.IndentationString;
            align = DEFAULTVhdlCodeFormat.DEFAULT.Align;
            repeatLabels = DEFAULTVhdlCodeFormat.DEFAULT.RepeatLabels;
            upperCaseKeywords = DEFAULTVhdlCodeFormat.DEFAULT.UpperCaseKeywords;
        }

        /// <summary>
        /// Returns/Sets the line separator.
        /// </summary>
        public virtual string LineSeparator
        {
            get { return lineSeparator; }
            set { lineSeparator = value; }
        }

        /// <summary>
        /// Returns/Sets the Indentation string.
        /// </summary>
        public virtual string IndentationString
        {
            get { return indentationString; }
            set { indentationString = value; }
        }

        /// <summary>
        /// Returns/Sets if the output should be aligned.
        /// </summary>
        public virtual bool Align
        {
            get { return align; }
            set { align = value; }
        }

        /// <summary>
        /// Returns/Sets if labels are repeated at the end of a block.
        /// </summary>
        public virtual bool RepeatLabels
        {
            get { return repeatLabels; }
            set { repeatLabels = value; }
        }

        /// <summary>
        /// Returns/Sets if upper case keywords are used.
        /// </summary>
        public virtual bool UpperCaseKeywords
        {
            get { return upperCaseKeywords; }
            set { upperCaseKeywords = value; }
        }
    }
}