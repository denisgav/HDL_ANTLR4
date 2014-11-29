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
using System.Xml;
using VHDL;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using VHDL.parser;
using VHDL.parser.util;
using VHDLParser;
using VHDL.libraryunit;

namespace VHDL.parser
{
    public class VHDL_Library_Manager
    {
        private VHDL_LibraryCompiler compiler;
        private List<LibraryInfo> parsedLibraries;
        //Используется исключительно для организации критической секции
        private System.Object lockThis = new System.Object();

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public VHDL_Library_Manager(string folderPath, string libraryConfigurationPath, Logger logger)
        {
            this.libraryConfigurationPath = libraryConfigurationPath;
            this.logger = logger;
        }


        private string libraryConfigurationPath;
        public string LibraryConfigurationPath
        {
            //get { return Schematix.CommonProperties.Configuration.CurrentConfiguration.LibrariesOptions.LibraryConfigurationPath;  }
            get { return libraryConfigurationPath; }
            set { libraryConfigurationPath = value; }
        }

        public bool IsLibraryCompiled
        {
            get { return File.Exists(LibraryConfigurationPath); }
        }

        /// <summary>
        /// Получение всех стандартных библиотек
        /// </summary>
        public List<LibraryInfo> Libraries
        {
            get
            {
                lock (lockThis)
                {
                    return parsedLibraries;
                }
            }
        }


        /// <summary>
        /// Загрузка библиотеки
        /// </summary>
        public void LoadData(string LibraryPath)
        {
            /*
            if (IsLibraryCompiled == false)
            {
                compiler = new VHDL_LibraryCompiler(this, LibraryPath, LibraryConfigurationPath);
                compiler.Compile();
            }
            parsedLibraries = LibraryInfo.LoadLibraries(logger, LibraryConfigurationPath);
            */
        }

        /// <summary>
        /// Скомпилироват библиотеку повторно
        /// </summary>
        public void RebuildData(string LibraryPath)
        {
            compiler = new VHDL_LibraryCompiler(this, LibraryPath, LibraryConfigurationPath);
            compiler.Compile();
            parsedLibraries = LibraryInfo.LoadLibraries(logger, LibraryConfigurationPath);
        }

        /*
        /// <summary>
        /// Скомпилироват библиотеку повторно
        /// </summary>
        public static void RebuildData()
        {
            RebuildData(Schematix.CommonProperties.Configuration.CurrentConfiguration.LibrariesOptions.VHDLLibrariesPaths)
        }
        */
        /// <summary>
        /// Известна ли библиотека с именем libraryName
        /// </summary>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        public bool ContainsLibrary(string libraryName)
        {
            if (libraryName.Equals("work", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (IsLibraryCompiled == false)
            {
                foreach (LibraryInfo inf in compiler.ParsedLibraries)
                    if (inf.Name.Equals(libraryName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
            }
            else
            {
                foreach (LibraryInfo inf in parsedLibraries)
                    if (inf.Name.Equals(libraryName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Получение распарсеной библиотеки по имени файла
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LibraryDeclarativeRegion GetLibrary(string name)
        {
            if (IsLibraryCompiled == false)
            {
                foreach (LibraryInfo inf in compiler.ParsedLibraries)
                    if (inf.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return inf.LibraryScope;
            }
            else
            {
                foreach (LibraryInfo inf in parsedLibraries)
                    if (inf.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return inf.LibraryScope;
            }
            return null;
        }

        /// <summary>
        /// Получение тела пакета по его имени и по имени библиотеки
        /// </summary>
        /// <param name="LibraryName"></param>
        /// <param name="PackageName"></param>
        /// <returns></returns>
        public PackageBody GetPackageBody(string LibraryName, string PackageName)
        {
            LibraryDeclarativeRegion library = GetLibrary(LibraryName);
            foreach (VhdlFile file in library.Files)
            {
                foreach (LibraryUnit unit in file.Elements)
                {
                    if (unit is PackageBody)
                    {
                        if ((unit as PackageBody).Package.Identifier.Equals(PackageName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return (unit as PackageBody);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Получение тела пакета по его декларации
        /// </summary>
        /// <param name="packDecl"></param>
        /// <returns></returns>
        public PackageBody GetPackageBody(PackageDeclaration packDecl)
        {
            foreach (LibraryInfo lib in Libraries)
            {
                foreach (PackageInfo pack in lib.Packages)
                {
                    if (pack.Declaration.Equals(packDecl))
                        return pack.Body;
                }
            }
            return null;
        }
    }
}
