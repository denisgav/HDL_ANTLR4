namespace VHDL.expression
{
    using VHDL.Object;

    public class NameVisitor : INameVisitor
    {
        public virtual void visit(FunctionCall name)
        {
        }

        public virtual void visit(IndexedName name)
        {
        }

        public virtual void visit(AttributeExpression name)
        {
        }

        public virtual void visit(SelectedName name)
        {
        }

        public virtual void visit(Slice name)
        {
        }

        public virtual void visit(TypedName name)
        {
        }

        public virtual void visit(VhdlObject name)
        {
        }
    }
}
