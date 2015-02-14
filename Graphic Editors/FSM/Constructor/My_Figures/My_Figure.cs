using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.FSM
{
    [Serializable]
    public abstract class My_Figure
    {
        protected bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (core == null)
                {
                    selected = value;
                    return;
                }
                if (value == false)//снимается выделение
                {
                    if (core.Graph.SelectedFigureList.Contains(this) == false)
                    {
                        selected = false;
                    }
                    else
                        selected = true;

                    /*возвращаем на всякий случай стандартный курсор*/
                    if (core.form.Cursor != Cursors.Default)
                        core.form.Cursor = Cursors.Default;
                }
                else
                    selected = true;
            }
        }

        public void Unselect()
        {
            selected = false;
        }

        public string name { get; set; }

        public virtual Point CenterPoint { get; set; }

        public static Color SelectedColor = Color.Aqua;

        //Делегаты - Без них никак :)
        public delegate void MouseMoveDelegate(object sender, MouseEventArgs e);
        public MouseMoveDelegate mouse_move;
        public delegate void MouseUpDelegate(object sender, MouseEventArgs e);
        public MouseUpDelegate mouse_up;
        public delegate void MouseDownDelegate(object sender, MouseEventArgs e);
        public MouseDownDelegate mouse_down;
        public delegate void DrawDelegate(object sender, PaintEventArgs e);
        public DrawDelegate draw;

        public abstract Size MaxSize { get; }

        [NonSerialized]
        protected Schematix.FSM.Constructor_Core core;
        public virtual Schematix.FSM.Constructor_Core Core
        {
            get { return core; }
            set { core = value; }
        }

        public My_Figure()
        { }

        public virtual void Draw(object sender, PaintEventArgs e)
        { }
        public virtual void Draw_Bitmap(object sender, PaintEventArgs e)
        { }
        public virtual void MouseMove(object sender, MouseEventArgs e)
        { }
        public virtual void MouseMoveResize(object sender, MouseEventArgs e)
        { }
        public virtual void MouseDown(object sender, MouseEventArgs e)
        { }
        public virtual void MouseUp(object sender, MouseEventArgs e)
        { }
        public virtual void MouseDownCreateNew(object sender, MouseEventArgs e)
        { }
        public virtual void MouseMoveCreateNew(object sender, MouseEventArgs e)
        { }
        public virtual void MouseUpCreateNew(object sender, MouseEventArgs e)
        { }
        public virtual bool IsSelected(Color point_color)
        {
            return false;
        }
        public virtual void Select(Color point_color)
        { }
        public virtual bool Select(Rectangle SelectedRectangle)
        {
            return false;
        }

        #region Random
        /*генерация случайных чисел для цвета*/
        private static Random rand = new Random();
        public static Color GenerateRandomColor()
        {
            return Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }
        /*************************************/
        #endregion

        public virtual void UpdateBitmapColors()
        { }

        public static bool IsSerializable(object obj)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            BinaryFormatter bin = new BinaryFormatter();
            try
            {
                bin.Serialize(mem, obj);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Объект не может быть сериализован. \n" + ex.ToString());
                return false;
            }
        }
    }
}