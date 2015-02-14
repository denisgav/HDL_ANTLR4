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
using Microsoft.Win32;

namespace Schematix.Windows.Schema
{
    /// <summary>
    /// Interaction logic for Schema.xaml
    /// </summary>
    public partial class Schema : SchematixBaseWindow
    {
        public Schema(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
            : base (projectElement, core)
        {
            InitializeComponent();
            schemaUserControl.file.name = projectElement.AbsolutePath;
            try
            {
            	schemaUserControl.Open(projectElement.AbsolutePath);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open CSX file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open CSX file: {0}", projectElement.AbsolutePath), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            schemaUserControl.history.RegisterSave();
        }

        public Schema(ProjectExplorer.ProjectElement projectElement)
            : base(projectElement)
        {
            InitializeComponent();
            schemaUserControl.file.name = projectElement.AbsolutePath;
            try
            {
	            schemaUserControl.Open(projectElement.AbsolutePath);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open CSX file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open CSX file: {0}", projectElement.AbsolutePath), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            schemaUserControl.history.ClearHistory();
            schemaUserControl.history.Changed();
            schemaUserControl.history.RegisterSave();
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            schemaUserControl.SetZoom((int)e.NewValue);
            if (textBlockZoom != null)
                textBlockZoom.Text = string.Format("{0} %", e.NewValue);
        }

        public override void Load()
        {
            schemaUserControl.Open(ProjectElement.AbsolutePath);
        }

        public override void Save()
        {
            schemaUserControl.Save();
            schemaUserControl.history.RegisterSave();
        }

        public override void Save(string filePath)
        {
            schemaUserControl.file.name = filePath;
            schemaUserControl.Save();
            schemaUserControl.history.RegisterSave();
        }

        public override void Save(System.IO.Stream stream)
        {
            schemaUserControl.Save(stream);
            schemaUserControl.history.RegisterSave();
        }

        public override void OnClose()
        {
            
        }

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanCopy();
        }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.Copy();
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanCut();
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.Cut();
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanPaste();
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.Paste();
        }

        public override void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanDelete();
        }

        public override void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.Delete();
        }

        public override void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.history.canUndo;
        }

        public override void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.history.Undo();
        }

        public override void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.history.canRedo;
        }

        public override void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.history.Redo();
        }

        public override void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanSave();
        }

        public override void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        public override void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (schemaUserControl != null) && schemaUserControl.CanSelectAll();
        }

        public override void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            schemaUserControl.SelectAll();
        }

        public override bool IsSaved()
        {
            return (schemaUserControl != null) && (schemaUserControl.CanSave() == false);
        }

        private void schemaUserControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var res = schemaUserControl.Scale.ConvertToBitmapCoordinate(e);
            string location = string.Format("X: {0}; Y:{1}", res.X, res.Y);
            textblockPosition.Text = location;
        }

        public override IList<ToolBar> GetListOfToolBars()
        {
            return MainToolBarTray.ToolBars;
        }

        public override ToolBarTray GetToolBarTray()
        {
            if (MainToolBarTray.Parent != null)
                layoutRoot.Children.Remove(MainToolBarTray);
            return MainToolBarTray;
        }

        public override System.Windows.Controls.Primitives.StatusBar GetStatusBar()
        {
            if (StatusBarMain.Parent != null)
                layoutRoot.Children.Remove(StatusBarMain);
            return StatusBarMain;
        }

        private void ButtonGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                schemaUserControl.GenerateCode(sfd.FileName);
            }
            if (System.IO.File.Exists(sfd.FileName))
            {
                ProjectExplorer.ProjectElement newElem = ProjectExplorer.ProjectElement.CreateProjectElementByPath(sfd.FileName, ProjectElement.Parent as ProjectExplorer.ProjectFolder);
                if (newElem != null)
                {
                    (ProjectElement.Parent as ProjectExplorer.ProjectFolder).AddElement(newElem);
                    Core.SaveSolution();
                    Core.UpdateExplorerPanel();
                    Core.OpenNewWindow(newElem);
                }
            }
        }

        private void ButtonAddSignal_Click(object sender, RoutedEventArgs e)
        {
            schemaUserControl.addSignal();
        }

        private void ButtonAddPort_Click(object sender, RoutedEventArgs e)
        {
            schemaUserControl.addExternPort();
        }

        private void ButtonImportElements_Click(object sender, RoutedEventArgs e)
        {
            schemaUserControl.importVHDL();
        }
    }
}
