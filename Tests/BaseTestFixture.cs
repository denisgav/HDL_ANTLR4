using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VHDL;
using VHDL.parser;

namespace Tests
{
    public class BaseTestFixture
    {
        protected StreamWriter writer;
        protected Logger logger;
        protected VHDL_Library_Manager libraryManager;

        protected BaseTestFixture()
        {
            this.writer = new StreamWriter(new MemoryStream());
            this.logger = new VHDL.parser.Logger(writer.BaseStream);
            this.libraryManager = new VHDL_Library_Manager("", @"Libraries\LibraryRepository.xml", logger);
        }

        protected VhdlFile parseCode(string code)
        {
            return VhdlParserWrapper.parseString(code, libraryManager);
        }

        // Below are utility functions for language constructs checking.

        protected void CheckName(object construct, object decl, string id)
        {
            var name = construct as VHDL.expression.Name;
            Assert.IsNotNull(name);
            Assert.IsNotNull(name.Referenced);
            Assert.IsTrue(name.Referenced.Identifier.EqualsIdentifier(id));
            Assert.AreEqual(name.Referenced, decl);
        }

        protected void CheckRange(object construct, VHDL.Range.RangeDirection dir, string from, string to)
        {
            var r = construct as VHDL.Range;
            Assert.IsNotNull(r);
            Assert.AreEqual(r.Direction, dir);
            CheckIntLiteral(r.From, from);
            CheckIntLiteral(r.To, to);
        }

        protected void CheckCharLiteral(object construct, char val)
        {
            var lit = construct as VHDL.literal.CharacterLiteral;
            Assert.IsNotNull(lit);
            Assert.AreEqual(lit.Character, val);
        }

        protected void CheckEnumLiteral(object construct, string val)
        {
            var lit = construct as VHDL.literal.EnumerationLiteral;
            Assert.IsNotNull(lit);
            Assert.AreEqual(lit.ToString(), val);
        }

        protected void CheckIntLiteral(object construct, string val)
        {
            var lit = construct as VHDL.literal.IntegerLiteral;
            Assert.IsNotNull(lit);
            Assert.AreEqual(lit.Value, val);
            Assert.AreEqual(lit.IntegerValue, int.Parse(val));
        }
    }
}
