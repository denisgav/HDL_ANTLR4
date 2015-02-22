using VHDL.literal;
using VHDL.type;

namespace VHDL.parser.typeinfer
{
    class LiteralInference : ILiteralVisitor
    {
        private TypeInference baseInfer;

        public LiteralInference(TypeInference baseInfer)
        {
            this.baseInfer = baseInfer;
        }

        public virtual void visit(CharacterLiteral literal)
        {
            if (CheckCharacter(literal.Character, baseInfer.ExpectedType))
                baseInfer.ResultType = baseInfer.ExpectedType;
        }

        public virtual void visit(IntegerLiteral literal)
        {
            var intType = baseInfer.ExpectedType as IntegerType;
            if (intType != null)
                baseInfer.ResultType = intType;
        }

        public virtual void visit(StringLiteral literal)
        {
            var arrType = baseInfer.ExpectedType as ArrayType;
            if (arrType != null)
            {
                var elemType = TypeHelper.GetBaseType(arrType.ElementType);
                for (int i = 0; i < literal.String.Length; ++i)
                {
                    char ch = literal.String[i];
                    if (!CheckCharacter(ch, elemType))
                        return;
                }
                baseInfer.ResultType = arrType;
            }
        }

        private static bool CheckCharacter(char ch, ISubtypeIndication type)
        {
            var enumType = type as EnumerationType;
            if (enumType != null)
            {
                // Special case: not all literals present in this type
                if (enumType.Identifier.EqualsIgnoreCase("character"))
                    return true;

                foreach (var enumLiteral in enumType.Literals)
                {
                    string enumStr = enumLiteral.ToString();
                    if (enumStr.IndexOf(ch) == 1) // e.g. '1'
                        return true;
                }
            }
            return false;
        }


        public void visit(BasedLiteral literal)
        {
            baseInfer.AnalyzeType(literal.Type);
        }

        public void visit(Literals.NullLiteral literal)
        {
            baseInfer.AnalyzeType(literal.Type);
        }

        public void visit(PhysicalLiteral literal)
        {
            baseInfer.AnalyzeType(literal.Type);
        }

        public void visit(RealLiteral literal)
        {
            baseInfer.AnalyzeType(literal.Type);
        }

        public void visit(EnumerationLiteral literal)
        {
            baseInfer.AnalyzeType(literal.Type);
        }
    }
}
