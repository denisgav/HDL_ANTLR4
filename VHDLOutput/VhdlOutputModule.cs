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

namespace VHDL.output
{

    using ConcurrentStatementVisitor = VHDL.concurrent.ConcurrentStatementVisitor;
    using ConfigurationVisitor = VHDL.configuration.ConfigurationVisitor;
    using DeclarationVisitor = VHDL.declaration.DeclarationVisitor;
    using ExpressionVisitor = VHDL.expression.ExpressionVisitor;
    using LibraryUnitVisitor = VHDL.libraryunit.LibraryUnitVisitor;
    using SequentialStatementVisitor = VHDL.statement.SequentialStatementVisitor;
    using TypeVisitor = VHDL.type.TypeVisitor;

    internal class VhdlOutputModule : OutputModule
    {

        private readonly SequentialStatementVisitor sequentialStatementVisitor;
        private readonly ConcurrentStatementVisitor concurrentStatementVisitor;
        private readonly LibraryUnitVisitor libraryUnitVisitor;
        private readonly DeclarationVisitor declarationVisitor;
        private readonly ExpressionVisitor expressionVisitor;
        private readonly ConfigurationVisitor configurationVisitor;
        private readonly TypeVisitor typeVisitor;
        private readonly IMiscellaneousElementOutput miscellaneousElementOutput;

        internal VhdlOutputModule(VhdlWriter writer)
        {
            sequentialStatementVisitor = new VhdlSequentialStatementVisitor(writer, this);
            concurrentStatementVisitor = new VhdlConcurrentStatementVisitor(writer, this);
            libraryUnitVisitor = new VhdlLibraryUnitVisitor(writer, this);
            declarationVisitor = new VhdlDeclarationVisitor(writer, this);
            expressionVisitor = new VhdlExpressionVisitor(writer, this);
            configurationVisitor = new VhdlConfigurationVisitor(writer, this);
            typeVisitor = new VhdlTypeVisitor(writer, this);
            miscellaneousElementOutput = new VhdlMiscellaneousElementOutput(writer, this);
        }

        protected internal override SequentialStatementVisitor getSequentialStatementVisitor()
        {
            return sequentialStatementVisitor;
        }

        protected internal override ConcurrentStatementVisitor getConcurrentStatementVisitor()
        {
            return concurrentStatementVisitor;
        }

        protected internal override LibraryUnitVisitor getLibraryUnitVisitor()
        {
            return libraryUnitVisitor;
        }

        protected internal override DeclarationVisitor getDeclarationVisitor()
        {
            return declarationVisitor;
        }

        protected internal override ExpressionVisitor getExpressionVisitor()
        {
            return expressionVisitor;
        }

        protected internal override ConfigurationVisitor getConfigurationVisitor()
        {
            return configurationVisitor;
        }

        protected internal override TypeVisitor getTypeVisitor()
        {
            return typeVisitor;
        }

        protected internal override IMiscellaneousElementOutput getMiscellaneousElementOutput()
        {
            return miscellaneousElementOutput;
        }
    }

}