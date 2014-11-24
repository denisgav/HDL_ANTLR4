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

using System.Collections.Generic;
using System;
using VHDL.Object;

namespace VHDL
{
    /// <summary>
    /// An element inside a VHDL file.
    /// </summary>
    [Serializable]
    public abstract class VhdlElement
    {
        private Dictionary<object, List<object>> annotationList;
        private IDeclarativeRegion parent;

        /// <summary>
        /// Returns/Sets the annotation list for this VhdlElement.
        /// Use the methods provided by the <code>Annotations</code> class instead.
        /// </summary>
        public virtual Dictionary<object, List<object>> AnnotationList
        {
            get { return annotationList; }
            internal set { this.annotationList = value; }
        }                

        /// <summary>
        /// Returns/Sets the parent of this VhdlChild.
        /// </summary>
        public virtual IDeclarativeRegion Parent
        {
            get { return parent; }
            set { parent = value; }
        }
    }

}