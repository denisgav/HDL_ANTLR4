using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, хранящий информацию о файле с временной диаграммой
    /// </summary>
    [Serializable]
    public class Waveform_File : ProjectElement
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public Waveform_File(string path, ProjectElement parent)
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
            get { return "/HDL_Light;component/Images/Files/WaveformFile.png"; }
        }
        /// <summary>
        /// Сохранение на диск
        /// </summary>
        public override void Save()
        {
            /*
            if (System.IO.File.Exists(path) == false)
            {
                var writer = System.IO.File.CreateText(path);
                writer.Close();
            }
            */
        }

        public override Windows.SchematixBaseWindow CreateNewWindow()
        {
            return new Schematix.Windows.Waveform.Waveform(this);
        }
    }
}
