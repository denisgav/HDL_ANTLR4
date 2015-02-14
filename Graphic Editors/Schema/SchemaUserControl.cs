using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using csx;
using Schematix.Windows.MDIChild;

namespace Schematix_all
{
    public partial class SchemaUserControl : UserControl
    {
        public Rectangle ModelBorder;
        public int ScrollDx = 0;
        public int ScrollDy = 0;

        public Schematix.Windows.MDIChild.File file;
        public History history;

        private Scale scale;
        public Scale Scale
        {
            get { return scale; }
        }

        public SchemaUserControl()
        {
            InitializeComponent();

            file = new Schematix.Windows.MDIChild.File(this);
            history = new History(this);

            this.model = new csx.Construct(this);
            // 
            // tip
            // 
            this.tip = new System.Windows.Forms.ToolTip();
            this.tip.AutomaticDelay = 0;
            this.tip.UseAnimation = false;
            this.tip.UseFading = false;
            this.tip.UseAnimation = true;
            this.tip.Popup += new PopupEventHandler(tip_Popup);

            scale = new Scale(this, model);
        }

        void tip_Popup(object sender, PopupEventArgs e)
        {
            popTimer.Start();
        }

        private csx.Construct model;
        public System.Windows.Forms.ToolTip tip;

        private string exportVHDL()
        {
            return model.export(Path.GetFileNameWithoutExtension(this.file.name));
        }

        public void importVHDL()
        {
            OpenFileDialog openImportFile = new OpenFileDialog();
            DialogResult result = openImportFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                csx.Parser ps = new csx.Parser();
                ps.Parsing(openImportFile.FileName);
                csx.ImportSelect IS = new csx.ImportSelect(ps.entities, @model);

                tip.Hide(this);

                IS.ShowDialog();
                this.Invalidate();
            }
        }

        public void addSignal()
        {
            csx.addSignal aS = new csx.addSignal(@model);

            tip.Hide(this);

            aS.ShowDialog();
            Invalidate();
        }

        public void addExternPort()
        {
            csx.addExternPort aEP = new csx.addExternPort(@model);

            tip.Hide(this);

            aEP.ShowDialog();
            Invalidate();
        }

        public void addExternPorts(List<Port> ports)
        {
            csx.addExternPort aEP = new csx.addExternPort(@model);

            for (int i = 0; i < ports.Count; ++i)
            {
                aEP.leftBound.Value = ports[i].LeftBusBound;
                aEP.rightBound.Value = ports[i].RightBusBound;
                aEP.portName.Text = ports[i].name;
                switch (ports[i].inout)
                {
                    case portInOut.In:
                        aEP.In.Checked = true;
                        break;
                    case portInOut.Out:
                        aEP.Out.Checked = true;
                        break;
                    case portInOut.InOut:
                        aEP.InOut.Checked = true;
                        break;
                }
                aEP.AddPort();
            }
        }

        public void GenerateCode(string fileName)
        {
            string result = this.exportVHDL();

            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(result);
            writer.Close();
            
        }

        private Rectangle returnScrollRect()
        {
            Rectangle rect = new Rectangle();
            rect.X = Math.Min(ModelBorder.X, ScrollDx) - 20;
            rect.Y = Math.Min(ModelBorder.Y, ScrollDy) - 20;
            rect.Width = Math.Max(ModelBorder.Right, ScrollDx + this.Width - 85) - rect.X - 40;
            rect.Height = Math.Max(ModelBorder.Bottom, ScrollDy + this.Height - 85) - rect.Y;
            return rect;
        }

        private void schema_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            dc.TranslateTransform(-hScrollBar1.Value + scale.ClientStartPoint.X, -vScrollBar1.Value + scale.ClientStartPoint.Y);
            Size MaxSize = scale.GetMaxSize();
            dc.BeginContainer(new Rectangle(0, 0, MaxSize.Width, MaxSize.Height), new Rectangle(0, 0, (int)(MaxSize.Width * scale.scale), (int)(MaxSize.Height * scale.scale)), GraphicsUnit.Pixel);
            RectangleF buf = model.events.onDraw(e.Graphics);
            ModelBorder = new Rectangle((int)buf.X - 20, (int)buf.Y - 20, (int)buf.Width, (int)buf.Height);
        }

        private void schema_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = scale.ConvertToBitmapCoordinate(e);
            model.events.onMouseMove(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
        }

        private void schema_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = scale.ConvertToBitmapCoordinate(e);
            model.events.onMouseDown(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
        }

        private void schema_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = scale.ConvertToBitmapCoordinate(e);
            model.events.onMouseUp(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
        }

        private void schema_KeyDown(object sender, KeyEventArgs e)
        {
            model.events.onKeyDown(e);
        }

        private void schema_KeyUp(object sender, KeyEventArgs e)
        {
            model.events.onKeyUp(e);
        }

        private void Schema_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                ScrollDx = e.NewValue + ModelBorder.X;// +ScrollMinX;
                HorizontalScroll.Value = e.NewValue;
            }
            else
            {
                ScrollDy = e.NewValue + ModelBorder.Y;// + ScrollMinY;
                VerticalScroll.Value = e.NewValue;
            }
            Rectangle scrollRect = returnScrollRect();
            this.AutoScrollMargin = new Size(scrollRect.Width, scrollRect.Height);
            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            model.events.ExecuteTip();
        }

        private void popTimer_Tick(object sender, EventArgs e)
        {
            tip.Hide(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics gr = this.CreateGraphics();
            int i, j;
            for (i = 0; i < model.elements.Count; i++)
                for (j = 0; j < model.elements[i].ports.Count; j++)
                    if (model.elements[i].ports[j].isLine)
                        if (!model.lines.Contains(model.elements[i].ports[j].line))
                        {
                            gr.DrawLines(Pens.Red, model.elements[i].ports[j].line.points);
                        }

        }

        //Save window state
        public bool Save(System.IO.Stream stream)
        {
            if (model != null)
            {
                try
                {
                    this.model.save(stream);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        //Open window state
        public bool Open(System.IO.Stream stream)
        {
            this.model.open(stream);
            return true;
        }

        //Open window state
        public bool Open(string fileName)
        {
            if (System.IO.File.Exists(fileName) == true)
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                this.model.open(stream);
                stream.Close();
            }
            return true;
        }

        public bool CanCopy()
        {
            return model.buffer.Count > 0;
        }

        public bool Copy()
        {
            model.buffer.CheckElementListChanged();
            model.buffer.CloneIntoBuffer(Actions.Buffer);
            return true;
        }

        public bool CanCut()
        {
            return model.buffer.Count > 0;
        }

        public bool Cut()
        {
            model.buffer.CheckElementListChanged();
            model.buffer.CloneIntoBuffer(Actions.Buffer);
            model.buffer.Delete(model);
            MouseEventArgs mea = new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0);
            model.events.onMouseMove(mea);
            this.Invalidate();
            return false;
        }

        public bool CanPaste()
        {
            return Actions.Buffer.Count > 0;
        }

        public bool Paste()
        {
            Actions.Buffer.CloneIntoBuffer(model.buffer);
            model.buffer.Paste(new PointF(100, 100), model);
            this.Invalidate();
            return false;
        }

        public bool CanDelete()
        {
            return model.buffer.Count > 0 || model.events.isNearLine;;
        }

        public bool Delete()
        {
            if (model.events.isNearLine)
            {
                model.events.DeleteNearLine();
                this.Invalidate();
            }
            else if (model.buffer.Count > 0)
            {
                model.buffer.Delete(model);
                MouseEventArgs mea = new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0);
                model.events.onMouseMove(mea);
                this.Invalidate();
            }
            return true;
        }

        public bool CanSelectAll()
        {
            return model.elements.Count > 0 || model.connects.Count > 0 || model.lines.Count > 0;
        }

        public bool SelectAll()
        {
            model.selectAll();
            Invalidate();
            return true;
        }
        
        public void RegisterSave()
        {
            this.history.RegisterSave();
        }

        public bool CanSave()
        {
            return history.isModified;
        }

        public bool Save()
        {
            return file.Save();
        }

        private void m_AddSignal_Click(object sender, EventArgs e)
        {
            this.addSignal();
        }

        private void m_AddPort_Click(object sender, EventArgs e)
        {
            this.addExternPort();
        }

        private void m_ImportElements_Click(object sender, EventArgs e)
        {
            this.importVHDL();
        }

        private void m_GenerateCode_Click(object sender, EventArgs e)
        {
            
        }

        private void Schema_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UnicodeText") == true)
            {
                String command = e.Data.GetData("UnicodeText") as String;
                AddEntityDrawningElement(command, new Point(e.X, e.Y));
            }
        }

        public void SetMode(string Mode)
        {
            AddEntityDrawningElement(Mode, new Point(0, 0));
        }

        private void AddEntityDrawningElement(string command, Point point)
        {
            Schematix.EntityDrawning.My_Picture picture = new Schematix.EntityDrawning.My_Picture();
            picture.openProject(command);
            Graphics gr = CreateGraphics();
            System.Drawing.Imaging.Metafile mf = picture.GetMetaFile(gr);
            gr.Dispose();
            Element el = new Element(mf, model, picture.info.Entity.name, ElementType.Element);
            el.border = new RectangleF(new PointF(point.X - this.Location.X, point.Y - this.Location.Y), picture.PictureRectangle.Size);

            List<Schematix.EntityDrawning.My_Port> ports = new List<Schematix.EntityDrawning.My_Port>();
            Rectangle PictRectangle = picture.PictureRectangle;
            foreach (Schematix.EntityDrawning.My_Figure fig in picture.FigureList)
            {
                if (fig is Schematix.EntityDrawning.My_Port)
                    ports.Add(fig as Schematix.EntityDrawning.My_Port);
            }
            foreach (Schematix.EntityDrawning.My_Port p in ports)
            {
                Port pt = new Port(p.vhdPort.inout, p.vhdPort.type, 0, p.Name, p.vhdPort.leftBound, p.vhdPort.rightBound);
                //napr = 0; //направление отвода линии: 0-влево, 1-вверх, 2-вправо, 3-вниз
                pt.location = new PointF(p.Points[0].X - PictRectangle.X, p.Points[0].Y - PictRectangle.Y);
                if (p.Points[0].X < p.Points[1].X)
                    pt.napr = 2;
                else
                    pt.napr = 0;
                el.Add(pt);
            }

            model.AddElement(el);
            model.parent.history.Changed();
        }

        public bool SetZoom(int zoom)
        {
            try
            {
                if ((zoom > 10) && (zoom <= 1000))
                {
                    scale.UpdateScale(zoom);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error :) (MouseMove)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void Schema_SizeChanged(object sender, EventArgs e)
        {
            scale.UpdateScale();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //ScrollDx = e.NewValue;
            Invalidate();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //ScrollDy = e.NewValue;
            Invalidate();
        }

        //Save window state to history
        public virtual bool SaveToHistory(System.IO.Stream stream)
        {
            return this.Save(stream);
        }

        //Open undo window state from history
        public virtual bool OpenUndoFromHistory(System.IO.Stream stream)
        {
            return this.Open(stream);
        }

        //Open redo window state from history
        public virtual bool OpenRedoFromHistory(System.IO.Stream stream)
        {
            return this.Open(stream);
        }
    }
}
