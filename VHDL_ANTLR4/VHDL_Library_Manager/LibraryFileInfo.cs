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
using System.Text.RegularExpressions;
using VHDL.util;

namespace VHDL.parser
{
    public class LibraryFileInfo
    {

        /// <summary>
        /// Имя библиотеки
        /// </summary>
        private string libraryName;
        public string LibraryName
        {
            get { return libraryName; }
            set { libraryName = value; }
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }


        /// <summary>
        /// Список тел пакетов
        /// </summary>
        private List<string> packageBodies;
        public List<string> PackageBodies
        {
            get { return packageBodies; }
            set { packageBodies = value; }
        }


        /// <summary>
        /// Список деклараций пакетов
        /// </summary>
        private List<string> packageDeclarations;
        public List<string> PackageDeclarations
        {
            get { return packageDeclarations; }
            set { packageDeclarations = value; }
        }

        /// <summary>
        /// Операторы use
        /// </summary>
        private List<UseClauseInfo> useClauses;
        public List<UseClauseInfo> UseClauses
        {
            get { return useClauses; }
            set { useClauses = value; }
        }

        /// <summary>
        /// Зависимости от других файлов
        /// </summary>
        private List<LibraryFileInfo> dependencies;
        public List<LibraryFileInfo> Dependencies
        {
            get { return dependencies; }
            set { dependencies = value; }
        }

        /// <summary>
        /// Список обьявленых Entity в файле
        /// </summary>
        private List<string> entities;
        public List<string> Entities
        {
            get { return entities; }
            set { entities = value; }
        }

        /// <summary>
        /// Список обьявленных архитектур
        /// </summary>
        private List<ArchitectureInfo> architectures;
        public List<ArchitectureInfo> Architectures
        {
            get { return architectures; }
            set { architectures = value; }
        }

        public string PrintInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Path: {0}\n", Path);
            builder.AppendFormat("LibraryName: {0}\n", LibraryName);

            if (PackageDeclarations.Count != 0)
            {
                builder.AppendFormat("Packages declaration:\n");
                foreach (var p in PackageDeclarations)
                    builder.AppendLine(p);
            }

            if (PackageBodies.Count != 0)
            {
                builder.AppendFormat("Packages bodies:\n");
                foreach (var p in PackageBodies)
                    builder.AppendLine(p);
            }

            if (Entities.Count != 0)
            {
                builder.AppendFormat("Entities list:\n");
                foreach (var p in Entities)
                    builder.AppendLine(p);
            }

            if (Architectures.Count != 0)
            {
                builder.AppendFormat("Architectures list:\n");
                foreach (var p in Architectures)
                    builder.AppendFormat("Architecture {0} of the entity {1} \n", p.Name, p.EntityName);
            }

            return builder.ToString();
        }



        /// <summary>
        /// Проверяет зависимости от другого файла
        /// </summary>
        /// <param name="otherLibrary"></param>
        /// <returns></returns>
        public bool IsDependedTo(LibraryFileInfo otherLibraryFile)
        {
            foreach (UseClauseInfo use in useClauses)
            {
                foreach (string package in otherLibraryFile.packageDeclarations)
                    if (use.Package.EqualsIgnoreCase(package))
                        return true;
                foreach (string package in otherLibraryFile.packageBodies)
                    if (use.Package.EqualsIgnoreCase(package))
                        return true;
            }
            foreach (string body in packageBodies)
                foreach (string decl in otherLibraryFile.packageDeclarations)
                    if (decl.EqualsIgnoreCase(body))
                        return true;
            foreach (ArchitectureInfo arch in architectures)
                foreach (string entity in otherLibraryFile.entities)
                    if (arch.EntityName.EqualsIdentifier(entity))
                        return true;
            return false;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="path"></param>
        public LibraryFileInfo(string libraryName, string path)
        {
            this.path = path;
            this.libraryName = libraryName;
            packageBodies = new List<string>();
            packageDeclarations = new List<string>();
            useClauses = new List<UseClauseInfo>();
            dependencies = new List<LibraryFileInfo>();
            architectures = new List<ArchitectureInfo>();
            entities = new List<string>();

            if (libraryName != "std")
                useClauses.Add(new UseClauseInfo("std", "standard", "all"));
        }

        /// <summary>
        /// Анализ файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="LibraryName"></param>
        /// <returns></returns>
        public static LibraryFileInfo AnalyzeFile(string path, string LibraryName)
        {
            if (File.Exists(path) == true)
            {
                string textinput = File.ReadAllText(path);
                return AnalyseText(textinput, path, LibraryName);
            }
            else
                return null;
        }

        /// <summary>
        /// Анализ текста
        /// </summary>
        /// <param name="text"></param>
        /// <param name="LibraryName"></param>
        /// <returns></returns>
        public static LibraryFileInfo AnalyseText(string textinput, string path, string LibraryName)
        {
            LibraryFileInfo res = new LibraryFileInfo(LibraryName, path);
            string text = commentRegex.Replace(textinput, string.Empty);

            //анализируем операторы use
            foreach (Match m in useClauseRegex.Matches(text))
            {
                try
                {
                    UseClauseInfo use = UseClauseInfo.Parse(m.Groups[1].Value);
                    if (res.ContainUse(use) == false)
                        res.useClauses.Add(use);
                }
                catch (Exception ex)
                { }
            }

            //анализируем определения пакетов
            foreach (Match m in packageBodyRegex.Matches(text))
            {
                res.packageBodies.Add(m.Groups[1].Value);
            }

            //анализируем обьявления пакетов
            foreach (Match m in packageDeclarationRegex.Matches(text))
            {
                res.packageDeclarations.Add(m.Groups[1].Value);
            }

            //анализируем обьявление entity
            foreach (Match m in entityRegex.Matches(text))
            {
                res.entities.Add(m.Groups[1].Value);
            }

            //анализируем обьявление архитектур
            foreach (Match m in architectureRegex.Matches(text))
            {
                res.architectures.Add(new ArchitectureInfo(m.Groups[1].Value, m.Groups[2].Value));
            }

            return res;
        }

        private bool ContainUse(UseClauseInfo inf)
        {
            foreach (UseClauseInfo i in useClauses)
                if (i.Package == inf.Package)
                    return true;
            return false;
        }

        //Регулярные выражения
        private static readonly Regex packageDeclarationRegex;
        private static readonly Regex packageBodyRegex;
        private static readonly Regex useClauseRegex;
        private static readonly Regex commentRegex;
        private static readonly Regex entityRegex;
        private static readonly Regex architectureRegex;

        //Статический конструктор
        static LibraryFileInfo()
        {
            packageDeclarationRegex = new Regex(@"package\s*([a-zA-Z0-9_]+)\s*is", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            packageBodyRegex = new Regex(@"package\s*body\s*([a-zA-Z0-9_]+)\s*is", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            entityRegex = new Regex(@"entity\s*([a-zA-Z0-9_]+)\s*is", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            architectureRegex = new Regex(@"architecture\s*([a-zA-Z0-9_]+)\s*of\s*([a-zA-Z0-9_]+)\s*is", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            useClauseRegex = new Regex(@"use\s*([a-zA-Z0-9_.]+)\s*;", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            commentRegex = new Regex(@"--.*\r\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public override string ToString()
        {
            return path;
        }
    }
}