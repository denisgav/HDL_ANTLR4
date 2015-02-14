using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.CommonProperties
{
    [Serializable()]
    public class TextEditorOptions : IOptions, ISerializable
    {

        #region constructors
        public TextEditorOptions()
        {
        }
        public TextEditorOptions(SerializationInfo info, StreamingContext ctxt)
        {
            this.FontName = (string)info.GetValue("FontName", typeof(string));
            this.FontSize = (float)info.GetValue("FontSize", typeof(float));
            this.FontSizeUnits = (System.Drawing.GraphicsUnit)info.GetValue("FontSizeUnits", typeof(System.Drawing.GraphicsUnit));
            this.FontBold = (bool)info.GetValue("FontBold", typeof(bool));
            this.FontItalic = (bool)info.GetValue("FontItalic", typeof(bool));
            this.FontUnderLine = (bool)info.GetValue("FontUnderLine", typeof(bool));
            this.FontStrikeout = (bool)info.GetValue("FontStrikeout", typeof(bool));
        }
        #endregion

        #region Font Data

        /// <summary>
        /// Имя шрифта
        /// </summary>
        private System.Drawing.FontFamily fontFamily;
        private string fontName;
        public string FontName
        {
            get
            {
                return fontName;
            }
            set
            {
                System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;
                foreach (System.Drawing.FontFamily f in families)
                {
                    if (f.Name.EndsWith(value))
                    {
                        fontName = value;
                        fontFamily = f;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Размер шрифта
        /// </summary>
        private float fontSize;
        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (value >= 1.0f)
                    fontSize = value;
            }
        }

        /// <summary>
        /// Жирный шрифт
        /// </summary>
        private bool fontBold;
        public bool FontBold
        {
            get
            {
                return fontBold;
            }
            set
            {
                fontBold = value;
            }
        }

        /// <summary>
        /// Зачеркнутый шрифт
        /// </summary>
        private bool fontStrikeout;
        public bool FontStrikeout
        {
            get
            {
                return fontStrikeout;
            }
            set
            {
                fontStrikeout = value;
            }
        }

        /// <summary>
        /// Курсивный шрифт
        /// </summary>
        private bool fontItalic;
        public bool FontItalic
        {
            get
            {
                return fontItalic;
            }
            set
            {
                fontItalic = value;
            }
        }

        /// <summary>
        /// Подчеркнутый шрифт
        /// </summary>
        private bool fontUnderLine;
        public bool FontUnderLine
        {
            get
            {
                return fontUnderLine;
            }
            set
            {
                fontUnderLine = value;
            }
        }

        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
                if (fontBold == true)
                {
                    style |= System.Drawing.FontStyle.Bold;
                }
                if (fontItalic == true)
                {
                    style |= System.Drawing.FontStyle.Italic;
                }
                if (fontStrikeout == true)
                {
                    style |= System.Drawing.FontStyle.Strikeout;
                }
                if (fontUnderLine == true)
                {
                    style |= System.Drawing.FontStyle.Underline;
                }
                return style;
            }
        }

        /// <summary>
        /// Единицы измерения шрифта
        /// </summary>
        private System.Drawing.GraphicsUnit fontSizeUnits;
        public System.Drawing.GraphicsUnit FontSizeUnits
        {
            get
            {
                return fontSizeUnits;
            }
            set
            {
                fontSizeUnits = value;
            }
        }

        /// <summary>
        /// Формирование конечного обьекта Font
        /// </summary>
        public System.Drawing.Font Font
        {
            get
            {
                System.Drawing.Font font = new System.Drawing.Font(fontFamily, fontSize, FontStyle, fontSizeUnits);
                return font;
            }
        }

        /// <summary>
        /// Загрузка списка имен шрифтов в ComboBox
        /// </summary>
        /// <param name="cb"></param>
        public void LoadFontFamiliesToComboBox(System.Windows.Forms.ComboBox cb)
        {
            cb.Items.Clear();
            System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;
            foreach (System.Drawing.FontFamily f in families)
            {
                cb.Items.Add(f.Name);
            }
        }

        /// <summary>
        /// Загрузка единиц измерения в ComboBox
        /// </summary>
        /// <param name="cb"></param>
        public void LoadGraphicsUnitToComboBox(System.Windows.Forms.ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.Add(System.Drawing.GraphicsUnit.Inch);
            cb.Items.Add(System.Drawing.GraphicsUnit.Millimeter);
            cb.Items.Add(System.Drawing.GraphicsUnit.Pixel);
            cb.Items.Add(System.Drawing.GraphicsUnit.Point);
        }
        #endregion

        #region IOptions Members

        public void LoadData(Options options)
        {
            options.SetOptionsData(this);
        }

        public void Accept(Options options)
        {
            options.GetOptionsData(this);
        }

        public void SetDefault()
        {
            FontName = "Times New Roman";
            FontSize = 14.0f;
            FontSizeUnits = System.Drawing.GraphicsUnit.Pixel;
            FontBold = false;
            FontItalic = false;
            FontStrikeout = false;
            FontUnderLine = false;
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FontName", FontName);
            info.AddValue("FontSize", FontSize);
            info.AddValue("FontSizeUnits", FontSizeUnits);
            info.AddValue("FontBold", FontBold);
            info.AddValue("FontItalic", FontItalic);
            info.AddValue("FontUnderLine", FontUnderLine);
            info.AddValue("FontStrikeout", FontStrikeout);
        }

        #endregion
    }
}