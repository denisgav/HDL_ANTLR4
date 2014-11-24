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

using System.Collections.Generic;
using System;

namespace VHDL.highlevel
{
    [Serializable]
    internal class ProxyList<E> : List<E>
    {

        private readonly List<E> list1;
        private readonly List<E> list2;
        private readonly List<E> list3;

        public ProxyList(List<E> list1, List<E> list2)
        {
            this.list1 = list1;
            this.list2 = list2;
            this.list3 = null;
        }

        public ProxyList(List<E> list1, List<E> list2, List<E> list3)
        {
            this.list1 = list1;
            this.list2 = list2;
            this.list3 = list3;
        }

        public new E this[int index]
        {
            get
            {
                if (index < list1.Count)
                {
                    return list1[index];
                }
                index -= list1.Count;

                if (index < list2.Count)
                {
                    return list2[index];
                }
                index -= list2.Count;

                if (list3 != null && index < list3.Count)
                {
                    return list3[index];
                }

                throw new Exception();
            }
        }

        public int size()
        {
            int size = list1.Count + list2.Count;
            if (list3 == null)
            {
                return size;
            }
            else
            {
                return size + list3.Count;
            }
        }
    }

}