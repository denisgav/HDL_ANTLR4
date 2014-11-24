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
namespace VHDL.annotation
{
    /// <summary>
    /// Position in the source code.
    /// </summary>
    [Serializable]
    public class SourcePosition
    {
        private readonly int line;
        private readonly int column;
        private readonly int index;

        /// <summary>
        /// Creates a SourcePosition.
        /// </summary>
        /// <param name="line">the line in the source code</param>
        /// <param name="column">the colum in the line</param>
        /// <param name="index"></param>
        public SourcePosition(int line, int column, int index)
        {
            this.line = line;
            this.column = column;
            this.index = index;
        }

        /// <summary>
        /// Returns the column inside the line.
        /// </summary>
        public virtual int Column
        {
            get { return column; }
        }

        /// <summary>
        /// Returns the line.
        /// </summary>
        public virtual int Line
        {
            get { return line; }
        }

        /// <summary>
        /// Returns the character index in the file.
        /// </summary>
        public virtual int Index
        {
            get { return index; }
        }

        public override string ToString()
        {
            return "row: " + line + ", col: " + column;
        }
    }

}