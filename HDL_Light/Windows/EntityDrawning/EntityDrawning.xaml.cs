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
using My_Editor.Folding;
using System.ComponentModel;
using My_Editor.Highlighting;
using Schematix.EntityDrawning;
using Microsoft.Win32;

namespace Schematix.Windows.EntityDrawning
{
    /// <summary>
    /// Interaction logic for EntityDrawning.xaml
    /// </summary>
    public partial class EntityDrawning : SchematixBaseWindow
    {
        private EntityDrawningCore core;
        public EntityDrawning(ProjectExplorer.ProjectElement projectElement)
            : base(projectElement)
        {
            InitializeComponent();

            this.core = entityDrawningForm.core;
            try
            {
            	this.core.Picture.openProject(projectElement.Path);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open EDR file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open EDR file: {0}", projectElement.Path), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }  
            zoomSlider.Value = 100;
        }

        public override bool IsSaved()
        {
            return (core != null) && (core.History.IsSaved());
        }

        public EntityDrawning(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
            : base (projectElement, core)
        {
            InitializeComponent();
            this.core = entityDrawningForm.core;
            try
            {
	            this.core.Picture.openProject(projectElement.Path);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open EDR file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open EDR file: {0}", projectElement.Path), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }  
            //zoomSlider.Value = textEditor.FontSize;

            //LoadText(ProjectElement);
            //LoadOptions();
        }

        public override void Load()
        {
            core.Picture.openProject(ProjectElement.Path);
        }

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) &&  (core.CanCopyCut());
        }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CopyToClipboard();
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanCopyCut());
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CutToClipboard();
        }

        public override void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanCopyCut());
        }

        public override void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.DeleteFigure();
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanPaste());
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.GetFromClipboard();
        }

        public override void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanUnDo());
        }

        public override void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.UnDo();
        }

        public override void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanReDo());
        }

        public override void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ReDo();
        }

        public override void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.Picture.FigureList.Count != 0);
        }

        public override void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.SelectAll();
        }

        public override void Save()
        {
            core.Picture.SaveProject(ProjectElement.Path);
        }

        public override void Save(string filePath)
        {
            core.Picture.SaveProject(filePath);
        }

        public override void Save(System.IO.Stream stream)
        {
            core.Picture.SaveProject(stream);
        }

        public void SetMode(string Mode)
        {
            core.CreateNewFigureDragged(Mode);
        }

        #region Events
        

        private void ButtonOpenVHDLFile_Click(object sender, RoutedEventArgs e)
        {
            /*
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "VHDL File|*.vhdl|VHD file|*.vhd";
            if (ProjectElement.Parent != null)
                ofd.InitialDirectory = ProjectElement.Parent.Path;
            ofd.Title = "Select VHDL file";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                core.Picture.openVHDLFile(ofd.FileName);
            }
            */
            Schematix.EntityDrawning.AddElem nf = new Schematix.EntityDrawning.AddElem(ProjectElement.Path);
            nf.ShowDialog();
            if (nf.EntityDrawningInfo != null)
            {
                core.Picture.info = nf.EntityDrawningInfo;
                core.Picture.openVHDLFile(nf.EntityDrawningInfo.VHDLFileName, nf.EntityDrawningInfo.Entity.name);
                core.Picture.SaveProject(ProjectElement.Path);
            }
        }        

        private void SceckBoxShowLayers_Checked(object sender, RoutedEventArgs e)
        {
            if(core != null)
                core.ShowLayer = true;
        }

        private void SceckBoxShowLayers_Unchecked(object sender, RoutedEventArgs e)
        {
            if (core != null)
                core.ShowLayer = false;
        }
        #endregion

        public override void OnClose()
        {
            //throw new NotImplementedException();
        }

        private void entityDrawningForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var res = core.Paper.ConvertToBitmapCoordinate(e);
            string location = string.Format("X: {0}; Y:{1}", res.X, res.Y);
            textblockPosition.Text = location;
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(core != null)
                core.UpdateScale((int)e.NewValue);
            if (textBlockZoom != null)
                textBlockZoom.Text = string.Format("{0} %", e.NewValue);
        }

        private void BringToFront_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanCopyCut());
        }

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.BringToFront();
        }

        private void SendToBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.CanCopyCut());
        }

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.BringToBack();
        }

        private void entityDrawningForm_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            String command = e.Data.GetData("UnicodeText") as String;
            core.CreateNewFigureDragged(command);
        }

        private void ButtonArc_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewArc();
        }

        private void ButtonBezier_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewSplineBezier();
        }

        private void ButtonCurve_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewCurve();
        }

        private void ButtonEllipse_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewEllipse();
        }

        private void ButtonImage_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewImage();
        }

        private void ButtonLine_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewLine();
        }

        private void ButtonPath_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewPath();
        }

        private void ButtonPie_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewPie();
        }

        private void ButtonPolygon_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewPolygon();
        }

        private void ButtonPolyline_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewPolyline();
        }

        private void ButtonRectangle_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewRectangle();
        }

        private void ButtonText_Click(object sender, RoutedEventArgs e)
        {
            core.CreateNewText();
        }

        public override IList<ToolBar> GetListOfToolBars()
        {
            return MainToolBarTray.ToolBars;
        }

        public override ToolBarTray GetToolBarTray()
        {
            if (MainToolBarTray.Parent != null)
                DockPanelRoot.Children.Remove(MainToolBarTray);
            return MainToolBarTray;
        }

        public override System.Windows.Controls.Primitives.StatusBar GetStatusBar()
        {
            if (StatusBarMain.Parent != null)
                GridContent.Children.Remove(StatusBarMain);
            return StatusBarMain;
        }
    }
}
