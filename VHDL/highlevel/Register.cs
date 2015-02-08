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

using System;
using System.Collections.Generic;
using VHDL.expression;

namespace VHDL.highlevel
{
    using IfStatement = VHDL.statement.IfStatement;
    using IndexSubtypeIndication = VHDL.type.IndexSubtypeIndication;
    using SequentialStatement = VHDL.statement.SequentialStatement;
    using Signal = VHDL.Object.Signal;
    using SignalAssignment = VHDL.statement.SignalAssignment;
    using StdLogic1164 = VHDL.builtin.StdLogic1164;

    /// <summary>
    /// Register.
    /// </summary>
    [Serializable]
    public class Register : AbstractRegister
    {

        private Signal input;
        private Signal output;
        private Signal clock;
        private Signal reset;
        private Signal writeEnable;
        private Expression resetExpression;
        private ResetLevelEnum resetLevel = ResetLevelEnum.HIGH;
        private ResetTypeEnum resetType = ResetTypeEnum.ASYNCHRONOUS;

        /// <summary>
        /// Creates a register with input, output and clock.
        /// </summary>
        /// <param name="input">the input signal</param>
        /// <param name="output">the output signal</param>
        /// <param name="clock">the clock signal</param>
        public Register(Signal input, Signal output, Signal clock)
        {
            this.input = input;
            this.output = output;
            this.clock = clock;
        }

        /// <summary>
        /// Creates a named register with input, output and clock.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="input">the input signal</param>
        /// <param name="output">the output signal</param>
        /// <param name="clock">the clock signal</param>
        public Register(string identifier, Signal input, Signal output, Signal clock)
            : base(identifier)
        {
            this.input = input;
            this.output = output;
            this.clock = clock;
        }

        /// <summary>
        /// Creates a register with input, output, clock and reset.
        /// </summary>
        /// <param name="input">the input signal</param>
        /// <param name="output">the output signal<the output signal/param>
        /// <param name="clock">the clock signal</param>
        /// <param name="reset">the reset signal</param>
        public Register(Signal input, Signal output, Signal clock, Signal reset)
        {
            this.input = input;
            this.output = output;
            this.clock = clock;
            this.reset = reset;
        }

        /// <summary>
        /// Creates a named register with input, output, clock and reset.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="input">the input signal</param>
        /// <param name="output">the output signal<the output signal/param>
        /// <param name="clock">the clock signal</param>
        /// <param name="reset">the reset signal</param>
        public Register(string identifier, Signal input, Signal output, Signal clock, Signal reset)
            : base(identifier)
        {
            this.input = input;
            this.output = output;
            this.clock = clock;
            this.reset = reset;
        }

        /// <summary>
        /// Returns/Sets the clock signal.
        /// </summary>
        public virtual Signal Clock
        {
            get { return clock; }
            set { clock = value; }
        }

        /// <summary>
        /// Returns/Sets the input signal.
        /// </summary>
        public virtual Signal Input
        {
            get { return input; }
            set { input = value; }
        }

        /// <summary>
        /// Returns/Sets the output signal.
        /// </summary>
        public virtual Signal Output
        {
            get { return output; }
            set { output = value; }
        }

        /// <summary>
        /// Returns/Sets the write enable signal.
        /// </summary>
        public virtual Signal WriteEnable
        {
            get { return writeEnable; }
            set { writeEnable = value; }
        }

        /// <summary>
        /// Returns/Sets the reset signal.
        /// </summary>
        public virtual Signal Reset
        {
            get { return reset; }
            set { reset = value; }
        }

        /// <summary>
        /// Returns/Sets the reset expression.
        /// </summary>
        public virtual Expression ResetExpression
        {
            get { return resetExpression; }
            set { resetExpression = value; }
        }

        /// <summary>
        /// Returns/Sets the reset type.
        /// </summary>
        public virtual ResetTypeEnum ResetType
        {
            get { return resetType; }
            set { resetType = value; }
        }

        /// <summary>
        /// Returns/Sets the reset level.
        /// </summary>
        public virtual ResetLevelEnum ResetLevel
        {
            get { return resetLevel; }
            set { resetLevel = value; }
        }


        public override List<Signal> SensitivityList
        {
            get
            {
                if (resetExpression != null && resetType == ResetTypeEnum.ASYNCHRONOUS)
                {
                    //FIXME: Add signals in resetExpression to sensitivity list
                    return new List<Signal>(new Signal[] { clock }); //, resetExpression);
                }
                else
                {
                    return new List<Signal>(new Signal[] { clock });
                }
            }
        }

        internal override Register FirstRegister
        {
            get { return this; }
        }

        internal override void addClockAssignments(List<SequentialStatement> statements)
        {
            SequentialStatement signalAssignment = new SignalAssignment(Name.reference(output), Name.reference(input));

            if (writeEnable != null)
            {
                Expression writeEnableCondition = new Equals(Name.reference(writeEnable), StdLogic1164.STD_LOGIC_1);
                IfStatement writeEnableIf = new IfStatement(writeEnableCondition);
                writeEnableIf.Statements.Add(signalAssignment);
                statements.Add(writeEnableIf);
            }
            else
            {
                statements.Add(signalAssignment);
            }
        }

        internal override void addResetAssignments(List<SequentialStatement> statements)
        {
            if (resetExpression == null)
            {
                //TODO: doesn't work for integer or enumeration types
                Expression tmpReset;
                if (output.Type is IndexSubtypeIndication)
                {
                    tmpReset = Aggregate.OTHERS(StdLogic1164.STD_LOGIC_0);
                }
                else
                {
                    tmpReset = StdLogic1164.STD_LOGIC_0;
                }
                statements.Add(new SignalAssignment(Name.reference(output), tmpReset));
            }
            else
            {
                statements.Add(new SignalAssignment(Name.reference(output), resetExpression));
            }
        }

        /// <summary>
        /// Register reset type.
        /// </summary>
        public enum ResetTypeEnum
        {

            /// Synchronous reset. 
            SYNCHRONOUS,
            /// Asynchronous reset. 
            ASYNCHRONOUS
        }

        /// <summary>
        /// 
        /// </summary>Register reset level.
        public enum ResetLevelEnum
        {

            /// Low active reset. 
            LOW,
            /// High active reset. 
            HIGH
        }
    }

}