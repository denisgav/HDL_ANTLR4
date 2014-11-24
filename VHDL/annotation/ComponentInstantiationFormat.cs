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
    ///  Component instantiation format annotation.
    ///  The component instantiation format annotation is used to modify the VHDL output
    ///  of component instantiations.
    /// </summary>
    [Serializable]
    public class ComponentInstantiationFormat
    {
        private bool useOptionalComponentKeyword;

        /// <summary>
        /// Creates a component instantiation format annotation.
        /// </summary>
        /// <param name="useOptionalComponentKeyword">useOptionalComponentKeyword true,
        /// if the component keyword should be included in the output</param>
        public ComponentInstantiationFormat(bool useOptionalComponentKeyword)
        {
            this.useOptionalComponentKeyword = useOptionalComponentKeyword;
        }

        //    *
        //     * 
        //     
        /// <summary>
        /// Returns if the component keyword is included in the VHDL output.
        /// </summary>
        /// <returns>return true, if the component keyword is used</returns>
        public virtual bool isUseOptionalComponentKeyword()
        {
            return useOptionalComponentKeyword;
        }

        /// <summary>
        /// Sets if the <code>component</code> keyword should be included in the VHDL output.
        /// </summary>
        /// <param name="useOptionalComponentKeyword">true, if the component keyword should be included in the output</param>
        public virtual void setUseOptionalComponentKeyword(bool useOptionalComponentKeyword)
        {
            this.useOptionalComponentKeyword = useOptionalComponentKeyword;
        }
    }
}