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
using Annotations = VHDL.Annotations;
using VhdlElement = VHDL.VhdlElement;
using CommentAnnotationAfter = VHDL.annotation.CommentAnnotationAfter;
using System;
using VHDL.annotation;

namespace VHDL.util
{
    /// <summary>
    /// Comment utility functions.
    /// </summary>
    [Serializable]
    public class Comments
    {
        private Comments()
        {
        }

        /// <summary>
        /// Returns the comments before the given vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <returns>a list of line comments</returns>
        public static List<string> GetComments(VhdlElement element)
        {
            CommentAnnotationBefore annotation = Annotations.getAnnotation<CommentAnnotationBefore>(element);
            if (annotation == null)
            {
                return new List<string>();
            }
            else
            {
                return annotation.getComments();
            }
        }

        /// <summary>
        /// Returns the comments after the given vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <returns>a list of line comments</returns>
        public static List<string> GetCommentsAfter(VhdlElement element)
        {
            CommentAnnotationAfter annotation = Annotations.getAnnotation<CommentAnnotationAfter>(element);
            if (annotation == null)
            {
                return new List<string>();
            }
            else
            {
                return annotation.getComments();
            }
        }

        /// <summary>
        /// Sets the comments before a vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <param name="comments">a list of line comments</param>
        public static void SetComments(VhdlElement element, IList<string> comments)
        {
            CommentAnnotationBefore annotation = new CommentAnnotationBefore(comments);
            Annotations.putAnnotation(element, annotation);
        }

        /// <summary>
        /// Sets the comments before a vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <param name="comments">zero or more line comments</param>
        public static void SetComments(VhdlElement element, params string[] comments)
        {
            SetComments(element, new List<string>(comments));
        }

        /// <summary>
        /// Sets the comments after a vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <param name="comments">a list of line comments</param>
        public static void SetCommentsAfter(VhdlElement element, List<string> comments)
        {
            CommentAnnotationAfter annotation = new CommentAnnotationAfter(comments);
            Annotations.putAnnotation(element, annotation);
        }

        /// <summary>
        /// Sets the comments after a vhdl element.
        /// </summary>
        /// <param name="element">the vhdl element</param>
        /// <param name="comments">zero or more line comments</param>
        public static void SetCommentsAfter(VhdlElement element, params string[] comments)
        {
            SetComments(element, new List<string>(comments));
        }
    }

}