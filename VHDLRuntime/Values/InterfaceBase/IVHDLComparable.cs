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
    public interface IVHDLComparable<T>
    {

        //=    test for equality, result is boolean
        ///=   test for inequality, result is boolean
        //<    test for less than, result is boolean
        //<=   test for less than or equal, result is boolean
        //>    test for greater than, result is boolean
        //>=   test for greater than or equal, result is boolean

        bool LessThan(T value);

        bool LessEquals(T value);
        
        bool GreaterThan(T value);

        bool GreaterEquals(T value);

        bool NotEquals(T value);

        bool Equals(T value);
    }
}
