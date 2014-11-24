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


namespace VHDL
{
    //
    // 
    /// <summary>
    ///  Utility class to retrieve and manipulate annotations.
    ///  Annotations are object of arbitrary classes that can be added to an instance of a meta class.
    ///  They can be used to add additional informations to a meta class instance. An example of using
    ///  anotations is the vMAGIC VHDL parser that stores error information in the created meta class
    ///  instances. Annotations are also used to store information about comments.
    /// </summary>
    [Serializable]
    public class Annotations
    {

        private Annotations()
        {
        }

        /// <summary>
        /// Returns an annotation instance of the given class.
        /// If no instance of the class is available in the element the function returns null
        /// </summary>
        /// <typeparam name="T">the class of the instance</typeparam>
        /// <param name="element">the element</param>
        /// <returns>the instance, or null</returns>
        public static T getAnnotation<T>(VhdlElement element) where T : class
        {
            if (element.AnnotationList == null)
            {
                return null;
            }
            else
            {
                Dictionary<object, List<object>> annotationList = element.AnnotationList;
                if (annotationList.ContainsKey(element))
                {
                    List<object> objects = annotationList[element];
                    foreach (object o in objects)
                        if (o is T)
                            return o as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Stores an annotation in the given element.
        /// If an annotation of the same class existed before the call to this function it is replaced
        /// by the new instance.
        /// </summary>
        /// <param name="element">the element</param>
        /// <param name="value">the instance</param>
        public static void putAnnotation(VhdlElement element, object @value)
        {
            if (element.AnnotationList == null)
            {
                element.AnnotationList = new Dictionary<object, List<object>>();
            }

            if (element.AnnotationList.ContainsKey(element) == false)
            {
                element.AnnotationList.Add(element, new List<object>() { @value });
            }
            else
                element.AnnotationList[element].Add(@value);
        }
    }
}