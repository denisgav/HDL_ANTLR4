using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Objects;

namespace DataContainer
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
        private List<IValueProvider> variables;
        public List<IValueProvider> Variables
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
            variables = new List<IValueProvider>();
        }

        public void Clear()
        {
            foreach (SimulationScope sc in items)
            {
                sc.Clear();
            }
            variables.Clear();
        }

        #region IEnumerable<IValueProvider> Members

        public IEnumerable<IValueProvider> GetVariablesEnumerator()
        {
            foreach (SimulationScope sc in items)
            {
                foreach (IValueProvider var in sc.GetVariablesEnumerator())
                    yield return var;
            }
            foreach (IValueProvider var in variables)
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
