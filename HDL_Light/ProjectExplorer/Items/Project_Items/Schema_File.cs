using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, хранящий информацию о схеме
    /// </summary>
    [Serializable]
    public class Schema_File : ProjectElement
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public Schema_File(string path, ProjectElement parent)
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
            get { return "/HDL_Light;component/Images/Files/SchemeFile.png"; }
        }

        /// <summary>
        /// Сохранение на диск
        /// </summary>
        public override void Save()
        {
            //throw new NotImplementedException();
        }

        public override Windows.SchematixBaseWindow CreateNewWindow()
        {
            return new Schematix.Windows.Schema.Schema(this);
        }
    }
}
