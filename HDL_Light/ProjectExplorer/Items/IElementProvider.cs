using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Управление подэлеметами
    /// </summary>
    public interface IElementProvider
    {
        /// <summary>
        /// Добавление одного элемента
        /// </summary>
        /// <param name="element"></param>
        void AddElement(ProjectElementBase element);

        /// <summary>
        /// Удаление одного дочернего элемента
        /// </summary>
        /// <param name="elem"></param>
        void RemoveElement(ProjectElementBase element);

        /// <summary>
        /// Добавление группы элементов
        /// </summary>
        /// <param name="elements"></param>
        void AddElementRange(IList<ProjectElementBase> elements);

        /// <summary>
        /// Удаление группы элементов
        /// </summary>
        /// <param name="elements"></param>
        void RemoveElementRange(IList<ProjectElementBase> elements);

        /// <summary>
        /// можно ли добавить новый дочерний элемент
        /// </summary>
        /// <param name="element"></param>
        bool CanAddElement(ProjectElementBase element);

        /// <summary>
        /// можно ли добавить новыt дочерний элемент
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        bool CanAddElementrange(IList<ProjectElementBase> elements);
    }
}
