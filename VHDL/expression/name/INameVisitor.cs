namespace VHDL.expression.name
{
    public interface INameVisitor
    {
        void visit(SimpleName name);

        void visit(FunctionCall name);

        void visit(IndexedName name);

        void visit(AttributeName name);

        void visit(SelectedName name);

        void visit(Slice name);
    }
}
