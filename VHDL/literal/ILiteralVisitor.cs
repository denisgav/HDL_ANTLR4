using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDL.literal
{
    public interface ILiteralVisitor
    {
        void visit(CharacterLiteral literal);

        void visit(IntegerLiteral literal);

        void visit(BasedLiteral literal);

        void visit(Literals.NullLiteral literal);

        void visit(PhysicalLiteral literal);

        void visit(RealLiteral literal);

        void visit(StringLiteral literal);

        void visit(EnumerationLiteral literal);
    }
}
