using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDL.Object
{
    using VHDL.expression;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using System.Collections.Generic;

    //TODO: check if slice is a valid signal assignment or variable assignment target
    /// <summary>
    /// Slice of a VhdlObject.
    /// </summary>
    [Serializable]
    public class TypedName : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
    {
        private readonly SubtypeIndication type;

        /// <summary>
        /// Creates a slice.
        /// </summary>
        /// <param name="prefix">the slice prefix</param>
        /// <param name="range">the range</param>
        public TypedName(SubtypeIndication type)
        {
            this.type = type;
        }

        public override SubtypeIndication Type
        {
            get
            {
                //TODO: implement correctly
                return type;
            }
        }
    }
}
