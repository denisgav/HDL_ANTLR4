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
using System.Windows.Controls.Primitives;

namespace Schematix.Windows.FSM
{
    /// <summary>
    /// Interaction logic for FSM.xaml
    /// </summary>
    public partial class FSM : SchematixBaseWindow
    {
        private Schematix.FSM.Constructor_Core constructorCore;
        public Schematix.FSM.Constructor_Core ConstructorCore
        {
            get { return constructorCore; }
        }

        public FSM(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
            : base (projectElement, core)
        {
            InitializeComponent();
            constructor1.core.UpdateHistory += new Schematix.FSM.Constructor_Core.UpdateHistoryDelegate(core_UpdateHistory);
            constructorCore = constructor1.core;
            try
            {
	            constructor1.core.OpenFile(projectElement.AbsolutePath);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open FSM file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open FSM file: {0}", projectElement.AbsolutePath), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }        

        public FSM(ProjectExplorer.ProjectElement projectElement)
            : base(projectElement)
        {
            InitializeComponent();
            constructor1.core.UpdateHistory += new Schematix.FSM.Constructor_Core.UpdateHistoryDelegate(core_UpdateHistory);
            constructorCore = constructor1.core;
            try
            {
            	constructor1.core.OpenFile(projectElement.AbsolutePath);
            }
            catch(Exception ex)
            {
            	//Не удалось открыть файл
                Schematix.Core.Logger.Log.Error(string.Format("Could not open FSM file: {0}", projectElement.Path), ex);
            	MessageBox.Show(string.Format("Could not open FSM file: {0}", projectElement.AbsolutePath), "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void Load()
        {
            constructor1.core.OpenFile(ProjectElement.AbsolutePath);
        }

        void core_UpdateHistory()
        {

        }

        public override bool IsSaved()
        {
        	return (constructorCore != null) && (constructorCore.Graph_History != null) && (constructorCore.Graph_History.CurrentElement != null) && (constructorCore.Graph_History.CurrentElement.IsSaved);
        }

        public override void Save()
        {
            constructorCore.SaveToFile(ProjectElement.Path);
        }

        public override void Save(string filePath)
        {
            constructorCore.SaveToFile(filePath, false);
        }

        public override void Save(System.IO.Stream stream)
        {
            constructorCore.SaveToFile(stream, false);
        }

        public override void OnClose()
        {
            //Дописать
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

        public void SetMode(string Mode)
        {
            switch (Mode)
            {
                case "CreateConstant":
                    //ButtonConstant.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewConstantMode();
                    break;

                case "PointerMode":
                    //ButtonPointer.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.mode = Schematix.FSM.FSM_MODES.MODE_SELECT;
                    break;

                case "CreateReset":
                    //ButtonReset.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateReset();
                    break;

                case "CreateSignal":
                    //ButtonSignal.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewSignalMode();
                    break;

                case "CreateState":
                    //ButtonState.IsChecked = true;
                    if (constructorCore != null)
                        constructor1.core.CreateNewStateMode();
                    break;

                case "CreateComment":
                    //ButtonComment.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewCommentMode();
                    break;

                case "CreateTransition":
                    //ButtonTransition.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewTransitionMode();
                    break;

                case "CreateVariable":
                    break;

                case "CreateBidirectionalPort":
                    //ButtonBidirectional.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewPort(Schematix.FSM.My_Port.PortDirection.InOut);
                    break;

                case "CreateInputPort":
                    //ButtonInput.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewPort(Schematix.FSM.My_Port.PortDirection.In);
                    break;

                case "CreateOutputPort":
                    //ButtonOutput.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewPort(Schematix.FSM.My_Port.PortDirection.Out);
                    break;

                case "CreateBufferPort":
                    //ButtonBuffer.IsChecked = true;
                    if (constructorCore != null)
                        constructorCore.CreateNewPort(Schematix.FSM.My_Port.PortDirection.Buffer);
                    break;

                default:
                    if (constructorCore != null)
                        constructorCore.mode = Schematix.FSM.FSM_MODES.MODE_SELECT;
                    break;
            }
        }


        public string Version
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                System.Reflection.AssemblyName[] names = assembly.GetReferencedAssemblies();

                return names.FirstOrDefault<System.Reflection.AssemblyName>(n => n.Name.StartsWith("My_Editor")).FullName.ToString();
            }
        }        

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && constructorCore.CanCopyCut();
            base.Copy_CanExecute(sender, e);
        }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.CopyToClipboard();
            base.Copy_Executed(sender, e);
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && constructorCore.CanCopyCut();
            base.Cut_CanExecute(sender, e);
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.CutToClipboard();
            base.Cut_Executed(sender, e);
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && constructorCore.CanPaste();
            base.Paste_CanExecute(sender, e);
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.GetFromClipboard();
            base.Paste_Executed(sender, e);
        }

        public override void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && constructorCore.CanCopyCut();
            base.Delete_CanExecute(sender, e);
        }

        public override void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.Graph.DeleteFigure();
            base.Delete_Executed(sender, e);
        }

        public override void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && (constructorCore.Graph != null) && constructorCore.Graph.CanSelectAllFigures();
            base.SelectAll_CanExecute(sender, e);
        }

        public override void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.Graph.SelectAllFigures();
            base.SelectAll_Executed(sender, e);
        }

        public override void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
        	if ((constructorCore != null) && (constructorCore.Graph_History != null) && (constructorCore.Graph_History.CurrentElement.IsSaved == false))
                e.CanExecute = true;
            base.Save_CanExecute(sender, e);
        }

        public override void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.SaveToFile(ProjectElement.Path);
            base.Save_Executed(sender, e);
        }

        public override void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (constructorCore != null) && (constructorCore.Graph_History != null) && constructorCore.Graph_History.СanUndo();
            base.Undo_CanExecute(sender, e);
        }

        public override void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.UnDo();
            base.Undo_Executed(sender, e);
        }

        public override void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
        	e.CanExecute = (constructorCore != null) && (constructorCore.Graph_History != null) && constructorCore.Graph_History.СanRedo();
            base.Redo_CanExecute(sender, e);
        }

        public override void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            constructorCore.ReDo();
            base.Redo_Executed(sender, e);
        }


        public void SetZoom(int zoom)
        {
            if (constructorCore != null)
                constructorCore.ChangeScale(zoom);
        }

        

        

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetZoom((int)e.NewValue);
            if (textBlockZoom != null)
                textBlockZoom.Text = string.Format("{0} %", e.NewValue);
        }

        private void constructor1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        	if((constructorCore != null) && (constructorCore.Paper != null))
        	{
            	var res = constructorCore.Paper.ConvertToBitmapCoordinate(e);
            	string location = string.Format("X: {0}; Y:{1}", res.X, res.Y);
            	textblockPosition.Text = location;
        	}
        }

        private void constructor1_Load(object sender, EventArgs e)
        {
            zoomSlider.Value = 100;
        }

        private void ButtonGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                constructorCore.GenerateCodeFile(sfd.FileName);
            }
            if(System.IO.File.Exists(sfd.FileName))
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

        private void ButtonPointer_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreatePointer");
        }

        private void ButtonComment_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateComment");
        }

        private void ButtonConstant_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateConstant");
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateReset");
        }

        private void ButtonSignal_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateSignal");
        }

        private void ButtonState_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateState");
        }

        private void ButtonTransition_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateTransition");
        }

        private void ButtonBidirectional_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateBidirectionalPort");
        }

        private void ButtonInput_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateInputPort");
        }

        private void ButtonOutput_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateOutputPort");
        }

        private void ButtonBuffer_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll(sender);
            SetMode("CreateBufferPort");
        }

        private void CommonToogleButton_Uncheck(object sender, RoutedEventArgs e)
        {
        }

        private void UncheckAll(object except)
        {
            foreach (ToolBar toolBar in MainToolBarTray.ToolBars)
            {
                foreach (object item in toolBar.Items)
                {
                    if ((item is ToggleButton) && (item != except) && ((item is CheckBox) == false))
                    {
                        (item as ToggleButton).IsChecked = false;
                    }
                }
            }
        }

        private void CheckBoxShowPriorityOfTransitions_Checked(object sender, RoutedEventArgs e)
        {
            if (constructorCore != null)
            {
                constructorCore.ShowLinePriority = true;
                constructor1.Invalidate();
            }
        }

        private void CheckBoxShowPriorityOfTransitions_Unchecked(object sender, RoutedEventArgs e)
        {
            if (constructorCore != null)
            {
                constructorCore.ShowLinePriority = false;
                constructor1.Invalidate();
            }
        }

        private void ButtonPointer_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
