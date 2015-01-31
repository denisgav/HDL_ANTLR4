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
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VHDL.parser
{
    using Annotations = VHDL.Annotations;
    using LibraryDeclarativeRegion = VHDL.LibraryDeclarativeRegion;
    using RootDeclarativeRegion = VHDL.RootDeclarativeRegion;
    using VhdlFile = VHDL.VhdlFile;
    using Antlr4.Runtime;
    using VHDL.parser.util;
    using VHDL_ANTLR4;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// VHDL parser.
    /// </summary>
    public class VhdlParserWrapper
    {
        public static readonly VhdlParserSettings DEFAULT_SETTINGS = new VhdlParserSettings();

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private VhdlParserWrapper()
        {
        }

        private static VhdlFile parse(VhdlParserSettings settings, ICharStream stream, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager)
        {
            vhdlLexer lexer = new vhdlLexer(stream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            vhdlParser parser = new vhdlParser(tokens);

            //--------------------------------------------
            //Optional - add listener
            //vhdlListener listener = new vhdlListener();
            //parser.AddParseListener(listener);
            //--------------------------------------------
            vhdlSemanticErrorListener vhdlSemanticErrorListener = new vhdlSemanticErrorListener(stream.SourceName);
            parser.AddErrorListener(vhdlSemanticErrorListener);

            IParseTree tree = parser.design_file();
            //Console.WriteLine(tree.ToStringTree(parser));
            vhdlVisitor visitor = new vhdlVisitor(settings, rootScope, libraryScope, libraryManager) { FileName = stream.SourceName };
            VhdlFile res = visitor.Visit(tree) as VhdlFile;
            return res;
        }

        private static VhdlFile parse(VhdlParserSettings settings, ICharStream stream, VHDL_Library_Manager libraryManager)
        {
            RootDeclarativeRegion rootScope = new RootDeclarativeRegion();
            LibraryDeclarativeRegion libraryScope = new LibraryDeclarativeRegion("work");
            rootScope.Libraries.Add(libraryScope);

            return parse(settings, stream, rootScope, libraryScope, libraryManager);
        }

        public static VhdlFile parseFile(string fileName, VHDL_Library_Manager libraryManager)
        {
            return parseFile(fileName, DEFAULT_SETTINGS, libraryManager);
        }

        public static VhdlFile parseFile(string fileName, VhdlParserSettings settings, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveFileStream(fileName), libraryManager);
        }

        public static VhdlFile parseFile(string fileName, VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libray, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveFileStream(fileName), rootScope, libray, libraryManager);
        }

        public static VhdlFile parseString(string str, VHDL_Library_Manager libraryManager)
        {
            return parseString(str, DEFAULT_SETTINGS, libraryManager);
        }

        public static VhdlFile parseString(string str, VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libray, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveStringStream(str), rootScope, libray, libraryManager);
        }

        public static VhdlFile parseString(string str, VhdlParserSettings settings, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveStringStream(str), libraryManager);
        }

        public static VhdlFile parseStream(Stream stream, VHDL_Library_Manager libraryManager)
        {
            return parseStream(stream, DEFAULT_SETTINGS, libraryManager);
        }

        public static VhdlFile parseStream(Stream stream, VhdlParserSettings settings, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveInputStream(stream), libraryManager);
        }

        public static VhdlFile parseStream(Stream stream, VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libray, VHDL_Library_Manager libraryManager)
        {
            return parse(settings, new CaseInsensitiveInputStream(stream), rootScope, libray, libraryManager);
        }
    }
}