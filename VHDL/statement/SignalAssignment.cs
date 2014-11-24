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

namespace VHDL.statement
{
    using DelayMechanism = VHDL.DelayMechanism;
    using WaveformElement = VHDL.WaveformElement;
    using Expression = VHDL.expression.Expression;
    using SignalAssignmentTarget = VHDL.Object.ISignalAssignmentTarget;

    /// <summary>
    ///  Signal assignment.
    /// 
    ///  Signal target = new Signal("TARGET", Standard.BIT);
    ///  SignalAssignment assignment = new SignalAssignment(target, Standard.BIT_0);
    ///  ---
    ///  TARGET <= '0';
    /// </summary>
    [Serializable]
    public class SignalAssignment : SequentialStatement
    {
        private SignalAssignmentTarget target;
        private readonly List<WaveformElement> waveform = new List<WaveformElement>();
        private DelayMechanism delayMechanism;

        /// <summary>
        /// Creates a signal assignment.
        /// </summary>
        /// <param name="target">the signal assignement target</param>
        /// <param name="waveformElements">the waveform</param>
        public SignalAssignment(SignalAssignmentTarget target, params WaveformElement[] waveformElements)
            : this(target, new List<WaveformElement>(waveformElements))
        {
        }

        /// <summary>
        /// Creates a signal assignment.
        /// </summary>
        /// <param name="target">the signal assignement target</param>
        /// <param name="waveformElements">the waveform</param>
        public SignalAssignment(SignalAssignmentTarget target, List<WaveformElement> waveformElements)
        {
            this.target = target;
            this.waveform.AddRange(waveformElements);
            delayMechanism = VHDL.DelayMechanism.INERTIAL;

            if ((waveform.Capacity == 1) && (waveform[0].After == null))
                delayMechanism = VHDL.DelayMechanism.DUTY_CYCLE;
        }

        /// <summary>
        /// Creates a signal assignement.
        /// </summary>
        /// <param name="target">the signal assignment target</param>
        /// <param name="value">the assigned value</param>
        public SignalAssignment(SignalAssignmentTarget target, Expression @value)
        {
            this.target = target;
            this.waveform.Add(new WaveformElement(@value));
        }

        /// <summary>
        /// Returns/Sets the signal assignment target.
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
            set
            {
                if (value != null)
                    this.delayMechanism = value;
            }
        }

        /// <summary>
        /// Retutns the waveform.
        /// </summary>
        public virtual List<WaveformElement> Waveform
        {
            get { return waveform; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitSignalAssignment(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            foreach (WaveformElement el in waveform)
            {
                res.Add(el.Value);
                res.Add(el.After);
            }
            res.Add(delayMechanism);
            return res;
        }
    }
}