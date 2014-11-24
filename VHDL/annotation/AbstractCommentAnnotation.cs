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

namespace VHDL.annotation
{
    /// <summary>
    ///  Abstract comment annotation.
    ///  The comment annotations are used to store VHDL comments in before and after
    ///  a VhdlElement. Each String in the list of comments represents a single line
    ///  comment. The strings must not contain line breaks. The
    ///  utility class provides an easier
    ///  interface to set and get comments.
    /// 
    ///  @see VHDL.util.Comments
    /// </summary>
    [Serializable]
    public abstract class AbstractCommentAnnotation
    {
        private readonly List<string> comments;

        /// <summary>
        /// Creates a new comment annotation. 
        /// </summary>
        /// <param name="comments">comments zero or more line comments</param>
        public AbstractCommentAnnotation(params string[] comments)
        {
            this.comments = new List<string>(comments);
        }

        /// <summary>
        /// Creates a new comment annotation.
        /// </summary>
        /// <param name="comments">comments a list of line comments</param>
        public AbstractCommentAnnotation(IList<string> comments)
        {
            this.comments = new List<string>(comments);
        }

        /// <summary>
        /// Returns a modifiable list of line comments.
        /// </summary>
        /// <returns>list of line comments</returns>
        public virtual List<string> getComments()
        {
            return comments;
        }
    }
}