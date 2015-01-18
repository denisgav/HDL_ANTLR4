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
            vhdlListener listener = new vhdlListener();
            vhdlParser parser = new vhdlParser(tokens);
            parser.AddParseListener(listener);
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

        public static bool hasParseErrors(VhdlFile file)
        {
            return Annotations.getAnnotation<ParseErrors>(file) != null;
        }

        public static List<ParseError> getParseErrors(VhdlFile file)
        {
            ParseErrors errors = Annotations.getAnnotation<ParseErrors>(file);
            if (errors == null)
            {
                return new List<ParseError>();
            }
            else
            {
                return errors.Errors;
            }
        }

        public static string errorToMessage(Exception ex)
        {
            return "";
        }

        private static string FormatExceptionText(string error_string)
        {
            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens.Add("DOUBLESTAR", "**");
            tokens.Add("LE", "<=");
            tokens.Add("GE", ">=");
            tokens.Add("ARROW", "=>");
            tokens.Add("NEQ", "/=");
            tokens.Add("VARASGN", ":=");
            tokens.Add("BOX", "<>");
            tokens.Add("DBLQUOTE", "\"");
            tokens.Add("SEMI", ";");
            tokens.Add("COMMA", ",");
            tokens.Add("AMPERSAND", "&");
            tokens.Add("LPAREN", "(");
            tokens.Add("RPAREN", ")");
            tokens.Add("LBRACKET", "[");
            tokens.Add("RBRACKET", "]");
            tokens.Add("COLON", ":");
            tokens.Add("MUL", "*");
            tokens.Add("DIV", "/");
            tokens.Add("PLUS", "+");
            tokens.Add("MINUS", "-");
            tokens.Add("LT", "<");
            tokens.Add("GT", ">");
            tokens.Add("EQ", "=");
            tokens.Add("BAR", "|");
            tokens.Add("EXCLAMATION", "!");
            tokens.Add("DOT", ".");
            tokens.Add("BACKSLASH", "\\");
            tokens.Add("EOF", "end of file");

            foreach (KeyValuePair<string, string> pair in tokens)
            {
                error_string = Regex.Replace(error_string, pair.Key, pair.Value);
            }
            return error_string;
        }

        public static string errorToMessage(ParseError error)
        {
            switch (error.Type)
            {
                case ParseError.ParseErrorTypeEnum.UNKNOWN_COMPONENT:
                    return string.Format("Line: {0}, {1} - unknown component: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_CONFIGURATION:
                    return string.Format("Line: {0}, {1} - unknown configuration: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_CONSTANT:
                    return string.Format("Line: {0}, {1} - unknown constant: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_ENTITY:
                    return string.Format("Line: {0}, {1} - unknown entity: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_FILE:
                    return string.Format("Line: {0}, {1} - unknown file: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_LOOP:
                    return string.Format("Line: {0}, {1} - unknown loop: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_CASE:
                    return string.Format("Line: {0}, {1} - unknown case: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_IF:
                    return string.Format("Line: {0}, {1} - unknown if: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_OTHER:
                    return string.Format("Line: {0}, {1} - unknown identifier: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_PROCESS:
                    return string.Format("Line: {0}, {1} - unknown process: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_PACKAGE:
                    return string.Format("Line: {0}, {1} - unknown pacakge: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_SIGNAL:
                    return string.Format("Line: {0}, {1} - unknown signal: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_SIGNAL_ASSIGNMENT_TARGET:
                    return string.Format("Line: {0}, {1} - unknown signal assignment target: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_TYPE:
                    return string.Format("Line: {0}, {1} - unknown type: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_VARIABLE:
                    return string.Format("Line: {0}, {1} - unknown variable: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_VARIABLE_ASSIGNMENT_TARGET:
                    return string.Format("Line: {0}, {1} - unknown variable assignment target: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.PROCESS_TYPE_ERROR:
                    return string.Format("Line: {0}, {1} - process type error: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_ARCHITECTURE:
                    return string.Format("Line: {0}, {1} - unknown architecture: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                case ParseError.ParseErrorTypeEnum.UNKNOWN_GENERATE_STATEMENT:
                    return string.Format("Line: {0}, {1} - unknown generate statement: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
                default:
                    return string.Format("Line: {0}, {1} - unknown error: {2}", error.Position.Begin.Line, error.Position.Begin.Column, error.Message);
            }
        }

        private static void reportErrors(List<ParseError> errors)
        {
            foreach (ParseError error in errors)
            {
                Console.Error.WriteLine("line " + error.Position.Begin.Line + ": " + errorToMessage(error));
            }

        }
    }
}