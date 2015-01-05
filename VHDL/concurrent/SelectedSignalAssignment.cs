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

namespace VHDL.concurrent
{
    using Choice = VHDL.Choice;
    using DelayMechanism = VHDL.DelayMechanism;
    using WaveformElement = VHDL.WaveformElement;
    using Expression = VHDL.expression.Expression;
    using SignalAssignmentTarget = VHDL.Object.ISignalAssignmentTarget;

    /// <summary>
    /// Selected signal assignment.
    /// </summary>
    [Serializable]
    public class SelectedSignalAssignment : AbstractPostponableConcurrentStatement
    {
        private Expression expression;
        private SignalAssignmentTarget target;
        private bool guarded;
        private DelayMechanism delayMechanism;
        private readonly List<SelectedWaveform> selectedWaveforms = new List<SelectedWaveform>();

        /// <summary>
        /// Creates a selected signal assignment.
        /// </summary>
        /// <param name="expression">the assigned expression</param>
        /// <param name="target">the assignment target</param>
        public SelectedSignalAssignment(Expression expression, SignalAssignmentTarget target)
            : this(expression, target, new List<SelectedWaveform>())
        {
        }

        /// <summary>
        /// Creates a selected signal assignment.
        /// </summary>
        /// <param name="expression">the assigned expression</param>
        /// <param name="target">the assignment target</param>
        public SelectedSignalAssignment(Expression expression, SignalAssignmentTarget target, List<SelectedWaveform> selectedWaveforms)
        {
            this.expression = expression;
            this.target = target;
            this.selectedWaveforms = selectedWaveforms;
        }

        /// <summary>
        /// Returns/Sets the assigned expression.
        /// </summary>
        public virtual Expression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// Returns/Sets the target of this selected signal assignment.
        /// </summary>
        public virtual SignalAssignmentTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Returns/Sets the delay mechanism.
        /// </summary>
        public virtual DelayMechanism DelayMechanism
        {
            get { return delayMechanism; }
            set { delayMechanism = value; }
        }

        /// <summary>
        /// Returns/Sets if this selected signal assignement is guarded.
        /// </summary>
        public virtual bool Guarded
        {
            get { return guarded; }
            set { guarded = value; }
        }

        /// <summary>
        /// Returns the selected waveforms.
        /// </summary>
        public virtual List<SelectedWaveform> SelectedWaveforms
        {
            get { return selectedWaveforms; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitSelectedSignalAssignment(this);
        }

        /// <summary>
        /// Selected waveform.
        /// </summary>
        public class SelectedWaveform : VhdlElement
        {
            private readonly List<WaveformElement> waveform;
            private readonly List<Choice> choices;

            /// <summary>
            /// Creates a selected waveform.
            /// </summary>
            /// <param name="waveform">the waveform</param>
            /// <param name="choices">the choices</param>
            public SelectedWaveform(Expression waveform, params Choice[] choices)
                : this(waveform, new List<Choice>(choices))
            {
            }

            /// <summary>
            /// Creates a selected waveform with a list of choices.
            /// </summary>
            /// <param name="waveform">the waveform</param>
            /// <param name="choices">a list of choices</param>
            public SelectedWaveform(Expression waveform, List<Choice> choices)
                : this(new List<WaveformElement>(new WaveformElement[] { new WaveformElement(waveform) }), choices)
            {
            }

            /// <summary>
            /// Creates a selected waveform with a list of waveform element and choices.
            /// </summary>
            /// <param name="waveform">a list of waveform elements</param>
            /// <param name="choices">a list of choices</param>
            public SelectedWaveform(List<WaveformElement> waveform, List<Choice> choices)
            {
                this.waveform = new List<WaveformElement>(waveform);
                this.choices = new List<Choice>(choices);
            }

            /// <summary>
            /// Returns the waveform.
            /// </summary>
            public virtual List<WaveformElement> Waveform
            {
                get { return waveform; }
            }

            /// <summary>
            /// Returns the choices.
            /// </summary>
            public virtual List<Choice> Choices
            {
                get { return choices; }
            }
        }
    }

}