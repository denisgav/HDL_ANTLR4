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

namespace VHDLRuntime.Range
{
    public enum RangeDirection
    {
        To,
        DownTo
    }

    [Serializable]
    public abstract class ScalarRange<T> : RangeBase<T> where T : VHDLScalarValue
    {
        /// <summary>
        /// Left Range
        /// </summary>
        protected T left = default(T);
        public T Left
        {
            get { return left; }
        }

        /// <summary>
        /// Right Range
        /// </summary>
        protected T right = default(T);
        public T Right
        {
            get { return right; }
        }

        /// <summary>
        /// Direction To/DownTo
        /// </summary>
        protected RangeDirection direction;
        public RangeDirection Direction
        {
            get { return direction; }
        }

        public ScalarRange(T left, T right, RangeDirection direction)
        {
            this.left = left;
            this.right = right;
            this.direction = direction;
        }

        public ScalarRange()
        {
        }

        
        public T High
        {
            get 
            {
                return (left.CompareTo(right) > 0)? left : right;
            }
        }

        public T Low
        {
            get
            {
                return (left.CompareTo(right) < 0) ? left : right;
            }
        }


        public override VHDLScalarValue LEFT()
        {
            return left;
        }

        public override VHDLScalarValue RIGHT()
        {
            return right;
        }

        public override VHDLScalarValue HIGH()
        {
            return High;
        }

        public override VHDLScalarValue LOW()
        {
            return Low;
        }

        public override T CorrectValue(T value)
        {
            if (value is T)
            {
                if (value > High)
                    return High;
                if (value < Low)
                    return Low;
            }
            return value;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", left.ToString(), direction.ToString(), right.ToString());
        }
    }
}
