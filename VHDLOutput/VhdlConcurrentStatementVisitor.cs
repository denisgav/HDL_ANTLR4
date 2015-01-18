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

    using Annotations = VHDL.Annotations;
    using Choice = VHDL.Choice;
    using ComponentInstantiationFormat = VHDL.annotation.ComponentInstantiationFormat;
    using OptionalIsFormat = VHDL.annotation.OptionalIsFormat;
    using AbstractComponentInstantiation = VHDL.concurrent.AbstractComponentInstantiation;
    using AbstractGenerateStatement = VHDL.concurrent.AbstractGenerateStatement;
    using AbstractProcessStatement = VHDL.concurrent.AbstractProcessStatement;
    using ArchitectureInstantiation = VHDL.concurrent.ArchitectureInstantiation;
    using BlockStatement = VHDL.concurrent.BlockStatement;
    using ComponentInstantiation = VHDL.concurrent.ComponentInstantiation;
    using ConcurrentAssertionStatement = VHDL.concurrent.ConcurrentAssertionStatement;
    using ConcurrentProcedureCall = VHDL.concurrent.ConcurrentProcedureCall;
    using ConcurrentStatement = VHDL.concurrent.ConcurrentStatement;
    using ConcurrentStatementVisitor = VHDL.concurrent.ConcurrentStatementVisitor;
    using ConditionalSignalAssignment = VHDL.concurrent.ConditionalSignalAssignment;
    using ConfigurationInstantiation = VHDL.concurrent.ConfigurationInstantiation;
    using EntityInstantiation = VHDL.concurrent.EntityInstantiation;
    using ForGenerateStatement = VHDL.concurrent.ForGenerateStatement;
    using IfGenerateStatement = VHDL.concurrent.IfGenerateStatement;
    using SelectedSignalAssignment = VHDL.concurrent.SelectedSignalAssignment;
    using Entity = VHDL.libraryunit.Entity;
    using Signal = VHDL.Object.Signal;

    /// <summary>
    /// Concurrent statement output visitor.
    /// </summary>
    internal class VhdlConcurrentStatementVisitor : ConcurrentStatementVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlConcurrentStatementVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(ConcurrentStatement statement)
        {
            VhdlOutputHelper.handleAnnotationsBefore(statement, writer);
            base.visit(statement);
            VhdlOutputHelper.handleAnnotationsAfter(statement, writer);
        }

        public override void visit<T1>(IList<T1> statements)
        {
            foreach (ConcurrentStatement statement in statements)
            {
                visit(statement);
            }
        }

        private void appendLabel(ConcurrentStatement statement)
        {
            if (statement.Label != null)
            {
                writer.Append(statement.Label).Append(" : ");
            }
        }

        private void appendComponentInstantiationMaps(AbstractComponentInstantiation instantiation)
        {
            if (instantiation.GenericMap.Count != 0)
            {
                writer.NewLine();
                writer.Append(KeywordEnum.GENERIC.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().genericMap(instantiation.GenericMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }

            if (instantiation.PortMap.Count != 0)
            {
                writer.NewLine();
                writer.Append(KeywordEnum.PORT.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().portMap(instantiation.PortMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }
        }

        private void appendGenerateStatementSuffix(AbstractGenerateStatement statement)
        {
            writer.Append(' ').Append(KeywordEnum.GENERATE.ToString()).NewLine();
            if (statement.Declarations.Count != 0)
            {
                writer.Indent();
                output.writeDeclarationMarkers(statement.Declarations);
                writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine();
            }
            writer.Indent();
            visit(statement.Statements);
            writer.Dedent();
            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.GENERATE.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(statement.Label);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitArchitectureInstantiation(ArchitectureInstantiation statement)
        {
            appendLabel(statement);
            Entity entity = statement.Architecture.Entity;
            writer.Append(KeywordEnum.ENTITY.ToString()).Append(' ').AppendIdentifier(entity);
            writer.Append('(').AppendIdentifier(statement.Architecture).Append(')');
            writer.Indent();
            appendComponentInstantiationMaps(statement);
            writer.Dedent().Append(';').NewLine();
        }

        protected override void visitBlockStatement(BlockStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.BLOCK.ToString());
            if (statement.GuardExpression != null)
            {
                writer.Append("(");
                output.writeExpression(statement.GuardExpression);
                writer.Append(")");
            }

            OptionalIsFormat format = Annotations.getAnnotation<OptionalIsFormat>(statement);
            if (format != null && format.UseIs)
            {
                writer.Append(' ').Append(KeywordEnum.IS.ToString());
            }

            writer.NewLine().Indent();
            if (statement.Generic.Count != 0)
            {
                output.getMiscellaneousElementOutput().generic(statement.Generic);
                if (statement.GenericMap.Count != 0)
                {
                    writer.Append(KeywordEnum.GENERIC.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                    writer.Indent().BeginAlign();
                    output.getMiscellaneousElementOutput().genericMap(statement.GenericMap);
                    writer.EndAlign().Dedent();
                    writer.Append(");").NewLine();
                }
            }

            if (statement.Port.Count != 0)
            {
                output.getMiscellaneousElementOutput().port(statement.Port);
                if (statement.PortMap.Count != 0)
                {
                    writer.Append(KeywordEnum.PORT.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                    writer.Indent().BeginAlign();
                    output.getMiscellaneousElementOutput().portMap(statement.PortMap);
                    writer.EndAlign().Dedent();
                    writer.Append(");").NewLine();
                }
            }

            output.writeDeclarationMarkers(statement.Declarations);
            writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine();
            writer.Indent();
            visit(statement.Statements);
            writer.Dedent();
            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.BLOCK.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(statement.Label);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitComponentInstantiation(ComponentInstantiation statement)
        {
            appendLabel(statement);

            ComponentInstantiationFormat format = Annotations.getAnnotation<ComponentInstantiationFormat>(statement);

            if (format != null && format.isUseOptionalComponentKeyword())
            {
                writer.Append(KeywordEnum.COMPONENT.ToString()).Append(' ');
            }

            writer.AppendIdentifier(statement.Component).Indent();
            appendComponentInstantiationMaps(statement);
            writer.Dedent().Append(';').NewLine();
        }

        protected override void visitConcurrentAssertionStatement(ConcurrentAssertionStatement statement)
        {
            appendLabel(statement);
            if (statement.Postponed)
            {
                writer.Append(KeywordEnum.POSTPONED.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.ASSERT.ToString()).Append(' ');
            output.writeExpression(statement.Condition);
            if (statement.ReportedExpression != null)
            {
                writer.Append(' ').Append(KeywordEnum.REPORT.ToString()).Append(' ');
                output.writeExpression(statement.ReportedExpression);
            }
            if (statement.Severity != null)
            {
                writer.Append(' ').Append(KeywordEnum.SEVERITY.ToString()).Append(' ');
                output.writeExpression(statement.Severity);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitConcurrentProcedureCall(ConcurrentProcedureCall statement)
        {
            appendLabel(statement);
            if (statement.Postponed)
            {
                writer.Append(KeywordEnum.POSTPONED.ToString()).Append(' ');
            }
            writer.Append(statement.Procedure);
            if (statement.Parameters.Count != 0)
            {
                writer.Append('(');
                output.getMiscellaneousElementOutput().concurrentProcedureCallParameters(statement.Parameters);
                writer.Append(')');
            }
            writer.Append(';').NewLine();
        }

        protected override void visitConditionalSignalAssignment(ConditionalSignalAssignment statement)
        {
            appendLabel(statement);
            if (statement.Postponed)
            {
                writer.Append(KeywordEnum.POSTPONED.ToString()).Append(' ');
            }

            output.writeSignalAssignmentTarget(statement.Target);
            writer.Append(" <= ");

            if (statement.Guarded)
            {
                writer.Append(KeywordEnum.GUARDED.ToString()).Append(' ');
            }

            if (statement.DelayMechanism != null)
            {
                output.getMiscellaneousElementOutput().delayMechanism(statement.DelayMechanism);
                writer.Append(' ');
            }

            bool first = true;
            foreach (ConditionalSignalAssignment.ConditionalWaveformElement element in statement.ConditionalWaveforms)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(' ').Append(KeywordEnum.ELSE.ToString()).Append(' ');
                }

                output.getMiscellaneousElementOutput().waveform(element.Waveform);

                if (element.Condition != null)
                {
                    writer.Append(' ').Append(KeywordEnum.WHEN.ToString()).Append(' ');
                    output.writeExpression(element.Condition);
                }
            }
            writer.Append(';').NewLine();
        }

        protected override void visitConfigurationInstantiation(ConfigurationInstantiation statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.CONFIGURATION.ToString()).Append(' ');
            writer.AppendIdentifier(statement.Configuration).Indent();
            appendComponentInstantiationMaps(statement);
            writer.Dedent().Append(';').NewLine();
        }

        protected override void visitEntityInstantiation(EntityInstantiation statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.ENTITY.ToString()).Append(' ');
            writer.AppendIdentifier(statement.Entity).Indent();
            appendComponentInstantiationMaps(statement);
            writer.Dedent().Append(';').NewLine();
        }

        protected override void visitForGenerateStatement(ForGenerateStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.FOR.ToString()).Append(' ');
            output.writeExpression(statement.Parameter);
            writer.Append(' ').Append(KeywordEnum.IN.ToString()).Append(' ');
            output.writeDiscreteRange(statement.Range);
            appendGenerateStatementSuffix(statement);
        }

        protected override void visitIfGenerateStatement(IfGenerateStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.IF.ToString()).Append(' ');
            output.writeExpression(statement.Condition);
            appendGenerateStatementSuffix(statement);
        }

        protected override void visitProcessStatement(AbstractProcessStatement statement)
        {
            appendLabel(statement);
            if (statement.Postponed)
            {
                writer.Append(KeywordEnum.POSTPONED.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.PROCESS.ToString());
            if (statement.SensitivityList.Count != 0)
            {
                writer.Append("(");
                bool first = true;
                foreach (Signal signal in statement.SensitivityList)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append(", ");
                    }
                    if (signal == null)
                    {
                        writer.Append("null");
                    }
                    else
                    {
                        writer.AppendIdentifier(signal);
                    }
                }
                writer.Append(")");
            }

            OptionalIsFormat format = Annotations.getAnnotation<OptionalIsFormat>(statement);
            if (format != null && format.UseIs)
            {
                writer.Append(' ').Append(KeywordEnum.IS.ToString());
            }

            writer.NewLine().Indent();
            output.writeDeclarationMarkers(statement.Declarations);
            writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine().Indent();
            output.writeSequentialStatements(statement.Statements);
            writer.Dedent().Append(KeywordEnum.END.ToString());
            if (statement.Postponed && writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(KeywordEnum.POSTPONED.ToString());
            }
            writer.Append(' ');
            writer.Append(KeywordEnum.PROCESS.ToString());
            if (statement.Label != null && writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(statement.Label);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitSelectedSignalAssignment(SelectedSignalAssignment statement)
        {
            appendLabel(statement);
            if (statement.Postponed)
            {
                writer.Append(KeywordEnum.POSTPONED.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.WITH.ToString()).Append(' ');
            output.writeExpression(statement.Expression);
            writer.Append(' ').Append(KeywordEnum.SELECT.ToString());
            writer.NewLine().Indent();
            output.writeSignalAssignmentTarget(statement.Target);
            writer.Append(" <=");
            if (statement.Guarded)
            {
                writer.Append(' ').Append(KeywordEnum.GUARDED.ToString());
            }
            if (statement.DelayMechanism != null)
            {
                writer.Append(' ');
                output.getMiscellaneousElementOutput().delayMechanism(statement.DelayMechanism);
            }
            writer.NewLine().Indent();

            bool first = true;
            foreach (SelectedSignalAssignment.SelectedWaveform waveform in statement.SelectedWaveforms)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(',').NewLine();
                }

                output.getMiscellaneousElementOutput().waveform(waveform.Waveform);

                writer.Append(' ').Append(KeywordEnum.WHEN.ToString()).Append(' ');

                bool first3 = true;
                foreach (Choice choice in waveform.Choices)
                {
                    if (first3)
                    {
                        first3 = false;
                    }
                    else
                    {
                        writer.Append(" | ");
                    }
                    output.writeChoice(choice);
                }
            }
            writer.Append(';').Dedent().Dedent().NewLine();
        }
    }

}