using VHDL;
using VHDL.expression;
using VHDL.expression.name;
using VHDL.Object;

namespace VHDL.parser.typeinfer
{
    class NameInference : NameVisitor
    {
        private TypeInference inferer;
        private IDeclarativeRegion scope;

        public NameInference(IDeclarativeRegion scope, TypeInference baseInfer)
        {
            this.inferer = baseInfer;
            this.scope = scope;
        }

        public override void visit(SelectedName name)
        {
            inferer.AnalyzeType(name.Type);
        }

        public override void visit(AttributeName name)
        {
            inferer.AnalyzeType(name.Type);
        }

        public override void visit(SimpleName name)
        {
            inferer.AnalyzeType(name.Type);
        }
    }
}
