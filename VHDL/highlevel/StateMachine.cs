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
using VHDL.util;
using VHDL.statement;
using VHDL.concurrent;
using VHDL.expression;
using VHDL.declaration;
using VHDL.type;
using VHDL.literal;
using VHDL.Object;

namespace VHDL.highlevel
{
    /// <summary>
    /// State machine.
    /// </summary>
    public class StateMachine
    {

        private string stateSignalIdentifier;
        private readonly List<IState> states = new List<IState>();
        private IState startState;
        private readonly List<SequentialStatement> combinatorialStatements = new List<SequentialStatement>();
        private readonly StateMachineProcess process;
        private readonly Register register;
        private List<IBlockDeclarativeItem> declarations;
        private readonly List<ConcurrentStatement> statements;
        private EnumerationType enumerationType;
        private Signal nextStateSignal;
        private Signal currentStateSignal;
        private Signal clock;
        private Signal reset;
        //FIXME: make configurable
        private const string NEXT_PREFIX = "NEXT_";
        private const string CURRENT_PREFIX = "CURRENT_";
        private const string TYPE_SUFFIX = "_TYPE";

        /// <summary>
        /// Creates a state machine.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="stateSignalIdentifier">the identifier of the state signals</param>
        /// <param name="clock">the clock signal</param>
        /// <param name="reset">the reset signal</param>
        public StateMachine(string identifier, string stateSignalIdentifier, Signal clock, Signal reset)
        {
            this.clock = clock;
            this.reset = reset;

            process = new StateMachineProcess(this);
            process.Label = identifier;

            enumerationType = new EnumerationType(stateSignalIdentifier + TYPE_SUFFIX);

            setStateSignalIdentifierHelper(stateSignalIdentifier);
            register = new StateMachineRegister(this);
            statements = new List<ConcurrentStatement>(new ConcurrentStatement[] { process, register });
        }

        private void setStateSignalIdentifierHelper(string stateSignalIdentifier)
        {
            this.stateSignalIdentifier = stateSignalIdentifier;
            nextStateSignal = new Signal(NEXT_PREFIX + stateSignalIdentifier, enumerationType);
            currentStateSignal = new Signal(CURRENT_PREFIX + stateSignalIdentifier, enumerationType);

            process.updateSignals();

            declarations = new List<IBlockDeclarativeItem>(new IBlockDeclarativeItem[] { enumerationType, new SignalDeclaration(nextStateSignal, currentStateSignal) });

            foreach (IState state in states)
            {
                if (state is IdentifierStateImpl)
                {
                    ((IdentifierStateImpl)state).updateStateChange();
                }
            }
        }

        /// <summary>
        /// Returns/Sets the identifier.
        /// </summary>
        public virtual string getIdentifier
        {
            get { return process.Label; }
            set { process.Label = value; }
        }

        /// <summary>
        /// Returns/Sets the identifier of the state signals.
        /// </summary>
        public virtual string StateSignalIdentifier
        {
            get { return stateSignalIdentifier; }
            set { stateSignalIdentifier = value; }
        }

        /// <summary>
        /// Returns a list of states.
        /// </summary>
        public virtual List<IState> States
        {
            get { return states; }
        }

        /// <summary>
        /// Returns the sensitivity list.
        /// </summary>
        public virtual List<Signal> SensitivityList
        {
            get { return process.SensitivityList; }
        }

        /// <summary>
        /// Returns the combinatorial statement.
        /// </summary>
        public virtual List<SequentialStatement> CombinatorialStatements
        {
            get { return combinatorialStatements; }
        }

        /// <summary>
        /// Returns/Sets the start state of this state machine.
        /// </summary>
        public virtual IState StartState
        {
            get
            {
                if (startState == null && states.Count != 0)
                {
                    startState = states[0];
                }

                return startState;
            }
            set { this.startState = value; }
        }

        /// <summary>
        /// Returns the signal that contains the current state.
        /// </summary>
        /// <returns></returns>
        public virtual Signal getCurrentStateSignal()
        {
            return currentStateSignal;
        }

        /// <summary>
        /// Returns the signal that contains the next state.
        /// </summary>
        /// <returns></returns>
        public virtual Signal getNextStateSignal()
        {
            return nextStateSignal;
        }

        /// <summary>
        /// Creates a new state and adds it to this state machine.
        /// </summary>
        /// <param name="identifier">the states identifier</param>
        /// <returns>the created state</returns>
        public virtual IState createState(string identifier)
        {
            IState state = new IdentifierStateImpl(identifier, this);
            states.Add(state);
            return state;
        }

        /// <summary>
        /// Creates a new others state and adds it to this state machine
        /// </summary>
        /// <returns>the created state</returns>
        public virtual IState createOthersState()
        {
            IState state = new OthersStateImpl(this);
            states.Add(state);
            return state;
        }

        /// <summary>
        /// Returns the declaration.
        /// </summary>
        public virtual List<IBlockDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public virtual List<ConcurrentStatement> Statements
        {
            get { return statements; }
        }

        /// <summary>
        /// Returns the statements before the case statement.
        /// </summary>
        /// <returns></returns>
        public virtual List<SequentialStatement> getStatementsBefore()
        {
            return process.getStatementsBefore();
        }

        /// <summary>
        /// Returns the statements after the case statement.
        /// </summary>
        /// <returns></returns>
        public virtual List<SequentialStatement> getStatementsAfter()
        {
            return process.getStatementsAfter();
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
        /// Returns/Sets the reset signal.
        /// </summary>
        public virtual Signal Reset
        {
            get { return reset; }
            set { reset = value; }
        }

        /// <summary>
        /// State in a state machine.
        /// </summary>
        public interface IState
        {
            /// <summary>
            /// Returns/Sets the identifier of this state.
            /// </summary>
            string Identifier { get; set; }

            /// <summary>
            /// Returns the enumeration literal associated with this state.
            /// </summary>
            EnumerationLiteral Literal { get; }

            /// <summary>
            ///  Returns the choice.
            /// </summary>
            Choice Choice { get; }

            /// <summary>
            /// Returns the statements in this state.
            /// </summary>
            List<SequentialStatement> Statements { get; }

            //TODO: change return type to signal assignment
            /// <summary>
            /// Creates an unconditional state change.
            /// </summary>
            /// <returns>the state change</returns>
            SequentialStatement createStateChange();

            //TODO: change return type to if statement -> allowes user to add else part
            /// <summary>
            /// Creates a conditional state change.
            /// </summary>
            /// <param name="condition">condition the condition</param>
            /// <returns>the state change</returns>
            SequentialStatement createStateChange(Expression condition);
        }

        private abstract class AbstractStateImpl : IState
        {
            protected StateMachine fsm;

            private readonly List<SequentialStatement> statements = new List<SequentialStatement>();

            public virtual List<SequentialStatement> Statements
            {
                get { return statements; }
            }

            #region State Members

            public virtual string Identifier
            {
                get { throw new System.NotImplementedException(); }
                set { throw new System.NotImplementedException(); }
            }


            public virtual EnumerationLiteral Literal
            {
                get { throw new System.NotImplementedException(); }
            }

            public virtual Choice Choice
            {
                get { throw new System.NotImplementedException(); }
            }

            public virtual SequentialStatement createStateChange()
            {
                throw new System.NotImplementedException();
            }

            public virtual SequentialStatement createStateChange(Expression condition)
            {
                throw new System.NotImplementedException();
            }

            public AbstractStateImpl(StateMachine fsm)
            {
                this.fsm = fsm;
            }

            #endregion
        }

        private sealed class IdentifierStateImpl : AbstractStateImpl
        {

            private EnumerationLiteral literal;
            private SignalAssignment stateChange;

            public IdentifierStateImpl(string identifier, StateMachine fsm)
                : base(fsm)
            {
                Identifier = identifier;
            }

            public override string Identifier
            {
                get { return Literal.ToString(); }
                set
                {
                    fsm.enumerationType.Literals.Remove(literal);
                    literal = fsm.enumerationType.createLiteral(value);
                    if (stateChange == null)
                    {
                        stateChange = new SignalAssignment(Name.reference(fsm.getNextStateSignal()), Literal);
                    }
                    else
                    {
                        updateStateChange();
                    }
                }
            }

            public override EnumerationLiteral Literal
            {
                get { return literal; }
            }

            public override SequentialStatement createStateChange()
            {
                return stateChange;
            }

            public override SequentialStatement createStateChange(Expression condition)
            {
                IfStatement statement = new IfStatement(condition);
                statement.Statements.Add(stateChange);

                return statement;
            }

            public override Choice Choice
            {
                get { return literal; }
            }

            internal void updateStateChange()
            {
                stateChange.Target = Name.reference(fsm.nextStateSignal);

                //TODO: simplify
                stateChange.Waveform.Clear();
                stateChange.Waveform.Add(new WaveformElement(Literal));
            }
        }

        private class OthersStateImpl : AbstractStateImpl
        {

            public OthersStateImpl(StateMachine fsm)
                : base(fsm)
            {
            }

            public virtual new string Identifier
            {
                get { return "others"; }
                set { throw new Exception(); }
            }


            public virtual new SequentialStatement createStateChange()
            {
                throw new Exception();
            }

            public virtual new SequentialStatement createStateChange(Expression condition)
            {
                throw new Exception();
            }

            public virtual new EnumerationLiteral Literal
            {
                get { throw new Exception(); }
            }

            public virtual new Choice Choice
            {
                get { return Choices.OTHERS; }
            }
        }

        private class StateMachineRegister : Register
        {

            public StateMachineRegister(StateMachine fsm)
                : base(fsm.nextStateSignal, fsm.currentStateSignal, fsm.clock, fsm.reset)
            {
                ResetType = ResetTypeEnum.ASYNCHRONOUS;
                ResetLevel = ResetLevelEnum.LOW;
            }

            //TODO: find other method to set reset expression before writing
            //        public InternalVhdlWriter writeVhdl(InternalVhdlWriter writer) throws IOException {
            //            setResetExpression(getStartState().getLiteral());
            //            return super.writeVhdl(writer);
            //        }
        }

        public class StateMachineProcess : AbstractProcessStatement
        {
            private StateMachine fsm;
            private readonly List<Signal> sensitivityList = new List<Signal>();
            private readonly CaseStatement caseStatement;
            private readonly List<SequentialStatement> caseStatementList;
            private readonly List<SequentialStatement> statementsBefore = new List<SequentialStatement>();
            private readonly List<SequentialStatement> statementsAfter = new List<SequentialStatement>();
            private readonly ParentSetList<SequentialStatement> statements;

            public StateMachineProcess(StateMachine fsm)
            {
                this.fsm = fsm;
                caseStatement = new CaseStatement(Name.reference(fsm.currentStateSignal));
                caseStatementList = new List<SequentialStatement>(new SequentialStatement[] { caseStatement });
                statements = ParentSetList<SequentialStatement>.Create(this);
                statements.AddRange(statementsBefore);
                statements.AddRange(caseStatementList);
                statements.AddRange(statementsAfter);
            }

            public virtual void updateSignals()
            {
                caseStatement.Expression = Name.reference(fsm.getCurrentStateSignal());
            }

            public virtual void updateStates()
            {
                caseStatement.Alternatives.Clear();

                foreach (StateMachine.IState state in fsm.States)
                {
                    CaseStatement.Alternative alternative = caseStatement.createAlternative(state.Choice);
                    foreach (var o in state.Statements)
                        alternative.Statements.Add(o);
                }
            }

            public override List<Signal> SensitivityList
            {
                get { return sensitivityList; }
            }

            public override List<IProcessDeclarativeItem> Declarations
            {
                get { return new List<IProcessDeclarativeItem>(); }
            }

            public override ParentSetList<SequentialStatement> Statements
            {
                get
                {
                    updateStates();
                    return statements;
                }
            }

            public virtual List<SequentialStatement> getStatementsAfter()
            {
                return statementsAfter;
            }

            public virtual List<SequentialStatement> getStatementsBefore()
            {
                return statementsBefore;
            }
        }
    }

}