//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//List of predefined attrivutes
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

//   ** The syntax of an attribute is some named entity followed
//   ** by an apostrophe and one of the following attribute names.
//   ** A parameter list is used with some attributes.
//   ** Generally: T represents any type, A represents any array 
//   ** or constrained array type, S represents any signal and
//   ** E represents a named entity.

//T'BASE       is the base type of the type T
//T'LEFT       is the leftmost value of type T. (Largest if downto)
//T'RIGHT      is the rightmost value of type T. (Smallest if downto)
//T'HIGH       is the highest value of type T.
//T'LOW        is the lowest value of type T.
//T'ASCENDING  is boolean true if range of T defined with to .
//T'IMAGE(X)   is a string representation of X that is of type T.
//T'VALUE(X)   is a value of type T converted from the string X.
//T'POS(X)     is the integer position of X in the discrete type T.
//T'VAL(X)     is the value of discrete type T at integer position X.
//T'SUCC(X)    is the value of discrete type T that is the successor of X.
//T'PRED(X)    is the value of discrete type T that is the predecessor of X.
//T'LEFTOF(X)  is the value of discrete type T that is left of X.
//T'RIGHTOF(X) is the value of discrete type T that is right of X.
//A'LEFT       is the leftmost subscript of array A or constrained array type.
//A'LEFT(N)    is the leftmost subscript of dimension N of array A.
//A'RIGHT      is the rightmost subscript of array A or constrained array type.
//A'RIGHT(N)   is the rightmost subscript of dimension N of array A.
//A'HIGH       is the highest subscript of array A or constrained array type.
//A'HIGH(N)    is the highest subscript of dimension N of array A.
//A'LOW        is the lowest subscript of array A or constrained array type.
//A'LOW(N)     is the lowest subscript of dimension N of array A.
//A'RANGE      is the range  A'LEFT to A'RIGHT  or  A'LEFT downto A'RIGHT .
//A'RANGE(N)   is the range of dimension N of A.
//A'REVERSE_RANGE  is the range of A with to and downto reversed.
//A'REVERSE_RANGE(N)  is the REVERSE_RANGE of dimension N of array A.
//A'LENGTH     is the integer value of the number of elements in array A.
//A'LENGTH(N)  is the number of elements of dimension N of array A.
//A'ASCENDING  is boolean true if range of A defined with to .
//A'ASCENDING(N)  is boolean true if dimension N of array A defined with to .
//S'DELAYED(t) is the signal value of S at time now - t .
//S'STABLE     is true if no event is occurring on signal S.
//S'STABLE(t)  is true if no even has occurred on signal S for t units of time.
//S'QUIET      is true if signal S is quiet. (no event this simulation cycle)
//S'QUIET(t)   is true if signal S has been quiet for t units of time.
//S'TRANSACTION  is a bit signal, the inverse of previous value each cycle S is active.
//S'EVENT      is true if signal S has had an event this simulation cycle.
//S'ACTIVE     is true if signal S is active during current simulation cycle.
//S'LAST_EVENT is the time since the last event on signal S.
//S'LAST_ACTIVE  is the time since signal S was last active.
//S'LAST_VALUE is the previous value of signal S.
//S'DRIVING    is false only if the current driver of S is a null transaction.
//S'DRIVING_VALUE  is the current driving value of signal S.
//E'SIMPLE_NAME  is a string containing the name of entity E.
//E'INSTANCE_NAME  is a string containing the design hierarchy including E.
//E'PATH_NAME  is a string containing the design hierarchy of E to design root.

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


using System;

namespace VHDL.declaration
{
    using NamedEntity = VHDL.INamedEntity;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
///
// * Attribute declaration.
// 
    [Serializable]
	public class Attribute : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, INamedEntity
	{
		private string identifier;
	//FIXME: use type mark instead of subtype indication
		private SubtypeIndication type;

        private static Attribute[] predefinedAttributes;
        static Attribute()
        {
            predefinedAttributes = new Attribute[] {
                new Attribute( "BASE",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "LEFT",          VHDL.type.Type.VHDLBaseType), 
                new Attribute( "RIGHT",         VHDL.type.Type.VHDLBaseType),
                new Attribute( "HIGH",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "LOW",           VHDL.type.Type.VHDLBaseType),
                new Attribute( "ASCENDING",     VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "IMAGE",         VHDL.builtin.Standard.STRING),
                new Attribute( "VALUE",         VHDL.type.Type.VHDLBaseType),
                new Attribute( "POS",           VHDL.builtin.Standard.INTEGER),
                new Attribute( "VAL",           VHDL.type.Type.VHDLBaseType),
                new Attribute( "SUCC",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "PRED",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "LEFTOF",        VHDL.type.Type.VHDLBaseType),
                new Attribute( "RIGHTOF",       VHDL.type.Type.VHDLBaseType),
                new Attribute( "LEFT",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "RIGHT",         VHDL.type.Type.VHDLBaseType),
                new Attribute( "HIGH",          VHDL.type.Type.VHDLBaseType),
                new Attribute( "LOW",           VHDL.type.Type.VHDLBaseType),
                new Attribute( "RANGE",         VHDL.type.Type.VHDLBaseType),
                new Attribute( "REVERSE_RANGE",   VHDL.type.Type.VHDLBaseType),
                new Attribute( "LENGTH",        VHDL.builtin.Standard.INTEGER),
                new Attribute( "ASCENDING",     VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "DELAYED",       VHDL.type.Type.VHDLBaseType),
                new Attribute( "STABLE",        VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "QUIET",         VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "TRANSACTION",   VHDL.builtin.Standard.BIT),
                new Attribute( "EVENT",          VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "ACTIVE",         VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "LAST_EVENT",     VHDL.builtin.Standard.TIME),
                new Attribute( "LAST_ACTIVE",   VHDL.builtin.Standard.TIME),
                new Attribute( "LAST_VALUE",     VHDL.type.Type.VHDLBaseType),
                new Attribute( "DRIVING",        VHDL.builtin.Standard.BOOLEAN),
                new Attribute( "DRIVING_VALUE",  VHDL.type.Type.VHDLBaseType),
                new Attribute( "SIMPLE_NAME",    VHDL.builtin.Standard.STRING),
                new Attribute( "INSTANCE_NAME",  VHDL.builtin.Standard.STRING),
                new Attribute( "PATH_NAME",      VHDL.builtin.Standard.STRING)
            };
        }

        /// <summary>
        /// Creates a attribute declartion.
        /// </summary>
        /// <param name="identifier">the identifer</param>
        /// <param name="type">the type of this attribtue</param>
		public Attribute(string identifier, SubtypeIndication type)
		{
			this.identifier = identifier;
			this.type = type;
		}

        public static Attribute GetStandardAttribute(string name)
        {
            foreach(Attribute a in predefinedAttributes)
            {
                if(a.identifier.VHDLIdentifierEquals(name))
                    return a;
            }
            return null;
        }

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
		public virtual string Identifier
		{
            get { return identifier; }
            set { identifier = value; }
		}

        /// <summary>
        /// Returns/Sets the type of this attribtue.
        /// </summary>
		public virtual SubtypeIndication Type
		{
            get { return type; }
            set { type = value; }
		}

		internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitAttributeDeclaration(this);
		}
	}
}