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
    /// <summary>
    /// Concurrent statement visitor.
    /// The concurrent statement visitor visits all statements in a hierarchy of statements.
    /// To use this class you need to subclass it and override the <code>visit...()</code> methods
    /// you want to handle.
    /// </summary>
    [Serializable]
    public class ConcurrentStatementVisitor
    {
        /// <summary>
        /// Visits a concurrent statement.
        /// No visit method is called if the parameter equals null<.
        /// </summary>
        /// <param name="statement">the concurrent statement or null</param>
        public virtual void visit(ConcurrentStatement statement)
        {
            if (statement != null)
            {
                statement.accept(this);
            }
        }

        /// <summary>
        /// Visits a list of concurrent statements.
        /// null items in the list are ignored.
        /// The list parameter must not be <code>null</code>.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="statements">the list of concurrent statements</param>
        public virtual void visit<T1>(IList<T1> statements) where T1 : ConcurrentStatement
        {
            foreach (ConcurrentStatement statement in statements)
            {
                if (statement != null)
                {
                    statement.accept(this);
                }
            }
        }

        /// <summary>
        /// Visits a process statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitProcessStatement(AbstractProcessStatement statement)
        {
        }

        /// <summary>
        /// Visits a for generate statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitForGenerateStatement(ForGenerateStatement statement)
        {
        }

        /// <summary>
        /// Visits a if generate statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitIfGenerateStatement(IfGenerateStatement statement)
        {
        }

        /// <summary>
        /// Visits a concurrent procedure call.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitConcurrentProcedureCall(ConcurrentProcedureCall statement)
        {
        }

        /// <summary>
        /// Visits an architecture instantiation.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitArchitectureInstantiation(ArchitectureInstantiation statement)
        {
        }

        /// <summary>
        /// Visits a component instantiation.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitComponentInstantiation(ComponentInstantiation statement)
        {
        }

        /// <summary>
        /// Visits a configuration instantiation.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitConfigurationInstantiation(ConfigurationInstantiation statement)
        {
        }

        /// <summary>
        /// Visits an entity instantiation.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitEntityInstantiation(EntityInstantiation statement)
        {
        }

        /// <summary>
        /// Visits a block statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitBlockStatement(BlockStatement statement)
        {
            visit(statement.Statements);
        }

        /// <summary>
        /// Visits a concurrent assertion statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitConcurrentAssertionStatement(ConcurrentAssertionStatement statement)
        {
        }

        /// <summary>
        /// Visits a conditional signal assignment.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitConditionalSignalAssignment(ConditionalSignalAssignment statement)
        {
        }

        /// <summary>
        /// Visits a selected signal assignement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitSelectedSignalAssignment(SelectedSignalAssignment statement)
        {
        }
    }
}