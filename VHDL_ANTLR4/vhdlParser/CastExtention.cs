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

namespace VHDL_ANTLR4
{
    /// <summary>
    /// Cast functions for vhdlVisitor.
    /// </summary>
    public static class CastExtention
    {
        public static Out Cast<In, Out>(In in_data)
            where In : class
            where Out : class
        {
            Type in_type = typeof(In);
            Type out_type = typeof(Out);
            if (in_data == null)
                throw new ArgumentNullException(string.Format("Null Object access when tried to cast {0} to {1}", in_type.Name, out_type.Name));
            Out res = in_data as Out;

            if (res == null)
                throw new InvalidCastException(string.Format("Failed cast {0} to {1}", in_type.Name, out_type.Name));
            return res;
        }

        public static bool TryCast<In, Out>(In in_data, out Out res)
            where In : class
            where Out : class
        {
            Type in_type = typeof(In);
            Type out_type = typeof(Out);

            res = in_data as Out;

            if (in_data == null)
                return false;

            if (res == null)
                return false;
            return true;
        }
    }
}