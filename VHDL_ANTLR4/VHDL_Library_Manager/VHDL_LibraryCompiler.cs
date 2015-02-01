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
using System.IO;
using VHDL.parser;
using VHDL;
using VHDL.libraryunit;

namespace VHDL.parser
{
    public class VHDL_LibraryCompiler
    {
        private List<string> LibraryPath;
        private string LibraryConfigurationPath;
        private List<LibraryFileInfo> libraryFiles;
        private List<LibraryFileInfo> compileQueue;
        private VHDL_Library_Manager libraryManager;

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        /// <summary>
        /// Обработанные библиотеки
        /// </summary>
        private List<LibraryInfo> parsedLibraries;
        public List<LibraryInfo> ParsedLibraries
        {
            get { return parsedLibraries; }
            set { parsedLibraries = value; }
        }

        public VHDL_LibraryCompiler(VHDL_Library_Manager libraryManager, List<string> LibraryPath, string LibraryConfigurationPath)
        {
            this.LibraryPath = LibraryPath;
            this.LibraryConfigurationPath = LibraryConfigurationPath;
            this.libraryManager = libraryManager;
            logger = libraryManager.Logger;
        }

        public VHDL_LibraryCompiler(VHDL_Library_Manager libraryManager, string LibraryPath, string LibraryConfigurationPath)
        {
            this.LibraryPath = new List<string>() { LibraryPath };
            this.LibraryConfigurationPath = LibraryConfigurationPath;
            this.libraryManager = libraryManager;
            logger = libraryManager.Logger;
        }

        public void Compile()
        {
            libraryFiles = new List<LibraryFileInfo>();
            compileQueue = new List<LibraryFileInfo>();
            parsedLibraries = new List<LibraryInfo>();
            foreach (string lib in LibraryPath)
                AnalyzeFolder(lib);
            SetDependencies();
            CreateQueue();
            CompileFiles();
            LibraryInfo.SaveLibraries(LibraryConfigurationPath, parsedLibraries);
        }

        /// <summary>
        /// Функция рекурсивно анализирует все VHDL файлы библиотеки
        /// </summary>
        /// <param name="path"></param>
        private void AnalyzeFolder(string path)
        {
            logger.WriteLineFormat("Analyzing folder {0}", path);
            string LibraryName = Path.GetFileNameWithoutExtension(path);
            if (Directory.Exists(path) == true)
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                    AnalyzeFolder(dir);

                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (isVHDLCodeFile(file) == true)
                    {
                        logger.WriteLineFormat("Added file for analyze {0}", file);
                        LibraryFileInfo inf = LibraryFileInfo.AnalyzeFile(file, LibraryName);
                        if (inf != null)
                            libraryFiles.Add(inf);
                    }
                }
            }
        }

        /// <summary>
        /// Установление связей между файлами
        /// </summary>
        private void SetDependencies()
        {
            foreach (LibraryFileInfo f1 in libraryFiles)
                foreach (LibraryFileInfo f2 in libraryFiles)
                    if ((f1 != f2) && (f1.IsDependedTo(f2) == true))
                        f1.Dependencies.Add(f2);
        }

        /// <summary>
        /// Организация очереди для компиляции
        /// </summary>
        private void CreateQueue()
        {
            logger.WriteLine("");
            logger.WriteLine("Creating compilation queue");
            List<LibraryFileInfo> tmp = new List<LibraryFileInfo>(libraryFiles);
            foreach (LibraryFileInfo file in tmp)
                if (file.Dependencies.Count == 0)
                    compileQueue.Add(file);
            foreach (LibraryFileInfo file in compileQueue)
                tmp.Remove(file);

            while (tmp.Count != 0)
            {
                LibraryFileInfo fileToRemove = null;
                foreach (LibraryFileInfo file in tmp)
                {
                    bool include = true;
                    foreach (LibraryFileInfo dep in file.Dependencies)
                        if (compileQueue.Contains(dep) == false)
                            include = false;

                    if (include == true)
                    {
                        compileQueue.Add(file);
                        fileToRemove = file;
                        break;
                    }
                }
                if (fileToRemove != null)
                    tmp.Remove(fileToRemove);
            }

            logger.WriteLine("Compilation order: ");
            foreach (LibraryFileInfo file in compileQueue)
            {
                logger.WriteLineFormat("Tarh: {0}", file.Path);
            }
        }

        /// <summary>
        /// Сам процесс компиляции
        /// </summary>
        private void CompileFiles()
        {
            VhdlParserSettings settings = VhdlParserWrapper.DEFAULT_SETTINGS;
            settings.AddPositionInformation = true;
            RootDeclarativeRegion rootScope = new RootDeclarativeRegion();
            foreach (LibraryFileInfo file in compileQueue)
            {
                logger.WriteLineFormat("Compilation of the file: {0}", file.Path);
                logger.Write(file.PrintInfo());
                CompileFile(file, settings, rootScope);
                logger.WriteLineFormat("Compilation of the file: {0}. success", file.Path);
            }
        }

        /// <summary>
        /// Сгенерировать имя файла для его компиляции
        /// Если нет папки для компиляции - то создать ее
        /// </summary>
        /// <param name="vhdl_path"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        private string FormCompilePath(string vhdl_path, string extention)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(vhdl_path) + "." + extention;
            string binaryPath = System.IO.Path.GetDirectoryName(vhdl_path);
            binaryPath = System.IO.Path.Combine(binaryPath, "Compiled");

            if (Directory.Exists(binaryPath) == false)
                Directory.CreateDirectory(binaryPath);

            binaryPath = System.IO.Path.Combine(binaryPath, filename);

            return binaryPath;
        }

        /// <summary>
        /// Компиляция файла
        /// </summary>
        /// <param name="file"></param>
        private void CompileFile(LibraryFileInfo file, VhdlParserSettings settings, RootDeclarativeRegion rootScope)
        {
            LibraryInfo currentLibrary = null;
            foreach (LibraryInfo inf in parsedLibraries)
                if (inf.Name.Equals(file.LibraryName, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    currentLibrary = inf;
                    break;
                }
            if (currentLibrary == null)
            {
                currentLibrary = new LibraryInfo(file.LibraryName);
                parsedLibraries.Add(currentLibrary);
                rootScope.Libraries.Add(currentLibrary.LibraryScope);
            }
            try
            {
                Console.WriteLine("parsing file {0} ", file.Path);
                VhdlFile vhdfile = VhdlParserWrapper.parseFile(file.Path, settings, rootScope, currentLibrary.LibraryScope, libraryManager);
                foreach (LibraryUnit unit in vhdfile.Elements)
                {
                    if (unit is PackageDeclaration)
                    {
                        PackageDeclaration pd = unit as PackageDeclaration;
                        pd.Parent = null;
                        bool foundPackage = false;
                        foreach (PackageInfo inf in currentLibrary.Packages)
                        {
                            if (inf.Name.VHDLIdentifierEquals(pd.Identifier))
                            {
                                inf.DeclarationPath = file.Path;
                                string path = FormCompilePath(file.Path, "decl");
                                inf.DeclarationLibPath = path;
                                inf.Declaration = pd;
                                foundPackage = true;
                                break;
                            }
                        }
                        if (foundPackage == false)
                        {
                            PackageInfo pi = new PackageInfo(pd.Identifier, currentLibrary.Name, file.Path);
                            pi.DeclarationPath = file.Path;
                            string path = FormCompilePath(file.Path, "decl");
                            pi.DeclarationLibPath = path;
                            pi.BodyLibPath = path;
                            pi.Declaration = pd;
                            currentLibrary.Packages.Add(pi);
                        }
                    }
                    if (unit is PackageBody)
                    {
                        PackageBody pb = unit as PackageBody;
                        pb.Parent = null;
                        bool foundPackage = false;
                        foreach (PackageInfo inf in currentLibrary.Packages)
                        {
                            if (inf.Name.VHDLIdentifierEquals(pb.Package.Identifier))
                            {
                                inf.BodyPath = file.Path;
                                string path = FormCompilePath(file.Path, "body");
                                inf.BodyLibPath = path;
                                inf.Body = pb;
                                foundPackage = true;
                                break;
                            }
                        }
                        if (foundPackage == false)
                        {
                            PackageInfo pi = new PackageInfo(pb.Package.Identifier, currentLibrary.Name, file.Path);
                            pi.BodyPath = file.Path;
                            string path = FormCompilePath(file.Path, "body");
                            pi.BodyLibPath = path;
                            pi.Body = pb;
                            currentLibrary.Packages.Add(pi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLineFormat("parsing {0} failed", file.Path);
                logger.WriteLine(ex.Message);
                logger.WriteLine(LoggerMessageVerbosity.Error, ex.Message);
            }
        }

        /// <summary>
        /// Проверяет расширение файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isVHDLCodeFile(string path)
        {
            try
            {
                System.IO.FileInfo info = new System.IO.FileInfo(path);
                if (info.Name.EndsWith(".vhd") || info.Name.EndsWith(".vhdl"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}