using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.Compiler
{
    /// <summary>
    /// Распарсеная архитектура
    /// </summary>
    public struct GHDLParsedArchitecture
    {
        public string entityName;
        public string architectureName;

        public GHDLParsedArchitecture(string entityName, string architectureName)
        {
            this.entityName = entityName;
            this.architectureName = architectureName;
        }
    }
}