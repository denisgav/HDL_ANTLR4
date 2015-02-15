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

namespace VHDL.Object
{
    using Expression = VHDL.expression.Expression;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    /// <summary>
    /// Variable.
    /// </summary>
    [Serializable]
	public class Variable : DefaultVhdlObject
	{
		private Expression defaultValue;
		private bool shared;

        /// <summary>
        /// Creates a variable.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="type">the type</param>
		public Variable(string identifier, SubtypeIndication type) : base(identifier, type)
		{
			Mode = ModeEnum.IN;
		}

        /// <summary>
        /// Creates a variable with a default value.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="type">the type</param>
        /// <param name="defaultValue">the default value</param>
		public Variable(string identifier, SubtypeIndication type, Expression defaultValue) : this(identifier, type)
		{
			this.defaultValue = defaultValue;
		}

        /// <summary>
        /// Returns/Sets the default value.
        /// </summary>
		public virtual Expression DefaultValue
		{
            get { return defaultValue; }
            set { defaultValue = value; }
		}
       
        /// <summary>
        /// Returns/Sets if this variable is shared.
        /// </summary>
		public virtual bool Shared
		{
            get { return shared; }
            set { shared = value; }
		}


        public override ModeEnum Mode
		{
            set
            {
                if (value == ModeEnum.BUFFER || value == ModeEnum.LINKAGE)
                {
                    throw new Exception("Mode " + value + " is not allowed for a variable");
                }

                base.Mode = value;
            }
		}

		public new List<Variable> VhdlObjects
		{
            get
            {
                List<Variable> res = new List<Variable>();
                res.Add(this);
                return res;
            }
		}

		public override ObjectClassEnum ObjectClass
		{
            get
            {
                return ObjectClassEnum.VARIABLE;
            }
		}
	}

}