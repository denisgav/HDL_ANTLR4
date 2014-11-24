using System.Collections.Generic;
using Scope = VHDL.IScope;
using BlockDeclarativeItem = VHDL.declaration.IBlockDeclarativeItem;
using ConfigurationDeclarativeItem = VHDL.declaration.IConfigurationDeclarativeItem;
using EntityDeclarativeItem = VHDL.declaration.IEntityDeclarativeItem;
using PackageBodyDeclarativeItem = VHDL.declaration.IPackageBodyDeclarativeItem;
using PackageDeclarativeItem = VHDL.declaration.IPackageDeclarativeItem;
using ProcessDeclarativeItem = VHDL.declaration.IProcessDeclarativeItem;
using SubprogramDeclarativeItem = VHDL.declaration.ISubprogramDeclarativeItem;
using System;

namespace VHDL.libraryunit
{
///
// * Use clause.
// 
//TODO: don't use names for declarations
//TODO: remove LibraryUnit?
    [Serializable]
	public class UseClause : LibraryUnit, BlockDeclarativeItem, ConfigurationDeclarativeItem, EntityDeclarativeItem, PackageBodyDeclarativeItem, PackageDeclarativeItem, ProcessDeclarativeItem, SubprogramDeclarativeItem
	{
		private readonly List<string> declarations;
        private readonly IScope scope;

        /// <summary>
        /// Подключенный элемент
        /// </summary>
        private List<INamedEntity> linkedElements;
        public List<INamedEntity> LinkedElements
        {
            get { return linkedElements; }
            set { linkedElements = value; }
        }
        

//    *
//     * Creates a use clause.
//     * @param declarations the declarations
//     
        public UseClause(params string[] declarations)
            : this(new List<string>(declarations))
		{
		}

//    *
//     * Creates a use clause.
//     * @param declarations the declarations
//     
		public UseClause(List<string> declarations)
		{
			this.declarations = new List<string>(declarations);
            scope = Scopes.createScope(this, new UseClauseResolvable(this));
            linkedElements = new List<INamedEntity>();
		}

//    *
//     * Returns the list of declarations in this use clause
//     * @return the list of declarations
//     
	//TODO: rename?
		public virtual List<string> getDeclarations()
		{
			return declarations;
		}

	//TODO: implement
        public override IScope Scope
        {
            get { return scope; }
        }

		internal override void accept(LibraryUnitVisitor visitor)
		{
			visitor.visitUseClause(this);
		}

        [Serializable]
        private class UseClauseResolvable : IResolvable
        {
            private UseClause parent;

            public UseClauseResolvable(UseClause parent)
            {
                this.parent = parent;
            }

            #region Resolvable Members

            public object Resolve(string identifier)
            {
                foreach (INamedEntity el in parent.LinkedElements)
                {
                    if (el.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        return el;
                    if (el is IDeclarativeRegion)
                    {
                        object o = (el as IDeclarativeRegion).Scope.resolveLocal(identifier);
                        if (o != null)
                            return o;
                    }
                }
                return null;
            }

            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (INamedEntity el in parent.LinkedElements)
                {
                    res.Add(el);
                    if (el is IDeclarativeRegion)
                        res.AddRange((el as IDeclarativeRegion).Scope.GetLocalListOfObjects());
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