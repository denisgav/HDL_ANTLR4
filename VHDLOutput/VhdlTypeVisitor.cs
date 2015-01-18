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

    using DiscreteRange = VHDL.DiscreteRange;
    using EnumerationLiteral = VHDL.literal.EnumerationLiteral;
    using AccessType = VHDL.type.AccessType;
    using ConstrainedArray = VHDL.type.ConstrainedArray;
    using EnumerationType = VHDL.type.EnumerationType;
    using FileType = VHDL.type.FileType;
    using IncompleteType = VHDL.type.IncompleteType;
    using IntegerType = VHDL.type.IntegerType;
    using PhysicalType = VHDL.type.PhysicalType;
    using RecordType = VHDL.type.RecordType;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using Type = VHDL.type.Type;
    using TypeVisitor = VHDL.type.TypeVisitor;
    using UnconstrainedArray = VHDL.type.UnconstrainedArray;
    using VHDL.type;

    /// <summary>
    /// VHDL output type visitor.
    /// </summary>
    internal class VhdlTypeVisitor : TypeVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlTypeVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(Type type)
        {
            VhdlOutputHelper.handleAnnotationsBefore(type, writer);
            base.visit(type);
            VhdlOutputHelper.handleAnnotationsAfter(type, writer);
        }

        private void appendTypePrefix(Type declaration)
        {
            writer.Append(KeywordEnum.TYPE.ToString()).Append(' ');
            writer.AppendIdentifier(declaration).Append(' ').Append(KeywordEnum.IS.ToString());
        }

        protected override void visitAccessType(AccessType type)
        {
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.ACCESS.ToString()).Append(' ');
            output.writeSubtypeIndication(type.DesignatedSubtype);
            writer.Append(';').NewLine();
        }

        protected override void visitConstrainedArray(ConstrainedArray type)
        {
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.ARRAY.ToString()).Append(" (");

            bool first = true;
            foreach (DiscreteRange range in type.IndexRanges)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }
                output.writeDiscreteRange(range);
            }

            writer.Append(") ").Append(KeywordEnum.OF.ToString()).Append(' ');
            output.writeSubtypeIndication(type.ElementType);
            writer.Append(';').NewLine();
        }

        protected override void visitEnumerationType(EnumerationType type)
        {
            appendTypePrefix(type);
            writer.Append(" (");

            bool first = true;
            foreach (EnumerationLiteral literal in type.Literals)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }

                writer.Append(literal.ToString());
            }

            writer.Append(");").NewLine();
        }

        protected override void visitFileType(FileType type)
        {
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.FILE.ToString()).Append(KeywordEnum.OF.ToString()).Append(' ');
            output.writeSubtypeIndication(type.ValueType);
            writer.Append(';').NewLine();
        }

        protected override void visitIncompleteType(IncompleteType type)
        {
            writer.Append(KeywordEnum.TYPE.ToString()).Append(' ');
            writer.AppendIdentifier(type);
            writer.Append(';').NewLine();
        }

        protected override void visitIntegerType(IntegerType type)
        {
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.RANGE.ToString()).Append(' ');
            output.writeDiscreteRange(type.Range);
            writer.Append(';').NewLine();
        }

        protected override void visitPhysicalType(PhysicalType type)
        {
            //TODO: implement repeated label
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.RANGE.ToString()).Append(' ');
            output.writeDiscreteRange(type.Range);
            writer.Append(' ').NewLine();
            writer.Indent().Append(KeywordEnum.UNITS.ToString()).NewLine();
            writer.Indent().Append(type.PrimaryUnit).Append(';').NewLine().BeginAlign();
            foreach (PhysicalType.Unit unit in type.Units)
            {
                writer.AppendIdentifier(unit).Align().Append(" = ");
                if (unit.Factor != null)
                {
                    output.writeExpression(unit.Factor);
                    writer.Append(' ');
                }
                writer.Append(unit.BaseUnit).Append(';').NewLine();
            }
            writer.EndAlign().Dedent();

            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.UNITS.ToString()).Append(';').Dedent().NewLine();
        }

        protected override void visitRecordType(RecordType type)
        {
            //TODO: implement repeated label
            appendTypePrefix(type);
            writer.Indent().NewLine();
            writer.Append(KeywordEnum.RECORD.ToString()).Indent().NewLine().BeginAlign();
            foreach (RecordType.ElementDeclaration element in type.Elements)
            {
                writer.AppendStrings(element.Identifiers, ", ");
                writer.Align().Append(" : ");
                output.writeSubtypeIndication(element.Type);
                writer.Append(';').NewLine();
            }
            writer.EndAlign().Dedent();
            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.RECORD.ToString());
            writer.Dedent();
            writer.Append(';').NewLine();
        }

        protected override void visitUnconstrainedArray(UnconstrainedArray type)
        {
            appendTypePrefix(type);
            writer.Append(' ').Append(KeywordEnum.ARRAY.ToString()).Append(" (");

            bool first = true;
            foreach (ISubtypeIndication subtype in type.IndexSubtypes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }
                output.writeSubtypeIndication(subtype);
                writer.Append(' ').Append(KeywordEnum.RANGE.ToString()).Append(' ').Append("<>");
            }

            writer.Append(") ").Append(KeywordEnum.OF.ToString()).Append(' ');
            output.writeSubtypeIndication(type.ElementType);
            writer.Append(';').NewLine();
        }
    }

}