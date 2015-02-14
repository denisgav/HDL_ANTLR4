using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public partial class BrushProperies : Form
    {
        public My_Brush brush;
        private My_Figure figure;
        private EntityDrawningCore core;
        public enum BrushType
        {
            SolidBrush,
            HatchBrush,
            TextureBrush,
            LinearGradientBrush,
            PathGradientBrush
        };

        BrushType type;

        public BrushProperies(My_Figure figure, EntityDrawningCore core)
        {
            InitializeComponent();
            this.brush = figure.brush;
            this.figure = figure;
            this.core = core;
            LoadData();
        }

        private void LoadData()
        {
            comboBoxType.Items.Add(BrushType.HatchBrush);
            comboBoxType.Items.Add(BrushType.LinearGradientBrush);
            comboBoxType.Items.Add(BrushType.PathGradientBrush);
            comboBoxType.Items.Add(BrushType.SolidBrush);
            comboBoxType.Items.Add(BrushType.TextureBrush);

            switch (brush.GetType().Name)
            {
                case "My_SolidBrush":
                    type = BrushType.SolidBrush;
                    break;
                case "My_HatchBrush":
                    type = BrushType.HatchBrush;
                    break;
                case "My_TextureBrush":
                    type = BrushType.TextureBrush;
                    break;
                case "My_LinearGradientBrush":
                    type = BrushType.LinearGradientBrush;
                    break;
                case "My_PathGradientBrush":
                    type = BrushType.PathGradientBrush;
                    break;
                default:
                    break;
            }
            comboBoxType.SelectedItem = type;

            #region Solid Brush Settings
            if(brush is My_SolidBrush)
            {
                colorDialog1.Color = (brush as My_SolidBrush).Color;
                numericUpDownAlpha1.Value = colorDialog1.Color.A;
            }
            #endregion

            #region Hatsh Brush Settings
            comboBoxHatchBrushes.Items.Add(HatchStyle.BackwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Cross);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DarkDownwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DarkHorizontal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DarkUpwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DarkVertical);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DashedDownwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DashedHorizontal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DashedUpwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DashedVertical);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DiagonalBrick);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DiagonalCross);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Divot);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DottedDiamond);
            comboBoxHatchBrushes.Items.Add(HatchStyle.DottedGrid);
            comboBoxHatchBrushes.Items.Add(HatchStyle.ForwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Horizontal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.HorizontalBrick);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LargeCheckerBoard);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LargeConfetti);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LargeGrid);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LightDownwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LightHorizontal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LightUpwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.LightVertical);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Max);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Min);
            comboBoxHatchBrushes.Items.Add(HatchStyle.NarrowHorizontal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.NarrowVertical);
            comboBoxHatchBrushes.Items.Add(HatchStyle.OutlinedDiamond);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent05);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent10);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent20);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent25);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent30);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent40);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent50);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent60);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent70);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent75);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent80);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Percent90);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Plaid);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Shingle);
            comboBoxHatchBrushes.Items.Add(HatchStyle.SmallCheckerBoard);
            comboBoxHatchBrushes.Items.Add(HatchStyle.SmallConfetti);
            comboBoxHatchBrushes.Items.Add(HatchStyle.SmallGrid);
            comboBoxHatchBrushes.Items.Add(HatchStyle.SolidDiamond);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Sphere);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Trellis);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Vertical);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Wave);
            comboBoxHatchBrushes.Items.Add(HatchStyle.Weave);
            comboBoxHatchBrushes.Items.Add(HatchStyle.WideDownwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.WideUpwardDiagonal);
            comboBoxHatchBrushes.Items.Add(HatchStyle.ZigZag);

            if (brush is My_HatchBrush)
            {
                comboBoxHatchBrushes.SelectedItem = (brush as My_HatchBrush).HatchStyle;
                colorDialog1.Color = (brush as My_HatchBrush).Color1;
                colorDialog2.Color = (brush as My_HatchBrush).Color2;
                numericUpDownAlpha1.Value = colorDialog1.Color.A;
                numericUpDownAlpha2.Value = colorDialog2.Color.A;
            }
            else
                comboBoxHatchBrushes.SelectedIndex = 0;
            #endregion

            #region Gradient Brush Settings
            comboBoxGradientBrushes.Items.Add(LinearGradientMode.BackwardDiagonal);
            comboBoxGradientBrushes.Items.Add(LinearGradientMode.ForwardDiagonal);
            comboBoxGradientBrushes.Items.Add(LinearGradientMode.Horizontal);
            comboBoxGradientBrushes.Items.Add(LinearGradientMode.Vertical);
            comboBoxGradientBrushes.SelectedIndex = 1;

            if (brush is My_LinearGradientBrush)
            {
                comboBoxGradientBrushes.SelectedItem = (brush as My_LinearGradientBrush).LinearGradientMode;
                colorDialog1.Color = (brush as My_LinearGradientBrush).Color1;
                colorDialog2.Color = (brush as My_LinearGradientBrush).Color2;
                numericUpDownAlpha1.Value = colorDialog1.Color.A;
                numericUpDownAlpha2.Value = colorDialog2.Color.A;
            }
            #endregion

            #region Path Gradient Brush
            comboBoxWrapMode.Items.Add(WrapMode.Clamp);
            comboBoxWrapMode.Items.Add(WrapMode.Tile);
            comboBoxWrapMode.Items.Add(WrapMode.TileFlipX);
            comboBoxWrapMode.Items.Add(WrapMode.TileFlipXY);
            comboBoxWrapMode.Items.Add(WrapMode.TileFlipY);
            if (brush is My_PathGradientBrush)
            {
                comboBoxWrapMode.SelectedItem = (brush as My_PathGradientBrush).WrapMode;
                colorDialog1.Color = (brush as My_PathGradientBrush).Color1;
                colorDialog2.Color = (brush as My_PathGradientBrush).Color2;
                numericUpDownAlpha1.Value = colorDialog1.Color.A;
                numericUpDownAlpha2.Value = colorDialog2.Color.A;
            }
            else
                comboBoxWrapMode.SelectedIndex = 1;
            #endregion
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            Rectangle rect = pictureBoxPreview.ClientRectangle;
            dc.FillRectangle(brush, rect);
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrushType type = (BrushType)comboBoxType.SelectedItem;
            switch (type)
            {
                case BrushType.HatchBrush:
                    comboBoxHatchBrushes.Enabled = true;
                    comboBoxGradientBrushes.Enabled = false;
                    buttonChange1.Enabled = true;
                    buttonChange2.Enabled = true;
                    numericUpDownAlpha1.Enabled = true;
                    numericUpDownAlpha2.Enabled = true;
                    textBoxFile.Enabled = false;
                    buttonBrowse.Enabled = false;
                    comboBoxWrapMode.Enabled = false;
                    break;

                case BrushType.SolidBrush:
                    comboBoxHatchBrushes.Enabled = false;
                    comboBoxGradientBrushes.Enabled = false;
                    buttonChange1.Enabled = true;
                    buttonChange2.Enabled = false;
                    numericUpDownAlpha1.Enabled = true;
                    numericUpDownAlpha2.Enabled = false;
                    textBoxFile.Enabled = false;
                    buttonBrowse.Enabled = false;
                    comboBoxWrapMode.Enabled = false;
                    break;

                case BrushType.LinearGradientBrush:
                    comboBoxHatchBrushes.Enabled = false;
                    comboBoxGradientBrushes.Enabled = true;
                    buttonChange1.Enabled = true;
                    buttonChange2.Enabled = true;
                    numericUpDownAlpha1.Enabled = true;
                    numericUpDownAlpha2.Enabled = true;
                    textBoxFile.Enabled = false;
                    buttonBrowse.Enabled = false;
                    comboBoxWrapMode.Enabled = false;
                    break;

                case BrushType.PathGradientBrush:
                    comboBoxHatchBrushes.Enabled = false;
                    comboBoxGradientBrushes.Enabled = false;
                    buttonChange1.Enabled = true;
                    buttonChange2.Enabled = true;
                    numericUpDownAlpha1.Enabled = true;
                    numericUpDownAlpha2.Enabled = true;
                    textBoxFile.Enabled = false;
                    buttonBrowse.Enabled = false;
                    comboBoxWrapMode.Enabled = true;
                    break;
                case BrushType.TextureBrush:
                    comboBoxHatchBrushes.Enabled = false;
                    comboBoxGradientBrushes.Enabled = false;
                    buttonChange1.Enabled = false;
                    buttonChange2.Enabled = false;
                    numericUpDownAlpha1.Enabled = false;
                    numericUpDownAlpha2.Enabled = false;
                    textBoxFile.Enabled = true;
                    buttonBrowse.Enabled = true;
                    comboBoxWrapMode.Enabled = false;
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            BrushType type = (BrushType)comboBoxType.SelectedItem;

            switch (type)
            {
                case BrushType.SolidBrush:
                    {
                        Color color1 = Color.FromArgb((byte)numericUpDownAlpha1.Value, colorDialog1.Color);
                        brush = new My_SolidBrush(color1);
                    }
                    break;
                case BrushType.HatchBrush:
                    {
                        HatchStyle style = (HatchStyle)comboBoxHatchBrushes.SelectedItem;
                        Color color1 = Color.FromArgb((byte)numericUpDownAlpha1.Value, colorDialog1.Color);
                        Color color2 = Color.FromArgb((byte)numericUpDownAlpha2.Value, colorDialog2.Color);
                        brush = new My_HatchBrush(color1, color2, style);
                    }
                    break;
                case BrushType.LinearGradientBrush:
                    {
                        Color color1 = Color.FromArgb((byte)numericUpDownAlpha1.Value, colorDialog1.Color);
                        Color color2 = Color.FromArgb((byte)numericUpDownAlpha2.Value, colorDialog2.Color);
                        LinearGradientMode mode = (LinearGradientMode)comboBoxGradientBrushes.SelectedItem;
                        brush = new My_LinearGradientBrush(color1, color2, mode, core.Form.ClientRectangle);
                    }
                    break;
                case BrushType.PathGradientBrush:
                    {
                        Color color1 = Color.FromArgb((byte)numericUpDownAlpha1.Value, colorDialog1.Color);
                        Color color2 = Color.FromArgb((byte)numericUpDownAlpha2.Value, colorDialog2.Color);
                        WrapMode w = (WrapMode)comboBoxWrapMode.SelectedItem;
                        brush = new My_PathGradientBrush(w, figure.Points, color1, color2);
                    }
                    break;
                case BrushType.TextureBrush:
                    {
                        string filename = textBoxFile.Text;
                        if (string.IsNullOrEmpty(filename) == false)
                        {
                            Image img = Image.FromFile(filename);
                            brush = new My_TextureBrush(img);
                        }
                    }
                    break;
            }
            pictureBoxPreview.Invalidate();
        }

        private void buttonChange1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
        }

        private void buttonChange2_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBoxFile.Text = openFileDialog1.FileName;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            buttonApply_Click(sender, e);
            figure.brush = brush;
            Close();
        }
    }
}
