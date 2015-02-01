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

using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using VHDL;
using VHDL.libraryunit;
using VHDL.parser;
using VHDL.annotation;
using VHDL.concurrent;
using VHDL.statement;
using VHDL.util;
using VHDL.Object;

namespace VHDL_ANTLR4
{
    /// <summary>
    /// Description of vhdlVisitor.
    /// </summary>
    public abstract class vhdlAbstractVisitor : vhdlBaseVisitor<VhdlElement>
    {
        public vhdlAbstractVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager)
        {
            this.settings = settings;
            this.rootScope = rootScope;
            this.libraryScope = libraryScope;
            this.libraryScope.Parent = rootScope;
            fileName = string.Empty;
            this.libraryManager = libraryManager;
        }

        public vhdlAbstractVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager, string fileName)
            : this(settings, rootScope, libraryScope, libraryManager)
        {
            this.fileName = fileName;
        }

        protected internal IDeclarativeRegion currentScope = null;
        protected List<LibraryUnit> contextItems = new List<LibraryUnit>();
        protected internal readonly VhdlParserSettings settings;
        protected internal readonly LibraryDeclarativeRegion libraryScope;
        protected internal readonly RootDeclarativeRegion rootScope;
        protected internal VHDL_Library_Manager libraryManager;

        /// <summary>
        /// Path to the file (optional)
        /// </summary>
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        protected internal virtual VhdlParserSettings Settings
        {
            get { return settings; }
        }

        protected internal virtual T resolve<T>(string identifier) where T : class
        {
            if (currentScope != null)
            {
                T res = currentScope.Scope.resolve<T>(identifier);
                if (res == null)
                    throw new Exception(string.Format("Could not find item with name {0} and type {1}", identifier, typeof(T).FullName));
                else
                    return res;
            }

            throw new Exception(string.Format("Could not find item with name {0} and type {1}", identifier, typeof(T).FullName));
        }

        private SourcePosition TokenToPosition(IToken token, bool start)
        {
            CommonToken t = (CommonToken)token;
            int index = start ? t.StartIndex : t.StopIndex;
            return new SourcePosition(t.Line, t.Column, index);
        }

        private PositionInformation ContextToPosition(ParserRuleContext context)
        {
            return new PositionInformation(fileName, TokenToPosition(context.Start, true), TokenToPosition(context.Stop, false));
        }

        protected LibraryDeclarativeRegion LoadLibrary(string library)
        {
            //if (library.Equals("IEEE", StringComparison.CurrentCultureIgnoreCase))
            //    return builtin.Libraries.IEEE;
            //return null;
            if (libraryScope.Identifier.VHDLIdentifierEquals(library))
                return libraryScope;
            return libraryManager.GetLibrary(library);
        }

        /// <summary>
        /// Проверка процесса на содержание операторов Wait
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="process"></param>
        public bool CheckProcess(ParserRuleContext tree, ProcessStatement process)
        {
            int WaitCount = 0;
            foreach (SequentialStatement SeqStatement in process.Statements)
            {
                WaitCount += GetWaitCount(SeqStatement);
            }
            if (process.SensitivityList.Count > 0)
            { // no wait statement
                if (WaitCount > 0)
                {
                    throw new VHDL.ParseError.vhdlIllegalWaitInProcessException(tree, FileName);
                }
            }
            else
            { // at least one wait statement
                if (WaitCount == 0)
                {
                    throw new VHDL.ParseError.vhdlWaitStatementRequiredException(tree, FileName);
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка оператора use (поиск соответствующего пакета или элемента пакета)
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="useClause"></param>
        public bool CheckUseClause(ParserRuleContext tree, UseClause useClause)
        {
            List<string> declarations = useClause.getDeclarations();
            foreach (string declaration in declarations)
            {
                string[] elems = declaration.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if ((elems != null) && (elems.Length == 3))
                {
                    //Ищем библиотеку
                    string libraryName = elems[0];
                    IList<LibraryDeclarativeRegion> libraries = rootScope.Libraries;
                    foreach (LibraryDeclarativeRegion library in libraries)
                    {
                        if ((library != null) && (library.Identifier.VHDLIdentifierEquals(libraryName)))
                        {
                            //Нашли необходимую библиотеку
                            //Ищем пакет
                            string packageName = elems[1];
                            foreach (VhdlFile file in library.Files)
                            {
                                foreach (LibraryUnit unit in file.Elements)
                                {
                                    if (unit is PackageDeclaration)
                                    {
                                        PackageDeclaration packege = unit as PackageDeclaration;
                                        if (packege.Identifier.VHDLIdentifierEquals(packageName))
                                        {
                                            //Нашли необходимый пакет
                                            //Ищем нужный элемент
                                            string elemName = elems[2];
                                            if (elemName.VHDLIdentifierEquals("all"))
                                            {
                                                if (useClause.LinkedElements.Contains(packege) == false)
                                                    useClause.LinkedElements.Add(packege);
                                                return true;
                                            }
                                            object o = packege.Scope.resolveLocal(elemName);
                                            if ((o != null) && (o is INamedEntity))
                                            {
                                                INamedEntity el = o as INamedEntity;
                                                if (useClause.LinkedElements.Contains(el) == false)
                                                    useClause.LinkedElements.Add(el);
                                                return true;
                                            }
                                            else
                                            {
                                                throw new VHDL.ParseError.vhdlUnknownUseClauseItemException(tree, FileName, elemName);
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                            throw new VHDL.ParseError.vhdlUnknownUseClausePrimaryUnitException(tree, FileName, packageName);
                            return false;
                        }
                    }
                    throw new VHDL.ParseError.vhdlUnknownLibraryException(tree, FileName, libraryName);
                    return false;
                }
                else
                {
                    throw new VHDL.ParseError.vhdlIllegalUseClauseException(tree, FileName);
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка наличия библиотеки
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="useClause"></param>
        public bool CheckLibraryClause(ParserRuleContext tree, LibraryClause libraryClause)
        {
            foreach (string lib in libraryClause.getLibraries())
            {
                if (libraryManager.ContainsLibrary(lib) == false)
                {
                    throw new VHDL.ParseError.vhdlUnknownLibraryException(tree, FileName, lib);
                    return false;
                }
                else
                {
                    LibraryDeclarativeRegion libraryDecl = libraryManager.GetLibrary(lib);
                    if (libraryClause.LibraryDeclarativeRegion.Contains(libraryDecl) == false)
                        libraryClause.LibraryDeclarativeRegion.Add(libraryDecl);
                }
            }
            return true;
        }

        private int GetWaitCount(SequentialStatement SeqStatement)
        {
            int WaitCount = 0;
            if (SeqStatement is WaitStatement)
                return 1;
            foreach (VhdlElement el in SeqStatement.GetAllStatements())
                if (el is SequentialStatement)
                    WaitCount += GetWaitCount(el as SequentialStatement);
            return WaitCount;
        }

        private void AddPositionAnnotation(VhdlElement element, ParserRuleContext context)
        {
            PositionInformation info = ContextToPosition(context);
            Annotations.putAnnotation(element, info);
        }

        private void AddCommentAnnotation(VhdlElement element, ParserRuleContext context)
        {
            List<string> comments = null;
            ITerminalNode[] commentTermnals = context.GetTokens(vhdlParser.COMMENT);

            if (commentTermnals.Length != 0)
            {
                comments = new List<string>();

                foreach (ITerminalNode t in commentTermnals)
                {
                    comments.Add(t.ToString());
                }
            }


            if (comments != null && comments.Count != 0)
            {
                Comments.SetComments(element, new List<string>(comments));
            }
        }

        protected internal virtual void AddAnnotations(VhdlElement element, ParserRuleContext context)
        {
            if (element == null || context == null)
            {
                return;
            }

            if (settings.AddPositionInformation)
            {
                AddPositionAnnotation(element, context);
            }

            if (settings.ParseComments)
            {
                AddCommentAnnotation(element, context);
            }
        }

        public static Signal.ModeEnum ParseSignalMode(VHDL_ANTLR4.vhdlParser.Signal_modeContext context)
        {
            return ((context == null) ? Signal.ModeEnum.IN : (Signal.ModeEnum)Enum.Parse(typeof(Signal.ModeEnum), context.GetText().ToUpper()));
        }

        public VHDL.type.ISubtypeIndication FindSubtypeIndication(string name)
        {
            return currentScope.Scope.resolve<VHDL.type.ISubtypeIndication>(name);
        }
    }
}