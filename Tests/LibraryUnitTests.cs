using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class LibraryUnitTests : BaseTestFixture
    {
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

        [TestMethod]
        public void PackageDecl()
        {
            string code =
                "package p is            \n" +
                "end package;            \n";

            var file = parseCode(code);

            Assert.AreEqual(file.Elements.Count, 1);

            var package = file.Elements[0] as VHDL.libraryunit.PackageDeclaration;
            Assert.IsNotNull(package);
            Assert.AreEqual(package.Identifier, "p");
            Assert.IsNull(package.PackageBody);
        }
    }
}
