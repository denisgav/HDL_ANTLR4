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
            if (TypeInference.AreTypesEqual(inferer.ExpectedType, name.Type))
                inferer.ResultType = name.Type;
        }

        public override void visit(AttributeName name)
        {
            if (TypeInference.AreTypesEqual(inferer.ExpectedType, name.Type))
                inferer.ResultType = name.Type;
        }

        public override void visit(SimpleName name)
        {
            if (TypeInference.AreTypesCompatible(inferer.ExpectedType, name.Type))
                inferer.ResultType = name.Type;
        }
    }
}
