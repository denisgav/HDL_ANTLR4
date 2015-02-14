using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Schematix.Core;
using System.IO;

namespace Schematix.FSM
{
    public partial class Constructor_Core
    {
        private Schematix.FSM.Constructor_UserControl Form;
        public Schematix.FSM.Constructor_UserControl form
        {
            get
            {
                return Form;
            }
        }

        public bool ShowLinePriority
        {
            get { return paper.ShowLinePriority; }
            set { paper.ShowLinePriority = value; }
        }

        public Color Selected_Color = Color.FromArgb(75, 63, 193, 237);

        /// <summary>
        /// Текущий FSM
        /// </summary>
        private Schematix.FSM.My_Graph graph;
        public Schematix.FSM.My_Graph Graph
        {
            get { return graph; }
            set { graph = value; }
        }


        /// <summary>
        /// История
        /// </summary>
        private Schematix.FSM.Graph_History graph_history;
        public Schematix.FSM.Graph_History Graph_History
        {
            get { return graph_history; }
        }

        /// <summary>
        /// Виртуальное изображение
        /// </summary>
        private My_Bitmap bitmap;
        public My_Bitmap Bitmap
        {
            get { return bitmap; }
        }
        
        /// <summary>
        /// Класс, отвечающий за работу с элементом управления
        /// </summary>
        private My_Paper paper;
        public My_Paper Paper
        {
            get { return paper; }
        }

        /// <summary>
        /// Объект, отвечающий за выбор группы объектов
        /// </summary>
        private Schematix.FSM.GroupSelector group_selector;
        public Schematix.FSM.GroupSelector GroupSelector
        {
            get { return group_selector; }
        }

        /// <summary>
        /// Текущая выбранная фигура
        /// </summary>
        private My_Figure SelectedFigure
        {
            get { return graph.SelectedFigure; }
            set { graph.SelectedFigure = value; }
        }

        /// <summary>
        /// Список выбранных объектов
        /// </summary>
        private IList<My_Figure> SelectedFigureList
        {
            get { return graph.SelectedFigureList; }
        }


        public bool Lock = false;

        public delegate void UpdateHistoryDelegate();
        public event UpdateHistoryDelegate UpdateHistory;

        public Constructor_Core(Schematix.FSM.Constructor_UserControl form)
        {
            this.Form = form;

            bitmap = new My_Bitmap(this);
            paper = new My_Paper(this);
            graph = new Schematix.FSM.My_Graph(this);
            graph_history = new Schematix.FSM.Graph_History(this);
            group_selector = new Schematix.FSM.GroupSelector(this);
        }

        #region Creating new items

        /// <summary>
        /// Создать новое состояние в графе
        /// </summary>
        public void CreateNewState()
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();

            Schematix.FSM.My_State state = new Schematix.FSM.My_State(this);
            SelectedFigure = state;
            graph.AddFigure(state);
            Lock = true;
            form.Invalidate();
        }

        /// <summary>
        /// Создание нового состояния при помощи ToolBox
        /// </summary>
        /// <param name="rect"></param>
        public void CreateNewState_Dragged(Rectangle rect)
        {
            rect.Location = form.PointToClient(rect.Location);
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();

            Schematix.FSM.My_State state = new Schematix.FSM.My_State(this, rect, true);
            graph.AddFigure(state);
            Lock = false;
            //AddToHistory("New state Draged");
            bitmap.UpdateBitmap();
            form.Invalidate();
        }

        /// <summary>
        /// Создание нового комментария
        /// </summary>
        public void CreateNewComment()
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();
            Schematix.FSM.My_Comment comment = new Schematix.FSM.My_Comment(this);
            SelectedFigure = comment;
            graph.AddFigure(comment);
            Lock = true;
        }

        /// <summary>
        /// Создание нового комментария при помощи ToolBox
        /// </summary>
        /// <param name="rect"></param>
        public void CreateNewComment_Dragged(Rectangle rect)
        {
            rect.Location = form.PointToClient(rect.Location);
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();

            Schematix.FSM.My_Comment comment = new Schematix.FSM.My_Comment(this, rect, true);
            SelectedFigure = comment;
            graph.AddFigure(comment);
            Lock = false;
            //AddToHistory("New comment Draged");
            bitmap.UpdateBitmap();
        }

        /// <summary>
        /// Создание нового перехода
        /// </summary>
        public void CreateNewLine()
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (graph.States.Count == 0)
            {
                MessageBox.Show("Нет ни одного состояния", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();

            Schematix.FSM.My_Line line = new Schematix.FSM.My_Line(true, this);
            SelectedFigure = line;
            line.Core = this;
            graph.AddFigure(line);
        }

        /// <summary>
        /// Создание сброса
        /// </summary>
        public void CreateReset()
        {
            if (Lock == true)
            {
                //MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (graph.Reset != null)
            {
                //MessageBox.Show("Reset element alredy exists in graph.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (graph.States.Count == 0)
            {
                //MessageBox.Show("There is no state in the FSM.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            graph.UnselectAllFigures();

            Schematix.FSM.My_Reset reset = new Schematix.FSM.My_Reset(this);
            SelectedFigureList.Add(reset);
            SelectedFigure = reset;
            graph.Reset = reset;
            Lock = true;
        }

        /// <summary>
        /// Создание нового сигнала
        /// </summary>
        public void CreateNewSignal()
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();

            Schematix.FSM.My_Signal signal = new Schematix.FSM.My_Signal(this);
            SelectedFigureList.Add(signal);
            SelectedFigure = signal;
            signal.Core = this;
            graph.AddFigure(signal);
        }

        /// <summary>
        /// Создание нового сигнала используя ToolBox
        /// </summary>
        /// <param name="pt"></param>
        public void CreateNewSignal_Dragged(Point pt)
        {
            pt = form.PointToClient(pt);
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();
            
            Schematix.FSM.My_Signal signal = null;
            if(Graph.Language == FSM_Language.VHDL)
                signal = new Schematix.FSM.My_Signal("Variable" + graph.Signals.Count.ToString(), "STD_LOGIC", "'0'", pt, this);
            if (Graph.Language == FSM_Language.Verilog)
                signal = new Schematix.FSM.My_Signal("Variable" + graph.Signals.Count.ToString(), "reg", "'0'", pt, this);
            SelectedFigure = signal;
            graph.AddFigure(signal);
            Lock = false;
            //AddToHistory("New signal Draged");
            bitmap.UpdateBitmap();
        }

        /// <summary>
        /// Создать новую константу
        /// </summary>
        public void CreateNewConstant()
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();
            
            Schematix.FSM.My_Constant constant = new Schematix.FSM.My_Constant(this);
            SelectedFigureList.Add(constant);
            SelectedFigure = constant;
            constant.Core = this;
            graph.AddFigure(constant);
            Lock = true;
        }

        /// <summary>
        /// Создать новую константу используя ToolBox
        /// </summary>
        /// <param name="pt"></param>
        public void CreateNewConstant_Dragged(Point pt)
        {
            pt = form.PointToClient(pt);
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();

            Schematix.FSM.My_Constant constant = new Schematix.FSM.My_Constant("Constant" + graph.Constants.Count.ToString(), "STD_LOGIC", "'0'", pt, My_Constant.GenerationType.Constant, this);
            SelectedFigure = constant;
            graph.AddFigure(constant);
            Lock = false;
            //AddToHistory("New constant Draged");
            bitmap.UpdateBitmap();
        }

        /// <summary>
        /// Создание нового порта
        /// </summary>
        /// <param name="Direction"></param>
        public void CreateNewPort(Schematix.FSM.My_Port.PortDirection Direction)
        {
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();

            Schematix.FSM.My_Port port = new Schematix.FSM.My_Port(this, Direction);
            SelectedFigureList.Add(port);
            SelectedFigure = port;
            graph.AddFigure(port);
            Lock = true;
        }

        /// <summary>
        /// Создание нового порта используя ToolBox
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="pt"></param>
        public void CreateNewPort_Dragged(Schematix.FSM.My_Port.PortDirection Direction, Point pt)
        {
            pt = form.PointToClient(pt);
            if (Lock == true)
            {
                MessageBox.Show("You must create figure " + SelectedFigure.name + " before.", "Fatal Error :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            graph.UnselectAllFigures();

            Schematix.FSM.My_Port port = new Schematix.FSM.My_Port("Port" + graph.Ports.Count.ToString(), "STD_LOGIC", pt, this);
            SelectedFigure = port;
            port.Direction = Direction;
            graph.AddFigure(port);
            Lock = false;
            //AddToHistory("New port Draged");
            bitmap.UpdateBitmap();
        }
        #endregion

        #region History
        public Schematix.FSM.HistoryElem AddToHistory(String name)
        {
            Schematix.FSM.HistoryElem newElem = new Schematix.FSM.HistoryElem(graph.Clone(), name);
            graph_history.add(newElem);
            UpdateHistory();
            return newElem;
        }

        public void UnDo()
        {
            Schematix.FSM.My_Graph tmp = graph_history.UnDo();
            if (tmp != null)
                graph = tmp;
            bitmap.UpdateBitmap();
            form.Invalidate();
        }

        public void ReDo()
        {
            Schematix.FSM.My_Graph tmp = graph_history.ReDo();
            if (tmp != null)
                graph = tmp;
            bitmap.UpdateBitmap();
            form.Invalidate();
        }
        #endregion

        /// <summary>
        /// Сменить масштаб
        /// </summary>
        /// <param name="scale"></param>
        public void ChangeScale(int scale)
        {
            paper.scale = scale;
            paper.scale = (double)100 / paper.scale;
            paper.ChangeScroll();

            form.Invalidate();
        }

        /// <summary>
        /// Вызвать окно свойств объекта
        /// </summary>
        public void ShowFigureProperties()
        {
            if (SelectedFigure != null)
            {
                if (SelectedFigure is Schematix.FSM.My_Line)
                {
                    Schematix.FSM.LineProperties LineProp  = new Schematix.FSM.LineProperties(SelectedFigure as Schematix.FSM.My_Line, this);
                    LineProp.ShowDialog();
                }
                if (SelectedFigure is Schematix.FSM.My_State)
                {
                    if (SelectedFigure is Schematix.FSM.My_Constant)
                    {
                        if (Graph.Language == FSM_Language.VHDL)
                        {
                            Schematix.FSM.SignalPropertiesVHDL ConstantProp = new Schematix.FSM.SignalPropertiesVHDL(SelectedFigure as Schematix.FSM.My_Constant, this);
                            ConstantProp.ShowDialog();
                        }
                        if (Graph.Language == FSM_Language.Verilog)
                        {
                            Schematix.FSM.SignalPropertiesVerilog ConstantProp = new Schematix.FSM.SignalPropertiesVerilog(SelectedFigure as Schematix.FSM.My_Constant, this);
                            ConstantProp.ShowDialog();
                        }
                    }
                    else
                    {
                        Schematix.FSM.StateProperties StateProp = new Schematix.FSM.StateProperties(SelectedFigure as Schematix.FSM.My_State, this);
                        StateProp.ShowDialog();
                    }
                }

                if ((SelectedFigure is Schematix.FSM.My_Constant) && (graph.Language == FSM_Language.Verilog))
                {
                    Schematix.FSM.ConstantProperties ConstProp = new Schematix.FSM.ConstantProperties(SelectedFigure as Schematix.FSM.My_Constant);
                    ConstProp.ShowDialog();
                    return;
                }
                
                if (SelectedFigure is Schematix.FSM.My_Signal)
                {
                    if (Graph.Language == FSM_Language.VHDL)
                    {
                        Schematix.FSM.SignalPropertiesVHDL SignalProp = new Schematix.FSM.SignalPropertiesVHDL(SelectedFigure as Schematix.FSM.My_Signal, this);
                        SignalProp.ShowDialog();
                    }
                    if (Graph.Language == FSM_Language.Verilog)
                    {
                        Schematix.FSM.SignalPropertiesVerilog SignalProp = new Schematix.FSM.SignalPropertiesVerilog(SelectedFigure as Schematix.FSM.My_Signal, this);
                        SignalProp.ShowDialog();
                    }
                }
                if (SelectedFigure is Schematix.FSM.My_Comment)
                {
                    Schematix.FSM.CommentProperties prop = new Schematix.FSM.CommentProperties(SelectedFigure as Schematix.FSM.My_Comment, this);                    
                    prop.ShowDialog();
                }
                if (SelectedFigure is Schematix.FSM.My_Reset)
                {
                    Schematix.FSM.Reset_Properties prop = new Schematix.FSM.Reset_Properties(SelectedFigure as Schematix.FSM.My_Reset, this);
                    prop.ShowDialog();
                }
            }
            form.Invalidate();
        }

        public static void SetColorText(Label label, Color color)
        {
            label.Text = "Current color: " + color.ToKnownColor().ToString();
            if (color.ToKnownColor().ToString() == "0")
                label.Text = "Current color: " + color.ToString();
        }

        #region Clipboard
        private DataFormats.Format clipboard_format = DataFormats.GetFormat(typeof(List<Schematix.FSM.My_Figure>).FullName);
        public void CutToClipboard()
        {
            CopyToClipboard();

            //Удаляем все лишнее
            graph.RemoveFigureRange(SelectedFigureList);


            SelectedFigureList.Clear();
            bitmap.UpdateBitmap();
            AddToHistory("Cut to clpboard");
            form.Invalidate();
        }

        public void CopyToClipboard()
        {
            // регистрируем свой собственный формат данных либо получаем его, если он уже зарегистрирован
            List<Schematix.FSM.My_Figure> copy_figure = new List<Schematix.FSM.My_Figure>();

            foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
            {
                copy_figure.Add(figure);
            }

            //Проверяем наличие линий
            foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
            {
                if (figure is My_Line)
                {
                    if (copy_figure.Contains((figure as My_Line).state_begin) == false)
                    {
                        copy_figure.Remove(figure);
                    }

                    if (copy_figure.Contains((figure as My_Line).state_end) == false)
                    {
                        copy_figure.Remove(figure);
                    }
                }
            }

            // копируем в буфер обмена
            IDataObject dataObj = new DataObject();
            dataObj.SetData(clipboard_format.Name, false, copy_figure);
            Clipboard.SetDataObject(dataObj, false);
        }

        public void GetFromClipboard()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            List<Schematix.FSM.My_Figure> pasted_figures = new List<Schematix.FSM.My_Figure>();
            if (dataObj.GetDataPresent(clipboard_format.Name))
            {
                pasted_figures = dataObj.GetData(clipboard_format.Name) as List<Schematix.FSM.My_Figure>;
                if (pasted_figures == null)
                    return;
            }
            foreach (Schematix.FSM.My_Figure figure in pasted_figures)
            {
                figure.Core = this;
                graph.AddFigure(figure);

                SelectedFigureList.Add(figure);
                figure.UpdateBitmapColors();
            }
            AddToHistory("Paste");
            bitmap.UpdateBitmap();
            form.Invalidate();
        }

        public bool CanPaste()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            bool res = dataObj.GetDataPresent(clipboard_format.Name);
            //bool res = Clipboard.ContainsData(clipboard_format.Name);

            if (res == true)
            {
                List<My_Figure> list = dataObj.GetData(clipboard_format.Name) as List<My_Figure>;
                dataObj.SetData(clipboard_format.Name, list);
                //List<My_Figure> list = Clipboard.GetData(clipboard_format.Name) as List<My_Figure>;
                //Clipboard.SetData(clipboard_format.Name, list);
            }
            return res;
        }

        public bool CanCopyCut()
        {
            return ((SelectedFigure != null) || (SelectedFigureList.Count != 0));
        }
        #endregion

        #region open/save
        /// <summary>
        /// Загрузить данные с потока
        /// </summary>
        /// <param name="stream"></param>
        public void OpenFile(Stream stream)
        {
            My_Graph item = null;
            item = My_Graph.OpenFile(stream);
                       
            item.Core = this;

            Graph_History.History.Clear();
            Graph = item;
            Bitmap.UpdateBitmap();
            form.Invalidate();
            HistoryElem newElem = AddToHistory("File Opened");
            graph_history.SetAsSaved();
        }

        /// <summary>
        /// Загрузить данные с файла
        /// </summary>
        /// <param name="adress"></param>
        public void OpenFile(String adress)
        {
            OpenFile(new FileStream(adress, FileMode.Open, FileAccess.Read, FileShare.None));
        }

        /// <summary>
        /// Сохранить в файл
        /// </summary>
        /// <param name="adress"></param>
        public void SaveToFile(String adress, bool SetAsSaved = true)
        {
            My_Graph.SaveToFile(graph, adress);
            if (SetAsSaved == true)
                graph_history.SetAsSaved();
        }

        /// <summary>
        /// Сохранить в поток
        /// </summary>
        /// <param name="stream"></param>
        public void SaveToFile(Stream stream, bool SetAsSaved = true)
        {
            My_Graph.SaveToFile(graph, stream);
            if (SetAsSaved == true)
                graph_history.SetAsSaved();
        }
        #endregion

        #region Modes
        public FSM_MODES mode = FSM_MODES.MODE_SELECT;
        public void CreateNewStateMode()
        {
            mode = FSM_MODES.MODE_ADD_STATE;
        }
        public void CreateNewTransitionMode()
        {
            mode = FSM_MODES.MODE_ADD_TRANSITION;
        }
        public void CreateNewConstantMode()
        {
            mode = FSM_MODES.MODE_ADD_CONSTANT;
        }
        public void CreateNewSignalMode()
        {
            mode = FSM_MODES.MODE_ADD_SIGNAL;
        }
        public void CreateNewCommentMode()
        {
            mode = FSM_MODES.MODE_ADD_COMMENT;
        }
        public void GoSelectionMode()
        {
            mode = FSM_MODES.MODE_SELECT;
        }

        public void ExecuteOperation() // Читает значение mode и выполняет команду
        {
            switch (mode)
            {
                case FSM_MODES.MODE_ADD_COMMENT:
                    {
                        CreateNewComment();
                    }
                    break;
                case FSM_MODES.MODE_ADD_CONSTANT:
                    {
                        CreateNewConstant();
                    }
                    break;
                case FSM_MODES.MODE_ADD_SIGNAL:
                    {
                        CreateNewSignal();
                    }
                    break;
                case FSM_MODES.MODE_ADD_STATE:
                    {
                        CreateNewState();
                    }
                    break;
                case FSM_MODES.MODE_ADD_TRANSITION:
                    {
                        CreateNewLine();
                    }
                    break;
                case FSM_MODES.MODE_SELECT:
                    { }
                    break;
                default:
                    { }
                    break;
            }
            mode = FSM_MODES.MODE_SELECT;
        }
        #endregion

        public void GenerateCodeFile(string filePath)
        {
            if (Graph.Language == FSM_Language.VHDL)
                GenerateCodeVHDL(filePath, this);
            if (Graph.Language == FSM_Language.Verilog)
                GenerateCodeVerilog(filePath, this);
        }

        private static void GenerateCodeVHDL(string filePath, Constructor_Core core)
        {
            Schematix.FSM.VHDL_Generator generator = new Schematix.FSM.VHDL_Generator(core);
            string code = generator.GenerateCode();

            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(code);
            writer.Close();
            stream.Close();
        }

        private static void GenerateCodeVerilog(string filePath, Constructor_Core core)
        {
            Schematix.FSM.Verilog_Generator generator = new Schematix.FSM.Verilog_Generator(core);
            string code = generator.GenerateCode();

            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(code);
            writer.Close();
            stream.Close();
        }
    }

    public enum FSM_MODES
    {
        MODE_SELECT,
        MODE_ADD_STATE,
        MODE_ADD_TRANSITION,
        MODE_ADD_SIGNAL,
        MODE_ADD_CONSTANT,
        MODE_ADD_COMMENT
    }

    public enum FSM_Language
    {
        VHDL,
        Verilog,
        SystemC
    }
}