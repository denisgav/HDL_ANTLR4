# HDL ANTLR
It's a set of tools for VHDL/Verilog parsing, dynamic compilation and simulation.
Today, the repository contain following components:
* **VHDL** - syntax/semantic model of the VHDL language
* **VHDL_ANTLR4** - parsing of VHDL and building of model
* **Verilog_ANTLR4** - Verilog parsing
* **VHDLCompiler**
* **VHDLInputGenerators**
* **VHDLOutput**
* **VHDLRuntime**
* **ParserSample**
* **ModelingSystemTest**

### Contribution
 Currently, solution can be built with Visual Studio. NuGet package manager is required.
 How to build:
* VS 2013 - just open solution and build it.
* VS 2010 - it has some problems with packages restore and further grammar generation, so you can:
  * run *restore.cmd* to restore packages, then open solution and build it
  * open solution, then build(it will get a lot of errors), then close it and open again, and build
* MSBuild command line - run *build.cmd*
