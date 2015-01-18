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

    using ConfigurationItem = VHDL.configuration.ConfigurationItem;
    using UseClause = VHDL.libraryunit.UseClause;
    using AbstractBlockConfiguration = VHDL.configuration.AbstractBlockConfiguration;
    using ArchitectureConfiguration = VHDL.configuration.ArchitectureConfiguration;
    using BlockStatementConfiguration = VHDL.configuration.BlockStatementConfiguration;
    using ComponentConfiguration = VHDL.configuration.ComponentConfiguration;
    using ConfigurationVisitor = VHDL.configuration.ConfigurationVisitor;
    using GenerateStatementConfiguration = VHDL.configuration.GenerateStatementConfiguration;

    /// <summary>
    /// Vhdl output configuration visitor.
    /// </summary>
    internal class VhdlConfigurationVisitor : ConfigurationVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlConfigurationVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(ConfigurationItem item)
        {
            VhdlOutputHelper.handleAnnotationsBefore(item, writer);
            base.visit(item);
            VhdlOutputHelper.handleAnnotationsAfter(item, writer);
        }

        public override void visit<T1>(List<T1> items)
        {
            foreach (ConfigurationItem item in items)
            {
                visit(item);
            }
        }

        private void appendBlockConfiguration(AbstractBlockConfiguration block, string blockSpecification)
        {
            writer.Append(KeywordEnum.FOR.ToString()).Append(' ');
            writer.Append(blockSpecification);
            writer.NewLine().Indent();

            foreach (UseClause useClause in block.UseClauses)
            {
                output.writeLibraryUnit(useClause);
            }
            visit(block.ConfigurationItems);

            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(KeywordEnum.FOR.ToString()).Append(";").NewLine();
        }

        protected override void visitArchitectureConfiguration(ArchitectureConfiguration configuration)
        {
            string blockIdentifier;
            if (configuration.Architecture != null)
            {
                blockIdentifier = configuration.Architecture.Identifier;
            }
            else
            {
                blockIdentifier = "null";
            }
            appendBlockConfiguration(configuration, blockIdentifier);
        }

        protected override void visitBlockStatementConfiguration(BlockStatementConfiguration configuration)
        {
            appendBlockConfiguration(configuration, configuration.Block.Label);
        }

        protected override void visitComponentConfiguration(ComponentConfiguration configuration)
        {
            writer.Append(KeywordEnum.FOR.ToString()).Append(' ');
            output.writeComponentSpecification(configuration.ComponentSpecification);
            writer.Indent();

            bool hasBindingIndication = false;

            if (configuration.EntityAspect != null)
            {
                hasBindingIndication = true;
                writer.NewLine().Append(KeywordEnum.USE.ToString()).Append(' ');
                writer.Append(configuration.EntityAspect.ToString());
            }

            if (configuration.GenericMap.Count != 0)
            {
                hasBindingIndication = true;
                writer.NewLine();
                writer.Append(KeywordEnum.GENERIC.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().genericMap(configuration.GenericMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }

            if (configuration.PortMap.Count != 0)
            {
                hasBindingIndication = true;
                writer.NewLine();
                writer.Append(KeywordEnum.PORT.ToString()).Append(KeywordEnum.MAP.ToString()).Append(" (").NewLine();
                writer.Indent().BeginAlign();
                output.getMiscellaneousElementOutput().portMap(configuration.PortMap);
                writer.EndAlign().Dedent();
                writer.Append(")");
            }

            if (hasBindingIndication)
            {
                writer.Append(";");
            }

            writer.NewLine();

            if (configuration.BlockConfiguration != null)
            {
                visit(configuration.BlockConfiguration);
            }
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(KeywordEnum.FOR.ToString());
            writer.Append(';').NewLine();
        }

        protected override void visitGenerateStatementConfiguration(GenerateStatementConfiguration configuration)
        {
            appendBlockConfiguration(configuration, configuration.getGenerateStatement().Label);
        }
    }

}