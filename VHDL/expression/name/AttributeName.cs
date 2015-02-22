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

using System;
using System.Collections.Generic;
using VHDL.type;

namespace VHDL.expression.name
{
    using Attribute = VHDL.declaration.Attribute;
    /// <summary>
    /// An attribute name denotes a value, function, type, range, signal,
    /// or constant associated with a named entity.
    /// </summary>
    [Serializable]
	public class AttributeName : Name
	{
        private readonly Name prefix;
		private readonly Attribute attribute;
        private readonly List<Expression> parameters;

        /// <summary>
        /// Creates an attribute expression.
        /// </summary>
        /// <param name="prefix">the prefix of this attribute expression</param>
        /// <param name="attribute">the attribute</param>
        public AttributeName(Name prefix, Attribute attribute)
		{
			this.prefix = prefix;
			this.attribute = attribute;
            this.parameters = new List<Expression>();
		}

        /// <summary>
        /// Creates an attribute expression with a parameter.
        /// </summary>
        /// <param name="prefix">the prefix of this attribute expression</param>
        /// <param name="attribute">the attribute</param>
        /// <param name="parameter">the parameter</param>
        public AttributeName(Name prefix, Attribute attribute, List<Expression> parameters)
		{
			this.prefix = prefix;
			this.attribute = attribute;
			this.parameters = parameters;
		}

        /// <summary>
        /// Returns the prefix of this attribute expression.
        /// </summary>
        public virtual Name Prefix
		{
            get { return prefix; }
		}

        /// <summary>
        /// Returns the attribute.
        /// </summary>
		public virtual Attribute Attribute
		{
            get { return attribute; }
		}

        /// <summary>
        /// Returns the parameter.
        /// </summary>
        public virtual List<Expression> Parameters
		{
            get { return parameters; }
		}

		public override ISubtypeIndication Type
		{
            get
            {
                //TODO: implement corrently
                if (attribute.Identifier.EqualsIgnoreCase("LAST_VALUE"))
                    return prefix.Type;
                return attribute.Type;
            }
		}

        public override VHDL.INamedEntity Referenced
        {
            get { return attribute; }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
	}

}