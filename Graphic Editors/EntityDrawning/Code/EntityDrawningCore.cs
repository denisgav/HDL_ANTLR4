using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public partial class EntityDrawningCore
    {        
        private EntityDrawningForm form;
        public EntityDrawningForm Form
        {
            get { return form; }
        }

        private My_History history;
        public My_History History
        {
            get { return history; }
        }

        private My_Picture picture;
        public My_Picture Picture
        {
            get { return picture; }
        }

        private List<My_Figure> selectedFigures;
        public List<My_Figure> SelectedFigures
        {
            get { return selectedFigures; }
        }

        private My_Figure selectedFigure;
        public My_Figure SelectedFigure
        {
            get { return selectedFigure; }
            set { selectedFigure = value; }
        }

        private GroupSelector group_selector;

        private My_Paper paper;
        public My_Paper Paper
        {
            get { return paper; }
        }

        private bool _lock;
        public bool Lock
        {
            get { return _lock; }
            set { _lock = value; }
        }        

        public delegate void UpdateHistoryDelegate();
        public event UpdateHistoryDelegate UpdateHistory;

        public KeyEventArgs KeyDown_ = null;

        public static Color SelectedColor = Color.Aqua;
        public bool ShowLayer = true;

        public EntityDrawningCore(EntityDrawningForm form)
        {
            this.form = form;
            Lock = false;
            picture = new My_Picture(this);
            history = new My_History(this);
            selectedFigures = new List<My_Figure>();
            group_selector = new GroupSelector(this);
            paper = new My_Paper(this);

            //Загрузка настроек по-умолчанию
            paper.BGColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.EntityDrawningOptions.BGColor;
            paper.LineColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.EntityDrawningOptions.BorderColor;
            paper.DrawBorder = Schematix.CommonProperties.Configuration.CurrentConfiguration.EntityDrawningOptions.ShowBorder;
            paper.DrawGrig = Schematix.CommonProperties.Configuration.CurrentConfiguration.EntityDrawningOptions.ShowGrid;
            SelectedColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.EntityDrawningOptions.SelectColor;

            UpdateHistory += new UpdateHistoryDelegate(EntityDrawningCore_UpdateHistory);
        }

        void EntityDrawningCore_UpdateHistory()
        {
            
        }

        public void SelectAll()
        {
            SelectedFigures.Clear();
            SelectedFigure = null;
            foreach (My_Figure fig in picture.FigureList)
            {
                SelectedFigures.Add(fig);
                fig.selected = true;
            }
        }

        public void UnselectAll()
        {
            SelectedFigures.Clear();
            SelectedFigure = null;
            foreach(My_Figure fig in picture.FigureList)
                fig.selected = false;
        }

        public bool CanUnDo()
        {
            return history.canUndo();
        }

        public bool CanReDo()
        {
            return history.canRedo();
        }

        public void CreateNewRectangle()
        {
            My_Rectangle rectangle = new My_Rectangle(this);
            UnselectAll();
            SelectedFigure = rectangle;
            SelectedFigures.Add(rectangle);
            picture.FigureList.Add(rectangle);
            Lock = true;
        }

        public void CreateNewEllipse()
        {
            My_Ellipse ellipse = new My_Ellipse(this);
            UnselectAll();
            SelectedFigure = ellipse;
            SelectedFigures.Add(ellipse);
            picture.FigureList.Add(ellipse);
            Lock = true;
        }

        public void CreateNewLine()
        {
            My_Line line = new My_Line(this);
            UnselectAll();
            SelectedFigure = line;
            SelectedFigures.Add(line);
            picture.FigureList.Add(line);
            Lock = true;
        }

        public void CreateNewPolygon()
        {
            My_Polygon polygon = new My_Polygon(this);
            UnselectAll();
            SelectedFigure = polygon;
            SelectedFigures.Add(polygon);
            picture.FigureList.Add(polygon);
            Lock = true;
        }

        public void CreateNewSplineBezier()
        {
            My_SplineBezier spline = new My_SplineBezier(this);
            UnselectAll();
            SelectedFigure = spline;
            SelectedFigures.Add(spline);
            picture.FigureList.Add(spline);
            Lock = true;
        }

        public void CreateNewCurve()
        {
            My_Curve curve = new My_Curve(this);
            UnselectAll();
            SelectedFigure = curve;
            SelectedFigures.Add(curve);
            picture.FigureList.Add(curve);
            Lock = true;
        }

        public void CreateNewPath()
        {
            My_Path path = new My_Path(this);
            UnselectAll();
            SelectedFigure = path;
            SelectedFigures.Add(path);
            picture.FigureList.Add(path);
            Lock = true;
        }

        public void CreateNewText()
        {
            My_Text text = new My_Text(this);
            UnselectAll();
            SelectedFigure = text;
            SelectedFigures.Add(text);
            picture.FigureList.Add(text);
            Lock = true;
        }

        public void CreateNewPolyline()
        {
            My_Polyline polyline = new My_Polyline(this);
            UnselectAll();
            SelectedFigure = polyline;
            SelectedFigures.Add(polyline);
            picture.FigureList.Add(polyline);
            Lock = true;
        }

        public void CreateNewImage()
        {
            DialogResult res = form.openFileDialogImage.ShowDialog();
            if (res != DialogResult.OK)
                return;
            string filename = form.openFileDialogImage.FileName;
            CreateNewImage(filename);
        }

        public void CreateNewImage(string filename)
        {
            Bitmap bitmap = Bitmap.FromFile(filename) as Bitmap;
            My_Image img = new My_Image(this, bitmap);
            UnselectAll();
            SelectedFigure = img;
            SelectedFigures.Add(img);
            picture.FigureList.Add(img);
            Lock = true;
        }

        public void CreateNewArc()
        {
            My_Arc arc = new My_Arc(this);
            UnselectAll();
            SelectedFigure = arc;
            SelectedFigures.Add(arc);
            picture.FigureList.Add(arc);
            Lock = true;
        }

        public void CreateNewPie()
        {
            My_Pie pie = new My_Pie(this);
            UnselectAll();
            SelectedFigure = pie;
            SelectedFigures.Add(pie);
            picture.FigureList.Add(pie);
            Lock = true;
        }

        public void CreateNewPort(My_Port.PortType type, bool Inverse)
        {
            My_Port p = new My_Port(this, type, Inverse);
            UnselectAll();
            SelectedFigure = p;
            SelectedFigures.Add(p);
            picture.FigureList.Add(p);
            Lock = true;
        }

        public void CreateNewFigureDragged(string Figure)
        {
            switch (Figure)
            {
                case "CreateAsynchronousPort":
                    CreateNewPort(My_Port.PortType.Asynchronous, false);
                break;

                case "CreateInversePort":
                    CreateNewPort(My_Port.PortType.Simple, true);
                break;

                case "CreateInverseAsynchronousPort":
                    CreateNewPort(My_Port.PortType.Asynchronous, true);
                break;

                case "CreateInverseSynchronousPort":
                    CreateNewPort(My_Port.PortType.Simultaneous, true);
                break;

                case "CreateSimplePort":
                    CreateNewPort(My_Port.PortType.Simple, false);
                break;

                case "CreateSynchronousPort":
                    CreateNewPort(My_Port.PortType.Simultaneous, false);
                break;

                case "CreateArc":
                    CreateNewArc();
                break;

                case "CreateBezier":
                    CreateNewSplineBezier();
                break;

                case "CreateCurve":
                    CreateNewCurve();
                break;

                case "CreateEllipse":
                    CreateNewEllipse();
                break;

                case "CreateImage":
                    CreateNewImage();
                break;

                case "CreateLine":
                    CreateNewLine();
                break;

                case "CreatePath":
                    CreateNewPath();
                break;

                case "CreatePie":
                    CreateNewPie();
                break;

                case "CreatePolygon":
                    CreateNewPolygon();
                break;

                case "CreatePolyline":
                    CreateNewPolyline();
                break;

                case "CreateRectangle":
                    CreateNewRectangle();
                break;

                case "CreateText":
                    CreateNewText();
                break;

                default:
                return;
            }
        }

        public void ShowBrushProperties()
        {
            BrushProperies br = new BrushProperies(SelectedFigure, this);
            br.ShowDialog();
            AddToHistory();
        }

        public void ShowPenProperties()
        {
            PenProperties penprop = new PenProperties(SelectedFigure, this);
            penprop.ShowDialog();
            AddToHistory();
        }

        public void ShowTextproperties()
        {
            if(SelectedFigure is My_Text == false)
                return;
            My_Text text = SelectedFigure as My_Text;
            TextProperties textprop = new TextProperties(text, this);
            textprop.ShowDialog();
            AddToHistory();
        }

        public void ShowPortProperties()
        {
            if (SelectedFigure is My_Port == false)
                return;
            My_Port port = SelectedFigure as My_Port;
            PortProperties port_prop = new PortProperties(this, port);
            port_prop.ShowDialog();
            AddToHistory();
        }

        public void DeleteFigure()
        {
            foreach (My_Figure fig in SelectedFigures)
                picture.FigureList.Remove(fig);
            AddToHistory();
            form.Invalidate();
        }

        public void BringToFront()
        {
            foreach (My_Figure figure in SelectedFigures)
            {
                figure.zIndex++;
            }
            picture.Sort();
            form.Invalidate();
        }

        public void BringToBack()
        {
            foreach (My_Figure figure in SelectedFigures)
            {
                figure.zIndex--;
            }
            picture.Sort();
            form.Invalidate();
        }

        public void AddToHistory()
        {
            history.add(picture);
            UpdateHistory();
        }

        public void UnDo()
        {
            My_Picture pict = history.Undo();
            if (pict != null)
                picture = pict;
            UnselectAll();
            form.Invalidate();
        }

        public void ReDo()
        {
            My_Picture pict = history.ReDo();
            if (pict != null)
                picture = pict;
            UnselectAll();
            form.Invalidate();
        }

        public void ShowPaperProperties()
        {
            PaperProperties paper_prop = new PaperProperties(this, paper);
            paper_prop.ShowDialog();
        }

        public void UpdateScale(int scale)
        {
            paper.scale = (double)100 / (double)scale;
            paper.UpdateScale();

            form.Invalidate();
        }

        #region Clipboard
        private DataFormats.Format clipboard_format = DataFormats.GetFormat(typeof(List<EntityDrawning.My_Figure>).FullName);
        public void CutToClipboard()
        {
            CopyToClipboard();

            //Удаляем все лишнее
            DeleteFigure();
            AddToHistory();
            form.Invalidate();
        }

        public void CopyToClipboard()
        {
            // регистрируем свой собственный формат данных либо получаем его, если он уже зарегистрирован
            List<My_Figure> copy_figure = new List<My_Figure>();
            foreach (My_Figure figure in SelectedFigures)
            {
                copy_figure.Add(figure);
            }

            // копируем в буфер обмена
            IDataObject dataObj = new DataObject();
            dataObj.SetData(My_Picture.Serialize(copy_figure));
            Clipboard.SetDataObject(dataObj, false);
        }

        public void GetFromClipboard()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            List<My_Figure> pasted_figures = new List<My_Figure>();
            if (dataObj.GetDataPresent(typeof(byte[])))
            {
                byte[] data = dataObj.GetData(typeof(byte[])) as byte[];
                pasted_figures = My_Picture.Deserialize(data);
                if (pasted_figures == null)
                    return;
            }
            foreach (My_Figure figure in pasted_figures)
            {
                figure.Core = this;
                picture.FigureList.Add(figure);
                SelectedFigures.Add(figure);
                if (figure is My_Port)
                {
                    (figure as My_Port).TextLabel.Core = this;
                }
            }

            AddToHistory();
            form.Invalidate();
        }

        public bool CanPaste()
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            bool res = dataObj.GetDataPresent(typeof(byte[]));
            //bool res = Clipboard.ContainsData(clipboard_format.Name);

            if (res == true)
            {
                List<My_Figure> list = dataObj.GetData(clipboard_format.Name) as List<My_Figure>;
                dataObj.SetData(clipboard_format.Name, list);
            }
            return res;
        }

        public bool CanCopyCut()
        {
            return ((SelectedFigure != null) || (SelectedFigures.Count != 0));
        }
        #endregion
    }

    public partial class EntityDrawningCore
    {
        public void MouseMove(object sender, MouseEventArgs e)
        {
            Point p = paper.ConvertToBitmapCoordinate(e);
            if (Lock == true)
            {
                MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
                if (group_selector.active == true)
                {
                    group_selector.MouseMove(sender, newE);
                    return;
                }

                foreach (My_Figure figure in SelectedFigures)
                    figure.mouse_move(sender, newE);
            }
            else
            {
                My_Figure fig = picture.GetSelectedFigure(new Point(p.X, p.Y));
                if (fig != null)
                {
                    if (SelectedFigure != null)
                    {
                        SelectedFigure.selected = false;
                        fig.selected = true;
                        SelectedFigure = fig;
                        form.Invalidate();
                    }
                    else
                    {
                        SelectedFigure = fig;
                        SelectedFigure.selected = true;
                        form.Invalidate();
                    }
                }
                else
                {
                    if (SelectedFigure != null)
                    {
                        SelectedFigure.selected = false;
                        SelectedFigure = null;
                        form.Invalidate();
                    }
                }

            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {           
            Point p = paper.ConvertToBitmapCoordinate(e);
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
            if (group_selector.active == true)
            {
                group_selector.MouseUp(sender, newE);
                Lock = false;
                return;
            }
            if (CanCopyCut())
            {
                if (Lock == true)
                    Lock = false;
                foreach (My_Figure figure in SelectedFigures)
                    figure.mouse_up(sender, newE);
                AddToHistory();
            }
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (Lock == false)
                Click_();

            Point p = paper.ConvertToBitmapCoordinate(e);
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
            if (SelectedFigure == null)
            {
                if (e.Button == MouseButtons.Right) // показываем меню рабочей области
                {
                    form.contextMenuStripPaper.Show(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
                else // активируем класс для групового выделения
                {
                    Lock = true;
                    group_selector.active = true;
                    group_selector.MouseDown(sender, newE);
                }
            }

            foreach (My_Figure figure in SelectedFigures)
                figure.mouse_down(sender, newE);
            if (Lock == false)
                Lock = true;
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            if ((form.hScrollBar1 != null) && (form.vScrollBar1 != null))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Graphics dc = e.Graphics;
                dc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                dc.TranslateTransform(-form.hScrollBar1.Value + paper.ClientStartPoint.X, -form.vScrollBar1.Value + paper.ClientStartPoint.Y);
                dc.BeginContainer(new Rectangle(0, 0, picture.MaxSize.Width, picture.MaxSize.Height), new Rectangle(0, 0, (int)(picture.MaxSize.Width * paper.scale), (int)(picture.MaxSize.Height * paper.scale)), GraphicsUnit.Pixel);

                paper.Paint(sender, e);
                picture.Draw(sender, e);
                if (group_selector.active == true)
                {
                    group_selector.Draw(sender, e);
                }
            }
        }

        private void Click_()
        {
            if (KeyDown_ != null)
            {
                if (KeyDown_.Shift == true)
                {
                    if (SelectedFigure != null)
                    {
                        if (SelectedFigures.Contains(SelectedFigure) == true)
                        {
                            SelectedFigures.Remove(SelectedFigure);
                        }
                        else
                        {
                            SelectedFigures.Add(SelectedFigure);
                            form.Invalidate();
                        }
                    }
                }
            }
            else
            {
                if (SelectedFigure != null)
                {
                    if (SelectedFigures.Contains(SelectedFigure) == false)
                    {
                        My_Figure[] array = new My_Figure[SelectedFigures.Count];
                        SelectedFigures.CopyTo(array);
                        SelectedFigures.Clear();
                        foreach (My_Figure figure in array)
                        {
                            figure.selected = false;
                        }
                        SelectedFigures.Add(SelectedFigure);
                        form.Invalidate();
                    }
                }
                else
                {
                    My_Figure[] array = new My_Figure[SelectedFigures.Count];
                    SelectedFigures.CopyTo(array);
                    SelectedFigures.Clear();
                    foreach (My_Figure figure in array)
                    {
                        figure.selected = false;
                    }
                    form.Invalidate();
                }
            }
        }        

        public void KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown_ = e;
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectedFigure is My_Polygon)
                {
                    (SelectedFigure as My_Polygon).Fix();
                    Lock = false;
                }

                if (SelectedFigure is My_Path)
                {
                    (SelectedFigure as My_Path).Fix();
                    Lock = false;
                }

                if (SelectedFigure is My_Polyline)
                {
                    (SelectedFigure as My_Polyline).Fix();
                    Lock = false;
                }

                if (SelectedFigure is My_SplineBezier)
                {
                    (SelectedFigure as My_SplineBezier).Fix();
                }

                if (SelectedFigure is My_Curve)
                {
                    (SelectedFigure as My_Curve).Fix();
                    Lock = false;
                }
            }

            if (KeyDown_ != null)
            {
                if (KeyDown_.Control == true)
                {
                    if (e.KeyCode == Keys.C)
                    {
                        CopyToClipboard();
                    }
                    if (e.KeyCode == Keys.V)
                    {
                        GetFromClipboard();
                    }
                    if (e.KeyCode == Keys.X)
                    {
                        CutToClipboard();
                    }
                }
            }
            KeyDown_ = e;
            if (e.KeyCode == Keys.Delete)
            {
                DeleteFigure();
            }
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            KeyDown_ = null;
        }
    }
}