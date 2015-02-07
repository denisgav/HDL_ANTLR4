using Name = VHDL.expression.Name;
using SubtypeIndication = VHDL.type.ISubtypeIndication;
using System;

namespace VHDL.Object
{
    //TODO: check if record element is a valid signal assignment or variable assignment target
    /// <summary>
    /// A selected name is used to denote a named entity whose declaration appears either
    /// within the declaration of another named entity or within a design library.
    /// </summary>
    [Serializable]
	public class SelectedName : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
	{
        private readonly Name prefix;
		private readonly string element;

//    *
//     * Creates a record element.
//     * @param prefix the prefix of this record element
//     * @param element the identifier of the element
//     
        public SelectedName(Name prefix, string element)
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