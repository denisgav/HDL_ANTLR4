using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, хранящий информацию о файле с VHDL кодом
    /// </summary>
    [Serializable]
    public class Text_File : ProjectElement
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public Text_File(string path, ProjectElement parent)
            : base(path, parent)
        {
            parentProjectElement = parent;
        }

        /// <summary>
        /// Родительский элемент
        /// </summary>
        private ProjectElement parentProjectElement;

        /// <summary>
        /// Контекстное меню
        /// </summary>
        public override ContextMenu CreateElementContextMenu(ProjectExplorerControl control)
        {
            return CreateContextMenu(this, control);
        }

        /// <summary>
        /// Иконка
        /// </summary>
        public override string Icon
        {
            get { return "/HDL_Light;component/Images/File/TextFile.ico"; }
        }

        /// <summary>
        /// Создание файла с текстом
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="projectFolder"></param>
        public static Text_File CreateFile(string filename, string text, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, filename);
            Text_File res = new Text_File(path, projectFolder);
            projectFolder.AddElement(res);

            var writer = System.IO.File.CreateText(path);
            writer.Write(text);
            writer.Close();

            projectFolder.Save();

            return res;
        }

        /// <summary>
        /// Сохранение на диск
        /// </summary>
        public override void Save()
        {
            if (System.IO.File.Exists(path) == false)
            {
                var writer = System.IO.File.CreateText(path);
                writer.Close();
            }
        }

        public override Windows.SchematixBaseWindow CreateNewWindow()
        {
            return new Schematix.Windows.Code.Code(this);
        }
    }
}
