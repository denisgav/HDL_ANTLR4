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
using VHDL.util;
using System.Collections.Generic;

namespace VHDL.highlevel
{

    using StdLogic1164 = VHDL.builtin.StdLogic1164;
    using AbstractProcessStatement = VHDL.concurrent.AbstractProcessStatement;
    using ProcessDeclarativeItem = VHDL.declaration.IProcessDeclarativeItem;
    using Equals = VHDL.expression.Equals;
    using Expression = VHDL.expression.Expression;
    using Expressions = VHDL.expression.Expressions;
    using IfStatement = VHDL.statement.IfStatement;
    using SequentialStatement = VHDL.statement.SequentialStatement;

    /// <summary>
    /// Abstract register or group.
    /// </summary>
    [Serializable]
    public abstract class AbstractRegister : AbstractProcessStatement
    {

        public AbstractRegister()
        {
        }

        public AbstractRegister(string label)
            : base(label)
        {
        }

        internal abstract Register FirstRegister { get; }

        internal abstract void addClockAssignments(List<SequentialStatement> statements);

        internal abstract void addResetAssignments(List<SequentialStatement> statements);

        public override List<ProcessDeclarativeItem> Declarations
        {
            get { return new List<ProcessDeclarativeItem>(); }
        }

        public override ParentSetList<SequentialStatement> Statements
        {
            get
            {
                Register reg = FirstRegister;
                if (reg == null)
                {
                    return ParentSetList<SequentialStatement>.Create(this);
                }

                Expression clkCondition = Expressions.risingEdge(reg.Clock);
                IfStatement statement;

                if (reg.Reset == null)
                {
                    statement = new IfStatement(clkCondition);
                    addClockAssignments(statement.Statements);
                }
                else
                {
                    Expression resetActive;
                    if (reg.ResetLevel == Register.ResetLevelEnum.LOW)
                    {
                        resetActive = StdLogic1164.STD_LOGIC_0;
                    }
                    else
                    {
                        resetActive = StdLogic1164.STD_LOGIC_1;
                    }
                    Expression resetCondition = new Equals(reg.Reset, resetActive);

                    if (reg.ResetType == Register.ResetTypeEnum.ASYNCHRONOUS)
                    {
                        statement = new IfStatement(resetCondition);
                        addResetAssignments(statement.Statements);

                        IfStatement.ElsifPart clkPart = statement.createElsifPart(clkCondition);
                        addClockAssignments(clkPart.Statements);
                    }
                    else
                    {
                        statement = new IfStatement(clkCondition);

                        IfStatement innerIf = new IfStatement(resetCondition);
                        addResetAssignments(innerIf.Statements);
                        addClockAssignments(innerIf.ElseStatements);

                        statement.Statements.Add(innerIf);
                    }
                }
                ParentSetList<SequentialStatement> res = ParentSetList<SequentialStatement>.Create(this); ;
                res.Add(statement);
                return res;
            }
        }
    }

}