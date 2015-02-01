using VHDL.expression;
using VHDL.Object;
using VHDL.type;
using VHDL;
using VHDL.declaration;

namespace VHDL.parser.typeinfer
{
    class ExpressionInference : ExpressionVisitor
    {
        private TypeInference baseInfer;
        private IDeclarativeRegion scope;
        public IFunction convertFunction;

        public ExpressionInference(IDeclarativeRegion scope, TypeInference baseInfer)
        {
            this.baseInfer = baseInfer;
            this.scope = scope;
        }

        public IFunction ConvertFunction
        {
            get { return convertFunction; }
        }

        protected override void visitLiteral(Literal expression)
        {
            var literalInfer = new LiteralInference(baseInfer);
            expression.accept(literalInfer);
        }

        protected override void visitName(Name name)
        {
            var nameInfer = new NameInference(scope, baseInfer);
            name.accept(nameInfer);
        }
    }
}
