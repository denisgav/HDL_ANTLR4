using System;
using System.Collections.Generic;
using System.Text;

namespace StartPage
{
	public class ProjectList
	{
        private static string defaultProjectExtention = ".sol";
        public static string DefaultProjectExpention
        {
            get { return defaultProjectExtention; }
            set { defaultProjectExtention = value; }
        }
        
		private List<String> Files;
		private string Path = @"C:\Projects\";
		public ProjectList( String Path)
		{
			this.Path = Path;
			this.Files = new List<String>();
		}
		public ProjectList()
		{
			this.Files = new List<String>();
		}
		
		public List<String> GetProjects()
		{
			AddFolder(Path);
			return Files;
		}
		
		private void AddFolder(string path)
        {
            try
            {
                string[] dirs = System.IO.Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    AddFolder(dir);
                }
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (isProjectFile(file))
                        Files.Add(file);
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
        }

        public static bool isProjectFile(string path)
        {
            try
            {
                System.IO.FileInfo info = new System.IO.FileInfo(path);
                if (info.Name.EndsWith(defaultProjectExtention))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
	}
}