using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.Compiler
{
    /// <summary>
    /// Скомпилированный файл
    /// </summary>
    public class GHDLCompiledFile
    {
        public string timeAdd;
        public string timeCompile;
        public SortedList<string, List<string>> vhdlStruct;

        public GHDLCompiledFile(string timeAdd, string timeCompile, SortedList<string, List<string>> vhdlStruct)
        {
            this.timeAdd = timeAdd;
            this.timeCompile = timeCompile;
            this.vhdlStruct = vhdlStruct;
        }

        public GHDLCompiledFile()
        {
            vhdlStruct = new SortedList<string, List<string>>();
            timeAdd = string.Empty;
            timeCompile = string.Empty;
        }
    }
}