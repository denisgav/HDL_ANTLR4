using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    [Serializable]
    public class My_Comment : My_Figure
    {
        public Rectangle rect;

        public Color color { get; set; }
        private Color color_name;
        private Color[] color_marks;
        private Color color_state;
        private int selected_mark;

        private Point start_point; // используется для перемещения
        private Point start_location;// используется для перемещения
        private bool creation_flag;  //используется при создании

        public override void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Brush brush;
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            if (selected == false)
                brush = new SolidBrush(Color.FromArgb(128, color.R, color.G, color.B));
            else
                brush = new SolidBrush(SelectedColor);

            dc.FillRectangle(brush, rect);
            dc.DrawRectangle(pen, rect);
            System.Drawing.StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            dc.DrawString(name, new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(color_name), rect, format);
            if (selected == true)
            {
                brush = new SolidBrush(Color.Black);
                dc.FillRectangle(brush, rect.Left, rect.Top, 5, 5);
                dc.FillRectangle(brush, (rect.Left + (rect.Right - rect.Left) / 2), rect.Top, 5, 5);
                dc.FillRectangle(brush, rect.Right - 5, rect.Top, 5, 5);

                dc.FillRectangle(brush, rect.Left, rect.Bottom - 5, 5, 5);
                dc.FillRectangle(brush, (rect.Left + (rect.Right - rect.Left) / 2), rect.Bottom - 5, 5, 5);
                dc.FillRectangle(brush, rect.Right - 5, rect.Bottom - 5, 5, 5);

                dc.FillRectangle(brush, rect.Left, (rect.Top + (rect.Bottom - rect.Top) / 2), 5, 5);
                dc.FillRectangle(brush, rect.Right - 5, (rect.Top + (rect.Bottom - rect.Top) / 2), 5, 5);
            }
        }

        public override void Draw_Bitmap(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Brush brush = new SolidBrush(color_state);

            dc.FillRectangle(brush, rect);

            dc.FillRectangle(new SolidBrush(color_marks[0]), rect.Left, rect.Top, 5, 5);
            dc.FillRectangle(new SolidBrush(color_marks[1]), (rect.Left + (rect.Right - rect.Left) / 2), rect.Top, 5, 5);
            dc.FillRectangle(new SolidBrush(color_marks[2]), rect.Right - 5, rect.Top, 5, 5);

            dc.FillRectangle(new SolidBrush(color_marks[5]), rect.Left, rect.Bottom - 5, 5, 5);
            dc.FillRectangle(new SolidBrush(color_marks[6]), (rect.Left + (rect.Right - rect.Left) / 2), rect.Bottom - 5, 5, 5);
            dc.FillRectangle(new SolidBrush(color_marks[7]), rect.Right - 5, rect.Bottom - 5, 5, 5);

            dc.FillRectangle(new SolidBrush(color_marks[3]), rect.Left, (rect.Top + (rect.Bottom - rect.Top) / 2), 5, 5);
            dc.FillRectangle(new SolidBrush(color_marks[4]), rect.Right - 5, (rect.Top + (rect.Bottom - rect.Top) / 2), 5, 5);
        }

        public My_Comment(Rectangle rect, string name, Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            this.rect = rect;
            this.name = name;
            mouse_move = MouseMoveCreateNew;
            mouse_up = MouseUpCreateNew;
            mouse_down = MouseDownCreateNew;
            draw = Draw;
            selected_mark = 0;
            color_name = Color.FromArgb(255, 0, 0, 0);
            color_marks = new Color[8];
            UpdateBitmapColors();
            color = Color.YellowGreen;
            color_name = Color.Black;
            creation_flag = true;
        }
        public My_Comment(Rectangle rect, Schematix.FSM.Constructor_Core core)
            : this(rect, ("Comment #" + core.Graph.Comments.Count.ToString()), core)
        { }
        public My_Comment(Schematix.FSM.Constructor_Core core)
            : this(new Rectangle(-80, -80, 40, 40), core)
        { }

        public My_Comment(My_Comment item)
        {
            this.core = item.core;
            this.color = item.color;
            this.color_name = item.color_name;
            this.color_state = item.color_state;
            this.core = item.core;
            this.name = item.name;
            this.rect = item.rect;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            draw = Draw;
            color_marks = new Color[8];
            UpdateBitmapColors();
        }

        public My_Comment(Constructor_Core core, Rectangle rect, bool isDraggedState)
        {
            this.core = core;
            this.rect = rect;
            color = Color.Orange;
            color_name = Color.Black;
            name = ("Comment" + core.Graph.Comments.Count.ToString());
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            draw = Draw;
            color_marks = new Color[8];
            UpdateBitmapColors();
        }

        public override Size MaxSize
        {
            get { return new Size(rect.Width, rect.Height); }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
        }

        public override void MouseMoveResize(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (pt.X < 0)
                pt.X = 0;
            if (pt.Y < 0)
                pt.Y = 0;

            switch (selected_mark)
            {
                case 1:
                    {
                        if (((rect.Right - pt.X) < 0) || ((rect.Bottom - pt.Y) < 0))
                            return;

                        Rectangle rect1 = new Rectangle(pt.X, pt.Y, (rect.Right - pt.X), (rect.Bottom - pt.Y));
                        rect = rect1;
                    }
                    break;
                case 2:
                    {
                        if ((rect.Bottom - pt.Y) < 0)
                            return;

                        Rectangle rect1 = new Rectangle(rect.Left, pt.Y, rect.Width, (rect.Bottom - pt.Y));
                        rect = rect1;
                    }
                    break;
                case 3:
                    {
                        if (((pt.X - rect.Left) < 0) || ((rect.Bottom - pt.Y) < 0))
                            return;

                        Rectangle rect1 = new Rectangle(rect.Left, pt.Y, (pt.X - rect.Left), (rect.Bottom - pt.Y));
                        rect = rect1;
                    }
                    break;
                case 4:
                    {
                        if ((rect.Right - pt.X) < 0)
                            return;

                        Rectangle rect1 = new Rectangle(pt.X, rect.Top, (rect.Right - pt.X), rect.Height);
                        rect = rect1;
                    }
                    break;
                case 5:
                    {
                        if ((pt.X - rect.Left) < 0)
                            return;

                        Rectangle rect1 = new Rectangle(rect.Left, rect.Top, (pt.X - rect.Left), rect.Height);
                        rect = rect1;
                    }
                    break;
                case 6:
                    {
                        if (((rect.Right - pt.X) < 0) || ((pt.Y - rect.Top) < 0))
                            return;

                        Rectangle rect1 = new Rectangle(pt.X, rect.Top, (rect.Right - pt.X), (pt.Y - rect.Top));
                        rect = rect1;
                    }
                    break;
                case 7:
                    {
                        if ((pt.Y - rect.Top) < 0)
                            return;

                        Rectangle rect1 = new Rectangle(rect.Left, rect.Top, rect.Width, (pt.Y - rect.Top));
                        rect = rect1;
                    }
                    break;
                case 8:
                    {
                        if (((pt.X - rect.Left) < 0) || ((pt.Y - rect.Top) < 0))
                            return;

                        Rectangle rect1 = new Rectangle(rect.Left, rect.Top, (pt.X - rect.Left), (pt.Y - rect.Top));
                        rect = rect1;
                    }
                    break;
                default:
                    {
                        Rectangle rect2 = new Rectangle(rect.Location, rect.Size);
                        rect2.Location = new Point(start_location.X - (this.start_point.X - pt.X), start_location.Y - (this.start_point.Y - pt.Y));

                        Point NewPoint = rect.Location;
                        if (rect2.Left >= 0)
                            NewPoint.X = rect2.Location.X;

                        if (rect2.Top >= 0)
                            NewPoint.Y = rect2.Location.Y;

                        rect.Location = NewPoint;
                    }
                    break;
            }
            core.form.Invalidate();
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;
            Point pt = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                //if (((pt.X < rect.Left) || (pt.X > rect.Right)) || ((pt.Y < rect.Top) || (pt.Y > rect.Bottom)))
                //{
                //    if ((selected == true) || (selected_mark != 0))
                //    {
                //        selected = false;
                //        this.form.Invalidate(this.rect);
                //    }
                //    return;
                //}
                start_point = pt;
                start_location = rect.Location;
                mouse_move = MouseMoveResize;
            }
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                core.form.contextMenuStrip.Show(p);
            }
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (rect.Width <= 40)
                    rect.Width = 40;

                if (rect.Height <= 40)
                    rect.Height = 40;

                mouse_move = MouseMove;
                core.Bitmap.UpdateBitmap();
                //core.AddToHistory("Comment moved");
            }
        }

        public override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            rect.Location = new Point(e.X, e.Y);
            creation_flag = false;
        }

        public override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (creation_flag == true)
                return;

            if ((e.X - rect.Location.X) < 0)
                return;
            if ((e.Y - rect.Location.Y) < 0)
                return;

            rect.Size = new Size((e.X - rect.Location.X), (e.Y - rect.Location.Y));
            core.form.Invalidate();
        }

        public override void MouseUpCreateNew(object sender, MouseEventArgs e)
        {
            if (creation_flag == true)
                return;

            if (rect.Width <= 40)
                rect.Width = 40;

            if (rect.Height <= 40)
                rect.Height = 40;

            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            //core.AddToHistory("Comment created");
            core.Bitmap.UpdateBitmap();
        }

        public override void Select(Color point_color)
        {
            if (point_color.ToArgb() == color_marks[0].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNWSE;
                selected_mark = 1;
                return;
            }

            if (point_color.ToArgb() == color_marks[1].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNS;
                selected_mark = 2;
                return;
            }

            if (point_color.ToArgb() == color_marks[2].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNESW;
                selected_mark = 3;
                return;
            }

            if (point_color.ToArgb() == color_marks[3].ToArgb())
            {
                core.form.Cursor = Cursors.SizeWE;
                selected_mark = 4;
                return;
            }

            if (point_color.ToArgb() == color_marks[4].ToArgb())
            {
                core.form.Cursor = Cursors.SizeWE;
                selected_mark = 5;
                return;
            }

            if (point_color.ToArgb() == color_marks[5].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNESW;
                selected_mark = 6;
                return;
            }

            if (point_color.ToArgb() == color_marks[6].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNS;
                selected_mark = 7;
                return;
            }

            if (point_color.ToArgb() == color_marks[7].ToArgb())
            {
                core.form.Cursor = Cursors.SizeNWSE;
                selected_mark = 8;
                return;
            }

            if (core.form.Cursor != Cursors.Default)
                core.form.Cursor = Cursors.Default;
            selected_mark = 0;
        }

        public override bool Select(Rectangle SelectedRectangle)
        {
            if (SelectedRectangle.Left > rect.Left)
            {
                return false;
            }
            if (SelectedRectangle.Right < rect.Right)
            {
                return false;
            }
            if (SelectedRectangle.Top > rect.Top)
            {
                return false;
            }
            if (SelectedRectangle.Bottom < rect.Bottom)
            {
                return false;
            }
            selected_mark = 0;
            return true;
        }

        public override bool IsSelected(Color point_color)
        {
            if (point_color.ToArgb() == color_marks[0].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[1].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[2].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[3].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[4].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[5].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[6].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_marks[7].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_state.ToArgb())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateBitmapColors()
        {
            color_marks[0] = GenerateRandomColor();
            color_marks[1] = GenerateRandomColor();
            color_marks[2] = GenerateRandomColor();
            color_marks[3] = GenerateRandomColor();
            color_marks[4] = GenerateRandomColor();
            color_marks[5] = GenerateRandomColor();
            color_marks[6] = GenerateRandomColor();
            color_marks[7] = GenerateRandomColor();
            color_state = GenerateRandomColor();
        }
    }
}