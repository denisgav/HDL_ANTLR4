using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Schematix.FSM;
using Schematix.ProjectExplorer;


namespace Schematix.Dialogs.NewFileDialogWizard
{
    public partial class AddNewFile : Form
    {
        private Schematix.SchematixCore core;
        private ProjectFolder projectFolder;

        /// <summary>
        /// Созданный новый файл
        /// </summary>
        private ProjectElement createdElement;
        public ProjectElement CreatedElement
        {
            get { return createdElement; }            
        }

        private bool openFileAfterCreation;
        public bool OpenFileAfterCreation
        {
            get { return openFileAfterCreation; }
            set { openFileAfterCreation = value; }
        }
        
        
        public AddNewFile(Schematix.SchematixCore core, ProjectFolder projectFolder, bool OpenFileAfterCreation = true)
        {
            this.openFileAfterCreation = OpenFileAfterCreation;
            this.projectFolder = projectFolder;
            this.core = core;
            InitializeComponent();
        }

        public AddNewFile(ProjectFolder projectFolder, bool OpenFileAfterCreation = true)
            : this(Schematix.SchematixCore.Core, projectFolder, OpenFileAfterCreation)
        {
            this.openFileAfterCreation = OpenFileAfterCreation;
        }

        private void listViewWizardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEmptyFileList.SelectedItems.Count == 0)
                textBoxEmptyFileName.Enabled = false;
            else
                textBoxEmptyFileName.Enabled = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (AcceptData())
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(GetErrorMessage(textBoxEmptyFileName.Text), "File Name Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool CheckCanCreateFile(string fileName)
        {
            //1. Пустое имя
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                return false;

            //2. Исп. неразрешенных символов
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in fileName)
                if (invalidChars.Contains(c))
                    return false;
            //3. Файл уже существует
            string fullPath = System.IO.Path.Combine(projectFolder.Path, fileName + GetExtention());
            if (System.IO.File.Exists(fullPath))
                return false;

            return true;
        }        

        private string GetErrorMessage(string fileName)
        {
            //1. Пустое имя
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                return "Name is empty";

            //2. Исп. неразрешенных символов
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in fileName)
                if (invalidChars.Contains(c))
                    return string.Format("Using of symbol '{0}' in file name is not alloved", c);

            //3. Файл уже существует
            string fullPath = System.IO.Path.Combine(projectFolder.Path, fileName + GetExtention());
            if (System.IO.File.Exists(fullPath))
                return string.Format("File {0} already exists", fullPath);

            return string.Empty;
        }

        private bool AcceptData()
        {
            if ((tabControl1.SelectedTab == tabPage1))
            {
                if (listViewEmptyFileList.SelectedIndices.Count == 0)
                    return false;
                if (CheckCanCreateFile(textBoxEmptyFileName.Text) == false)
                    return false;
                
                //создание пустого файла
                switch (listViewEmptyFileList.SelectedIndices[0])
                {
                    case 0:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyVHDLCodeFile(textBoxEmptyFileName.Text, core, projectFolder);
                        break;
                    case 1:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyVerilogCodeFile(textBoxEmptyFileName.Text, core, projectFolder);
                        break;
                    case 2:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyFSM(textBoxEmptyFileName.Text, Schematix.FSM.FSM_Language.VHDL, core, projectFolder);
                        break;
                    case 3:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyFSM(textBoxEmptyFileName.Text, Schematix.FSM.FSM_Language.VHDL, core, projectFolder);
                        break;
                    case 4:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyScheme(textBoxEmptyFileName.Text, core, projectFolder);
                        break;
                    case 5:
                        createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateEmptyEDRFile(textBoxEmptyFileName.Text, core, projectFolder);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (listViewWizardList.SelectedIndices.Count == 0)
                    return false;
                //создание файла при помощи визарда
                switch (listViewWizardList.SelectedIndices[0])
                {
                    case 0:
                        {
                            Schematix.Dialogs.NewFileDialogWizard.AddNewVHDL wizard = new Schematix.Dialogs.NewFileDialogWizard.AddNewVHDL(SchematixCore.Core);
                            this.Visible = false;
                            DialogResult res = wizard.ShowDialog();
                            if (res == DialogResult.OK)
                            {
                                VHDL_VizardCodeGenerator generator = new VHDL_VizardCodeGenerator(wizard.VHDLModule);
                                string text = generator.Generate();

                                createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateWizardVHDLCodeFile(wizard.FileName.Text, SchematixCore.Core, text, core, projectFolder);
                            }
                        }
                        break;
                    case 1:
                        {
                            Schematix.Dialogs.NewFileDialogWizard.AddNewVerilog wizard = new Schematix.Dialogs.NewFileDialogWizard.AddNewVerilog(SchematixCore.Core);
                            this.Visible = false;
                            DialogResult res = wizard.ShowDialog();
                            if (res == DialogResult.OK)
                            {
                                Verilog_VizardCodeGenerator generator = new Verilog_VizardCodeGenerator(wizard.VerilogModule);
                                string text = generator.Generate();

                                createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateWizardVerilogCodeFile(wizard.FileName.Text, text, core, projectFolder);

                            }
                        }
                        break;
                    case 2:
                        {
                            Schematix.Dialogs.NewFileDialogWizard.AddNewVHDL wizard = new Schematix.Dialogs.NewFileDialogWizard.AddNewVHDL(core);
                            this.Visible = false;
                            DialogResult res1 = wizard.ShowDialog();
                            if (res1 == DialogResult.OK)
                            {
                                FSM_Options options = new FSM_Options(wizard.VHDLModule);
                                DialogResult res2 = options.ShowDialog();
                                if (res2 == DialogResult.OK)
                                {
                                    createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateWizardFSM(wizard.FileName.Text, wizard.VHDLModule, options.Options, core, projectFolder);
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            Schematix.Dialogs.NewFileDialogWizard.AddNewVerilog wizard = new Schematix.Dialogs.NewFileDialogWizard.AddNewVerilog(core);
                            this.Visible = false;
                            DialogResult res1 = wizard.ShowDialog();
                            if (res1 == DialogResult.OK)
                            {
                                FSM_Options options = new FSM_Options(wizard.VerilogModule);
                                DialogResult res2 = options.ShowDialog();
                                if (res2 == DialogResult.OK)
                                {
                                    createdElement = Schematix.Dialogs.NewFileDialogWizard.CreateFileHelper.CreateWizardFSM(wizard.FileName.Text, SchematixCore.Core, wizard.VerilogModule, options.Options, core, projectFolder);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                
            }
            if ((openFileAfterCreation == true) && (createdElement != null))
            {
                core.OpenNewWindow(createdElement);
            }
            return (createdElement != null);
        }

        private string GetExtention()
        {
            if ((tabControl1.SelectedTab == tabPage1))
            {
                switch (listViewEmptyFileList.SelectedIndices[0])
                {
                    case 0:
                        return ".vhdl";
                        break;
                    case 1:
                        return ".v";
                        break;
                    case 2:
                        return ".fsm";
                        break;
                    case 3:
                        return ".fsm";
                        break;
                    case 4:
                        return ".csx";
                        break;
                    case 5:
                        return ".edr";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (listViewWizardList.SelectedIndices.Count == 0)
                    return string.Empty;
                //создание файла при помощи визарда
                switch (listViewWizardList.SelectedIndices[0])
                {
                    case 0:
                        return ".vhdl";
                        break;
                    case 1:
                        return ".v";
                        break;
                    case 2:
                        return ".fsm";
                        break;
                    case 3:
                        return ".fsm";
                        break;
                    default:
                        break;
                }
            }
            return string.Empty;
        }
    }
}
