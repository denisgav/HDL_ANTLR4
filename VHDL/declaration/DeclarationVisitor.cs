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

namespace VHDL.declaration
{
    /// <summary>
    /// Declaration visitor.
    /// </summary>
    [Serializable]
    public class DeclarationVisitor
    {
        /// <summary>
        /// Visits a declaration.
        /// No visit method is called if the parameter is null.
        /// </summary>
        /// <param name="declaration">the declaration or null</param>
        public virtual void visit(DeclarativeItem declaration)
        {
            if (declaration != null)
            {
                declaration.accept(this);
            }
        }

        /// <summary>
        /// Visits a list of declarations.
        /// null> items in the list are ignored.
        /// The list parameter must not be null.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="declarations">the list of declarations</param>
        public virtual void visit<T1>(List<T1> declarations) where T1 : DeclarativeItem
        {
            foreach (DeclarativeItem declaration in declarations)
            {
                if (declaration != null)
                {
                    declaration.accept(this);
                }
            }
        }

        /// <summary>
        /// Vists an alias declaration.
        /// </summary>
        /// <param name="declaration">the alias declaration</param>
        protected internal virtual void visitAliasDeclaration(Alias declaration)
        {
        }

        /// <summary>
        /// Visits an attribute declaration.
        /// </summary>
        /// <param name="declaration">the attribute declaration</param>
        protected internal virtual void visitAttributeDeclaration(Attribute declaration)
        {
        }

        /// <summary>
        /// Visits an attribute specification.
        /// </summary>
        /// <param name="specification">the attribute specification</param>
        protected internal virtual void visitAttributeSpecification(AttributeSpecification specification)
        {
        }

        /// <summary>
        /// Visits a component declaration.
        /// </summary>
        /// <param name="declaration">the component declaration</param>
        protected internal virtual void visitComponentDeclaration(Component declaration)
        {
        }

        /// <summary>
        /// Visits a configuration specification.
        /// </summary>
        /// <param name="specification">the configuration specification</param>
        protected internal virtual void visitConfigurationSpecification(ConfigurationSpecification specification)
        {
        }

        /// <summary>
        /// Visits a constant declaration.
        /// </summary>
        /// <param name="declaration">the constant declaration</param>
        protected internal virtual void visitConstantDeclaration(ConstantDeclaration declaration)
        {
        }

        /// <summary>
        /// Visits a disconnection specification.
        /// </summary>
        /// <param name="specification">the disconnection specification</param>
        protected internal virtual void visitDisconnectionSpecification(DisconnectionSpecification specification)
        {
        }

        /// <summary>
        /// Visits a file declaration.
        /// </summary>
        /// <param name="declaration">the file declaration</param>
        protected internal virtual void visitFileDeclaration(FileDeclaration declaration)
        {
        }

        /// <summary>
        /// Visits a function body.
        /// </summary>
        /// <param name="declaration">the function body</param>
        protected internal virtual void visitFunctionBody(FunctionBody declaration)
        {
        }

        /// <summary>
        /// Visits a function declaration.
        /// </summary>
        /// <param name="declaration">the function declaration</param>
        protected internal virtual void visitFunctionDeclaration(FunctionDeclaration declaration)
        {
        }

        /// <summary>
        /// Visits a group declaration.
        /// </summary>
        /// <param name="declaration">the group declaration</param>
        protected internal virtual void visitGroupDeclaration(Group declaration)
        {
        }

        /// <summary>
        /// Visits a group template declaration.
        /// </summary>
        /// <param name="declaration">the group template declaration</param>
        protected internal virtual void visitGroupTemplateDeclaration(GroupTemplate declaration)
        {
        }

        /// <summary>
        /// Visits a procedure body.
        /// </summary>
        /// <param name="declaration">the procedure body</param>
        protected internal virtual void visitProcedureBody(ProcedureBody declaration)
        {
        }

        /// <summary>
        /// Visits a procedure declaration.
        /// </summary>
        /// <param name="declaration">the procedure declaration</param>
        protected internal virtual void visitProcedureDeclaration(ProcedureDeclaration declaration)
        {
        }

        /// <summary>
        /// Visits a signal declaration.
        /// </summary>
        /// <param name="declaration">the signal declaration</param>
        protected internal virtual void visitSignalDeclaration(SignalDeclaration declaration)
        {
        }

        /// <summary>
        /// Visits a subtype declaration.
        /// </summary>
        /// <param name="declaration">the subtype declaration</param>
        protected internal virtual void visitSubtypeDeclaration(Subtype declaration)
        {
        }

        /// <summary>
        /// Visits a variable declaration.
        /// </summary>
        /// <param name="declaration">the variable declaration</param>
        protected internal virtual void visitVariableDeclaration(VariableDeclaration declaration)
        {
        }
    }

}