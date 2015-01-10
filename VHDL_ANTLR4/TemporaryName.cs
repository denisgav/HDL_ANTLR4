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

namespace VHDL.parser.antlr
{
    /// <summary>
    /// Temporary name used during meta class generation.
    /// </summary>
    public class TemporaryName : VhdlElement
    {
        private List<Part> parts = new List<Part>();
        private IDeclarativeRegion currentScore;
        private VHDL_ANTLR4.vhdlVisitor visitor;
        private Antlr4.Runtime.ParserRuleContext context;
        public static VHDL.expression.Name CurrentAssignTarget;
        public static VHDL.expression.Name CurrentName;

        public TemporaryName(List<Part> parts, VHDL_ANTLR4.vhdlVisitor visitor, Antlr4.Runtime.ParserRuleContext context)
            : this(parts, visitor.currentScope, visitor, context)
        { }

        public TemporaryName(List<Part> parts, IDeclarativeRegion currentScore, VHDL_ANTLR4.vhdlVisitor visitor, Antlr4.Runtime.ParserRuleContext context)
        {
            this.parts = parts;
            this.currentScore = currentScore;
            this.visitor = visitor;
            this.context = context;
        }

        private T resolve<T>(IDeclarativeRegion scope) where T : class
        {
            return ObjectSearcher.Search<T>(scope, parts);
        }

        private List<T> resolveAll<T>(IDeclarativeRegion scope) where T : class
        {
            return ObjectSearcher.SearchAll<T>(scope, parts);
        }

        //used only for error reporting
        private string Identifier
        {
            get
            {
                System.Text.StringBuilder selectedName = new System.Text.StringBuilder();

                bool first = true;
                foreach (Part part in parts)
                {
                    if (part.Type == Part.TypeEnum.SELECTED)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            selectedName.Append('.');
                        }
                        selectedName.Append((part as Part.SelectedPart).Suffix);
                    }
                }

                return (selectedName.Length == 0 ? "unknown" : selectedName.ToString());
            }
        }

        public virtual VHDL.libraryunit.Entity GetEntity()
        {
            VHDL.libraryunit.Entity entity = resolve<VHDL.libraryunit.Entity>(currentScore);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.UNKNOWN_ENTITY, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    VHDL.libraryunit.Entity dummy = new VHDL.libraryunit.Entity(identifier);
                    //set parent to allow resolving of names in architectures
                    dummy.Parent = currentScore;
                    return dummy;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual VHDL.libraryunit.Configuration GetConfiguration()
        {
            VHDL.libraryunit.Configuration configuration = resolve<VHDL.libraryunit.Configuration>(currentScore);
            if (configuration != null)
            {
                return configuration;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.UNKNOWN_CONFIGURATION, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.libraryunit.Configuration(identifier, null, null);
                }
                else
                {
                    return null;
                }
            }
        }

        //TODO: don't use subtype indication
        public virtual VHDL.type.ISubtypeIndication GetTypeMark()
        {
            VHDL.type.ISubtypeIndication type = resolve<VHDL.type.ISubtypeIndication>(currentScore);
            if (type != null)
            {
                return type;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.UNKNOWN_TYPE, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.type.EnumerationType(identifier);
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual VHDL.declaration.Component GetComponent()
        {
            VHDL.declaration.Component component = resolve<VHDL.declaration.Component>(currentScore);
            if (component != null)
            {
                return component;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.UNKNOWN_COMPONENT, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.declaration.Component(identifier);
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual VHDL.Object.Signal GetSignal()
        {
            VHDL.Object.Signal signal = resolve<VHDL.Object.Signal>(currentScore);
            if (signal != null)
            {
                return signal;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.UNKNOWN_SIGNAL, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.Object.Signal(identifier, null);
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual VHDL.expression.FunctionCall ResolveFunctionCall(List<AssociationElement> arguments, VHDL.type.ISubtypeIndication currentAssignTarget, List<VHDL.declaration.FunctionDeclaration> candidates)
        {
            VHDL.declaration.FunctionDeclaration declaration = VHDLParser.typeinfer.TypeInference.ResolveOverloadFunction(currentScore, candidates, arguments, currentAssignTarget);
            VHDL.expression.FunctionCall call = new VHDL.expression.FunctionCall(declaration, arguments);
            return call;
        }

        public virtual VHDL.statement.ProcedureCall ResolveProcedureCall(List<AssociationElement> arguments, List<VHDL.declaration.ProcedureDeclaration> candidates)
        {
            VHDL.declaration.ProcedureDeclaration declaration = VHDLParser.typeinfer.TypeInference.ResolveOverloadProcedure(currentScore, candidates, arguments);
            VHDL.statement.ProcedureCall call = new VHDL.statement.ProcedureCall(declaration, arguments);
            return call;
        }

        public virtual VHDL.declaration.ProcedureDeclaration GetProcedure()
        {
            VHDL.declaration.ProcedureDeclaration procedure = resolve<VHDL.declaration.ProcedureDeclaration>(currentScore);
            if (procedure != null)
            {
                return procedure;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.PROCESS_TYPE_ERROR, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.declaration.ProcedureDeclaration(identifier);
                }
                else
                {
                    return null;
                }
            }
        }


        public virtual VHDL.statement.ProcedureCall GetProcedureCall(List<AssociationElement> arguments)
        {
            List<VHDL.declaration.ProcedureDeclaration> procedure_candidates = resolveAll<VHDL.declaration.ProcedureDeclaration>(currentScore);
            return ResolveProcedureCall(arguments, procedure_candidates);
        }

        public virtual VHDL.declaration.FunctionDeclaration GetFunction()
        {
            VHDL.declaration.FunctionDeclaration function = resolve<VHDL.declaration.FunctionDeclaration>(currentScore);
            if (function != null)
            {
                return function;
            }
            else
            {
                string identifier = Identifier;
                visitor.resolveError(context, ParseError.ParseErrorTypeEnum.PROCESS_TYPE_ERROR, identifier);

                if (visitor.Settings.CreateDummyObjects)
                {
                    return new VHDL.declaration.FunctionDeclaration(identifier, null);
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual VHDL.expression.FunctionCall GetFunctionCall(List<AssociationElement> arguments, VHDL.type.ISubtypeIndication currentAssignTarget)
        {
            List<VHDL.declaration.FunctionDeclaration> function_candidates = resolveAll<VHDL.declaration.FunctionDeclaration>(currentScore);
            return ResolveFunctionCall(arguments, currentAssignTarget, function_candidates);
        }        
    }
}