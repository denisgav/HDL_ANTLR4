namespace VHDL.expression
{
    using VHDL.Object;

    public interface INameVisitor
    {
         void visit(FunctionCall name);

         void visit(ArrayElement name);

         void visit(AttributeExpression name);

         void visit(RecordElement name);

         void visit(Slice name);

         void visit(TypedName name);

         void visit(VhdlObject name);
    }
}
