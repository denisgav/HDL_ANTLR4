using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;

namespace csx
{
    public enum ElementType { Element, ExternPort, Terminator, none };
    public class Element:Common //элемент
    {
        public ElementType elementType = ElementType.Element;
        public bool isLeftInOut; //размещение входов-выходов в элементе
        public Metafile img; //изображение элемента
        public string text = "";
        public Element(Metafile image, Construct parent, string name, ElementType elementType)
        {
            this.elementType = elementType;
            isElement = true;
            this.name = name;
            this.isLeftInOut = false;
            img = image;
            ports = new List<Port>();
            this.parent = parent;
        }
        public Element(Metafile image, Construct parent, bool isLeftInOut, string name, ElementType elementType)
        {
            this.elementType = elementType;
            isElement = true;
            this.name = name;
            this.isLeftInOut = isLeftInOut;
            img = image;
            ports = new List<Port>();
            this.parent = parent;
        }
        public Element(Metafile image, Construct parent, ElementType elementType)
        {
            this.elementType = elementType;
            this.name = "Element_" + count.ToString();
            count++;
            isElement = true;
            this.name = name;
            this.isLeftInOut = false;
            img = image;
            ports = new List<Port>();
            this.parent = parent;
        }
        public Element(Metafile image, Construct parent, bool isLeftInOut, ElementType elementType)
        {
            this.elementType = elementType;
            this.name = "Element_" + count.ToString();
            count++;
            isElement = true;
            this.name = name;
            this.isLeftInOut = isLeftInOut;
            img = image;
            ports = new List<Port>();
            this.parent = parent;
        }
        public override void Draw(Graphics e)
        {
            e.DrawImage(img, border);
        }        
    }
}
