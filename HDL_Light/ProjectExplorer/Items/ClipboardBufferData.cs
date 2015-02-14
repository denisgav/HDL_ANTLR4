using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, используемый для операций с буфером обмена
    /// </summary>
    [Serializable]
    public class ClipboardBufferData
    {
        /// <summary>
        /// Тип операции
        /// </summary>
        private ClipboardOperationType opType;
        public ClipboardOperationType OperationType
        {
            get { return opType; }
            set { opType = value; }
        }

        private GroupType groupType;
        public GroupType GroupType
        {
            get { return groupType; }
        }
        

        /// <summary>
        /// Элементы, над которыми производится операция
        /// </summary>
        private List<ProjectElementBase> elements;
        public List<ProjectElementBase> Elements
        {
            get { return elements; }
            set { elements = value; }
        }

        public ClipboardBufferData(ClipboardOperationType opType, params ProjectElementBase[] elements)
            : this(CheckGroupValid(elements), opType, new List<ProjectElementBase>(elements))
        {            
        }

        public ClipboardBufferData(ClipboardOperationType opType, IList<ProjectElementBase> elements)
            : this(CheckGroupValid(elements), opType, new List<ProjectElementBase>(elements))
        {            
        }

        public ClipboardBufferData(GroupType groupType, ClipboardOperationType opType, params ProjectElementBase[] elements)
            : this(groupType, opType, new List<ProjectElementBase>(elements))
        {            
        }

        public ClipboardBufferData(GroupType groupType, ClipboardOperationType opType, IList<ProjectElementBase> elements)
        {
            this.groupType = groupType;
            this.opType = opType;
            this.elements = new List<ProjectElementBase>(elements);
        }

        /// <summary>
        /// Можно ли скопировать группу элементов
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static GroupType CheckGroupValid(params ProjectElementBase[] elements)
        {
            return CheckGroupValid(elements);
        }

        /// <summary>
        /// Можно ли скопировать группу элементов
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static GroupType CheckGroupValid(IList<ProjectElementBase> elements)
        {
            //1. Все элементы - проэкты или проектные папки
            bool res1 = true;
            foreach (ProjectElementBase el in elements)
            {
                if ((el is Project == false) && (el is SolutionFolder == false))
                {
                    res1 = false;
                    break;
                }
            }
            if (res1 == true)
                return GroupType.ProjectsAndSolutionsFolder;

            //2. Все элементы - подэлементы проекта (не включая самого проекта)
            bool res2 = true;
            foreach (ProjectElementBase el in elements)
            {
                if ((el is Project == true) || (el is ProjectElement == false))
                {
                    res2 = false;
                    break;
                }
            }
            if (res2 == true)
                return GroupType.ProjectElements;

            return GroupType.IllegalGroup;
        }



        #region Clipboard functions
        private static DataFormats.Format clipboard_format = DataFormats.GetFormat(typeof(ClipboardBufferData).FullName);

        /// <summary>
        /// Возможно ли отправить данные в буфер обмена
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CanSendToClipboard(ClipboardBufferData data)
        {
            return IsSerializable(data);
        }

        /// <summary>
        /// Можно ли получить данные с буфера обмена
        /// </summary>
        /// <returns></returns>
        public static bool CanGetFromClipboard()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            bool res = dataObj.GetDataPresent(clipboard_format.Name);

            if (res == true)
            {
                ClipboardBufferData data = dataObj.GetData(clipboard_format.Name) as ClipboardBufferData;
                dataObj.SetData(clipboard_format.Name, data);
            }
            return res;
        }        

        /// <summary>
        /// Отправка данных в буфер обмена
        /// </summary>
        /// <param name="data"></param>
        public static void SendToClipboard(ClipboardBufferData data)
        {
            // копируем в буфер обмена
            System.Windows.Forms.IDataObject dataObj = new DataObject();
            dataObj.SetData(clipboard_format.Name, false, data);
            Clipboard.SetDataObject(dataObj, false);
        }

        /// <summary>
        /// Получение данных с буфера обмена
        /// </summary>
        /// <returns></returns>
        public static ClipboardBufferData GetFromClipboard()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            ClipboardBufferData data = null;
            if (dataObj.GetDataPresent(clipboard_format.Name))
            {
                data = dataObj.GetData(clipboard_format.Name) as ClipboardBufferData;                
            }
            return data;
        }


        /// <summary>
        /// Можно ли сериализовать объект
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsSerializable(object obj)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                bin.Serialize(mem, obj);
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Объект не может быть сериализован. \n" + ex.ToString());
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Тип операции с буфером обмена
    /// </summary>
    public enum ClipboardOperationType
    {
        /// <summary>
        /// Вырезать
        /// </summary>
        Cut,
        /// <summary>
        /// Копировать
        /// </summary>
        Copy
    }

    /// <summary>
    /// Тип группы элементов
    /// </summary>
    public enum GroupType
    {
        ProjectsAndSolutionsFolder,
        ProjectElements,
        IllegalGroup
    }
}
