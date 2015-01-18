/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 22.11.2014
 * Time: 20:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Verilog_ANTLR4;
using VHDL_ANTLR4;
using VHDL.parser;
using VHDL;

namespace ParserSample
{
    class Program
    {
        public static void Main(string[] args)
        {
            VHDLSample();
        }

        public static void VerilogSample()
        {
            Console.WriteLine("Hello World!");

            // TODO: Implement Functionality Here
            Stream stream = new FileStream("mux_using_assign.v", FileMode.Open);
            StreamReader inputStream = new StreamReader(stream);
            AntlrInputStream input = new AntlrInputStream(inputStream.ReadToEnd());
            Verilog2001Lexer lexer = new Verilog2001Lexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            Verilog2001Parser parser = new Verilog2001Parser(tokens);
            IParseTree tree = parser.source_text();
            Console.WriteLine(tree.ToStringTree(parser));
            //VhdlAntlrVisitor visitor = new VhdlAntlrVisitor();
            //Console.WriteLine(visitor.Visit(tree));



            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        public static void VHDLSample()
        {
            Console.WriteLine("Hello World!");

            // TODO: Implement Functionality Here
            string appBase = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            VHDL.parser.Logger loggercompile = VHDL.parser.Logger.CreateLogger(appBase, "compiler");

            VHDL_Library_Manager libraryManager = new VHDL_Library_Manager("", @"Libraries\LibraryRepository.xml", loggercompile);
            libraryManager.Logger.OnWriteEvent += new VHDL.parser.Logger.OnWriteDeleagte(Logger_OnWriteEvent);
            libraryManager.LoadData(@"Libraries");
            VhdlParserSettings settings = VhdlParserWrapper.DEFAULT_SETTINGS;
            RootDeclarativeRegion rootScope = new RootDeclarativeRegion();
            LibraryDeclarativeRegion currentLibrary = new LibraryDeclarativeRegion("work");
            rootScope.Libraries.Add(currentLibrary);
            rootScope.Libraries.Add(libraryManager.GetLibrary("STD"));

            Console.WriteLine("Parsing code");
            VhdlFile file = VhdlParserWrapper.parseFile("sample.vhdl", settings, rootScope, currentLibrary, libraryManager);
            Console.WriteLine("Parsing complete");


            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        static void Logger_OnWriteEvent(VHDL.parser.LoggerMessageVerbosity verbosity, string message)
        {
            Console.WriteLine(String.Format("[{0}] {1}", verbosity, message));
        }
    }
}