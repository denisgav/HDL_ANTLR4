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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDLRuntime.Values
{
    public interface IVHDLIntegerValue :
        IVHDLComparable<Int64>, IVHDLComparable<VHDLIntegerValue>,
        IVHDLArithmetic<Int64>, IVHDLArithmetic<VHDLIntegerValue>
    {

        //mod  modulo,          integer mod integer,  result integer
        //rem  remainder,       integer rem integer,  result integer

        //"The result of the rem operator has the sign of its first 
        //operand while the result of the mod operators has the sign of the second operand."

        //5 mod 3 = 2
        //(-5) mod 3 = 1
        //5 mod (-3) = -1
        //(-5) mod (-3) = -2
        //
        //whereas
        //
        //5 rem 3 = 2
        //(-5) rem 3 = -2
        //5 rem (-3) = 2
        //(-5) rem (-3) = -2

        VHDLIntegerValue Mod(Int64 value);
        VHDLIntegerValue Mod(VHDLIntegerValue value);

        VHDLIntegerValue Rem(Int64 value);
        VHDLIntegerValue Rem(VHDLIntegerValue value);

    }
}
