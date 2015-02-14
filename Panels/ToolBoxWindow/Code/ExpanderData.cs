using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ToolBoxWindow
{
    public class ExpanderData
    {
        public ExpanderData()
        {
            items = new List<ToolBoxItem>();
        }

        public System.Windows.Controls.Expander Expander;

        public void CreateExpander()
        {
            Expander = new System.Windows.Controls.Expander();

            //заполняем заголовок Expander

            System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock();
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

            System.Windows.Controls.WrapPanel wrapPanel = new System.Windows.Controls.WrapPanel();
            wrapPanel.Children.Add(image);
            wrapPanel.Children.Add(textBlock);
            Expander.Header = wrapPanel;
            Expander.IsExpanded = true;

            //Закончили работу с заголовком Expander

            //Заполняем содержимое Expander
            System.Windows.Controls.StackPanel stackPanel = new System.Windows.Controls.StackPanel();
            foreach (ToolBoxItem item in items)
            {
                item.CreateToggleButton();
                stackPanel.Children.Add(item.ToggleButton);
            }
            Expander.Content = stackPanel;
            Expander.Margin = new System.Windows.Thickness(5, 2, 5, 2);
            //Содержимое заполенео

            System.Windows.Controls.ToolTip toolTip = new System.Windows.Controls.ToolTip();
            System.Windows.Controls.Border border = new System.Windows.Controls.Border();
            border.Background = System.Windows.Media.Brushes.WhiteSmoke;
            border.BorderThickness = new System.Windows.Thickness(2);
            border.BorderBrush = System.Windows.Media.Brushes.Black;
            toolTip.Content = border;

            System.Windows.Controls.TextBlock toolTipTextBlock = new System.Windows.Controls.TextBlock();
            toolTipTextBlock.Text = Description;
            border.Child = toolTipTextBlock;
            Expander.ToolTip = toolTip;
        }

        private List<ToolBoxItem> items; //список элементов
        public List<ToolBoxItem> Items
        {
            get
            {
                return items;
            }
            private set
            {
                items = value;
            }
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

        private string description; //описание
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

        private string typeOfElements; //тип окна к которым относятся элементы
        public string TypeOfElements
        {
            get
            {
                return typeOfElements;
            }
            set
            {
                typeOfElements = value;
            }
        }

        public static List<ExpanderData> LoadSystemData()
        {
            try
            {
                List<ExpanderData> res = null;
                string data = ToolBoxWindow.Resources.ToolBoxDataKernel;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(data.Length))
                {
                    foreach (char c in data)
                    {
                        stream.WriteByte(Convert.ToByte(c));
                    }
                    stream.Position = 0;

                    res = LoadDatafromXml(stream);
                    stream.Close();
                }
                return res;
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Load ToolBox data error.", ex);
                return new List<ExpanderData>();
            }
        }

        public static List<ExpanderData> LoadUserData()
        {
            try
            {
                string CurDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string CommonPath = CurDirectory + "\\ToolBoxData.xml";
                string UserPath = Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation + "\\ToolBoxData.xml";

                if (System.IO.File.Exists(UserPath) == false)
                {
                    if (System.IO.File.Exists(CommonPath) != false)
                    {
                        System.IO.File.Copy(CommonPath, UserPath);
                        return LoadDatafromXml(UserPath);
                    }
                    else
                        return new List<ExpanderData>();
                }
                else
                {
                    return LoadDatafromXml(UserPath);
                }
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Load ToolBox data error.", ex);
                return new List<ExpanderData>();
            }
        }

        public static List<ExpanderData> LoadDatafromXml(String fileName)
        {
            try
            {
                List<ExpanderData> res = null;
                using (System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
                {
                    res = LoadDatafromXml(stream);
                }
                return res;
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Load ToolBox data error.", ex);
                return new List<ExpanderData>();
            }
        }

        public static List<ExpanderData> LoadDatafromXml(System.IO.Stream stream)
        {
            List<ExpanderData> res = new List<ExpanderData>();

            try
            {
                XmlDocument _doc = new XmlDocument();
                _doc.Load(stream);

                XmlNodeList nodes = _doc.SelectNodes("/ExpanderList/ExpanderData");
                foreach (XmlNode node in nodes)
                {
                    ExpanderData data = new ExpanderData();
                    data.Caption = node.SelectSingleNode("Caption").InnerText;
                    data.IconPath = node.SelectSingleNode("IconPath").InnerText;
                    data.Description = node.SelectSingleNode("Description").InnerText;
                    data.TypeOfElements = node.SelectSingleNode("TypeOfElements").InnerText;

                    XmlNodeList EdpanderDataItems = node.SelectNodes("ToolBoxItem");
                    foreach (XmlNode nodeitem in EdpanderDataItems)
                    {
                        ToolBoxItem item = new ToolBoxItem(data);
                        item.Caption = nodeitem.SelectSingleNode("Caption").InnerText;
                        item.Command = nodeitem.SelectSingleNode("Command").InnerText;
                        item.Description = nodeitem.SelectSingleNode("Description").InnerText;
                        item.IconPath = nodeitem.SelectSingleNode("IconPath").InnerText;
                        data.items.Add(item);
                    }

                    res.Add(data);
                }
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Load ToolBox data error.", ex);
                return res;
            }

            return res;
        }

        public static void SaveToXML(List<ExpanderData> expanderList)
        {
            string UserPath = Schematix.CommonProperties.Configuration.CurrentConfiguration.ProjectOptions.DefaultProjectLocation + "\\ToolBoxData.xml";
            SaveToXML(expanderList, UserPath);
        }

        public static void SaveToXML(List<ExpanderData> expanderList, string Path)
        {
            try
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(Path, System.IO.FileMode.Create))
                {
                    SaveToXML(expanderList, stream);
                }
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Save ToolBox data error.", ex);
            }
        }

        public static void SaveToXML(List<ExpanderData> expanderList, System.IO.Stream stream)
        {
            try
            {
                XmlDocument _doc = new XmlDocument();
                XmlElement rootNode = _doc.CreateElement("ExpanderList");
                foreach (ExpanderData data in expanderList)
                {
                    XmlElement DataElement = _doc.CreateElement("ExpanderData");

                    XmlElement ElementCaption = _doc.CreateElement("Caption");
                    XmlCDataSection descrcapt = _doc.CreateCDataSection(data.Caption);
                    ElementCaption.AppendChild(descrcapt);
                    DataElement.AppendChild(ElementCaption);

                    XmlElement ElementIconPath = _doc.CreateElement("IconPath");
                    ElementIconPath.InnerText = data.GetOriginalIconPath();
                    DataElement.AppendChild(ElementIconPath);

                    XmlElement ElementDescription = _doc.CreateElement("Description");
                    XmlCDataSection descr = _doc.CreateCDataSection(data.Description);
                    ElementDescription.AppendChild(descr);
                    DataElement.AppendChild(ElementDescription);

                    XmlElement ElementTypeOfElements = _doc.CreateElement("TypeOfElements");
                    ElementTypeOfElements.InnerText = data.TypeOfElements;
                    DataElement.AppendChild(ElementTypeOfElements);

                    foreach (ToolBoxItem item in data.Items)
                    {
                        XmlElement ToolBoxItem = _doc.CreateElement("ToolBoxItem");

                        XmlElement ItemCaption = _doc.CreateElement("Caption");
                        XmlCDataSection descrcapt2 = _doc.CreateCDataSection(item.Caption);
                        ItemCaption.AppendChild(descrcapt2);
                        ToolBoxItem.AppendChild(ItemCaption);

                        XmlElement ItemIconPath = _doc.CreateElement("IconPath");
                        ItemIconPath.InnerText = item.GetOriginalIconPath();
                        ToolBoxItem.AppendChild(ItemIconPath);

                        XmlElement ItemDescription = _doc.CreateElement("Description");
                        XmlCDataSection idescr = _doc.CreateCDataSection(item.Description);
                        ItemDescription.AppendChild(idescr);
                        ToolBoxItem.AppendChild(ItemDescription);

                        XmlElement ItemCommand = _doc.CreateElement("Command");
                        ItemCommand.InnerText = item.Command;
                        ToolBoxItem.AppendChild(ItemCommand);

                        DataElement.AppendChild(ToolBoxItem);
                    }
                    rootNode.AppendChild(DataElement);
                }
                _doc.AppendChild(rootNode);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.Unicode;
                settings.NewLineOnAttributes = true;

                XmlWriter writer = XmlWriter.Create(stream, settings);
                _doc.Save(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Save ToolBox data error.", ex);
            }
        }

        public static readonly string[] TypeOfElementsAlloved = new string[]
        {
            "FSM",
            "EntityDrawning",
            "SchemaEditor",
            "VHDLTextTemplates",
            "VerilogTetxTemplates",
            "SystemCTextTemplates"
        };

        public static readonly string[] Icons = new string[]
        {
            @"vhdl.png",
            @"verilog.png",
            @"EntityDrawning/arc.gif",
            @"EntityDrawning/asynchronous.gif",
            @"EntityDrawning/asynchronous2.gif",
            @"EntityDrawning/bezie1.gif",
            @"EntityDrawning/bezie2.gif",
            @"EntityDrawning/curve.gif",
            @"EntityDrawning/ellipse.gif",
            @"EntityDrawning/image.gif",
            @"EntityDrawning/inverse.gif",
            @"EntityDrawning/inverse1.gif",
            @"EntityDrawning/inverse2.gif",
            @"EntityDrawning/inverse_asynchronous.gif",
            @"EntityDrawning/inverse_synchronous.gif",
            @"EntityDrawning/line.gif",
            @"EntityDrawning/path.gif",
            @"EntityDrawning/pie.gif",
            @"EntityDrawning/polygon.gif",
            @"EntityDrawning/polyline s.gif",
            @"EntityDrawning/polyline.gif",
            @"EntityDrawning/port1.gif",
            @"EntityDrawning/port2.gif",
            @"EntityDrawning/port3.gif",
            @"EntityDrawning/port4.gif",
            @"EntityDrawning/rectangle.gif",
            @"EntityDrawning/simple.gif",
            @"EntityDrawning/synchronous.gif",
            @"EntityDrawning/synchronous2.gif",
            @"EntityDrawning/text.gif",
            @"FSM/const.png",
            @"FSM/constant.png",
            @"FSM/port1.gif",
            @"FSM/port2.gif",
            @"FSM/port3.gif",
            @"FSM/port4.gif",
            @"FSM/reset.png",
            @"FSM/reset1.gif",
            @"FSM/reset1_.gif",
            @"FSM/reset2.gif",
            @"FSM/reset2_.gif",
            @"FSM/signal.png",
            @"FSM/state.png",
            @"FSM/transition.png",
            @"FSM/variable.gif",
            @"Scheme/AddPort.bmp",
            @"Scheme/AddSignal.bmp",
            @"Scheme/Architecture.bmp",
            @"Scheme/Entity.bmp",
            @"cursor.png",
            @"FSM/FSM.png",
            @"Toolbox.bmp",
            @"Unknown.bmp"
        };
    }
}
