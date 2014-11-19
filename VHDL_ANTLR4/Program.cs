/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 15.11.2014
 * Time: 16:59
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

namespace VHDL_ANTLR4
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			Stream stream = new FileStream("sample.vhdl", FileMode.Open);
			StreamReader inputStream = new StreamReader(stream);
			      AntlrInputStream input = new AntlrInputStream(inputStream.ReadToEnd());
			      vhdlLexer lexer = new vhdlLexer(input);
			      CommonTokenStream tokens = new CommonTokenStream(lexer);
			      vhdlParser parser = new vhdlParser(tokens);
			      IParseTree tree = parser.design_file();
			      Console.WriteLine(tree.ToStringTree(parser));
			      //VhdlAntlrVisitor visitor = new VhdlAntlrVisitor();
			      //Console.WriteLine(visitor.Visit(tree));
			
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}