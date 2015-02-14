﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision$</version>
// </file>

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                         //
// DO NOT EDIT GlobalAssemblyInfo.cs, it is recreated using AssemblyInfo.template whenever //
// ICSharpCode.Core is compiled.                                                           //
//                                                                                         //
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////

using System.Resources;
using System.Reflection;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("SharpDevelop")]
[assembly: AssemblyCopyright("2000-2010 Denis Gavrysh Corp.")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]
[assembly: NeutralResourcesLanguage("en-US")]

internal static class RevisionClass
{
	public const string Major = "1";
	public const string Minor = "0";
	public const string Build = "0";
	public const string Revision = "0";
	
	public const string MainVersion = Major + "." + Minor;
	public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision;
	
	public const string BranchName = null;
}
