using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Schematix.FSM;


namespace Schematix.FSM
{
    public partial class Constructor_UserControl : UserControl
    {
        public Schematix.FSM.Constructor_Core core;
        
        IntPtr nextClipboardViewer;

        public Constructor_UserControl()
        {
            InitializeComponent();
            core = new Schematix.FSM.Constructor_Core(this);

            core.ChangeScale(100);
            core.mode = Schematix.FSM.FSM_MODES.MODE_SELECT;
            core.Bitmap.ArrowPaint = true;

            this.MouseDown += core.Constructor_MouseDown;
            this.MouseMove += core.Constructor_MouseMove;
            this.MouseUp += core.Constructor_MouseUp;
            this.Paint += core.Constructor_Paint;
            this.KeyDown += core.Constructor_KeyDown;
            this.KeyUp += core.Constructor_KeyUp;
            this.DragEnter += new DragEventHandler(core.Constructor_UserControl_DragEnter);

            //Нужно для буфера обмена
            nextClipboardViewer = (IntPtr)User32.SetClipboardViewer(this.Handle);

            //Загрузка настроек по-умолчанию
            core.Paper.BGColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.FSMOptions.BGColor;
            core.Paper.LineColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.FSMOptions.BorderColor;
            core.Paper.DrawBorder = Schematix.CommonProperties.Configuration.CurrentConfiguration.FSMOptions.ShowBorder;
            core.Paper.DrawGrig = Schematix.CommonProperties.Configuration.CurrentConfiguration.FSMOptions.ShowGrid;
            My_Figure.SelectedColor = Schematix.CommonProperties.Configuration.CurrentConfiguration.FSMOptions.SelectColor;
        }


        


        private void Constructor_Resize(object sender, EventArgs e)
        {
            core.Paper.ChangeScroll();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.ShowFigureProperties();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.Graph.DeleteFigure();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaperProperties paper_prop = new PaperProperties(core, core.Paper);
            paper_prop.ShowDialog();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void Constructor_DoubleClick(object sender, EventArgs e)
        {
            core.ShowFigureProperties();
        }
    }
}
