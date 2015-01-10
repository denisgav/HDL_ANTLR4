using VHDL.expression;
using VHDL.Object;
using VHDL.type;
using VHDL;
using VHDL.declaration;

namespace VHDLParser.typeinfer
{
    class ExpressionInference : ExpressionVisitor
    {
        private TypeInference baseInfer;
        private IDeclarativeRegion scope;
        public IFunction  convertFunction;

        public ExpressionInference(IDeclarativeRegion scope, TypeInference baseInfer)
        {
            this.baseInfer = baseInfer;
            this.scope = scope;
        }

        public IFunction  ConvertFunction
        {get{return convertFunction;}}

        protected override void visitLiteral(Literal expression)
        {
            var literalInfer = new LiteralInference(baseInfer);
            expression.accept(literalInfer);
        }

        protected override void visitName(Name name)
        {
            var arrElem = name as ArrayElement;
            if (arrElem != null)
            {
            }

            var recElem = name as RecordElement;
            if (recElem != null)
            {
                if (CheckTypeRelation(baseInfer.ExpectedType, recElem.Type))
                    baseInfer.ResultType = recElem.Type;
            }

            var attribute = name as AttributeExpression;
            if (attribute != null)
            {
                if (CheckTypeRelation(baseInfer.ExpectedType, attribute.Type))
                    baseInfer.ResultType = attribute.Type;
            }
        }

        public bool CheckTypeRelation(ISubtypeIndication expected, ISubtypeIndication actual)
        {
            string expectedName = TypeHelper.GetTypeName(expected);
            string actual_name = TypeHelper.GetTypeName(actual);
            if (expectedName != "" && expectedName == actual_name)
                return true;

            return false;
        }
    }
}
