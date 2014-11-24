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
    /// This class is used to change the output of VHDL constructs with an optional
    /// </summary>
    [Serializable]
    public class OptionalIsFormat
    {
        private bool useIs;

        /// <summary>
        /// Creates a optional <code>is</code> format annotation.
        /// </summary>
        /// <param name="useIs">useIs true, if the optional is should be added to the output</param>
        public OptionalIsFormat(bool useIs)
        {
            this.useIs = useIs;
        }

        public virtual bool UseIs
        {
            /// Returns if the optional is keyword is added to the output.
            /// return true, if the optional is is added
            get { return useIs; }
            /// Sets if the optional is keyword should be added to the output
            /// useIs true, if the optional is should be added
            set { useIs = value; }
        }
    }
}