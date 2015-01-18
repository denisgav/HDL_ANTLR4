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

namespace VHDLRuntime.Types
{
    [Serializable]
    public abstract class VHDLBaseType
    {
        //Listof the predefined attributes of the value

        /// <summary>
        /// T'IMAGE(X)   is a string representation of X that is of type T.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual string IMAGE(VHDLBaseValue X)
        {
            return X.ToString();
        }
    }

    [Serializable]
    public abstract class VHDLBaseType<T> : VHDLBaseType where T : VHDLBaseValue
    {
        //Listof the predefined attributes of the value
        
        /// <summary>
        /// T'ASCENDING  is boolean true if range of T defined with to .
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract bool ASCENDING(VHDLBaseType<T> X);

        /// <summary>
        /// T'IMAGE(X)   is a string representation of X that is of type T.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual string IMAGE(T X)
        {
            return X.ToString();
        }

        /// <summary>
        /// T'VALUE(X)   is a value of type T converted from the string X.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public abstract T VALUE(string X);

        public virtual T CorrectValue(T value)
        {
            return value;
        }
    }
}
