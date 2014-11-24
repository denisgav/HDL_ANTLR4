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
using DeclarativeRegion = VHDL.IDeclarativeRegion;
using Scope = VHDL.IScope;
using Scopes = VHDL.Scopes;
using VhdlElement = VHDL.VhdlElement;
using System;

namespace VHDL.util
{
    /// <summary>
    /// Implementation of the List interfaces that sets the parent of
    /// </summary>
    /// <typeparam name="E">the type of the list elements</typeparam>
    [Serializable]
    public class ParentSetList<E> : List<E> where E : VhdlElement
    {
        protected readonly IDeclarativeRegion parent;

        /// <summary>
        /// Creates a new ParentSetList which uses an ArrayList for storage.
        /// </summary>
        /// <param name="parent">the parent declarative region</param>
        /// <returns>the parent set list</returns>
        public static ParentSetList<E> Create(IDeclarativeRegion parent)
        {
            return new ParentSetList<E>(parent);
        }

        /// <summary>
        /// Creates a new ParentSetList.
        /// </summary>
        /// <param name="parent">the parent declarative region</param>
        /// <param name="list">the list used for element storage</param>
        /// <returns>the parent set list</returns>
        public static ParentSetList<E> Create(IDeclarativeRegion parent, IList<E> list)
        {
            return new ParentSetList<E>(parent, list);
        }

        /// <summary>
        /// Creates a new proxy ParentSetList which uses an ArrayList for storage.
        /// A proxy ParentSetList is used for parents which aren't declarative regions
        /// (e.g. IfStatements).
        /// </summary>
        /// <param name="parent">the parent</param>
        /// <returns>the proxy list</returns>
        public static ParentSetList<E> CreateProxyList(VhdlElement parent)
        {
            return new ParentSetList<E>(new ParentProxy(parent));
        }

        /// <summary>
        /// Creates a new proxy ParentSetList and initializes it from a given list.
        /// A proxy ParentSetList is used for parents which aren't declarative regions
        /// (e.g. IfStatements).
        /// </summary>
        /// <param name="list">the list used for element storage</param>
        /// <param name="parent">the parent</param>
        /// <returns>the proxy list</returns>
        public static ParentSetList<E> CreateProxyList(List<E> list, VhdlElement parent)
        {
            return new ParentSetList<E>(new ParentProxy(parent), list);
        }

        private ParentSetList(IDeclarativeRegion parent, IList<E> list)
            : base(list)
        {
            this.parent = parent;
        }

        private ParentSetList(IDeclarativeRegion parent)
            : base(new List<E>())
        {
            this.parent = parent;
        }

        public ParentSetList()
        {
        }

        public new bool Add(E e)
        {
            if (e != null)
            {
                e.Parent = parent;
            }
            base.Add(e);
            return true;
        }

        public new void Insert(int index, E element)
        {
            if (element != null)
            {
                element.Parent = parent;
            }

            base.Insert(index, element);
        }

        public bool AddRange(ICollection<E> c)
        {
            foreach (E e in c)
            {
                if (e != null)
                {
                    e.Parent = parent;
                }
            }
            base.AddRange(c);
            return true;
        }

        public bool Remove(object o)
        {
            bool removed = base.Remove(o as E);
            if (removed)
            {
                if (o != null && o is VhdlElement)
                {
                    VhdlElement c = (VhdlElement)o;
                    c.Parent = null;
                }
            }
            return removed;
        }

        public E Remove(int index)
        {
            E element = base[index];
            base.RemoveAt(index);
            if (element != null)
            {
                element.Parent = null;
            }
            return element;
        }

        public bool RemoveAll(IEnumerable<E> c)
        {
            bool removed = false;
            foreach (E o in c)
            {
                if (Remove(o))
                {
                    removed = true;
                }
            }
            return removed;
        }

        //TODO: implement
        public bool RetainAll<T1>(ICollection<T1> c)
        {
            throw new Exception();
        }

        public void Clear()
        {
            foreach (VhdlElement e in this)
            {
                if (e != null)
                {
                    e.Parent = null;
                }
            }
            base.Clear();
        }

        //TODO: implement
        public void Set(int index, E element)
        {
            base[index] = element;
        }

        [Serializable]
        private sealed class ParentProxy : IDeclarativeRegion
        {

            private readonly IScope scope;

            public ParentProxy(VhdlElement parent)
            {
                scope = Scopes.createScope(parent);
            }

            #region DeclarativeRegion Members

            IScope IDeclarativeRegion.Scope
            {
                get { return scope; }
            }

            #endregion
        }
    }
}