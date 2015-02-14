using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.ValueDump
{
    public struct VCDScope
    {
        public enum ScopeType
        {
            begin,
            fork,
            function,
            module,
            task
        }

        /// <summary>
        /// Тип Scope
        /// </summary>
        private ScopeType scType;
        public ScopeType ScType
        {
            get
            {
                return scType;
            }
        }

        /// <summary>
        /// Имя Scope
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public static VCDScope Parse(string[] Words)
        {
            VCDScope sc = new VCDScope();
            switch (Words[1])
            {
                case "begin": sc.scType = ScopeType.begin; break;
                case "fork": sc.scType = ScopeType.fork; break;
                case "function": sc.scType = ScopeType.function; break;
                case "module": sc.scType = ScopeType.begin; break;
                case "task": sc.scType = ScopeType.begin; break;
                default: throw new Exception("Scope Type is incorrect");
            }
            sc.name = Words[2];
            return sc;
        }
    }
}
