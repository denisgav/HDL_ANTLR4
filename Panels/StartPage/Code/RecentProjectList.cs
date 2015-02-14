using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace StartPage
{
    public class ProjectData
    {
        public string FileName;
        public string FilePath;
        public DateTime Date;

        public ProjectData(string FilePath)
        {
            this.FilePath = FilePath;
            if (System.IO.File.Exists(FilePath) == false)
                throw new Exception("File " + FilePath + " not exists");

            FileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
            Date = System.IO.File.GetLastAccessTime(FilePath);
        }

        public ProjectData(string FileName, string FilePath, DateTime Date)
        {
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.Date = Date;
        }

        public ProjectData(string FileName, string FilePath)
        {
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.Date = DateTime.Now;
        }

        public ProjectData()
        {
            FileName = string.Empty;
            FilePath = string.Empty;
            Date = DateTime.Now;
        }
    }
    public class RecentProjectList
    {
        private List<ProjectData> projects;
        public List<ProjectData> Projects
        {
            get
            {
                return projects;
            }
            set
            {
                projects = value;
            }
        }
        private string SettingsFilePath = "RecentProjects.xml";

        public RecentProjectList()
        {
            projects = new List<ProjectData>();
        }

        public RecentProjectList(string SettingsFilePath)
        {
            this.SettingsFilePath = SettingsFilePath;
            projects = new List<ProjectData>();
        }

        public void LoadRecentProjects()
        {
            try
            {
                projects.Clear();

                XmlDocument _doc = new XmlDocument();
                _doc.Load(SettingsFilePath);

                XmlNodeList nodes = _doc.SelectNodes("/ProjectList/ProjectData");
                foreach (XmlNode node in nodes)
                {
                    ProjectData data = new ProjectData();
                    data.FileName = node.SelectSingleNode("FileName").InnerText;
                    data.FilePath = node.SelectSingleNode("FilePath").InnerText;
                    data.Date = DateTime.Parse(node.SelectSingleNode("Date").InnerText);

                    projects.Add(data);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace, "Fatal Error :(", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Schematix.Core.Logger.Log.Error("Load Recent Projects Exception.", ex);
            }
        }

        public void SaveRecentProjects()
        {
            XmlDocument _doc = new XmlDocument();
            XmlElement rootNode = _doc.CreateElement("ProjectList");
            foreach (ProjectData data in projects)
            {
                XmlElement DataElement = _doc.CreateElement("ProjectData");

                XmlElement ElementFileName = _doc.CreateElement("FileName");
                XmlCDataSection descrfilename = _doc.CreateCDataSection(data.FileName);
                ElementFileName.AppendChild(descrfilename);
                DataElement.AppendChild(ElementFileName);

                XmlElement ElementFilePath = _doc.CreateElement("FilePath");
                XmlCDataSection descrfilepath = _doc.CreateCDataSection(data.FilePath);
                ElementFilePath.AppendChild(descrfilepath);
                DataElement.AppendChild(ElementFilePath);

                XmlElement ElementDate = _doc.CreateElement("Date");
                XmlCDataSection descrdate = _doc.CreateCDataSection(data.Date.ToString());
                ElementDate.AppendChild(descrdate);
                DataElement.AppendChild(ElementDate);

                rootNode.AppendChild(DataElement);
            }
            _doc.AppendChild(rootNode);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.Unicode;
            settings.NewLineOnAttributes = true;

            XmlWriter writer = XmlWriter.Create(SettingsFilePath, settings);
            _doc.Save(writer);
            writer.Close();
        }
    }
}