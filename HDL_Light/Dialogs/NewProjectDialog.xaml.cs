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
using Schematix.ProjectExplorer;
using System.Windows.Forms;

namespace Schematix.Dialogs
{
    /// <summary>
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        private Solution solution;
        public Solution Solution
        {
            get { return solution; }
        }

        private SolutionFolder parent;
        public SolutionFolder Parent
        {
            get { return parent; }
        }
        

        private FolderBrowserDialog folderDialog;

        public NewProjectDialog()
        {
            folderDialog = new FolderBrowserDialog();
            InitializeComponent();
            FillDefaultData();
        }

        public NewProjectDialog(SolutionFolder parent, Solution solution)
        {
            this.parent = parent;
            folderDialog = new FolderBrowserDialog();
            this.solution = solution;
            InitializeComponent();            
        }

        private void FillDefaultData()
        {
            TextBoxProjectName.Text = "Project1";
            TextBoxSolutionName.Text = "Solution1";
            TextBoxProjectLocation.Text = Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation;
            ComboBoxSolutionTypeSelection.SelectedItem = ComboBoxItemNewSolution;
            ComboBoxSolutionTypeSelection.Items.Remove(ComboBoxItemAddToSolution);
            folderDialog.SelectedPath = Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation;
        }

        private void FillCustomData()
        {
            TextBoxProjectName.Text = "Project1";
            TextBoxSolutionName.Text = solution.Caption;
            TextBoxProjectLocation.Text = parent.Path;
            ComboBoxSolutionTypeSelection.SelectedItem = ComboBoxItemAddToSolution;
            folderDialog.SelectedPath = parent.Path;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (ApplyChanges() == true)
            {
                DialogResult = true;
                Close();
            }
            else
                e.Handled = true;
        }

        /// <summary>
        /// Применение изменений
        /// </summary>
        private bool ApplyChanges()
        {
            string Path = TextBoxProjectLocation.Text;
            string projName = TextBoxProjectName.Text;
            string solName = TextBoxSolutionName.Text;
            if ((solution != null) && (ComboBoxSolutionTypeSelection.SelectedItem == ComboBoxItemAddToSolution))
            {
                string msg = Project.CanCreateProject(Path, parent.Path);
                if (msg != null)
                {
                    System.Windows.MessageBox.Show(msg, "Error");
                    return false;
                }
                Project proj = Project.CreateNewProject(projName, parent);
                parent.AddElement(proj);
                solution.Save();
                SchematixCore.Core.UpdateExplorerPanel();
            }
            if (ComboBoxSolutionTypeSelection.SelectedItem == ComboBoxItemNewSolution)
            {
                string msg = Solution.CanCreateSolution(Path, solName);
                if (msg != null)
                {
                    System.Windows.MessageBox.Show(msg, "Error");
                    return false;
                }
                solution = Solution.CreateNewSolution(solName, Path);
                Project proj = Project.CreateNewProject(projName, solution.RootFolder);
                solution.AddElement(proj);
                solution.Save();
                SchematixCore.Core.UpdateExplorerPanel();
            }
            return true;
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                TextBoxProjectLocation.Text = folderDialog.SelectedPath;
        }

        private void ComboBoxSolutionTypeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded == true)
                TextBoxSolutionName.IsEnabled = (ComboBoxSolutionTypeSelection.SelectedIndex == 0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (solution == null)
                FillDefaultData();
            else
                FillCustomData();
        }
    }
}
