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
    /// The VhdlCodeFormatOld defines the output format of the generated source code.
    /// </summary>
	public interface IVhdlCodeFormat
	{
        /// <summary>
        /// Returns the line separator.
        /// </summary>
        string LineSeparator { get; }

        /// <summary>
        /// Returns the Indentation string.
        /// </summary>
        string IndentationString { get; }

        /// <summary>
        /// Returns true if output elements should be aligned
        /// (e.g. signals in a port).
        /// </summary>
        bool Align { get; }

        /// <summary>
        /// Returns true if labeles should be repeated at the end of a construct
        /// (e.g. an entity).
        /// </summary>
        bool RepeatLabels { get; }

        /// <summary>
        /// Returns true if upper case keywords should be used.
        /// </summary>
        bool UpperCaseKeywords { get; }
    }
    
    /// <summary>
    /// The default code format.
    /// </summary>
    public sealed class DEFAULTVhdlCodeFormat : IVhdlCodeFormat
    {
        private static DEFAULTVhdlCodeFormat _default;
        public static DEFAULTVhdlCodeFormat DEFAULT
        {
            get { return _default; }
        }

        static DEFAULTVhdlCodeFormat()
        {
            _default = new DEFAULTVhdlCodeFormat();
        }

        public string LineSeparator
        {
            get { return "\n\r"; }
        }

        public string IndentationString
        {
            get { return "    "; }
        }

        public bool Align
        {
            get { return false; }
        }

        public bool RepeatLabels
        {
            get { return false; }
        }

        public bool UpperCaseKeywords
        {
            get { return false; }
        }
    }
}