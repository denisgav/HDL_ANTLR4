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
using System.Linq;
using System.Text;
using VHDL.statement;
using VHDL.parser;
using VHDLCompiler.CodeGenerator;
using VHDL.Object;
using VHDL.expression;
using VHDLRuntime.Values;
using VHDLCompiler.CodeTemplates.Statements;
using VHDLCompiler.CodeTemplates;
using VHDLCompiler.CodeTemplates.Helpers;

namespace VHDLCompiler.VHDLObserver
{
    public class SequentialStatementObserver : VHDLObserverBase
    {
        private SequentialStatement statement;
        public SequentialStatement Statement
        {
            get { return statement; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        

        public SequentialStatementObserver(SequentialStatement statement, Logger logger)
        {
            this.logger = logger;
            this.statement = statement;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            if (statement is ReportStatement)
            {
                ObserveReportStatement(compiler, statement as ReportStatement);
                return;
            }

            if (statement is VariableAssignment)
            {
                ObserveVariableAssignmentStatement(compiler, statement as VariableAssignment);
                return;
            }

            if (statement is SignalAssignment)
            {
                ObserveSignalAssignmentStatement(compiler, statement as SignalAssignment);
                return;
            }

            if (statement is IfStatement)
            {
                ObserveIfStatement(compiler, statement as IfStatement);
                return;
            }

            if (statement is ForStatement)
            {
                ObserveForStatement(compiler, statement as ForStatement);
                return;
            }

            if (statement is WhileStatement)
            {
                ObserveWhileStatement(compiler, statement as WhileStatement);
                return;
            }

            if (statement is LoopStatement)
            {
                ObserveLoopStatement(compiler, statement as LoopStatement);
                return;
            }            

            if (statement is WaitStatement)
            { return; }

            throw new NotImplementedException();
        }

        public void ObserveReportStatement(VHDLCompilerInterface compiler, ReportStatement report)
        {
            string op = VHDLOperandGenerator.GetOperand(report.ReportExpression, compiler);
            code = GenReportStatement(op);
        }

        public void ObserveVariableAssignmentStatement(VHDLCompilerInterface compiler, VariableAssignment statement)
        {
            IVariableAssignmentTarget interpretedTarget = statement.Target;
            if (interpretedTarget is Expression)
            {
                string target = VHDLOperandGenerator.GetOperand(interpretedTarget as Expression, compiler, false);
                string targetType = VHDLExpressionTypeGenerator.GetExpressionType(interpretedTarget as Expression, compiler);
                string value = VHDLOperandGenerator.GetOperand(statement.Value, compiler);

                VariableAssignTemplate template = new VariableAssignTemplate(target, value, targetType);
                code = template.TransformText();
                return;
            }
        }

        public void ObserveSignalAssignmentStatement(VHDLCompilerInterface compiler, SignalAssignment statement)
        {
            ISignalAssignmentTarget interpretedTarget = statement.Target;
            if (interpretedTarget is Expression)
            {
                string target = VHDLOperandGenerator.GetOperand(interpretedTarget as Expression, compiler, false);
                if ((statement.Waveform.Count == 1) && (statement.Waveform[0].After == null))
                {
                    string value = VHDLOperandGenerator.GetOperand(statement.Waveform[0].Value, compiler);
                    RegisterDutyCycleDelayEvent template = new RegisterDutyCycleDelayEvent(target, value);
                    code = template.TransformText();
                }
                else
                {
                    List<string> events = new List<string>();
                    foreach (VHDL.WaveformElement wfe in statement.Waveform)
                    {
                        events.Add(GetScheduledEvent(compiler, wfe));
                    }

                    if (statement.DelayMechanism == VHDL.DelayMechanism.TRANSPORT)
                    {
                        RegisterTransportDelayEvent template = new RegisterTransportDelayEvent(target, events);
                        code = template.TransformText();
                    }
                    else
                    {
                        RegisterInertialDelayEvent template;
                        if (statement.DelayMechanism.PulseRejectionLimit == null)
                        {
                            template = new RegisterInertialDelayEvent(target, events);
                        }
                        else
                        {
                            string Rejection = VHDLOperandGenerator.GetOperand(statement.DelayMechanism.PulseRejectionLimit, compiler);
                            template = new RegisterInertialDelayEvent(target, events, Rejection);
                        }
                        code = template.TransformText();
                    }
                }
            }
        }

        public string GetScheduledEvent(VHDLCompilerInterface compiler, VHDL.WaveformElement wfe)
        {
            string value = VHDLOperandGenerator.GetOperand(wfe.Value, compiler);
            string after = VHDLOperandGenerator.GetOperand(wfe.After, compiler);

            NewStatementTemplate template = new NewStatementTemplate("ScheduledEvent", value, after);
            return template.TransformText();
        }

        public void ObserveIfStatement(VHDLCompilerInterface compiler, IfStatement statement)
        {
            string condition = VHDLOperandGenerator.GetOperand(statement.Condition, compiler);
            List<string> statements = new List<string>();
            List<string> elseStatements = new List<string>();
            List<IfTemplateElsifStatement> elsifParts = new List<IfTemplateElsifStatement>();

            foreach (var st in statement.Statements)
            {
                statements.Add(GetSequentialCode(compiler, st));
            }

            foreach (var st in statement.ElseStatements)
            {
                elseStatements.Add(GetSequentialCode(compiler, st));
            }

            foreach (var elsif in statement.ElsifParts)
            {
                elsifParts.Add(GetElsifObject(compiler, elsif));
            }

            IfTemplate template = new IfTemplate(condition, statements, elsifParts, elseStatements);
            string resultCode = template.TransformText();
            code = resultCode;
        }

        private string GetSequentialCode(VHDLCompilerInterface compiler, SequentialStatement st)
        {
            SequentialStatementObserver observer = new SequentialStatementObserver(st, logger);
            observer.Observe(compiler);
            return observer.code;
        }

        private IfTemplateElsifStatement GetElsifObject(VHDLCompilerInterface compiler, IfStatement.ElsifPart elsif)
        {
            string condition = VHDLOperandGenerator.GetOperand(elsif.Condition, compiler);
            List<string> statements = new List<string>();
            foreach (var st in elsif.Statements)
            {
                statements.Add(GetSequentialCode(compiler, st));
            }
            IfTemplateElsifStatement res = new IfTemplateElsifStatement(condition, statements);
            return res;
        }

        public void ObserveLoopStatement(VHDLCompilerInterface compiler, LoopStatement statement)
        {
            throw new NotImplementedException();
        }

        public void ObserveWhileStatement(VHDLCompilerInterface compiler, WhileStatement statement)
        {
            string Condition = VHDLOperandGenerator.GetOperand(statement.Condition, compiler);

            List<string> Statements = new List<string>();
            foreach (var st in statement.Statements)
            {
                Statements.Add(GetSequentialCode(compiler, st));
            }

            WhileTemplate template = new WhileTemplate(Condition, Statements);
            code = template.TransformText();
        }

        public void ObserveForStatement(VHDLCompilerInterface compiler, ForStatement statement)
        {
            string RangeType;
            List<string> Parameters;

            VHDLRangeGenerator.FormRange(statement.Range, compiler, out RangeType, out Parameters);

            string IndexName = statement.Parameter.Identifier;
            List<string> Statements = new List<string>();
            foreach (var st in statement.Statements)
            {
                Statements.Add(GetSequentialCode(compiler, st));
            }

            ForTemplate template = new ForTemplate(RangeType, Parameters, IndexName, Statements);
            code = template.TransformText();
        }
    }
}
