using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.CommonProperties
{
    public partial class Options : Form
    {
        TextEditorOptions _texEditorOptions;
        Configuration conf;
        public Options()
        {
            _texEditorOptions = new TextEditorOptions();

            InitializeComponent();
            conf = Configuration.CurrentConfiguration;
            conf.LoadDataToForm(this);
        }

        #region Get/Set Options
        /// <summary>
        /// загрузка данных из обьекта класса ProjectOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(ProjectOptions options)
        {
            textBoxDefaultProjectLocation.Text = options.DefaultProjectLocation;
            folderBrowserDialogDefaultProjectLocation.SelectedPath = options.DefaultProjectLocation;
        }

        /// <summary>
        /// загрузка данных из обьекта класса TextEditorOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(TextEditorOptions options)
        {
            options.LoadFontFamiliesToComboBox(comboBoxTextEditorSystemFonts);
            options.LoadGraphicsUnitToComboBox(comboBoxTextEditorFontSizeUnits);
            comboBoxTextEditorSystemFonts.SelectedItem = options.FontName;
            comboBoxTextEditorFontSizeUnits.SelectedItem = options.FontSizeUnits;

            comboBoxTextEditorFontSize.Text = options.FontSize.ToString();
            checkBoxTextEditorBold.Checked = options.FontBold;
            checkBoxTextEditorItalic.Checked = options.FontItalic;
            checkBoxTextEditorStrikeout.Checked = options.FontStrikeout;
            checkBoxTextEditorUnderLine.Checked = options.FontUnderLine;
        }

        /// <summary>
        /// загрузка данных из обьекта класса LibrariesOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(LibrariesOptions options)
        {
            listViewLibrariesVHDL.Items.Clear();
            foreach (string vhdlLibrary in options.VHDLLibrariesPaths)
            {
                ListViewItem item = new ListViewItem(vhdlLibrary);
                listViewLibrariesVHDL.Items.Add(item);
            }

            listViewLibrariesVerilog.Items.Clear();
            foreach (string verilogLibrary in options.VerilogLibrariesPaths)
            {
                ListViewItem item = new ListViewItem(verilogLibrary);
                listViewLibrariesVerilog.Items.Add(item);
            }
        }

        /// <summary>
        /// загрузка данных из обьекта класса FSMOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(FSMOptions options)
        {
            graphicsOptionsUserControlFSM.SetOptionsData(options);
        }

        /// <summary>
        /// загрузка данных из обьекта класса EntityDrawningOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(EntityDrawningOptions options)
        {
            graphicsOptionsUserControlEntityDrawning.SetOptionsData(options);
        }

        /// <summary>
        /// получение данных в виде обьекта класса ProjectOptions
        /// </summary>
        /// <returns></returns>
        public void GetOptionsData(ProjectOptions options)
        {
            options.DefaultProjectLocation = textBoxDefaultProjectLocation.Text;
        }

        /// <summary>
        /// загрузка данных из обьекта класса GHDLOptions
        /// </summary>
        /// <param name="options"></param>
        public void SetOptionsData(GHDLOptions options)
        {
            radioButtonGHDLEnvirPath.Checked = options.IsEnvirPathUsed;
            radioButtonGHDLSpecifiedPath.Checked = !options.IsEnvirPathUsed;
            textBoxGHDLPathEnvir.Text = GHDLOptions.GetGHDLEnvirPath();
            textBoxGHDLPathSpecified.Text =  options.GHDL_BIN_Path;
            richTextBoxGHDLCheckSyntaxCommand.Text = options.SyntaxAnalyseCommandExpression;
            richTextBoxGHDLCompileCommand.Text = options.CompileCommandExpression;
            richTextBoxGHDLElaborateCommand.Text = options.ElaborationCommandExpression;
            richTextBoxGHDLSimulateCommand.Text = options.SimulationCommandExpression;
            richTextBoxGHDLCleanCommand.Text = options.CleanCommandExpression;
        }

        /// <summary>
        /// получение данных из обьекта класса GHDLOptions
        /// </summary>
        /// <param name="options"></param>
        public void GetOptionsData(GHDLOptions options)
        {
            options.IsEnvirPathUsed = radioButtonGHDLEnvirPath.Checked == true ;
            options.GHDL_BIN_Path = textBoxGHDLPathSpecified.Text;
            options.SyntaxAnalyseCommandExpression = richTextBoxGHDLCheckSyntaxCommand.Text;
            options.CompileCommandExpression = richTextBoxGHDLCompileCommand.Text;
            options.ElaborationCommandExpression = richTextBoxGHDLElaborateCommand.Text;
            options.SimulationCommandExpression = richTextBoxGHDLSimulateCommand.Text;
            options.CleanCommandExpression = richTextBoxGHDLCleanCommand.Text;
        }

        

        /// <summary>
        /// получение данных в виде обьекта класса TextEditorOptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetOptionsData(TextEditorOptions options)
        {
            options.FontName = comboBoxTextEditorSystemFonts.SelectedItem as string;
            string fontSize = comboBoxTextEditorFontSize.Text;
            if (string.IsNullOrEmpty(fontSize) == true)
                fontSize = (string)comboBoxTextEditorFontSize.SelectedItem;
            float ffontSize = 1.0f;
            bool res = float.TryParse(fontSize, out ffontSize);
            if (res == true)
                options.FontSize = ffontSize;
            options.FontSizeUnits = (GraphicsUnit)comboBoxTextEditorFontSizeUnits.SelectedItem;
            options.FontBold = checkBoxTextEditorBold.Checked;
            options.FontItalic = checkBoxTextEditorItalic.Checked;
            options.FontStrikeout = checkBoxTextEditorStrikeout.Checked;
            options.FontUnderLine = checkBoxTextEditorUnderLine.Checked;
        }
                
        /// <summary>
        /// получение данных в виде обьекта класса LibrariesOptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetOptionsData(LibrariesOptions options)
        {
            List<string> VHDLLibraries = new List<string>();
            foreach (ListViewItem item in listViewLibrariesVHDL.Items)
            {
                VHDLLibraries.Add(item.Text);
            }
            List<string> VerilogLibraries = new List<string>();
            foreach (ListViewItem item in listViewLibrariesVerilog.Items)
            {
                VerilogLibraries.Add(item.Text);
            }
            options.VerilogLibrariesPaths = VerilogLibraries;
            options.VHDLLibrariesPaths = VHDLLibraries;
        }

        /// <summary>
        /// получение данных в виде обьекта класса FSMOptions
        /// </summary>
        /// <param name="options"></param>
        public void GetOptionsData(FSMOptions options)
        {
            graphicsOptionsUserControlFSM.GetOptionsData(options);
        }

        /// <summary>
        /// получение данных в виде обьекта класса EntityDrawningOptions
        /// </summary>
        /// <param name="options"></param>
        public void GetOptionsData(EntityDrawningOptions options)
        {
            graphicsOptionsUserControlEntityDrawning.GetOptionsData(options);
        }
        #endregion

        private void buttonDefaultProjectLocation_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialogDefaultProjectLocation.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBoxDefaultProjectLocation.Text = folderBrowserDialogDefaultProjectLocation.SelectedPath;
            }
        }

        private void TextEditorOptionsFontChanged(object sender, EventArgs e)
        {
            _texEditorOptions.FontName = comboBoxTextEditorSystemFonts.SelectedItem as string;
            string fontSize = comboBoxTextEditorFontSize.Text;
            if (string.IsNullOrEmpty(fontSize) == true)
                fontSize = (string)comboBoxTextEditorFontSize.SelectedItem;
            float ffontSize = 1.0f;
            bool res = float.TryParse(fontSize, out ffontSize);
            if (res == true)
                _texEditorOptions.FontSize = ffontSize;
            else
                _texEditorOptions.FontSize = 14.0f;
            if (comboBoxTextEditorFontSizeUnits.SelectedItem != null)
                _texEditorOptions.FontSizeUnits = (GraphicsUnit)comboBoxTextEditorFontSizeUnits.SelectedItem;
            _texEditorOptions.FontBold = checkBoxTextEditorBold.Checked;
            _texEditorOptions.FontItalic = checkBoxTextEditorItalic.Checked;
            _texEditorOptions.FontStrikeout = checkBoxTextEditorStrikeout.Checked;
            _texEditorOptions.FontUnderLine = checkBoxTextEditorUnderLine.Checked;
            labelTextEditorPreview.Font = _texEditorOptions.Font;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            conf.SaveChanges(this);
            Close();
        }

        private void buttonSetDefaultSettings_Click(object sender, EventArgs e)
        {
            conf.SetDefaultConfiguration();
            conf.LoadDataToForm(this);
        }

        private void listViewLibrariesVHDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteVHDLLibrary.Enabled = listViewLibrariesVHDL.SelectedItems.Count != 0;
        }

        private void listViewLibrariesVerilog_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteVerilogLibrary.Enabled = listViewLibrariesVerilog.SelectedItems.Count != 0;
        }

        private void buttonDeleteVHDLLibrary_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewLibrariesVHDL.SelectedItems)
                listViewLibrariesVHDL.Items.Remove(item);
        }

        private void buttonDeleteVerilogLibrary_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewLibrariesVerilog.SelectedItems)
                listViewLibrariesVerilog.Items.Remove(item);
        }

        private void buttonAddVHDLLibrary_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialogLibraries.ShowDialog();
            if (res == DialogResult.OK)
            {
                listViewLibrariesVHDL.Items.Add(new ListViewItem(folderBrowserDialogLibraries.SelectedPath));
            }
        }

        private void buttonAddVerilogLibrary_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialogLibraries.ShowDialog();
            if (res == DialogResult.OK)
            {
                listViewLibrariesVerilog.Items.Add(new ListViewItem(folderBrowserDialogLibraries.SelectedPath));
            }
        }

        private void buttonGHDLPathBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = conf.GHDLOptions.GHDL_BIN_Path;
            fbd.ShowNewFolderButton = false;
            fbd.Description = "Select folder with GHDL file";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxGHDLPathSpecified.Text = fbd.SelectedPath;
                radioButtonGHDLSpecifiedPath.Checked = true;
            }
        }            
    }
}