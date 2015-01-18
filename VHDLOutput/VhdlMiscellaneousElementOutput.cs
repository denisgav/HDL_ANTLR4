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
    using AssociationElement = VHDL.AssociationElement;
    using ComponentSpecification = VHDL.ComponentSpecification;
    using DelayMechanism = VHDL.DelayMechanism;
    using DiscreteRange = VHDL.DiscreteRange;
    using Range = VHDL.Range;
    using RangeAttributeName = VHDL.RangeAttributeName;
    using Signature = VHDL.Signature;
    using SubtypeDiscreteRange = VHDL.SubtypeDiscreteRange;
    using WaveformElement = VHDL.WaveformElement;
    using InterfaceDeclarationFormat = VHDL.annotation.InterfaceDeclarationFormat;
    using Subtype = VHDL.declaration.Subtype;
    using Constant = VHDL.Object.Constant;
    using Signal = VHDL.Object.Signal;
    using VhdlObject = VHDL.Object.VhdlObject;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using RangeSubtypeIndication = VHDL.type.RangeSubtypeIndication;
    using ResolvedSubtypeIndication = VHDL.type.ResolvedSubtypeIndication;
    using Type = VHDL.type.Type;
    using UnresolvedType = VHDL.type.UnresolvedType;
    using VHDL.Object;
    using VHDL.type;

    /// <summary>
    /// VHDL output module for elements that are not handled by a visitor.
    /// </summary>
    internal class VhdlMiscellaneousElementOutput : IMiscellaneousElementOutput
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlMiscellaneousElementOutput(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public void delayMechanism(DelayMechanism delayMechanism)
        {
            if (delayMechanism == DelayMechanism.INERTIAL)
            {
                writer.Append(KeywordEnum.INERTIAL.ToString());
            }
            else if (delayMechanism == DelayMechanism.TRANSPORT)
            {
                writer.Append(KeywordEnum.TRANSPORT.ToString());
            }
            else
            {
                writer.Append(KeywordEnum.REJECT.ToString()).Append(' ');
                output.writeExpression(delayMechanism.PulseRejectionLimit);
                writer.Append(' ').Append(KeywordEnum.INERTIAL.ToString());
            }
        }

        private void associationElement(AssociationElement element)
        {
            if (element.Formal != null)
            {
                writer.Append(element.Formal);
                writer.Align();
                writer.Append(" => ");
            }
            if (element.Actual == null)
            {
                writer.Append(KeywordEnum.OPEN.ToString());
            }
            else
            {
                output.writeExpression(element.Actual);
            }
        }

        private void associationList(List<AssociationElement> associationList, bool addLineBreaks)
        {
            bool first = true;
            foreach (AssociationElement element in associationList)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(',');
                    if (addLineBreaks)
                    {
                        writer.NewLine();
                    }
                    else
                    {
                        writer.Append(' ');
                    }
                }
                VhdlOutputHelper.handleAnnotationsBefore(element, writer);
                associationElement(element);
                VhdlOutputHelper.handleAnnotationsAfter(element, writer);
            }
            if (addLineBreaks)
            {
                writer.NewLine();
            }
        }

        private void writeInterfaceList<T1>(IList<T1> list, bool isGeneric) where T1 : VhdlObjectProvider
        {
            writer.BeginAlign();

            bool first = true;
            foreach (VhdlObjectProvider objectProvider in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(";").NewLine();
                }

                if (objectProvider is VhdlObjectGroup<VhdlObject>)
                {
                    VhdlObjectGroup<VhdlObject> group = (VhdlObjectGroup<VhdlObject>)objectProvider;
                    VhdlOutputHelper.handleAnnotationsBefore(group, writer);
                }

                //TODO: check for equal types etc.
                VhdlObject obj0 = objectProvider.VhdlObjects[0];

                InterfaceDeclarationFormat format = Annotations.getAnnotation<InterfaceDeclarationFormat>(obj0);

                if (format != null && format.UseObjectClass)
                {
                    writer.Append(obj0.ObjectClass.ToString()).Append(' ');
                }

                writer.AppendIdentifiers(objectProvider.VhdlObjects, ", ");
                writer.Align();

                writer.Append(" : ");

                if (format != null)
                {
                    if (format.UseMode || (obj0.Mode != VhdlObject.ModeEnum.IN))
                    {
                        writer.Append(obj0.Mode.ToString()).Append(' ');
                    }
                }
                else
                {
                    if (!isGeneric || obj0.Mode != VhdlObject.ModeEnum.IN)
                    {
                        writer.Append(obj0.Mode.ToString()).Append(' ');
                    }
                }

                VhdlObjectOutputHelper.interfaceSuffix(obj0, writer, output);
            }

            writer.NewLine().EndAlign();
        }

        public void generic(IList<VhdlObjectProvider> generic)
        {
            writer.Append(KeywordEnum.GENERIC.ToString()).Append(" (").NewLine().Indent();
            writeInterfaceList(generic, true);
            writer.Dedent().Append(");").NewLine();
        }

        public void port(IList<VhdlObjectProvider> port)
        {
            writer.Append(KeywordEnum.PORT.ToString()).Append(" (").NewLine().Indent();
            writeInterfaceList(port, false);
            writer.Dedent().Append(");").NewLine();
        }

        public void genericMap(List<AssociationElement> genericMap)
        {
            associationList(genericMap, true);
        }

        public void portMap(List<AssociationElement> genericMap)
        {
            associationList(genericMap, true);
        }

        public void procedureCallParameters(List<AssociationElement> parameters)
        {
            associationList(parameters, false);
        }

        public void concurrentProcedureCallParameters(List<AssociationElement> parameters)
        {
            associationList(parameters, false);
        }

        public void functionCallParameters(List<AssociationElement> parameters)
        {
            associationList(parameters, false);
        }

        public void signature(Signature signature)
        {
            writer.Append('[');

            bool first = true;
            foreach (ISubtypeIndication type in signature.ParameterTypes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }
                output.writeSubtypeIndication(type);
            }

            if (signature.ReturnType != null)
            {
                if (signature.ParameterTypes.Count != 0)
                {
                    writer.Append(' ');
                }
                writer.Append(KeywordEnum.RETURN.ToString()).Append(' ');
                output.writeSubtypeIndication(signature.ReturnType);
            }

            writer.Append(']');
        }

        public void waveform(List<WaveformElement> waveform)
        {
            if (waveform.Count == 0)
            {
                writer.Append(KeywordEnum.UNAFFECTED.ToString());
            }
            else
            {
                bool first = true;
                foreach (WaveformElement waveformElement in waveform)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Append(", ");
                    }

                    output.writeExpression(waveformElement.Value);
                    if (waveformElement.After != null)
                    {
                        writer.Append(' ').Append(KeywordEnum.AFTER.ToString()).Append(' ');
                        output.writeExpression(waveformElement.After);
                    }
                }
            }
        }

        public void range(Range range)
        {
            output.writeExpression(range.From);
            writer.Append(' ');
            switch (range.Direction)
            {
                case Range.RangeDirection.DOWNTO:
                    writer.Append(KeywordEnum.DOWNTO.ToString());
                    break;

                case Range.RangeDirection.TO:
                    writer.Append(KeywordEnum.TO.ToString());
                    break;
            }
            writer.Append(' ');
            output.writeExpression(range.To);
        }

        public void rangeAttributeName(RangeAttributeName range)
        {
            writer.Append(range.Prefix.ToString()).Append('\'').Append(range.Type.ToString());
            if (range.Index != null)
            {
                writer.Append('(');
                output.writeExpression(range.Index);
                writer.Append(')');
            }
        }

        public void subtypeDiscreteRange(SubtypeDiscreteRange range)
        {
            output.writeSubtypeIndication(range.SubtypeIndication);
        }

        public void choiceOthers()
        {
            writer.Append(KeywordEnum.OTHERS.ToString());
        }

        public void indexSubtypeIndication(IndexSubtypeIndication indication)
        {
            if (indication.BaseType != null)
            {
                output.writeSubtypeIndication(indication.BaseType);
            }
            else
            {
                writer.Append("null");
            }
            writer.Append('(');
            bool first = true;
            foreach (DiscreteRange range in indication.Ranges)
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
            writer.Append(')');
        }

        public void rangeSubtypeIndication(RangeSubtypeIndication indication)
        {
            if (indication.BaseType != null)
            {
                output.writeSubtypeIndication(indication.BaseType);
            }
            else
            {
                writer.Append("null");
            }
            writer.Append(' ').Append(KeywordEnum.RANGE.ToString()).Append(' ');
            output.writeDiscreteRange(indication.Range);
        }

        public void resolvedSubtypeIndication(ResolvedSubtypeIndication indication)
        {
            writer.Append(indication.ResolutionFunction).Append(' ');
            if (indication.BaseType != null)
            {
                output.writeSubtypeIndication(indication.BaseType);
            }
            else
            {
                writer.Append("null");
            }
        }

        public void subtypeSubtypeIndication(Subtype subtype)
        {
            writer.AppendIdentifier(subtype);
        }

        public void typeSubtypeIndication(Type subtype)
        {
            writer.AppendIdentifier(subtype);
        }

        public void unresolvedTypeSubtypeIndication(UnresolvedType subtype)
        {
            writer.AppendIdentifier(subtype);
        }

        public void allComponentSpecification(ComponentSpecification specification)
        {
            writer.Append(KeywordEnum.ALL.ToString()).Append(" : ");
            writer.AppendIdentifier(specification.Component);
        }

        public void othersComponentSpecification(ComponentSpecification specification)
        {
            writer.Append(KeywordEnum.OTHERS.ToString()).Append(" : ");
            writer.AppendIdentifier(specification.Component);
        }

        public void instantiationListComponentSpecification(ComponentSpecification specification)
        {
            writer.AppendStrings(specification.Labels, ", ");
            writer.Append(" : ").AppendIdentifier(specification.Component);
        }
    }

}