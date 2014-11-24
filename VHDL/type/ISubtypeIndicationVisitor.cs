using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.declaration;

namespace VHDL.type
{
    public interface ISubtypeIndicationVisitor
    {
        void visit(Subtype item);
        void visit(IndexSubtypeIndication item);
        void visit(RangeSubtypeIndication item);
        void visit(UnresolvedType item);
        void visit(ResolvedSubtypeIndication item);
        void visit(Type item);
    }
}
