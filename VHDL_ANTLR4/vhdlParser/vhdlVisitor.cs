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

namespace VHDL_ANTLR4
{
    using System.Collections.Generic;

    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    using VHDL;
    using VHDL.libraryunit;
    using VHDL.builtin;
    using VHDL.concurrent;
    using VHDL.statement;
    using VHDL.expression;
    using VHDL.parser;
    using VHDL.Object;
    using VHDL.annotation;
    using VHDL.declaration;
    using VHDL.literal;

    using Annotations = VHDL.Annotations;
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using LibraryDeclarativeRegion = VHDL.LibraryDeclarativeRegion;
    using RootDeclarativeRegion = VHDL.RootDeclarativeRegion;
    using VhdlElement = VHDL.VhdlElement;
    using DeclarativeItemMarker = VHDL.declaration.IDeclarativeItemMarker;
    using VhdlParserSettings = VHDL.parser.VhdlParserSettings;
    using PositionInformation = VHDL.annotation.PositionInformation;
    using SourcePosition = VHDL.annotation.SourcePosition;
    using Comments = VHDL.util.Comments;



    /// <summary>
    /// Description of vhdlVisitor.
    /// </summary>
    public partial class vhdlVisitor : vhdlAbstractVisitor
    {
        public vhdlVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager)
            : base(settings, rootScope, libraryScope, libraryManager)
        {
        }

        public vhdlVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager, string fileName)
            : base(settings, rootScope, libraryScope, libraryManager, fileName)
        {
        }


        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.assertion_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssertion_statement([NotNull] vhdlParser.Assertion_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var assertion_in = context.assertion();

            AssertionStatement assertion = ParseExtention.Parse<vhdlParser.AssertionContext, AssertionStatement>(assertion_in, VisitAssertion);
            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            assertion.Label = label;

            return assertion;
        }


        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_waveforms"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_waveforms([NotNull] vhdlParser.Conditional_waveformsContext context)
        {
            var waveform_in = context.waveform();
            var condition_in = context.condition();

            List<WaveformElement> wes = new List<WaveformElement>();
            foreach (var we_in in waveform_in.waveform_element())
            {
                WaveformElement we = ParseExtention.Parse<vhdlParser.Waveform_elementContext, WaveformElement>(we_in, VisitWaveform_element);
                wes.Add(we);
            }
            Expression condition = (condition_in != null) ? ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition) : null;
            VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement cwe = new ConditionalSignalAssignment.ConditionalWaveformElement(wes, condition);

            return cwe;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.sequential_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSequential_statement([NotNull] vhdlParser.Sequential_statementContext context)
        {
            if (context.NULL() != null)
                return new NullStatement();

            return VisitChildren(context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.record_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRecord_type_definition([NotNull] vhdlParser.Record_type_definitionContext context)
        {
            var element_declarations_in = context.element_declaration();

            VHDL.type.RecordType res = new VHDL.type.RecordType("unknown");

            foreach (var element_declaration in element_declarations_in)
            {
                VHDL.type.RecordType.ElementDeclaration el = ParseExtention.Parse<vhdlParser.Element_declarationContext, VHDL.type.RecordType.ElementDeclaration>(element_declaration, VisitElement_declaration);
                res.Elements.Add(el);
            }

            return VisitChildren(context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.choice"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitChoice([NotNull] vhdlParser.ChoiceContext context)
        {
            var discrete_range = context.discrete_range();
            var identifier = context.identifier();
            var OTHERS = context.OTHERS();
            var simple_expression = context.simple_expression();

            if (discrete_range != null)
            {
                return VisitDiscrete_range(discrete_range);
            }

            if (identifier != null)
            {
                return resolve<VhdlElement>(identifier.GetText());
            }

            if (OTHERS != null)
            {
                return Choices.OTHERS;
            }

            if (simple_expression != null)
            {
                return VisitSimple_expression(simple_expression);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.alias_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAlias_declaration([NotNull] vhdlParser.Alias_declarationContext context)
        {
            var alias_designator_in = context.alias_designator();
            var alias_indication_in = context.alias_indication();
            var name_in = context.name();
            var signature_in = context.signature();

            string designator = alias_designator_in.GetText();
            Name name = ParseExtention.Parse<vhdlParser.NameContext, Name>(name_in, VisitName);
            Signature signature = (signature_in != null) ? ParseExtention.Parse<vhdlParser.SignatureContext, Signature>(signature_in, VisitSignature) : null;
            VHDL.type.ISubtypeIndication subtypeIndication = ((alias_indication_in != null) && (alias_indication_in.subtype_indication() != null)) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(alias_indication_in.subtype_indication(), VisitSubtype_indication) : null;

            Alias res = new Alias(designator, subtypeIndication, name);
            res.Signature = signature;

            AddAnnotations(res, context);

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_body([NotNull] vhdlParser.Architecture_bodyContext context)
        {
            //--------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //--------------------------------------------------
            var identifiers_in = context.identifier();
            var architecture_declarative_part_in = context.architecture_declarative_part();
            var architecture_statement_part_in = context.architecture_statement_part();

            string architecture_name = identifiers_in[0].GetText();
            string entity_name = identifiers_in[1].GetText();

            Entity parentEntity = resolve<Entity>(entity_name);
            if (parentEntity == null)
            {
                throw new System.ArgumentException(string.Format("Could not find entity {0}", entity_name));
            }
            Architecture res = new Architecture(architecture_name, parentEntity);
            res.ContextItems.AddRange(contextItems);
            res.Parent = oldScope;
            currentScope = res;

            //------ architecture_declarative_part  ------------
            ParseArchitectureDeclarativePart(res, architecture_declarative_part_in);
            //--------------------------------------------------

            //------ architecture_statement_part_in ------------
            ParseArchitectureStatements(res, architecture_statement_part_in);
            //--------------------------------------------------

            //--------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(res, context);

            if (identifiers_in.Length == 3)
            {
                //end identifier shouls be the same as start identifier
                string architecture_name_end = identifiers_in[2].GetText();
                if (architecture_name_end.VHDLIdentifierEquals(architecture_name) == false)
                {
                    throw new System.ArgumentException(string.Format("Architecture end identifier mismatch. Architecture name is {0}, end identifier is {1}", architecture_name, architecture_name_end));
                }
            }

            //--------------------------------------------------

            return res;
        }

        public void ParseArchitectureDeclarativePart([NotNull] Architecture arch, [NotNull] vhdlParser.Architecture_declarative_partContext architecture_declarative_part_in)
        {
            var block_declarative_items_in = architecture_declarative_part_in.block_declarative_item();

            arch.Declarations.Clear();
            foreach (var d in block_declarative_items_in)
            {
                VHDL.declaration.IBlockDeclarativeItem declaration = ParseExtention.Parse<vhdlParser.Block_declarative_itemContext, VHDL.declaration.IBlockDeclarativeItem>(d, VisitBlock_declarative_item);
                arch.Declarations.Add(declaration);
            }
        }

        public void ParseArchitectureStatements([NotNull] Architecture arch, [NotNull] vhdlParser.Architecture_statement_partContext architecture_statement_part)
        {
            foreach (var statement in architecture_statement_part.architecture_statement())
            {
                ConcurrentStatement st = ParseExtention.Parse<vhdlParser.Architecture_statementContext, ConcurrentStatement>(statement, VisitArchitecture_statement);
                arch.Statements.Add(st);
            }
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subtype_indication"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubtype_indication([NotNull] vhdlParser.Subtype_indicationContext context)
        {
            var constraint_in = context.constraint();
            var tolerance_aspect_in = context.tolerance_aspect();
            var name_in = context.selected_name()[0];
            string string_name = name_in.GetText();
            var resolution_function_in = (context.selected_name().Length == 2) ? context.selected_name()[1] : null;

            VHDL.type.ISubtypeIndication res = null;
            if (resolution_function_in != null)
            {
                string resolution_function_name = resolution_function_in.GetText();
                res = new VHDL.type.ResolvedSubtypeIndication(resolution_function_name, res);
            }
            else
            {
                res = resolve<VHDL.type.ISubtypeIndication>(string_name);
            }

            if (constraint_in != null)
            {
                var range_constraint = constraint_in.range_constraint();
                if (range_constraint != null)
                {
                    RangeProvider range = ParseExtention.Parse<vhdlParser.RangeContext, RangeProvider>(range_constraint.range(), VisitRange);
                    res = new VHDL.type.RangeSubtypeIndication(res, range);
                }

                var index_constraint = constraint_in.index_constraint();
                if (index_constraint != null)
                {
                    List<DiscreteRange> ranges = ParseExtention.ParseList<vhdlParser.Discrete_rangeContext, DiscreteRange>(index_constraint.discrete_range(), VisitDiscrete_range);
                    res = new VHDL.type.IndexSubtypeIndication(res, ranges);
                }

                if ((range_constraint == null) && (index_constraint == null))
                {
                    throw new System.NotSupportedException(string.Format("Could not analyse item {0}", constraint_in.ToStringTree()));
                }
            }

            VhdlElement return_value = res as VhdlElement;

            return return_value;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.process_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcess_statement([NotNull] vhdlParser.Process_statementContext context)
        {
            var identifier_in = context.identifier();
            var label_colon_in = context.label_colon();
            bool is_postponed = context.POSTPONED() != null;
            var process_declarative_part_in = context.process_declarative_part();
            var process_statement_part_in = context.process_statement_part();
            var sensitivity_list_in = context.sensitivity_list();

            ProcessStatement process = new ProcessStatement();

            //----------------------------------------------------------
            //   Before parsing
            //----------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            process.Parent = oldScope;
            currentScope = process;
            //----------------------------------------------------------

            // 1. check label
            if (label_colon_in != null)
            {
                string label = label_colon_in.identifier().GetText();
                process.Label = label;
            }

            //2. check postponed
            process.Postponed = is_postponed;

            //3. check sensitivity list
            if (sensitivity_list_in != null)
            {
                foreach (var item in sensitivity_list_in.name())
                {
                    Signal s = resolve<Signal>(item.GetText());
                    process.SensitivityList.Add(s);
                }
            }

            //4. Add process declarations
            foreach (var declaration in process_declarative_part_in.process_declarative_item())
            {
                IProcessDeclarativeItem pdi = ParseExtention.Parse<vhdlParser.Process_declarative_itemContext, IProcessDeclarativeItem>(declaration, VisitProcess_declarative_item);
                process.Declarations.Add(pdi);
            }

            //5. Add process sequential statements
            foreach (var statement in process_statement_part_in.sequential_statement())
            {
                SequentialStatement st = ParseExtention.Parse<vhdlParser.Sequential_statementContext, SequentialStatement>(statement, VisitSequential_statement);
                process.Statements.Add(st);
            }

            //----------------------------------------------------------
            //   After parsing
            //----------------------------------------------------------
            currentScope = oldScope;
            //----------------------------------------------------------

            //----------------------------------------------------------
            //   Additioal check
            //----------------------------------------------------------
            CheckProcess(context, process);
            if (identifier_in != null)
            {
                string end_identifier = identifier_in.GetText();
                if (end_identifier.VHDLIdentifierEquals(process.Label))
                {
                    throw new System.ArgumentException(string.Format("Identifier mismatch in process. End identifier is '{0}', process label is '{1}'", end_identifier, process.Label));
                }
            }
            //----------------------------------------------------------

            return process;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.choices"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitChoices([NotNull] vhdlParser.ChoicesContext context)
        {
            var choices_in = context.choice();
            List<Choice> ch = ParseExtention.ParseList<vhdlParser.ChoiceContext, Choice>(choices_in, VisitChoice);
            return new Choices(ch);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.design_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDesign_unit([NotNull] vhdlParser.Design_unitContext context)
        {
            var context_clause = context.context_clause();
            var library_unit = context.library_unit();

            LibraryUnit res = null;

            if (context_clause != null)
            {
                VhdlElement clause = VisitContext_clause(context_clause);
            }

            if (library_unit != null)
            {
                res = ParseExtention.Parse<vhdlParser.Library_unitContext, LibraryUnit>(library_unit, VisitLibrary_unit);
                return res;
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.factor"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFactor([NotNull] vhdlParser.FactorContext context)
        {
            var primary1_in = context.primary()[0];
            Expression primary1 = ParseExtention.Parse<vhdlParser.PrimaryContext, Expression>(primary1_in, VisitPrimary);
            if (context.DOUBLESTAR() != null)
            {
                var primary2_in = context.primary()[1];
                Expression primary2 = ParseExtention.Parse<vhdlParser.PrimaryContext, Expression>(primary2_in, VisitPrimary);

                return new Pow(primary1, primary2);
            }

            if (context.ABS() != null)
            {
                return new Abs(primary1);
            }

            if (context.NOT() != null)
            {
                return new Not(primary1);
            }

            return primary1;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.index_subtype_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIndex_subtype_definition([NotNull] vhdlParser.Index_subtype_definitionContext context)
        {
            var name_in = context.name();

            VHDL.type.ISubtypeIndication subtype = resolve<VHDL.type.ISubtypeIndication>(name_in.GetText());

            VHDL.type.RangeSubtypeIndication range_si = new VHDL.type.RangeSubtypeIndication(subtype, null);

            return range_si;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_body([NotNull] vhdlParser.Subprogram_bodyContext context)
        {
            var designator_in = context.designator();
            var subprogram_declarative_part_in = context.subprogram_declarative_part();
            var subprogram_kind_in = context.subprogram_kind();
            var subprogram_specification_in = context.subprogram_specification();
            var subprogram_statement_part_in = context.subprogram_statement_part();

            VHDL.declaration.SubprogramDeclaration declaration = ParseExtention.Parse<vhdlParser.Subprogram_specificationContext, VHDL.declaration.SubprogramDeclaration>(subprogram_specification_in, VisitSubprogram_specification);
            VHDL.declaration.SubprogramBody body = null;
            if (declaration is VHDL.declaration.FunctionDeclaration)
            {
                body = new VHDL.declaration.FunctionBody(declaration as VHDL.declaration.FunctionDeclaration);
            }
            else
            {
                body = new VHDL.declaration.ProcedureBody(declaration as VHDL.declaration.ProcedureDeclaration);
            }

            //-------------------------------------------
            //   Before parsing
            //-------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            body.Parent = oldScope;
            currentScope = body;
            //-------------------------------------------

            //Analyse declaration part
            foreach (var declaration_item in subprogram_declarative_part_in.subprogram_declarative_item())
            {
                ISubprogramDeclarativeItem di = ParseExtention.Parse<vhdlParser.Subprogram_declarative_itemContext, ISubprogramDeclarativeItem>(declaration_item, VisitSubprogram_declarative_item);
                body.Declarations.Add(di);
            }

            //Analyse sequential statements
            body.Statements.AddRange(ParseExtention.ParseList<vhdlParser.Sequential_statementContext, VHDL.statement.SequentialStatement>(subprogram_statement_part_in.sequential_statement(), VisitSequential_statement));

            //-------------------------------------------
            //   After parsing
            //-------------------------------------------
            currentScope = oldScope;
            AddAnnotations(body, context);
            //-------------------------------------------

            //-------------------------------------------
            //     Additional checkers
            //-------------------------------------------

            //1. End identier equals to function/procedure name
            if (designator_in != null)
            {
                string end_name = designator_in.GetText();
                if (end_name.VHDLIdentifierEquals(declaration.Identifier) == false)
                {
                    throw new System.ArgumentException(string.Format("End name And name in declaration mismatch. End name is '{0}', name in declaration is '{1}'", end_name, declaration.Identifier));
                }
            }

            // 2. Check that routine type is correct
            if ((subprogram_kind_in != null) && (subprogram_kind_in.FUNCTION() != null))
            {
                if ((declaration is VHDL.declaration.FunctionDeclaration) == false)
                {
                    throw new System.ArgumentException(string.Format("End program body is for function, but declaration for procedure"));
                }
            }

            if ((subprogram_kind_in != null) && (subprogram_kind_in.PROCEDURE() != null))
            {
                if ((declaration is VHDL.declaration.ProcedureDeclaration) == false)
                {
                    throw new System.ArgumentException(string.Format("End program body is for procedure, but declaration for function"));
                }
            }
            //-------------------------------------------
            return body;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.delay_mechanism"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDelay_mechanism([NotNull] vhdlParser.Delay_mechanismContext context)
        {
            if (context.TRANSPORT() != null)
            {
                return DelayMechanism.TRANSPORT;
            }
            else
            {
                if (context.INERTIAL() != null)
                {
                    if (context.REJECT() == null)
                    {
                        return DelayMechanism.INERTIAL;
                    }
                    else
                    {
                        var expression_in = context.expression();
                        Expression exp = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);
                        return DelayMechanism.REJECT_INERTIAL(exp);
                    }
                }
                else
                {
                    return DelayMechanism.DUTY_CYCLE;
                }
            }
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_body([NotNull] vhdlParser.Package_bodyContext context)
        {
            var identifiers_in = context.identifier();
            var identifier_begin_in = ((identifiers_in != null) && (identifiers_in.Length != 0)) ? identifiers_in[0] : null;
            var identifier_end_in = ((identifiers_in != null) && (identifiers_in.Length == 2)) ? identifiers_in[1] : null;

            var package_body_declarative_part_in = context.package_body_declarative_part();

            string identifier_begin = (identifier_begin_in != null) ? identifier_begin_in.GetText() : string.Empty;
            string identifier_end = (identifier_end_in != null) ? identifier_end_in.GetText() : string.Empty;

            PackageDeclaration declaration = resolve<PackageDeclaration>(identifier_begin);

            PackageBody pb = new PackageBody(declaration);
            pb.ContextItems.AddRange(contextItems);

            //--------------------------------------------------------------------------
            //         Before Parsing
            //--------------------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //--------------------------------------------------------------------------

            pb.Parent = oldScope;
            currentScope = pb;

            foreach (var declaration_in in package_body_declarative_part_in.package_body_declarative_item())
            {
                IPackageBodyDeclarativeItem item = ParseExtention.Parse<vhdlParser.Package_body_declarative_itemContext, IPackageBodyDeclarativeItem>(declaration_in, VisitPackage_body_declarative_item);
                pb.Declarations.Add(item);
            }

            if (identifier_begin.VHDLCheckBeginEndIdentifierForEquals(identifier_end) == false)
            {
                throw new System.ArgumentException(string.Format("Package begin & end name mismatch. Identifier is '{0}', end name is '{1}'", identifier_begin, identifier_end));
            }

            //--------------------------------------------------------------------------
            //         After Parsing
            //--------------------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(pb, context);
            //--------------------------------------------------------------------------
            return pb;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.range_constraint"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRange_constraint([NotNull] vhdlParser.Range_constraintContext context)
        {
            var range_in = context.range();

            VHDL.Range range = ParseExtention.Parse<vhdlParser.RangeContext, Range>(range_in, VisitRange);

            VHDL.type.ISubtypeIndication range_from_type = range.From.Type;
            VHDL.type.ISubtypeIndication range_to_type = range.To.Type;

            if ((range_from_type == VHDL.builtin.Standard.INTEGER) && (range_to_type == VHDL.builtin.Standard.INTEGER))
            {
                VHDL.type.IntegerType res = new VHDL.type.IntegerType("unknown", range);
                return res;
            }
            else
            {
                VHDL.type.RealType res = new VHDL.type.RealType("unknown", range);
                return res;
            }

            return VisitChildren(context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_call_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedure_call_statement([NotNull] vhdlParser.Procedure_call_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var procedure_call_in = context.procedure_call();

            ProcedureCall procedureCall = ParseExtention.Parse<vhdlParser.Procedure_callContext, ProcedureCall>(procedure_call_in, VisitProcedure_call);

            string label = (label_colon_in != null) ? (label_colon_in.identifier().GetText()) : string.Empty;
            procedureCall.Label = label;

            return procedureCall;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitExpression([NotNull] vhdlParser.ExpressionContext context)
        {
            var relations_in = context.relation();
            var logical_operators_in = context.logical_operator();

            List<Expression> parsed_relations = ParseExtention.ParseList<vhdlParser.RelationContext, Expression>(relations_in, VisitRelation);

            if (parsed_relations.Count == 0)
            {
                throw new System.NotSupportedException(string.Format("Could not analyse item {0}. Amount of relations is 0", context.ToStringTree()));
            }

            Expression res = parsed_relations[0]; ;

            for (int i = 0; i < logical_operators_in.Length; i++)
            {
                Expression next_expression = parsed_relations[i + 1];
                vhdlParser.Logical_operatorContext op = logical_operators_in[i];

                if (op.AND() != null)
                {
                    Expression new_res = new VHDL.expression.And(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.NAND() != null)
                {
                    Expression new_res = new VHDL.expression.Nand(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.NOR() != null)
                {
                    Expression new_res = new VHDL.expression.Nor(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.OR() != null)
                {
                    Expression new_res = new VHDL.expression.Or(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.XNOR() != null)
                {
                    Expression new_res = new VHDL.expression.Xnor(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.XOR() != null)
                {
                    Expression new_res = new VHDL.expression.Xor(res, next_expression);
                    res = new_res;
                    continue;
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}.", op.ToStringTree()));
            }

            return res;
            //throw new NotSupportedException(String.Format("Could not analyse item {0}.", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.abstract_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAbstract_literal([NotNull] vhdlParser.Abstract_literalContext context)
        {
            var integer_literal = context.INTEGER();
            var real_literal = context.REAL_LITERAL();
            var based_literal = context.BASE_LITERAL();

            if (integer_literal != null)
            {
                DecBasedInteger res = new DecBasedInteger(integer_literal.GetText());
                return res;
            }

            if (real_literal != null)
            {
                RealLiteral real = new RealLiteral(real_literal.GetText());
                return real;
            }

            if (based_literal != null)
            {
                string text = based_literal.GetText();
                switch (text.Split('#')[0])
                {
                    case "2":
                        {
                            BinaryBasedInteger res = new BinaryBasedInteger(text);
                            return res;
                        }
                        break;
                    case "8":
                        {
                            OctalBasedInteger res = new OctalBasedInteger(text);
                            return res;
                        }
                        break;
                    case "16":
                        {
                            HexBasedInteger res = new HexBasedInteger(text);
                            return res;
                        }
                        break;
                    default:
                        throw new System.NotSupportedException(string.Format("Could not analyse item {0}", based_literal.ToStringTree()));
                        break;
                }
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_variable_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_variable_declaration([NotNull] vhdlParser.Interface_variable_declarationContext context)
        {
            bool hasObjectClass = context.VARIABLE() != null;
            bool hasMode = context.signal_mode() != null;

            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            var def_value_in = context.expression();
            Expression def_value = (def_value_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(def_value_in, VisitExpression) : null;

            var identifiers_in = context.identifier_list();
            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);
            VariableGroup res = new VariableGroup();

            VhdlObject.ModeEnum mode = (hasMode) ? (VhdlObject.ModeEnum)(System.Enum.Parse(typeof(VhdlObject.ModeEnum), context.signal_mode().GetText().ToUpper())) : VhdlObject.ModeEnum.IN;

            foreach (var identifier in identifiers_in.identifier())
            {
                Variable v = new Variable(identifier.GetText(), si, def_value);
                if (hasMode)
                {
                    v.Mode = mode;
                }
                Annotations.putAnnotation(v, format);

                res.Elements.Add(v);
            }

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.next_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNext_statement([NotNull] vhdlParser.Next_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var identifier_in = context.identifier();
            var condition_in = context.condition();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string identifier = identifier_in.GetText();
            Expression condition = (condition_in != null) ? ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition) : null;

            LoopStatement loop = resolve<LoopStatement>(identifier);

            NextStatement next = new NextStatement(loop, condition);
            next.Label = label;

            return next;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.scalar_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitScalar_type_definition([NotNull] vhdlParser.Scalar_type_definitionContext context)
        {
            var enumeration_type_definition_in = context.enumeration_type_definition();
            var physical_type_definition_in = context.physical_type_definition();
            var range_constraint_in = context.range_constraint();

            if (enumeration_type_definition_in != null)
                return VisitEnumeration_type_definition(enumeration_type_definition_in);

            if (physical_type_definition_in != null)
                return VisitPhysical_type_definition(physical_type_definition_in);

            if (range_constraint_in != null)
                return VisitRange_constraint(range_constraint_in);

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constant_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstant_declaration([NotNull] vhdlParser.Constant_declarationContext context)
        {
            var identifier_list_in = context.identifier_list();
            var subtype_indication_in = context.subtype_indication();
            var expression_in = context.expression();

            Expression def = (expression_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication_in != null) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication) : null;

            ConstantDeclaration res = new ConstantDeclaration();

            foreach (var identifier in identifier_list_in.identifier())
            {
                string constant_name = identifier.GetText();
                Constant c = new Constant(constant_name, type, def);

                res.Objects.Add(c);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            res.Parent = currentScope;
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_file_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_file_declaration([NotNull] vhdlParser.Interface_file_declarationContext context)
        {
            FileGroup res = new FileGroup();
            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            var identifier_list_in = context.identifier_list();
            foreach (var identifier in identifier_list_in.identifier())
            {
                FileObject obj = new FileObject(identifier.GetText(), si);
                res.Elements.Add(obj);
            }

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.association_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssociation_element([NotNull] vhdlParser.Association_elementContext context)
        {
            var actual_part_in = context.actual_part();
            var formal_part_in = context.formal_part();

            string formal = (formal_part_in != null) ? formal_part_in.identifier().GetText() : string.Empty;
            Expression exp = ParseExtention.Parse<vhdlParser.Actual_partContext, Expression>(actual_part_in, VisitActual_part);

            AssociationElement el = new AssociationElement(formal, exp);
            return el;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.case_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCase_statement([NotNull] vhdlParser.Case_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();
            var case_statement_alternative_in = context.case_statement_alternative();
            var identifier_in = context.identifier();

            string label_begin = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            //1. parse expression
            Expression expression = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);

            CaseStatement case_statement = new CaseStatement(expression);

            //2. parse alternatives
            case_statement.Alternatives.AddRange(ParseExtention.ParseList<vhdlParser.Case_statement_alternativeContext, CaseStatement.Alternative>(case_statement_alternative_in, VisitCase_statement_alternative));

            //3. Check that end identifier is the same as label at the beginning
            case_statement.Label = label_begin;

            if (label_begin.VHDLCheckBeginEndIdentifierForEquals(label_end) == false)
            {
                throw new System.ArgumentException(string.Format("If statement begin & ennd identifier mismatch. Label at the begin is '{0}', albel at the end is '{1}'", label_begin, label_end));
            }

            return case_statement;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.relation"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRelation([NotNull] vhdlParser.RelationContext context)
        {
            var shift_expressions_in = context.shift_expression();
            var relational_operator_in = context.relational_operator();

            Expression parsed_shift_expression_1 = ParseExtention.Parse<vhdlParser.Shift_expressionContext, Expression>(shift_expressions_in[0], VisitShift_expression);

            if (relational_operator_in == null)
            {
                return parsed_shift_expression_1;
            }
            else
            {
                Expression parsed_shift_expression_2 = ParseExtention.Parse<vhdlParser.Shift_expressionContext, Expression>(shift_expressions_in[1], VisitShift_expression);

                if (relational_operator_in.EQ() != null)
                {
                    return new VHDL.expression.Equals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.GE() != null)
                {
                    return new VHDL.expression.GreaterEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.GREATERTHAN() != null)
                {
                    return new VHDL.expression.GreaterThan(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.LE() != null)
                {
                    return new VHDL.expression.LessEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.LOWERTHAN() != null)
                {
                    return new VHDL.expression.LessThan(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.NEQ() != null)
                {
                    return new VHDL.expression.NotEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}", relational_operator_in.ToStringTree()));
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constrained_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstrained_nature_definition([NotNull] vhdlParser.Constrained_nature_definitionContext context) { return VisitChildren(context); }

        private List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> FormConditionalWaveformList([NotNull] vhdlParser.Conditional_waveformsContext conditional_waveforms_in)
        {
            List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> res = new List<ConditionalSignalAssignment.ConditionalWaveformElement>();

            vhdlParser.Conditional_waveformsContext current_conditional_waveforms = conditional_waveforms_in;
            while (current_conditional_waveforms != null)
            {
                var conditional_waveforms_new_in = current_conditional_waveforms.conditional_waveforms();

                VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement CWE = ParseExtention.Parse<vhdlParser.Conditional_waveformsContext, VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement>(current_conditional_waveforms, VisitConditional_waveforms);

                res.Add(CWE);

                current_conditional_waveforms = conditional_waveforms_new_in;
            }
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_signal_assignment"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_signal_assignment([NotNull] vhdlParser.Conditional_signal_assignmentContext context)
        {
            var opts_in = context.opts();
            var target_in = context.target();
            var conditional_waveforms_in = context.conditional_waveforms();

            ISignalAssignmentTarget target = ParseExtention.Parse<vhdlParser.TargetContext, ISignalAssignmentTarget>(target_in, VisitTarget);
            bool is_guarded = opts_in.GUARDED() != null;
            DelayMechanism delay_mechanism = (opts_in.delay_mechanism() != null) ? ParseExtention.Parse<vhdlParser.Delay_mechanismContext, DelayMechanism>(opts_in.delay_mechanism(), VisitDelay_mechanism) : DelayMechanism.DUTY_CYCLE;

            List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> CWEs = FormConditionalWaveformList(conditional_waveforms_in);

            ConditionalSignalAssignment csa = new ConditionalSignalAssignment(target, CWEs);
            csa.DelayMechanism = delay_mechanism;
            csa.Guarded = is_guarded;

            return csa;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.source_quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSource_quantity_declaration([NotNull] vhdlParser.Source_quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.identifier"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIdentifier([NotNull] vhdlParser.IdentifierContext context)
        {
            VhdlElement ve = resolve<VhdlElement>(context.GetText());
            return ve;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.composite_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComposite_type_definition([NotNull] vhdlParser.Composite_type_definitionContext context)
        {
            var array_type_definition_in = context.array_type_definition();
            var record_type_definition_in = context.record_type_definition();

            if (array_type_definition_in != null)
                return VisitArray_type_definition(array_type_definition_in);

            if (record_type_definition_in != null)
                return VisitRecord_type_definition(record_type_definition_in);

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedural_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedural_declarative_item([NotNull] vhdlParser.Procedural_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_declarative_part([NotNull] vhdlParser.Entity_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_case_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_case_statement([NotNull] vhdlParser.Simultaneous_case_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_mode"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_mode([NotNull] vhdlParser.Signal_modeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_configuration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_configuration([NotNull] vhdlParser.Block_configurationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.physical_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPhysical_literal([NotNull] vhdlParser.Physical_literalContext context)
        {
            var abstract_literal = context.abstract_literal();
            var name = context.identifier();
            AbstractLiteral al = ParseExtention.Parse<vhdlParser.Abstract_literalContext, AbstractLiteral>(abstract_literal, VisitAbstract_literal);
            PhysicalLiteral physLiteral = resolve<PhysicalLiteral>(name.GetText());
            PhysicalLiteral res = new PhysicalLiteral(al, name.GetText(), physLiteral.GetPhysicalType());
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.enumeration_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEnumeration_literal([NotNull] vhdlParser.Enumeration_literalContext context)
        {
            var identifier = context.identifier();
            var character_literal = context.CHARACTER_LITERAL();

            if (identifier != null)
            {
                /*
                EnumerationLiteral literal = resolve<EnumerationLiteral>(identifier.GetText());
                if (literal != null)
                    return literal;
                else
                */
                return resolve<VhdlElement>(identifier.GetText());
            }

            if (character_literal != null)
            {
                VHDL.type.EnumerationType.CharacterEnumerationLiteral literal = resolve<VHDL.type.EnumerationType.CharacterEnumerationLiteral>(character_literal.GetText());
                return literal;
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_constant_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_constant_declaration([NotNull] vhdlParser.Interface_constant_declarationContext context)
        {
            bool hasObjectClass = context.CONSTANT() != null;
            bool hasMode = context.IN() != null;

            var def_value_in = context.expression();
            Expression def_value = (def_value_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(def_value_in, VisitExpression) : null;

            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);

            ConstantGroup res = new ConstantGroup();
            foreach (var i in context.identifier_list().identifier())
            {
                Constant c = new Constant(i.GetText(), si, def_value);
                Annotations.putAnnotation(c, format);

                res.Elements.Add(c);
            }

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.name"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitName([NotNull] vhdlParser.NameContext context)
        {
            var name_parts_in = context.name_part();
            bool first_entry = true;

            Name res = null;

            if (context.selected_name() != null)
            {
                VHDL.parser.antlr.TemporaryName tm = ParseExtention.Parse<vhdlParser.Selected_nameContext, VHDL.parser.antlr.TemporaryName>(context.selected_name(), VisitSelected_name);
                res = tm.GetName();
            }
            else
            {
                foreach (var name_part_in in name_parts_in)
                {
                    var selected_name = name_part_in.selected_name();

                    if (first_entry)
                    {
                        res = TemporaryVisitFirstNamePart(name_part_in);
                        first_entry = false;
                    }
                    else
                    {
                        res = TemporaryVisitNamePart(name_part_in, res);
                    }
                }
            }

            return res;
        }

        private Name TemporaryVisitFirstNamePart(vhdlParser.Name_partContext name_part_in)
        {
            if (name_part_in.name_function_call_or_indexed_part() != null)
                return TemporaryVisitFirstIndiciesNamePart(name_part_in);

            List<VHDL.parser.antlr.Part> parts = TemporaryNameSelectedPart(name_part_in);
            VHDL.parser.antlr.TemporaryName tm = ParseExtention.Parse<vhdlParser.Selected_nameContext, VHDL.parser.antlr.TemporaryName>(name_part_in.selected_name(), VisitSelected_name);
            Name res = tm.GetName();

            if (name_part_in.name_slice_part() != null)
                return TemporaryVisitSliceNamePart(name_part_in, res);
            if (name_part_in.name_attribute_part() != null)
                return TemporaryVisitAttributeNamePart(name_part_in, res);


            return res;
        }

        private Name TemporaryVisitNamePart(vhdlParser.Name_partContext name_part_in, Name previousName)
        {
            List<VHDL.parser.antlr.Part> parts = TemporaryNameSelectedPart(name_part_in);

            Name res = previousName;
            for (int i = 1; i < parts.Count; i++)
            {
                VHDL.parser.antlr.Part p = parts[i];
                res = new RecordElement(res, (p as VHDL.parser.antlr.Part.SelectedPart).Suffix);
            }

            if (name_part_in.name_function_call_or_indexed_part() != null)
                return TemporaryVisitIndiciesNamePart(name_part_in, res);
            if (name_part_in.name_slice_part() != null)
                return TemporaryVisitSliceNamePart(name_part_in, res);
            if (name_part_in.name_attribute_part() != null)
                return TemporaryVisitAttributeNamePart(name_part_in, res);

            return res;
        }

        private Name TemporaryVisitAttributeNamePart(vhdlParser.Name_partContext name_part_in, Name previousName)
        {
            List<VHDL.parser.antlr.Part> parts = TemporaryNameSelectedPart(name_part_in);

            Name res = previousName;
            foreach (VHDL.parser.antlr.Part p in parts)
            {
                res = new RecordElement(res, (p as VHDL.parser.antlr.Part.SelectedPart).Suffix);
            }

            string attributeName = name_part_in.name_attribute_part().attribute_designator().GetText();
            Attribute standard_attribute = Attribute.GetStandardAttribute(attributeName);
            Attribute attribute = (standard_attribute == null) ? resolve<Attribute>(attributeName) : standard_attribute;

            List<Expression> expressions = ParseExtention.ParseList<vhdlParser.ExpressionContext, Expression>(name_part_in.name_attribute_part().expression(), VisitExpression);
            res = new AttributeExpression(res, attribute, expressions);
            VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = res;
            return res;
        }


        private Name TemporaryVisitFirstIndiciesNamePart(vhdlParser.Name_partContext name_part_in)
        {
            List<VHDL.parser.antlr.Part> parts = TemporaryNameSelectedPart(name_part_in);
            VHDL.parser.antlr.TemporaryName tm = new VHDL.parser.antlr.TemporaryName(parts, this, name_part_in);
            FunctionDeclaration FD = tm.GetFunction();
            if (FD != null)
            {
                List<AssociationElement> arguments = ParseExtention.ParseList<vhdlParser.Association_elementContext, AssociationElement>(name_part_in.name_function_call_or_indexed_part().actual_parameter_part().association_list().association_element(), VisitAssociation_element);

                //TODO::IMPLEMENT FUNCTION CALL RESO:UTION FUNCTION IN THE FUTURE!!!!!!!!

                //FunctionCall functionCall = tm.GetFunctionCall(arguments, VHDL.parser.antlr.TemporaryName.CurrentAssignTarget.Type);
                FunctionCall functionCall = new FunctionCall(FD, arguments);
                VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = functionCall;
                Name res = functionCall;
                return res;
            }
            else
            {
                List<Expression> expressions = TemporaryIndiciesNamePartExpressions(name_part_in.name_function_call_or_indexed_part().actual_parameter_part());
                Name base_name = tm.GetName();
                ArrayElement ae = new ArrayElement(base_name, expressions);
                VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = ae;
                Name res = ae;
                return res;
            }
        }

        private Name TemporaryVisitIndiciesNamePart(vhdlParser.Name_partContext name_part_in, Name previousName)
        {
            Name res = previousName;
            List<Expression> expressions = TemporaryIndiciesNamePartExpressions(name_part_in.name_function_call_or_indexed_part().actual_parameter_part());
            res = new ArrayElement(res, expressions);
            VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = res;
            return res;
        }

        private Name TemporaryVisitSliceNamePart(vhdlParser.Name_partContext name_part_in, Name previousName)
        {
            Name res = previousName;
            List<DiscreteRange> ranges = new List<DiscreteRange>();
            ranges.AddRange(ParseExtention.ParseList<vhdlParser.Explicit_rangeContext, Range>(name_part_in.name_slice_part().explicit_range(), VisitExplicit_range));
            res = new Slice(res, ranges);
            VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = res;
            return res;
        }

        private List<Expression> TemporaryIndiciesNamePartExpressions(vhdlParser.Actual_parameter_partContext actual_parameter_part_in)
        {
            List<Expression> expressions = new List<Expression>();
            foreach (var ae_in in actual_parameter_part_in.association_list().association_element())
            {
                Expression exp = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(ae_in.actual_part().actual_designator().expression(), VisitExpression);
                expressions.Add(exp);
            }

            return expressions;
        }

        private List<VHDL.parser.antlr.Part> TemporaryNameSelectedPart(vhdlParser.Name_partContext name_part_in)
        {
            var selected_name_in = name_part_in.selected_name();
            List<VHDL.parser.antlr.Part> parts = new List<VHDL.parser.antlr.Part>();
            parts.Add(VHDL.parser.antlr.Part.CreateSelected(selected_name_in.identifier().GetText()));
            foreach (var suffix_in in selected_name_in.suffix())
                parts.Add(VHDL.parser.antlr.Part.CreateSelected(suffix_in.GetText()));

            return parts;
        }



        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.selected_name"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override VhdlElement VisitSelected_name(vhdlParser.Selected_nameContext context)
        {
            var suffixes_in = context.suffix();
            var identifier = context.identifier();

            List<VHDL.parser.antlr.Part> parts = new List<VHDL.parser.antlr.Part>();
            parts.Add(VHDL.parser.antlr.Part.CreateSelected(identifier.GetText()));
            foreach (var suffix_in in suffixes_in)
                parts.Add(VHDL.parser.antlr.Part.CreateSelected(suffix_in.GetText()));

            return new VHDL.parser.antlr.TemporaryName(parts, this, context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_declaration([NotNull] vhdlParser.Package_declarationContext context)
        {
            var identifiers_in = context.identifier();
            var identifier_begin_in = ((identifiers_in != null) && (identifiers_in.Length != 0)) ? identifiers_in[0] : null;
            var identifier_end_in = ((identifiers_in != null) && (identifiers_in.Length == 2)) ? identifiers_in[1] : null;

            var package_declarative_part_in = context.package_declarative_part();

            string identifier_begin = (identifier_begin_in != null) ? identifier_begin_in.GetText() : string.Empty;
            string identifier_end = (identifier_end_in != null) ? identifier_end_in.GetText() : string.Empty;

            PackageDeclaration pd = new PackageDeclaration(identifier_begin);
            pd.ContextItems.AddRange(contextItems);

            //--------------------------------------------------------------------------
            //         Before Parsing
            //--------------------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //--------------------------------------------------------------------------

            pd.Parent = oldScope;
            currentScope = pd;

            foreach (var declaration_in in package_declarative_part_in.package_declarative_item())
            {
                IPackageDeclarativeItem item = ParseExtention.Parse<vhdlParser.Package_declarative_itemContext, IPackageDeclarativeItem>(declaration_in, VisitPackage_declarative_item);
                pd.Declarations.Add(item);
            }

            if (identifier_begin.VHDLCheckBeginEndIdentifierForEquals(identifier_end) == false)
            {
                throw new System.ArgumentException(string.Format("Package begin & end name mismatch. Identifier is '{0}', end name is '{1}'", identifier_begin, identifier_end));
            }

            //--------------------------------------------------------------------------
            //         After Parsing
            //--------------------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(pd, context);
            //--------------------------------------------------------------------------
            return pd;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_class_entry"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_class_entry([NotNull] vhdlParser.Entity_class_entryContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.group_constituent"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGroup_constituent([NotNull] vhdlParser.Group_constituentContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.unconstrained_array_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitUnconstrained_array_definition([NotNull] vhdlParser.Unconstrained_array_definitionContext context)
        {
            var index_subtype_definitions = context.index_subtype_definition();
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            VHDL.type.UnconstrainedArray res = new VHDL.type.UnconstrainedArray("unknown", si);

            List<VHDL.type.ISubtypeIndication> ranges = new List<VHDL.type.ISubtypeIndication>();
            foreach (var index_subtype_definition in index_subtype_definitions)
            {
                VHDL.type.ISubtypeIndication range = ParseExtention.Parse<vhdlParser.Index_subtype_definitionContext, VHDL.type.ISubtypeIndication>(index_subtype_definition, VisitIndex_subtype_definition);
                ranges.Add(range);
            }

            res.IndexSubtypes.AddRange(ranges);

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_header"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_header([NotNull] vhdlParser.Block_headerContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNature_definition([NotNull] vhdlParser.Nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_kind"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_kind([NotNull] vhdlParser.Signal_kindContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_logical_name"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_logical_name([NotNull] vhdlParser.File_logical_nameContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.quantity_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQuantity_specification([NotNull] vhdlParser.Quantity_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.assertion"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssertion([NotNull] vhdlParser.AssertionContext context)
        {
            var condition_in = context.condition();
            var has_report = context.REPORT() != null;
            var has_severity = context.SEVERITY() != null;
            var expression_in = context.expression();
            int expression_idx_for_parse = 0;

            //1. parse condition
            Expression condition = ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition);

            //2. Parse report epression
            Expression report_expression = (has_report) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in[expression_idx_for_parse], VisitExpression) : null;
            if (has_report) expression_idx_for_parse++;

            //3. Parse severity
            Expression severity_expression = (has_severity) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in[expression_idx_for_parse], VisitExpression) : null;
            if (has_severity) expression_idx_for_parse++;

            AssertionStatement assertionStatement = new AssertionStatement(condition, report_expression, severity_expression);

            return assertionStatement;
        }        

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.array_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArray_type_definition([NotNull] vhdlParser.Array_type_definitionContext context)
        {
            var constrained_array_definition_in = context.constrained_array_definition();
            var unconstrained_array_definition_in = context.unconstrained_array_definition();

            if (constrained_array_definition_in != null)
                return VisitConstrained_array_definition(constrained_array_definition_in);

            if (unconstrained_array_definition_in != null)
                return VisitUnconstrained_array_definition(unconstrained_array_definition_in);

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }
        

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_declaration([NotNull] vhdlParser.Entity_declarationContext context)
        {
            IDeclarativeRegion oldScope = this.currentScope;
            //--------------------------------------
            VHDL_ANTLR4.vhdlParser.IdentifierContext[] identifiers = context.identifier();

            if (identifiers.Length == 2)
            {
                string begin_identifier = identifiers[0].GetText();
                string end_identifier = identifiers[1].GetText();
                if (begin_identifier.VHDLCheckBeginEndIdentifierForEquals(end_identifier) == false)
                    throw new System.Exception(string.Format("Self check failure. Entity declaration identifier is {0}, End identifier is {1}.", begin_identifier, end_identifier));
            }

            VHDL_ANTLR4.vhdlParser.IdentifierContext identifier = identifiers[0];

            Entity res = new Entity(identifier.GetText());
            res.ContextItems.AddRange(contextItems);
            res.Parent = oldScope;
            currentScope = res;

            var entity_header = context.entity_header();
            var entity_declarative_part = context.entity_declarative_part();
            var entity_statement_part = context.entity_statement_part();

            //1. Visit all declaration parts (generics and ports)
            var port_clause = entity_header.port_clause();
            if (port_clause != null)
            {
                foreach (var port in port_clause.port_list().interface_port_list().interface_port_declaration())
                {
                    res.Port.Add(ParseExtention.Parse<vhdlParser.Interface_port_declarationContext, IVhdlObjectProvider>(port, VisitInterface_port_declaration));
                }
            }

            var generic_clause = entity_header.generic_clause();
            if (generic_clause != null)
            {
                foreach (var generic in generic_clause.generic_list().interface_constant_declaration())
                {
                    res.Generic.Add(ParseExtention.Parse<vhdlParser.Interface_constant_declarationContext, IVhdlObjectProvider>(generic, VisitInterface_constant_declaration));
                }
            }

            foreach (var declaration in context.entity_declarative_part().entity_declarative_item())
            {
                IEntityDeclarativeItem decl = ParseExtention.Parse<vhdlParser.Entity_declarative_itemContext, IEntityDeclarativeItem>(declaration, VisitEntity_declarative_item);
                res.Declarations.Add(decl);
            }

            if (entity_statement_part != null)
            {
                foreach (var statement in entity_statement_part.entity_statement())
                {
                    EntityStatement st = ParseExtention.Parse<vhdlParser.Entity_statementContext, EntityStatement>(statement, VisitEntity_statement);
                    res.Statements.Add(st);
                }
            }

            //--------------------------------------
            currentScope = oldScope;
            AddAnnotations(res, context);
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.aggregate"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAggregate([NotNull] vhdlParser.AggregateContext context)
        {
            bool has_choises = context.element_association()[0].ARROW() != null;
            Aggregate res = new Aggregate();

            foreach (var aggregate_item in context.element_association())
            {
                if ((has_choises == true) && (aggregate_item.ARROW() == null))
                {
                    throw new System.Exception(string.Format("Expression {0} should hawe all choises, or not use them in all cases", context.ToStringTree()));
                }
                var expression_in = aggregate_item.expression();
                Expression exp = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);

                if (has_choises)
                {
                    var choices_in = aggregate_item.choices();
                    List<Choice> ch = new List<Choice>();
                    foreach (var curr_choice in choices_in.choice())
                    {
                        Choice c = ParseExtention.Parse<vhdlParser.ChoiceContext, Choice>(curr_choice, VisitChoice);
                        ch.Add(c);
                    }
                    res.CreateAssociation(exp, ch);
                }
                else
                {
                    res.CreateAssociation(exp);
                }
            }
            return res;
        }
                

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.case_statement_alternative"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCase_statement_alternative([NotNull] vhdlParser.Case_statement_alternativeContext context)
        {
            var choices_in = context.choices();
            var sequence_of_statements_in = context.sequence_of_statements();

            List<Choice> choices = ParseExtention.ParseList<vhdlParser.ChoiceContext, Choice>(choices_in.choice(), VisitChoice);
            List<VHDL.statement.SequentialStatement> statements = ParseExtention.ParseList<vhdlParser.Sequential_statementContext, VHDL.statement.SequentialStatement>(sequence_of_statements_in.sequential_statement(), VisitSequential_statement);
            VHDL.statement.CaseStatement.Alternative res = new VHDL.statement.CaseStatement.Alternative(choices);
            res.Statements.AddRange(statements);

            return res;
        }
        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_element([NotNull] vhdlParser.Interface_elementContext context)
        {
            var interface_declaration = context.interface_declaration();
            return VisitInterface_declaration(interface_declaration);
        }
        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_declarative_item([NotNull] vhdlParser.Block_declarative_itemContext context)
        {
            var alias_declaration_in = context.alias_declaration();
            var attribute_declaration_in = context.attribute_declaration();
            var attribute_specification_in = context.attribute_specification();
            var component_declaration_in = context.component_declaration();
            var configuration_specification_in = context.configuration_specification();
            var constant_declaration_in = context.constant_declaration();
            var disconnection_specification_in = context.disconnection_specification();
            var file_declaration_in = context.file_declaration();
            var group_declaration_in = context.group_declaration();
            var group_template_declaration_in = context.group_template_declaration();
            var nature_declaration_in = context.nature_declaration();
            var quantity_declaration_in = context.quantity_declaration();
            var signal_declaration_in = context.signal_declaration();
            var step_limit_specification_in = context.step_limit_specification();
            var subnature_declaration_in = context.subnature_declaration();
            var subprogram_body_in = context.subprogram_body();
            var subprogram_declaration_in = context.subprogram_declaration();
            var subtype_declaration_in = context.subtype_declaration();
            var terminal_declaration_in = context.terminal_declaration();
            var type_declaration_in = context.type_declaration();
            var use_clause_in = context.use_clause();
            var variable_declaration_in = context.variable_declaration();

            if (alias_declaration_in != null) return VisitAlias_declaration(alias_declaration_in);
            if (attribute_declaration_in != null) return VisitAttribute_declaration(attribute_declaration_in);
            if (attribute_specification_in != null) return VisitAttribute_specification(attribute_specification_in);
            if (component_declaration_in != null) return VisitComponent_declaration(component_declaration_in);
            if (configuration_specification_in != null) return VisitConfiguration_specification(configuration_specification_in);
            if (constant_declaration_in != null) return VisitConstant_declaration(constant_declaration_in);
            if (disconnection_specification_in != null) return VisitDisconnection_specification(disconnection_specification_in);
            if (file_declaration_in != null) return VisitFile_declaration(file_declaration_in);
            if (group_declaration_in != null) return VisitGroup_declaration(group_declaration_in);
            if (group_template_declaration_in != null) return VisitGroup_template_declaration(group_template_declaration_in);
            if (nature_declaration_in != null) return VisitNature_declaration(nature_declaration_in);
            if (quantity_declaration_in != null) return VisitQuantity_declaration(quantity_declaration_in);
            if (signal_declaration_in != null) return VisitSignal_declaration(signal_declaration_in);
            if (step_limit_specification_in != null) return VisitStep_limit_specification(step_limit_specification_in);
            if (subnature_declaration_in != null) return VisitSubnature_declaration(subnature_declaration_in);
            if (subprogram_body_in != null) return VisitSubprogram_body(subprogram_body_in);
            if (subprogram_declaration_in != null) return VisitSubprogram_declaration(subprogram_declaration_in);
            if (subtype_declaration_in != null) return VisitSubtype_declaration(subtype_declaration_in);
            if (terminal_declaration_in != null) return VisitTerminal_declaration(terminal_declaration_in);
            if (type_declaration_in != null) return VisitType_declaration(type_declaration_in);
            if (use_clause_in != null) return VisitUse_clause(use_clause_in);
            if (variable_declaration_in != null) return VisitVariable_declaration(variable_declaration_in);

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_assignment_statement([NotNull] vhdlParser.Signal_assignment_statementContext context)
        {
            var delay_mechanism_in = context.delay_mechanism();
            var label_colon_in = context.label_colon();
            var target_in = context.target();
            var waveform_in = context.waveform();

            ISignalAssignmentTarget target = ParseExtention.Parse<vhdlParser.TargetContext, ISignalAssignmentTarget>(target_in, VisitTarget);

            List<WaveformElement> waveformelements = new List<WaveformElement>();
            foreach (var w in waveform_in.waveform_element())
            {
                WaveformElement el = ParseExtention.Parse<vhdlParser.Waveform_elementContext, WaveformElement>(w, VisitWaveform_element);
                waveformelements.Add(el);
            }


            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            DelayMechanism delay = (delay_mechanism_in != null) ? ParseExtention.Parse<vhdlParser.Delay_mechanismContext, DelayMechanism>(delay_mechanism_in, VisitDelay_mechanism) : DelayMechanism.DUTY_CYCLE;

            SignalAssignment sa = new SignalAssignment(target, waveformelements);
            sa.Label = label;
            sa.DelayMechanism = delay;

            return sa;
        }
        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.primary_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPrimary_unit([NotNull] vhdlParser.Primary_unitContext context)
        {
            var configuration_declaration = context.configuration_declaration();
            var entity_declaration = context.entity_declaration();
            var package_declaration = context.package_declaration();

            if (configuration_declaration != null)
            {
                return VisitConfiguration_declaration(configuration_declaration);
            }

            if (entity_declaration != null)
            {
                return VisitEntity_declaration(entity_declaration);
            }

            if (package_declaration != null)
            {
                return VisitPackage_declaration(package_declaration);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }
        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.shift_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitShift_expression([NotNull] vhdlParser.Shift_expressionContext context)
        {
            var shift_operator_in = context.shift_operator();
            var simple_expressions_in = context.simple_expression();

            Expression parsed_simple_expression_1 = ParseExtention.Parse<vhdlParser.Simple_expressionContext, Expression>(simple_expressions_in[0], VisitSimple_expression);

            if (shift_operator_in == null)
            {
                return parsed_simple_expression_1;
            }
            else
            {
                Expression parsed_simple_expression_2 = ParseExtention.Parse<vhdlParser.Simple_expressionContext, Expression>(simple_expressions_in[1], VisitSimple_expression);

                if (shift_operator_in.ROL() != null)
                {
                    return new VHDL.expression.Rol(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.ROR() != null)
                {
                    return new VHDL.expression.Ror(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SLA() != null)
                {
                    return new VHDL.expression.Sla(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SLL() != null)
                {
                    return new VHDL.expression.Sll(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SRA() != null)
                {
                    return new VHDL.expression.Sra(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SRL() != null)
                {
                    return new VHDL.expression.Srl(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}", shift_operator_in.ToStringTree()));
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.iteration_scheme"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIteration_scheme([NotNull] vhdlParser.Iteration_schemeContext context)
        {
            bool is_while_iteration_scheme = context.WHILE() != null;
            bool is_for_iteration_scheme = context.FOR() != null;

            if (is_while_iteration_scheme)
            {
                var condition_in = context.condition();
                Expression condition = ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition);
                WhileStatement whileStatement = new WhileStatement(condition);
                return whileStatement;
            }

            if (is_for_iteration_scheme)
            {
                var identifier_in = context.parameter_specification().identifier();
                var discrete_range_in = context.parameter_specification().discrete_range();

                string identifier = identifier_in.GetText();
                DiscreteRange range = ParseExtention.Parse<vhdlParser.Discrete_rangeContext, DiscreteRange>(discrete_range_in, VisitDiscrete_range);

                ForStatement forStatement = new ForStatement(identifier, range);
                return forStatement;
            }

            return new LoopStatement();
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.discrete_range"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDiscrete_range([NotNull] vhdlParser.Discrete_rangeContext context)
        {
            var range = context.range();
            var subtype_indication_in = context.subtype_indication();

            if (range != null)
            {
                return VisitRange(range);
            }

            if (subtype_indication_in != null)
            {
                VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
                SubtypeDiscreteRange res = new SubtypeDiscreteRange(si);
                return res;
            }
            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subtype_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubtype_declaration([NotNull] vhdlParser.Subtype_declarationContext context)
        {
            var identifier_in = context.identifier();
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            string identifier = identifier_in.GetText();

            VHDL.declaration.Subtype subtype = new VHDL.declaration.Subtype(identifier, si);

            return subtype;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_specification([NotNull] vhdlParser.Subprogram_specificationContext context)
        {
            var function_specification_in = context.function_specification();
            var procedure_specification_in = context.procedure_specification();

            if (function_specification_in != null)
                return VisitFunction_specification(function_specification_in);

            if (procedure_specification_in != null)
                return VisitProcedure_specification(procedure_specification_in);

            return VisitChildren(context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.function_specification"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override VhdlElement VisitFunction_specification([NotNull] vhdlParser.Function_specificationContext context)
        {
            bool is_impure = context.IMPURE() != null;
            bool is_pure = context.PURE() != null;

            if ((is_impure == true) && (is_pure == true))
            {
                throw new System.NotSupportedException(string.Format("Could not analyse item {0}. IMPURE AND PURE tokens are set in one statement.", context.ToStringTree()));
            }

            var designator_in = context.designator();
            var formal_parameter_list_in = context.formal_parameter_list();
            var return_type_in = context.subtype_indication();

            string name = designator_in.GetText();

            List<IVhdlObjectProvider> parameters = new List<IVhdlObjectProvider>();
            if ((formal_parameter_list_in != null) && (formal_parameter_list_in.interface_list() != null))
            {
                foreach (var p in formal_parameter_list_in.interface_list().interface_element())
                {
                    IVhdlObjectProvider o = ParseExtention.Parse<vhdlParser.Interface_elementContext, IVhdlObjectProvider>(p, VisitInterface_element);
                    parameters.Add(o);
                }
            }

            VHDL.type.ISubtypeIndication returnType = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(return_type_in, VisitSubtype_indication);

            VHDL.declaration.FunctionDeclaration fd = new FunctionDeclaration(name, returnType, parameters);
            fd.Impure = is_impure;
            return fd;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_specification"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override VhdlElement VisitProcedure_specification([NotNull] vhdlParser.Procedure_specificationContext context)
        {
            var designator_in = context.designator();
            var formal_parameter_list_in = context.formal_parameter_list();

            string name = designator_in.GetText();

            List<IVhdlObjectProvider> parameters = new List<IVhdlObjectProvider>();
            if ((formal_parameter_list_in != null) && (formal_parameter_list_in.interface_list() != null))
            {
                foreach (var p in formal_parameter_list_in.interface_list().interface_element())
                {
                    IVhdlObjectProvider o = ParseExtention.Parse<vhdlParser.Interface_elementContext, IVhdlObjectProvider>(p, VisitInterface_element);
                    parameters.Add(o);
                }
            }

            VHDL.declaration.ProcedureDeclaration pd = new ProcedureDeclaration(name, parameters);
            return pd;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.range"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRange([NotNull] vhdlParser.RangeContext context)
        {
            RangeProvider res = null;

            var name_in = context.name();
            if (name_in != null)
            {
                Name name = ParseExtention.Parse<vhdlParser.NameContext, Name>(name_in, VisitName);
                if (name is TypedName)
                {
                    res = new RangeAttributeName(name, RangeAttributeName.RangeAttributeNameType.RANGE);
                }
                else
                {
                    if (name is AttributeExpression)
                    {
                        AttributeExpression ae = name as AttributeExpression;
                        if (ae.Attribute.Identifier.VHDLIdentifierEquals("RANGE"))
                        {
                            res = new RangeAttributeName(name, RangeAttributeName.RangeAttributeNameType.RANGE);
                            return res;
                        }
                        if (ae.Attribute.Identifier.VHDLIdentifierEquals("REVERSE_RANGE"))
                        {
                            res = new RangeAttributeName(name, RangeAttributeName.RangeAttributeNameType.REVERSE_RANGE);
                            return res;
                        }
                    }
                    res = resolve<RangeProvider>(name_in.GetText());
                }
                return res;
            }
            else
            {
                return ParseExtention.Parse<vhdlParser.Explicit_rangeContext, Range>(context.explicit_range(), VisitExplicit_range);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        public override VhdlElement VisitExplicit_range(vhdlParser.Explicit_rangeContext context)
        {
            var simple_expressions_in = context.simple_expression();
            var direction_in = context.direction();
            if ((simple_expressions_in.Length == 2) && (direction_in != null))
            {
                var expression_left_in = simple_expressions_in[0];
                var expression_right_in = simple_expressions_in[1];

                Range.RangeDirection direction = (direction_in.GetText().VHDLIdentifierEquals("To")) ? Range.RangeDirection.TO : Range.RangeDirection.DOWNTO;
                Expression expression_left = ParseExtention.Parse<vhdlParser.Simple_expressionContext, Expression>(expression_left_in, VisitSimple_expression);
                Expression expression_right = ParseExtention.Parse<vhdlParser.Simple_expressionContext, Expression>(expression_right_in, VisitSimple_expression);

                Range res = new Range(expression_left, direction, expression_right);
                return res;
            }
            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.variable_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitVariable_assignment_statement([NotNull] vhdlParser.Variable_assignment_statementContext context)
        {
            var target_in = context.target();
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            Expression expresion = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);
            IVariableAssignmentTarget target = ParseExtention.Parse<vhdlParser.TargetContext, IVariableAssignmentTarget>(target_in, VisitTarget);
            VHDL.parser.antlr.TemporaryName.CurrentAssignTarget = target as Name;

            VariableAssignment va = new VariableAssignment(target, expresion);
            va.Label = label;

            return va;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.if_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIf_statement([NotNull] vhdlParser.If_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var conditions_in = context.condition();
            var sequence_of_statements_in = context.sequence_of_statements();
            var identifier_in = context.identifier();

            int condition_counter = 0;
            int sequence_of_statements_counter = 0;

            Expression condition = ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(conditions_in[condition_counter], VisitCondition);
            condition_counter++;
            string label_begin = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            VHDL.statement.IfStatement if_statement = new VHDL.statement.IfStatement(condition);

            //1. parse statements in if block
            if_statement.Statements.AddRange(ParseExtention.ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement));
            sequence_of_statements_counter++;

            //2. parse elseif statements
            int end_index_of_elseif_statements = (context.ELSE() != null) ? (sequence_of_statements_in.Length - 2) : (sequence_of_statements_in.Length - 1);
            while (sequence_of_statements_counter <= end_index_of_elseif_statements)
            {
                Expression elseif_condition = ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(conditions_in[condition_counter], VisitCondition);
                condition_counter++;
                List<SequentialStatement> elseif_statements = ParseExtention.ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement);
                sequence_of_statements_counter++;
                VHDL.statement.IfStatement.ElsifPart elseif = new VHDL.statement.IfStatement.ElsifPart(elseif_condition, elseif_statements);
                if_statement.ElsifParts.Add(elseif);
            }

            //3. parse else statements
            if (context.ELSE() != null)
            {
                List<SequentialStatement> else_statements = ParseExtention.ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement);
                if_statement.ElseStatements.AddRange(else_statements);
            }

            //4. Check that end identifier is the same as label at the beginning
            if_statement.Label = label_begin;

            if (label_begin.VHDLCheckBeginEndIdentifierForEquals(label_end) == false)
            {
                throw new System.ArgumentException(string.Format("If statement begin & ennd identifier mismatch. Label at the begin is '{0}', albel at the end is '{1}'", label_begin, label_end));
            }

            return if_statement;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.physical_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPhysical_type_definition([NotNull] vhdlParser.Physical_type_definitionContext context)
        {
            var base_unit_declaration_in = context.base_unit_declaration();
            var range_constraint_in = context.range_constraint();
            var secondary_unit_declarations_in = context.secondary_unit_declaration();

            string base_unit_name = base_unit_declaration_in.identifier().GetText();

            VHDL.Range range = ParseExtention.Parse<vhdlParser.RangeContext, VHDL.Range>(range_constraint_in.range(), VisitRange);

            VHDL.type.PhysicalType pt = new VHDL.type.PhysicalType("unknown", range, base_unit_name);
            pt.createUnit(base_unit_name);

            foreach (var unit in secondary_unit_declarations_in)
            {
                string identifier = unit.identifier().GetText();
                var physical_literal_in = unit.physical_literal();

                var abstract_literal_in = physical_literal_in.abstract_literal();
                var base_identifier_in = physical_literal_in.identifier();

                string base_identifier = base_identifier_in.GetText();

                AbstractLiteral al = ParseExtention.Parse<vhdlParser.Abstract_literalContext, AbstractLiteral>(abstract_literal_in, VisitAbstract_literal);

                pt.createUnit(identifier, al, base_identifier);
            }

            return pt;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.variable_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitVariable_declaration([NotNull] vhdlParser.Variable_declarationContext context)
        {
            var identifier_list = context.identifier_list();
            var subtype_indication = context.subtype_indication();
            var expression = context.expression();
            bool is_shared = context.SHARED() != null;

            Expression def = (expression != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression, VisitExpression) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication != null) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication, VisitSubtype_indication) : null;

            VariableDeclaration res = new VariableDeclaration();

            foreach (var identifier in identifier_list.identifier())
            {
                string variable_name = identifier.GetText();
                Variable v = new Variable(variable_name, type, def);

                v.Shared = is_shared;

                res.Objects.Add(v);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            res.Parent = currentScope;
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_declaration([NotNull] vhdlParser.Signal_declarationContext context)
        {
            var identifier_list = context.identifier_list();
            var subtype_indication = context.subtype_indication();
            var expression = context.expression();
            var kind_in = context.signal_kind();
            Signal.KindEnum kind = (kind_in != null) ? (Signal.KindEnum)System.Enum.Parse(typeof(Signal.KindEnum), kind_in.GetText().ToUpper()) : Signal.KindEnum.DEFAULT;

            Expression def = (expression != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression, VisitExpression) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication != null) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication, VisitSubtype_indication) : null;

            SignalDeclaration res = new SignalDeclaration();

            foreach (var identifier in identifier_list.identifier())
            {
                string signal_name = identifier.GetText();
                Signal s = new Signal(signal_name, type, def);

                s.Kind = kind;

                res.Objects.Add(s);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            res.Parent = currentScope;
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simple_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimple_expression([NotNull] vhdlParser.Simple_expressionContext context)
        {
            var plus_in = context.PLUS();
            var minus_in = context.MINUS();

            var terms_in = context.term();
            var adding_operators_in = context.adding_operator();

            List<Expression> parsed_terms = new List<Expression>();
            foreach (var t in terms_in)
            {
                Expression term = ParseExtention.Parse<vhdlParser.TermContext, Expression>(t, VisitTerm);
                parsed_terms.Add(term);
            }

            if (parsed_terms.Count == 0)
            {
                throw new System.NotSupportedException(string.Format("Could not analyse item {0}. Amount of terms is 0.", context.ToStringTree()));
            }

            Expression res = parsed_terms[0];

            if (plus_in != null)
            {
                Expression new_exp = new VHDL.expression.Plus(res);
                res = new_exp;
            }

            if (minus_in != null)
            {
                Expression new_exp = new VHDL.expression.Minus(res);
                res = new_exp;
            }

            for (int i = 0; i < adding_operators_in.Length; i++)
            {
                var op = adding_operators_in[i];
                Expression curr_expression = parsed_terms[i + 1];

                if (op.MINUS() != null)
                {
                    Expression new_res = new VHDL.expression.Subtract(res, curr_expression);
                    res = new_res;
                    continue;
                }

                if (op.PLUS() != null)
                {
                    Expression new_res = new VHDL.expression.Add(res, curr_expression);
                    res = new_res;
                    continue;
                }

                if (op.AMPERSAND() != null)
                {
                    Expression new_res = new VHDL.expression.Concatenate(res, curr_expression);
                    res = new_res;
                    continue;
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}", op.ToStringTree()));
            }

            return res;
            //throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.use_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitUse_clause([NotNull] vhdlParser.Use_clauseContext context)
        {
            var names_in = context.selected_name();


            List<string> names = new List<string>();
            foreach (var name_in in names_in)
            {
                names.Add(name_in.GetText());
            }

            UseClause use = new UseClause(names);
            AddAnnotations(use, context);
            CheckUseClause(context, use);
            return use;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.return_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitReturn_statement([NotNull] vhdlParser.Return_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            Expression expression = (expression_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression) : null;

            ReturnStatement returnStatement = new ReturnStatement(expression);
            returnStatement.Label = label;

            return returnStatement;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.enumeration_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEnumeration_type_definition([NotNull] vhdlParser.Enumeration_type_definitionContext context)
        {
            var enumeration_literals_in = context.enumeration_literal();

            VHDL.type.EnumerationType enumeration_type = new VHDL.type.EnumerationType("unknown");

            foreach (var l in enumeration_literals_in)
            {
                if (l.CHARACTER_LITERAL() != null)
                {
                    char literal = l.CHARACTER_LITERAL().GetText()[1];
                    enumeration_type.createLiteral(literal);
                    continue;
                }

                if (l.identifier() != null)
                {
                    string literal = l.identifier().GetText();
                    enumeration_type.createLiteral(literal);
                    continue;
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}", l.ToStringTree()));
            }

            return enumeration_type;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constrained_array_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstrained_array_definition([NotNull] vhdlParser.Constrained_array_definitionContext context)
        {
            var index_constraints_in = context.index_constraint();
            var subtype_indication_in = context.subtype_indication();

            List<VHDL.DiscreteRange> ranges = new List<DiscreteRange>();
            foreach (var constraint in index_constraints_in.discrete_range())
            {
                VHDL.DiscreteRange range = ParseExtention.Parse<vhdlParser.Discrete_rangeContext, VHDL.DiscreteRange>(constraint, VisitDiscrete_range);
                ranges.Add(range);
            }

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.ConstrainedArray res = new VHDL.type.ConstrainedArray("unknown", si, ranges);
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.allocator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAllocator([NotNull] vhdlParser.AllocatorContext context)
        {
            var qualified_expression_in = context.qualified_expression();
            var subtype_indication_in = context.subtype_indication();

            if (qualified_expression_in != null)
            {
                QualifiedExpression qae = ParseExtention.Parse<vhdlParser.Qualified_expressionContext, QualifiedExpression>(qualified_expression_in, VisitQualified_expression);
                QualifiedExpressionAllocator qaa = new QualifiedExpressionAllocator(qae);
                return qaa;
            }

            if (subtype_indication_in != null)
            {
                VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
                SubtypeIndicationAllocator sia = new SubtypeIndicationAllocator(si);
                return sia;
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.numeric_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNumeric_literal([NotNull] vhdlParser.Numeric_literalContext context)
        {
            var al = context.abstract_literal();
            var pl = context.physical_literal();

            if (al != null)
            {
                return VisitAbstract_literal(al);
            }

            if (pl != null)
            {
                return VisitPhysical_literal(pl);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.design_file"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDesign_file([NotNull] vhdlParser.Design_fileContext context)
        {
            VhdlFile res = new VhdlFile(FileName);
            List<LibraryUnit> contextItems = new List<LibraryUnit>();
            libraryScope.Files.Add(res);
            currentScope = res;
            foreach (VHDL_ANTLR4.vhdlParser.Design_unitContext item in context.design_unit())
            {
                LibraryUnit unit = ParseExtention.Parse<vhdlParser.Design_unitContext, LibraryUnit>(item, VisitDesign_unit);
                res.Elements.Add(unit);
            }
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.exit_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitExit_statement([NotNull] vhdlParser.Exit_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var identifier_in = context.identifier();
            var condition_in = context.condition();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string identifier = (identifier_in != null) ? identifier_in.GetText() : string.Empty;
            Expression condition = (condition_in != null) ? ParseExtention.Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition) : null;

            LoopStatement loop = resolve<LoopStatement>(identifier);

            ExitStatement exit = new ExitStatement(loop, condition);
            exit.Label = label;

            return exit;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.secondary_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSecondary_unit([NotNull] vhdlParser.Secondary_unitContext context)
        {
            var package_body = context.package_body();
            var architecture_body = context.architecture_body();

            if (package_body != null)
            {
                return VisitPackage_body(package_body);
            }

            if (architecture_body != null)
            {
                return VisitArchitecture_body(architecture_body);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_declaration([NotNull] vhdlParser.Interface_declarationContext context)
        {
            var interface_constant_declaration = context.interface_constant_declaration();
            var interface_variable_declaration = context.interface_variable_declaration();
            var interface_signal_declaration = context.interface_signal_declaration();
            var interface_terminal_declaration = context.interface_terminal_declaration();
            var interface_quantity_declaration = context.interface_quantity_declaration();
            var interface_file_declaration = context.interface_file_declaration();

            if (interface_constant_declaration != null)
            {
                return VisitInterface_constant_declaration(interface_constant_declaration);
            }

            if (interface_variable_declaration != null)
            {
                return VisitInterface_variable_declaration(interface_variable_declaration);
            }

            if (interface_signal_declaration != null)
            {
                return VisitInterface_signal_declaration(interface_signal_declaration);
            }

            if (interface_terminal_declaration != null)
            {
                return VisitInterface_terminal_declaration(interface_terminal_declaration);
            }

            if (interface_quantity_declaration != null)
            {
                return VisitInterface_quantity_declaration(interface_quantity_declaration);
            }

            if (interface_file_declaration != null)
            {
                return VisitInterface_file_declaration(interface_file_declaration);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_declaration([NotNull] vhdlParser.Subprogram_declarationContext context)
        {
            var subprogram_specification_in = context.subprogram_specification();

            SubprogramDeclaration res = ParseExtention.Parse<vhdlParser.Subprogram_specificationContext, SubprogramDeclaration>(subprogram_specification_in, VisitSubprogram_specification);
            AddAnnotations(res, context);

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.selected_signal_assignment"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSelected_signal_assignment([NotNull] vhdlParser.Selected_signal_assignmentContext context)
        {
            var expression_in = context.expression();
            var target_in = context.target();
            var opts_in = context.opts();
            var selected_waveforms_in = context.selected_waveforms();

            DelayMechanism delay = (opts_in.delay_mechanism() != null) ? ParseExtention.Parse<vhdlParser.Delay_mechanismContext, DelayMechanism>(opts_in.delay_mechanism(), VisitDelay_mechanism) : DelayMechanism.DUTY_CYCLE;
            bool is_guarded = opts_in.GUARDED() != null;

            ISignalAssignmentTarget target = ParseExtention.Parse<vhdlParser.TargetContext, ISignalAssignmentTarget>(target_in, VisitTarget);
            Expression exp = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);

            List<VHDL.concurrent.SelectedSignalAssignment.SelectedWaveform> SWEs = new List<SelectedSignalAssignment.SelectedWaveform>();
            for (int i = 0; i < selected_waveforms_in.choices().Length; i++)
            {
                var choises_in = selected_waveforms_in.choices()[i];
                var waveform_in = selected_waveforms_in.waveform()[i];

                List<Choice> choices = new List<Choice>();
                List<WaveformElement> waveforms = new List<WaveformElement>();
                foreach (var ch_in in choises_in.choice())
                {
                    Choice ch = ParseExtention.Parse<vhdlParser.ChoiceContext, Choice>(ch_in, VisitChoice);
                    choices.Add(ch);
                }
                foreach (var w_in in waveform_in.waveform_element())
                {
                    WaveformElement we = ParseExtention.Parse<vhdlParser.Waveform_elementContext, WaveformElement>(w_in, VisitWaveform_element);
                    waveforms.Add(we);
                }

                VHDL.concurrent.SelectedSignalAssignment.SelectedWaveform SWE = new SelectedSignalAssignment.SelectedWaveform(waveforms, choices);
                SWEs.Add(SWE);
            }

            SelectedSignalAssignment ssa = new SelectedSignalAssignment(exp, target, SWEs);
            ssa.DelayMechanism = delay;
            ssa.Guarded = is_guarded;

            return ssa;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.primary"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPrimary([NotNull] vhdlParser.PrimaryContext context)
        {
            var aggregate = context.aggregate();
            var allocator = context.allocator();
            var expression = context.expression();
            var literal = context.literal();
            var qualified_expression = context.qualified_expression();
            var name_in = context.name();


            if (aggregate != null)
            {
                return VisitAggregate(aggregate);
            }

            if (allocator != null)
            {
                return VisitAllocator(allocator);
            }

            if (expression != null)
            {
                return VisitExpression(expression);
            }

            if (literal != null)
            {
                return VisitLiteral(literal);
            }

            if (qualified_expression != null)
            {
                return VisitQualified_expression(qualified_expression);
            }

            if (name_in != null)
            {
                return VisitName(name_in);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.library_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLibrary_clause([NotNull] vhdlParser.Library_clauseContext context)
        {
            var logical_name_list_in = context.logical_name_list();

            List<string> libraries = new List<string>();
            foreach (var lib_in in logical_name_list_in.logical_name())
            {
                libraries.Add(lib_in.identifier().GetText());
            }

            LibraryClause library = new LibraryClause(libraries);

            AddAnnotations(library, context);
            CheckLibraryClause(context, library);

            return library;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.qualified_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQualified_expression([NotNull] vhdlParser.Qualified_expressionContext context)
        {
            var subtype_indication_in = context.subtype_indication();
            var aggregate_in = context.aggregate();
            var expression_in = context.expression();

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            if (aggregate_in != null)
            {
                Aggregate agg = ParseExtention.Parse<vhdlParser.AggregateContext, Aggregate>(aggregate_in, VisitAggregate);
                return new QualifiedExpression(si, agg);
            }

            if (expression_in != null)
            {
                Expression exp = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);
                return new QualifiedExpression(si, exp);
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_signal_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_signal_assignment_statement([NotNull] vhdlParser.Concurrent_signal_assignment_statementContext context)
        {
            var label_colon = context.label_colon();
            var selected_signal_assignment_in = context.selected_signal_assignment();
            var conditional_signal_assignment_in = context.conditional_signal_assignment();

            string label = (label_colon != null) ? label_colon.identifier().GetText() : string.Empty;
            bool is_postponed = context.POSTPONED() != null;

            AbstractPostponableConcurrentStatement apcs = null;
            if (selected_signal_assignment_in != null)
            {
                apcs = ParseExtention.Parse<vhdlParser.Selected_signal_assignmentContext, SelectedSignalAssignment>(selected_signal_assignment_in, VisitSelected_signal_assignment);
            }

            if (conditional_signal_assignment_in != null)
            {
                apcs = ParseExtention.Parse<vhdlParser.Conditional_signal_assignmentContext, ConditionalSignalAssignment>(conditional_signal_assignment_in, VisitConditional_signal_assignment);
            }

            apcs.Postponed = is_postponed;
            apcs.Label = label;

            return apcs;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.library_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLibrary_unit([NotNull] vhdlParser.Library_unitContext context)
        {
            var primary_unit = context.primary_unit();
            var secondary_unit = context.secondary_unit();

            if (primary_unit != null)
            {
                PrimaryUnit primary = ParseExtention.Parse<vhdlParser.Primary_unitContext, PrimaryUnit>(primary_unit, VisitPrimary_unit);
                contextItems.Clear();
                return primary;
            }

            if (secondary_unit != null)
            {
                SecondaryUnit secondary = ParseExtention.Parse<vhdlParser.Secondary_unitContext, SecondaryUnit>(secondary_unit, VisitSecondary_unit);
                contextItems.Clear();
                return secondary;
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.context_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitContext_clause([NotNull] vhdlParser.Context_clauseContext context)
        {
            var context_items_in = context.context_item();

            List<UseClause> uses = new List<UseClause>();
            foreach (var c_in in context_items_in)
            {
                var use_clause_in = c_in.use_clause();
                var library_clause_in = c_in.library_clause();

                if (use_clause_in != null)
                {
                    UseClause use = ParseExtention.Parse<vhdlParser.Use_clauseContext, UseClause>(use_clause_in, VisitUse_clause);
                    uses.Add(use);
                }

                if (library_clause_in != null)
                {
                    foreach (var library_in in library_clause_in.logical_name_list().logical_name())
                    {
                        LibraryDeclarativeRegion loaded_library = LoadLibrary(library_in.identifier().GetText());
                        if (loaded_library != null)
                            rootScope.AddLibrary(loaded_library);
                    }
                }
            }

            contextItems.Clear();
            contextItems.AddRange(uses);

            return null;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.element_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitElement_declaration([NotNull] vhdlParser.Element_declarationContext context)
        {
            var element_subtype_definition_in = context.element_subtype_definition();
            var identifier_list_in = context.identifier_list();

            List<string> identifiers = new List<string>();
            foreach (var i in identifier_list_in.identifier())
            {
                identifiers.Add(i.GetText());
            }

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Element_subtype_definitionContext, VHDL.type.ISubtypeIndication>(element_subtype_definition_in, VisitElement_subtype_definition);

            VHDL.type.RecordType.ElementDeclaration el = new VHDL.type.RecordType.ElementDeclaration(si, identifiers);

            return el;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_assertion_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_assertion_statement([NotNull] vhdlParser.Concurrent_assertion_statementContext context)
        {
            var label_colon_in = context.label_colon();
            bool is_postponed = context.POSTPONED() != null;
            var assertion_in = context.assertion();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            AssertionStatement assertionStatement = ParseExtention.Parse<vhdlParser.AssertionContext, AssertionStatement>(assertion_in, VisitAssertion);
            assertionStatement.Label = label;

            ConcurrentAssertionStatement concurrentAssertionStatement = new ConcurrentAssertionStatement(assertionStatement);
            concurrentAssertionStatement.Postponed = is_postponed;

            return concurrentAssertionStatement;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_type_definition([NotNull] vhdlParser.File_type_definitionContext context)
        {
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.FileType file = new VHDL.type.FileType("unknown", si);
            return file;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.wait_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitWait_statement([NotNull] vhdlParser.Wait_statementContext context)
        {
            var condition_clause_in = context.condition_clause();
            var label_colon_in = context.label_colon();
            var sensitivity_clause_in = context.sensitivity_clause();
            var timeout_clause_in = context.timeout_clause();

            string label = (label_colon_in != null) ? label_colon_in.GetText() : string.Empty;
            List<Signal> sensitivityList = new List<Signal>();
            if ((sensitivity_clause_in != null) && (sensitivity_clause_in.sensitivity_list() != null))
            {
                foreach (var s_in in sensitivity_clause_in.sensitivity_list().name())
                {
                    Signal s = ParseExtention.Parse<vhdlParser.NameContext, Signal>(s_in, VisitName);
                    sensitivityList.Add(s);
                }
            }
            Expression timeout = (timeout_clause_in != null) ? ParseExtention.Parse<vhdlParser.Timeout_clauseContext, Expression>(timeout_clause_in, VisitTimeout_clause) : null;
            Expression condition = (condition_clause_in != null) ? ParseExtention.Parse<vhdlParser.Condition_clauseContext, Expression>(condition_clause_in, VisitCondition_clause) : null;

            WaitStatement ws = new WaitStatement(sensitivityList)
            {
                Timeout = timeout,
                Label = label,
                Condition = condition
            };
            return ws;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.loop_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLoop_statement([NotNull] vhdlParser.Loop_statementContext context)
        {
            var label_colon_in = context.label_colon();
            var iteration_scheme_in = context.iteration_scheme();
            var sequence_of_statements_in = context.sequence_of_statements();
            var identifier_in = context.identifier();

            string label_begin = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            //-------------------------------------------------------------
            //      Before parsing
            //-------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //-------------------------------------------------------------


            VHDL.statement.LoopStatement loop = (iteration_scheme_in != null) ? ParseExtention.Parse<vhdlParser.Iteration_schemeContext, VHDL.statement.LoopStatement>(iteration_scheme_in, VisitIteration_scheme) : new LoopStatement();

            loop.Label = label_begin;
            loop.Parent = oldScope;
            currentScope = loop;

            loop.Statements.AddRange(ParseExtention.ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in.sequential_statement(), VisitSequential_statement));

            //-------------------------------------------------------------
            //      After parsing
            //-------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(loop, context);
            //-------------------------------------------------------------

            if (label_begin.VHDLCheckBeginEndIdentifierForEquals(label_end) == false)
            {
                throw new System.ArgumentException(string.Format("Loop identifiers mismatch. Loop begin identifier is '{0}' and end identifier is '{1}'", label_begin, label_end));
            }

            return loop;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.actual_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitActual_part([NotNull] vhdlParser.Actual_partContext context)
        {
            if ((context.actual_designator() != null) && (context.actual_designator().expression() != null))
                return VisitExpression(context.actual_designator().expression());

            return VisitChildren(context);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_declaration([NotNull] vhdlParser.File_declarationContext context)
        {
            var file_open_information_in = context.file_open_information();
            var identifier_list_in = context.identifier_list();
            var subtype_indication_in = context.subtype_indication();

            //1. parse identifiers
            List<string> identifiers = new List<string>();
            foreach (var identifier_in in identifier_list_in.identifier())
            {
                identifiers.Add(identifier_in.GetText());
            }

            //2. parse file type
            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            //FileDeclaration res = new FileDeclaration(
            Expression open_kind = null;
            Expression filename_logical_name = null;

            //3. parse file open information
            if (file_open_information_in != null)
            {
                var file_open_expression_in = file_open_information_in.expression();
                var file_logical_name_in = file_open_information_in.file_logical_name();

                open_kind = (file_open_expression_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(file_open_expression_in, VisitExpression) : null;
                filename_logical_name = (file_logical_name_in != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(file_logical_name_in.expression(), VisitExpression) : null;
            }

            List<FileObject> FOs = new List<FileObject>();
            foreach (string filetype_name in identifiers)
            {
                FileObject fo = new FileObject(filetype_name, si, open_kind, filename_logical_name);
                FOs.Add(fo);
            }

            FileDeclaration FD = new FileDeclaration(FOs);

            AddAnnotations(FD, context);

            return FD;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_signal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_signal_declaration([NotNull] vhdlParser.Interface_signal_declarationContext context)
        {
            bool hasObjectClass = false;
            bool hasMode = false;
            bool isBus = false;
            //--------------------------------------------------
            var identifier_list = context.identifier_list();
            var subtype_indication_in = context.subtype_indication();
            var expression = context.expression();

            var BUS = context.BUS();

            isBus = BUS != null;
            hasObjectClass = context.SIGNAL() != null;

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);

            Expression def = (expression != null) ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression, VisitExpression) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication_in != null) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication) : null;

            SignalGroup res = new SignalGroup();

            foreach (var identifier in identifier_list.identifier())
            {
                string signal_name = identifier.GetText();
                Signal s = new Signal(signal_name, Signal.ModeEnum.INOUT, type, def);
                if (isBus)
                {
                    s.Kind = Signal.KindEnum.BUS;
                }
                Annotations.putAnnotation(s, format);

                res.Elements.Add(s);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            return res;
        }

        public override VhdlElement VisitInterface_port_declaration(vhdlParser.Interface_port_declarationContext context)
        {
            bool hasObjectClass = false;
            bool hasMode = false;
            bool isBus = false;
            //--------------------------------------------------
            var identifier_list = context.identifier_list();
            var signal_mode = context.signal_mode();
            var subtype_indication_in = context.subtype_indication();

            var BUS = context.BUS();

            isBus = BUS != null;
            hasMode = signal_mode != null;

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);
            Signal.ModeEnum m = ParseSignalMode(signal_mode);

            VHDL.type.ISubtypeIndication type = (subtype_indication_in != null) ? ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication) : null;

            SignalGroup res = new SignalGroup();

            foreach (var identifier in identifier_list.identifier())
            {
                string signal_name = identifier.GetText();
                Signal s = new Signal(signal_name, m, type, null);
                if (isBus)
                {
                    s.Kind = Signal.KindEnum.BUS;
                }
                Annotations.putAnnotation(s, format);

                res.Elements.Add(s);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            return res;
        }



        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.access_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAccess_type_definition([NotNull] vhdlParser.Access_type_definitionContext context)
        {
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = ParseExtention.Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.AccessType access = new VHDL.type.AccessType("unknown", si);
            return access;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.report_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitReport_statement([NotNull] vhdlParser.Report_statementContext context)
        {
            var expressions_in = context.expression();
            var label_colon_in = context.label_colon();
            bool has_severity = context.SEVERITY() != null;

            VHDL.expression.Expression expression = ParseExtention.Parse<vhdlParser.ExpressionContext, VHDL.expression.Expression>(expressions_in[0], VisitExpression);

            ReportStatement report = null;
            if (has_severity)
            {
                VHDL.expression.Expression expression_severity = ParseExtention.Parse<vhdlParser.ExpressionContext, VHDL.expression.Expression>(expressions_in[1], VisitExpression);
                report = new ReportStatement(expression, expression_severity);
            }
            else
            {
                report = new ReportStatement(expression);
            }

            if (label_colon_in != null)
                report.Label = label_colon_in.identifier().GetText();

            return report;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_call"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedure_call([NotNull] vhdlParser.Procedure_callContext context)
        {
            var selected_name_in = context.selected_name();
            var actual_parameter_part_in = context.actual_parameter_part();

            List<AssociationElement> parameters = (actual_parameter_part_in != null) ? ParseExtention.ParseList<vhdlParser.Association_elementContext, AssociationElement>(actual_parameter_part_in.association_list().association_element(), VisitAssociation_element) : new List<AssociationElement>();

            VHDL.parser.antlr.TemporaryName tn = ParseExtention.Parse<vhdlParser.Selected_nameContext, VHDL.parser.antlr.TemporaryName>(selected_name_in, VisitSelected_name);

            // TODO: Add procedure call resolution code here !!!!!!!!!!!
            //ProcedureCall procedureCall = tn.GetProcedureCall(parameters);

            ProcedureDeclaration procedureDeclaration = tn.GetProcedure();
            ProcedureCall procedureCall = new ProcedureCall(procedureDeclaration, parameters);

            return procedureCall;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.waveform_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitWaveform_element([NotNull] vhdlParser.Waveform_elementContext context)
        {
            var expression_in = context.expression();
            bool has_delay = context.AFTER() != null;

            Expression wa_value = ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in[0], VisitExpression);
            Expression wa_after = has_delay ? ParseExtention.Parse<vhdlParser.ExpressionContext, Expression>(expression_in[1], VisitExpression) : null;

            WaveformElement wa = new WaveformElement(wa_value, wa_after);

            return wa;
        }

        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.type_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitType_declaration([NotNull] vhdlParser.Type_declarationContext context)
        {
            var identifier_in = context.identifier();
            var type_definition_in = context.type_definition();

            string typename = identifier_in.GetText();

            if (type_definition_in.access_type_definition() != null)
            {
                var access_type_definition_in = type_definition_in.access_type_definition();

                VHDL.type.AccessType access = ParseExtention.Parse<vhdlParser.Access_type_definitionContext, VHDL.type.AccessType>(access_type_definition_in, VisitAccess_type_definition);
                access.Identifier = typename;
                return access;
            }

            if (type_definition_in.composite_type_definition() != null)
            {
                var composite_type_definition_in = type_definition_in.composite_type_definition();

                VHDL.type.Type composite = ParseExtention.Parse<vhdlParser.Composite_type_definitionContext, VHDL.type.Type>(composite_type_definition_in, VisitComposite_type_definition);
                composite.Identifier = typename;
                return composite;
            }

            if (type_definition_in.file_type_definition() != null)
            {
                var file_type_definition_in = type_definition_in.file_type_definition();

                VHDL.type.FileType file = ParseExtention.Parse<vhdlParser.File_type_definitionContext, VHDL.type.FileType>(file_type_definition_in, VisitFile_type_definition);
                file.Identifier = typename;
                return file;
            }

            if (type_definition_in.scalar_type_definition() != null)
            {
                var scalar_type_definition_in = type_definition_in.scalar_type_definition();

                VHDL.type.Type scalar = ParseExtention.Parse<vhdlParser.Scalar_type_definitionContext, VHDL.type.Type>(scalar_type_definition_in, VisitScalar_type_definition);
                scalar.Identifier = typename;
                return scalar;
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.attribute_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAttribute_declaration([NotNull] vhdlParser.Attribute_declarationContext context)
        {
            var label_colon_in = context.label_colon();
            var name_in = context.name();

            string attribute_name = label_colon_in.identifier().GetText();

            VHDL.type.ISubtypeIndication si = resolve<VHDL.type.ISubtypeIndication>(name_in.GetText());

            VHDL.declaration.Attribute attribute = new VHDL.declaration.Attribute(attribute_name, si);

            return attribute;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.term"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTerm([NotNull] vhdlParser.TermContext context)
        {
            var factors_in = context.factor();
            var multiplying_operators_in = context.multiplying_operator();

            List<Expression> parsed_factors = new List<Expression>();

            foreach (var f in factors_in)
            {
                Expression parsed_factor = ParseExtention.Parse<vhdlParser.FactorContext, Expression>(f, VisitFactor);
                parsed_factors.Add(parsed_factor);
            }

            if (parsed_factors.Count == 0)
                throw new System.NotSupportedException(string.Format("Could not analyse item {0}. Amount of factors is 0.", context.ToStringTree()));

            Expression res = parsed_factors[0];

            for (int i = 0; i < multiplying_operators_in.Length; i++)
            {
                var op = multiplying_operators_in[i];
                Expression curr_exp = parsed_factors[i + 1];

                if (op.DIV() != null)
                {
                    Expression new_exp = new VHDL.expression.Divide(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if (op.MOD() != null)
                {
                    Expression new_exp = new VHDL.expression.Mod(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if (op.MUL() != null)
                {
                    Expression new_exp = new VHDL.expression.Multiply(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if (op.REM() != null)
                {
                    Expression new_exp = new VHDL.expression.Rem(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                throw new System.NotSupportedException(string.Format("Could not analyse item {0}", op.ToStringTree()));

            }

            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLiteral([NotNull] vhdlParser.LiteralContext context)
        {
            var BIT_STRING_LITERAL = context.BIT_STRING_LITERAL();
            var enumeration_literal = context.enumeration_literal();
            var NULL = context.NULL();
            var numeric_literal = context.numeric_literal();
            var STRING_LITERAL = context.STRING_LITERAL();

            if (BIT_STRING_LITERAL != null)
            {
                string text = BIT_STRING_LITERAL.GetText().ToLower();
                switch (text[0])
                {
                    case 'b':
                        {
                            return new BinaryStringLiteral(text);
                        }
                        break;
                    case 'o':
                        {
                            return new OctalStringLiteral(text);
                        }
                        break;
                    case 'x':
                        {
                            return new HexStringLiteral(text);
                        }
                        break;
                    default:
                        {
                            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", BIT_STRING_LITERAL.ToStringTree()));
                        }
                        break;
                }
            }

            if (enumeration_literal != null)
            {
                return VisitEnumeration_literal(enumeration_literal);
            }

            if (NULL != null)
            {
                return new VHDL.literal.Literals.NullLiteral();
            }

            if (numeric_literal != null)
            {
                return VisitNumeric_literal(numeric_literal);
            }

            if (STRING_LITERAL != null)
            {
                return new StringLiteral(STRING_LITERAL.GetText());
            }

            throw new System.NotSupportedException(string.Format("Could not analyse item {0}", context.ToStringTree()));
        }
    }
}

