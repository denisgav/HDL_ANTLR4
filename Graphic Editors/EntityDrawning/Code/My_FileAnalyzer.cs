using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using csx;

namespace Schematix.EntityDrawning
{
    public class My_FileAnalyzer
    {

        private string filename;
        private EntityDrawningCore core;

        public ArrayList entities;
        public vhdEntity SelectedEntity { get; set; }

        public List<My_Figure> figures;

        public My_FileAnalyzer(string filename, EntityDrawningCore core)
        {
            this.filename = filename;
            this.core = core;
            figures = new List<My_Figure>();
            Parser parser = new Parser();
            parser.Parsing(filename);
            entities = parser.entities;
        }

        private void GenerateFigures()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            int InPortsCount = SelectedEntity.ports.Count((f) => f.inout == portInOut.In);
            int OutPortsCount = SelectedEntity.ports.Count - InPortsCount;

            int Max_Input_Port = 0;
            int Max_Out_Port = 0;
            foreach (vhdPort p in SelectedEntity.ports)
            {
                if ((p.inout == portInOut.In) && (p.name.Length > Max_Input_Port))
                    Max_Input_Port = p.name.Length;
                if ((p.inout != portInOut.Out) && (p.name.Length > Max_Out_Port))
                    Max_Out_Port = p.name.Length;
            }

            int width = (SelectedEntity.name.Length + Max_Input_Port + Max_Out_Port) * 10 + 10;
            int height = (Math.Max(InPortsCount, OutPortsCount)) * 15;

            Point Location = new Point(200, 200);

            My_Rectangle rect = new My_Rectangle(core, new Rectangle(200, 200, width, height));
            rect.zIndex--;
            My_Line line1 = new My_Line(core, new Point(200 + Max_Input_Port * 10, 200), new Point(200 + 10 * Max_Input_Port, 200 + height));
            line1.zIndex++;
            My_Line line2 = new My_Line(core, new Point(200 + width - 10 * Max_Out_Port, 200), new Point(200 + width - 10 * Max_Out_Port, 200 + height));
            line2.zIndex++;
            My_Text text = new My_Text(core, SelectedEntity.name, new Point(200 + (width / 2), 200 + (height / 2)));

            int delta = (InPortsCount != 0)?height / InPortsCount:0;
            int index = 0;
            foreach (vhdPort p in SelectedEntity.ports)
            {
                if (p.inout != portInOut.In)
                    continue;

                My_Port port = new My_Port(core, p.name, My_Port.PortType.Simple, false, p, new Point(190, 200 + delta / 2 + delta * index), new Point(200, 200 + delta / 2 + delta * index));
                port.TextLabel.CenterPoint = new Point(port.CenterPoint.X + Max_Input_Port*5, port.CenterPoint.Y);
                port.zIndex++;
                figures.Add(port);
                index++;
            }

            delta = (InPortsCount != 0) ? height / OutPortsCount : 0;
            index = 0;
            foreach (vhdPort p in SelectedEntity.ports)
            {
                if (p.inout == portInOut.In)
                    continue;

                My_Port port = new My_Port(core, p.name, My_Port.PortType.Simple, false, p, new Point(200 + width + 10, 200 + delta / 2 + delta * index), new Point(200 + width, 200 + delta / 2 + delta * index));
                port.TextLabel.CenterPoint = new Point(port.CenterPoint.X - Max_Input_Port * 5, port.CenterPoint.Y);
                port.zIndex++;
                figures.Add(port);
                index++;
            }

            figures.Add(rect);
            figures.Add(line1);
            figures.Add(line2);
            figures.Add(text);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public void Analyze(string EntityName)
        {
            foreach (vhdEntity entity in entities)
            {
                if (entity.name.Equals(EntityName))
                {
                    SelectedEntity = entity;
                    break;
                }
            }
            if (SelectedEntity == null)
                return;
            GenerateFigures();
        }

        public void Analyze()
        {
            Entity_Select ent_sel = new Entity_Select(this);
            ent_sel.ShowDialog();
            if (SelectedEntity == null)
                return;
            GenerateFigures();
        }
    }
}