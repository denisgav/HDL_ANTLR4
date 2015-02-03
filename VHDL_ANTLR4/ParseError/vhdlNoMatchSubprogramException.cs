using System;

namespace VHDL.ParseError
{
    // TODO: semantic error or smth else
    public class vhdlNoMatchSubprogramException : Exception
    {
        public vhdlNoMatchSubprogramException(string id, string subprogramKind)
            : base(string.Format("None of overloads matches the '{0}' {1} call", id, subprogramKind))
        {
        }
    }
}
