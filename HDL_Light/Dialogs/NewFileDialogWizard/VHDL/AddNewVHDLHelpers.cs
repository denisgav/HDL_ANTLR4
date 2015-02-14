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
    internal class VHDL_EntityDrawning
    {
        public List<VHDL_Port> PortList { get; set; }
        public string EntityName { get; set; }
        public string ArchitectureName { get; set; }
        public Font Font { get; set; }

        public VHDL_EntityDrawning(Font font)
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
            g.DrawString(EntityName,
                         Font, new SolidBrush(Color.Black), 70, 201, sf);
            int cLeft = 0, cRight = 0, cInOut = 0;
            for (int i = 0; i < PortList.Count; i++)
            {
                if (PortList[i].Direction == VHDLPortDirection.In) cLeft++;
                else if (PortList[i].Direction == VHDLPortDirection.InOut) cInOut++;
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
                VHDL_Port pi = PortList[i];

                sPortName = pi.Name;
                if ((pi.Type == "STD_LOGIC_VECTOR") || (pi.Type == "BIT_VECTOR"))
                {
                    LinePen = ThickPen;
                    sPortName = sPortName + '(' + pi.LeftIndex + ':' + pi.RightIndex + ')';
                }
                else
                {
                    LinePen = ThinPen;
                }

                if ((pi.Direction == VHDLPortDirection.In) ||
                    ((pi.Direction == VHDLPortDirection.InOut) && bInOutAtLeft))
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

    internal class VHDL_VizardCodeGenerator
    {
        public string EntityName { get; set; }
        public string ArchitectureName { get; set; }

        public List<VHDL_Port> PortList { get; set; }

        public VHDL_VizardCodeGenerator(string EntityName, string ArchitectureName, List<VHDL_Port> PortList)
        {
            this.EntityName = EntityName;
            this.ArchitectureName = ArchitectureName;
            this.PortList = PortList;
        }

        public VHDL_VizardCodeGenerator(VHDL_Module module)
        {
            this.EntityName = module.EntityName;
            this.ArchitectureName = module.ArchitectureName;
            this.PortList = module.PortList;
        }

        public VHDL_VizardCodeGenerator()
        {
        }

        public string Generate()
        {
            StringBuilder fo = new StringBuilder();

            fo.Append("library ieee; \nuse ieee.std_logic_1164.all;\n");
            fo.Append("entity " + EntityName + " is\n");
            if (PortList.Count > 0)
            {
                fo.Append("   port(\n");
                for (int i = 0; i < PortList.Count; i++)
                {
                    fo.Append("     " + PortList[i].Name + "  :  ");
                    switch (PortList[i].Direction)
                    {
                        case VHDLPortDirection.In:
                            fo.Append("in ");
                            break;
                        case VHDLPortDirection.Out:
                            fo.Append("out ");
                            break;
                        case VHDLPortDirection.InOut:
                            fo.Append("inout ");
                            break;
                        case VHDLPortDirection.Buffer:
                            fo.Append("buffer ");
                            break;
                    }
                    fo.Append(PortList[i].Type);
                    if ((PortList[i].LeftIndex != 0) || (PortList[i].RightIndex != 0))
                    {
                        if (PortList[i].LeftIndex < PortList[i].RightIndex)
                        {
                            fo.Append("(" + PortList[i].LeftIndex + " to " + PortList[i].RightIndex + ")");
                        }
                        else
                        {
                            fo.Append("(" + PortList[i].LeftIndex + " downto " + PortList[i].RightIndex + ")");
                        }

                    }
                    if (i != PortList.Count - 1)
                        fo.Append(";");
                    fo.Append("\n");
                }
                fo.Append(");\n");
            }
            fo.Append("end entity;\n\n");
            fo.Append("architecture " + ArchitectureName + " of " + EntityName + " is\n");
            fo.Append("begin\n");
            fo.Append("   -- Statements\n");
            fo.Append("end architecture " + ArchitectureName + ';');

            return fo.ToString();
        }
    }
}