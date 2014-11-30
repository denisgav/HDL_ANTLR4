/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 22.11.2014
 * Time: 21:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using VHDL;
using Antlr4.Runtime.Misc;
using VHDL.libraryunit;

namespace VHDL_ANTLR4
{
    using VHDL.builtin;
    using VHDL.concurrent;
    using VHDL.statement;
    using VHDL.libraryunit;
    using VHDL.expression;

    using Annotations = VHDL.Annotations;
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using LibraryDeclarativeRegion = VHDL.LibraryDeclarativeRegion;
    using RootDeclarativeRegion = VHDL.RootDeclarativeRegion;
    using VhdlElement = VHDL.VhdlElement;
    using DeclarativeItemMarker = VHDL.declaration.IDeclarativeItemMarker;
    using VhdlParserSettings = VHDL.parser.VhdlParserSettings;
    using ParseError = VHDL.parser.ParseError;
    using PositionInformation = VHDL.annotation.PositionInformation;
    using SourcePosition = VHDL.annotation.SourcePosition;
    using Comments = VHDL.util.Comments;
    using System.Collections.Generic;
    using VHDL.parser;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using VHDL.Object;
    using VHDL.annotation;
    using VHDL.declaration;
    using VHDL.literal;

    /// <summary>
    /// Description of vhdlVisitor.
    /// </summary>
    public class vhdlVisitor : vhdlBaseVisitor<VhdlElement>
    {
        static Out Cast<In, Out>(In in_data)
            where In : class
            where Out : class
        {
            Type in_type = typeof(In);
            Type out_type = typeof(Out);
            if (in_data == null)
                throw new ArgumentNullException(string.Format("Null Object access when tried to cast {0} to {1}", in_type.Name, out_type.Name));
            Out res = in_data as Out;

            if (res == null)
                throw new InvalidCastException(string.Format("Failed cast {0} to {1}", in_type.Name, out_type.Name));
            return res;
        }


        private readonly List<ParseError> errors = new List<ParseError>();
        protected internal DeclarativeRegion currentScope = null;
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

        public vhdlVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager)
        {
            this.settings = settings;
            this.rootScope = rootScope;
            this.libraryScope = libraryScope;
            this.libraryScope.Parent = rootScope;
            fileName = string.Empty;
            this.libraryManager = libraryManager;
        }

        public vhdlVisitor(VhdlParserSettings settings, RootDeclarativeRegion rootScope, LibraryDeclarativeRegion libraryScope, VHDL_Library_Manager libraryManager, string fileName)
            : this(settings, rootScope, libraryScope, libraryManager)
        {
            this.fileName = fileName;
        }

        protected internal virtual VhdlParserSettings Settings
        {
            get { return settings; }
        }

        protected internal virtual T resolve<T>(string identifier) where T : class
        {
            if (currentScope != null)
            {
                return currentScope.Scope.resolve<T>(identifier);
            }

            return null;
        }

        private SourcePosition tokenToPosition(IToken token, bool start)
        {
            CommonToken t = (CommonToken)token;
            int index = start ? t.StartIndex : t.StopIndex;
            return new SourcePosition(t.Line, t.Column, index);
        }

        public virtual List<ParseError> getErrors()
        {
            return errors;
        }

        
        public static void AddRange<E>(IList<E> collection1, IList<E> collection2) where E : class
        {
            foreach (E e in collection2)
                collection1.Add(e);
        }

        protected internal virtual void resolveError(ParserRuleContext context, ParseError.ParseErrorTypeEnum type, string identifier)
        {
            if (settings.EmitResolveErrors)
            {
                PositionInformation pos = contextToPosition(context);
                errors.Add(new ParseError(pos, type, identifier));
            }
        }

        private PositionInformation contextToPosition(ParserRuleContext context)
        {
            return new PositionInformation(fileName, tokenToPosition(context.Start, true), tokenToPosition(context.Stop, false));
        }

        protected LibraryDeclarativeRegion LoadLibrary(string library)
        {
            //if (library.Equals("IEEE", StringComparison.CurrentCultureIgnoreCase))
            //    return builtin.Libraries.IEEE;
            //return null;
            if (libraryScope.Identifier.Equals(library, StringComparison.InvariantCultureIgnoreCase))
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
                    resolveError(tree, ParseError.ParseErrorTypeEnum.PROCESS_TYPE_ERROR, "wait statement not allowed");
                    return false;
                }
            }
            else
            { // at least one wait statement
                if (WaitCount == 0)
                {
                    resolveError(tree, ParseError.ParseErrorTypeEnum.PROCESS_TYPE_ERROR, "wait statement required");
                    return false;
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
                        if ((library != null) && (library.Identifier.Equals(libraryName, StringComparison.InvariantCultureIgnoreCase)))
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
                                        if (packege.Identifier.Equals(packageName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            //Нашли необходимый пакет
                                            //Ищем нужный элемент
                                            string elemName = elems[2];
                                            if (elemName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
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
                                                resolveError(tree, ParseError.ParseErrorTypeEnum.UNKNOWN_OTHER, "Incorrect use clause (item )");
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                            resolveError(tree, ParseError.ParseErrorTypeEnum.UNKNOWN_OTHER, "Incorrect use clause (primary unit name )");
                            return false;
                        }
                    }
                    resolveError(tree, ParseError.ParseErrorTypeEnum.UNKNOWN_OTHER, "Incorrect use clause (library name)");
                    return false;
                }
                else
                {
                    resolveError(tree, ParseError.ParseErrorTypeEnum.UNKNOWN_PACKAGE, "Incorrect use clause");
                    return false;
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
                    resolveError(tree, ParseError.ParseErrorTypeEnum.UNKNOWN_OTHER, string.Format("Incorrect library clause, unknown library {0})", lib));
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
            PositionInformation info = contextToPosition(context);
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
            return currentScope.Scope.resolve< VHDL.type.ISubtypeIndication > (name);
        }
        
        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.assertion_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssertion_statement([NotNull] vhdlParser.Assertion_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_kind"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_kind([NotNull] vhdlParser.Subprogram_kindContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.association_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssociation_list([NotNull] vhdlParser.Association_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.unconstrained_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitUnconstrained_nature_definition([NotNull] vhdlParser.Unconstrained_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_header"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_header([NotNull] vhdlParser.Entity_headerContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.sensitivity_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSensitivity_list([NotNull] vhdlParser.Sensitivity_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_statement_part([NotNull] vhdlParser.Simultaneous_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_waveforms"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_waveforms([NotNull] vhdlParser.Conditional_waveformsContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.sequential_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSequential_statement([NotNull] vhdlParser.Sequential_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_quantity_declaration([NotNull] vhdlParser.Interface_quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.terminal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTerminal_declaration([NotNull] vhdlParser.Terminal_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.tolerance_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTolerance_aspect([NotNull] vhdlParser.Tolerance_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subnature_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubnature_declaration([NotNull] vhdlParser.Subnature_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signature"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignature([NotNull] vhdlParser.SignatureContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_statement([NotNull] vhdlParser.Simultaneous_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.port_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPort_list([NotNull] vhdlParser.Port_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.instantiation_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInstantiation_list([NotNull] vhdlParser.Instantiation_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.quantity_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQuantity_list([NotNull] vhdlParser.Quantity_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.parameter_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitParameter_specification([NotNull] vhdlParser.Parameter_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.identifier_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIdentifier_list([NotNull] vhdlParser.Identifier_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_declarative_part([NotNull] vhdlParser.Block_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.record_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRecord_type_definition([NotNull] vhdlParser.Record_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.multiplying_operator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitMultiplying_operator([NotNull] vhdlParser.Multiplying_operatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.generic_map_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGeneric_map_aspect([NotNull] vhdlParser.Generic_map_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_list([NotNull] vhdlParser.Signal_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.branch_quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBranch_quantity_declaration([NotNull] vhdlParser.Branch_quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.function_call"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFunction_call([NotNull] vhdlParser.Function_callContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.timeout_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTimeout_clause([NotNull] vhdlParser.Timeout_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_name_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_name_list([NotNull] vhdlParser.Entity_name_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.object_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitObject_declaration([NotNull] vhdlParser.Object_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_waveforms_bi"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_waveforms_bi([NotNull] vhdlParser.Conditional_waveforms_biContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.choice"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitChoice([NotNull] vhdlParser.ChoiceContext context) 
        {
            var discrete_range = context.discrete_range();
            var identifier = context.identifier();
            var OTHERS = context.OTHERS();
            var simple_expression = context.simple_expression();

            if (discrete_range != null)
            {
                return VisitDiscrete_range(discrete_range);
            }

            if (identifier != null)
            {
                return resolve<VhdlElement>(identifier.GetText());
            }

            if (OTHERS != null)
            {
                return Choices.OTHERS;
            }

            if (simple_expression != null)
            {
                return VisitSimple_expression(simple_expression);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.generate_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGenerate_statement([NotNull] vhdlParser.Generate_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.alias_designator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAlias_designator([NotNull] vhdlParser.Alias_designatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_statement([NotNull] vhdlParser.Entity_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.sensitivity_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSensitivity_clause([NotNull] vhdlParser.Sensitivity_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.alias_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAlias_declaration([NotNull] vhdlParser.Alias_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.attribute_designator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAttribute_designator([NotNull] vhdlParser.Attribute_designatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_alternative"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_alternative([NotNull] vhdlParser.Simultaneous_alternativeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_body([NotNull] vhdlParser.Architecture_bodyContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_tag"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_tag([NotNull] vhdlParser.Entity_tagContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subtype_indication"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubtype_indication([NotNull] vhdlParser.Subtype_indicationContext context) 
        {
            
            var constraint = context.constraint();
            var tolerance_aspect = context.tolerance_aspect();
            var name =  context.name()[0];
            string string_name = name.GetText();
            var resolution_function = (context.name().Length == 2) ? context.name()[1] : null;

            VHDL.type.ISubtypeIndication res = resolve<VHDL.type.ISubtypeIndication>(string_name);
            if (resolution_function != null)
            {
                string resolution_function_name = resolution_function.GetText();
                res = new VHDL.type.ResolvedSubtypeIndication(resolution_function_name, res);
            }

            if (constraint != null)
            {
                var range_constraint = constraint.range_constraint();
                if (range_constraint != null)
                {
                    RangeProvider range = Cast<VhdlElement, RangeProvider>(VisitRange(range_constraint.range()));
                    res = new VHDL.type.RangeSubtypeIndication(res, range);
                }

                var index_constraint = constraint.index_constraint();
                if (index_constraint != null)
                {
                    //index_constraint.discrete_range
                    DiscreteRange[] ranges = new DiscreteRange[index_constraint.discrete_range().Length];
                    for(int i=0; i<index_constraint.discrete_range().Length; i++)
                    {
                        ranges[i] = Cast<VhdlElement, DiscreteRange>(VisitDiscrete_range(index_constraint.discrete_range()[i]));
                    }
                    res = new VHDL.type.IndexSubtypeIndication(res, ranges);
                }


                throw new NotSupportedException(String.Format("Could not analyse item {0}", constraint.ToStringTree()));
            }

            VhdlElement return_value = res as VhdlElement;

            return return_value; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.process_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcess_statement([NotNull] vhdlParser.Process_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_aspect([NotNull] vhdlParser.Entity_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.choices"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitChoices([NotNull] vhdlParser.ChoicesContext context) 
        {
            var choices_in = context.choice();
            List<Choice> ch = new List<Choice>();
            foreach (var curr_choice in choices_in)
            {
                Choice c = Cast<VhdlElement, Choice>(VisitChoice(curr_choice));
                ch.Add(c);
            }
            return new Choices(ch);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.design_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDesign_unit([NotNull] vhdlParser.Design_unitContext context) 
        {
            var context_clause = context.context_clause();
            var library_unit = context.library_unit();

            LibraryUnit res = null;

            if (context_clause != null)
            {
                VhdlElement clause = VisitContext_clause(context_clause);
            }

            if (library_unit != null)
            {
                res = Cast<VhdlElement, LibraryUnit>(VisitLibrary_unit(library_unit));
                return res;

            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.factor"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFactor([NotNull] vhdlParser.FactorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.relational_operator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRelational_operator([NotNull] vhdlParser.Relational_operatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.index_subtype_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIndex_subtype_definition([NotNull] vhdlParser.Index_subtype_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_body([NotNull] vhdlParser.Subprogram_bodyContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.delay_mechanism"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDelay_mechanism([NotNull] vhdlParser.Delay_mechanismContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.process_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcess_declarative_item([NotNull] vhdlParser.Process_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.group_template_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGroup_template_declaration([NotNull] vhdlParser.Group_template_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_body([NotNull] vhdlParser.Package_bodyContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.range_constraint"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRange_constraint([NotNull] vhdlParser.Range_constraintContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.secondary_unit_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSecondary_unit_declaration([NotNull] vhdlParser.Secondary_unit_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_body_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_body_declarative_part([NotNull] vhdlParser.Package_body_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_call_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedure_call_statement([NotNull] vhdlParser.Procedure_call_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitExpression([NotNull] vhdlParser.ExpressionContext context) 
        {
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.abstract_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAbstract_literal([NotNull] vhdlParser.Abstract_literalContext context) 
        {
            var integer_literal = context.INTEGER();
            var real_literal = context.REAL_LITERAL();
            var based_literal = context.BASE_LITERAL();

            if (integer_literal != null)
            {
                DecBasedInteger res = new DecBasedInteger(integer_literal.GetText());
                return res;
            }

            if (real_literal != null)
            {
                RealLiteral real = new RealLiteral(real_literal.GetText());
                return real;
            }

            if (based_literal != null)
            {
                string text = based_literal.GetText();
                switch(text.Split('#')[0])
                {
                    case "2":
                        {
                            BinaryBasedInteger res = new BinaryBasedInteger(text);
                            return res;
                        }
                        break;
                    case "8":
                        {
                            OctalBasedInteger res = new OctalBasedInteger(text);
                            return res;
                        }
                        break;
                    case "16":
                        {
                            HexBasedInteger res = new HexBasedInteger(text);
                            return res;
                        }
                        break;
                    default:
                        throw new NotSupportedException(String.Format("Could not analyse item {0}", based_literal.ToStringTree()));
                        break;
                }
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_variable_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_variable_declaration([NotNull] vhdlParser.Interface_variable_declarationContext context) 
        {
            bool hasObjectClass = context.VARIABLE() != null;
            bool hasMode = context.signal_mode() != null;

            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication_in));
            var def_value_in = context.expression();
            Expression def_value = Cast<VhdlElement, Expression>(VisitExpression(def_value_in));

            var identifiers_in = context.identifier_list();
            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);
            VariableGroup res = new VariableGroup();

            VhdlObject.ModeEnum mode = (hasMode)? (VhdlObject.ModeEnum)(Enum.Parse(typeof(VhdlObject.ModeEnum), context.signal_mode().GetText().ToUpper())) : VhdlObject.ModeEnum.IN;

            foreach (var identifier in identifiers_in.identifier())
            {
                Variable v = new Variable(identifier.GetText(), si, def_value);
                if (hasMode)
                {
                    v.Mode = mode;
                }
                Annotations.putAnnotation(v, format);

                res.Elements.Add(v);
            }

            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.next_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNext_statement([NotNull] vhdlParser.Next_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.scalar_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitScalar_type_definition([NotNull] vhdlParser.Scalar_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constant_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstant_declaration([NotNull] vhdlParser.Constant_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.component_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComponent_declaration([NotNull] vhdlParser.Component_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_file_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_file_declaration([NotNull] vhdlParser.Interface_file_declarationContext context) 
        {
            FileGroup res = new FileGroup();
            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication_in));
            var identifier_list_in = context.identifier_list();
            foreach (var identifier in identifier_list_in.identifier())
            {
                FileObject obj = new FileObject(identifier.GetText(), si);
                res.Elements.Add(obj);
            }

            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_break_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_break_statement([NotNull] vhdlParser.Concurrent_break_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.context_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitContext_item([NotNull] vhdlParser.Context_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.configuration_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConfiguration_specification([NotNull] vhdlParser.Configuration_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.association_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssociation_element([NotNull] vhdlParser.Association_elementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.condition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCondition([NotNull] vhdlParser.ConditionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.case_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCase_statement([NotNull] vhdlParser.Case_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.logical_name_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLogical_name_list([NotNull] vhdlParser.Logical_name_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.relation"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRelation([NotNull] vhdlParser.RelationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constrained_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstrained_nature_definition([NotNull] vhdlParser.Constrained_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_signal_assignment"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_signal_assignment([NotNull] vhdlParser.Conditional_signal_assignmentContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.process_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcess_declarative_part([NotNull] vhdlParser.Process_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.waveform"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitWaveform([NotNull] vhdlParser.WaveformContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.port_map_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPort_map_aspect([NotNull] vhdlParser.Port_map_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQuantity_declaration([NotNull] vhdlParser.Quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_statement([NotNull] vhdlParser.Architecture_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.component_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComponent_specification([NotNull] vhdlParser.Component_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.logical_operator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLogical_operator([NotNull] vhdlParser.Logical_operatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.source_quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSource_quantity_declaration([NotNull] vhdlParser.Source_quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.identifier"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIdentifier([NotNull] vhdlParser.IdentifierContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.composite_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComposite_type_definition([NotNull] vhdlParser.Composite_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedural_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedural_declarative_item([NotNull] vhdlParser.Procedural_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_declarative_part([NotNull] vhdlParser.Entity_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_case_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_case_statement([NotNull] vhdlParser.Simultaneous_case_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_mode"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_mode([NotNull] vhdlParser.Signal_modeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_configuration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_configuration([NotNull] vhdlParser.Block_configurationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.physical_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPhysical_literal([NotNull] vhdlParser.Physical_literalContext context) 
        {
            var abstract_literal = context.abstract_literal();
            var name = context.name();
            AbstractLiteral al = Cast<VhdlElement, AbstractLiteral>(VisitAbstract_literal(abstract_literal));
            PhysicalLiteral  physLiteral = resolve<PhysicalLiteral>(name.GetText());
            PhysicalLiteral res = new PhysicalLiteral(al, name.GetText(), physLiteral.GetPhysicalType());
            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.enumeration_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEnumeration_literal([NotNull] vhdlParser.Enumeration_literalContext context) 
        {
            var identifier = context.identifier();
            var character_literal = context.CHARACTER_LITERAL();

            if (identifier != null)
            {
                EnumerationLiteral literal = resolve<EnumerationLiteral>(identifier.GetText());
                if (literal != null)
                    return literal;
                else
                    return resolve<VhdlElement>(identifier.GetText());
            }

            if (character_literal != null)
            {
                CharacterLiteral literal = resolve<CharacterLiteral>(identifier.GetText());
                return literal;
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_constant_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_constant_declaration([NotNull] vhdlParser.Interface_constant_declarationContext context) 
        {
            bool hasObjectClass = context.CONSTANT() != null;
            bool hasMode = context.IN() != null;

            var def_value_in = context.expression();
            Expression def_value = Cast<VhdlElement, Expression>(VisitExpression(def_value_in));

            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication_in));

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);

            ConstantGroup res = new ConstantGroup();
            foreach(var i in context.identifier_list().identifier())
            {
                Constant c = new Constant(i.GetText(), si, def_value);
                Annotations.putAnnotation(c, format);

                res.Elements.Add(c);
            }

            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.name"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitName([NotNull] vhdlParser.NameContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_declaration([NotNull] vhdlParser.Package_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_class_entry"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_class_entry([NotNull] vhdlParser.Entity_class_entryContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.group_constituent"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGroup_constituent([NotNull] vhdlParser.Group_constituentContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.unconstrained_array_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitUnconstrained_array_definition([NotNull] vhdlParser.Unconstrained_array_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_header"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_header([NotNull] vhdlParser.Block_headerContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNature_definition([NotNull] vhdlParser.Nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_kind"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_kind([NotNull] vhdlParser.Signal_kindContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_logical_name"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_logical_name([NotNull] vhdlParser.File_logical_nameContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.quantity_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQuantity_specification([NotNull] vhdlParser.Quantity_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.assertion"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAssertion([NotNull] vhdlParser.AssertionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_body_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_body_declarative_item([NotNull] vhdlParser.Package_body_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.group_constituent_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGroup_constituent_list([NotNull] vhdlParser.Group_constituent_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.source_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSource_aspect([NotNull] vhdlParser.Source_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.composite_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComposite_nature_definition([NotNull] vhdlParser.Composite_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_declarative_item([NotNull] vhdlParser.Subprogram_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.through_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitThrough_aspect([NotNull] vhdlParser.Through_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.array_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArray_type_definition([NotNull] vhdlParser.Array_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.nature_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNature_declaration([NotNull] vhdlParser.Nature_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_declaration([NotNull] vhdlParser.Entity_declarationContext context) 
        {
            IDeclarativeRegion oldScope = this.currentScope;            
            //--------------------------------------
            VHDL_ANTLR4.vhdlParser.IdentifierContext[] identifiers = context.identifier();

            if (identifiers.Length == 2)
            {
                string begin_identifier = identifiers[0].ToString();
                string end_identifier = identifiers[1].ToString();
                if (begin_identifier.Equals(end_identifier, StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception(string.Format("Self check failure. Entity declaration identifier is {0}, End identifier is {1}.", begin_identifier, end_identifier));
            }

            VHDL_ANTLR4.vhdlParser.IdentifierContext identifier = identifiers[0];
            
            Entity res = new Entity(identifier.GetText());
            res.Parent = oldScope;
            currentScope = res;

            var entity_header = context.entity_header();
            var entity_declarative_part = context.entity_declarative_part();
            var entity_statement_part = context.entity_statement_part();

            //1. Visit all declaration parts (generics and ports)
            var port_clause = entity_header.port_clause();
            if (port_clause != null)
            {
                foreach (var port in port_clause.port_list().interface_signal_list().interface_signal_declaration())
                {
                    res.Port.Add(Cast<VhdlElement, IVhdlObjectProvider>(VisitInterface_signal_declaration(port)));
                }
            }

            var generic_clause = entity_header.generic_clause();
            if (generic_clause != null)
            {
                foreach (var generic in generic_clause.generic_list().interface_constant_declaration())
                {
                    res.Generic.Add(Cast<VhdlElement, IVhdlObjectProvider>(VisitInterface_constant_declaration(generic)));
                }
            }

            //--------------------------------------
            currentScope = oldScope;
            AddAnnotations(res, context);
            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.aggregate"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAggregate([NotNull] vhdlParser.AggregateContext context) 
        {
            bool has_choises = context.element_association()[0].ARROW() != null;
            Aggregate res = new Aggregate();

            foreach (var aggregate_item in context.element_association())
            {
                if ((has_choises == true) && (aggregate_item.ARROW() == null))
                {
                    throw new Exception(string.Format("Expression {0} should hawe all choises, or not use them in all cases", context.ToStringTree()));
                }
                var expression_in = aggregate_item.expression();
                Expression exp = Cast<VhdlElement, Expression>(VisitExpression(expression_in));

                if (has_choises)
                {
                    var choices_in = aggregate_item.choices();
                    List<Choice> ch = new List<Choice>();
                    foreach (var curr_choice in choices_in.choice())
                    {
                        Choice c = Cast<VhdlElement, Choice>(VisitChoice(curr_choice));
                        ch.Add(c);
                    }
                    res.CreateAssociation(exp, ch);
                }
                else
                {
                    res.CreateAssociation(exp);
                }
            }
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_designator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_designator([NotNull] vhdlParser.Entity_designatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.case_statement_alternative"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCase_statement_alternative([NotNull] vhdlParser.Case_statement_alternativeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.binding_indication"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBinding_indication([NotNull] vhdlParser.Binding_indicationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.component_configuration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComponent_configuration([NotNull] vhdlParser.Component_configurationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.designator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDesignator([NotNull] vhdlParser.DesignatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_element([NotNull] vhdlParser.Interface_elementContext context) 
        {
            var interface_declaration = context.interface_declaration();
            return VisitInterface_declaration(interface_declaration);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_statement_part([NotNull] vhdlParser.Architecture_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_declarative_item([NotNull] vhdlParser.Block_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_assignment_statement([NotNull] vhdlParser.Signal_assignment_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.element_subtype_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitElement_subtype_definition([NotNull] vhdlParser.Element_subtype_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedural_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedural_statement_part([NotNull] vhdlParser.Procedural_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.component_instantiation_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComponent_instantiation_statement([NotNull] vhdlParser.Component_instantiation_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_specification([NotNull] vhdlParser.Block_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.step_limit_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitStep_limit_specification([NotNull] vhdlParser.Step_limit_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.formal_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFormal_part([NotNull] vhdlParser.Formal_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.primary_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPrimary_unit([NotNull] vhdlParser.Primary_unitContext context) 
        {
            var configuration_declaration = context.configuration_declaration();
            var entity_declaration = context.entity_declaration();
            var package_declaration = context.package_declaration();

            if (configuration_declaration != null)
            {
                return VisitConfiguration_declaration(configuration_declaration);
            }

            if (entity_declaration != null)
            {
                return VisitEntity_declaration(entity_declaration);
            }

            if (package_declaration != null)
            {
                return VisitPackage_declaration(package_declaration);
            }
            
            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.configuration_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConfiguration_declarative_part([NotNull] vhdlParser.Configuration_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_declarative_part([NotNull] vhdlParser.Package_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.shift_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitShift_expression([NotNull] vhdlParser.Shift_expressionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.iteration_scheme"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIteration_scheme([NotNull] vhdlParser.Iteration_schemeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_procedure_call_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_procedure_call_statement([NotNull] vhdlParser.Concurrent_procedure_call_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.discrete_range"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDiscrete_range([NotNull] vhdlParser.Discrete_rangeContext context) 
        {
            var range = context.range();
            var subtype_indication = context.subtype_indication();

            if (range != null)
            {
                return VisitRange(range);
            }

            if (subtype_indication != null)
            {
                VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication));
                SubtypeDiscreteRange res = new SubtypeDiscreteRange(si);
                return res;
            }
            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.element_association"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitElement_association([NotNull] vhdlParser.Element_associationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subtype_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubtype_declaration([NotNull] vhdlParser.Subtype_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_specification([NotNull] vhdlParser.Subprogram_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.range"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRange([NotNull] vhdlParser.RangeContext context) 
        {
            RangeProvider res = null;

            var name = context.name();
            if(name != null)
            {
                res = resolve<RangeProvider>(name.GetText());
                return res;
            }
            else
            {
                if ((context.simple_expression().Length == 2) && (context.direction() != null))
                {
                    var expression_left_in = context.simple_expression()[0];
                    var expression_right_in = context.simple_expression()[1];
                    var direction_in = context.direction();

                    Range.RangeDirection direction = (direction_in.GetText().Equals("To", StringComparison.InvariantCultureIgnoreCase)) ? Range.RangeDirection.TO : Range.RangeDirection.DOWNTO;
                    Expression expression_left = Cast<VhdlElement, Expression>(VisitSimple_expression(expression_left_in));
                    Expression expression_right = Cast<VhdlElement, Expression>(VisitSimple_expression(expression_right_in));

                    res = new Range(expression_left, direction, expression_right);
                    return res;
                }
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.variable_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitVariable_assignment_statement([NotNull] vhdlParser.Variable_assignment_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.if_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIf_statement([NotNull] vhdlParser.If_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constraint"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstraint([NotNull] vhdlParser.ConstraintContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.break_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBreak_element([NotNull] vhdlParser.Break_elementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.configuration_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConfiguration_item([NotNull] vhdlParser.Configuration_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_statement_part([NotNull] vhdlParser.Block_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.physical_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPhysical_type_definition([NotNull] vhdlParser.Physical_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.configuration_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConfiguration_declaration([NotNull] vhdlParser.Configuration_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.logical_name"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLogical_name([NotNull] vhdlParser.Logical_nameContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedural_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedural_declarative_part([NotNull] vhdlParser.Procedural_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.variable_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitVariable_declaration([NotNull] vhdlParser.Variable_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.base_unit_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBase_unit_declaration([NotNull] vhdlParser.Base_unit_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_declaration([NotNull] vhdlParser.Signal_declarationContext context) 
        {
            var identifier_list = context.identifier_list();
            var subtype_indication = context.subtype_indication();
            var expression = context.expression();
            var kind_in = context.signal_kind();
            Signal.KindEnum kind = (kind_in != null)?(Signal.KindEnum)Enum.Parse(typeof(Signal.KindEnum), kind_in.GetText().ToUpper()) : Signal.KindEnum.DEFAULT;

            Expression def = (expression != null) ? Cast<VhdlElement, Expression>(VisitExpression(expression)) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication != null) ? Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication)) : null;

            SignalDeclaration res = new SignalDeclaration();

            foreach (var identifier in identifier_list.identifier())
            {
                string signal_name = identifier.GetText();
                Signal s = new Signal(signal_name, type, def);
                
                s.Kind = kind;                

                res.Objects.Add(s);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            res.Parent = currentScope;
            return res; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simple_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimple_expression([NotNull] vhdlParser.Simple_expressionContext context) 
        {

            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.actual_parameter_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitActual_parameter_part([NotNull] vhdlParser.Actual_parameter_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.break_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBreak_list([NotNull] vhdlParser.Break_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.instantiated_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInstantiated_unit([NotNull] vhdlParser.Instantiated_unitContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_class_entry_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_class_entry_list([NotNull] vhdlParser.Entity_class_entry_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_terminal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_terminal_declaration([NotNull] vhdlParser.Interface_terminal_declarationContext context) 
        {
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.adding_operator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAdding_operator([NotNull] vhdlParser.Adding_operatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.use_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitUse_clause([NotNull] vhdlParser.Use_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.return_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitReturn_statement([NotNull] vhdlParser.Return_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.enumeration_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEnumeration_type_definition([NotNull] vhdlParser.Enumeration_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.port_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPort_clause([NotNull] vhdlParser.Port_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.constrained_array_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConstrained_array_definition([NotNull] vhdlParser.Constrained_array_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.index_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIndex_specification([NotNull] vhdlParser.Index_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.allocator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAllocator([NotNull] vhdlParser.AllocatorContext context) 
        {
            var qualified_expression_in = context.qualified_expression();
            var subtype_indication_in = context.subtype_indication();

            if (qualified_expression_in != null)
            {
                QualifiedExpression qae = Cast<VhdlElement, QualifiedExpression>(VisitQualified_expression(qualified_expression_in));
                QualifiedExpressionAllocator qaa = new QualifiedExpressionAllocator(qae);
                return qaa;
            }

            if (subtype_indication_in != null)
            {
                VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication_in));
                SubtypeIndicationAllocator sia = new SubtypeIndicationAllocator(si);
                return sia;
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.record_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRecord_nature_definition([NotNull] vhdlParser.Record_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_procedural_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_procedural_statement([NotNull] vhdlParser.Simultaneous_procedural_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.numeric_literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNumeric_literal([NotNull] vhdlParser.Numeric_literalContext context) 
        {
            var al = context.abstract_literal();
            var pl = context.physical_literal();

            if (al != null)
            {
                return VisitAbstract_literal(al);
            }

            if (pl != null)
            {
                return VisitPhysical_literal(pl);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.index_constraint"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIndex_constraint([NotNull] vhdlParser.Index_constraintContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.design_file"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDesign_file([NotNull] vhdlParser.Design_fileContext context)
        {
            VhdlFile res = new VhdlFile(fileName);
            List<LibraryUnit> contextItems = new List<LibraryUnit>();
            libraryScope.Files.Add(res);
            currentScope = res;
            foreach (VHDL_ANTLR4.vhdlParser.Design_unitContext item in context.design_unit())
            {
                LibraryUnit unit = Cast<VhdlElement, LibraryUnit>(VisitDesign_unit(item));
                res.Elements.Add(unit);
            }
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.break_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBreak_statement([NotNull] vhdlParser.Break_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.element_subnature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitElement_subnature_definition([NotNull] vhdlParser.Element_subnature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.exit_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitExit_statement([NotNull] vhdlParser.Exit_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.block_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBlock_statement([NotNull] vhdlParser.Block_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.actual_designator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitActual_designator([NotNull] vhdlParser.Actual_designatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.group_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGroup_declaration([NotNull] vhdlParser.Group_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.opts"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitOpts([NotNull] vhdlParser.OptsContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.secondary_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSecondary_unit([NotNull] vhdlParser.Secondary_unitContext context) 
        {
            var package_body = context.package_body();
            var architecture_body = context.architecture_body();

            if (package_body != null)
            {
                return VisitPackage_body(package_body);
            }

            if (architecture_body != null)
            {
                return VisitArchitecture_body(architecture_body);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.generic_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGeneric_clause([NotNull] vhdlParser.Generic_clauseContext context) 
        {
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simple_simultaneous_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimple_simultaneous_statement([NotNull] vhdlParser.Simple_simultaneous_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_declarative_item([NotNull] vhdlParser.Entity_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_declaration([NotNull] vhdlParser.Interface_declarationContext context) 
        {
            var interface_constant_declaration = context.interface_constant_declaration();
            var interface_variable_declaration = context.interface_variable_declaration();
            var interface_signal_declaration = context.interface_signal_declaration();
            var interface_terminal_declaration = context.interface_terminal_declaration();
            var interface_quantity_declaration = context.interface_quantity_declaration();
            var interface_file_declaration = context.interface_file_declaration();

            if (interface_constant_declaration != null)
            {
                return VisitInterface_constant_declaration(interface_constant_declaration);
            }

            if (interface_variable_declaration != null)
            {
                return VisitInterface_variable_declaration(interface_variable_declaration);
            }

            if (interface_signal_declaration != null)
            {
                return VisitInterface_signal_declaration(interface_signal_declaration);
            }

            if (interface_terminal_declaration != null)
            {
                return VisitInterface_terminal_declaration(interface_terminal_declaration);
            }

            if (interface_quantity_declaration != null)
            {
                return VisitInterface_quantity_declaration(interface_quantity_declaration);
            }

            if (interface_file_declaration != null)
            {
                return VisitInterface_file_declaration(interface_file_declaration);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.label_colon"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLabel_colon([NotNull] vhdlParser.Label_colonContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.alias_indication"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAlias_indication([NotNull] vhdlParser.Alias_indicationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_declaration([NotNull] vhdlParser.Subprogram_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.free_quantity_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFree_quantity_declaration([NotNull] vhdlParser.Free_quantity_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.selected_signal_assignment"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSelected_signal_assignment([NotNull] vhdlParser.Selected_signal_assignmentContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitType_definition([NotNull] vhdlParser.Type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.primary"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPrimary([NotNull] vhdlParser.PrimaryContext context) 
        {
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.simultaneous_if_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSimultaneous_if_statement([NotNull] vhdlParser.Simultaneous_if_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.disconnection_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDisconnection_specification([NotNull] vhdlParser.Disconnection_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.library_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLibrary_clause([NotNull] vhdlParser.Library_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_declarative_part([NotNull] vhdlParser.Architecture_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.condition_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitCondition_clause([NotNull] vhdlParser.Condition_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.selected_waveforms"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSelected_waveforms([NotNull] vhdlParser.Selected_waveformsContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.qualified_expression"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitQualified_expression([NotNull] vhdlParser.Qualified_expressionContext context) 
        {
            var subtype_indication_in = context.subtype_indication();
            var aggregate_in = context.aggregate();
            var expression_in = context.expression();

            VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication_in));
            if (aggregate_in != null)
            {
                Aggregate agg = Cast<VhdlElement, Aggregate>(VisitAggregate(aggregate_in));
                return new QualifiedExpression(si, agg);
            }

            if (expression_in != null)
            {
                Expression exp = Cast<VhdlElement, Expression>(VisitExpression(expression_in));
                return new QualifiedExpression(si, exp);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_signal_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_signal_assignment_statement([NotNull] vhdlParser.Concurrent_signal_assignment_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.terminal_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTerminal_aspect([NotNull] vhdlParser.Terminal_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_declarative_item([NotNull] vhdlParser.Package_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.library_unit"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLibrary_unit([NotNull] vhdlParser.Library_unitContext context) 
        {
            var primary_unit = context.primary_unit();
            var secondary_unit = context.secondary_unit();

            if (primary_unit != null)
            {
                return VisitPrimary_unit(primary_unit);
            }

            if (secondary_unit != null)
            {
                return VisitSecondary_unit(secondary_unit);
            }
            
            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.context_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitContext_clause([NotNull] vhdlParser.Context_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.shift_operator"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitShift_operator([NotNull] vhdlParser.Shift_operatorContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.sequence_of_statements"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSequence_of_statements([NotNull] vhdlParser.Sequence_of_statementsContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_declarative_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_declarative_part([NotNull] vhdlParser.Subprogram_declarative_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subnature_indication"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubnature_indication([NotNull] vhdlParser.Subnature_indicationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.element_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitElement_declaration([NotNull] vhdlParser.Element_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.attribute_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAttribute_specification([NotNull] vhdlParser.Attribute_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.generic_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGeneric_list([NotNull] vhdlParser.Generic_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.concurrent_assertion_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConcurrent_assertion_statement([NotNull] vhdlParser.Concurrent_assertion_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_class"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_class([NotNull] vhdlParser.Entity_classContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.across_aspect"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAcross_aspect([NotNull] vhdlParser.Across_aspectContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.configuration_declarative_item"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConfiguration_declarative_item([NotNull] vhdlParser.Configuration_declarative_itemContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.scalar_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitScalar_nature_definition([NotNull] vhdlParser.Scalar_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_type_definition([NotNull] vhdlParser.File_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.generation_scheme"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGeneration_scheme([NotNull] vhdlParser.Generation_schemeContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.nature_element_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitNature_element_declaration([NotNull] vhdlParser.Nature_element_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.direction"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDirection([NotNull] vhdlParser.DirectionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.wait_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitWait_statement([NotNull] vhdlParser.Wait_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.formal_parameter_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFormal_parameter_list([NotNull] vhdlParser.Formal_parameter_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.loop_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLoop_statement([NotNull] vhdlParser.Loop_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.actual_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitActual_part([NotNull] vhdlParser.Actual_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_statement_part([NotNull] vhdlParser.Entity_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.array_nature_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArray_nature_definition([NotNull] vhdlParser.Array_nature_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.break_selector_clause"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitBreak_selector_clause([NotNull] vhdlParser.Break_selector_clauseContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_declaration([NotNull] vhdlParser.File_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_signal_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_signal_declaration([NotNull] vhdlParser.Interface_signal_declarationContext context) 
        {
            bool hasObjectClass = false;
            bool hasMode = false;
            bool isBus = false;
            //--------------------------------------------------
            var identifier_list = context.identifier_list();
            var signal_mode = context.signal_mode();
            var subtype_indication = context.subtype_indication();
            var expression = context.expression();

            var BUS = context.BUS();

            isBus = BUS != null;
            hasMode = signal_mode != null;
            hasObjectClass = context.SIGNAL() != null;

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);
            Signal.ModeEnum m = ParseSignalMode(signal_mode);

            Expression def = (expression != null)?Cast<VhdlElement, Expression>(VisitExpression(expression)):null;
            VHDL.type.ISubtypeIndication type = (subtype_indication != null)?Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication)):null;

            SignalGroup res = new SignalGroup();

            foreach (var identifier in identifier_list.identifier()) {
                string signal_name = identifier.GetText();
                Signal s = new Signal(signal_name, m, type, def);
                if (isBus) 
                {
                    s.Kind = Signal.KindEnum.BUS;
                }
                Annotations.putAnnotation(s, format);

                res.Elements.Add(s);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            return res; 
        }



        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.access_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAccess_type_definition([NotNull] vhdlParser.Access_type_definitionContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.report_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitReport_statement([NotNull] vhdlParser.Report_statementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_call"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedure_call([NotNull] vhdlParser.Procedure_callContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.file_open_information"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitFile_open_information([NotNull] vhdlParser.File_open_informationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.entity_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEntity_specification([NotNull] vhdlParser.Entity_specificationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.interface_list"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitInterface_list([NotNull] vhdlParser.Interface_listContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.process_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcess_statement_part([NotNull] vhdlParser.Process_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.waveform_element"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitWaveform_element([NotNull] vhdlParser.Waveform_elementContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_statement_part"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_statement_part([NotNull] vhdlParser.Subprogram_statement_partContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.suffix"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSuffix([NotNull] vhdlParser.SuffixContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.type_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitType_declaration([NotNull] vhdlParser.Type_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.attribute_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAttribute_declaration([NotNull] vhdlParser.Attribute_declarationContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.term"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTerm([NotNull] vhdlParser.TermContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.guarded_signal_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitGuarded_signal_specification([NotNull] vhdlParser.Guarded_signal_specificationContext context) 
        {

            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.target"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTarget([NotNull] vhdlParser.TargetContext context) { return VisitChildren(context); }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.literal"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitLiteral([NotNull] vhdlParser.LiteralContext context) 
        {
            var BIT_STRING_LITERAL = context.BIT_STRING_LITERAL();
            var enumeration_literal = context.enumeration_literal();
            var NULL = context.NULL();
            var numeric_literal = context.numeric_literal();
            var STRING_LITERAL = context.STRING_LITERAL();

            if (BIT_STRING_LITERAL != null)
            {
                string text = BIT_STRING_LITERAL.GetText().ToLower();
                switch (text[0])
                {
                    case 'b':
                        {
                            return new BinaryStringLiteral(text);
                        }
                        break;
                    case 'o':
                        {
                            return new OctalStringLiteral(text);
                        }
                        break;
                    case 'x':
                        {
                            return new HexStringLiteral(text);
                        }
                        break;
                    default:
                        {
                            throw new NotSupportedException(String.Format("Could not analyse item {0}", BIT_STRING_LITERAL.ToStringTree()));
                        }
                        break;
                }
            }

            if (enumeration_literal != null)
            {
                return VisitEnumeration_literal(enumeration_literal);
            }

            if (NULL != null)
            {
                return new VHDL.literal.Literals.NullLiteral();
            }

            if (numeric_literal != null)
            {
                return VisitNumeric_literal(numeric_literal);
            }

            if (STRING_LITERAL != null)
            {
                return new StringLiteral(STRING_LITERAL.GetText());
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }
    }
}
