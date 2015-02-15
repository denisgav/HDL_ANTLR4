using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DeclarationTest : BaseTestFixture
    {
        [TestMethod]
        public void EnumTypeDecl()
        {
            string code =
                "package p is           \n" +
                " type e is (a,b);      \n" +
                "end package;           \n";

            var file = parseCode(code);

            var package = file.Elements[0] as VHDL.libraryunit.PackageDeclaration;

            var typeDecl = package.Declarations[0] as VHDL.type.EnumerationType;
            Assert.IsNotNull(typeDecl);
            Assert.AreEqual(typeDecl.Identifier, "e");
            Assert.AreEqual(typeDecl.Literals.Count, 2);

            CheckIdLiteral(typeDecl.Literals[0], "a");
            CheckIdLiteral(typeDecl.Literals[1], "b");
        }

        private void CheckIdLiteral(VHDL.literal.EnumerationLiteral obj, string id)
        {
            var literal = obj as VHDL.type.EnumerationType.IdentifierEnumerationLiteral;
            Assert.IsNotNull(literal);
            Assert.AreEqual(literal.getLiteral(), id);
        }
    }
}
