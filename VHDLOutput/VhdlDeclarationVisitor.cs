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
    using InterfaceDeclarationFormat = VHDL.annotation.InterfaceDeclarationFormat;
    using OptionalIsFormat = VHDL.annotation.OptionalIsFormat;
    using Alias = VHDL.declaration.Alias;
    using Attribute = VHDL.declaration.Attribute;
    using AttributeSpecification = VHDL.declaration.AttributeSpecification;
    using Component = VHDL.declaration.Component;
    using ConfigurationSpecification = VHDL.declaration.ConfigurationSpecification;
    using ConstantDeclaration = VHDL.declaration.ConstantDeclaration;
    using DeclarationVisitor = VHDL.declaration.DeclarationVisitor;
    using DeclarativeItem = VHDL.declaration.DeclarativeItem;
    using DisconnectionSpecification = VHDL.declaration.DisconnectionSpecification;
    using FileDeclaration = VHDL.declaration.FileDeclaration;
    using FunctionBody = VHDL.declaration.FunctionBody;
    using FunctionDeclaration = VHDL.declaration.FunctionDeclaration;
    using Group = VHDL.declaration.Group;
    using GroupTemplate = VHDL.declaration.GroupTemplate;
    using ProcedureBody = VHDL.declaration.ProcedureBody;
    using ProcedureDeclaration = VHDL.declaration.ProcedureDeclaration;
    using SignalDeclaration = VHDL.declaration.SignalDeclaration;
    using Subtype = VHDL.declaration.Subtype;
    using VariableDeclaration = VHDL.declaration.VariableDeclaration;
    using Constant = VHDL.Object.Constant;
    using FileObject = VHDL.Object.FileObject;
    using Signal = VHDL.Object.Signal;
    using Variable = VHDL.Object.Variable;
    using VhdlObject = VHDL.Object.VhdlObject;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;

    /// <summary>
    /// Declaration output visitor.
    /// </summary>
    internal class VhdlDeclarationVisitor : DeclarationVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlDeclarationVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(DeclarativeItem declaration)
        {
            VhdlOutputHelper.handleAnnotationsBefore(declaration, writer);
            base.visit(declaration);
            VhdlOutputHelper.handleAnnotationsAfter(declaration, writer);
        }

        public override void visit<T1>(List<T1> declarations)
        {
            foreach (DeclarativeItem declaration in declarations)
            {
                visit(declaration);
            }
        }

        private bool isOutputMode(VhdlObject @object)
        {
            if (@object.Mode == VhdlObject.ModeEnum.NONE)
            {
                return false;
            }

            InterfaceDeclarationFormat format = Annotations.getAnnotation<InterfaceDeclarationFormat>(@object);

            if (format != null && format.UseMode)
            {
                return true;
            }

            return @object.Mode != VhdlObject.ModeEnum.IN;
        }

        private bool isOutputObjectClassProcedure(VhdlObject @object)
        {
            InterfaceDeclarationFormat format = Annotations.getAnnotation<InterfaceDeclarationFormat>(@object);

            if (format != null && format.UseObjectClass)
            {
                return true;
            }

            switch (@object.ObjectClass)
            {
                case VhdlObject.ObjectClassEnum.CONSTANT:
                    return false;

                case VhdlObject.ObjectClassEnum.VARIABLE:
                    return (@object.Mode != Variable.ModeEnum.INOUT && @object.Mode != Variable.ModeEnum.OUT);

                default:
                    return true;
            }
        }

        private bool isOutputObjectClassFunction(VhdlObject @object)
        {
            InterfaceDeclarationFormat format = Annotations.getAnnotation<InterfaceDeclarationFormat>(@object);

            if (format != null && format.UseObjectClass)
            {
                return true;
            }

            return @object.ObjectClass != VhdlObject.ObjectClassEnum.CONSTANT;
        }

        private void appendProcedureParameters(IList<VhdlObjectProvider> parameters)
        {
            if (parameters.Count != 0)
            {
                writer.Append(" (");
                bool first = true;
                foreach (var provider in parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append("; ");
                    }

                    VhdlObject object0 = provider.VhdlObjects[0];

                    if (isOutputObjectClassProcedure(object0))
                    {
                        writer.Append(object0.ObjectClass.ToString()).Append(' ');
                    }

                    writer.AppendIdentifiers(provider.VhdlObjects, ", ");

                    writer.Append(" : ");

                    if (isOutputMode(object0))
                    {
                        writer.Append(object0.Mode.ToString()).Append(' ');
                    }

                    VhdlObjectOutputHelper.interfaceSuffix(object0, writer, output);
                }
                writer.Append(")");
            }
        }

        private void appendFunctionParameters(IList<VhdlObjectProvider> parameters)
        {
            if (parameters.Count != 0)
            {
                writer.Append(" (");
                bool first = true;
                foreach (var provider in parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append("; ");
                    }

                    VhdlObject object0 = provider.VhdlObjects[0];

                    //don't add type if object is a constant
                    if (isOutputObjectClassFunction(object0))
                    {
                        writer.Append(object0.ObjectClass.ToString()).Append(' ');
                    }

                    writer.AppendIdentifiers(provider.VhdlObjects, ", ");

                    writer.Append(" : ");

                    if (isOutputMode(object0))
                    {
                        writer.Append(object0.Mode.ToString()).Append(' ');
                    }

                    VhdlObjectOutputHelper.interfaceSuffix(object0, writer, output);
                }
                writer.Append(")");
            }
        }

        protected override void visitAliasDeclaration(Alias declaration)
        {
            writer.Append(KeywordEnum.ALIAS.ToString()).Append(' ').Append(declaration.Designator);
            if (declaration.SubtypeIndication != null)
            {
                writer.Append(" : ");
                output.writeSubtypeIndication(declaration.SubtypeIndication);
            }
            writer.Append(' ').Append(KeywordEnum.IS.ToString()).Append(' ').Append(declaration.Aliased.ToString());
            if (declaration.Signature != null)
            {
                writer.Append(' ');
                output.getMiscellaneousElementOutput().signature(declaration.Signature);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitAttributeDeclaration(Attribute declaration)
        {
            writer.Append(KeywordEnum.ATTRIBUTE.ToString()).Append(' ');
            writer.AppendIdentifier(declaration);
            writer.Append(" : ");
            output.writeSubtypeIndication(declaration.Type);
            writer.Append(';').NewLine();
        }

        private void appendEntityDesignator(AttributeSpecification.EntityNameList.EntityDesignator designator)
        {
            writer.Append(designator.EntityTag);
            if (designator.Signature != null)
            {
                writer.Append(' ');
                output.getMiscellaneousElementOutput().signature(designator.Signature);
            }
        }

        protected override void visitAttributeSpecification(AttributeSpecification specification)
        {
            writer.Append(KeywordEnum.ATTRIBUTE.ToString()).Append(' ');
            writer.AppendIdentifier(specification.Attribute).Append(' ');
            writer.Append(KeywordEnum.OF.ToString()).Append(' ');
            AttributeSpecification.EntityNameList entityNames = specification.Entities;
            if (entityNames == AttributeSpecification.EntityNameList.ALL)
            {
                writer.Append(KeywordEnum.ALL.ToString());
            }
            else if (entityNames == AttributeSpecification.EntityNameList.OTHERS)
            {
                writer.Append(KeywordEnum.OTHERS.ToString());
            }
            else
            {
                bool first = true;
                foreach (AttributeSpecification.EntityNameList.EntityDesignator designator in entityNames.Designators)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append(", ");
                    }
                    appendEntityDesignator(designator);
                }
            }
            writer.Append(" : ").Append(specification.EntityClass.ToString()).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).Append(' ');
            output.writeExpression(specification.Value);
            writer.Append(';').NewLine();
        }

        protected override void visitComponentDeclaration(Component declaration)
        {
            writer.Append(KeywordEnum.COMPONENT.ToString()).Append(' ');
            writer.AppendIdentifier(declaration);

            OptionalIsFormat format = Annotations.getAnnotation<OptionalIsFormat>(declaration);
            if (format != null && format.UseIs)
            {
                writer.Append(' ').Append(KeywordEnum.IS.ToString());
            }

            writer.Indent().NewLine();
            if (declaration.Generic.Count != 0)
            {
                output.getMiscellaneousElementOutput().generic(declaration.Generic);
            }
            if (declaration.Port.Count != 0)
            {
                output.getMiscellaneousElementOutput().port(declaration.Port);
            }
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(KeywordEnum.COMPONENT.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').AppendIdentifier(declaration);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitConfigurationSpecification(ConfigurationSpecification specification)
        {
            writer.Append(KeywordEnum.FOR.ToString()).Append(' ');
            output.writeComponentSpecification(specification.ComponentSpecification);

            if (specification.EntityAspect != null)
            {
                writer.Append(' ').Append(KeywordEnum.USE.ToString()).Append(' ');
                writer.Append(specification.EntityAspect.ToString());
            }

            if (specification.GenericMap.Count != 0)
            {
                writer.NewLine();
                writer.Append(KeywordEnum.GENERIC.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().genericMap(specification.GenericMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }

            if (specification.PortMap.Count != 0)
            {
                writer.NewLine();
                writer.Append(KeywordEnum.PORT.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().portMap(specification.PortMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }

            writer.Append(';').NewLine();
        }

        //TODO: check if default values are equal
        //TODO: check if types match
        protected override void visitConstantDeclaration(ConstantDeclaration declaration)
        {
            Constant constant = declaration.Objects[0];

            writer.Append(KeywordEnum.CONSTANT.ToString()).Append(' ');
            writer.AppendIdentifiers(declaration.Objects, ", ");
            writer.Append(" : ");
            output.writeSubtypeIndication(constant.Type);
            if (constant.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(constant.DefaultValue);
            }
            writer.Append(";").NewLine();
        }

        private void appendSignalList(DisconnectionSpecification.SignalList signals)
        {
            if (signals == DisconnectionSpecification.SignalList.ALL)
            {
                writer.Append(KeywordEnum.ALL.ToString());
            }
            else if (signals == DisconnectionSpecification.SignalList.OTHERS)
            {
                writer.Append(KeywordEnum.OTHERS.ToString());
            }
            else
            {
                bool first = true;
                foreach (Signal signal in signals.Signals)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append(", ");
                    }
                    writer.AppendIdentifier(signal);
                }
            }
        }

        protected override void visitDisconnectionSpecification(DisconnectionSpecification specification)
        {
            writer.Append(KeywordEnum.DISCONNECT.ToString()).Append(' ');
            appendSignalList(specification.Signals);
            writer.Append(" : ");
            output.writeSubtypeIndication(specification.Type);
            writer.Append(' ').Append(KeywordEnum.AFTER.ToString()).Append(' ');
            output.writeExpression(specification.After);
            writer.Append(';').NewLine();
        }

        //TODO: check if types match
        protected override void visitFileDeclaration(FileDeclaration declaration)
        {
            FileObject file = declaration.Objects[0];

            writer.Append(KeywordEnum.FILE.ToString()).Append(' ');
            writer.AppendIdentifiers(declaration.Objects, ", ");
            writer.Append(" : ");
            output.writeSubtypeIndication(file.Type);
            if (file.OpenKind != null)
            {
                writer.Append(' ').Append(KeywordEnum.OPEN.ToString()).Append(' ');
                output.writeExpression(file.OpenKind);
            }
            if (file.LogicalName != null)
            {
                writer.Append(' ').Append(KeywordEnum.IS.ToString()).Append(' ');
                output.writeExpression(file.LogicalName);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitFunctionBody(FunctionBody declaration)
        {
            if (declaration.Impure)
            {
                writer.Append(KeywordEnum.IMPURE.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.FUNCTION.ToString()).Append(' ').AppendIdentifier(declaration);
            appendFunctionParameters(declaration.Parameters);
            writer.Append(' ').Append(KeywordEnum.RETURN.ToString()).Append(' ');
            output.writeSubtypeIndication(declaration.ReturnType);
            writer.Append(' ').Append(KeywordEnum.IS.ToString()).NewLine().Indent();
            output.writeDeclarationMarkers(declaration.Declarations);
            writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine().Indent();
            output.writeSequentialStatements(declaration.Statements);

            //TODO: add repeated label
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(";").NewLine();
        }

        protected override void visitFunctionDeclaration(FunctionDeclaration declaration)
        {
            if (declaration.Impure)
            {
                writer.Append(KeywordEnum.IMPURE.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.FUNCTION.ToString()).Append(' ').AppendIdentifier(declaration);
            appendFunctionParameters(declaration.Parameters);
            writer.Append(' ').Append(KeywordEnum.RETURN.ToString()).Append(' ');
            output.writeSubtypeIndication(declaration.ReturnType);
            writer.Append(";").NewLine();
        }

        protected override void visitGroupDeclaration(Group declaration)
        {
            writer.Append(KeywordEnum.GROUP.ToString()).Append(' ');
            writer.AppendIdentifier(declaration).Append(" : ");
            writer.AppendIdentifier(declaration.Template).Append(" (");
            writer.AppendStrings(declaration.Constituents, ", ");
            writer.Append(");").NewLine();
        }

        protected override void visitGroupTemplateDeclaration(GroupTemplate declaration)
        {
            writer.Append(KeywordEnum.GROUP.ToString()).Append(' ');
            writer.AppendIdentifier(declaration).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).Append(" (");
            writer.AppendOutputEnums(declaration.EntityClasses, ", ");
            if (declaration.RepeatLast)
            {
                writer.Append(" <>");
            }
            writer.Append(");").NewLine();
        }

        protected override void visitProcedureBody(ProcedureBody declaration)
        {
            writer.Append(KeywordEnum.PROCEDURE.ToString()).Append(' ').AppendIdentifier(declaration);
            appendProcedureParameters(declaration.Parameters);
            writer.Append(' ').Append(KeywordEnum.IS.ToString()).NewLine().Indent();
            output.writeDeclarationMarkers(declaration.Declarations);
            writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine().Indent();
            output.writeSequentialStatements(declaration.Statements);

            //TODO: add repeated label
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(";").NewLine();
        }

        protected override void visitProcedureDeclaration(ProcedureDeclaration declaration)
        {
            writer.Append(KeywordEnum.PROCEDURE.ToString()).Append(' ').AppendIdentifier(declaration);
            appendProcedureParameters(declaration.Parameters);
            writer.Append(";").NewLine();
        }

        //TODO: check if default values are equal
        //TODO: check if types match
        protected override void visitSignalDeclaration(SignalDeclaration declaration)
        {
            Signal signal = declaration.Objects[0];

            writer.Append(KeywordEnum.SIGNAL.ToString()).Append(' ');
            writer.AppendIdentifiers(declaration.Objects, ", ");
            writer.Append(" : ");
            output.writeSubtypeIndication(signal.Type);
            if (signal.Kind != Signal.KindEnum.DEFAULT)
            {
                writer.Append(' ').Append(signal.Kind.ToString());
            }
            if (signal.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(signal.DefaultValue);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitSubtypeDeclaration(Subtype declaration)
        {
            writer.Append(KeywordEnum.SUBTYPE.ToString()).Append(' ');
            writer.AppendIdentifier(declaration).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).Append(' ');
            output.writeSubtypeIndication(declaration.SubtypeIndication);
            writer.Append(';').NewLine();
        }

        //TODO: check if default values are equal
        //TODO: check if shared attributes are equal
        //TODO: check if types match
        protected override void visitVariableDeclaration(VariableDeclaration declaration)
        {
            Variable variable = declaration.Objects[0];

            if (variable.Shared)
            {
                writer.Append(KeywordEnum.SHARED.ToString()).Append(' ');
            }
            writer.Append(KeywordEnum.VARIABLE.ToString()).Append(' ');
            writer.AppendIdentifiers(declaration.Objects, ", ");
            writer.Append(" : ");
            output.writeSubtypeIndication(variable.Type);
            if (variable.DefaultValue != null)
            {
                writer.Append(" := ");
                output.writeExpression(variable.DefaultValue);
            }
            writer.Append(";").NewLine();
        }
    }

}