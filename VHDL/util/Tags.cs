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
using VhdlElement = VHDL.VhdlElement;
using System;

namespace VHDL.util
{
    /// <summary>
    /// Utility class for handling tags.
    /// </summary>
    [Serializable]
    public class Tags
    {
        /// <summary>
        /// Returns the list of tags in front of the element.
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns>unmodifiable list of tags</returns>
        public static List<string> GetTags(VhdlElement element)
        {
            List<string> tags = new List<string>();
            foreach (string comment in Comments.GetComments(element))
            {
                if (comment.StartsWith("*"))
                {
                    tags.Add(comment.Substring(1));
                }
            }

            return tags;
        }

        /// <summary>
        /// Returns the value of a tag with the format "--name:value".
        /// If there are multiple tags with the same name, the first one is returned.
        /// </summary>
        /// <param name="element">the element</param>
        /// <param name="name">the name of the tag</param>
        /// <returns>the value of the tag or null if no tag with the given name was found</returns>
        public static string GetTag(VhdlElement element, string name)
        {
            foreach (string comment in Comments.GetComments(element))
            {
                if (comment.StartsWith("*"))
                {
                    string[] parts = comment.Substring(1).Split(new char[] { ':' }, 2);

                    if (parts.Length == 2 && parts[0].Trim().Equals(name))
                    {
                        return parts[1].Trim();
                    }
                }
            }

            return null;
        }

        //prevent instantiation
        private Tags()
        {
        }
    }

}