using System.Collections.Generic;

namespace VHDL.libraryunit
{


///
// * Library unit visitor.
// * The library unit visits all library units in a list of library units.
// * To use this class you need to subclass it and override the <code>visit...()</code> methods
// * you want to handle.
// 
	public class LibraryUnitVisitor
	{

//    *
//     * Visits a library unit.
//     * No visit method is called when the parameter is <code>null</code>.
//     * @param unit the library unit
//     
		public virtual void visit(LibraryUnit unit)
		{
			if (unit != null)
			{
				unit.accept(this);
			}
		}

//    *
//     * Visits a list of library units.
//     * <code>null</code> items in the list are ignored.
//     * The list parameter must not be <code>null</code>.
//     * @param units the list of units
//     
		public virtual void visit<T1>(IList<T1> units) where T1 : LibraryUnit
		{
			foreach (LibraryUnit unit in units)
			{
				if (unit != null)
				{
					unit.accept(this);
				}
			}
		}

//    *
//     * Visits an architecture.
//     * @param architecture the architecture
//     
		protected internal virtual void visitArchitecture(Architecture architecture)
		{
		}

//    *
//     * Visits a configuration.
//     * @param configuration the configuration
//     
		protected internal virtual void visitConfiguration(Configuration configuration)
		{
		}

//    *
//     * Visits an entity.
//     * @param entity the entity
//     
		protected internal virtual void visitEntity(Entity entity)
		{
		}

//    *
//     * Visits a package body.
//     * @param packageBody the package body
//     
		protected internal virtual void visitPackageBody(PackageBody packageBody)
		{
		}

//    *
//     * Visits a package declaration.
//     * @param packageDeclaration the pacakge declaration
//     
		protected internal virtual void visitPackageDeclaration(PackageDeclaration packageDeclaration)
		{
		}

//    *
//     * Visits a library clause.
//     * @param libraryClause the library clause
//     
	//TODO: remove: library clause is no library unit
		protected internal virtual void visitLibraryClause(LibraryClause libraryClause)
		{
		}

//    *
//     * Visits a use clause
//     * @param useClause the use clause
//     
		protected internal virtual void visitUseClause(UseClause useClause)
		{
		}
	}

}