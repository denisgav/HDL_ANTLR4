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

using System.IO;
using Antlr4.Runtime;

namespace VHDL.parser.util
{
    /// <summary>
    /// A case insensitive version of ANTLRInputStream.
    /// CaseInsensitiveInputStream modifies the lookahead to be lower case only.
    /// The case inside the token text is preserved for further usage.
    /// </summary>
    public class CaseInsensitiveInputStream : AntlrInputStream
    {
        /// <summary>
        /// Create new stream by calling the super class constructor.
        /// </summary>
        /// <param name="input">the input stream</param>
        public CaseInsensitiveInputStream(Stream input)
            : base(input)
        {
        }

        /// <summary>
        /// Returns the lookahead as a lower case char.
        /// </summary>
        /// <param name="i">i the amount of lookahead</param>
        /// <returns>the character at the position i</returns>
        public override int La(int i)
        {
            if (i == 0)
            {
                return 0;
            }

            if (i < 0)
            {
                i++;
                if ((p + i - 1) < 0)
                {
                    return -1;
                }
            }

            if ((p + i - 1) >= n)
            {
                return -1;
            }

            return char.ToLower(data[p + i - 1]);
        }
    }

}