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

namespace Schematix.Windows.Code
{
    /// <summary>
    /// Interaction logic for Code.xaml
    /// </summary>
    public partial class Code : SchematixBaseWindow
    {
        public Code(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
            : base (projectElement, core)
        {
            InitializeComponent();
            zoomSlider.Value = textEditor.FontSize;

            LoadText(ProjectElement);
            LoadOptions();
            textEditor.Focus();
        }

        public Code(ProjectExplorer.ProjectElement projectElement)
            : base(projectElement)
        {
            InitializeComponent();
            zoomSlider.Value = textEditor.FontSize;

            LoadText(ProjectElement);
            LoadOptions();
            textEditor.Focus();
        }

        FoldingManager foldingManager;
        AbstractFoldingStrategy foldingStrategy;
        string Language = string.Empty;

        public string EditorText
        {
            set
            {
                textEditor.Text = value;
            }
            get
            {
                return textEditor.Text;
            }
        }

        public override void Load()
        {
            LoadText(ProjectElement);
        }

        /// <summary>
        /// Используется для загрузки текста с файла
        /// </summary>
        /// <param name="projectElement"></param>
        private void LoadText(ProjectExplorer.ProjectElement projectElement)
        {
            if(projectElement is ProjectExplorer.VHDL_Code_File)
                Language = "VHDL";
            if(projectElement is ProjectExplorer.Verilog_Code_File)
                Language = "Verilog";

            textEditor.FilePath = projectElement.Path;

            textEditor.Encoding = System.Text.Encoding.GetEncoding("windows-1251");           

            ProjectExplorer.Project project = projectElement.Project;
            if (project != null)
            {
                textEditor.ChangeHighliting(Language);
                if(projectElement is ProjectExplorer.VHDL_Code_File)
                    textEditor.Compiler = project.Compiler.CurrentCompiler;
            }
            else
            {
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(Language);
            }

            textEditor.Load(projectElement.Path);
        }

        public override bool IsSaved()
        {
            return ! textEditor.IsModified;
        }

        public override void Save()
        {
            textEditor.Save(ProjectElement.Path);
        }

        public override void Save(string filePath)
        {
            textEditor.Save(filePath);
        }

        public override void Save(System.IO.Stream stream)
        {
            textEditor.Save(stream);
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

        void superior_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //textEditor.Compiler = superior.mCompiler.CurrentCompiler;
        }

        void textEditor_TextChanged(object sender, EventArgs e)
        {
            //RegisterChanged();
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

        private void compile()
        {
            /*
            superior.mOutBox.ClearMessages();
            superior.mCompiler.Compile(this.id);
            superior.mCompiler.ReparseLibrary();
            */
        }

        private void Code_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void HighlightingComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            textEditor.WordWrap = !textEditor.WordWrap;
        }

        private void toolStripButtonShowLLineNumbers_Click(object sender, RoutedEventArgs e)
        {
            textEditor.ShowLineNumbers = !textEditor.ShowLineNumbers;
        }

        private void toolStripButtonShowHiddenSymbols_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowEndOfLine = !textEditor.Options.ShowEndOfLine;
        }

        private void toolStripButtonCommentLines_Click(object sender, RoutedEventArgs e)
        {
            textEditor.CommentLines();
        }

        private void toolStripButtonUnCommentLines_Click(object sender, RoutedEventArgs e)
        {
            textEditor.UnCommentLines();
        }

        private void toolStripButtonIndentHS_Click(object sender, RoutedEventArgs e)
        {
            textEditor.IndentHS();
        }

        private void toolStripButtonOutdentHS_Click(object sender, RoutedEventArgs e)
        {
            textEditor.OutdentHS();
        }

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ! string.IsNullOrEmpty(textEditor.SelectedText);
            base.Copy_CanExecute(sender, e);
        }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Copy();
            base.Copy_Executed(sender, e);
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ! string.IsNullOrEmpty(textEditor.SelectedText);
            base.Cut_CanExecute(sender, e);
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Cut();
            base.Cut_Executed(sender, e);
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText();
            base.Paste_CanExecute(sender, e);
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Paste();
            base.Paste_Executed(sender, e);
        }

        public override void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ! string.IsNullOrEmpty(textEditor.SelectedText);
            base.Delete_CanExecute(sender, e);
        }

        public override void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Document.Remove(textEditor.SelectionStart, textEditor.SelectionLength);
            base.Delete_Executed(sender, e);
        }

        public override void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = textEditor.Text.Length != 0;
            base.SelectAll_CanExecute(sender, e);
        }

        public override void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.SelectAll();
            base.SelectAll_Executed(sender, e);
        }

        public override void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textEditor.IsModified == true)
            e.CanExecute = true;
            base.Save_CanExecute(sender, e);
        }

        public override void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Save(ProjectElement.Path);
            base.Save_Executed(sender, e);
        }

        public override void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = textEditor.CanUndo;
            base.Undo_CanExecute(sender, e);
        }

        public override void  Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Undo();
 	        base.Undo_Executed(sender, e);
        }

        public override void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = textEditor.CanRedo;
            base.Redo_CanExecute(sender, e);
        }

        public override void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Redo();
            base.Redo_Executed(sender, e);
        }


        public void SetZoom(int zoom)
        {
            textEditor.TextArea.FontSize = 10.0 * (double)zoom / (double)100;
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            //SearchReplace search = new SearchReplace(Core, this);
            //search.Show();
        }

        private void textEditor_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (textEditor.CurrentCaretLocation.IsEmpty != true)
            {
                textblockLine.Text = textEditor.CurrentCaretLocation.Line.ToString();
                textblockColumn.Text = textEditor.CurrentCaretLocation.Column.ToString(); ;
            }
        }

        private void textEditor_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (textEditor.CurrentCaretLocation.IsEmpty != true)
            {
                textblockLine.Text = textEditor.CurrentCaretLocation.Line.ToString();
                textblockColumn.Text = textEditor.CurrentCaretLocation.Column.ToString(); ;
            }
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textEditor.FontSize = zoomSlider.Value;
        }

        private void CheckBoxWordWrap_Checked(object sender, RoutedEventArgs e)
        {
            textEditor.WordWrap = true;
        }

        private void CheckBoxShowLineNumbers_Checked(object sender, RoutedEventArgs e)
        {
            textEditor.ShowLineNumbers = true;
        }

        private void CheckBoxShowEndOfLine_Checked(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowEndOfLine = true;
            textEditor.Options.ShowSpaces = true;
            textEditor.Options.ShowTabs = true;
        }

        private void CheckBoxWordWrap_Unchecked(object sender, RoutedEventArgs e)
        {
            textEditor.WordWrap = false;
        }

        private void CheckBoxShowLineNumbers_Unchecked(object sender, RoutedEventArgs e)
        {
            textEditor.ShowLineNumbers = false;
        }

        private void CheckBoxShowEndOfLine_Unchecked(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowEndOfLine = false;
            textEditor.Options.ShowSpaces = false;
            textEditor.Options.ShowTabs = false;
        }

        public override void OnClose()
        {
            textEditor.DisposeLexter();
        }

        private void LoadOptions()
        {
            textEditor.FontSize = CommonProperties.Configuration.CurrentConfiguration.TextEditorOptions.FontSize;
            zoomSlider.Value = CommonProperties.Configuration.CurrentConfiguration.TextEditorOptions.FontSize;
            textEditor.FontFamily = new FontFamily(CommonProperties.Configuration.CurrentConfiguration.TextEditorOptions.FontName);
            
            textEditor.FontWeight = CommonProperties.Configuration.CurrentConfiguration.TextEditorOptions.FontBold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
            textEditor.FontStyle = CommonProperties.Configuration.CurrentConfiguration.TextEditorOptions.FontItalic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
        }

        public void SetPosition(int line, int position)
        {
            textEditor.Select(line, position);
            My_Editor.Document.DocumentLine doc_line = textEditor.TextArea.Document.GetLineByNumber(line);
            textEditor.CaretOffset = doc_line.Offset + position;
            textEditor.ScrollTo(line, position);
        }

        private void textEditor_Loaded(object sender, RoutedEventArgs e)
        {
            textEditor.Focus();
        }
    }
}
