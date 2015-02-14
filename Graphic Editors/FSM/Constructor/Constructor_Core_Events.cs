using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Schematix.FSM
{
    partial class Constructor_Core
    {
        private KeyEventArgs KeyDown_ = null;

        public void Constructor_Paint(object sender, PaintEventArgs e)
        {
            //try
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Graphics dc = e.Graphics;
                dc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                dc.TranslateTransform(-form.hScrollBar1.Value + paper.ClientStartPoint.X, -form.vScrollBar1.Value + paper.ClientStartPoint.Y);
                dc.BeginContainer(new Rectangle(0, 0, form.ClientRectangle.Width, form.ClientRectangle.Height), new Rectangle(0, 0, (int)(form.ClientRectangle.Width * paper.scale), (int)(form.ClientRectangle.Height * paper.scale)), GraphicsUnit.Pixel);

                paper.Paint(sender, e);

                //dc.DrawImage(Bitmap.bitmap, 0, 0);

                if (group_selector.active == true)
                    group_selector.Draw(sender, e);

                if (graph.Reset != null)
                {
                    graph.Reset.draw(sender, e);
                }
                foreach (Schematix.FSM.My_Figure fig in graph.Figures)
                {
                    fig.draw(sender, e);
                }                
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Fatal Error :) (Paint)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
        }

        private void Click_()
        {
            if (KeyDown_ != null)
            {
                if (KeyDown_.Shift == true)
                {
                    if (SelectedFigure != null)
                    {
                        if (SelectedFigureList.Contains(SelectedFigure) == true)
                        {
                            SelectedFigureList.Remove(SelectedFigure);
                        }
                        else
                        {
                            SelectedFigureList.Add(SelectedFigure);
                            form.Invalidate();
                        }
                    }
                }
            }
            else
            {
                if (SelectedFigure != null)
                {
                    if (SelectedFigureList.Contains(SelectedFigure) == false)
                    {
                        graph.UnselectAllFigures();
                        SelectedFigureList.Add(SelectedFigure);
                        form.Invalidate();
                    }
                }
                else
                {
                    graph.UnselectAllFigures();
                    form.Invalidate();
                }
            }
        }

        public void Constructor_MouseMove(object sender, MouseEventArgs e)
        {
            try
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
                    foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
                    {
                        figure.mouse_move(sender, newE);
                    }
                }
                else
                {
                    Schematix.FSM.My_Figure fig = bitmap.SelectElem(p);
                    if (fig != null)
                    {
                        if (SelectedFigure != null)
                        {
                            SelectedFigure.Selected = false;
                            fig.Selected = true;
                            SelectedFigure.Select(bitmap.SelectedColor);
                            SelectedFigure = fig;
                            form.Invalidate();
                        }
                        else
                        {
                            SelectedFigure = fig;
                            SelectedFigure.Selected = true;
                            SelectedFigure.Select(bitmap.SelectedColor);
                            form.Invalidate();
                        }
                    }
                    else
                    {
                        if (SelectedFigure != null)
                        {
                            SelectedFigure.Selected = false;
                            SelectedFigure = null;
                            form.Invalidate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error :) (MouseMove)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void Constructor_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ExecuteOperation();
                Click_();

                Point p = paper.ConvertToBitmapCoordinate(e);
                MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);

                if (SelectedFigure == null)
                {
                    if (e.Button == MouseButtons.Right) // показываем менб рабочей области
                    {
                        form.contextMenuStripPaper.Show(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    }
                    else // активируем класс для групового выделения
                    {
                        Lock = true;
                        group_selector.active = true;
                        group_selector.MouseDown(sender, newE);
                    }
                    return;
                }

                Lock = true;
                SelectedFigure.Selected = true;

                foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
                {
                    figure.mouse_down(sender, newE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error :) (MouseDown)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void Constructor_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                CanCopyCut();
                Point p = paper.ConvertToBitmapCoordinate(e);
                MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
                if (group_selector.active == true)
                {
                    group_selector.MouseUp(sender, newE);
                    return;
                }

                if (SelectedFigure == null)
                    return;

                Lock = false;
                SelectedFigure.Selected = true;
                foreach (Schematix.FSM.My_Figure figure in SelectedFigureList)
                {
                    figure.mouse_up(sender, newE);
                }
                if (SelectedFigureList.Count != 0)
                    AddToHistory("Figure moved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error :) (MouseUp)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void Constructor_KeyDown(object sender, KeyEventArgs e)
        {
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
                graph.DeleteFigure();
            }
        }

        public void Constructor_KeyUp(object sender, KeyEventArgs e)
        {
            KeyDown_ = null;
        }

        public void Constructor_UserControl_DragEnter(object sender, DragEventArgs e)
        {
            String command = e.Data.GetData("UnicodeText") as String;
            switch (command)
            {
                case "CreateConstant":
                    CreateNewConstant_Dragged(new Point(e.X, e.Y));
                    break;

                case "CreateReset":
                    CreateReset();
                    break;

                case "CreateSignal":
                    CreateNewSignal_Dragged(new Point(e.X, e.Y));
                    break;

                case "CreateState":
                    CreateNewState_Dragged(new Rectangle(new Point(e.X - 20, e.Y - 20), new Size(40, 40)));
                    break;

                case "CreateComment":
                    CreateNewComment_Dragged(new Rectangle(new Point(e.X - 20, e.Y - 20), new Size(40, 40)));
                    break;

                case "CreateTransition":
                    CreateNewTransitionMode();
                    break;

                case "CreateBidirectionalPort":
                    CreateNewPort_Dragged(My_Port.PortDirection.InOut, new Point(e.X, e.Y));
                    break;

                case "CreateInputPort":
                    CreateNewPort_Dragged(My_Port.PortDirection.In, new Point(e.X, e.Y));
                    break;

                case "CreateOutputPort":
                    CreateNewPort_Dragged(My_Port.PortDirection.Out, new Point(e.X, e.Y));
                    break;

                case "CreateBufferPort":
                    CreateNewPort_Dragged(My_Port.PortDirection.Buffer, new Point(e.X, e.Y));
                    break;

                default:
                    break;
            }
            mode = FSM_MODES.MODE_SELECT;
        }
    }
}
