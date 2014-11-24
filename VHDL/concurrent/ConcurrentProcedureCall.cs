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

using System.Collections.Generic;
using System;

namespace VHDL.concurrent
{
    using AssociationElement = VHDL.AssociationElement;

    //TODO: don't use string for procedure name

    /// <summary>
    /// Concurrent procedure call statement.
    /// </summary>
    [Serializable]
    public class ConcurrentProcedureCall : EntityStatement
    {
        private string procedure;
        private readonly List<AssociationElement> parameters;

        /// <summary>
        /// Creates a procedure call.
        /// </summary>
        /// <param name="procedure">the called procedure</param>
        public ConcurrentProcedureCall(string procedure)
        {
            this.procedure = procedure;
            this.parameters = new List<AssociationElement>();
        }

        /// <summary>
        /// Creates a procedure call with a list of parameters.
        /// </summary>
        /// <param name="procedure">the called procedure</param>
        /// <param name="parameters">the call parameters</param>
        public ConcurrentProcedureCall(string procedure, List<AssociationElement> parameters)
        {
            this.procedure = procedure;
            this.parameters = new List<AssociationElement>(parameters);
        }

        /// <summary>
        /// Creates a procedure call with parameters.
        /// </summary>
        /// <param name="procedure">the called procedure</param>
        /// <param name="parameters">the call parameters</param>
        public ConcurrentProcedureCall(string procedure, params AssociationElement[] parameters)
            : this(procedure, new List<AssociationElement>(parameters))
        {
        }

        /// <summary>
        /// Returns the parameters.
        /// </summary>
        public virtual List<AssociationElement> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Returns/Sets the called procedure.
        /// </summary>
        public virtual string Procedure
        {
            get { return procedure; }
            set { procedure = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitConcurrentProcedureCall(this);
        }
    }

}