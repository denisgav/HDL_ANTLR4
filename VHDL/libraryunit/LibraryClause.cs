using System.Collections.Generic;
using Scope = VHDL.IScope;
using System;
using VHDL.declaration;

namespace VHDL.libraryunit
{
///
// * Library clause.
// 
//TODO: remove LibraryUnit
    [Serializable]
    public class LibraryClause : LibraryUnit
	{
		private readonly List<string> libraries = new List<string>();
        private readonly IScope scope;

        /// <summary>
        /// Область декларации библиотек
        /// </summary>
        private List<LibraryDeclarativeRegion> libraryDeclarativeRegion;
        public List<LibraryDeclarativeRegion> LibraryDeclarativeRegion
        {
            get { return libraryDeclarativeRegion; }
            set { libraryDeclarativeRegion = value; }
        }
        

//    *
//     * Crates a library clause.
//     * @param libraries the libraries
//     
		public LibraryClause(params string[] libraries)
		{
			this.libraries.AddRange(new List<string>(libraries));
            libraryDeclarativeRegion = new List<VHDL.LibraryDeclarativeRegion>();
            scope = Scopes.createScope(this, new LibraryClauseResolvable(this));
		}
               
//    *
//     * Crates a library clause.
//     * @param libraries the libraries
//     
		public LibraryClause(List<string> libraries)
		{
			this.libraries.AddRange(libraries);
            libraryDeclarativeRegion = new List<VHDL.LibraryDeclarativeRegion>();
            scope = Scopes.createScope(this, new LibraryClauseResolvable(this));
		}

//    *
//     * Returns the list of libraries in this library clause.
//     * @return the list of libraries
//     
		public virtual List<string> getLibraries()
		{
			return libraries;
		}

	    //TODO: implement
		public override IScope Scope
		{
            get { return scope; }
		}

		internal override void accept(LibraryUnitVisitor visitor)
		{
			visitor.visitLibraryClause(this);
		}

        [Serializable]
        private class LibraryClauseResolvable : IResolvable
        {
            private LibraryClause parent;

            public LibraryClauseResolvable(LibraryClause parent)
            {
                this.parent = parent;
            }

            #region Resolvable Members

            public object Resolve(string identifier)
            {
                foreach(LibraryDeclarativeRegion lib in parent.libraryDeclarativeRegion)
                    if(lib.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        return lib;
                return null;
            }

            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                res.AddRange(parent.libraryDeclarativeRegion);
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