using VHDL.expression;
using VHDL.expression.name;

namespace VHDL.output
{
    internal class VhdlNameVisitor : INameVisitor
    {
        private VhdlWriter writer;
        private OutputModule output;

        public VhdlNameVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public void visit(SimpleName name)
        {
            writer.AppendIdentifier(name.Referenced);
        }

        public void visit(expression.FunctionCall name)
        {
            output.getExpressionVisitor().visit(name);
        }

        public void visit(IndexedName name)
        {
            output.writeExpression(name.Prefix);
            writer.Append('(');
            bool first = true;
            foreach (Expression index in name.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }
                output.writeExpression(index);
            }
            writer.Append(')');
        }

        public void visit(AttributeName name)
        {
            output.writeExpression(name.Prefix);
            writer.Append('\'');
            writer.AppendIdentifier(name.Attribute);
            if (name.Parameters.Count != 0)
            {
                writer.Append('(');
                for (int i = 0; i < name.Parameters.Count; i++)
                {
                    Expression p = name.Parameters[i];
                    output.writeExpression(p);
                    if (i != name.Parameters.Count - 1)
                    {
                        writer.Append(", ");
                    }
                }
                writer.Append(')');
            }
        }

        public void visit(SelectedName name)
        {
            output.writeExpression(name.getPrefix());
            writer.Append('.');
            writer.Append(name.getElement());
        }

        public void visit(Slice name)
        {
            output.writeExpression(name.Prefix);
            writer.Append('(');
            for (int i = 0; i < name.Ranges.Count; i++)
            {
                DiscreteRange r = name.Ranges[i];
                output.writeDiscreteRange(r);
                if (i < name.Ranges.Count - 1)
                    writer.Append(", ");
            }
            writer.Append(')');
        }
    }
}
