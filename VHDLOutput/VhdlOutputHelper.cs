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

namespace VHDL.output
{
    using VhdlElement = VHDL.VhdlElement;
    using Comments = VHDL.util.Comments;

    /// <summary>
    /// Vhdl output helper methods.
    /// </summary>
    internal class VhdlOutputHelper
    {

        public static void handleAnnotationsBefore(VhdlElement element, VhdlWriter writer)
        {
            if (element == null)
            {
                return;
            }

            foreach (string comment in Comments.GetComments(element))
            {
                writer.Append("--").Append(comment).NewLine();
            }
        }

        public static void handleAnnotationsAfter(VhdlElement element, VhdlWriter writer)
        {
            if (element == null)
            {
                return;
            }

            foreach (string comment in Comments.GetCommentsAfter(element))
            {
                writer.Append("--").Append(comment).NewLine();
            }
        }

        private VhdlOutputHelper()
        {
        }
    }

}