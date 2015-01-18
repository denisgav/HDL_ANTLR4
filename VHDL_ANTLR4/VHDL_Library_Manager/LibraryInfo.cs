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

namespace VHDL.parser
{
    /// <summary>
    /// Информация о библиотеке
    /// </summary>
    public class LibraryInfo
    {
        /// <summary>
        /// Имя библиотеки
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Список пакетов
        /// </summary>
        private List<PackageInfo> packages;
        public List<PackageInfo> Packages
        {
            get { return packages; }
            set { packages = value; }
        }

        public LibraryInfo(string name)
        {
            this.name = name;
            packages = new List<PackageInfo>();
            libraryScope = new LibraryDeclarativeRegion(name);
        }

        /// <summary>
        /// Используется для синтаксического разбора
        /// </summary>
        private LibraryDeclarativeRegion libraryScope;
        public LibraryDeclarativeRegion LibraryScope
        {
            get { return libraryScope; }
            set { libraryScope = value; }
        }

        /// <summary>
        /// Обновление параметров LibraryScope
        /// </summary>
        public void UpdateLibraryScope()
        {
            if (libraryScope == null)
                libraryScope = new LibraryDeclarativeRegion(name);

            VhdlFile file = new VhdlFile(name);
            foreach (PackageInfo inf in packages)
                file.Elements.Add(inf.Declaration);

            libraryScope.Files.Add(file);
        }

        #region Сохранение / Загрузка в XML

        /// <summary>
        /// Загрузка списка библиотек с XML файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<LibraryInfo> LoadLibraries(Logger logger, string path)
        {
            logger.WriteLineFormat("Loading compiled libraries from file {0}", path);
            List<LibraryInfo> res = new List<LibraryInfo>();

            XmlDocument _doc = new XmlDocument();
            _doc.Load(path);
            XmlNodeList nodes = _doc.SelectNodes("/Libraries/Library");
            foreach (XmlNode node in nodes)
            {
                string LibraryName = node.Attributes["LibraryName"].Value;
                LibraryInfo inf = new LibraryInfo(LibraryName);
                res.Add(inf);
                foreach (XmlNode package in node.ChildNodes)
                {
                    string PackageName = package.Attributes["Name"].Value;
                    string DeclarationPath = package.Attributes["DeclarationPath"].Value;
                    string BodyPath = package.Attributes["BodyPath"].Value;
                    string DeclarationLibPath = package.Attributes["DeclarationLibPath"].Value;
                    string BodyLibPath = package.Attributes["BodyLibPath"].Value;

                    PackageInfo pi = new PackageInfo(PackageName, LibraryName, DeclarationPath, BodyPath, DeclarationLibPath, BodyLibPath);
                    inf.packages.Add(pi);

                    logger.WriteLine("----------------------------------");
                    logger.WriteLineFormat("PackageName: {0}", PackageName);
                    logger.WriteLineFormat("DeclarationPath: {0}", DeclarationPath);
                    logger.WriteLineFormat("BodyPath: {0}", BodyPath);
                    logger.WriteLineFormat("DeclarationLibPath: {0}", DeclarationLibPath);
                    logger.WriteLineFormat("BodyLibPath: {0}", BodyLibPath);
                    logger.WriteLine("----------------------------------");
                }
                inf.UpdateLibraryScope();
            }

            return res;
        }

        /// <summary>
        /// Сохранение списка библиотек в XML файл
        /// </summary>
        /// <param name="path"></param>
        /// <param name="libraries"></param>
        public static void SaveLibraries(string path, List<LibraryInfo> libraries)
        {
            XmlDocument _doc = new XmlDocument();
            XmlElement rootNode = _doc.CreateElement("Libraries");
            foreach (LibraryInfo inf in libraries)
            {
                XmlElement Library = _doc.CreateElement("Library");

                XmlAttribute LibraryName = _doc.CreateAttribute("LibraryName");
                LibraryName.Value = inf.Name;
                Library.Attributes.Append(LibraryName);

                foreach (PackageInfo pi in inf.packages)
                {
                    XmlElement Package = _doc.CreateElement("Package");

                    XmlAttribute PackageName = _doc.CreateAttribute("Name");
                    PackageName.Value = pi.Name;
                    Package.Attributes.Append(PackageName);

                    XmlAttribute DeclarationPath = _doc.CreateAttribute("DeclarationPath");
                    DeclarationPath.Value = pi.DeclarationPath;
                    Package.Attributes.Append(DeclarationPath);

                    XmlAttribute BodyPath = _doc.CreateAttribute("BodyPath");
                    BodyPath.Value = pi.BodyPath;
                    Package.Attributes.Append(BodyPath);

                    XmlAttribute DeclarationLibPath = _doc.CreateAttribute("DeclarationLibPath");
                    DeclarationLibPath.Value = pi.DeclarationLibPath;
                    Package.Attributes.Append(DeclarationLibPath);

                    XmlAttribute BodyLibPath = _doc.CreateAttribute("BodyLibPath");
                    BodyLibPath.Value = pi.BodyLibPath;
                    Package.Attributes.Append(BodyLibPath);

                    Library.AppendChild(Package);
                }

                rootNode.AppendChild(Library);
            }
            _doc.AppendChild(rootNode);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.Unicode;
            settings.NewLineOnAttributes = true;

            XmlWriter writer = XmlWriter.Create(path, settings);
            _doc.Save(writer);
            writer.Close();
        }
        #endregion
    }
}
