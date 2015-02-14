using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_Picture
    {
        private List<My_Figure> figureList;
        public List<My_Figure> FigureList
        {
            get { return figureList; }
        }
        public EntityDrawningInfo info;

        public Size MaxSize
        {
            get
            {
                Size res;

                if (core != null)
                    res = core.Form.ClientSize;
                else
                    res = new Size(0, 0);
                foreach (My_Figure fig in FigureList)
                {
                    foreach (Point p in fig.Points)
                    {
                        if (p.X > res.Width)
                            res.Width = p.X + 2;
                        if (p.Y > res.Height)
                            res.Height = p.Y + 2;
                    }
                }
                return res;
            }
        }

        private Point GetMinimumLocation()
        {
            Point minPoint = FigureList[0].Points[0];
            //Находим минимальную точку
            foreach (My_Figure fig in FigureList)
            {
                foreach (Point p in fig.Points)
                {
                    if (p.X < minPoint.X)
                        minPoint.X = p.X - 2;
                    if (p.Y < minPoint.Y)
                        minPoint.Y = p.Y - 2;
                }
            }
            return minPoint;
        }

        public Rectangle PictureRectangle
        {
            get
            {
                Rectangle rect = new Rectangle();
                Size MaxSize_ = this.MaxSize;

                Rectangle metafile_rect = new Rectangle();
                rect.Location = GetMinimumLocation();
                rect.Width = MaxSize_.Width - rect.Location.X;
                rect.Height = MaxSize_.Height - rect.Location.Y;
                return rect;
            }
        }

        public static bool IsSerializable(object obj)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            BinaryFormatter bin = new BinaryFormatter();
            try
            {
                bin.Serialize(mem, obj);
                object pict = (object)bin.Deserialize(mem);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Could not serialize object. \n" + ex.ToString());
                Schematix.Core.Logger.Log.Error("Could not serialize object.", ex);
                return false;
            }
        }

        public static byte[] Serialize(List<My_Figure> figures)
        {
            if (figures != null)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(mem, figures);
                byte[] res = mem.GetBuffer();
                mem.Close();
                return res;
            }
            return new byte[0];
        }

        public static List<My_Figure> Deserialize(byte[] data)
        {
            if (data != null)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                mem.Write(data, 0, data.Length);
                mem.Seek(0, SeekOrigin.Begin);
                BinaryFormatter bin = new BinaryFormatter();
                List<My_Figure> res = bin.Deserialize(mem) as List<My_Figure>;
                mem.Close();
                return res;
            }
            return null;
        }

        /// <summary>
        /// Создать дубликат объекта
        /// </summary>
        /// <returns></returns>
        public My_Picture Clone()
        {
            My_Picture res = ObjectCopier.Clone(this);
            res.Core = core;
            return res;
        }

        [NonSerialized]
        private EntityDrawningCore core;
        public EntityDrawningCore Core
        {
            get { return core; }
            set
            {
                core = value;
                foreach (My_Figure fig in figureList)
                    fig.Core = value;
            }
        }

        public My_Picture() // необходим для десериализации
        {
            figureList = new List<My_Figure>();
        }

        public My_Picture(EntityDrawningCore core)
        {
            figureList = new List<My_Figure>();
            this.core = core;
        }        
    
        public void Sort()
        {
            FigureList.Sort((f1, f2) => f1.zIndex.CompareTo(f2.zIndex)); //сортировка
        }

        public void SaveProject(Stream stream)
        {
            //core.UnselectAll();
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, this);
            stream.Close();
            core.History.SetAsSaved();
        }
        
        public void SaveProject(string filename)
        {
            SaveProject(File.Create(filename));
        }

        public void SaveToBitmap(string filename)
        {
            Size MaxSize_ = this.MaxSize;
            Bitmap bitmap = new Bitmap(MaxSize_.Width, MaxSize_.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bitmap);
            Draw(this, new PaintEventArgs(gr, new Rectangle(0, 0, MaxSize_.Width, MaxSize_.Height)));
            bitmap.Save(filename);
        }

        public System.Drawing.Imaging.Metafile GetMetaFile(Graphics gr)
        {
            foreach (My_Figure fig in FigureList)
                fig.selected = false;

            Size MaxSize_ = this.MaxSize;

            Rectangle metafile_rect = new Rectangle();
            metafile_rect.Location = GetMinimumLocation();
            metafile_rect.Width = MaxSize_.Width - metafile_rect.Location.X;
            metafile_rect.Height = MaxSize_.Height - metafile_rect.Location.Y;

            IntPtr hdc = gr.GetHdc();
            System.Drawing.Imaging.Metafile res = new System.Drawing.Imaging.Metafile(new MemoryStream(), hdc, new RectangleF(new PointF(0, 0),  metafile_rect.Size), System.Drawing.Imaging.MetafileFrameUnit.Pixel);
            Graphics graphics = Graphics.FromImage(res);
            graphics.TranslateTransform(-metafile_rect.X, -metafile_rect.Y);
            Draw(this, new PaintEventArgs(graphics, metafile_rect));
            graphics.Dispose();
            return res;
        }

        public void openProject(Stream stream)
        {
            if (stream.Length == 0)
                return;
            IFormatter formatter = new BinaryFormatter();
            My_Picture pict = (My_Picture)formatter.Deserialize(stream);
            info = pict.info;
            stream.Close();

            if ((info != null) && (info.IsCorrect() == true))
            {
                foreach (My_Figure fig in pict.FigureList)
                {
                    fig.Core = core;

                    if (fig is My_Port)
                    {
                        (fig as My_Port).TextLabel.Core = core;
                    }
                }
                pict.core = core;
                this.figureList = pict.FigureList;
            }
            else
            {
                if (info != null)
                    core.Picture.openVHDLFile(info.VHDLFileName, info.Entity.name);
            }

            if (core != null)
            {
                core.History.ClearHistory();
                core.History.add(this);
                core.History.SetAsSaved();
            }
        }

        public void openProject(string filename)
        {
            openProject(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None));
        }

        public void openVHDLFile(string filename)
        {
            My_FileAnalyzer analyzer = new My_FileAnalyzer(filename, core);
            analyzer.Analyze();
            foreach (My_Figure fig in analyzer.figures)
            {
                FigureList.Add(fig);
            }
            Sort();
        }

        public void openVHDLFile(string filename, string EntityName)
        {
            if (System.IO.File.Exists(filename) == false)
            {
                return;
            }
            My_FileAnalyzer analyzer = new My_FileAnalyzer(filename, core);
            analyzer.Analyze(EntityName);
            info.Entity = analyzer.SelectedEntity;
            foreach (My_Figure fig in analyzer.figures)
            {
                FigureList.Add(fig);
            }
            Sort();
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            foreach (My_Figure figure in FigureList)
                figure.draw(sender, e);
            if ((core != null) && (core.ShowLayer == true))
                DrawLayer(e.Graphics);
        }

        private void DrawLayer(Graphics dc)
        {
            Brush brush = new SolidBrush(Color.DarkOrange);
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Rectangle rect = new Rectangle();
            rect.Width = 20;
            rect.Height = 20;
            System.Drawing.StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            foreach (My_Figure figure in FigureList)
            {
                rect.Location = new Point(figure.CenterPoint.X-10, figure.CenterPoint.Y-10);
                dc.FillEllipse(brush, rect);
                dc.DrawEllipse(new Pen(Color.Black), rect);
                dc.DrawString(figure.zIndex.ToString(), new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect, format);
            }
        }

        public My_Figure GetSelectedFigure(Point pt)
        {
            for (int i = FigureList.Count -1 ; i >= 0; i--)
            {
                My_Figure figure = FigureList[i];
                bool res = figure.IsSelected(pt);
                if (res == true)
                    return figure;

                if (figure is My_Port)
                {
                    res = (figure as My_Port).TextLabel.IsSelected(pt);
                    if (res == true)
                        return (figure as My_Port).TextLabel;
                }
            }
            return null;
        }

        public List<My_Figure> GetSelectedFigure(Rectangle rect)
        {
            List<My_Figure> res = new List<My_Figure>();
            foreach (My_Figure figure in FigureList)
            {
                if (figure.IsSelected(rect))
                    res.Add(figure);
            }
            return res;
        }

        public Point GetNearestPoint(My_Figure figure, Point pt)
        {
            Point res = pt;
            int? Min_Distance = null;
            foreach (My_Figure fig in FigureList)
            {
                if (fig.Equals(figure))
                    continue;
                if (fig is My_Path)
                    continue;

                foreach (Point p in fig.Points)
                {
                    int Distance = ((p.X - pt.X) * (p.X - pt.X) + (p.Y - pt.Y) * (p.Y - pt.Y));
                    if((Min_Distance == null) || (Min_Distance > Distance))
                    {
                        Min_Distance = Distance;
                        res = p;
                    }
                }
            }
            if (Min_Distance >= 100)
                res = pt;
            return res;
        }
    }
}