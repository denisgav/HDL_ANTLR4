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
using VHDL.libraryunit;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VHDL.parser
{
    public class PackageInfo
    {
        /// <summary>
        /// Имя пакета
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Имя библиотеки
        /// </summary>
        private string libraryName;
        public string LibraryName
        {
            get { return libraryName; }
            set { libraryName = value; }
        }

        public PackageInfo(string name, string libraryName, string DeclarationPath, string BodyPath, string DeclarationLibPath, string BodyLibPath)
        {
            this.name = name;
            this.libraryName = libraryName;
            declarationLibPath = DeclarationLibPath;
            bodyLibPath = BodyLibPath;
            declarationPath = DeclarationPath;
            bodyPath = BodyPath;
        }

        public PackageInfo(string name, string libraryName, string path)
        {
            this.name = name;
            this.libraryName = libraryName;
            declarationLibPath = Path.Combine(path, name + ".decl");
            bodyLibPath = Path.Combine(path, name + ".body");
        }

        /// <summary>
        /// Путь к файлу с декларацией пакета
        /// </summary>
        private string declarationPath;
        public string DeclarationPath
        {
            get { return declarationPath; }
            set { declarationPath = value; }
        }

        /// <summary>
        /// Путь к файлу с телом пакета
        /// </summary>
        private string bodyPath;
        public string BodyPath
        {
            get { return bodyPath; }
            set { bodyPath = value; }
        }

        /// <summary>
        /// Путь к файлу с откомпилированной декларацией
        /// </summary>
        private string declarationLibPath;
        public string DeclarationLibPath
        {
            get { return declarationLibPath; }
            set { declarationLibPath = value; }
        }

        /// <summary>
        /// Путь к файлу с откомпилированным телом пакета
        /// </summary>
        private string bodyLibPath;
        public string BodyLibPath
        {
            get { return bodyLibPath; }
            set { bodyLibPath = value; }
        }

        /// <summary>
        /// Декларация пакета
        /// </summary>
        private PackageDeclaration declaration;
        public PackageDeclaration Declaration
        {
            get
            {
                if (declaration == null)
                    declaration = LoadDeclaration(declarationLibPath);
                return declaration;
            }
            set
            {
                declaration = value;
                SaveDeclaration(declarationLibPath, declaration);
            }
        }

        /// <summary>
        /// Тело пакета
        /// </summary>
        private PackageBody body;
        public PackageBody Body
        {
            get
            {
                if (body == null)
                {
                    body = LoadBody(bodyLibPath);
                    body.PrimaryUnit = Declaration;
                    body.Package = Declaration;
                }
                return body;
            }
            set
            {
                body = value;
                SaveBody(bodyLibPath, body);
            }
        }

        /// <summary>
        /// Статическая функция для загрузки декларации пакета
        /// </summary>
        /// <param name="declarationLibPath"></param>
        /// <returns></returns>
        private static PackageDeclaration LoadDeclaration(string declarationLibPath)
        {
            if (File.Exists(declarationLibPath))
            {
                try
                {
                    Stream stream = new FileStream(declarationLibPath, FileMode.Open);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    PackageDeclaration res = (PackageDeclaration)deserializer.Deserialize(stream);
                    stream.Close();
                    return res;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Сериализация декларации в файл
        /// </summary>
        /// <param name="declarationLibPath"></param>
        /// <param name="declaration"></param>
        private static void SaveDeclaration(string declarationLibPath, PackageDeclaration declaration)
        {
            Stream stream = new FileStream(declarationLibPath, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, declaration);
            stream.Close();
        }

        /// <summary>
        /// Статическая функция для загрузки тела пакета
        /// </summary>
        /// <param name="declarationLibPath"></param>
        /// <returns></returns>
        private static PackageBody LoadBody(string bodyLibPath)
        {
            if (File.Exists(bodyLibPath))
            {
                try
                {
                    Stream stream = new FileStream(bodyLibPath, FileMode.Open);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    PackageBody res = (PackageBody)deserializer.Deserialize(stream);
                    stream.Close();
                    return res;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Сериализация декларации в файл
        /// </summary>
        /// <param name="declarationLibPath"></param>
        /// <param name="declaration"></param>
        private static void SaveBody(string bodyLibPath, PackageBody body)
        {
            Stream stream = new FileStream(bodyLibPath, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, body);
            stream.Close();
        }
    }
}
