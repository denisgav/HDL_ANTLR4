namespace VHDL.expression.name
{
    public class NameVisitor : INameVisitor
    {
        public virtual void visit(SimpleName name)
        {
        }

        public virtual void visit(FunctionCall name)
        {
        }

        public virtual void visit(IndexedName name)
        {
        }

        public virtual void visit(AttributeName name)
        {
        }

        public virtual void visit(SelectedName name)
        {
        }

        public virtual void visit(Slice name)
        {
        }
    }
}
