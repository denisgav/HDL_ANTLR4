using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using Schematix.Core;

namespace Schematix.FSM
{
    [Serializable]
    public class My_Graph
    {
        /// <summary>
        /// Язык проекта
        /// </summary>
        public FSM_Language Language { get; set; }

        /// <summary>
        /// Данные о модуле на языке Verilog
        /// </summary>
        public Verilog_Module VerilogModule { get; set; }

        /// <summary>
        /// Данные о модуле на языке VHDL
        /// </summary>
        public VHDL_Module VHDLModule { get; set; }

        /// <summary>
        /// Перечень хранимых фигур в проекте
        /// </summary>
        private List<My_Figure> figures;
        public IList<My_Figure> Figures
        {
            get { return new List<My_Figure> (figures); }
        }

        /// <summary>
        /// текущая выбранная фигура
        /// </summary>
        private Schematix.FSM.My_Figure selectedFigure;
        public Schematix.FSM.My_Figure SelectedFigure
        {
            get { return selectedFigure; }
            set { selectedFigure = value; }
        }

        /// <summary>
        /// Выбранная группа фигур
        /// </summary>
        private List<Schematix.FSM.My_Figure> selectedFigureList;
        public List<Schematix.FSM.My_Figure> SelectedFigureList
        {
            get { return selectedFigureList; }
        }

        /// <summary>
        /// Удалить фигуру
        /// </summary>
        /// <param name="rem"></param>
        public void RemoveFigure<T>(T rem) where T:My_Figure
        {
            figures.Remove(rem);
        }

        /// <summary>
        /// Добавление фигуры
        /// </summary>
        /// <param name="fig"></param>
        public void AddFigure<T>(T fig) where T : My_Figure
        {
            figures.Add(fig);
        }

        /// <summary>
        /// Удалить фигуры
        /// </summary>
        /// <param name="rem"></param>
        public void RemoveFigureRange<T>(IList<T> rem) where T : My_Figure
        {
            foreach(T f in rem)
                figures.Remove(f);
        }

        /// <summary>
        /// Добавление фигур
        /// </summary>
        /// <param name="fig"></param>
        public void AddFigureRange<T>(IList<T> fig) where T : My_Figure
        {
            figures.AddRange(fig);
        }

        #region Filter figures
        /// <summary>
        /// Отфильстровать фигуры по типу данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> FilterFiguresSoft<T>() where T : My_Figure
        {
            List<T> res = new List<T>();
            foreach (My_Figure fig in figures)
                if (fig is T) 
                    res.Add(fig as T);

            return res;
        }

        /// <summary>
        /// Отфильстровать фигуры по типу данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> FilterFigures<T>() where T : My_Figure
        {
            List<T> res = new List<T>();
            foreach (My_Figure fig in figures)
                if ((fig is T) && (fig.GetType().FullName == typeof(T).FullName))
                    res.Add(fig as T);

            return res;
        }
        #endregion

        #region Figures
        /// <summary>
        /// Перечень состояний
        /// </summary>
        public IList<My_State> States
        {
            get { return FilterFigures<My_State>();  }
        }

        /// <summary>
        /// Перечень переходов
        /// </summary>
        public IList<My_Line> Lines
        {
            get { return FilterFigures<My_Line>(); }
        }

        /// <summary>
        /// Перечень комментариев
        /// </summary>
        public IList<My_Comment> Comments
        {
            get { return FilterFigures<My_Comment>(); }
        }

        /// <summary>
        /// Перечень сигналов
        /// </summary>
        public IList<My_Signal> Signals
        {
            get { return FilterFigures<My_Signal>(); }
        }

        /// <summary>
        /// Перечень констант
        /// </summary>
        public IList<My_Constant> Constants
        {
            get { return FilterFigures<My_Constant>(); }
        }

        /// <summary>
        /// Перечень портов
        /// </summary>
        public IList<My_Port> Ports
        {
            get { return FilterFigures<My_Port>(); }
        }

        /// <summary>
        /// Найти порт типа Clk
        /// </summary>
        public My_Port ClockPort
        {
            get
            {
                foreach (My_Port port in Ports)
                {
                    if (port.Port_Type == My_Port.PortType.Clock)
                        return port;
                }
                return null;
            }
        }

        /// <summary>
        /// возвращает список линий, которые соединяют состояния s1 и s2
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        private List<My_Line> GetLinesByStates(My_State s1, My_State s2)
        {
            List<My_Line> res = new List<My_Line>();
            foreach (My_Line line in Lines)
            {
                if (
                    ((line.state_begin == s1) && (line.state_end == s2))
                    ||
                    ((line.state_begin == s2) && (line.state_end == s1))
                )
                {
                    res.Add(line);
                }
            }
            return res;
        }
        #endregion

        /// <summary>
        /// Сброс
        /// </summary>
        private My_Reset reset = null;
        public My_Reset Reset
        {
            get { return reset; }
            set { reset = value; }
        }

        public Size MaxSize
        {
            get
            {
                Size res = core.form.ClientSize;

                foreach (My_Figure fig in figures)
                {
                    if (fig.MaxSize.Width > res.Width)
                        res.Width = fig.MaxSize.Width;
                    if (fig.MaxSize.Height > res.Height)
                        res.Height = fig.MaxSize.Height;
                }

                if ((Reset != null) && (Reset.rect.Right > res.Width))
                    res.Width = Reset.rect.Right;
                if ((Reset != null) && (Reset.rect.Bottom > res.Height))
                    res.Height = Reset.rect.Height;

                if (res.Width < 10)
                    res.Width = 10;
                if (res.Height < 10)
                    res.Height = 10;

                return res;
            }
        }

        [NonSerialized]
        private Schematix.FSM.Constructor_Core core;
        public Schematix.FSM.Constructor_Core Core
        {
            get { return core; }
            set 
            {
                core = value;

                //копируем данные
                foreach (My_Figure fig in figures)
                {
                    fig.Core = core;
                }

                Reset = Reset;
                if (Reset != null)
                    Reset.Core = core;
            }
        }

        public My_Graph(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            figures = new List<My_Figure>();
            selectedFigure = null;
            selectedFigureList = new List<My_Figure>();
        }

        #region SaveFile
        /// <summary>
        /// Сохранить в файл
        /// </summary>
        /// <param name="adress"></param>
        public static void SaveToFile(My_Graph graph, String adress)
        {
            SaveToFile(graph, File.Create(adress));
        }

        /// <summary>
        /// Сохранить в поток
        /// </summary>
        /// <param name="stream"></param>
        public static void SaveToFile(My_Graph graph, Stream stream)
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, graph);
            stream.Close();
        }
        #endregion

        #region OpenFile

        /// <summary>
        /// Загрузить данные с потока
        /// </summary>
        /// <param name="stream"></param>
        public static My_Graph OpenFile(Stream stream)
        {
            //выполняем чтение
            IFormatter formatter = new BinaryFormatter();
            My_Graph item = (My_Graph)formatter.Deserialize(stream);
            stream.Close();

            return item;
        }

        /// <summary>
        /// Загрузить данные с файла
        /// </summary>
        /// <param name="adress"></param>
        public static My_Graph OpenFile(String adress)
        {
            return OpenFile(new FileStream(adress, FileMode.Open, FileAccess.Read, FileShare.None));
        }
        #endregion

        #region Remove handling
        public void Delete_Line(Schematix.FSM.My_Line line)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete " + line.name + "?", "Delete Line?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                RemoveFigure(line);
        }

        public void Delete_Comment(Schematix.FSM.My_Comment comment)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete comment ?", "Delete Comment?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                RemoveFigure(comment);
        }

        public void Delete_Constant(Schematix.FSM.My_Constant c)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete constant " + c.name +" ?", "Delete Constant?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                RemoveFigure(c);
        }

        public void Delete_Port(Schematix.FSM.My_Port p)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete port " + p.name + " ?", "Delete Port?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                RemoveFigure(p);
        }

        public void Delete_Signal(Schematix.FSM.My_Signal s)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete signal " + s.name + " ?", "Delete Signal?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                RemoveFigure(s);
        }

        public void Delete_State(Schematix.FSM.My_State state)
        {
            DialogResult res = MessageBox.Show("Are you sure want to delete " + state.name + "?", "Delete State?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                List<Schematix.FSM.My_Line> removed_lines = new List<My_Line>();
                foreach (Schematix.FSM.My_Line line in Lines)
                {
                    if (line.state_begin == state)
                        removed_lines.Add(line);
                    if (line.state_end == state)
                        removed_lines.Add(line);
                }
                RemoveFigureRange(removed_lines);
                RemoveFigure(state);
            }
        }
        #endregion               


        /// <summary>
        /// Обновление углов под которыми выходят линии из состояний s1 и s2
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        public void UpdateLinesAggle(My_State s1, My_State s2)
        {
            List<My_Line> lines = GetLinesByStates(s1, s2);
            if ((lines.Count == 0) || (lines.Count == 1))
                return;

            double sweep_angle = (Math.PI / 4.0) / ((double)lines.Count - 1);
            double cur_angle = -(Math.PI / 4.0) / 2.0;
            
            for (int i = 0; i < lines.Count; i++, cur_angle += sweep_angle)
            {
                if (lines[i].state_begin == s1)
                {
                    lines[i].RotateStartPoint(cur_angle);
                    lines[i].RotateEndPoint(-cur_angle);
                }
                else
                {
                    lines[i].RotateStartPoint(-cur_angle);
                    lines[i].RotateEndPoint(cur_angle);
                }
            }
        }

        /// <summary>
        /// Создать дубликат объекта
        /// </summary>
        /// <returns></returns>
        public My_Graph Clone()
        {
            My_Graph res = Schematix.FSM.ObjectCopier.Clone(this);
            res.Core = core;
            return res;
        }

        /// <summary>
        /// Удалить выбранную фигуру
        /// </summary>
        public void DeleteFigure()
        {
            if (SelectedFigureList.Count == 0)
                return;
            foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
            {
                String NameOfFigure = figure.name;
                if (figure is Schematix.FSM.My_Line)
                    Delete_Line(figure as Schematix.FSM.My_Line);
                if (figure is Schematix.FSM.My_State)
                    Delete_State(figure as Schematix.FSM.My_State);
                if (figure is Schematix.FSM.My_Comment)
                    Delete_Comment(figure as Schematix.FSM.My_Comment);

                if (figure is Schematix.FSM.My_Constant)
                    Delete_Constant(figure as Schematix.FSM.My_Constant);
                else
                {
                    if (figure is Schematix.FSM.My_Port)
                        Delete_Port(figure as Schematix.FSM.My_Port);
                    if (figure is Schematix.FSM.My_Signal)
                        Delete_Signal(figure as Schematix.FSM.My_Signal);
                }

                if (figure is My_Reset)
                    Reset = null;
            }
            UnselectAllFigures();
        }

        /// <summary>
        /// Можно ли выбрать все фигуры
        /// </summary>
        /// <returns></returns>
        public bool CanSelectAllFigures()
        {
            if (reset != null)
                return true;
            if (figures.Count != 0)
                return true;

            return false;
        }

        /// <summary>
        /// Выбрать все фигуры
        /// </summary>
        public void SelectAllFigures()
        {
            SelectedFigureList.Clear();
            foreach (My_Figure fig in figures)
            {
                fig.Selected = true;
                SelectedFigureList.Add(fig);
            }            
            if (Reset != null)
            {
                Reset.Selected = true;
                SelectedFigureList.Add(Reset);
            }
        }

        /// <summary>
        /// Выбрать набор фигур
        /// </summary>
        /// <param name="figures"></param>
        public void SelectFigures(IList<My_Figure> sel_figures)
        {
            SelectedFigureList.Clear();
            foreach (My_Figure fig in sel_figures)
            {
                fig.Selected = true;
                SelectedFigureList.Add(fig);
            }            
        }

        /// <summary>
        /// Снфть выделение с всех фигур
        /// </summary>
        public void UnselectAllFigures()
        {
            foreach (My_Figure fig in figures)
            {
                fig.Unselect();
            }
            
            if (Reset != null)
                Reset.Unselect();
            
            SelectedFigureList.Clear();
            //SelectedFigure = null;
        }
    }
}