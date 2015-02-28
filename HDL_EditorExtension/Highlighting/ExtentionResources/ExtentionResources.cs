// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5583 $</version>
// </file>

using System;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Xml;
using System.Diagnostics;

namespace HDL_EditorExtension.Highlighting
{
	public static class ExtentionResources
	{
        static readonly string Prefix = typeof(ExtentionResources).FullName + ".";
		
		public static Stream OpenStream(string name)
		{
            Stream s = typeof(ExtentionResources).Assembly.GetManifestResourceStream(Prefix + name);
			if (s == null)
				throw new FileNotFoundException("The resource file '" + name + "' was not found.");
			return s;
		}

        public static void RegisterBuiltInHighlightings(ExtendedHighlightingManager hlm)
		{
            hlm.RegisterHighlighting("VHDL", new[] { ".vhd" }, "VHDL.xshd");
            hlm.RegisterHighlighting("Verilog", new[] { ".v" }, "Verilog.xshd");
		}
        
	}
}
