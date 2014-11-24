/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 22.11.2014
 * Time: 21:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace VHDL_ANTLR4
{
	/// <summary>
	/// Description of vhdlListener.
	/// </summary>
	public class vhdlListener : vhdlBaseListener
	{
		public vhdlListener()
		{
		}

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.assertion_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAssertion_statement([NotNull] vhdlParser.Assertion_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.assertion_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAssertion_statement([NotNull] vhdlParser.Assertion_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_kind"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_kind([NotNull] vhdlParser.Subprogram_kindContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_kind"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_kind([NotNull] vhdlParser.Subprogram_kindContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.association_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAssociation_list([NotNull] vhdlParser.Association_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.association_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAssociation_list([NotNull] vhdlParser.Association_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.unconstrained_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterUnconstrained_nature_definition([NotNull] vhdlParser.Unconstrained_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.unconstrained_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitUnconstrained_nature_definition([NotNull] vhdlParser.Unconstrained_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_header"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_header([NotNull] vhdlParser.Entity_headerContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_header"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_header([NotNull] vhdlParser.Entity_headerContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.sensitivity_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSensitivity_list([NotNull] vhdlParser.Sensitivity_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.sensitivity_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSensitivity_list([NotNull] vhdlParser.Sensitivity_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_statement_part([NotNull] vhdlParser.Simultaneous_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_statement_part([NotNull] vhdlParser.Simultaneous_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.conditional_waveforms"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConditional_waveforms([NotNull] vhdlParser.Conditional_waveformsContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.conditional_waveforms"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConditional_waveforms([NotNull] vhdlParser.Conditional_waveformsContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.sequential_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSequential_statement([NotNull] vhdlParser.Sequential_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.sequential_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSequential_statement([NotNull] vhdlParser.Sequential_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_quantity_declaration([NotNull] vhdlParser.Interface_quantity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_quantity_declaration([NotNull] vhdlParser.Interface_quantity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.terminal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTerminal_declaration([NotNull] vhdlParser.Terminal_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.terminal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTerminal_declaration([NotNull] vhdlParser.Terminal_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.tolerance_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTolerance_aspect([NotNull] vhdlParser.Tolerance_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.tolerance_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTolerance_aspect([NotNull] vhdlParser.Tolerance_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subnature_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubnature_declaration([NotNull] vhdlParser.Subnature_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subnature_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubnature_declaration([NotNull] vhdlParser.Subnature_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signature"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignature([NotNull] vhdlParser.SignatureContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signature"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignature([NotNull] vhdlParser.SignatureContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_statement([NotNull] vhdlParser.Simultaneous_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_statement([NotNull] vhdlParser.Simultaneous_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.port_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPort_list([NotNull] vhdlParser.Port_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.port_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPort_list([NotNull] vhdlParser.Port_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.instantiation_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInstantiation_list([NotNull] vhdlParser.Instantiation_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.instantiation_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInstantiation_list([NotNull] vhdlParser.Instantiation_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.quantity_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterQuantity_list([NotNull] vhdlParser.Quantity_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.quantity_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitQuantity_list([NotNull] vhdlParser.Quantity_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.parameter_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterParameter_specification([NotNull] vhdlParser.Parameter_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.parameter_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitParameter_specification([NotNull] vhdlParser.Parameter_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.identifier_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIdentifier_list([NotNull] vhdlParser.Identifier_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.identifier_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIdentifier_list([NotNull] vhdlParser.Identifier_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_declarative_part([NotNull] vhdlParser.Block_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_declarative_part([NotNull] vhdlParser.Block_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.record_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRecord_type_definition([NotNull] vhdlParser.Record_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.record_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRecord_type_definition([NotNull] vhdlParser.Record_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.multiplying_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterMultiplying_operator([NotNull] vhdlParser.Multiplying_operatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.multiplying_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitMultiplying_operator([NotNull] vhdlParser.Multiplying_operatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.generic_map_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGeneric_map_aspect([NotNull] vhdlParser.Generic_map_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.generic_map_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGeneric_map_aspect([NotNull] vhdlParser.Generic_map_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signal_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignal_list([NotNull] vhdlParser.Signal_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signal_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignal_list([NotNull] vhdlParser.Signal_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.branch_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBranch_quantity_declaration([NotNull] vhdlParser.Branch_quantity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.branch_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBranch_quantity_declaration([NotNull] vhdlParser.Branch_quantity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.function_call"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFunction_call([NotNull] vhdlParser.Function_callContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.function_call"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFunction_call([NotNull] vhdlParser.Function_callContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.timeout_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTimeout_clause([NotNull] vhdlParser.Timeout_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.timeout_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTimeout_clause([NotNull] vhdlParser.Timeout_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_name_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_name_list([NotNull] vhdlParser.Entity_name_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_name_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_name_list([NotNull] vhdlParser.Entity_name_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.object_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterObject_declaration([NotNull] vhdlParser.Object_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.object_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitObject_declaration([NotNull] vhdlParser.Object_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.conditional_waveforms_bi"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConditional_waveforms_bi([NotNull] vhdlParser.Conditional_waveforms_biContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.conditional_waveforms_bi"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConditional_waveforms_bi([NotNull] vhdlParser.Conditional_waveforms_biContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.choice"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterChoice([NotNull] vhdlParser.ChoiceContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.choice"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitChoice([NotNull] vhdlParser.ChoiceContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.generate_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGenerate_statement([NotNull] vhdlParser.Generate_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.generate_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGenerate_statement([NotNull] vhdlParser.Generate_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.alias_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAlias_designator([NotNull] vhdlParser.Alias_designatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.alias_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAlias_designator([NotNull] vhdlParser.Alias_designatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_statement([NotNull] vhdlParser.Entity_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_statement([NotNull] vhdlParser.Entity_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.sensitivity_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSensitivity_clause([NotNull] vhdlParser.Sensitivity_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.sensitivity_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSensitivity_clause([NotNull] vhdlParser.Sensitivity_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.alias_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAlias_declaration([NotNull] vhdlParser.Alias_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.alias_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAlias_declaration([NotNull] vhdlParser.Alias_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.attribute_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAttribute_designator([NotNull] vhdlParser.Attribute_designatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.attribute_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAttribute_designator([NotNull] vhdlParser.Attribute_designatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_alternative"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_alternative([NotNull] vhdlParser.Simultaneous_alternativeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_alternative"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_alternative([NotNull] vhdlParser.Simultaneous_alternativeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.architecture_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArchitecture_body([NotNull] vhdlParser.Architecture_bodyContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.architecture_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArchitecture_body([NotNull] vhdlParser.Architecture_bodyContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_tag"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_tag([NotNull] vhdlParser.Entity_tagContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_tag"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_tag([NotNull] vhdlParser.Entity_tagContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subtype_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubtype_indication([NotNull] vhdlParser.Subtype_indicationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subtype_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubtype_indication([NotNull] vhdlParser.Subtype_indicationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.process_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcess_statement([NotNull] vhdlParser.Process_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.process_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcess_statement([NotNull] vhdlParser.Process_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_aspect([NotNull] vhdlParser.Entity_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_aspect([NotNull] vhdlParser.Entity_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.choices"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterChoices([NotNull] vhdlParser.ChoicesContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.choices"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitChoices([NotNull] vhdlParser.ChoicesContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.design_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDesign_unit([NotNull] vhdlParser.Design_unitContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.design_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDesign_unit([NotNull] vhdlParser.Design_unitContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.factor"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFactor([NotNull] vhdlParser.FactorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.factor"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFactor([NotNull] vhdlParser.FactorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.relational_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRelational_operator([NotNull] vhdlParser.Relational_operatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.relational_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRelational_operator([NotNull] vhdlParser.Relational_operatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.index_subtype_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIndex_subtype_definition([NotNull] vhdlParser.Index_subtype_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.index_subtype_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIndex_subtype_definition([NotNull] vhdlParser.Index_subtype_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_body([NotNull] vhdlParser.Subprogram_bodyContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_body([NotNull] vhdlParser.Subprogram_bodyContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.delay_mechanism"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDelay_mechanism([NotNull] vhdlParser.Delay_mechanismContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.delay_mechanism"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDelay_mechanism([NotNull] vhdlParser.Delay_mechanismContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.process_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcess_declarative_item([NotNull] vhdlParser.Process_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.process_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcess_declarative_item([NotNull] vhdlParser.Process_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.group_template_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGroup_template_declaration([NotNull] vhdlParser.Group_template_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.group_template_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGroup_template_declaration([NotNull] vhdlParser.Group_template_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_body([NotNull] vhdlParser.Package_bodyContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_body"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_body([NotNull] vhdlParser.Package_bodyContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.range_constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRange_constraint([NotNull] vhdlParser.Range_constraintContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.range_constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRange_constraint([NotNull] vhdlParser.Range_constraintContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.secondary_unit_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSecondary_unit_declaration([NotNull] vhdlParser.Secondary_unit_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.secondary_unit_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSecondary_unit_declaration([NotNull] vhdlParser.Secondary_unit_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_body_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_body_declarative_part([NotNull] vhdlParser.Package_body_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_body_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_body_declarative_part([NotNull] vhdlParser.Package_body_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.procedure_call_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcedure_call_statement([NotNull] vhdlParser.Procedure_call_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.procedure_call_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcedure_call_statement([NotNull] vhdlParser.Procedure_call_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterExpression([NotNull] vhdlParser.ExpressionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitExpression([NotNull] vhdlParser.ExpressionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.abstract_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAbstract_literal([NotNull] vhdlParser.Abstract_literalContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.abstract_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAbstract_literal([NotNull] vhdlParser.Abstract_literalContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_variable_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_variable_declaration([NotNull] vhdlParser.Interface_variable_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_variable_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_variable_declaration([NotNull] vhdlParser.Interface_variable_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.next_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterNext_statement([NotNull] vhdlParser.Next_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.next_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitNext_statement([NotNull] vhdlParser.Next_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.scalar_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterScalar_type_definition([NotNull] vhdlParser.Scalar_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.scalar_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitScalar_type_definition([NotNull] vhdlParser.Scalar_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.constant_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConstant_declaration([NotNull] vhdlParser.Constant_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.constant_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConstant_declaration([NotNull] vhdlParser.Constant_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.component_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComponent_declaration([NotNull] vhdlParser.Component_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.component_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComponent_declaration([NotNull] vhdlParser.Component_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_file_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_file_declaration([NotNull] vhdlParser.Interface_file_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_file_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_file_declaration([NotNull] vhdlParser.Interface_file_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.concurrent_break_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConcurrent_break_statement([NotNull] vhdlParser.Concurrent_break_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.concurrent_break_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConcurrent_break_statement([NotNull] vhdlParser.Concurrent_break_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.context_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterContext_item([NotNull] vhdlParser.Context_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.context_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitContext_item([NotNull] vhdlParser.Context_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.configuration_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConfiguration_specification([NotNull] vhdlParser.Configuration_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.configuration_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConfiguration_specification([NotNull] vhdlParser.Configuration_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.association_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAssociation_element([NotNull] vhdlParser.Association_elementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.association_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAssociation_element([NotNull] vhdlParser.Association_elementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.condition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterCondition([NotNull] vhdlParser.ConditionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.condition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitCondition([NotNull] vhdlParser.ConditionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.case_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterCase_statement([NotNull] vhdlParser.Case_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.case_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitCase_statement([NotNull] vhdlParser.Case_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.logical_name_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLogical_name_list([NotNull] vhdlParser.Logical_name_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.logical_name_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLogical_name_list([NotNull] vhdlParser.Logical_name_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.relation"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRelation([NotNull] vhdlParser.RelationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.relation"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRelation([NotNull] vhdlParser.RelationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.constrained_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConstrained_nature_definition([NotNull] vhdlParser.Constrained_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.constrained_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConstrained_nature_definition([NotNull] vhdlParser.Constrained_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.conditional_signal_assignment"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConditional_signal_assignment([NotNull] vhdlParser.Conditional_signal_assignmentContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.conditional_signal_assignment"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConditional_signal_assignment([NotNull] vhdlParser.Conditional_signal_assignmentContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.process_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcess_declarative_part([NotNull] vhdlParser.Process_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.process_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcess_declarative_part([NotNull] vhdlParser.Process_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.waveform"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterWaveform([NotNull] vhdlParser.WaveformContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.waveform"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitWaveform([NotNull] vhdlParser.WaveformContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.port_map_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPort_map_aspect([NotNull] vhdlParser.Port_map_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.port_map_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPort_map_aspect([NotNull] vhdlParser.Port_map_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterQuantity_declaration([NotNull] vhdlParser.Quantity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitQuantity_declaration([NotNull] vhdlParser.Quantity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.architecture_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArchitecture_statement([NotNull] vhdlParser.Architecture_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.architecture_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArchitecture_statement([NotNull] vhdlParser.Architecture_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.component_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComponent_specification([NotNull] vhdlParser.Component_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.component_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComponent_specification([NotNull] vhdlParser.Component_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.logical_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLogical_operator([NotNull] vhdlParser.Logical_operatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.logical_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLogical_operator([NotNull] vhdlParser.Logical_operatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.source_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSource_quantity_declaration([NotNull] vhdlParser.Source_quantity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.source_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSource_quantity_declaration([NotNull] vhdlParser.Source_quantity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.identifier"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIdentifier([NotNull] vhdlParser.IdentifierContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.identifier"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIdentifier([NotNull] vhdlParser.IdentifierContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.composite_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComposite_type_definition([NotNull] vhdlParser.Composite_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.composite_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComposite_type_definition([NotNull] vhdlParser.Composite_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.procedural_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcedural_declarative_item([NotNull] vhdlParser.Procedural_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.procedural_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcedural_declarative_item([NotNull] vhdlParser.Procedural_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_declarative_part([NotNull] vhdlParser.Entity_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_declarative_part([NotNull] vhdlParser.Entity_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_case_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_case_statement([NotNull] vhdlParser.Simultaneous_case_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_case_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_case_statement([NotNull] vhdlParser.Simultaneous_case_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signal_mode"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignal_mode([NotNull] vhdlParser.Signal_modeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signal_mode"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignal_mode([NotNull] vhdlParser.Signal_modeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_configuration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_configuration([NotNull] vhdlParser.Block_configurationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_configuration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_configuration([NotNull] vhdlParser.Block_configurationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.physical_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPhysical_literal([NotNull] vhdlParser.Physical_literalContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.physical_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPhysical_literal([NotNull] vhdlParser.Physical_literalContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.enumeration_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEnumeration_literal([NotNull] vhdlParser.Enumeration_literalContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.enumeration_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEnumeration_literal([NotNull] vhdlParser.Enumeration_literalContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_constant_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_constant_declaration([NotNull] vhdlParser.Interface_constant_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_constant_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_constant_declaration([NotNull] vhdlParser.Interface_constant_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterName([NotNull] vhdlParser.NameContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitName([NotNull] vhdlParser.NameContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_declaration([NotNull] vhdlParser.Package_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_declaration([NotNull] vhdlParser.Package_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_class_entry"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_class_entry([NotNull] vhdlParser.Entity_class_entryContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_class_entry"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_class_entry([NotNull] vhdlParser.Entity_class_entryContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.group_constituent"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGroup_constituent([NotNull] vhdlParser.Group_constituentContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.group_constituent"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGroup_constituent([NotNull] vhdlParser.Group_constituentContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.unconstrained_array_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterUnconstrained_array_definition([NotNull] vhdlParser.Unconstrained_array_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.unconstrained_array_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitUnconstrained_array_definition([NotNull] vhdlParser.Unconstrained_array_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_header"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_header([NotNull] vhdlParser.Block_headerContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_header"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_header([NotNull] vhdlParser.Block_headerContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterNature_definition([NotNull] vhdlParser.Nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitNature_definition([NotNull] vhdlParser.Nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signal_kind"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignal_kind([NotNull] vhdlParser.Signal_kindContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signal_kind"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignal_kind([NotNull] vhdlParser.Signal_kindContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.file_logical_name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFile_logical_name([NotNull] vhdlParser.File_logical_nameContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.file_logical_name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFile_logical_name([NotNull] vhdlParser.File_logical_nameContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.quantity_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterQuantity_specification([NotNull] vhdlParser.Quantity_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.quantity_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitQuantity_specification([NotNull] vhdlParser.Quantity_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.assertion"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAssertion([NotNull] vhdlParser.AssertionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.assertion"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAssertion([NotNull] vhdlParser.AssertionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_body_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_body_declarative_item([NotNull] vhdlParser.Package_body_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_body_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_body_declarative_item([NotNull] vhdlParser.Package_body_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.group_constituent_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGroup_constituent_list([NotNull] vhdlParser.Group_constituent_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.group_constituent_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGroup_constituent_list([NotNull] vhdlParser.Group_constituent_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.source_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSource_aspect([NotNull] vhdlParser.Source_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.source_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSource_aspect([NotNull] vhdlParser.Source_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.composite_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComposite_nature_definition([NotNull] vhdlParser.Composite_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.composite_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComposite_nature_definition([NotNull] vhdlParser.Composite_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_declarative_item([NotNull] vhdlParser.Subprogram_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_declarative_item([NotNull] vhdlParser.Subprogram_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.through_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterThrough_aspect([NotNull] vhdlParser.Through_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.through_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitThrough_aspect([NotNull] vhdlParser.Through_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.array_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArray_type_definition([NotNull] vhdlParser.Array_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.array_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArray_type_definition([NotNull] vhdlParser.Array_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.nature_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterNature_declaration([NotNull] vhdlParser.Nature_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.nature_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitNature_declaration([NotNull] vhdlParser.Nature_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_declaration([NotNull] vhdlParser.Entity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_declaration([NotNull] vhdlParser.Entity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.aggregate"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAggregate([NotNull] vhdlParser.AggregateContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.aggregate"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAggregate([NotNull] vhdlParser.AggregateContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_designator([NotNull] vhdlParser.Entity_designatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_designator([NotNull] vhdlParser.Entity_designatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.case_statement_alternative"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterCase_statement_alternative([NotNull] vhdlParser.Case_statement_alternativeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.case_statement_alternative"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitCase_statement_alternative([NotNull] vhdlParser.Case_statement_alternativeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.binding_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBinding_indication([NotNull] vhdlParser.Binding_indicationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.binding_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBinding_indication([NotNull] vhdlParser.Binding_indicationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.component_configuration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComponent_configuration([NotNull] vhdlParser.Component_configurationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.component_configuration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComponent_configuration([NotNull] vhdlParser.Component_configurationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDesignator([NotNull] vhdlParser.DesignatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDesignator([NotNull] vhdlParser.DesignatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_element([NotNull] vhdlParser.Interface_elementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_element([NotNull] vhdlParser.Interface_elementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.architecture_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArchitecture_statement_part([NotNull] vhdlParser.Architecture_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.architecture_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArchitecture_statement_part([NotNull] vhdlParser.Architecture_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_declarative_item([NotNull] vhdlParser.Block_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_declarative_item([NotNull] vhdlParser.Block_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signal_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignal_assignment_statement([NotNull] vhdlParser.Signal_assignment_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signal_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignal_assignment_statement([NotNull] vhdlParser.Signal_assignment_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.element_subtype_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterElement_subtype_definition([NotNull] vhdlParser.Element_subtype_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.element_subtype_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitElement_subtype_definition([NotNull] vhdlParser.Element_subtype_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.procedural_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcedural_statement_part([NotNull] vhdlParser.Procedural_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.procedural_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcedural_statement_part([NotNull] vhdlParser.Procedural_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.component_instantiation_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterComponent_instantiation_statement([NotNull] vhdlParser.Component_instantiation_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.component_instantiation_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitComponent_instantiation_statement([NotNull] vhdlParser.Component_instantiation_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_specification([NotNull] vhdlParser.Block_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_specification([NotNull] vhdlParser.Block_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.step_limit_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterStep_limit_specification([NotNull] vhdlParser.Step_limit_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.step_limit_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitStep_limit_specification([NotNull] vhdlParser.Step_limit_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.formal_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFormal_part([NotNull] vhdlParser.Formal_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.formal_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFormal_part([NotNull] vhdlParser.Formal_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.primary_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPrimary_unit([NotNull] vhdlParser.Primary_unitContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.primary_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPrimary_unit([NotNull] vhdlParser.Primary_unitContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.configuration_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConfiguration_declarative_part([NotNull] vhdlParser.Configuration_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.configuration_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConfiguration_declarative_part([NotNull] vhdlParser.Configuration_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_declarative_part([NotNull] vhdlParser.Package_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_declarative_part([NotNull] vhdlParser.Package_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.shift_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterShift_expression([NotNull] vhdlParser.Shift_expressionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.shift_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitShift_expression([NotNull] vhdlParser.Shift_expressionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.iteration_scheme"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIteration_scheme([NotNull] vhdlParser.Iteration_schemeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.iteration_scheme"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIteration_scheme([NotNull] vhdlParser.Iteration_schemeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.concurrent_procedure_call_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConcurrent_procedure_call_statement([NotNull] vhdlParser.Concurrent_procedure_call_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.concurrent_procedure_call_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConcurrent_procedure_call_statement([NotNull] vhdlParser.Concurrent_procedure_call_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.discrete_range"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDiscrete_range([NotNull] vhdlParser.Discrete_rangeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.discrete_range"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDiscrete_range([NotNull] vhdlParser.Discrete_rangeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.element_association"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterElement_association([NotNull] vhdlParser.Element_associationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.element_association"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitElement_association([NotNull] vhdlParser.Element_associationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subtype_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubtype_declaration([NotNull] vhdlParser.Subtype_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subtype_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubtype_declaration([NotNull] vhdlParser.Subtype_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_specification([NotNull] vhdlParser.Subprogram_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_specification([NotNull] vhdlParser.Subprogram_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.range"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRange([NotNull] vhdlParser.RangeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.range"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRange([NotNull] vhdlParser.RangeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.variable_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterVariable_assignment_statement([NotNull] vhdlParser.Variable_assignment_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.variable_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitVariable_assignment_statement([NotNull] vhdlParser.Variable_assignment_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.if_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIf_statement([NotNull] vhdlParser.If_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.if_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIf_statement([NotNull] vhdlParser.If_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConstraint([NotNull] vhdlParser.ConstraintContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConstraint([NotNull] vhdlParser.ConstraintContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.break_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBreak_element([NotNull] vhdlParser.Break_elementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.break_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBreak_element([NotNull] vhdlParser.Break_elementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.configuration_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConfiguration_item([NotNull] vhdlParser.Configuration_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.configuration_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConfiguration_item([NotNull] vhdlParser.Configuration_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_statement_part([NotNull] vhdlParser.Block_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_statement_part([NotNull] vhdlParser.Block_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.physical_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPhysical_type_definition([NotNull] vhdlParser.Physical_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.physical_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPhysical_type_definition([NotNull] vhdlParser.Physical_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.configuration_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConfiguration_declaration([NotNull] vhdlParser.Configuration_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.configuration_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConfiguration_declaration([NotNull] vhdlParser.Configuration_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.logical_name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLogical_name([NotNull] vhdlParser.Logical_nameContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.logical_name"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLogical_name([NotNull] vhdlParser.Logical_nameContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.procedural_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcedural_declarative_part([NotNull] vhdlParser.Procedural_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.procedural_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcedural_declarative_part([NotNull] vhdlParser.Procedural_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.variable_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterVariable_declaration([NotNull] vhdlParser.Variable_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.variable_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitVariable_declaration([NotNull] vhdlParser.Variable_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.base_unit_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBase_unit_declaration([NotNull] vhdlParser.Base_unit_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.base_unit_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBase_unit_declaration([NotNull] vhdlParser.Base_unit_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.signal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSignal_declaration([NotNull] vhdlParser.Signal_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.signal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSignal_declaration([NotNull] vhdlParser.Signal_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simple_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimple_expression([NotNull] vhdlParser.Simple_expressionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simple_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimple_expression([NotNull] vhdlParser.Simple_expressionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.actual_parameter_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterActual_parameter_part([NotNull] vhdlParser.Actual_parameter_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.actual_parameter_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitActual_parameter_part([NotNull] vhdlParser.Actual_parameter_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.break_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBreak_list([NotNull] vhdlParser.Break_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.break_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBreak_list([NotNull] vhdlParser.Break_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.instantiated_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInstantiated_unit([NotNull] vhdlParser.Instantiated_unitContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.instantiated_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInstantiated_unit([NotNull] vhdlParser.Instantiated_unitContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_class_entry_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_class_entry_list([NotNull] vhdlParser.Entity_class_entry_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_class_entry_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_class_entry_list([NotNull] vhdlParser.Entity_class_entry_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_terminal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_terminal_declaration([NotNull] vhdlParser.Interface_terminal_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_terminal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_terminal_declaration([NotNull] vhdlParser.Interface_terminal_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.adding_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAdding_operator([NotNull] vhdlParser.Adding_operatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.adding_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAdding_operator([NotNull] vhdlParser.Adding_operatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.use_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterUse_clause([NotNull] vhdlParser.Use_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.use_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitUse_clause([NotNull] vhdlParser.Use_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.return_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterReturn_statement([NotNull] vhdlParser.Return_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.return_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitReturn_statement([NotNull] vhdlParser.Return_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.enumeration_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEnumeration_type_definition([NotNull] vhdlParser.Enumeration_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.enumeration_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEnumeration_type_definition([NotNull] vhdlParser.Enumeration_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.port_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPort_clause([NotNull] vhdlParser.Port_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.port_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPort_clause([NotNull] vhdlParser.Port_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.constrained_array_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConstrained_array_definition([NotNull] vhdlParser.Constrained_array_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.constrained_array_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConstrained_array_definition([NotNull] vhdlParser.Constrained_array_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.index_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIndex_specification([NotNull] vhdlParser.Index_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.index_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIndex_specification([NotNull] vhdlParser.Index_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.allocator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAllocator([NotNull] vhdlParser.AllocatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.allocator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAllocator([NotNull] vhdlParser.AllocatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.record_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterRecord_nature_definition([NotNull] vhdlParser.Record_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.record_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitRecord_nature_definition([NotNull] vhdlParser.Record_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_procedural_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_procedural_statement([NotNull] vhdlParser.Simultaneous_procedural_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_procedural_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_procedural_statement([NotNull] vhdlParser.Simultaneous_procedural_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.numeric_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterNumeric_literal([NotNull] vhdlParser.Numeric_literalContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.numeric_literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitNumeric_literal([NotNull] vhdlParser.Numeric_literalContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.index_constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterIndex_constraint([NotNull] vhdlParser.Index_constraintContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.index_constraint"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitIndex_constraint([NotNull] vhdlParser.Index_constraintContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.design_file"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDesign_file([NotNull] vhdlParser.Design_fileContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.design_file"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDesign_file([NotNull] vhdlParser.Design_fileContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.break_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBreak_statement([NotNull] vhdlParser.Break_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.break_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBreak_statement([NotNull] vhdlParser.Break_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.element_subnature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterElement_subnature_definition([NotNull] vhdlParser.Element_subnature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.element_subnature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitElement_subnature_definition([NotNull] vhdlParser.Element_subnature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.exit_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterExit_statement([NotNull] vhdlParser.Exit_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.exit_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitExit_statement([NotNull] vhdlParser.Exit_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.block_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBlock_statement([NotNull] vhdlParser.Block_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.block_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBlock_statement([NotNull] vhdlParser.Block_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.actual_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterActual_designator([NotNull] vhdlParser.Actual_designatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.actual_designator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitActual_designator([NotNull] vhdlParser.Actual_designatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.group_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGroup_declaration([NotNull] vhdlParser.Group_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.group_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGroup_declaration([NotNull] vhdlParser.Group_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.opts"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterOpts([NotNull] vhdlParser.OptsContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.opts"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitOpts([NotNull] vhdlParser.OptsContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.secondary_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSecondary_unit([NotNull] vhdlParser.Secondary_unitContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.secondary_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSecondary_unit([NotNull] vhdlParser.Secondary_unitContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.generic_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGeneric_clause([NotNull] vhdlParser.Generic_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.generic_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGeneric_clause([NotNull] vhdlParser.Generic_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simple_simultaneous_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimple_simultaneous_statement([NotNull] vhdlParser.Simple_simultaneous_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simple_simultaneous_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimple_simultaneous_statement([NotNull] vhdlParser.Simple_simultaneous_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_declarative_item([NotNull] vhdlParser.Entity_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_declarative_item([NotNull] vhdlParser.Entity_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_declaration([NotNull] vhdlParser.Interface_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_declaration([NotNull] vhdlParser.Interface_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.label_colon"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLabel_colon([NotNull] vhdlParser.Label_colonContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.label_colon"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLabel_colon([NotNull] vhdlParser.Label_colonContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.alias_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAlias_indication([NotNull] vhdlParser.Alias_indicationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.alias_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAlias_indication([NotNull] vhdlParser.Alias_indicationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_declaration([NotNull] vhdlParser.Subprogram_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_declaration([NotNull] vhdlParser.Subprogram_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.free_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFree_quantity_declaration([NotNull] vhdlParser.Free_quantity_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.free_quantity_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFree_quantity_declaration([NotNull] vhdlParser.Free_quantity_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.selected_signal_assignment"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSelected_signal_assignment([NotNull] vhdlParser.Selected_signal_assignmentContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.selected_signal_assignment"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSelected_signal_assignment([NotNull] vhdlParser.Selected_signal_assignmentContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterType_definition([NotNull] vhdlParser.Type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitType_definition([NotNull] vhdlParser.Type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.primary"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPrimary([NotNull] vhdlParser.PrimaryContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.primary"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPrimary([NotNull] vhdlParser.PrimaryContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.simultaneous_if_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSimultaneous_if_statement([NotNull] vhdlParser.Simultaneous_if_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.simultaneous_if_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSimultaneous_if_statement([NotNull] vhdlParser.Simultaneous_if_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.disconnection_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDisconnection_specification([NotNull] vhdlParser.Disconnection_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.disconnection_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDisconnection_specification([NotNull] vhdlParser.Disconnection_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.library_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLibrary_clause([NotNull] vhdlParser.Library_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.library_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLibrary_clause([NotNull] vhdlParser.Library_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.architecture_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArchitecture_declarative_part([NotNull] vhdlParser.Architecture_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.architecture_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArchitecture_declarative_part([NotNull] vhdlParser.Architecture_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.condition_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterCondition_clause([NotNull] vhdlParser.Condition_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.condition_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitCondition_clause([NotNull] vhdlParser.Condition_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.selected_waveforms"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSelected_waveforms([NotNull] vhdlParser.Selected_waveformsContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.selected_waveforms"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSelected_waveforms([NotNull] vhdlParser.Selected_waveformsContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.qualified_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterQualified_expression([NotNull] vhdlParser.Qualified_expressionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.qualified_expression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitQualified_expression([NotNull] vhdlParser.Qualified_expressionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.concurrent_signal_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConcurrent_signal_assignment_statement([NotNull] vhdlParser.Concurrent_signal_assignment_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.concurrent_signal_assignment_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConcurrent_signal_assignment_statement([NotNull] vhdlParser.Concurrent_signal_assignment_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.terminal_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTerminal_aspect([NotNull] vhdlParser.Terminal_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.terminal_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTerminal_aspect([NotNull] vhdlParser.Terminal_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.package_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterPackage_declarative_item([NotNull] vhdlParser.Package_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.package_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitPackage_declarative_item([NotNull] vhdlParser.Package_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.library_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLibrary_unit([NotNull] vhdlParser.Library_unitContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.library_unit"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLibrary_unit([NotNull] vhdlParser.Library_unitContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.context_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterContext_clause([NotNull] vhdlParser.Context_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.context_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitContext_clause([NotNull] vhdlParser.Context_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.shift_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterShift_operator([NotNull] vhdlParser.Shift_operatorContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.shift_operator"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitShift_operator([NotNull] vhdlParser.Shift_operatorContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.sequence_of_statements"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSequence_of_statements([NotNull] vhdlParser.Sequence_of_statementsContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.sequence_of_statements"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSequence_of_statements([NotNull] vhdlParser.Sequence_of_statementsContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_declarative_part([NotNull] vhdlParser.Subprogram_declarative_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_declarative_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_declarative_part([NotNull] vhdlParser.Subprogram_declarative_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subnature_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubnature_indication([NotNull] vhdlParser.Subnature_indicationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subnature_indication"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubnature_indication([NotNull] vhdlParser.Subnature_indicationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.element_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterElement_declaration([NotNull] vhdlParser.Element_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.element_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitElement_declaration([NotNull] vhdlParser.Element_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.attribute_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAttribute_specification([NotNull] vhdlParser.Attribute_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.attribute_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAttribute_specification([NotNull] vhdlParser.Attribute_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.generic_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGeneric_list([NotNull] vhdlParser.Generic_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.generic_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGeneric_list([NotNull] vhdlParser.Generic_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.concurrent_assertion_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConcurrent_assertion_statement([NotNull] vhdlParser.Concurrent_assertion_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.concurrent_assertion_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConcurrent_assertion_statement([NotNull] vhdlParser.Concurrent_assertion_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_class"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_class([NotNull] vhdlParser.Entity_classContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_class"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_class([NotNull] vhdlParser.Entity_classContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.across_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAcross_aspect([NotNull] vhdlParser.Across_aspectContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.across_aspect"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAcross_aspect([NotNull] vhdlParser.Across_aspectContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.configuration_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterConfiguration_declarative_item([NotNull] vhdlParser.Configuration_declarative_itemContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.configuration_declarative_item"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitConfiguration_declarative_item([NotNull] vhdlParser.Configuration_declarative_itemContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.scalar_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterScalar_nature_definition([NotNull] vhdlParser.Scalar_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.scalar_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitScalar_nature_definition([NotNull] vhdlParser.Scalar_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.file_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFile_type_definition([NotNull] vhdlParser.File_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.file_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFile_type_definition([NotNull] vhdlParser.File_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.generation_scheme"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGeneration_scheme([NotNull] vhdlParser.Generation_schemeContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.generation_scheme"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGeneration_scheme([NotNull] vhdlParser.Generation_schemeContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.nature_element_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterNature_element_declaration([NotNull] vhdlParser.Nature_element_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.nature_element_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitNature_element_declaration([NotNull] vhdlParser.Nature_element_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.direction"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterDirection([NotNull] vhdlParser.DirectionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.direction"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitDirection([NotNull] vhdlParser.DirectionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.wait_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterWait_statement([NotNull] vhdlParser.Wait_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.wait_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitWait_statement([NotNull] vhdlParser.Wait_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.formal_parameter_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFormal_parameter_list([NotNull] vhdlParser.Formal_parameter_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.formal_parameter_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFormal_parameter_list([NotNull] vhdlParser.Formal_parameter_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.loop_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLoop_statement([NotNull] vhdlParser.Loop_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.loop_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLoop_statement([NotNull] vhdlParser.Loop_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.actual_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterActual_part([NotNull] vhdlParser.Actual_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.actual_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitActual_part([NotNull] vhdlParser.Actual_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_statement_part([NotNull] vhdlParser.Entity_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_statement_part([NotNull] vhdlParser.Entity_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.array_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterArray_nature_definition([NotNull] vhdlParser.Array_nature_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.array_nature_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitArray_nature_definition([NotNull] vhdlParser.Array_nature_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.break_selector_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterBreak_selector_clause([NotNull] vhdlParser.Break_selector_clauseContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.break_selector_clause"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitBreak_selector_clause([NotNull] vhdlParser.Break_selector_clauseContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.file_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFile_declaration([NotNull] vhdlParser.File_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.file_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFile_declaration([NotNull] vhdlParser.File_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_signal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_signal_declaration([NotNull] vhdlParser.Interface_signal_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_signal_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_signal_declaration([NotNull] vhdlParser.Interface_signal_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.access_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAccess_type_definition([NotNull] vhdlParser.Access_type_definitionContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.access_type_definition"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAccess_type_definition([NotNull] vhdlParser.Access_type_definitionContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.report_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterReport_statement([NotNull] vhdlParser.Report_statementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.report_statement"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitReport_statement([NotNull] vhdlParser.Report_statementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.procedure_call"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcedure_call([NotNull] vhdlParser.Procedure_callContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.procedure_call"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcedure_call([NotNull] vhdlParser.Procedure_callContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.file_open_information"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterFile_open_information([NotNull] vhdlParser.File_open_informationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.file_open_information"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitFile_open_information([NotNull] vhdlParser.File_open_informationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.entity_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterEntity_specification([NotNull] vhdlParser.Entity_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.entity_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitEntity_specification([NotNull] vhdlParser.Entity_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.interface_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterInterface_list([NotNull] vhdlParser.Interface_listContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.interface_list"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitInterface_list([NotNull] vhdlParser.Interface_listContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.process_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterProcess_statement_part([NotNull] vhdlParser.Process_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.process_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitProcess_statement_part([NotNull] vhdlParser.Process_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.waveform_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterWaveform_element([NotNull] vhdlParser.Waveform_elementContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.waveform_element"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitWaveform_element([NotNull] vhdlParser.Waveform_elementContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.subprogram_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSubprogram_statement_part([NotNull] vhdlParser.Subprogram_statement_partContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.subprogram_statement_part"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSubprogram_statement_part([NotNull] vhdlParser.Subprogram_statement_partContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.suffix"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterSuffix([NotNull] vhdlParser.SuffixContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.suffix"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitSuffix([NotNull] vhdlParser.SuffixContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.type_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterType_declaration([NotNull] vhdlParser.Type_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.type_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitType_declaration([NotNull] vhdlParser.Type_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.attribute_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterAttribute_declaration([NotNull] vhdlParser.Attribute_declarationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.attribute_declaration"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitAttribute_declaration([NotNull] vhdlParser.Attribute_declarationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.term"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTerm([NotNull] vhdlParser.TermContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.term"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTerm([NotNull] vhdlParser.TermContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.guarded_signal_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterGuarded_signal_specification([NotNull] vhdlParser.Guarded_signal_specificationContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.guarded_signal_specification"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitGuarded_signal_specification([NotNull] vhdlParser.Guarded_signal_specificationContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.target"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTarget([NotNull] vhdlParser.TargetContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.target"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTarget([NotNull] vhdlParser.TargetContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="vhdlParser.literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterLiteral([NotNull] vhdlParser.LiteralContext context) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="vhdlParser.literal"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitLiteral([NotNull] vhdlParser.LiteralContext context) { }

        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public override void EnterEveryRule([NotNull] ParserRuleContext context) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public override void ExitEveryRule([NotNull] ParserRuleContext context) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public override void VisitTerminal([NotNull] ITerminalNode node) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public override void VisitErrorNode([NotNull] IErrorNode node) { }
	}
}
