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

namespace VHDLParser
{
    public class UseClauseInfo
    {
        /// <summary>
        /// Библиотека
        /// </summary>
        private string library;
        public string Library
        {
            get { return library; }
            set { library = value; }
        }

        /// <summary>
        /// Пакет
        /// </summary>
        private string package;
        public string Package
        {
            get { return package; }
            set { package = value; }
        }

        /// <summary>
        /// Подключаемый элемент пакета
        /// </summary>
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public UseClauseInfo(string library, string package, string content)
        {
            this.library = library;
            this.package = package;
            this.content = content;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(library))
                return string.Format("{0}.{1}", package, content);
            else
                return string.Format("{0}.{1}.{2}", library, package, content);
        }

        /// <summary>
        /// Распознавание Use
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static UseClauseInfo Parse(string text)
        {
            string[] elems = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (elems.Length == 2)
            {
                UseClauseInfo inf = new UseClauseInfo(string.Empty, elems[0], elems[1]);
                return inf;
            }
            if (elems.Length == 3)
            {
                UseClauseInfo inf = new UseClauseInfo(elems[0], elems[1], elems[2]);
                return inf;
            }
            throw new Exception("Invalid text");
        }
    }
}
