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

    using Architecture = VHDL.libraryunit.Architecture;
    using Configuration = VHDL.libraryunit.Configuration;
    using Entity = VHDL.libraryunit.Entity;
    using LibraryClause = VHDL.libraryunit.LibraryClause;
    using LibraryUnit = VHDL.libraryunit.LibraryUnit;
    using LibraryUnitVisitor = VHDL.libraryunit.LibraryUnitVisitor;
    using PackageBody = VHDL.libraryunit.PackageBody;
    using PackageDeclaration = VHDL.libraryunit.PackageDeclaration;
    using UseClause = VHDL.libraryunit.UseClause;

    /// <summary>
    /// Library unit output visitor.
    /// </summary>
    internal class VhdlLibraryUnitVisitor : LibraryUnitVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlLibraryUnitVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(LibraryUnit unit)
        {
            VhdlOutputHelper.handleAnnotationsBefore(unit, writer);
            base.visit(unit);
            VhdlOutputHelper.handleAnnotationsAfter(unit, writer);
        }

        public override void visit<T1>(IList<T1> units)
        {
            foreach (LibraryUnit unit in units)
            {
                visit(unit);
            }
        }

        protected override void visitArchitecture(Architecture architecture)
        {
            writer.Append(KeywordEnum.ARCHITECTURE.ToString()).Append(' ');
            writer.AppendIdentifier(architecture).Append(' ');
            writer.Append(KeywordEnum.OF.ToString()).Append(' ');
            writer.AppendIdentifier(architecture.Entity).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).NewLine().Indent();
            output.writeDeclarationMarkers(architecture.Declarations);
            writer.Dedent().Append(KeywordEnum.BEGIN.ToString()).NewLine().Indent();
            output.writeConcurrentStatements(architecture.Statements);
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(";").NewLine();
        }

        protected override void visitConfiguration(Configuration configuration)
        {
            writer.Append(KeywordEnum.CONFIGURATION.ToString()).Append(' ');
            writer.AppendIdentifier(configuration).Append(' ');
            writer.Append(KeywordEnum.OF.ToString()).Append(' ');
            writer.AppendIdentifier(configuration.Entity).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).NewLine();

            writer.Indent();
            output.writeDeclarationMarkers(configuration.Declarations);
            output.writeConfigurationItem(configuration.BlockConfiguration);
            writer.Dedent();

            writer.Append(KeywordEnum.END.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(KeywordEnum.CONFIGURATION.ToString());
                writer.Append(' ').AppendIdentifier(configuration.Entity);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitEntity(Entity entity)
        {
            writer.Append(KeywordEnum.ENTITY.ToString()).Append(' ');
            writer.AppendIdentifier(entity).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).NewLine();
            writer.Indent();
            if (entity.Generic.Count != 0)
            {
                output.getMiscellaneousElementOutput().generic(entity.Generic);
            }
            if (entity.Port.Count != 0)
            {
                output.getMiscellaneousElementOutput().port(entity.Port);
            }
            output.writeDeclarationMarkers(entity.Declarations);
            writer.Dedent();
            if (entity.Statements.Count != 0)
            {
                writer.Append(KeywordEnum.BEGIN.ToString()).NewLine().Indent();
                output.writeConcurrentStatements(entity.Statements);
                writer.Dedent();
            }
            writer.Append(KeywordEnum.END.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(KeywordEnum.ENTITY.ToString());
                writer.Append(' ').AppendIdentifier(entity);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitPackageBody(PackageBody packageBody)
        {
            writer.Append(KeywordEnum.PACKAGE.ToString()).Append(KeywordEnum.BODY.ToString()).Append(' ');
            writer.AppendIdentifier(packageBody.Package).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).NewLine();

            writer.Indent();
            output.writeDeclarationMarkers(packageBody.Declarations);
            writer.Dedent();

            writer.Append(KeywordEnum.END.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(KeywordEnum.PACKAGE.ToString()).Append(KeywordEnum.BODY.ToString());
                writer.Append(' ').AppendIdentifier(packageBody.Package);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitPackageDeclaration(PackageDeclaration packageDeclaration)
        {
            writer.Append(KeywordEnum.PACKAGE.ToString()).Append(' ');
            writer.AppendIdentifier(packageDeclaration).Append(' ');
            writer.Append(KeywordEnum.IS.ToString()).NewLine();

            writer.Indent();
            output.writeDeclarationMarkers(packageDeclaration.Declarations);
            writer.Dedent();

            writer.Append(KeywordEnum.END.ToString());
            if (writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(KeywordEnum.PACKAGE.ToString());
                writer.Append(' ').AppendIdentifier(packageDeclaration);
            }
            writer.Append(";").NewLine();
        }

        protected override void visitLibraryClause(LibraryClause libraryClause)
        {
            writer.Append(KeywordEnum.LIBRARY.ToString()).Append(' ');
            writer.AppendStrings(libraryClause.getLibraries(), ", ");
            writer.Append(";").NewLine();

        }

        protected override void visitUseClause(UseClause useClause)
        {
            writer.Append(KeywordEnum.USE.ToString()).Append(' ');
            writer.AppendStrings(useClause.getDeclarations(), ", ");
            writer.Append(';').NewLine();
        }
    }

}