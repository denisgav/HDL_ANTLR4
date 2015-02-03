using System;

namespace VHDL.ParseError
{
    // TODO: semantic error or smth else
    public class vhdlAmbiguousCallException : Exception
    {
        public vhdlAmbiguousCallException(string id, string subprogramKind)
            : base(string.Format("Ambiguous call to '{0}' {1}", id, subprogramKind))
        {
        }
    }
}
