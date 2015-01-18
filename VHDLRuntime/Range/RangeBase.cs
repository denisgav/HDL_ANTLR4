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
using VHDLRuntime.Values;
using VHDLRuntime.Types;

namespace VHDLRuntime.Range
{
    [Serializable]
    public abstract class RangeBase
    {

        /// <summary>
        /// T'LEFT       is the leftmost value of type T. (Largest if downto)
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract VHDLScalarValue LEFT();

        /// <summary>
        /// T'RIGHT      is the rightmost value of type T. (Smallest if downto)
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract VHDLScalarValue RIGHT();

        /// <summary>
        /// T'HIGH       is the highest value of type T.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract VHDLScalarValue HIGH();

        /// <summary>
        /// T'LOW        is the lowest value of type T.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract VHDLScalarValue LOW();
    }

    public abstract class RangeBase<T> : RangeBase where T : VHDLScalarValue
    {
        public abstract T CorrectValue(T value);
    }
}
