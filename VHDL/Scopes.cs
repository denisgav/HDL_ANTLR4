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
namespace VHDL
{
    /// <summary>
    /// Default implementations of the Scope interface.
    /// This class is used to implement vMAGIC scope feature and isn't usually used by
    /// the user of the library
    /// </summary>
    [Serializable]
    public class Scopes
    {
        /// <summary>
        /// Creates a scope.
        /// </summary>
        /// <param name="parent">the associated VhdlElement</param>
        /// <param name="list">a list of resolvables</param>
        /// <returns>the created scope</returns>
        public static IScope createScope(VhdlElement parent, params IResolvable[] list)
        {
            return new ResolvableListsScope(parent, list);
        }

        private Scopes()
        {
        }

        [Serializable]
        private abstract class AbstractScope : IScope
        {
            private readonly VhdlElement parent;

            public AbstractScope(VhdlElement parent)
            {
                this.parent = parent;
            }

            public object resolve(string identifier)
            {
                object o = resolveLocal(identifier);
                IScope parentScope = getParentScope();

                if (o == null && parentScope != null)
                {
                    if ((parentScope != parent) && (parent.Parent != parent))
                        return parentScope.resolve(identifier);
                }

                return o;
            }

            public T resolve<T>(string identifier) where T : class
            {
                object o = resolve(identifier);

                if (o is T)
                {
                    return o as T;
                }

                return null;
            }

            private IScope getParentScope()
            {
                if (parent != null && parent.Parent != null)
                {
                    return parent.Parent.Scope;
                }
                else
                {
                    return null;
                }
            }

            #region Scope Members


            public abstract object resolveLocal(string identifier);

            public T resolveLocal<T>(string identifier) where T : class
            {
                object o = resolveLocal(identifier);

                if (o is T)
                {
                    T tmp = (T)o;
                    return tmp;
                }

                return null;
            }

            public abstract List<object> resolveAllLocal(string identifier);
            public List<object> resolveAll(string identifier)
            {
                List<object> res = new List<object>();
                res.AddRange(resolveAllLocal(identifier));
                IScope parent = getParentScope();
                if (parent != null)
                    res.AddRange(parent.resolveAll(identifier));
                return res;
            }

            #endregion


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                res.AddRange(GetLocalListOfObjects());
                IScope parent = getParentScope();
                if (parent != null)
                    res.AddRange(parent.GetListOfObjects());
                return res;
            }

            public List<T> GetListOfObjects<T>() where T : class
            {
                List<T> res = new List<T>();
                res.AddRange(GetLocalListOfObjects<T>());
                IScope parent = getParentScope();
                if (parent != null)
                    res.AddRange(parent.GetListOfObjects<T>());
                return res;
            }

            public abstract List<object> GetLocalListOfObjects();
            public abstract List<T> GetLocalListOfObjects<T>() where T : class;
        }

        [Serializable]
        private class ResolvableListsScope : AbstractScope
        {
            private readonly IResolvable[] lists;

            public ResolvableListsScope(VhdlElement parent, params IResolvable[] lists)
                : base(parent)
            {
                this.lists = new IResolvable[lists.Length];
                lists.CopyTo(this.lists, 0);
            }

            public override object resolveLocal(string identifier)
            {
                foreach (IResolvable list in lists)
                {
                    object o = list.Resolve(identifier);
                    if (o != null)
                    {
                        return o;
                    }
                }
                return null;
            }



            public override List<object> GetLocalListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (IResolvable list in lists)
                    res.AddRange(list.GetListOfObjects());
                return res;
            }

            public override List<T> GetLocalListOfObjects<T>()
            {
                List<T> res = new List<T>();
                foreach (IResolvable list in lists)
                {
                    List<object> objects = list.GetListOfObjects();
                    foreach (object o in objects)
                        if (o is T)
                            res.Add(o as T);
                }
                return res;
            }

            public override List<object> resolveAllLocal(string name)
            {
                List<object> result = new List<object>();
                foreach (IResolvable list in lists)
                {
                    var objects = list.GetListOfObjects();
                    foreach (var obj in objects)
                    {
                        var named = obj as INamedEntity;
                        if (named != null)
                            if (named.Identifier == name)
                                result.Add(named);
                    }
                }
                return result;
            }
        }
    }
}