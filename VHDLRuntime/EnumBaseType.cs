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
using System.Collections.ObjectModel;

namespace VHDLRuntime.Range
{
    [Serializable]
    public class EnumBaseContainer
    {
        protected int key;
        protected string value;

        public int Key
        {
            get { return key; }
        }

        public string Value
        {
            get {return value;}
        }

        public EnumBaseContainer(int key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public abstract class EnumBaseType<T> : EnumBaseContainer  where T : EnumBaseType<T>
    {
        protected static List<T> enumValues = new List<T>();

        public EnumBaseType(int key, string value)
            :base(key, value)
        {
            enumValues.Add(this as T);
        }

        public static ReadOnlyCollection<T> GetBaseValues()
        {
            return enumValues.AsReadOnly();
        }

        public static IEnumerable<T> GetBaseValuesInRange(T left, T right, RangeDirection direction)
        {
            return new EnumBaseTypeIterator<T>(left, right, direction);
        }

        public static T GetFirstValue()
        {
            return enumValues[0];
        }

        public static T GetLastValue()
        {
            int idx = enumValues.Count - 1;
            return enumValues[idx];
        }

        public static bool ContainsKey(int key)
        {
            foreach (T t in enumValues)
            {
                if (t.Key == key) return true;
            }
            return false;
        }

        public static T GetBaseByKey(int key)
        {
            foreach (T t in enumValues)
            {
                if (t.Key == key) return t;
            }
            return GetFirstValue();
        }

        public override string ToString()
        {
            return Value;
        }

        public static T Parse(string s)
        {
            foreach (T i in enumValues)
            {
                if(i.Value.Equals(s))
                    return i;
            }

            throw new Exception(string.Format("Could not parse item {0}", s));
        }

        public static int IndexOf(T item)
        {
            for (int i = 0; i < enumValues.Count; i++)
            {
                if (enumValues[i] == item)
                    return i;
            }
            return -1;
        }

        public static T ElementAt(int idx)
        {
            return enumValues[idx];
        }

        public static T PRED(T item)
        {
            int idx = IndexOf(item);
            if (idx == 0)
                return ElementAt(idx);
            else
                return ElementAt(idx - 1);
        }

        public static T SUCC(T item)
        {
            int idx = IndexOf(item);
            if (idx == (enumValues.Count - 1))
                return ElementAt(idx);
            else
                return ElementAt(idx + 1);
        }
    }

    public class EnumBaseTypeIterator<T> : IEnumerable<T> where T : EnumBaseType<T>
    {
        private T left;
        public T Left
        {
            get { return left; }
            set { left = value; }
        }

        private T right;
        public T Right
        {
            get { return right; }
            set { right = value; }
        }

        private RangeDirection direction;
        public RangeDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public EnumBaseTypeIterator(T left, T right, RangeDirection direction)
        {
            this.left = left;
            this.right = right;
            this.direction = direction;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            ReadOnlyCollection<T> enumValues = EnumBaseType<T>.GetBaseValues();
            int leftIndex = enumValues.IndexOf(left);
            int rightIndex = enumValues.IndexOf(right);
            if (direction == RangeDirection.To)
            {
                int iterator = leftIndex;
                do
                {
                    yield return enumValues[iterator];
                    iterator++;
                    if (iterator >= enumValues.Count)
                        iterator = 0;
                }
                while (iterator != rightIndex);
                yield return enumValues[rightIndex];
            }
            else
            {
                int iterator = rightIndex;
                do
                {
                    yield return enumValues[iterator];
                    iterator--;
                    if (iterator < 0)
                        iterator = enumValues.Count - 1;
                }
                while (iterator != leftIndex);
                yield return enumValues[leftIndex];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            ReadOnlyCollection<T> enumValues = EnumBaseType<T>.GetBaseValues();
            int leftIndex = enumValues.IndexOf(left);
            int rightIndex = enumValues.IndexOf(right);
            if (direction == RangeDirection.To)
            {
                int iterator = leftIndex;
                do
                {
                    yield return enumValues[iterator];
                    iterator++;
                    if (iterator >= enumValues.Count)
                        iterator = 0;
                }
                while (iterator != rightIndex);
                yield return enumValues[rightIndex];
            }
            else
            {
                int iterator = rightIndex;
                do
                {
                    yield return enumValues[iterator];
                    iterator--;
                    if (iterator < 0)
                        iterator = enumValues.Count - 1;
                }
                while (iterator != leftIndex);
                yield return enumValues[leftIndex];
            }
        }
    }
}
