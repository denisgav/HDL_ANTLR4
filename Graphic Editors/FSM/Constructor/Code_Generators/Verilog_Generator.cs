using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.FSM
{
    public class Verilog_Generator
    {
        Schematix.FSM.My_Graph graph;
        Constructor_Core core;

        public Verilog_Generator(Constructor_Core core)
        {
            this.graph = core.Graph;
            this.core = core;
        }

        public string GenerateCode()
        {
            StringBuilder res = new StringBuilder();

            //выписываем имя модуля
            res.AppendFormat("`timescale {0}\n", graph.VerilogModule.Timescale);
            res.AppendFormat("module {0} (", graph.VerilogModule.ModuleName);   //имя модуля
            
            //выписываем перечень портов
            for (int i = 0; i < graph.Ports.Count; i++ )
            {
                My_Port p = graph.Ports[i];
                if (i < (graph.Ports.Count - 1))
                    res.AppendFormat("{0}, ", p.name);
                else
                    res.AppendFormat("{0}", p.name);
            }
            res.Append("); \n");

            //выписываем перечень входных и выходных сигналов
            foreach (My_Port p in graph.Ports)
            {
                string direction = string.Empty;
                switch (p.Direction)
                {
                    case My_Port.PortDirection.In:
                        direction = "input";
                        break;
                    case My_Port.PortDirection.Out:
                        direction = "output";
                        break;
                    case My_Port.PortDirection.InOut:
                        direction = "inout";
                        break;
                    default:
                        break;
                }
                if (p.Type_inf.avaliable == false)
                {
                    res.AppendFormat("\t{0} {1} {2};\n", direction, p.Type, p.name);
                }
                else
                {
                    res.AppendFormat("\t{0} {1} [{2} : {3}] {4};\n", direction, p.Type, p.Type_inf.range1, p.Type_inf.range2, p.name);
                }
            }

            int bus_width = 0;

            if (graph.Signals.Count != 0)
            {
                //Обьявление переменных (Variables)
                res.Append("\t//Variables Declaration\n");
                foreach (My_Signal s in graph.Signals)
                {
                    res.AppendFormat("\t{0} {1};\n", s.Type, s.name);
                }
            }

            if (graph.Constants.Count != 0)
            {
                //Обьявление констант
                res.Append("\t//Constant Declaration\n");
                foreach(My_Constant c in graph.Constants)
                {
                    if(c.Gen_Type == My_Constant.GenerationType.Generic)
                        res.AppendFormat("\t`define {0} {1}\n", c.name, c.Default_Value);
                    if (c.Gen_Type == My_Constant.GenerationType.Constant)
                        res.AppendFormat("\tparameter {0} = {1};\n", c.name, c.Default_Value);
                }
            }

            //Обьявление состояний автомата
            for (int i = 0; i < graph.States.Count; i++)
            {
                My_State s = graph.States[i];
                string bin_value = System.Convert.ToString(i, 2);
                string value = string.Format("{0}'b{1}", bin_value.Length, bin_value);

                if (bus_width < bin_value.Length)
                    bus_width = bin_value.Length;

                res.AppendFormat("`define {0} {1}\n", s.name, value);
            }

            //обьявление текущего и следующего состояния
            res.AppendFormat("reg [{0} : 0] Current_State;\n", bus_width-1);
            res.AppendFormat("reg [{0} : 0] Next_State;\n", bus_width-1);

            //Здесь происходит инициалзация автомата
            res.Append("initial\n");
	        res.Append("\tbegin\n");
    	    res.Append("\t\tCurrent_State <= `S0;");
            res.Append("\tend\n");

            //процесс, отвечающий за действия в текущем состоянии и переход в следующее состояние

            res.Append("\t always @ (Current_State)\n");
            res.Append("\t\t begin\n");
            res.Append("\t\t\t Next_State = Current_State; \n");
            res.Append("\t\t\t case (Current_State)\n");
            foreach (My_State s in graph.States)
            {
                //выписываем имя состояния
                res.AppendFormat("\t\t\t\t `{0}:\n", s.name);
                res.Append("\t\t\t\tbegin\n");

                //выписываем действия при входе (ЕСЛИ ТАКОВЫ ИМЕЮТСЯ)
                if (string.IsNullOrEmpty(s.ActivityInput) == false)
                {
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                    res.AppendFormat("\t\t\t\t\t{0}\n", s.ActivityInput);
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                }
                //выписываем действия внутри (ЕСЛИ ТАКОВЫ ИМЕЮТСЯ)
                if (string.IsNullOrEmpty(s.condition) == false)
                {
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                    res.AppendFormat("\t\t\t\t\t{0}\n", s.condition);
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                }
                //выписываем действия на выходе (ЕСЛИ ТАКОВЫ ИМЕЮТСЯ)
                if (string.IsNullOrEmpty(s.ActivityExit) == false)
                {
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                    res.AppendFormat("\t\t\t\t\t{0}\n", s.ActivityExit);
                    res.Append("\t\t\t\t\t////////////////////////////////\n");
                }

                //перебираем все выходящие дуги...

                List<Schematix.FSM.My_Line> out_lines = new List<My_Line>();
                //это список линий, которые выходят из данного состояния
                //первым делом нужно построить данный список
                out_lines = graph.Lines.ToList().FindAll(r => r.state_begin == s);

                //теперь нам нужно отсортировать список по приоритетам
                out_lines.Sort((r1, r2) => r1.priority.CompareTo(r2.priority));
                if (out_lines.Count != 0)
                {
                    bool start = false;
                    foreach (Schematix.FSM.My_Line line in out_lines)
                    {
                        if (string.IsNullOrEmpty(line.condition) == false)
                        {
                            //пишем условие перехода
                            if (start == false)
                            {
                                start = true;
                                res.AppendFormat("\t\t\t if ( {0} )\n", line.condition);
                                res.Append("\t\t\t\t begin\n");
                            }
                            else
                            {
                                res.AppendFormat("\t\t\t else if ({0})\n", line.condition);
                                res.Append("\t\t\t\t begin\n");
                            }
                            //действия внутри перехода
                            if (string.IsNullOrEmpty(line.Action) == false)
                                res.AppendFormat("\t\t\t\t {0} \n ", line.Action);
                            //переход к следующему состоянию
                            res.AppendFormat("\t\t\t\t Next_State <= `{0}; \n", line.state_end.name);
                            //конец
                            res.AppendFormat("\t\t\t\t end \n\n ");
                        }
                        else
                        {
                            if (start == false)
                            {
                                res.Append("\t\t\t\t begin\n");
                                if (string.IsNullOrEmpty(line.Action) == false)
                                    res.AppendFormat("\t\t\t\t {0} \n ", line.Action);
                                res.AppendFormat("\t\t\t\t Next_State <= `{0}; \n", line.state_end.name);
                                res.AppendFormat("\t\t\t\t end \n\n ");
                            }
                            else
                            {
                                res.Append("\t\t\t else \n");
                                res.Append("\t\t\t\t begin\n");
                                if (string.IsNullOrEmpty(line.Action) == false)
                                    res.AppendFormat("\t\t\t\t {0} \n ", line.Action);
                                res.AppendFormat("\t\t\t\t then Next_State <= `{0}; \n", line.state_end.name);
                                res.AppendFormat("\t\t\t\t end \n\n ");
                            }
                        }
                    }
                }
                res.Append("\t\t\t\tend\n");
            }
            res.Append("\t\t endcase  \n");
            res.Append("\t end  \n");

            //процесс, реагирующий на таймер
            //и выполняющий переход в следующее состояние
            //Также в этом процессе выполняется сброс

            My_Port ClockedPort = graph.ClockPort;
            string argument = "";
            if (ClockedPort != null)
                argument = ClockedPort.name;

            if (graph.Reset == null)
            {
                res.AppendFormat("\t always @(posedge {0}) \n", argument);
                res.Append("\t\tbegin\n");
                res.Append("\t\t\tCurrent_State = Next_State;\n");
                res.Append("\t\tend\n");
            }
            else
            {
                if (graph.Reset.res_type == My_Reset.Reset_Type.Asynchonous)
                {
                    argument += ", " + graph.Reset.signal;
                }
                res.AppendFormat("\t always @(posedge {0}) \n", argument);
                res.Append("\t\tbegin\n");
                //выписываем условие перехода
                res.AppendFormat("\t\tif({0})\n", graph.Reset.condition);
                res.AppendFormat("\t\t\tCurrent_State = `{0};\n", graph.Reset.state.name);
                res.Append("\t\telse\n");
                res.Append("\t\t\tCurrent_State = Next_State;\n");
                res.Append("\t\tend\n");
            }

            res.Append("endmodule");
            return res.ToString();
        }

    }
}