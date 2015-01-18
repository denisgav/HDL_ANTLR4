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
using VHDL.parser;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using VHDLCompiler.VHDLObserver;
using VHDLCompiler.CodeGenerator;

namespace VHDLCompiler
{
    public class VHDLCompilerInterface
    {
        /// <summary>
        /// Моделируемая архитектура
        /// </summary>
        private readonly VHDL.libraryunit.Architecture architecture;
        public VHDL.libraryunit.Architecture Architecture
        {
            get { return architecture; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private string tempCodeFolder;
        public string TempCodeFolder
        {
            get { return tempCodeFolder; }
            set { tempCodeFolder = value; }
        }

        private List<string> tempCodeFiles;

        public List<string> TempCodeFiles
        {
            get { return tempCodeFiles; }
            set { tempCodeFiles = value; }
        }

        private VHDLTypeDictionary typeDictionary;
        public VHDLTypeDictionary TypeDictionary
        {
            get { return typeDictionary; }
        }

        private VHDLTypeRangeDictionary typeRangeDictionary;
        public VHDLTypeRangeDictionary TypeRangeDictionary
        {
            get { return typeRangeDictionary; }
        }

        private VHDLObjectDictionary objectDictionary;
        public VHDLObjectDictionary ObjectDictionary
        {
            get { return objectDictionary; }
        }

        private VHDLLiteralDictionary literalDictionary;
        public VHDLLiteralDictionary LiteralDictionary
        {
            get { return literalDictionary; }
        }




        public VHDLCompilerInterface(VHDL.libraryunit.Architecture architecture, Logger logger)
        {
            this.logger = logger;
            this.architecture = architecture;
            typeDictionary = new VHDLTypeDictionary();
            objectDictionary = new VHDLObjectDictionary();
            literalDictionary = new VHDLLiteralDictionary();
            typeRangeDictionary = new VHDLTypeRangeDictionary();
        }

        public void SaveCode(string text, string name)
        {
            string path = System.IO.Path.Combine(tempCodeFolder, string.Format("{0}.cs", name));
            tempCodeFiles.Add(path);
            System.IO.File.WriteAllText(path, text);
        }

        public void Compile(string outPath)
        {
            logger.WriteLineFormat("Compilation of the architecture {0}, entity {1} to the file {2}", architecture.Identifier, architecture.Entity.Identifier, outPath);

            string testName = architecture.Identifier;
            string appBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string fileName = string.Format("{0}\\{1}.dll", appBase, architecture.Identifier);

            tempCodeFolder = System.IO.Path.Combine(appBase, "temp");
            if (System.IO.Directory.Exists(tempCodeFolder) == true)
            {
                System.IO.Directory.Delete(tempCodeFolder, true);
            }

            System.IO.Directory.CreateDirectory(tempCodeFolder);
            tempCodeFiles = new List<string>();

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("VHDLRuntime.dll");
            parameters.ReferencedAssemblies.Add("VHDLParser.dll");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = fileName;

            ArchitectureObserver architectureObserver = new ArchitectureObserver(architecture, logger);
            architectureObserver.Observe(this);            

            CompilerResults results = provider.CompileAssemblyFromFile(parameters, tempCodeFiles.ToArray());

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
        }
    }
}
