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

    using AssociationElement = VHDL.AssociationElement;
    using ComponentSpecification = VHDL.ComponentSpecification;
    using DelayMechanism = VHDL.DelayMechanism;
    using Range = VHDL.Range;
    using RangeAttributeName = VHDL.RangeAttributeName;
    using Signature = VHDL.Signature;
    using SubtypeDiscreteRange = VHDL.SubtypeDiscreteRange;
    using WaveformElement = VHDL.WaveformElement;
    using Subtype = VHDL.declaration.Subtype;
    using Constant = VHDL.Object.Constant;
    using Signal = VHDL.Object.Signal;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using RangeSubtypeIndication = VHDL.type.RangeSubtypeIndication;
    using ResolvedSubtypeIndication = VHDL.type.ResolvedSubtypeIndication;
    using Type = VHDL.type.Type;
    using UnresolvedType = VHDL.type.UnresolvedType;

    /// <summary>
    /// Output module for elements that are not handled by a visitor.
    /// </summary>
    public interface IMiscellaneousElementOutput
    {
        /// <summary>
        /// Outputs a delay mechanism.
        /// </summary>
        /// <param name="delayMechanism">the delay mechanism</param>
        void delayMechanism(DelayMechanism delayMechanism);

        /// <summary>
        /// Outputs a generic clause.
        /// </summary>
        /// <param name="generic">the generic clause</param>
        void generic(IList<VhdlObjectProvider> generic);

        /// <summary>
        /// Outputs a port clause.
        /// </summary>
        /// <param name="port">the port clause</param>
        void port(IList<VhdlObjectProvider> port);

        /// <summary>
        /// Outputs a generic map.
        /// </summary>
        /// <param name="genericMap">the generic map</param>
        void genericMap(List<AssociationElement> genericMap);

        /// <summary>
        /// Outputs a port map.
        /// </summary>
        /// <param name="portMap">the port map</param>
        void portMap(List<AssociationElement> portMap);

        /// <summary>
        /// Outputs the parameters of a procedure call.
        /// </summary>
        /// <param name="parameters">the parameters</param>
        void procedureCallParameters(List<AssociationElement> parameters);

        /// <summary>
        /// Outputs the parameters of a concurrent procedure call.
        /// </summary>
        /// <param name="parameters">the parameters</param>
        void concurrentProcedureCallParameters(List<AssociationElement> parameters);

        /// <summary>
        /// Outputs the parameters of a function call.
        /// </summary>
        /// <param name="parameters">the paramters</param>
        void functionCallParameters(List<AssociationElement> parameters);

        /// <summary>
        /// Outputs a signature.
        /// </summary>
        /// <param name="signature">the signature</param>
        void signature(Signature signature);

        /// <summary>
        /// Outputs a waveform.
        /// </summary>
        /// <param name="waveform">the waveform</param>
        void waveform(List<WaveformElement> waveform);

        /// <summary>
        /// Outputs a range.
        /// </summary>
        /// <param name="range">the range</param>
        void range(Range range);

        /// <summary>
        /// Outputs a range attribue name.
        /// </summary>
        /// <param name="range">the range attribute name</param>
        void rangeAttributeName(RangeAttributeName range);

        /// <summary>
        /// Outputs a subtype discrete range.
        /// </summary>
        /// <param name="range">the subtype discrete range</param>
        void subtypeDiscreteRange(SubtypeDiscreteRange range);

        /// <summary>
        /// Outputs a OTHERS choice.
        /// </summary>
        void choiceOthers();

        /// <summary>
        /// Outputs an index subtype indication.
        /// </summary>
        /// <param name="subtype">the index subtype indication</param>
        void indexSubtypeIndication(IndexSubtypeIndication subtype);

        /// <summary>
        /// Outputs a range subtype indication.
        /// </summary>
        /// <param name="subtype">the range subtype indication</param>
        void rangeSubtypeIndication(RangeSubtypeIndication subtype);

        /// <summary>
        /// Outputs a resolved subtype indication.
        /// </summary>
        /// <param name="subtype">the resolved subtype indication</param>
        void resolvedSubtypeIndication(ResolvedSubtypeIndication subtype);

        /// <summary>
        /// Outputs a type subtype indication.
        /// </summary>
        /// <param name="subtype">the type</param>
        void typeSubtypeIndication(Type subtype);

        /// <summary>
        /// Outputs a subtype subtype indication.
        /// </summary>
        /// <param name="subtype">the subtype</param>
        void subtypeSubtypeIndication(Subtype subtype);

        /// <summary>
        /// Outputs an unresolved type subtype indication.
        /// </summary>
        /// <param name="subtype">the unresolved type</param>
        void unresolvedTypeSubtypeIndication(UnresolvedType subtype);

        /// <summary>
        /// Outputs an ALL component specification.
        /// </summary>
        /// <param name="specification">the component specification</param>
        void allComponentSpecification(ComponentSpecification specification);

        /// <summary>
        /// Outputs an OTHERS component specification.
        /// </summary>
        /// <param name="specification">the component specification</param>
        void othersComponentSpecification(ComponentSpecification specification);

        /// <summary>
        /// Outputs an instantiation list component specification.
        /// </summary>
        /// <param name="specification">the component specification</param>
        void instantiationListComponentSpecification(ComponentSpecification specification);
    }
}