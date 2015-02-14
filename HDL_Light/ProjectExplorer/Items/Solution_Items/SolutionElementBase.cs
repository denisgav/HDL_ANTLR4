using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Элемент решения
    /// </summary>
    [Serializable]
    public abstract class SolutionElementBase : ProjectElementBase, IEnumerable<ProjectElementBase>
    {
        /// <summary>
        /// Коструктор для элемента, чье имя не совадает с именем файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caption"></param>
        /// <param name="parent"></param>
        public SolutionElementBase(string path, string caption, ProjectElementBase parent)
            :base (path, caption, parent)
        {
        }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public SolutionElementBase(string path, ProjectElementBase parent)
            : base(path, parent)
        {
        }

        /// <summary>
        /// Перечисляем все вложенные компоненты
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<ProjectElementBase> GetEnumerator();

        /// <summary>
        /// Перечисляем все вложенные компоненты
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
