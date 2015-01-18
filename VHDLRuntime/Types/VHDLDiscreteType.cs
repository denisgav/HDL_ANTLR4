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
using VHDLRuntime.Range;

namespace VHDLRuntime.Types
{
    [Serializable]
    public abstract class VHDLDiscreteType<T> : VHDLScalarType<T> where T:VHDLDiscreteValue
    {
        /// <summary>
        /// T'POS(X)     is the integer position of X in the discrete type T.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual VHDLIntegerValue POS(T X)
        {
            return DiscreteRange.POS(X);
        }

        /// <summary>
        /// T'SUCC(X)    is the value of discrete type T that is the successor of X.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual T SUCC(T X)
        {
            return DiscreteRange.SUCC(X);
        }

        /// <summary>
        /// T'PRED(X)    is the value of discrete type T that is the predecessor of X.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual T PRED(T X)
        {
            return DiscreteRange.PRED(X);
        }

        /// <summary>
        /// T'LEFTOF(X)  is the value of discrete type T that is left of X.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual T LEFTOF(T X)
        {
            return DiscreteRange.LEFTOF(X);
        }

        /// <summary>
        /// T'RIGHTOF(X) is the value of discrete type T that is right of X.
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public virtual T RIGHTOF(T X)
        {
            return DiscreteRange.RIGHTOF(X);
        }

        public override T CorrectValue(T value)
        {
            return DiscreteRange.CorrectValue(value);
        }

        public abstract DiscreteRange<T> DiscreteRange { get; }
    }
}
