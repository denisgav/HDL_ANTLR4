using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VHDL;
using VHDL.parser;

namespace Tests
{
    [TestClass]
    public class LibraryUnitTests
    {
        private static StreamWriter writer;
        private static Logger logger;
        private static VHDL_Library_Manager libraryManager;

        static LibraryUnitTests()
        {
            writer = new StreamWriter(new MemoryStream());
            logger = new VHDL.parser.Logger(writer.BaseStream);
            libraryManager = new VHDL_Library_Manager("", @"Libraries\LibraryRepository.xml", logger);
        }

        private VhdlFile parseCode(string code)
        {
            return VhdlParserWrapper.parseString(code, libraryManager);
        }

        [TestMethod]
        public void SingleEntityNoDecls()
        {
            string code =
                "entity e is    \n" +
                "end entity;    \n";
            
            var file = parseCode(code);

            Assert.AreEqual(file.Elements.Count, 1);

            var entity = file.Elements[0] as VHDL.libraryunit.Entity;
            Assert.IsNotNull(entity);
            Assert.AreEqual(entity.Identifier, "e");
            Assert.AreEqual(entity.Architectures.Count, 0);
        }

        [TestMethod]
        public void EntityWithArchitecture()
        {
            string code =
                "entity e is            \n" +
                "end entity;            \n" +
                "architecture a of e    \n" +
                "is begin               \n" +
                "end architecture;      \n";

            var file = parseCode(code);

            Assert.AreEqual(file.Elements.Count, 2);

            var entity = file.Elements[0] as VHDL.libraryunit.Entity;
            Assert.IsNotNull(entity);
            Assert.AreEqual(entity.Architectures.Count, 1);

            var architecture = entity.Architectures[0];
            Assert.IsNotNull(architecture);
            Assert.AreEqual(architecture.Entity, entity);
            Assert.AreEqual(architecture.Identifier, "a");
        }
    }
}
