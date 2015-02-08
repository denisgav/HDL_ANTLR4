using System;

namespace VHDL.expression.name
{
    /// <summary>
    /// A simple name for a named entity is either the identifier associated with the entity by
    /// its declaration or another identifier associated with the entity by an alias declaration.
    /// </summary>
    [Serializable]
    public class SimpleName : Name
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

        public override type.ISubtypeIndication Type
        {
            get { throw new NotImplementedException(); }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
