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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace StartPage
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class StartPageUserControl : UserControl
    {
		List<String> Files;
        public RecentProjectList recentProjects;
        public StartPageUserControl()
        {
            InitializeComponent();
        }

        public delegate void OpenProjectDelegate(object sender);
        public delegate void OpenRecentProjectDelegate(object sender, string FilePath);
        private event OpenRecentProjectDelegate OpenRecentProjectClick;
        private OpenProjectDelegate OpenProjectClick;

        public event OpenRecentProjectDelegate OpenRecentProject
        {
            add
            {
                OpenRecentProjectClick += value;
            }
            remove
            {
                OpenRecentProjectClick -= value;
            }

        }


        public event OpenProjectDelegate OpenProject
        {
            add
            {
                OpenProjectClick += value;
            }
            remove
            {
                OpenProjectClick -= value;
            }

        }

        public delegate void CreateProjectDelegate(object sender);
        private CreateProjectDelegate CreateProjectClick;
		public event CreateProjectDelegate CreateProject
        {
            add
            {
                CreateProjectClick += value;
            }
            remove
            {
                CreateProjectClick -= value;
            }

        }
        


        private void ButtonOpenProject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			OpenProjectClick(sender);
        }

        private void ButtonNewProject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			CreateProjectClick(sender);
        }


        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ProjectData data = button.Tag as ProjectData;
                if(data != null)
                    OpenRecentProjectClick(sender, data.FilePath);
            }
        }	

        private void Url1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenURL("http://localhost/hdl-light/");
        }
		
		private void Url2_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenURL("http://localhost/hdl-light/forums/forum/first-release-of-the-programm/");
        }

        private void OpenURL(string site)
        {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = site;
                myProcess.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
		
		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			LoadProjectList();
            LoadRecentProjectList();
            CreateProject += new CreateProjectDelegate(StartPageUserControl_CreateProject);
            OpenProject += new OpenProjectDelegate(StartPageUserControl_OpenProject);
            OpenRecentProject += new OpenRecentProjectDelegate(StartPageUserControl_OpenRecentProject);

            LRMWebBrowser.Navigate(new Uri(@"file:///" + AppDomain.CurrentDomain.BaseDirectory + @"LRM/1076_axa.html"));
		}

        void StartPageUserControl_OpenRecentProject(object sender, string e)
        {
            recentProjects.SaveRecentProjects();
            RefreshProjectData();
        }

        void StartPageUserControl_OpenProject(object sender)
        {
            recentProjects.SaveRecentProjects();
            RefreshProjectData();
        }

        void StartPageUserControl_CreateProject(object sender)
        {
            recentProjects.SaveRecentProjects();
            RefreshProjectData();
        }

        public void RefreshProjectData()
        {
            LoadProjectList();
            LoadRecentProjectList();
        }


        private Button FormButton(ProjectData data)
        {
            Button button = new Button();
            WrapPanel wrapPanel = new WrapPanel();
            Image image = new Image();
            TextBlock text = new TextBlock();

            text.Text = System.IO.Path.GetFileName(data.FileName);
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("Resources/SmallIcon.ico", UriKind.Relative);
            bi3.EndInit();
            image.Stretch = System.Windows.Media.Stretch.Fill;
            image.Source = bi3;
            image.Width = 16;
            image.Height = 16;
            image.Margin = new System.Windows.Thickness(0, 0, 5, 0);

            wrapPanel.Orientation = Orientation.Horizontal;
            wrapPanel.Children.Add(image);
            wrapPanel.Children.Add(text);
            wrapPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            wrapPanel.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            button.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;

            button.Tag = data;
            button.Content = wrapPanel;
            button.Click += new System.Windows.RoutedEventHandler(button_Click);

            return button;
        }

        private void LoadRecentProjectList()
        {
            recentProjects = new RecentProjectList(System.IO.Path.Combine(Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation, "RecentProjects.xml"));
            recentProjects.LoadRecentProjects();
            RecentProjectsList.Children.Clear();

            try
            {
                foreach (ProjectData data in recentProjects.Projects)
                {
                    Button button = FormButton(data);
                    RecentProjectsList.Children.Add(button);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Some Error :(", MessageBoxButton.OK, MessageBoxImage.Error);
                Schematix.Core.Logger.Log.Error("Load Recent Projects Exception during processing data.", ex);
            }
        }

		
		private void LoadProjectList()
		{
            Files = new ProjectList(Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation).GetProjects();
			ProjectsList.Children.Clear();
			
			try
			{
			
				foreach(String file in Files)
				{
                    ProjectData data = new ProjectData(file);
                    Button button = FormButton(data);
                    ProjectsList.Children.Add(button);
				}
			}
			catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Some Error :(", MessageBoxButton.OK, MessageBoxImage.Error);
                Schematix.Core.Logger.Log.Error("Load Recent Projects Exception during load recent projects.", ex);
            }
		}

        public void AddNewRecentProject(string filePath)
        {
            AddNewRecentProject(new ProjectData(filePath));
        }

        public void AddNewRecentProject(ProjectData data)
        {
            for (int i = 0; i < recentProjects.Projects.Count; i++)
            {
                StartPage.ProjectData d = recentProjects.Projects[i];
                if (d.FilePath.Equals(data.FilePath))
                {
                    recentProjects.Projects.Remove(d);
                    break;
                }
            }
            recentProjects.Projects.Insert(0, data);
            recentProjects.SaveRecentProjects();
        }
    }
}
