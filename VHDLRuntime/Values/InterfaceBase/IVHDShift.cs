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
    public interface IVHDShift<T>
    {
        //sll  shift left logical,     logical array sll integer,  result same
        //srl  shift right logical,    logical array srl integer,  result same
        //sla  shift left arithmetic,  logical array sla integer,  result same
        //sra  shift right arithmetic, logical array sra integer,  result same
        //rol  rotate left,            logical array rol integer,  result same
        //ror  rotate right,           logical array ror integer,  result same

        T ShiftLeftLogical(int value);
        T ShiftLeftLogical(VHDLIntegerValue value);

        T ShiftRightLogical(int value);
        T ShiftRightLogical(VHDLIntegerValue value);

        T ShiftLeftArithmetic(int value);
        T ShiftLeftArithmetic(VHDLIntegerValue value);

        T ShiftRightArithmetic(int value);
        T ShiftRightArithmetic(VHDLIntegerValue value);

        T RotateLeft(int value);
        T RotateLeft(VHDLIntegerValue value);

        T RotateRight(int value);
        T RotateRight(VHDLIntegerValue value);
    }
}
