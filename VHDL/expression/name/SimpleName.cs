using System;
using VHDL.Object;

namespace VHDL.expression.name
{
    /// <summary>
    /// A simple name for a named entity is either the identifier associated with the entity by
    /// its declaration or another identifier associated with the entity by an alias declaration.
    /// </summary>
    [Serializable]
    public class SimpleName : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
    {
        private INamedEntity referenced;

        public SimpleName(INamedEntity referenced)
        {
            this.referenced = referenced;
        }

        public override INamedEntity Referenced
        {
            get { return referenced; }
        }

        // TODO: generalize types
        public override type.ISubtypeIndication Type
        {
            get
            {
                if (referenced is VhdlObject)
                    return (referenced as VhdlObject).Type;
                else if (referenced is declaration.Alias)
                    return (referenced as declaration.Alias).Aliased.Type;
                return null;
            }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
