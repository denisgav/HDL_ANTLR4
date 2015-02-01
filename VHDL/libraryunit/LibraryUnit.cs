using DeclarativeRegion = VHDL.IDeclarativeRegion;
using VhdlElement = VHDL.VhdlElement;
using System;
using System.Collections.Generic;
using VHDL.util;

namespace VHDL.libraryunit
{
///
// * Library unit.
// 
    [Serializable]
	public abstract class LibraryUnit : VhdlElement, IDeclarativeRegion
	{
        /// <summary>
        /// Подключаемые библиотеки и пакеты
        /// </summary>
        [NonSerialized]
        private List<LibraryUnit> contextItems;
        public List<LibraryUnit> ContextItems
        {
            get { return contextItems; }
            set { contextItems = value; }
        }

		internal abstract void accept(LibraryUnitVisitor visitor);

        public LibraryUnit()
        {
            contextItems = new List<LibraryUnit>();
        }

        #region DeclarativeRegion Members

        public virtual IScope Scope
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        [Serializable]
        protected class LibraryUnitResolvable : IResolvable
        {
            private LibraryUnit parent;

            public LibraryUnitResolvable(LibraryUnit parent)
            {
                this.parent = parent;
            }

            #region Resolvable Members

            public object Resolve(string identifier)
            {
                if (parent.contextItems != null)
                {
                    foreach (LibraryUnit unit in parent.contextItems)
                        if (unit is UseClause)
                        {
                            UseClause use = unit as UseClause;
                            object o = use.Scope.resolveLocal(identifier);
                            if (o != null)
                                return o;
                        }
                        else
                            if (unit is INamedEntity)
                                if ((unit as INamedEntity).Identifier.EqualsIdentifier(identifier))
                                    return unit;
                }
                return null;
            }

            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                if (parent.contextItems != null)
                {
                    foreach (LibraryUnit unit in parent.contextItems)
                        if (unit is UseClause)
                        {
                            UseClause use = unit as UseClause;
                            res.AddRange(use.Scope.GetLocalListOfObjects());
                        }
                        else
                            res.Add(unit);
                }
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return new List<object>();
            }

            #endregion
        }

    }
}