using Name = VHDL.expression.Name;
using SubtypeIndication = VHDL.type.ISubtypeIndication;
using System;

namespace VHDL.Object
{
///
// * Record element.
// * @param <T> the object type
// 
//TODO: check if record element is a valid signal assignment or variable assignment target
    [Serializable]
	public class RecordElement : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
	{
        private readonly Name prefix;
		private readonly string element;

//    *
//     * Creates a record element.
//     * @param prefix the prefix of this record element
//     * @param element the identifier of the element
//     
        public RecordElement(Name prefix, string element)
		{
			this.prefix = prefix;
			this.element = element;
		}

//    *
//     * Returns the prefix of this record element.
//     * @return the preifx
//     
        public virtual Name getPrefix()
		{
			return prefix;
		}

//    *
//     * Returns the identifier of the record element.
//     * @return the identifier
//     
		public virtual string getElement()
		{
			return element;
		}

		public override SubtypeIndication Type
		{
            get
            {
                //TODO: implement correctly
                return prefix.Type;
            }
		}

        public override void accept(VHDL.expression.INameVisitor visitor)
        {
            visitor.visit(this);
        }
	}

}