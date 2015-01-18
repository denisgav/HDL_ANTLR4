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
using VHDL;
using Antlr4.Runtime.Misc;
using VHDL.libraryunit;
using System.Collections.Generic;

namespace VHDL_ANTLR4
{
    /// <summary>
    /// Parse functions for vhdlVisitor.
    /// </summary>
    public static class ParseExtention
    {
        public static List<Out> ParseList<In, Out>(IList<In> data_in, Func<In, VhdlElement> visit_function)
            where In : class
            where Out : class
        {
            if (data_in == null)
                throw new ArgumentNullException(string.Format("Argument data_in is null"));

            List<Out> res = new List<Out>();

            foreach (In i in data_in)
            {
                Out new_item = CastExtention.Cast<VhdlElement, Out>(visit_function(i));
                res.Add(new_item);
            }

            return res;
        }

        public static bool TryParseList<In, Out>(IList<In> data_in, Func<In, VhdlElement> visit_function, out List<Out> res)
            where In : class
            where Out : class
        {
            res = new List<Out>();
            if (data_in == null)
                return false;

            foreach (In i in data_in)
            {
                Out new_item;
                try
                {
                    bool successfull = CastExtention.TryCast<VhdlElement, Out>(visit_function(i), out new_item);
                    if (successfull)
                    {
                        res.Add(new_item);
                    }
                    else
                        return false;
                }
                catch { return false; }
            }

            return true;
        }

        public static Out Parse<In, Out>(In data_in, Func<In, VhdlElement> visit_function)
            where In : class
            where Out : class
        {
            if (data_in == null)
                throw new ArgumentNullException(string.Format("Argument data_in is null"));

            VhdlElement parsed_data = visit_function(data_in);
            Out res = CastExtention.Cast<VhdlElement, Out>(parsed_data);
            return res;
        }

        public static bool TryParse<In, Out>(In data_in, Func<In, VhdlElement> visit_function, out Out res)
            where In : class
            where Out : class
        {
            res = null;
            if (data_in == null)
                return false;

            try
            {
                VhdlElement parsed_data = visit_function(data_in);
                bool successfull = CastExtention.TryCast<VhdlElement, Out>(parsed_data, out res);
                return successfull;
            }
            catch
            {
                res = null;
                return false;
            }
        }
    }
}