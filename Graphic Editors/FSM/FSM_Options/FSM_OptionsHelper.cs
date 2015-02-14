using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.FSM
{
    public class FSM_OptionsHelper
    {
        public static readonly string Default_Design_Unit_Header_VHDL =
            "library IEEE; \n" +
            "use IEEE.std_logic_1164.all; \n" +
            "use IEEE.std_logic_arith.all; \n" +
            "use IEEE.std_logic_unsigned.all;";

        public static readonly string Default_Design_Unit_Header_Verilog =
            "`timescale 1ns / 1ps";

        public enum FSMStatesLayout
        {
            Circular,
            LinearHorisontal,
            LinearVertical
        }

        public enum FSMTransition
        {
            None,
            Forward,
            Backward,
            Both
        }

        public string Design_UnitHeader { get; set; }
        public FSMStatesLayout StatesLayout { get; set; }
        public FSMTransition Transition { get; set; }
        public string ResetState { get; set; }
        private int numberOfStates;
        public int NumberOfStates
        {
            get
            {
                return numberOfStates;
            }
            set
            {
                if ((value >= 0) && (value <= 20))
                {
                    numberOfStates = value;
                }
                else
                    throw new Exception("Invalid Number of states");
            }
        }

        public FSM_OptionsHelper()
        {
            ResetState = "none";
            numberOfStates = 0;
            StatesLayout = FSMStatesLayout.Circular;
            Transition = FSMTransition.Forward;
        }
    }
}
