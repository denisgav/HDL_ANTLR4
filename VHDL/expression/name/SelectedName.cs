using System;
using VHDL.type;
using VHDL.Object;

namespace VHDL.expression.name
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
        private readonly Name suffix;

        public SelectedName(Name prefix, Name suffix)
        {
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public virtual Name Prefix
        {
            get { return prefix; }
        }

        public virtual Name Suffix
        {
            get { return suffix; }
        }

        public virtual string Element
        {
            get { return suffix.Referenced.Identifier; }
        }

        public override ISubtypeIndication Type
        {
            get
            {
                //TODO: implement correctly
                return prefix.Type;
            }
        }

        public override INamedEntity Referenced
        {
            get { return suffix.Referenced; }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}