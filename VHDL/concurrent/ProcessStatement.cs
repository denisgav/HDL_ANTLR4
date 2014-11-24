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


using VHDL.util;
using System;
using System.Collections.Generic;

namespace VHDL.concurrent
{
    using Signal = VHDL.Object.Signal;
    using ProcessDeclarativeItem = VHDL.declaration.IProcessDeclarativeItem;
    using SequentialStatement = VHDL.statement.SequentialStatement;

    /// <summary>
    /// Process statement.
    /// </summary>
    [Serializable]
    public class ProcessStatement : AbstractProcessStatement
    {
        private readonly List<ProcessDeclarativeItem> declarations;
        private readonly ParentSetList<SequentialStatement> statements;
        private readonly List<Signal> sensitivityList;

        /// <summary>
        /// Creates a process statement without a label.
        /// </summary>
        public ProcessStatement()
            : base()
        {
            declarations = new List<ProcessDeclarativeItem>();
            statements = ParentSetList<SequentialStatement>.Create(this);
            sensitivityList = new List<Signal>();
            Label = "UnnamedProcess";
        }

        /// <summary>
        /// Creates a process statement.
        /// </summary>
        /// <param name="label">the process label</param>
        public ProcessStatement(string label)
            : base(label)
        {
            declarations = new List<ProcessDeclarativeItem>();
            statements = ParentSetList<SequentialStatement>.Create(this);
            sensitivityList = new List<Signal>();
            Label = label;
        }

        /// <summary>
        /// Returns the declarations.
        /// </summary>
        public override List<ProcessDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public override ParentSetList<SequentialStatement> Statements
        {
            get { return statements; }
        }

        /// <summary>
        /// Returns the sensitivity list.
        /// </summary>
        public override List<Signal> SensitivityList
        {
            get { return sensitivityList; }
        }
    }

}