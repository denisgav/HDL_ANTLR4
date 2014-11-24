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
using System.Runtime.Serialization;
namespace VHDL.annotation
{
    /// <summary>
    // The interface declaration format annotation is used to customize the VHDL
    // output of interface declarations. The options only affect cases where the
    // mode and/or object class are optional. The mode and/or object class are
    // automatically added to the output if they are semantically necessary.
    /// </summary>
    [Serializable]
    public class InterfaceDeclarationFormat
    {
        private bool useObjectClass;
        private bool useMode;

        /// <summary>
        /// Creates a interface declaration format annotation.
        /// </summary>
        /// <param name="useObjectClass">true, if the object class should be outputted</param>
        /// <param name="useMode">true, if the mode should be included in the output</param>
        public InterfaceDeclarationFormat(bool useObjectClass, bool useMode)
        {
            this.useObjectClass = useObjectClass;
            this.useMode = useMode;
        }

        /// <summary>
        /// </summary>
        public virtual bool UseMode
        {
            /// Returns if the mode is always included in the output.
            /// return true, if the mode is always included in the output
            get { return useMode; }

            /// Sets if the mode should always be included in the output.
            /// useMode true, if the mode should always be included in the output
            set { this.useMode = value; }
        }

        public virtual bool UseObjectClass
        {
            /// Returns if the object class is always included in the output.
            /// return true, if the object class is always included in the output
            get { return useObjectClass; }

            /// Sets if the object class should always be included in the outup.
            /// @param useObjectClass <code>true</code>, if the object class should always be included
            set { this.useObjectClass = value; }
        }
    }
}