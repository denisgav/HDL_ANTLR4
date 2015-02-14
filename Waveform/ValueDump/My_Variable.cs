using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.ComponentModel;
using Schematix.Waveform.Value_Dump;
using System.Collections.ObjectModel;
using DataContainer;
using DataContainer.Objects;
using DataContainer.MySortedDictionary;
using VHDL;

namespace Schematix.Waveform.Value_Dump
{
    public class My_Variable : INotifyPropertyChanged
    {
        /// <summary>
        /// Полное имя переменной
        /// </summary>
        private string fullname;
        public string FullName
        {
            get
            {
                return fullname;
            }
        }

        /// <summary>
        /// Имя переменной
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Отображать ли тип данных
        /// </summary>
        private Visibility showTypeName;
        public Visibility ShowTypeName
        {
            get { return showTypeName; }
            set 
            {
                showTypeName = value;
                NotifyPropertyChanged("ShowTypeName");
            }
        }

        /// <summary>
        /// Показывать ли размерность сигнала
        /// </summary>
        private Visibility showSize;
        public Visibility ShowSize
        {
            get { return showSize; }
            set 
            {
                showSize = value;
                NotifyPropertyChanged("ShowSize");
            }
        }

        /// <summary>
        /// Высота, используется для представления данных
        /// </summary>
        private double height;
        public double Height
        {
            get { return height; }
            set 
            {
                height = value;
                NotifyPropertyChanged("Height");
            }
        }

        /// <summary>
        /// Тип данных
        /// </summary>
        public string VariableType
        {
            get
            {
                return signal.Type.Name;
            }
        }

        /// <summary>
        /// Размерность типа данных
        /// </summary>
        public string VariableSize
        {
            get
            {
                ResolvedDiscreteRange[] ranges = signal.Type.Dimension;
                if (ranges.Length == 1)
                    return (ranges[0].Length != 1)?ranges[0].ToString():string.Empty;
                else
                {
                    StringBuilder res = new StringBuilder();
                    res.Append('(');
                    for (int i = 0; i < ranges.Length; i++ )
                    {
                        ResolvedDiscreteRange range = ranges[i];
                        if(i < ranges.Length - 1)
                            res.Append(string.Format("{0}, ", range.ToString()));
                        else
                            res.Append(range.ToString());
                    }
                    res.Append(')');
                    return res.ToString();
                }                
            }
        }

        /// <summary>
        /// Текущее значение сигнала относительно координат курсора
        /// </summary>
        private string cursorValue = "value";
        public string CursorValue
        {
            get { return cursorValue; }
            set 
            {
                cursorValue = value;
                NotifyPropertyChanged("CursorValue");
            }
        }

        /// <summary>
        /// Можно ли изменить имя переменной
        /// </summary>
        private bool isEditableName;
        public bool IsEditableName
        {
            get { return isEditableName; }
            set { isEditableName = value; }
        }
        


        /// <summary>
        /// Представление данных
        /// </summary>
        private DataRepresentation dataRepresentation;
        public DataRepresentation DataRepresentation
        {
            get
            {
                return dataRepresentation;
            }
            set
            {
                dataRepresentation = value;
                NotifyPropertyChanged("DataRepresentation");
            }
        }

        /// <summary>
        /// Сигнал, используемій для представления
        /// </summary>
        private Signal signal;
        public Signal Signal
        {
            get
            {
                return signal;
            }
        }

        public My_Variable(Signal signal)
            : this(signal.Name, signal.Name, signal)
        { }

        public My_Variable(string Name, string FullName, Signal signal)
        {
            this.name = Name;
            this.fullname = FullName;
            this.signal = signal;

            if (signal.Range[0].Length != 1)
                DataRepresentation = new VectorDataRepresentation();
            else
                DataRepresentation = new DataRepresentation();

            showTypeName = Visibility.Collapsed;
            showSize = Visibility.Collapsed;
            height = 20;
            
            CreateChildrens();
        }

        /// <summary>
        /// Формирование дочерних элементов
        /// </summary>
        private void CreateChildrens()
        {
            List<Signal> ch = signal.Childrens;
            if (ch.Count != 0)
            {
                childrens = new List<My_Variable>();
                foreach (Signal variable in ch)
                {
                    childrens.Add(new My_Variable(variable));
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Итератор для обработки данных
        /// </summary>
        public IValueIterator Iterator
        {
            get
            {
                IValueIterator iter = signal.Dump.Iterator;
                iter.DataRepresentation = dataRepresentation;
                return iter;
            }
        }

        
        /// <summary>
        /// Есть ли вложенные переменные
        /// </summary>
        public bool HasChildrens
        {
            get
            {
                return (childrens != null) && (childrens.Count != 0);
            }
        }

        /// <summary>
        /// Вложенные переменные
        /// </summary>
        private List<My_Variable> childrens;
        public List<My_Variable> Childrens
        {
            get 
            {
                return childrens;
            }
        }

        /// <summary>
        /// Зафиксировать изменение данных
        /// </summary>
        public void Update()
        {
            NotifyPropertyChanged("DataRepresentation");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}