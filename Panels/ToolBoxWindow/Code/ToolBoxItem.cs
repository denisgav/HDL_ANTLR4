using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBoxWindow
{
    public class ToolBoxItem
    {
        public System.Windows.Controls.Primitives.ToggleButton ToggleButton;
        public void CreateToggleButton()
        {
            try
            {
                ToggleButton = new System.Windows.Controls.Primitives.ToggleButton();


                ToggleButton.Content = CreateToggleButtonContent();

                System.Windows.Controls.ToolTip toolTip = new System.Windows.Controls.ToolTip();
                System.Windows.Controls.Border border = new System.Windows.Controls.Border();
                border.Background = System.Windows.Media.Brushes.WhiteSmoke;
                border.BorderThickness = new System.Windows.Thickness(2);
                border.BorderBrush = System.Windows.Media.Brushes.Black;
                toolTip.Content = border;

                System.Windows.Controls.TextBlock toolTipTextBlock = new System.Windows.Controls.TextBlock();
                toolTipTextBlock.Text = Description;
                border.Child = toolTipTextBlock;
                ToggleButton.ToolTip = toolTip;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Some Error :(", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Load ToolBox data error.", ex);
            }
        }

        public System.Windows.Controls.WrapPanel CreateToggleButtonContent()
        {
            System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock();
            textBlock.TextWrapping = System.Windows.TextWrapping.Wrap;
            textBlock.Text = Caption;

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(IconPath);
            bi3.EndInit();
            image.Stretch = System.Windows.Media.Stretch.Fill;
            image.Source = bi3;
            image.Width = 16;
            image.Height = 16;
            image.Margin = new System.Windows.Thickness(0, 0, 5, 0);

            System.Windows.Controls.WrapPanel wrapPanel = new System.Windows.Controls.WrapPanel();
            wrapPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            wrapPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            wrapPanel.Children.Add(image);
            wrapPanel.Children.Add(textBlock);
            return wrapPanel;
        }

        private ExpanderData expanderData;
        public ToolBoxItem(ExpanderData expanderData)
        {
            this.expanderData = expanderData;
        }

        private string caption; //заголовок
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        private string iconPath;//путь к картинке
        public string IconPath
        {
            get
            {
                //производим некоторую обработку пути к иконке
                return "pack://application:,,,/ToolBoxWindow;component/Icons/" + iconPath;
            }
            set
            {
                iconPath = value;
            }
        }
        public string GetOriginalIconPath()
        {
            return iconPath;
        }

        private string command;//команда
        public string Command
        {
            get
            {
                return command;
            }
            set
            {
                command = value;
            }
        }

        private string description; //описание (в случае с текстовым полем - текст который будет вставляться)
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public string Description2
        {
            get
            {
                if (description.Length < 200)
                    return description;
                return description.Substring(0, 200) + "...";
            }
        }

        public System.Windows.DataObject GetData(string SelectedType)
        {
            System.Windows.DataObject data = new System.Windows.DataObject();
            switch (SelectedType)
            {
                case "VHDLTextTemplates":
                case "VerilogTextTemplates":
                case "SystemCTextTemplates":
                    data.SetText(description);
                break;

                case "FSM":
                case "EntityDrawning":
                case "Schema":
                    data.SetText(command);
                break;

                default:
                break;
            }
            return data;
        }
    }
}
