using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Schematix.Windows.Waveform
{
    /// <summary>
    /// Interaction logic for Waveform.xaml
    /// </summary>
    public partial class Waveform : SchematixBaseWindow
    {
        public Waveform(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
            : base (projectElement, core)
        {
            InitializeComponent();
            waveformUserControl1.LoadVCDFile(projectElement.Path);
        }

        public Waveform(ProjectExplorer.ProjectElement projectElement)
            : base(projectElement)
        {
            InitializeComponent();
            waveformUserControl1.LoadVCDFile(projectElement.Path);
            waveformUserControl1.Run += new Schematix.Waveform.WaveformUserControl.RunDelegate(waveformUserControl1_Run);
        }

        void waveformUserControl1_Run(Schematix.Waveform.WaveformCore waveformCore)
        {
            if ((Core != null) && (Core.IsCompilerBusy == false))
            {
                //1. Сохраняем VCD файл
                waveformUserControl1.SaveVCDFile(ProjectElement.Path);

                //2. Генерируем тест
                string TBPath = Core.CurrentCompiler.GenerateTestBenchFileName(waveformCore.FileName, waveformCore.EntityName, waveformCore.ArchitectureName);
                string TBArchName = "testbench_architecture";
                string TBEntityName = string.Format("{0}_testbench", waveformCore.EntityName);
                waveformCore.GenerateTestBench(TBPath);

                string VCDPath = Core.CurrentCompiler.GenerateVCDFileName(waveformCore.FileName, waveformCore.EntityName, waveformCore.ArchitectureName);


                if (Core.CmdConsole != null)
                    Core.CurrentCompiler.ProcessInterface = Core.CmdConsole.ProcessInterface;
                Core.CurrentCompiler.CreateTestBenchDiagram(VCDPath, TBPath, TBEntityName, TBArchName, waveformCore.FileName, waveformCore.EntityName, waveformCore.ArchitectureName);
            }
        }

        public override void Load()
        {
            waveformUserControl1.LoadVCDFile(ProjectElement.Path);
        }

        public void SetParameters(string vhdFile, string EntityName, string ArchitectureName)
        {
            waveformUserControl1.Core.FileName = vhdFile;            
            waveformUserControl1.Core.ArchitectureName = ArchitectureName;
            waveformUserControl1.Core.EntityName = EntityName;
            waveformUserControl1.Core.AnalyseVHDLFile();
        }

        public override bool IsSaved()
        {
            return (waveformUserControl1 != null) && (waveformUserControl1.Core != null) && (waveformUserControl1.Core.IsModified == false);
        }

        public override void Save()
        {
            waveformUserControl1.SaveVCDFile(ProjectElement.Path);
        }

        public override void Save(string filePath)
        {
            waveformUserControl1.SaveVCDFile(filePath);
        }

        public override void Save(System.IO.Stream stream)
        {
            string confPath = System.IO.Path.ChangeExtension(ProjectElement.Path, "conf");
            waveformUserControl1.SaveVCDFile(stream, confPath);
        }

        public override void OnClose()
        {
            Save();
        }        

        public override IList<ToolBar> GetListOfToolBars()
        {
            return waveformUserControl1.GetListOfToolBars();
        }

        public override ToolBarTray GetToolBarTray()
        {
            return waveformUserControl1.GetToolBarTray();
        }

        public override System.Windows.Controls.Primitives.StatusBar GetStatusBar()
        {
            if (StatusBarMain.Parent != null)
                GridContent.Children.Remove(StatusBarMain);
            return StatusBarMain;
        }

        #region standard handlers

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Copy_CanExecute(sender, e);
        }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Copy_Executed(sender, e);
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Cut_CanExecute(sender, e);
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Cut_Executed(sender, e);
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Paste_CanExecute(sender, e);
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Paste_Executed(sender, e);
        }

        public override void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Delete_CanExecute(sender, e);
        }

        public override void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Delete_Executed(sender, e);
        }

        public override void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.SelectAll_CanExecute(sender, e);
        }

        public override void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.SelectAll_Executed(sender, e);
        }

        public override void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            base.Save_CanExecute(sender, e);
        }

        public override void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            waveformUserControl1.SaveVCDFile(ProjectElement.Path);
            base.Save_Executed(sender, e);
        }

        public override void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Undo_CanExecute(sender, e);
        }

        public override void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Undo_Executed(sender, e);
        }

        public override void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            base.Redo_CanExecute(sender, e);
        }

        public override void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.Redo_Executed(sender, e);
        }
        #endregion
    }
}
