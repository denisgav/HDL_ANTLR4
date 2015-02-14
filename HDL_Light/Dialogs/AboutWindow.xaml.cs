/*
 * Created by SharpDevelop.
 * User: Denis
 * Date: 29.06.2013
 * Time: 21:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Schematix
{
	public class AssemblyInfo
	{
		string name;		
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		
		string path;		
		public string Path
		{
			get { return path; }
			set { path = value; }
		}

		string version;		
		public string Version
		{
			get { return version; }
			set { version = value; }
		}
		
		public AssemblyInfo()
		{}
		
		public AssemblyInfo(string name, string path, string version)
		{
			this.name = name;
			this.path = path;
			this.version = version;
		}
		
		public AssemblyInfo(System.Reflection.Assembly asm)
		{
			this.name = asm.GetName().Name;
			this.path = asm.Location;
			this.version = asm.GetName().Version.ToString();
		}
	}
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
		}
		
		void ButtonOk_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		void Window_Loaded(object sender, RoutedEventArgs e)
		{
			foreach(System.Reflection.AssemblyName an in System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies())
			{
            	System.Reflection.Assembly asm = System.Reflection.Assembly.Load(an.ToString());
            	AssemblyInfo inf = new AssemblyInfo(asm);
            	ListViewAssemblies.Items.Add(inf);
            }
		}
	}
}