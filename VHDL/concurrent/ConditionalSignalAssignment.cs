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
using System;
using VHDL.Object;

namespace VHDL.concurrent
{
    using DelayMechanism = VHDL.DelayMechanism;
    using WaveformElement = VHDL.WaveformElement;
    using Expression = VHDL.expression.Expression;
    using SignalAssignmentTarget = VHDL.Object.ISignalAssignmentTarget;

    /// <summary>
    /// Conditional signal assignment.
    /// </summary>
    [Serializable]
    public class ConditionalSignalAssignment : AbstractPostponableConcurrentStatement
    {
        private SignalAssignmentTarget target;
        private readonly List<ConditionalWaveformElement> conditionalWaveforms;
        private DelayMechanism delayMechanism;
        internal bool guarded;

        /// <summary>
        /// Creates a conditional signal assignment.
        /// </summary>
        /// <param name="target">the target of this signal assignment</param>
        /// <param name="conditionalWaveforms">the assigned waveform</param>
        public ConditionalSignalAssignment(SignalAssignmentTarget target, params ConditionalWaveformElement[] conditionalWaveforms)
            : this(target, new List<ConditionalWaveformElement>(conditionalWaveforms))
        {
        }

        /// <summary>
        /// Creates a conditional signal assignment.
        /// </summary>
        /// <param name="target">the target of this signal assignment</param>
        /// <param name="conditionalWaveforms">the assigned waveform</param>
        public ConditionalSignalAssignment(SignalAssignmentTarget target, List<ConditionalWaveformElement> conditionalWaveforms)
        {
            this.target = target;
            this.conditionalWaveforms = new List<ConditionalWaveformElement>(conditionalWaveforms);

            delayMechanism = DelayMechanism.INERTIAL;
        }

        /// <summary>
        /// Creates a conditional signal assignment.
        /// </summary>
        /// <param name="target">the target of this signal assignment</param>
        /// <param name="value">the assigned value</param>
        public ConditionalSignalAssignment(SignalAssignmentTarget target, Expression @value)
        {
            this.target = target;
            this.conditionalWaveforms = new List<ConditionalWaveformElement>();

            WaveformElement element = new WaveformElement(@value);
            this.conditionalWaveforms.Add(new ConditionalWaveformElement(new List<WaveformElement>(new WaveformElement[] { element })));

            delayMechanism = DelayMechanism.INERTIAL;
        }

        /// <summary>
        /// Returns/Sets the target of this conditional signal assignment.
        /// </summary>
        public virtual SignalAssignmentTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Returns the conditional waveforms.
        /// </summary>
        public virtual List<ConditionalWaveformElement> ConditionalWaveforms
        {
            get { return conditionalWaveforms; }
        }

        /// <summary>
        /// Returns the delay mechanism.
        /// </summary>
        public virtual DelayMechanism DelayMechanism
        {
            get { return delayMechanism; }
            set { delayMechanism = value; }
        }

        /// <summary>
        /// Returns/Sets if this conditional signal assignement is guarded.
        /// </summary>
        public virtual bool Guarded
        {
            get { return guarded; }
            set { guarded = value; }
        }


        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitConditionalSignalAssignment(this);
        }

        /// <summary>
        /// Conditional waveform element.
        /// </summary>
        [Serializable]
        public class ConditionalWaveformElement
        {
            private readonly List<WaveformElement> waveform;
            private Expression condition;

            /// <summary>
            /// Creates a conditional waveform element.
            /// </summary>
            /// <param name="waveform">the waveform</param>
            public ConditionalWaveformElement(List<WaveformElement> waveform)
            {
                this.waveform = new List<WaveformElement>(waveform);
            }

            /// <summary>
            /// Creates a conditional waveform element with a condition.
            /// </summary>
            /// <param name="waveform">the waveform</param>
            /// <param name="condition">the condition</param>
            public ConditionalWaveformElement(List<WaveformElement> waveform, Expression condition)
            {
                this.waveform = new List<WaveformElement>(waveform);
                this.condition = condition;
            }

            /// <summary>
            /// Returns/Sets the condition of this conditional waveform element.
            /// </summary>
            public virtual Expression Condition
            {
                get { return condition; }
                set { condition = value; }
            }

            /// <summary>
            /// Returns the list of waveform elements in this conditional waveform element.
            /// </summary>
            public virtual List<WaveformElement> Waveform
            {
                get { return waveform; }
            }
        }
    }
}