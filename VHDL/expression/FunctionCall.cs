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

namespace VHDL.expression
{
    using AssociationElement = VHDL.AssociationElement;
    using Function = VHDL.declaration.IFunction;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Function call.
    /// </summary>
    [Serializable]
	public class FunctionCall : Name
	{
		private Function function;
		private readonly List<AssociationElement> parameters = new List<AssociationElement>();

        /// <summary>
        /// Creates a function call.
        /// </summary>
        /// <param name="function">the called function</param>
        public FunctionCall(Function function)
            : this(function, new List<AssociationElement>())
        {
        }

        /// <summary>
        /// Creates a function call.
        /// </summary>
        /// <param name="function">the called function</param>
        public FunctionCall(Function function, List<AssociationElement> parameters)
		{
            this.parameters = parameters;
			this.function = function;
		}

        /// <summary>
        /// Creates a function call.
        /// </summary>
        /// <param name="function">the called function</param>
        public FunctionCall(Function function, params AssociationElement[] parameters)
        {
            this.parameters.AddRange(parameters);
            this.function = function;
        }

        /// <summary>
        /// Returns/Sets the called function.
        /// </summary>
		public virtual Function Function
		{
            get { return function; }
            set { function = value; }
		}

        /// <summary>
        /// Returns the function call parameters.
        /// </summary>
		public virtual List<AssociationElement> Parameters
		{
            get { return parameters; }
		}

		public override SubtypeIndication Type
		{
            get { return function.ReturnType; }
		}

        public override void accept(ExpressionVisitor visitor)
		{
			visitor.visitFunctionCall(this);
		}

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }

        public override Choice copy()
		{
			FunctionCall call = new FunctionCall(function);
			foreach (AssociationElement parameter in parameters)
			{
				call.Parameters.Add(new AssociationElement(parameter.Formal, parameter.Actual.copy() as Expression));
			}

			return call;
		}
	}

}