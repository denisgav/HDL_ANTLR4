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
using System.Linq;
using System.Text;
using VHDLRuntime.Values;
using VHDLRuntime.Objects;

namespace VHDLRuntime
{
    [System.Serializable]
    public class SimulationScope
    {
        /// <summary>
        /// Имя элемента
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public SimulationScope Parent { get; set; }

        /// <summary>
        /// Вложенные элементы
        /// </summary>
        private List<SimulationScope> items;
        public List<SimulationScope> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        /// <summary>
        /// Сигналы, которые принадлежат этому элементу
        /// </summary>
        private List<Signal> variables;
        public List<Signal> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
            }
        }

        public SimulationScope(string Name, SimulationScope Parent)
        {
            this.Parent = Parent;
            this.Name = Name;
            items = new List<SimulationScope>();
            variables = new List<Signal>();
        }

        public void Clear()
        {
            foreach (SimulationScope sc in items)
            {
                sc.Clear();
            }
            variables.Clear();
        }

        #region IEnumerable<ValueProvider> Members

        public IEnumerable<Signal> GetVariablesEnumerator()
        {
            foreach (SimulationScope sc in items)
            {
                foreach (Signal var in sc.GetVariablesEnumerator())
                    yield return var;
            }
            foreach (Signal var in variables)
                yield return var;
        }

        #endregion

        public SimulationScope AddNewScope(string Name)
        {
            SimulationScope sc = new SimulationScope(Name, this);
            items.Add(sc);
            return sc;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
