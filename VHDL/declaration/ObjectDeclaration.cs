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
using VHDL.annotation;

namespace VHDL.declaration
{
    using VhdlObject = VHDL.Object.VhdlObject;

    /// <summary>
    /// Vhdl object declaration.
    /// </summary>
    /// <typeparam name="T">the object type</typeparam>
    [Serializable]
	public abstract class ObjectDeclaration<T> : DeclarativeItem where T : VhdlObject
	{
		private readonly List<T> objects;

		internal ObjectDeclaration(List<T> objects)
		{
            var annotations = this.AnnotationList;
			this.objects = new List<T>(objects);
            foreach (T o in this.objects)
                o.AnnotationList = annotations;
		}

        /// <summary>
        /// Returns the list of declared objects in this declaration.
        /// </summary>
		public virtual List<T> Objects
		{
            get
            {
                PositionInformation pos = Annotations.getAnnotation<PositionInformation>(this);
                foreach (T o in this.objects)
                {
                    o.AnnotationList = new Dictionary<object, List<object>>();
                    o.AnnotationList.Add(o, new List<object>() { pos });
                    o.Parent = this.Parent;
                }
                return objects;
            }
		}
	}

}