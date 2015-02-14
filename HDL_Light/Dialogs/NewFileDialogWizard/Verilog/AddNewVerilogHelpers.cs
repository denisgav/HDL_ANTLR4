using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Schematix.Core;

namespace Schematix.Dialogs.NewFileDialogWizard
{
    internal class Verilog_EntityDrawning
    {
        public List<Verilog_Port> PortList { get; set; }
        public string ModuleName { get; set; }
        public Font Font { get; set; }

        public Verilog_EntityDrawning(Font font)
        {
            this.Font = font;
        }

        public void Draw(Graphics g)
        {
            Pen ThinPen = Pens.Black;
            Pen ThickPen = new Pen(Color.Black, 3);
            Pen LinePen;
            g.FillRectangle(new SolidBrush(Color.LemonChiffon), 20, 1, 100, 200);
            g.DrawRectangle(ThickPen, 20, 0, 100, 200);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString(ModuleName,
                         Font, new SolidBrush(Color.Black), 70, 201, sf);
            int cLeft = 0, cRight = 0, cInOut = 0;
            for (int i = 0; i < PortList.Count; i++)
            {
                if (PortList[i].Direction == VerilogPortDirection.In) cLeft++;
                else if (PortList[i].Direction == VerilogPortDirection.InOut) cInOut++;
                else cRight++;
            }
            bool bInOutAtLeft = false;
            if (cLeft + cInOut < cRight)
            {
                cLeft += cInOut;
                bInOutAtLeft = true;
            }
            else
                cRight += cInOut;
            int iStepLeft = (200 / (cLeft + 1));
            int iStepRight = (200 / (cRight + 1));
            cLeft = cRight = 0;
            sf.LineAlignment = StringAlignment.Center;
            String sPortName;
            for (int i = 0; i < PortList.Count; i++)
            {
                Verilog_Port pi = PortList[i];

                sPortName = pi.Name;
                if (pi.isBus == true)
                {
                    LinePen = ThickPen;
                    sPortName = sPortName + '(' + pi.LeftIndex + ':' + pi.RightIndex + ')';
                }
                else
                {
                    LinePen = ThinPen;
                }

                if ((pi.Direction == VerilogPortDirection.In) ||
                    ((pi.Direction == VerilogPortDirection.InOut) && bInOutAtLeft))
                {
                    // Draw at left side
                    cLeft++;
                    g.DrawLine(LinePen, 0, cLeft * iStepLeft, 20, cLeft * iStepLeft);
                    sf.Alignment = StringAlignment.Near;
                    g.DrawString(sPortName, this.Font, SystemBrushes.ControlText, 22, cLeft * iStepLeft, sf);
                }
                else
                {
                    // Draw at right side
                    cRight++;
                    g.DrawLine(LinePen, 120, cRight * iStepRight, 140, cRight * iStepRight);
                    sf.Alignment = StringAlignment.Far;
                    g.DrawString(sPortName, this.Font, SystemBrushes.ControlText, 118, cRight * iStepRight, sf);
                }

            }
        }
    }


    internal class Verilog_VizardCodeGenerator
    {
        public string ModuleName { get; set; }
        public string Timescale { get; set; }
        public List<Verilog_Port> PortList { get; set; }

        public Verilog_VizardCodeGenerator(string ModuleName, string Timescale, List<Verilog_Port> PortList)
        {
            this.ModuleName = ModuleName;
            this.Timescale = Timescale;
            this.PortList = PortList;
        }

        public Verilog_VizardCodeGenerator(Verilog_Module module)
        {
            this.ModuleName = module.ModuleName;
            this.Timescale = module.Timescale;
            this.PortList = module.PortList;
        }

        public Verilog_VizardCodeGenerator()
        {
        }

        public string Generate()
        {
            StringBuilder fo = new StringBuilder();

            fo.AppendFormat("`timescale {0}\n", Timescale);
            fo.AppendFormat("module {0} ( ", ModuleName);
            //выводим перечень портов
            for(int i=0; i<PortList.Count; i++)
            {
                fo.Append(PortList[i].Name);
                if (i < (PortList.Count - 1))
                    fo.Append(", ");
            }
            fo.Append(");\n");

            //обьявление сигналов
            foreach (Verilog_Port port in PortList)
            {
                string direction = string.Empty;
                switch(port.Direction)
                {
                    case VerilogPortDirection.In:
                        direction = "input";
                        break;
                    case VerilogPortDirection.Out:
                        direction = "output";
                        break;
                    case VerilogPortDirection.InOut:
                        direction = "inout";
                        break;
                    default:
                        break;
                }
                if (port.isBus == false)
                {
                    fo.AppendFormat("\t{0} {1} {2};\n", direction, port.Type, port.Name);
                }
                else
                {
                    fo.AppendFormat("\t{0} {1} [{2} : {3}] {4};\n", direction, port.Type, port.LeftIndex, port.RightIndex, port.Name);
                }
            }

            fo.Append("//Statements\n");
            fo.Append("endmodule");

            return fo.ToString();
        }
    }
}