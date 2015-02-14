using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс для работы с файловыми операциями в проекте
    /// </summary>
    public static class FileOperations
    {
        /// <summary>
        /// Выполнить копирование папки
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            if (sourceDirName.Equals(destDirName))
                return;

            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Скопировать содержимое папки с ее содержимым в новый каталог
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newParent"></param>
        public static void DirectoryCopy(ProjectFolder source, ProjectFolder newParent)
        {
            //Определяем новое имя каталога
            string oldDirPath = source.Path;
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(oldDirPath);
            string dirName = info.Name;
            string newDirPath = System.IO.Path.Combine(newParent.Path, dirName);

            //создаем новую папку
            if (!Directory.Exists(newDirPath))
            {
                Directory.CreateDirectory(newDirPath);
            }

            //изменяем путь к папке
            source.Path = newDirPath;

            //копируем все содержимое в новую папку
            foreach (ProjectElement el in source.SubElements)
            {
                if (el is ProjectFolder)
                {
                    DirectoryCopy(el as ProjectFolder, source);
                }
                else
                {
                    FileCopy(el, source);
                }
            }
            newParent.AddElement(source);
        }

        /// <summary>
        /// Скопировать содержимое папки с ее содержимым в новый каталог
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newParent"></param>
        public static void DirectoryMove(ProjectFolder source, ProjectFolder newParent)
        {
            //Определяем новое имя каталога
            string oldDirPath = source.Path;
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(oldDirPath);
            string dirName = info.Name;
            string newDirPath = System.IO.Path.Combine(newParent.Path, dirName);

            //создаем новую папку
            if (!Directory.Exists(newDirPath))
            {
                Directory.CreateDirectory(newDirPath);
            }

            //изменяем путь к папке
            source.Path = newDirPath;

            //копируем все содержимое в новую папку
            foreach (ProjectElement el in source.SubElements)
            {
                if (el is ProjectFolder)
                {
                    DirectoryMove(el as ProjectFolder, source);
                }
                else
                {
                    FileMove(el, source);
                }
            }

            //Удаляем папку, если она пуста
            DirectoryInfo dir = new DirectoryInfo(oldDirPath);
            if ((dir.GetFiles().Length != 0) || (dir.GetDirectories().Length != 0))
            {
                dir.Delete(true);
            }
            else
                dir.Delete();

            newParent.AddElement(source);
        }

        /// <summary>
        /// Скопировать файл в папку
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newParent"></param>
        public static void FileCopy(ProjectElement source, ProjectFolder newParent)
        {
            if ((source is Project) || (source is ProjectFolder)) //Что-то нехорошее произошло
            {
                throw new Exception("Incorrect argument type");
            }

            //Определяем новое имя файла
            string fileName = System.IO.Path.GetFileName(source.Path);
            string newFilePath = System.IO.Path.Combine(newParent.Path, fileName);

            //копируем файл
            if(System.IO.File.Exists(newFilePath))
            {
                System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(string.Format("File {0} already extsts. Overwrite it?", newFilePath), "File copy warning", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                if (res == System.Windows.MessageBoxResult.Yes)
                    System.IO.File.Copy(source.Path, newFilePath, true);
            }
            else
                System.IO.File.Copy(source.Path, newFilePath);

            //Меняем поле Path файла
            source.Path = newFilePath;
            newParent.AddElement(source);
            //Все
        }

        /// <summary>
        /// Переместить файл в папку
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newParent"></param>
        public static void FileMove(ProjectElement source, ProjectFolder newParent)
        {
            if ((source is Project) || (source is ProjectFolder)) //Что-то нехорошее произошло
            {
                throw new Exception("Incorrect argument type");
            }

            //Устанавливаем нового родителя
            source.Parent = newParent;

            //Определяем новое имя файла
            string fileName = System.IO.Path.GetFileName(source.Path);
            string newFilePath = System.IO.Path.Combine(newParent.Path, fileName);

            //копируем файл
            if (System.IO.File.Exists(newFilePath))
            {
                System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(string.Format("File {0} already extsts. Overwrite it?", newFilePath), "File copy warning", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                if (res == System.Windows.MessageBoxResult.Yes)
                {
                    System.IO.File.Delete(newFilePath);
                    System.IO.File.Move(source.Path, newFilePath);
                }
            }
            else
                System.IO.File.Move(source.Path, newFilePath);

            //Меняем поле Path файла
            source.Path = newFilePath;
            newParent.AddElement(source);
            //Все
        }
    }
}
