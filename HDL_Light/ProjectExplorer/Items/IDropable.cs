using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Интерфейс для Drag/Drop функций
    /// </summary>
    public interface IDropable
    {
        /// <summary>
        /// Можно ли вставить данные
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool CanDrop(ClipboardBufferData data);

        /// <summary>
        /// Вставить данные
        /// </summary>
        /// <param name="data"></param>
        void Drop(ClipboardBufferData data);
    }
}
