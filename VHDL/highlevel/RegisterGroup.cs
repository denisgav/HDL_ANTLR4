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

namespace VHDL.highlevel
{
    using Signal = VHDL.Object.Signal;
    using SequentialStatement = VHDL.statement.SequentialStatement;

    //TODO: check if reset types and clock signals are equal for all registers
    /// <summary>
    /// Group of registers.
    /// A register group allows to use a single VDHL process for more than one registered signal.
    /// </summary>
    [Serializable]
    public class RegisterGroup : AbstractRegister
    {

        private readonly List<Register> registers;

        /// <summary>
        /// Creates a register group.
        /// </summary>
        /// <param name="registers">a list of registers</param>
        public RegisterGroup(List<Register> registers)
        {
            this.registers = new List<Register>(registers);
        }

        /// <summary>
        /// Creates a register group.
        /// </summary>
        /// <param name="registers">a list of registers</param>
        public RegisterGroup(params Register[] registers)
            : this(new List<Register>(registers))
        {
        }

        //    *
        //     * Returns the registers in this group
        //     * @return a list of registers
        //     
        public virtual List<Register> Registers
        {
            get { return registers; }
        }

        //TODO: add signals in reset expression
        public override List<Signal> SensitivityList
        {
            get
            {
                Register reg = FirstRegister;

                if (reg == null)
                {
                    return new List<Signal>();
                }
                else
                {
                    return new List<Signal>(new Signal[] { reg.Clock });
                }
            }
        }

        internal override Register FirstRegister
        {
            get
            {
                if (registers.Count == 0)
                {
                    return null;
                }
                else
                {
                    return registers[0];
                }
            }
        }

        internal override void addClockAssignments(List<SequentialStatement> statements)
        {
            foreach (Register register in registers)
            {
                register.addClockAssignments(statements);
            }
        }

        internal override void addResetAssignments(List<SequentialStatement> statements)
        {
            foreach (Register register in registers)
            {
                register.addResetAssignments(statements);
            }
        }
    }

}