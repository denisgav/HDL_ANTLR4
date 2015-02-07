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

namespace VHDL.output
{

    using Choice = VHDL.Choice;
    using Choices = VHDL.Choices;
    using ComponentSpecification = VHDL.ComponentSpecification;
    using DiscreteRange = VHDL.DiscreteRange;
    using Range = VHDL.Range;
    using RangeAttributeName = VHDL.RangeAttributeName;
    using SubtypeDiscreteRange = VHDL.SubtypeDiscreteRange;
    using VhdlElement = VHDL.VhdlElement;
    using VhdlFile = VHDL.VhdlFile;
    using ConcurrentStatement = VHDL.concurrent.ConcurrentStatement;
    using ConcurrentStatementVisitor = VHDL.concurrent.ConcurrentStatementVisitor;
    using ConfigurationItem = VHDL.configuration.ConfigurationItem;
    using ConfigurationVisitor = VHDL.configuration.ConfigurationVisitor;
    using DeclarationVisitor = VHDL.declaration.DeclarationVisitor;
    using DeclarativeItem = VHDL.declaration.DeclarativeItem;
    using DeclarativeItemMarker = VHDL.declaration.IDeclarativeItemMarker;
    using Subtype = VHDL.declaration.Subtype;
    using Aggregate = VHDL.expression.Aggregate;
    using Expression = VHDL.expression.Expression;
    using ExpressionVisitor = VHDL.expression.ExpressionVisitor;
    using LibraryUnit = VHDL.libraryunit.LibraryUnit;
    using LibraryUnitVisitor = VHDL.libraryunit.LibraryUnitVisitor;
    using UseClause = VHDL.libraryunit.UseClause;
    using ArrayElement = VHDL.Object.IndexedName;
    using RecordElement = VHDL.Object.SelectedName;
    using Signal = VHDL.Object.Signal;
    using SignalAssignmentTarget = VHDL.Object.ISignalAssignmentTarget;
    using Slice = VHDL.Object.Slice;
    using Variable = VHDL.Object.Variable;
    using VariableAssignmentTarget = VHDL.Object.IVariableAssignmentTarget;
    using SequentialStatement = VHDL.statement.SequentialStatement;
    using SequentialStatementVisitor = VHDL.statement.SequentialStatementVisitor;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using RangeSubtypeIndication = VHDL.type.RangeSubtypeIndication;
    using ResolvedSubtypeIndication = VHDL.type.ResolvedSubtypeIndication;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using Type = VHDL.type.Type;
    using TypeVisitor = VHDL.type.TypeVisitor;
    using UnresolvedType = VHDL.type.UnresolvedType;
    using System;

    /// <summary>
    /// Output module.
    /// An output module contains all visitors that are necessary to output a hierarchy
    /// of VhdlElements to a file or another data structure.
    /// </summary>
    public abstract class OutputModule
    {
        /// <summary>
        /// Writes a sequential statement.
        /// </summary>
        /// <param name="statement">the statment</param>
        public virtual void writeSeqentialStatement(SequentialStatement statement)
        {
            getSequentialStatementVisitor().visit(statement);
        }

        /// <summary>
        /// Writes a list of sequential statements.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="statements">the list of statements</param>
        public virtual void writeSequentialStatements<T1>(IList<T1> statements) where T1 : SequentialStatement
        {
            getSequentialStatementVisitor().visit(statements);
        }

        /// <summary>
        /// Writes a concurrent statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        public virtual void writeConcurrentStatement(ConcurrentStatement statement)
        {
            getConcurrentStatementVisitor().visit(statement);
        }

        /// <summary>
        /// Writes a list of concurrent statments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statements">the list of statement</param>
        public virtual void writeConcurrentStatements<T>(IList<T> statements) where T : ConcurrentStatement
        {
            getConcurrentStatementVisitor().visit(statements);
        }

        /// <summary>
        /// Writes a library unit.
        /// </summary>
        /// <param name="unit">the library unit</param>
        public virtual void writeLibraryUnit(LibraryUnit unit)
        {
            getLibraryUnitVisitor().visit(unit);
        }

        /// <summary>
        /// Writes a list of library units.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="units">the list of library units</param>
        public virtual void writeLibraryUnits<T1>(IList<T1> units) where T1 : LibraryUnit
        {
            getLibraryUnitVisitor().visit(units);
        }

        /// <summary>
        /// Writes a declaration.
        /// </summary>
        /// <param name="declaration">the declaration</param>
        public virtual void writeDeclaration(DeclarativeItem declaration)
        {
            getDeclarationVisitor().visit(declaration);
        }

        /// <summary>
        /// Writes a list of declarations.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="declarations">the declarations</param>
        public virtual void writeDeclarations<T1>(List<T1> declarations) where T1 : DeclarativeItem
        {
            getDeclarationVisitor().visit(declarations);
        }

        //TODO: remove
        /// <summary>
        /// Writes a declaration.
        /// </summary>
        /// <param name="declaration">the declaration</param>
        public virtual void writeDeclarationMarker(DeclarativeItemMarker declaration)
        {
            if (declaration is DeclarativeItem)
            {
                getDeclarationVisitor().visit((DeclarativeItem)declaration);
            }
            else if (declaration is Type)
            {
                getTypeVisitor().visit((Type)declaration);
            }
            else if (declaration is UseClause)
            {
                getLibraryUnitVisitor().visit((UseClause)declaration);
            }
            else if (declaration == null)
            {
                //ignore
            }
            else
            {
                throw new Exception("Unknown declaration marker.");
            }
        }

        //TODO: remove
        /// <summary>
        /// Writes a list of delcarations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="declarations">the list of declarations</param>
        public virtual void writeDeclarationMarkers<T>(IList<T> declarations) where T : DeclarativeItemMarker
        {
            foreach (DeclarativeItemMarker declarativeItemMarker in declarations)
            {
                writeDeclarationMarker(declarativeItemMarker);
            }
        }

        /// <summary>
        /// Writes an expression.
        /// </summary>
        /// <param name="expression">the expression</param>
        public virtual void writeExpression(Expression expression)
        {
            getExpressionVisitor().visit(expression);
        }

        /// <summary>
        /// Writes a configuration item.
        /// </summary>
        /// <param name="configuration">the configuration item</param>
        public virtual void writeConfigurationItem(ConfigurationItem configuration)
        {
            getConfigurationVisitor().visit(configuration);
        }

        /// <summary>
        /// Writes a list of configuration items.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="configurations">the list of configuration items</param>
        public virtual void writeConfigurationItems<T1>(List<T1> configurations) where T1 : ConfigurationItem
        {
            getConfigurationVisitor().visit(configurations);
        }

        /// <summary>
        /// Writes a signal assignment target.
        /// </summary>
        /// <param name="target">the target</param>
        public virtual void writeSignalAssignmentTarget(SignalAssignmentTarget target)
        {
            if (target is Aggregate)
            {
                writeExpression((Aggregate)target);
            }
            else if (target is RecordElement)
            {
                writeExpression((RecordElement)target);
            }
            else if (target is ArrayElement)
            {
                writeExpression((ArrayElement)target);
            }
            else if (target is Slice)
            {
                writeExpression((Slice)target);
            }
            else if (target is Signal)
            {
                writeExpression((Signal)target);
            }
            else if (target == null)
            {
                //ignore
            }
            else
            {
                throw new ArgumentException("Unknown signal assignment target.");
            }
        }

        /// <summary>
        /// Writes a variable assignment target.
        /// </summary>
        /// <param name="target">the target</param>
        public virtual void writeVariableAssignmentTarget(VariableAssignmentTarget target)
        {
            if (target is Aggregate)
            {
                writeExpression((Aggregate)target);
            }
            else if (target is RecordElement)
            {
                writeExpression((RecordElement)target);
            }
            else if (target is ArrayElement)
            {
                writeExpression((ArrayElement)target);
            }
            else if (target is Slice)
            {
                writeExpression((Slice)target);
            }
            else if (target is Variable)
            {
                writeExpression((Variable)target);
            }
            else if (target is Signal)
            {
                writeExpression((Signal)target);
            }
            else if (target == null)
            {
                //ignore
            }
            else
            {
                throw new ArgumentException("Unknown variable assignment target.");
            }
        }

        /// <summary>
        /// Writes a subtype indication.
        /// </summary>
        /// <param name="indication">the subtype indication</param>
        public virtual void writeSubtypeIndication(SubtypeIndication indication)
        {
            if (indication is IndexSubtypeIndication)
            {
                getMiscellaneousElementOutput().indexSubtypeIndication((IndexSubtypeIndication)indication);
            }
            else if (indication is ResolvedSubtypeIndication)
            {
                getMiscellaneousElementOutput().resolvedSubtypeIndication((ResolvedSubtypeIndication)indication);
            }
            else if (indication is RangeSubtypeIndication)
            {
                getMiscellaneousElementOutput().rangeSubtypeIndication((RangeSubtypeIndication)indication);
            }
            else if (indication is Type)
            {
                getMiscellaneousElementOutput().typeSubtypeIndication((Type)indication);
            }
            else if (indication is Subtype)
            {
                getMiscellaneousElementOutput().subtypeSubtypeIndication((Subtype)indication);
            }
            else if (indication is UnresolvedType)
            {
                getMiscellaneousElementOutput().unresolvedTypeSubtypeIndication((UnresolvedType)indication);
            }
            else if (indication == null)
            {
                //ignore
            }
            else
            {
                throw new ArgumentException("Unknown subtype indication.");
            }
        }

        /// <summary>
        /// Writes a discrete range.
        /// </summary>
        /// <param name="range">the discrete range</param>
        public virtual void writeDiscreteRange(DiscreteRange range)
        {
            if (range is Range)
            {
                getMiscellaneousElementOutput().range((Range)range);
            }
            else if (range is RangeAttributeName)
            {
                getMiscellaneousElementOutput().rangeAttributeName((RangeAttributeName)range);
            }
            else if (range is SubtypeDiscreteRange)
            {
                getMiscellaneousElementOutput().subtypeDiscreteRange((SubtypeDiscreteRange)range);
            }
            else if (range == null)
            {
                //ignore
            }
            else
            {
                throw new ArgumentException("Unknown range.");
            }
        }

        /// <summary>
        /// Writes a choice.
        /// </summary>
        /// <param name="choice">the choice</param>
        public virtual void writeChoice(Choice choice)
        {
            if (choice == Choices.OTHERS)
            {
                getMiscellaneousElementOutput().choiceOthers();
            }
            else if (choice is Expression)
            {
                writeExpression((Expression)choice);
            }
            else if (choice is DiscreteRange)
            {
                writeDiscreteRange((DiscreteRange)choice);
            }
            else if (choice == null)
            {
                //ignore
            }
            else
            {
                throw new ArgumentException("Unknown choice.");
            }
        }

        /// <summary>
        /// Writes a component specification.
        /// </summary>
        /// <param name="specification">the component specification</param>
        public virtual void writeComponentSpecification(ComponentSpecification specification)
        {
            if (specification == null)
            {
                return;
            }

            switch (specification.Type)
            {
                case ComponentSpecification.ComponentType.ALL:
                    getMiscellaneousElementOutput().allComponentSpecification(specification);
                    break;

                case ComponentSpecification.ComponentType.INSTANTIATION_LIST:
                    getMiscellaneousElementOutput().instantiationListComponentSpecification(specification);
                    break;

                case ComponentSpecification.ComponentType.OTHERS:
                    getMiscellaneousElementOutput().othersComponentSpecification(specification);
                    break;
            }
        }

        /// <summary>
        /// Writes a VhdlElement.
        /// </summary>
        /// <param name="element">the element</param>
        public virtual void writeVhdlElement(VhdlElement element)
        {
            if (element is VhdlFile)
            {
                writeLibraryUnits(((VhdlFile)element).Elements);
            }
            else if (element is ComponentSpecification)
            {
                writeComponentSpecification((ComponentSpecification)element);
            }
            else if (element is ConcurrentStatement)
            {
                writeConcurrentStatement((ConcurrentStatement)element);
            }
            else if (element is ConfigurationItem)
            {
                writeConfigurationItem((ConfigurationItem)element);
            }
            else if (element is DeclarativeItem)
            {
                writeDeclaration((DeclarativeItem)element);
            }
            else if (element is Expression)
            {
                writeExpression((Expression)element);
            }
            else if (element is LibraryUnit)
            {
                writeLibraryUnit((LibraryUnit)element);
            }
            else if (element is SequentialStatement)
            {
                writeSeqentialStatement((SequentialStatement)element);
            }
            else
            {
                throw new ArgumentException("cannot output this element");
            }
        }

        /// <summary>
        /// Returns the sequential statement visitor.
        /// </summary>
        /// <returns>the sequential statement visitor</returns>
        protected internal abstract SequentialStatementVisitor getSequentialStatementVisitor();

        /// <summary>
        /// Returns the concurrent statement visitor.
        /// </summary>
        /// <returns>the concurrent statement visitor</returns>
        protected internal abstract ConcurrentStatementVisitor getConcurrentStatementVisitor();

        /// <summary>
        /// Returns the library unit visitor.
        /// </summary>
        /// <returns>the library unit visitor</returns>
        protected internal abstract LibraryUnitVisitor getLibraryUnitVisitor();

        /// <summary>
        /// Returns the declaration visitor.
        /// </summary>
        /// <returns>the declaration visitor</returns>
        protected internal abstract DeclarationVisitor getDeclarationVisitor();

        /// <summary>
        /// Returns the expression visitor.
        /// </summary>
        /// <returns>the expression visitor</returns>
        protected internal abstract ExpressionVisitor getExpressionVisitor();

        /// <summary>
        /// Returns the configuration visitor.
        /// </summary>
        /// <returns>the configuration visitor</returns>
        protected internal abstract ConfigurationVisitor getConfigurationVisitor();

        /// <summary>
        /// Returns the type visitor.
        /// </summary>
        /// <returns>the type visitor</returns>
        protected internal abstract TypeVisitor getTypeVisitor();

        /// <summary>
        /// Returns the miscellaneous element output.
        /// </summary>
        /// <returns>the miscellaneous element output</returns>
        protected internal abstract IMiscellaneousElementOutput getMiscellaneousElementOutput();

        internal void writeExpression(long p)
        {
            throw new NotImplementedException();
        }
    }

}