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

        static bool TryCast<In, Out>(In in_data, out Out res)
            where In : class
            where Out : class
        {
            Type in_type = typeof(In);
            Type out_type = typeof(Out);

            res = in_data as Out;

            if (in_data == null)
                return false;

            if (res == null)
                return false;
            return true;
        }

        static List<Out> ParseList<In, Out>(IList<In> data_in, Func<In, VhdlElement> visit_function)
            where In: class
            where Out: class
        {
            List<Out> res = new List<Out>();

            foreach (In i in data_in)
            {
                Out new_item = Cast<VhdlElement, Out>(visit_function(i));
                res.Add(new_item);
            }

            return res;
        }

        static bool TryParseList<In, Out>(IList<In> data_in, Func<In, VhdlElement> visit_function, out List<Out> res)
            where In : class
            where Out : class
        {
            res = new List<Out>();

            foreach (In i in data_in)
            {
                Out new_item;
                bool successfull = TryCast<VhdlElement, Out>(visit_function(i), out new_item);
                if (successfull)
                {
                    res.Add(new_item);
                }
                else
                    return false;
            }

            return true;
        }

        static Out Parse<In, Out>(In data_in, Func<In, VhdlElement> visit_function)
            where In : class
            where Out : class
        {
            VhdlElement parsed_data = visit_function(data_in);
            Out res = Cast<VhdlElement, Out>(parsed_data);
            return res;
        }

        static bool TryParse<In, Out>(In data_in, Func<In, VhdlElement> visit_function, out Out res)
            where In : class
            where Out : class
        {
            VhdlElement parsed_data = visit_function(data_in);
            bool successfull = TryCast<VhdlElement, Out>(parsed_data, out res);
            return successfull;
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
                T res = currentScope.Scope.resolve<T>(identifier);
                if (res == null)
                    throw new Exception(string.Format("Could not find item with name {0} and type {1}", identifier, typeof(T).FullName));
                else
                    return res;
            }

            throw new Exception(string.Format("Could not find item with name {0} and type {1}", identifier, typeof(T).FullName));
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
        public override VhdlElement VisitAssertion_statement([NotNull] vhdlParser.Assertion_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var assertion_in = context.assertion();

            AssertionStatement assertion = Cast<VhdlElement, AssertionStatement>(VisitAssertion(assertion_in));
            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            assertion.Label = label;
            
            return assertion; 
        }

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
        public override VhdlElement VisitConditional_waveforms([NotNull] vhdlParser.Conditional_waveformsContext context) 
        {
            var waveform_in = context.waveform();
            var condition_in = context.condition();

            List<WaveformElement> wes = new List<WaveformElement>();
            foreach (var we_in in waveform_in.waveform_element())
            {
                WaveformElement we = Cast<VhdlElement, WaveformElement>(VisitWaveform_element(we_in));
                wes.Add(we);
            }
            Expression condition = (condition_in != null) ? Cast<VhdlElement, Expression>(VisitCondition(condition_in)) : null;
            VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement cwe = new ConditionalSignalAssignment.ConditionalWaveformElement(wes, condition);

            return cwe; 
        }

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
        public override VhdlElement VisitRecord_type_definition([NotNull] vhdlParser.Record_type_definitionContext context) 
        {
            var element_declarations_in = context.element_declaration();

            VHDL.type.RecordType res = new VHDL.type.RecordType("unknown");

            foreach (var element_declaration in element_declarations_in)
            {
                VHDL.type.RecordType.ElementDeclaration el = Cast<VhdlElement, VHDL.type.RecordType.ElementDeclaration>(VisitElement_declaration(element_declaration));
                res.Elements.Add(el);
            }
            
            return VisitChildren(context); 
        }

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
        public override VhdlElement VisitFunction_call([NotNull] vhdlParser.Function_callContext context) 
        {
            var name = context.name();
            var actual_parameter_part = context.actual_parameter_part();

            IFunction func = resolve<IFunction>(name.GetText());

            FunctionCall call = new FunctionCall(func);

            return call; 
        }

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

        public void ParseArchitectureDeclarativePart([NotNull] Architecture arch, [NotNull] vhdlParser.Architecture_declarative_partContext architecture_declarative_part_in)
        {
            var block_declarative_items_in = architecture_declarative_part_in.block_declarative_item();

            arch.Declarations.Clear();
            foreach (var d in block_declarative_items_in)
            {
                VHDL.declaration.IBlockDeclarativeItem declaration = Cast<VhdlElement, VHDL.declaration.IBlockDeclarativeItem>(VisitBlock_declarative_item(d));
                arch.Declarations.Add(declaration);
            }
        }

        public void ParseArchitectureStatements([NotNull] Architecture arch, [NotNull] vhdlParser.Architecture_statement_partContext architecture_statement_part)
        {
            foreach (var statement in architecture_statement_part.architecture_statement())
            {
                ConcurrentStatement st = Cast<VhdlElement, ConcurrentStatement>(VisitArchitecture_statement(statement));
                arch.Statements.Add(st);
            }
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.architecture_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitArchitecture_body([NotNull] vhdlParser.Architecture_bodyContext context) 
        {
            //--------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //--------------------------------------------------
            var identifiers_in = context.identifier();
            var architecture_declarative_part_in = context.architecture_declarative_part();
            var architecture_statement_part_in = context.architecture_statement_part();

            string architecture_name = identifiers_in[0].GetText();
            string entity_name = identifiers_in[1].GetText();
            
            Entity parentEntity = resolve<Entity>(entity_name);
            if (parentEntity == null)
            {
                throw new ArgumentException(string.Format("Could not find entity {0}", entity_name));
            }
            Architecture res = new Architecture(architecture_name, parentEntity);
            res.Parent = oldScope;
            currentScope = res;

            //------ architecture_declarative_part  ------------
            ParseArchitectureDeclarativePart(res, architecture_declarative_part_in);
            //--------------------------------------------------

            //------ architecture_statement_part_in ------------
            ParseArchitectureStatements(res, architecture_statement_part_in);
            //--------------------------------------------------

            //--------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(res, context);

            if (identifiers_in.Length == 3)
            {
                //end identifier shouls be the same as start identifier
                string architecture_name_end = identifiers_in[2].GetText();
                if (architecture_name_end.Equals(architecture_name, StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    throw new ArgumentException(string.Format("Architecture end identifier mismatch. Architecture name is {0}, end identifier is {1}", architecture_name, architecture_name_end));
                }
            }

            //--------------------------------------------------
            
            return res; 
        }

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
            var constraint_in = context.constraint();
            var tolerance_aspect_in = context.tolerance_aspect();
            var name_in =  context.name()[0];
            string string_name = name_in.GetText();
            var resolution_function_in = (context.name().Length == 2) ? context.name()[1] : null;

            VHDL.type.ISubtypeIndication res = resolve<VHDL.type.ISubtypeIndication>(string_name);
            if (resolution_function_in != null)
            {
                string resolution_function_name = resolution_function_in.GetText();
                res = new VHDL.type.ResolvedSubtypeIndication(resolution_function_name, res);
            }

            if (constraint_in != null)
            {
                var range_constraint = constraint_in.range_constraint();
                if (range_constraint != null)
                {
                    RangeProvider range = Parse<vhdlParser.RangeContext, RangeProvider>(range_constraint.range(), VisitRange);
                    res = new VHDL.type.RangeSubtypeIndication(res, range);
                }

                var index_constraint = constraint_in.index_constraint();
                if (index_constraint != null)
                {
                    List<DiscreteRange> ranges = ParseList<vhdlParser.Discrete_rangeContext, DiscreteRange>(index_constraint.discrete_range(), VisitDiscrete_range);
                    res = new VHDL.type.IndexSubtypeIndication(res, ranges);
                }

                if ((range_constraint == null) && (index_constraint == null))
                {
                    throw new NotSupportedException(String.Format("Could not analyse item {0}", constraint_in.ToStringTree()));
                }
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
        public override VhdlElement VisitProcess_statement([NotNull] vhdlParser.Process_statementContext context) 
        {
            var identifier_in =  context.identifier();
            var label_colon_in = context.label_colon();
            bool is_postponed = context.POSTPONED() != null;
            var process_declarative_part_in = context.process_declarative_part();
            var process_statement_part_in = context.process_statement_part();
            var sensitivity_list_in = context.sensitivity_list();

            ProcessStatement process = new ProcessStatement();

            //----------------------------------------------------------
            //   Before parsing
            //----------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            process.Parent = oldScope;
            currentScope = process;
            //----------------------------------------------------------

            // 1. check label
            if (label_colon_in != null)
            {
                string label = label_colon_in.identifier().GetText();
                process.Label = label;
            }

            //2. check postponed
            process.Postponed = is_postponed;

            //3. check sensitivity list
            if(sensitivity_list_in != null)
            {
                foreach (var item in sensitivity_list_in.name())
                {
                    Signal s = resolve<Signal>(item.GetText());
                    process.SensitivityList.Add(s);
                }
            }

            //4. Add process declarations
            foreach (var declaration in process_declarative_part_in.process_declarative_item())
            {
                IProcessDeclarativeItem pdi = Parse<vhdlParser.Process_declarative_itemContext, IProcessDeclarativeItem>(declaration, VisitProcess_declarative_item);
                process.Declarations.Add(pdi);
            }

            //5. Add process sequential statements
            foreach (var statement in process_statement_part_in.sequential_statement())
            {
                SequentialStatement st = Parse<vhdlParser.Sequential_statementContext, SequentialStatement>(statement, VisitSequential_statement);
                process.Statements.Add(st);
            }

            //----------------------------------------------------------
            //   After parsing
            //----------------------------------------------------------
            currentScope = oldScope;
            //----------------------------------------------------------

            //----------------------------------------------------------
            //   Additioal check
            //----------------------------------------------------------
            CheckProcess(context, process);
            if (identifier_in != null)
            {
                string end_identifier = identifier_in.GetText();
                if (end_identifier.Equals(process.Label, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException(string.Format("Identifier mismatch in process. End identifier is '{0}', process label is '{1}'", end_identifier, process.Label));
                }
            }
            //----------------------------------------------------------

            return process; 
        }

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
            List<Choice> ch = ParseList<vhdlParser.ChoiceContext, Choice>(choices_in, VisitChoice);
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
                res = Parse<vhdlParser.Library_unitContext, LibraryUnit>(library_unit, VisitLibrary_unit);
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
        public override VhdlElement VisitFactor([NotNull] vhdlParser.FactorContext context) 
        {
            var primary1_in = context.primary()[0];
            Expression primary1 = Parse<vhdlParser.PrimaryContext, Expression>(primary1_in, VisitPrimary);
            if (context.DOUBLESTAR() != null)
            {
                var primary2_in = context.primary()[1];
                Expression primary2 = Parse<vhdlParser.PrimaryContext, Expression>(primary2_in, VisitPrimary);

                return new Pow(primary1, primary2);
            }

            if (context.ABS() != null)
            {
                return new Abs(primary1);
            }

            if (context.NOT() != null)
            {
                return new Not(primary1);
            }

            return primary1; 
        }

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
        public override VhdlElement VisitIndex_subtype_definition([NotNull] vhdlParser.Index_subtype_definitionContext context) 
        {
            var name_in = context.name();

            VHDL.type.ISubtypeIndication subtype = resolve<VHDL.type.ISubtypeIndication>(name_in.GetText());

            VHDL.type.RangeSubtypeIndication range_si = new VHDL.type.RangeSubtypeIndication(subtype, null);

            return range_si; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_body"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_body([NotNull] vhdlParser.Subprogram_bodyContext context) 
        {
            var designator_in = context.designator();
            var subprogram_declarative_part_in = context.subprogram_declarative_part();
            var subprogram_kind_in = context.subprogram_kind();
            var subprogram_specification_in = context.subprogram_specification();
            var subprogram_statement_part_in = context.subprogram_statement_part();

            VHDL.declaration.SubprogramDeclaration declaration = Parse<vhdlParser.Subprogram_specificationContext, VHDL.declaration.SubprogramDeclaration>(subprogram_specification_in, VisitSubprogram_specification);
            VHDL.declaration.SubprogramBody body = null;
            if (declaration is VHDL.declaration.FunctionDeclaration)
            {
                body = new VHDL.declaration.FunctionBody(declaration as VHDL.declaration.FunctionDeclaration);                
            }
            else
            {
                body = new VHDL.declaration.ProcedureBody(declaration as VHDL.declaration.ProcedureDeclaration);
            }

            //-------------------------------------------
            //   Before parsing
            //-------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            body.Parent = oldScope;
            currentScope = body;
            //-------------------------------------------

            //Analyse declaration part
            foreach (var declaration_item in subprogram_declarative_part_in.subprogram_declarative_item())
            {
                ISubprogramDeclarativeItem di = Parse<vhdlParser.Subprogram_declarative_itemContext, ISubprogramDeclarativeItem>(declaration_item, VisitSubprogram_declarative_item);
                body.Declarations.Add(di);
            }

            //Analyse sequential statements
            body.Statements.AddRange(ParseList<vhdlParser.Sequential_statementContext, VHDL.statement.SequentialStatement>(subprogram_statement_part_in.sequential_statement(), VisitSequential_statement));
                        
            //-------------------------------------------
            //   After parsing
            //-------------------------------------------
            currentScope = oldScope;
            AddAnnotations(body, context);
            //-------------------------------------------

            //-------------------------------------------
            //     Additional checkers
            //-------------------------------------------

            //1. End identier equals to function/procedure name
            if (designator_in != null)
            {
                string end_name = designator_in.GetText();
                if (end_name.Equals(declaration.Identifier, StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    throw new ArgumentException(string.Format("End name And name in declaration mismatch. End name is '{0}', name in declaration is '{1}'", end_name, declaration.Identifier));
                }                
            }

            // 2. Check that routine type is correct
            if ((subprogram_kind_in != null) && (subprogram_kind_in.FUNCTION() != null))
            {
                if ((declaration is VHDL.declaration.FunctionDeclaration) == false)
                {
                    throw new ArgumentException(string.Format("End program body is for function, but declaration for procedure"));
                }
            }

            if ((subprogram_kind_in != null) && (subprogram_kind_in.PROCEDURE() != null))
            {
                if ((declaration is VHDL.declaration.ProcedureDeclaration) == false)
                {
                    throw new ArgumentException(string.Format("End program body is for procedure, but declaration for function"));
                }
            }
            //-------------------------------------------
            return body;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.delay_mechanism"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitDelay_mechanism([NotNull] vhdlParser.Delay_mechanismContext context) 
        {
            if (context.TRANSPORT() != null)
            {
                return DelayMechanism.TRANSPORT;
            }
            else
            {
                if (context.INERTIAL() != null)
                {
                    if (context.REJECT() == null)
                    {
                        return DelayMechanism.INERTIAL;
                    }
                    else
                    {
                        var expression_in = context.expression();
                        Expression exp = Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);
                        return DelayMechanism.REJECT_INERTIAL(exp);
                    }
                }
                else
                {
                    return DelayMechanism.DUTY_CYCLE;
                }
            }
        }

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
        public override VhdlElement VisitPackage_body([NotNull] vhdlParser.Package_bodyContext context) 
        {
            var identifiers_in = context.identifier();
            var identifier_begin_in = ((identifiers_in != null) && (identifiers_in.Length != 0)) ? identifiers_in[0] : null;
            var identifier_end_in = ((identifiers_in != null) && (identifiers_in.Length == 2)) ? identifiers_in[1] : null;

            var package_body_declarative_part_in = context.package_body_declarative_part();

            string identifier_begin = (identifier_begin_in != null) ? identifier_begin_in.GetText() : string.Empty;
            string identifier_end = (identifier_end_in != null) ? identifier_end_in.GetText() : string.Empty;

            PackageDeclaration declaration = resolve<PackageDeclaration>(identifier_begin);

            PackageBody pb = new PackageBody(declaration); 

            //--------------------------------------------------------------------------
            //         Before Parsing
            //--------------------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //--------------------------------------------------------------------------

            pb.Parent = oldScope;
            currentScope = pb;

            foreach (var declaration_in in package_body_declarative_part_in.package_body_declarative_item())
            {
                IPackageBodyDeclarativeItem item = Parse<vhdlParser.Package_body_declarative_itemContext, IPackageBodyDeclarativeItem>(declaration_in, VisitPackage_body_declarative_item);
                pb.Declarations.Add(item);
            }

            if ((string.IsNullOrEmpty(identifier_end) == false) && (identifier_end.Equals(identifier_begin, StringComparison.InvariantCultureIgnoreCase) == false))
            {
                throw new ArgumentException(string.Format("Package begin & end name mismatch. Identifier is '{0}', end name is '{1}'", identifier_begin, identifier_end));
            }

            //--------------------------------------------------------------------------
            //         After Parsing
            //--------------------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(pb, context);
            //--------------------------------------------------------------------------
            return pb; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.range_constraint"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitRange_constraint([NotNull] vhdlParser.Range_constraintContext context) 
        {
            var range_in = context.range();

            VHDL.Range range = Parse<vhdlParser.RangeContext, Range>(range_in, VisitRange);

            VHDL.type.ISubtypeIndication range_from_type = range.From.Type;
            VHDL.type.ISubtypeIndication range_to_type = range.To.Type;

            if ((range_from_type == VHDL.builtin.Standard.INTEGER) && (range_to_type == VHDL.builtin.Standard.INTEGER))
            {
                VHDL.type.IntegerType res = new VHDL.type.IntegerType("unknown", range);
                return res;
            }
            else
            {
                VHDL.type.RealType res = new VHDL.type.RealType("unknown", range);
                return res;
            }

            return VisitChildren(context); 
        }

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
        public override VhdlElement VisitProcedure_call_statement([NotNull] vhdlParser.Procedure_call_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var procedure_call_in = context.procedure_call();

            ProcedureCall procedureCall = Parse<vhdlParser.Procedure_callContext, ProcedureCall>(procedure_call_in, VisitProcedure_call);

            string label = (label_colon_in != null) ? (label_colon_in.identifier().GetText()) : string.Empty;
            procedureCall.Label = label;

            return procedureCall; 
        }

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
            var relations_in = context.relation();
            var logical_operators_in = context.logical_operator();

            List<Expression> parsed_relations = ParseList<vhdlParser.RelationContext, Expression>(relations_in, VisitRelation);

            if (parsed_relations.Count == 0)
            {
                throw new NotSupportedException(String.Format("Could not analyse item {0}. Amount of relations is 0", context.ToStringTree()));
            }
            
            Expression res = parsed_relations[0];;

            for (int i = 0; i < logical_operators_in.Length; i++)
            {
                Expression next_expression = parsed_relations[i+1];
                vhdlParser.Logical_operatorContext op = logical_operators_in[i];

                if (op.AND() != null)
                {
                    Expression new_res = new VHDL.expression.And(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.NAND() != null)
                {
                    Expression new_res = new VHDL.expression.Nand(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.NOR() != null)
                {
                    Expression new_res = new VHDL.expression.Nor(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.OR() != null)
                {
                    Expression new_res = new VHDL.expression.Or(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.XNOR() != null)
                {
                    Expression new_res = new VHDL.expression.Xnor(res, next_expression);
                    res = new_res;
                    continue;
                }

                if (op.XOR() != null)
                {
                    Expression new_res = new VHDL.expression.Xor(res, next_expression);
                    res = new_res;
                    continue;
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}.", op.ToStringTree()));
            }

            return res;
            //throw new NotSupportedException(String.Format("Could not analyse item {0}.", context.ToStringTree()));
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
            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            var def_value_in = context.expression();
            Expression def_value = Parse<vhdlParser.ExpressionContext, Expression>(def_value_in, VisitExpression);

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
        public override VhdlElement VisitNext_statement([NotNull] vhdlParser.Next_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var identifier_in = context.identifier();
            var condition_in = context.condition();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string identifier = identifier_in.GetText();
            Expression condition = (condition_in != null)? Parse<vhdlParser.ConditionContext, Expression>(condition_in, VisitCondition) : null;

            LoopStatement loop = resolve<LoopStatement>(identifier);

            NextStatement next = new NextStatement(loop, condition);
            next.Label = label;

            return next; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.scalar_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitScalar_type_definition([NotNull] vhdlParser.Scalar_type_definitionContext context) 
        {
            var enumeration_type_definition_in = context.enumeration_type_definition();
            var physical_type_definition_in = context.physical_type_definition();
            var range_constraint_in = context.range_constraint();

            if (enumeration_type_definition_in != null)
                return VisitEnumeration_type_definition(enumeration_type_definition_in);

            if (physical_type_definition_in != null)
                return VisitPhysical_type_definition(physical_type_definition_in);

            if (range_constraint_in != null)
                return VisitRange_constraint(range_constraint_in);

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

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
            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
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
        public override VhdlElement VisitCase_statement([NotNull] vhdlParser.Case_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();
            var case_statement_alternative_in = context.case_statement_alternative();
            var identifier_in = context.identifier();

            string label_begin = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            //1. parse expression
            Expression expression = Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression);

            CaseStatement case_statement = new CaseStatement(expression);

            //2. parse alternatives
            case_statement.Alternatives.AddRange(ParseList<vhdlParser.Case_statement_alternativeContext, CaseStatement.Alternative>(case_statement_alternative_in, VisitCase_statement_alternative));
            
            //3. Check that end identifier is the same as label at the beginning
            case_statement.Label = label_begin;

            if ((string.IsNullOrEmpty(label_end) == false) && (label_begin.Equals(label_end, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException(string.Format("If statement begin & ennd identifier mismatch. Label at the begin is '{0}', albel at the end is '{1}'", label_begin, label_end));
            }

            return case_statement; 
        }

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
        public override VhdlElement VisitRelation([NotNull] vhdlParser.RelationContext context) 
        {
            var shift_expressions_in = context.shift_expression();
            var relational_operator_in = context.relational_operator();

            Expression parsed_shift_expression_1 = Parse<vhdlParser.Shift_expressionContext, Expression>(shift_expressions_in[0], VisitShift_expression);

            if (relational_operator_in == null)
            {
                return parsed_shift_expression_1;
            }
            else
            {
                Expression parsed_shift_expression_2 = Parse<vhdlParser.Shift_expressionContext, Expression>(shift_expressions_in[1], VisitShift_expression);

                if (relational_operator_in.EQ() != null)
                {
                    return new VHDL.expression.Equals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.GE() != null)
                {
                    return new VHDL.expression.GreaterEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.GREATERTHAN() != null)
                {
                    return new VHDL.expression.GreaterThan(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.LE() != null)
                {
                    return new VHDL.expression.LessEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.LOWERTHAN() != null)
                {
                    return new VHDL.expression.LessThan(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                if (relational_operator_in.NEQ() != null)
                {
                    return new VHDL.expression.NotEquals(parsed_shift_expression_1, parsed_shift_expression_2);
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}", relational_operator_in.ToStringTree()));
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

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

        private List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> FormConditionalWaveformList([NotNull] vhdlParser.Conditional_waveformsContext conditional_waveforms_in)
        {
            List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> res = new List<ConditionalSignalAssignment.ConditionalWaveformElement>();

            vhdlParser.Conditional_waveformsContext current_conditional_waveforms = conditional_waveforms_in;
            while (current_conditional_waveforms != null)
            {
                var conditional_waveforms_new_in = current_conditional_waveforms.conditional_waveforms();               

                VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement CWE = Cast<VhdlElement, VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement>(VisitConditional_waveforms(current_conditional_waveforms));

                res.Add(CWE);

                current_conditional_waveforms = conditional_waveforms_new_in;
            }
            return res;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.conditional_signal_assignment"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitConditional_signal_assignment([NotNull] vhdlParser.Conditional_signal_assignmentContext context) 
        {
            var opts_in = context.opts();
            var target_in = context.target();
            var conditional_waveforms_in = context.conditional_waveforms();

            ISignalAssignmentTarget target = Cast<VhdlElement, ISignalAssignmentTarget>(VisitTarget(target_in));
            bool is_guarded = opts_in.GUARDED() != null;
            DelayMechanism delay_mechanism = (opts_in.delay_mechanism() != null) ? Cast<VhdlElement, DelayMechanism>(VisitDelay_mechanism(opts_in.delay_mechanism())) : DelayMechanism.DUTY_CYCLE;

            List<VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement> CWEs = FormConditionalWaveformList(conditional_waveforms_in);


            ConditionalSignalAssignment csa = new ConditionalSignalAssignment(target, CWEs);
            csa.DelayMechanism = delay_mechanism;
            csa.Guarded = is_guarded;

            return csa; 
        }

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
        public override VhdlElement VisitArchitecture_statement([NotNull] vhdlParser.Architecture_statementContext context) 
        {
            
            return VisitChildren(context); 
        }

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
        public override VhdlElement VisitIdentifier([NotNull] vhdlParser.IdentifierContext context) 
        {
            VhdlElement ve = resolve<VhdlElement>(context.GetText());
            return ve;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.composite_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitComposite_type_definition([NotNull] vhdlParser.Composite_type_definitionContext context) 
        {
            var array_type_definition_in = context.array_type_definition();
            var record_type_definition_in = context.record_type_definition();

            if (array_type_definition_in != null)
                return VisitArray_type_definition(array_type_definition_in);

            if (record_type_definition_in != null)
                return VisitRecord_type_definition(record_type_definition_in);

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

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
            var name = context.identifier();
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
                /*
                EnumerationLiteral literal = resolve<EnumerationLiteral>(identifier.GetText());
                if (literal != null)
                    return literal;
                else
                */
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
            Expression def_value = Parse<vhdlParser.ExpressionContext, Expression>(def_value_in, VisitExpression);

            var subtype_indication_in = context.subtype_indication();
            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

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
        public override VhdlElement VisitName([NotNull] vhdlParser.NameContext context) 
        {
            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.package_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitPackage_declaration([NotNull] vhdlParser.Package_declarationContext context) 
        {
            var identifiers_in = context.identifier();
            var identifier_begin_in = ((identifiers_in != null) && (identifiers_in.Length != 0))?identifiers_in[0]:null;
            var identifier_end_in = ((identifiers_in != null) && (identifiers_in.Length == 2))?identifiers_in[1]:null;

            var package_declarative_part_in = context.package_declarative_part();

            string identifier_begin = (identifier_begin_in != null)? identifier_begin_in.GetText() : string.Empty;
            string identifier_end = (identifier_end_in != null)? identifier_end_in.GetText() : string.Empty;

            PackageDeclaration pd = new PackageDeclaration(identifier_begin);

            //--------------------------------------------------------------------------
            //         Before Parsing
            //--------------------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope; 
            //--------------------------------------------------------------------------

            pd.Parent = oldScope;
            currentScope = pd;

            foreach(var declaration_in in package_declarative_part_in.package_declarative_item())
            {
                IPackageDeclarativeItem item = Parse<vhdlParser.Package_declarative_itemContext, IPackageDeclarativeItem>(declaration_in, VisitPackage_declarative_item);
                pd.Declarations.Add(item);
            }

            if ((string.IsNullOrEmpty(identifier_end) == false) && (identifier_end.Equals(identifier_begin, StringComparison.InvariantCultureIgnoreCase) == false))
            {
                throw new ArgumentException(string.Format("Package begin & end name mismatch. Identifier is '{0}', end name is '{1}'", identifier_begin, identifier_end));
            }

            //--------------------------------------------------------------------------
            //         After Parsing
            //--------------------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(pd, context);
            //--------------------------------------------------------------------------
            return pd; 
        }

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
        public override VhdlElement VisitUnconstrained_array_definition([NotNull] vhdlParser.Unconstrained_array_definitionContext context) 
        {
            var index_subtype_definitions = context.index_subtype_definition();
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            VHDL.type.UnconstrainedArray res = new VHDL.type.UnconstrainedArray("unknown", si);

            List<VHDL.type.ISubtypeIndication> ranges = new List<VHDL.type.ISubtypeIndication>();
            foreach (var index_subtype_definition in index_subtype_definitions)
            {
                VHDL.type.ISubtypeIndication range = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitIndex_subtype_definition(index_subtype_definition));
                ranges.Add(range);
            }

            res.IndexSubtypes.AddRange(ranges);
            
            return res;
        }

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
        public override VhdlElement VisitAssertion([NotNull] vhdlParser.AssertionContext context) 
        {
            var condition_in = context.condition();
            var has_report = context.REPORT() != null;
            var has_severity = context.SEVERITY() != null;
            var expression_in = context.expression();
            int expression_idx_for_parse = 0;

            //1. parse condition
            Expression condition = Cast<VhdlElement, Expression>(VisitCondition(condition_in));

            //2. Parse report epression
            Expression report_expression = (has_report)? Cast<VhdlElement, Expression>(VisitExpression(expression_in[expression_idx_for_parse])) : null;
            if (has_report) expression_idx_for_parse++;

            //3. Parse severity
            Expression severity_expression = (has_severity) ? Cast<VhdlElement, Expression>(VisitExpression(expression_in[expression_idx_for_parse])) : null;
            if (has_severity) expression_idx_for_parse++;

            AssertionStatement assertionStatement = new AssertionStatement(condition, report_expression, severity_expression);

            return assertionStatement; 
        }

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
        public override VhdlElement VisitArray_type_definition([NotNull] vhdlParser.Array_type_definitionContext context) 
        {
            var constrained_array_definition_in = context.constrained_array_definition();
            var unconstrained_array_definition_in = context.unconstrained_array_definition();

            if (constrained_array_definition_in != null)
                return VisitConstrained_array_definition(constrained_array_definition_in);

            if (unconstrained_array_definition_in != null)
                return VisitUnconstrained_array_definition(unconstrained_array_definition_in);

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

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

            foreach(var declaration in context.entity_declarative_part().entity_declarative_item())
            {
                IEntityDeclarativeItem decl = Cast<VhdlElement, IEntityDeclarativeItem>(VisitEntity_declarative_item(declaration));
                res.Declarations.Add(decl);
            }

            if (entity_statement_part != null)
            {
                foreach (var statement in entity_statement_part.entity_statement())
                {
                    EntityStatement st = Cast<VhdlElement, EntityStatement>(VisitEntity_statement(statement));
                    res.Statements.Add(st);
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
                Expression exp = Parse<vhdlParser.ExpressionContext, Expression>(expression_in, VisitExpression); 

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
        public override VhdlElement VisitBlock_declarative_item([NotNull] vhdlParser.Block_declarative_itemContext context) 
        {
            var alias_declaration_in = context.alias_declaration();
            var attribute_declaration_in = context.attribute_declaration();
            var attribute_specification_in = context.attribute_specification();
            var component_declaration_in = context.component_declaration();
            var configuration_specification_in = context.configuration_specification();
            var constant_declaration_in = context.constant_declaration();
            var disconnection_specification_in = context.disconnection_specification();
            var file_declaration_in = context.file_declaration();
            var group_declaration_in = context.group_declaration();
            var group_template_declaration_in = context.group_template_declaration();
            var nature_declaration_in = context.nature_declaration();
            var quantity_declaration_in = context.quantity_declaration();
            var signal_declaration_in = context.signal_declaration();
            var step_limit_specification_in = context.step_limit_specification();
            var subnature_declaration_in = context.subnature_declaration();
            var subprogram_body_in = context.subprogram_body();
            var subprogram_declaration_in = context.subprogram_declaration();
            var subtype_declaration_in = context.subtype_declaration();
            var terminal_declaration_in = context.terminal_declaration();
            var type_declaration_in = context.type_declaration();
            var use_clause_in = context.use_clause();
            var variable_declaration_in = context.variable_declaration();

            if (alias_declaration_in != null) return VisitAlias_declaration(alias_declaration_in);
            if (attribute_declaration_in != null) return VisitAttribute_declaration(attribute_declaration_in);
            if (attribute_specification_in != null) return VisitAttribute_specification(attribute_specification_in);
            if (component_declaration_in != null) return VisitComponent_declaration(component_declaration_in);
            if (configuration_specification_in != null) return VisitConfiguration_specification(configuration_specification_in);
            if (constant_declaration_in != null) return VisitConstant_declaration(constant_declaration_in);
            if (disconnection_specification_in != null) return VisitDisconnection_specification(disconnection_specification_in);
            if (file_declaration_in != null) return VisitFile_declaration(file_declaration_in);
            if (group_declaration_in != null) return VisitGroup_declaration(group_declaration_in);
            if (group_template_declaration_in != null) return VisitGroup_template_declaration(group_template_declaration_in);
            if (nature_declaration_in != null) return VisitNature_declaration(nature_declaration_in);
            if (quantity_declaration_in != null) return VisitQuantity_declaration(quantity_declaration_in);
            if (signal_declaration_in != null) return VisitSignal_declaration(signal_declaration_in);
            if (step_limit_specification_in != null) return VisitStep_limit_specification(step_limit_specification_in);
            if (subnature_declaration_in != null) return VisitSubnature_declaration(subnature_declaration_in);
            if (subprogram_body_in != null) return VisitSubprogram_body(subprogram_body_in);
            if (subprogram_declaration_in != null) return VisitSubprogram_declaration(subprogram_declaration_in);
            if (subtype_declaration_in != null) return VisitSubtype_declaration(subtype_declaration_in);
            if (terminal_declaration_in != null) return VisitTerminal_declaration(terminal_declaration_in);
            if (type_declaration_in != null) return VisitType_declaration(type_declaration_in);
            if (use_clause_in != null) return VisitUse_clause(use_clause_in);
            if (variable_declaration_in != null) return VisitVariable_declaration(variable_declaration_in);
 
            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.signal_assignment_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSignal_assignment_statement([NotNull] vhdlParser.Signal_assignment_statementContext context) 
        {
            var delay_mechanism_in = context.delay_mechanism();
            var label_colon_in = context.label_colon();
            var target_in = context.target();
            var waveform_in = context.waveform();

            ISignalAssignmentTarget target = Cast<VhdlElement, ISignalAssignmentTarget>(VisitTarget(target_in));
            
            List<WaveformElement> waveformelements = new List<WaveformElement>();
            foreach(var w in waveform_in.waveform_element())
            {
                WaveformElement el = Cast<VhdlElement, WaveformElement>(VisitWaveform_element(w));
                waveformelements.Add(el);
            }
            
            
            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            DelayMechanism delay = (delay_mechanism_in != null) ? Cast<VhdlElement, DelayMechanism>(VisitDelay_mechanism(delay_mechanism_in)) : DelayMechanism.DUTY_CYCLE;

            SignalAssignment sa = new SignalAssignment(target, waveformelements);
            sa.Label = label;
            sa.DelayMechanism = delay;

            return sa; 
        }

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
        /// </summar0y>
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
        public override VhdlElement VisitShift_expression([NotNull] vhdlParser.Shift_expressionContext context) 
        {
            var shift_operator_in = context.shift_operator();
            var simple_expressions_in = context.simple_expression();

            Expression parsed_simple_expression_1 = Cast<VhdlElement, Expression>(VisitSimple_expression(simple_expressions_in[0]));

            if (shift_operator_in == null)
            {
                return parsed_simple_expression_1;
            }
            else
            {
                Expression parsed_simple_expression_2 = Cast<VhdlElement, Expression>(VisitSimple_expression(simple_expressions_in[1]));

                if (shift_operator_in.ROL() != null)
                {
                    return new VHDL.expression.Rol(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.ROR() != null)
                {
                    return new VHDL.expression.Ror(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SLA() != null)
                {
                    return new VHDL.expression.Sla(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SLL() != null)
                {
                    return new VHDL.expression.Sll(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SRA() != null)
                {
                    return new VHDL.expression.Sra(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                if (shift_operator_in.SRL() != null)
                {
                    return new VHDL.expression.Srl(parsed_simple_expression_1, parsed_simple_expression_2);
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}", shift_operator_in.ToStringTree()));
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.iteration_scheme"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIteration_scheme([NotNull] vhdlParser.Iteration_schemeContext context) 
        {
            bool is_while_iteration_scheme = context.WHILE() != null;
            bool is_for_iteration_scheme = context.FOR() != null;

            if (is_while_iteration_scheme)
            {
                var condition_in = context.condition();
                Expression condition = Cast<VhdlElement, Expression>(VisitCondition(condition_in));
                WhileStatement whileStatement = new WhileStatement(condition);
                return whileStatement;
            }

            if (is_for_iteration_scheme)
            {
                var identifier_in = context.parameter_specification().identifier();
                var discrete_range_in = context.parameter_specification().discrete_range();

                string identifier = identifier_in.GetText();
                DiscreteRange range = Cast<VhdlElement, DiscreteRange>(VisitDiscrete_range(discrete_range_in));

                ForStatement forStatement = new ForStatement(identifier, range);
                return forStatement;
            }

            return new LoopStatement();
        }

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
            var subtype_indication_in = context.subtype_indication();

            if (range != null)
            {
                return VisitRange(range);
            }

            if (subtype_indication_in != null)
            {
                VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
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
        public override VhdlElement VisitSubtype_declaration([NotNull] vhdlParser.Subtype_declarationContext context) 
        {
            var identifier_in = context.identifier();
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
            string identifier = identifier_in.GetText();

            VHDL.declaration.Subtype subtype = new VHDL.declaration.Subtype(identifier, si);

            return subtype; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.subprogram_specification"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitSubprogram_specification([NotNull] vhdlParser.Subprogram_specificationContext context) 
        {
            var function_specification_in = context.function_specification();
            var procedure_specification_in = context.procedure_specification();

            if (function_specification_in != null)
                return VisitFunction_specification(function_specification_in);

            if (procedure_specification_in != null)
                return VisitProcedure_specification(procedure_specification_in);

            return VisitChildren(context); 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.function_specification"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override VhdlElement VisitFunction_specification([NotNull] vhdlParser.Function_specificationContext context) 
        {
            bool is_impure = context.IMPURE() != null;
            bool is_pure = context.PURE() != null;

            if ((is_impure == true) && (is_pure == true))
            {
                throw new NotSupportedException(String.Format("Could not analyse item {0}. IMPURE AND PURE tokens are set in one statement.", context.ToStringTree()));
            }

            var designator_in = context.designator();
            var formal_parameter_list_in = context.formal_parameter_list();
            var return_type_in = context.subtype_indication();

            string name = designator_in.GetText();

            List<IVhdlObjectProvider> parameters = new List<IVhdlObjectProvider>();
            if ((formal_parameter_list_in != null) && (formal_parameter_list_in.interface_list() != null))
            {
                foreach (var p in formal_parameter_list_in.interface_list().interface_element())
                {
                    IVhdlObjectProvider o = Cast<VhdlElement, IVhdlObjectProvider>(VisitInterface_element(p));
                    parameters.Add(o);
                }
            }

            VHDL.type.ISubtypeIndication returnType = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(return_type_in));

            VHDL.declaration.FunctionDeclaration fd = new FunctionDeclaration(name, returnType, parameters);
            fd.Impure = is_impure;
            return fd;
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_specification"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override VhdlElement VisitProcedure_specification([NotNull] vhdlParser.Procedure_specificationContext context) 
        {
            var designator_in = context.designator();
            var formal_parameter_list_in = context.formal_parameter_list();

            string name = designator_in.GetText();

            List<IVhdlObjectProvider> parameters = new List<IVhdlObjectProvider>();
            if ((formal_parameter_list_in != null) && (formal_parameter_list_in.interface_list() != null))
            {
                foreach (var p in formal_parameter_list_in.interface_list().interface_element())
                {
                    IVhdlObjectProvider o = Cast<VhdlElement, IVhdlObjectProvider>(VisitInterface_element(p));
                    parameters.Add(o);
                }
            }
            
            VHDL.declaration.ProcedureDeclaration pd = new ProcedureDeclaration(name, parameters);
            return pd; 
        }

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
        public override VhdlElement VisitVariable_assignment_statement([NotNull] vhdlParser.Variable_assignment_statementContext context) 
        {
            var target_in = context.target();
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            Expression expresion = Cast<VhdlElement, Expression>(VisitExpression(expression_in));
            IVariableAssignmentTarget target = Cast<VhdlElement, IVariableAssignmentTarget>(VisitTarget(target_in));

            VariableAssignment va = new VariableAssignment(target, expresion);
            va.Label = label;

            return va; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.if_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitIf_statement([NotNull] vhdlParser.If_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var conditions_in = context.condition();
            var sequence_of_statements_in = context.sequence_of_statements();
            var identifier_in = context.identifier();

            int condition_counter = 0;
            int sequence_of_statements_counter = 0;

            Expression condition = Cast<VhdlElement, Expression>(VisitCondition(conditions_in[condition_counter]));
            condition_counter++;
            string label_begin = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            VHDL.statement.IfStatement if_statement = new VHDL.statement.IfStatement(condition);

            //1. parse statements in if block
            if_statement.Statements.AddRange(ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement));
            sequence_of_statements_counter++;

            //2. parse elseif statements
            int end_index_of_elseif_statements = (context.ELSE() != null) ? (sequence_of_statements_in.Length - 2) : (sequence_of_statements_in.Length - 1);
            while (sequence_of_statements_counter <= end_index_of_elseif_statements)
            {
                Expression elseif_condition = Cast<VhdlElement, Expression>(VisitCondition(conditions_in[condition_counter]));
                condition_counter++;
                List<SequentialStatement> elseif_statements = ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement);
                sequence_of_statements_counter++;
                VHDL.statement.IfStatement.ElsifPart elseif = new VHDL.statement.IfStatement.ElsifPart(elseif_condition, elseif_statements);
                if_statement.ElsifParts.Add(elseif);
            }

            //3. parse else statements
            if (context.ELSE() != null)
            {
                List<SequentialStatement> else_statements = ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in[sequence_of_statements_counter].sequential_statement(), VisitSequential_statement);
                if_statement.ElseStatements.AddRange(else_statements);
            }

            //4. Check that end identifier is the same as label at the beginning
            if_statement.Label = label_begin;

            if ((string.IsNullOrEmpty(label_end) == false) && (label_begin.Equals(label_end, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException(string.Format("If statement begin & ennd identifier mismatch. Label at the begin is '{0}', albel at the end is '{1}'", label_begin, label_end));
            }

            return if_statement; 
        }

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
        public override VhdlElement VisitPhysical_type_definition([NotNull] vhdlParser.Physical_type_definitionContext context) 
        {
            var base_unit_declaration_in = context.base_unit_declaration();
            var range_constraint_in = context.range_constraint();
            var secondary_unit_declarations_in = context.secondary_unit_declaration();

            string base_unit_name = base_unit_declaration_in.GetText();

            VHDL.Range range = Cast<VhdlElement, VHDL.Range>(VisitRange_constraint(range_constraint_in));

            VHDL.type.PhysicalType pt = new VHDL.type.PhysicalType("unknown", range, base_unit_name);
            pt.createUnit(base_unit_name);

            foreach (var unit in secondary_unit_declarations_in)
            {
                string identifier = unit.identifier().GetText();
                var physical_literal_in = unit.physical_literal();

                var abstract_literal_in = physical_literal_in.abstract_literal();
                var base_identifier_in = physical_literal_in.identifier();

                string base_identifier = base_identifier_in.GetText();

                AbstractLiteral al = Cast<VhdlElement, AbstractLiteral>(VisitAbstract_literal(abstract_literal_in));

                pt.createUnit(identifier, al, base_identifier);
            }
            
            return pt; 
        }

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
        public override VhdlElement VisitVariable_declaration([NotNull] vhdlParser.Variable_declarationContext context) 
        {
            var identifier_list = context.identifier_list();
            var subtype_indication = context.subtype_indication();
            var expression = context.expression();
            bool is_shared = context.SHARED() != null;

            Expression def = (expression != null) ? Cast<VhdlElement, Expression>(VisitExpression(expression)) : null;
            VHDL.type.ISubtypeIndication type = (subtype_indication != null) ? Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitSubtype_indication(subtype_indication)) : null;

            VariableDeclaration res = new VariableDeclaration();

            foreach (var identifier in identifier_list.identifier())
            {
                string variable_name = identifier.GetText();
                Variable v = new Variable(variable_name, type, def);

                v.Shared = is_shared;

                res.Objects.Add(v);
            }

            //--------------------------------------------------
            AddAnnotations(res, context);
            res.Parent = currentScope;
            return res; 
        }

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
            var plus_in = context.PLUS();
            var minus_in = context.MINUS();

            var terms_in = context.term();
            var adding_operators_in = context.adding_operator();

            List<Expression> parsed_terms = new List<Expression>();
            foreach (var t in terms_in)
            {
                Expression term = Cast<VhdlElement, Expression>(VisitTerm(t));
                parsed_terms.Add(term);
            }

            if (parsed_terms.Count == 0)
            {
                throw new NotSupportedException(String.Format("Could not analyse item {0}. Amount of terms is 0.", context.ToStringTree()));
            }

            Expression res = parsed_terms[0];

            if (plus_in != null)
            {
                Expression new_exp = new VHDL.expression.Plus(res);
                res = new_exp;
            }

            if (minus_in != null)
            {
                Expression new_exp = new VHDL.expression.Minus(res);
                res = new_exp;
            }

            for (int i = 0; i < adding_operators_in.Length; i++)
            {
                var op = adding_operators_in[i];
                Expression curr_expression = parsed_terms[i + 1];

                if (op.MINUS() != null)
                {
                    Expression new_res = new VHDL.expression.Subtract(res, curr_expression);
                    res = new_res;
                    continue;
                }

                if (op.PLUS() != null)
                {
                    Expression new_res = new VHDL.expression.Add(res, curr_expression);
                    res = new_res;
                    continue;
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}", op.ToStringTree()));
            }

            return res;
            //throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
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
        public override VhdlElement VisitReturn_statement([NotNull] vhdlParser.Return_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var expression_in = context.expression();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            Expression expression = (expression_in != null) ? Cast<VhdlElement, Expression>(VisitExpression(expression_in)) : null;

            ReturnStatement returnStatement = new ReturnStatement(expression);
            returnStatement.Label = label;

            return returnStatement; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.enumeration_type_definition"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitEnumeration_type_definition([NotNull] vhdlParser.Enumeration_type_definitionContext context) 
        {
            var enumeration_literals_in = context.enumeration_literal();

            List<char> character_literals = new List<char>();
            List<string> string_literals = new List<string>();

            foreach (var l in enumeration_literals_in)
            {
                if (l.CHARACTER_LITERAL() != null)
                {
                    character_literals.Add(l.CHARACTER_LITERAL().GetText()[1]);
                    continue;
                }

                if (l.identifier() != null)
                {
                    string_literals.Add(l.identifier().GetText());
                    continue;
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}", l.ToStringTree()));
            }

            if ((character_literals.Count != 0) && (string_literals.Count != 0))
            {
                throw new NotSupportedException(String.Format("Could not analyse item {0}. Amount of string literals is {1}, Amount of character literals is {2}", context.ToStringTree(), string_literals.Count, character_literals.Count));
            }

            if(character_literals.Count != 0)
            {
                VHDL.type.EnumerationType enumeration_type = new VHDL.type.EnumerationType("unknown", character_literals.ToArray());
                return enumeration_type;
            }

            if (string_literals.Count != 0)
            {
                VHDL.type.EnumerationType enumeration_type = new VHDL.type.EnumerationType("unknown", string_literals.ToArray());
                return enumeration_type;
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}. Amount of literals is 0.", context.ToStringTree()));
        }

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
        public override VhdlElement VisitConstrained_array_definition([NotNull] vhdlParser.Constrained_array_definitionContext context) 
        {
            var index_constraints_in = context.index_constraint();
            var subtype_indication_in = context.subtype_indication();

            List<VHDL.DiscreteRange> ranges = new List<DiscreteRange>();
            foreach (var constraint in index_constraints_in.discrete_range())
            {
                VHDL.DiscreteRange range = Cast<VhdlElement, VHDL.DiscreteRange>(VisitDiscrete_range(constraint));
                ranges.Add(range);
            }

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.ConstrainedArray res = new VHDL.type.ConstrainedArray("unknown", si, ranges);
            return res;
        }

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
                VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
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
        public override VhdlElement VisitBreak_statement([NotNull] vhdlParser.Break_statementContext context) 
        {
            return VisitChildren(context); 
        }

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
        public override VhdlElement VisitExit_statement([NotNull] vhdlParser.Exit_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var identifier_in = context.identifier();
            var condition_in = context.condition();

            string label = (label_colon_in != null) ? label_colon_in.identifier().GetText() : string.Empty;
            string identifier = identifier_in.GetText();
            Expression condition = (condition_in != null) ? Cast<VhdlElement, Expression>(VisitCondition(condition_in)) : null;

            LoopStatement loop = resolve<LoopStatement>(identifier);

            ExitStatement exit = new ExitStatement(loop, condition);
            exit.Label = label;

            return exit; 
        }

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
        public override VhdlElement VisitSubprogram_declaration([NotNull] vhdlParser.Subprogram_declarationContext context) 
        {
            var subprogram_specification_in = context.subprogram_specification();

            SubprogramDeclaration res = Cast<VhdlElement, SubprogramDeclaration>(VisitSubprogram_specification(subprogram_specification_in));
            AddAnnotations(res, context);
            
            return res; 
        }

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
        public override VhdlElement VisitSelected_signal_assignment([NotNull] vhdlParser.Selected_signal_assignmentContext context)
        {
            var expression_in = context.expression();
            var target_in = context.target();
            var opts_in = context.opts();
            var selected_waveforms_in = context.selected_waveforms();

            DelayMechanism delay = (opts_in.delay_mechanism() != null) ? Cast<VhdlElement, DelayMechanism>(VisitDelay_mechanism(opts_in.delay_mechanism())) : DelayMechanism.DUTY_CYCLE;
            bool is_guarded = opts_in.GUARDED() != null;

            ISignalAssignmentTarget target = Cast<VhdlElement, ISignalAssignmentTarget>(VisitTarget(target_in));
            Expression exp = Cast<VhdlElement, Expression>(VisitExpression(expression_in));

            List<VHDL.concurrent.SelectedSignalAssignment.SelectedWaveform> SWEs = new List<SelectedSignalAssignment.SelectedWaveform>();
            for(int i=0; i<selected_waveforms_in.choices().Length; i++)
            {
                var choises_in = selected_waveforms_in.choices()[i];
                var waveform_in = selected_waveforms_in.waveform()[i];

                List<Choice> choices = new List<Choice>();
                List<WaveformElement> waveforms = new List<WaveformElement>();
                foreach (var ch_in in choises_in.choice())
                {
                    Choice ch = Cast<VhdlElement, Choice>(VisitChoice(ch_in));
                    choices.Add(ch);
                }
                foreach (var w_in in waveform_in.waveform_element())
                {
                    WaveformElement we = Cast<VhdlElement, WaveformElement>(VisitWaveform_element(w_in));
                    waveforms.Add(we);
                }

                VHDL.concurrent.SelectedSignalAssignment.SelectedWaveform SWE = new SelectedSignalAssignment.SelectedWaveform(waveforms, choices);
                SWEs.Add(SWE);
            }

            SelectedSignalAssignment ssa = new SelectedSignalAssignment(exp, target, SWEs);
            ssa.DelayMechanism = delay;
            ssa.Guarded = is_guarded;

            return ssa;
        }

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
            var aggregate = context.aggregate();
            var allocator = context.allocator();
            var expression = context.expression();
            var function_call = context.function_call();
            var literal = context.literal();
            var qualified_expression = context.qualified_expression();


            if (aggregate != null)
            {
                return VisitAggregate(aggregate);
            }

            if (allocator != null)
            {
                return VisitAllocator(allocator);
            }

            if (expression != null)
            {
                return VisitExpression(expression);
            }

            if (function_call != null)
            {
                return VisitFunction_call(function_call);
            }

            if (literal != null)
            {
                return VisitLiteral(literal);
            }
            
            if (qualified_expression != null)
            {
                return VisitQualified_expression(qualified_expression);
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
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

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);
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
        public override VhdlElement VisitConcurrent_signal_assignment_statement([NotNull] vhdlParser.Concurrent_signal_assignment_statementContext context) 
        {
            var label_colon = context.label_colon();
            var selected_signal_assignment_in = context.selected_signal_assignment();
            var conditional_signal_assignment_in = context.conditional_signal_assignment();

            string label = (label_colon != null) ? label_colon.identifier().GetText() : string.Empty;
            bool is_postponed = context.POSTPONED() != null;

            AbstractPostponableConcurrentStatement apcs = null;
            if (selected_signal_assignment_in != null)
            {
                apcs = Cast<VhdlElement, SelectedSignalAssignment>(VisitSelected_signal_assignment(selected_signal_assignment_in));
            }

            if (conditional_signal_assignment_in != null)
            {
                apcs = Cast<VhdlElement, ConditionalSignalAssignment>(VisitConditional_signal_assignment(conditional_signal_assignment_in));
            }

            apcs.Postponed = is_postponed;
            apcs.Label = label;

            return apcs; 
        }

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
        public override VhdlElement VisitElement_declaration([NotNull] vhdlParser.Element_declarationContext context) 
        {
            var element_subtype_definition_in = context.element_subtype_definition();
            var identifier_list_in = context.identifier_list();

            List<string> identifiers = new List<string>();
            foreach (var i in identifier_list_in.identifier())
            {
                identifiers.Add(i.GetText());
            }

            VHDL.type.ISubtypeIndication si = Cast<VhdlElement, VHDL.type.ISubtypeIndication>(VisitElement_subtype_definition(element_subtype_definition_in));

            VHDL.type.RecordType.ElementDeclaration el = new VHDL.type.RecordType.ElementDeclaration(si, identifiers);

            return el; 
        }

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
        public override VhdlElement VisitConcurrent_assertion_statement([NotNull] vhdlParser.Concurrent_assertion_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            bool is_postponed = context.POSTPONED() != null;
            var assertion_in = context.assertion();

            string label = (label_colon_in != null)? label_colon_in.identifier().GetText() : string.Empty;
            AssertionStatement assertionStatement = Cast<VhdlElement, AssertionStatement>(VisitAssertion(assertion_in));
            assertionStatement.Label = label;

            ConcurrentAssertionStatement concurrentAssertionStatement = new ConcurrentAssertionStatement(assertionStatement);
            concurrentAssertionStatement.Postponed = is_postponed;

            return concurrentAssertionStatement;
        }

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
        public override VhdlElement VisitFile_type_definition([NotNull] vhdlParser.File_type_definitionContext context) 
        {
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.FileType file = new VHDL.type.FileType("unknown", si);
            return file;
        }

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
        public override VhdlElement VisitWait_statement([NotNull] vhdlParser.Wait_statementContext context) 
        {
            var condition_clause_in = context.condition_clause();
            var label_colon_in = context.label_colon();
            var sensitivity_clause_in = context.sensitivity_clause();
            var timeout_clause_in = context.timeout_clause();

            string label = (label_colon_in != null) ? label_colon_in.GetText() : string.Empty;
            List<Signal> sensitivityList = new List<Signal>();
            if ((sensitivity_clause_in != null) && (sensitivity_clause_in.sensitivity_list() != null))
            {
                foreach (var s_in in sensitivity_clause_in.sensitivity_list().name())
                {
                    Signal s = Cast<VhdlElement, Signal>(VisitName(s_in));
                    sensitivityList.Add(s);
                }
            }
            Expression timeout = (timeout_clause_in != null) ? Cast<VhdlElement, Expression>(VisitTimeout_clause(timeout_clause_in)) : null;
            Expression condition = (condition_clause_in != null) ? Cast<VhdlElement, Expression>(VisitCondition_clause(condition_clause_in)) : null;

            WaitStatement ws = new WaitStatement(sensitivityList)
            {
                Timeout = timeout,
                Label = label,
                Condition = condition
            };
            return ws;
        }

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
        public override VhdlElement VisitLoop_statement([NotNull] vhdlParser.Loop_statementContext context) 
        {
            var label_colon_in = context.label_colon();
            var iteration_scheme_in = context.iteration_scheme();
            var sequence_of_statements_in = context.sequence_of_statements();
            var identifier_in = context.identifier();

            string label_begin = (label_colon_in != null)? label_colon_in.identifier().GetText() : string.Empty;
            string label_end = (identifier_in != null) ? identifier_in.GetText() : string.Empty;

            //-------------------------------------------------------------
            //      Before parsing
            //-------------------------------------------------------------
            IDeclarativeRegion oldScope = currentScope;
            //-------------------------------------------------------------


            VHDL.statement.LoopStatement loop = Cast<VhdlElement, VHDL.statement.LoopStatement>(VisitIteration_scheme(iteration_scheme_in));

            loop.Label = label_begin;
            loop.Parent = oldScope;
            currentScope = loop;

            loop.Statements.AddRange(ParseList<vhdlParser.Sequential_statementContext, SequentialStatement>(sequence_of_statements_in.sequential_statement(), VisitSequential_statement));

            //-------------------------------------------------------------
            //      After parsing
            //-------------------------------------------------------------
            currentScope = oldScope;
            AddAnnotations(loop, context);
            //-------------------------------------------------------------

            if ((string.IsNullOrEmpty(label_end) == false) && (label_begin.Equals(label_end, StringComparison.InvariantCultureIgnoreCase) == false))
            {
                throw new ArgumentException(String.Format("Loop identifiers mismatch. Loop begin identifier is '{0}' and end identifier is '{1}'", label_begin, label_end));
            }

            return loop; 
        }

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
            var subtype_indication_in = context.subtype_indication();
            var expression = context.expression();

            var BUS = context.BUS();

            isBus = BUS != null;
            hasMode = signal_mode != null;
            hasObjectClass = context.SIGNAL() != null;

            InterfaceDeclarationFormat format = new InterfaceDeclarationFormat(hasObjectClass, hasMode);
            Signal.ModeEnum m = ParseSignalMode(signal_mode);

            Expression def = (expression != null)?Cast<VhdlElement, Expression>(VisitExpression(expression)):null;
            VHDL.type.ISubtypeIndication type = (subtype_indication_in != null)?Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication):null;

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
        public override VhdlElement VisitAccess_type_definition([NotNull] vhdlParser.Access_type_definitionContext context) 
        {
            var subtype_indication_in = context.subtype_indication();

            VHDL.type.ISubtypeIndication si = Parse<vhdlParser.Subtype_indicationContext, VHDL.type.ISubtypeIndication>(subtype_indication_in, VisitSubtype_indication);

            VHDL.type.AccessType access = new VHDL.type.AccessType("unknown", si);
            return access; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.report_statement"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitReport_statement([NotNull] vhdlParser.Report_statementContext context) 
        {
            var expressions_in = context.expression();
            var label_colon_in = context.label_colon();
            bool has_severity = context.SEVERITY() != null;

            VHDL.expression.Expression expression = Cast<VhdlElement, VHDL.expression.Expression>(VisitExpression(expressions_in[0]));

            ReportStatement report = null;
            if (has_severity)
            {
                VHDL.expression.Expression expression_severity = Cast<VhdlElement, VHDL.expression.Expression>(VisitExpression(expressions_in[1]));
                report = new ReportStatement(expression, expression_severity);
            }
            else
            {
                report = new ReportStatement(expression);
            }

            if (label_colon_in != null)
                report.Label = label_colon_in.identifier().GetText();

            return report; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.procedure_call"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitProcedure_call([NotNull] vhdlParser.Procedure_callContext context) 
        {
            var identifier_in = context.identifier();
            var actual_parameter_part_in = context.actual_parameter_part();

            string procedure_name = identifier_in.GetText();

            List<AssociationElement> parameters = ParseList<vhdlParser.Association_elementContext, AssociationElement>(actual_parameter_part_in.association_list().association_element(), VisitAssociation_element);

            ProcedureCall procedureCall = new ProcedureCall(procedure_name, parameters);

            return procedureCall; 
        }

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
        public override VhdlElement VisitWaveform_element([NotNull] vhdlParser.Waveform_elementContext context) 
        {
            var expression_in = context.expression();
            bool has_delay = context.AFTER() != null;

            Expression wa_value = Cast<VhdlElement, Expression>(VisitExpression(expression_in[0]));
            Expression wa_after = has_delay? Cast<VhdlElement, Expression>(VisitExpression(expression_in[1])) : null;

            WaveformElement wa = new WaveformElement(wa_value, wa_after);

            return wa; 
        }

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
        public override VhdlElement VisitType_declaration([NotNull] vhdlParser.Type_declarationContext context) 
        {
            var identifier_in =  context.identifier();
            var type_definition_in = context.type_definition();

            string typename = identifier_in.GetText();

            if (type_definition_in.access_type_definition() != null)
            {
                var access_type_definition_in = type_definition_in.access_type_definition();

                VHDL.type.AccessType access = Cast<VhdlElement, VHDL.type.AccessType>(VisitAccess_type_definition(access_type_definition_in));
                access.Identifier = typename;                
                return access;
            }
            
            if (type_definition_in.composite_type_definition() != null)
            {
                var composite_type_definition_in = type_definition_in.composite_type_definition();

                VHDL.type.Type composite = Cast<VhdlElement, VHDL.type.Type>(VisitComposite_type_definition(composite_type_definition_in));
                composite.Identifier = typename;
                return composite;
            }

            if (type_definition_in.file_type_definition() != null)
            {
                var file_type_definition_in = type_definition_in.file_type_definition();

                VHDL.type.FileType file = Cast<VhdlElement, VHDL.type.FileType>(VisitFile_type_definition(file_type_definition_in));
                file.Identifier = typename;
                return file;
            }

            if (type_definition_in.scalar_type_definition() != null)
            {
                var scalar_type_definition_in = type_definition_in.scalar_type_definition();

                VHDL.type.Type scalar = Cast<VhdlElement, VHDL.type.Type>(VisitScalar_type_definition(scalar_type_definition_in));
                scalar.Identifier = typename;
                return scalar;
            }

            throw new NotSupportedException(String.Format("Could not analyse item {0}", context.ToStringTree()));
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.attribute_declaration"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitAttribute_declaration([NotNull] vhdlParser.Attribute_declarationContext context) 
        {
            var label_colon_in = context.label_colon();
            var name_in = context.name();

            string attribute_name = label_colon_in.identifier().GetText();

            VHDL.type.ISubtypeIndication si = resolve<VHDL.type.ISubtypeIndication>(name_in.GetText());

            VHDL.declaration.Attribute attribute = new VHDL.declaration.Attribute(attribute_name, si);

            return attribute; 
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="vhdlParser.term"/>.
        /// <para>
        /// The default implementation returns the VhdlElement of calling <see cref="AbstractParseTreeVisitor{VhdlElement}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor VhdlElement.</return>
        public override VhdlElement VisitTerm([NotNull] vhdlParser.TermContext context) 
        {
            var factors_in = context.factor();
            var multiplying_operators_in = context.multiplying_operator();

            List<Expression> parsed_factors = new List<Expression>();

            foreach (var f in factors_in)
            {
                Expression parsed_factor = Cast<VhdlElement, Expression>(VisitFactor(f));
                parsed_factors.Add(parsed_factor);
            }

            if (parsed_factors.Count == 0)
                throw new NotSupportedException(String.Format("Could not analyse item {0}. Amount of factors is 0.", context.ToStringTree()));

            Expression res = parsed_factors[0];

            for (int i = 0; i < multiplying_operators_in.Length; i++ )
            {
                var op = multiplying_operators_in[i];
                Expression curr_exp = parsed_factors[i+1];

                if(op.DIV() != null)
                {
                    Expression new_exp = new VHDL.expression.Divide(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if(op.MOD() != null)
                {
                    Expression new_exp = new VHDL.expression.Mod(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if(op.MUL() != null)
                {
                    Expression new_exp = new VHDL.expression.Multiply(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                if(op.REM() != null)
                {
                    Expression new_exp = new VHDL.expression.Rem(res, curr_exp);
                    res = new_exp;
                    continue;
                }

                throw new NotSupportedException(String.Format("Could not analyse item {0}", op.ToStringTree()));

            }

            return res; 
        }

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
