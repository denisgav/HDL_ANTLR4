using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class NameTests : BaseTestFixture
    {
        [TestMethod]
        public void SimpleName()
        {
            string code =
                "entity e is                    \n" +
                "end entity;                    \n" +
                "architecture a of e is         \n" +
                " signal i1, i2 : integer;      \n" +
                "begin                          \n" +
                " i1 <= i2;                     \n" +
                "end architecture;              \n";

            var file = parseCode(code);

            var architecture = file.Elements[1] as VHDL.libraryunit.Architecture;
            var assign = architecture.Statements[0] as VHDL.concurrent.ConditionalSignalAssignment;
            var decl = architecture.Declarations[0] as VHDL.declaration.SignalDeclaration;

            CheckName(assign.Target, decl.Objects[0], "i1");
            CheckName(assign.ConditionalWaveforms[0].Waveform[0].Value, decl.Objects[1], "i2");
        }

        [TestMethod]
        public void SelectedName()
        {
            string code =
                "package p is                   \n" +
                " signal s : integer;           \n" +
                "end package;                   \n" +
                "entity e is                    \n" +
                "end entity;                    \n" +
                "library work; use work.p.all;  \n" +
                "architecture a of e is         \n" +
                " signal i1, i2 : integer;      \n" +
                "begin                          \n" +
                " i1 <= p.s;                    \n" +
                " work.p.s <= i2;               \n" +
                "end architecture;              \n";

            var file = parseCode(code);

            var package = file.Elements[0] as VHDL.libraryunit.PackageDeclaration;
            var architecture = file.Elements[2] as VHDL.libraryunit.Architecture;
            var assign1 = architecture.Statements[0] as VHDL.concurrent.ConditionalSignalAssignment;
            var assign2 = architecture.Statements[1] as VHDL.concurrent.ConditionalSignalAssignment;
            var decl1 = package.Declarations[0] as VHDL.declaration.SignalDeclaration;

            var name1 = assign1.ConditionalWaveforms[0].Waveform[0].Value as VHDL.expression.name.SelectedName;
            CheckSelName(name1, package, "p", decl1.Objects[0], "s");
            Assert.AreEqual(name1.Referenced, decl1.Objects[0]);

            var name2 = assign2.Target as VHDL.expression.name.SelectedName;
            Assert.IsNotNull(name2);
            CheckName(name2.getPrefix(), file.Parent, "work");
            CheckSelName(name2.getSuffix(), package, "p", decl1.Objects[0], "s");
            Assert.AreEqual(name2.Referenced, decl1.Objects[0]);
        }

        private void CheckSelName(object construct, object prefixDecl, string prefixId,
            object suffixDecl, string suffixId)
        {
            var name = construct as VHDL.expression.name.SelectedName;
            Assert.IsNotNull(name);
            CheckName(name.getPrefix(), prefixDecl, prefixId);
            CheckName(name.getSuffix(), suffixDecl, suffixId);
        }

        [TestMethod]
        public void IndexedName()
        {
            string code =
                "entity e is                    \n" +
                "end entity;                    \n" +
                "architecture a of e is         \n" +
                " signal s: bit_vector(0 to 2); \n" +
                "begin                          \n" +
                " s(0) <= '1';                  \n" +
                "end architecture;              \n";

            var file = parseCode(code);

            var architecture = file.Elements[1] as VHDL.libraryunit.Architecture;
            var assign = architecture.Statements[0] as VHDL.concurrent.ConditionalSignalAssignment;
            var decl = architecture.Declarations[0] as VHDL.declaration.SignalDeclaration;

            var name = assign.Target as VHDL.expression.name.IndexedName;
            Assert.IsNotNull(name);
            CheckName(name, decl.Objects[0], "s");
            CheckName(name.Prefix, decl.Objects[0], "s");

            Assert.AreEqual(name.Indices.Count, 1);
            CheckIntLiteral(name.Indices[0], "0");
        }

        [TestMethod]
        public void SliceName()
        {
            string code =
                "entity e is                    \n" +
                "end entity;                    \n" +
                "architecture a of e is         \n" +
                " signal s: bit_vector(0 to 2); \n" +
                "begin                          \n" +
                " s(0 to 1) <= \"01\";          \n" +
                "end architecture;              \n";

            var file = parseCode(code);

            var architecture = file.Elements[1] as VHDL.libraryunit.Architecture;
            var assign = architecture.Statements[0] as VHDL.concurrent.ConditionalSignalAssignment;
            var decl = architecture.Declarations[0] as VHDL.declaration.SignalDeclaration;

            var name = assign.Target as VHDL.expression.name.Slice;
            CheckName(name, decl.Objects[0], "s");
            CheckName(name.Prefix, decl.Objects[0], "s");

            Assert.AreEqual(name.Ranges.Count, 1);
            CheckRange(name.Ranges[0], VHDL.Range.RangeDirection.TO, "0", "1");
        }

        [TestMethod]
        public void AttributeName()
        {
            string code =
                "entity e is                    \n" +
                "end entity;                    \n" +
                "architecture a of e is         \n" +
                " signal s: bit_vector(0 to 2); \n" +
                " signal i: integer;            \n" +
                "begin                          \n" +
                " i <= s'length;                \n" +
                "end architecture;              \n";

            var file = parseCode(code);

            var architecture = file.Elements[1] as VHDL.libraryunit.Architecture;
            var assign = architecture.Statements[0] as VHDL.concurrent.ConditionalSignalAssignment;
            var decl = architecture.Declarations[0] as VHDL.declaration.SignalDeclaration;
            var attrDecl = VHDL.declaration.Attribute.GetStandardAttribute("length");

            var name = assign.ConditionalWaveforms[0].Waveform[0].Value as VHDL.expression.name.AttributeName;
            Assert.IsNotNull(name);
            CheckName(name.Prefix, decl.Objects[0], "s");
            CheckName(name, attrDecl, "LENGTH");

            Assert.IsNotNull(name.Attribute);
            Assert.AreEqual(name.Attribute, attrDecl);
        }

    }
}
