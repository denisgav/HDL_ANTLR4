using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.Compiler
{
    /// <summary>
    /// Распарсеный заголовок
    /// </summary>
    public struct GHDLParsedHead
    {
        public string fileName;
        public string timeAdd;
        public string timeCompile;

        public GHDLParsedHead(string fileName, string timeAdd, string timeCompile)
        {
            this.fileName = fileName;
            this.timeAdd = timeAdd;
            this.timeCompile = timeCompile;
        }
    }
}