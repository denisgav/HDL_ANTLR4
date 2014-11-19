/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 19.11.2014
 * Time: 17:00
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

namespace Verilog_ANTLR4
{
	class Program
	{
		public static void Main(string[] args)
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
	}
}