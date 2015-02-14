using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.FSM
{
    public class VHDL_Generator
    {
        Schematix.FSM.My_Graph graph;
        Constructor_Core core;

        private string use;         //блоки use
        private string entity;      //обьявнеие портов
        private string declaration; //обьявление перечисления сигналов
        private string process1;    //процесс вычисления перехода
        private string process2;    //процесс непосредственно перехода
        private string process3;    //процесс в котором выполняются все действия состояний

        public VHDL_Generator(Constructor_Core core)
        {
            this.core = core;
            this.graph = core.Graph;
        }

        private string Generate_Use()
        {
            StringBuilder res = new StringBuilder();
            res.Append("library IEEE;\n");
            res.Append("use IEEE.std_logic_1164.all; \n");
            res.Append("use IEEE.std_logic_arith.all; \n");
            res.Append("use IEEE.std_logic_unsigned.all; \n \n");
            return res.ToString();
        }

        private string Generate_Declaration()
        {
            StringBuilder res = new StringBuilder();
            res.Append("\t type Sreg_type is (");
            foreach (Schematix.FSM.My_State state in graph.States)
            {
                res.Append(state.name);
                if (state != graph.States.Last<Schematix.FSM.My_State>())
                    res.Append(", ");
            }
            res.Append("); \n");

            res.Append("\t signal State: Sreg_type; \n");
            res.Append("\t signal State_Next: Sreg_type; \n\n");

            //Добавляем обьявление костант
            List<Schematix.FSM.My_Constant> ConstantList = new List<Schematix.FSM.My_Constant>();
            foreach (Schematix.FSM.My_Constant c in graph.Constants)
            {
                    if (c.Gen_Type == My_Constant.GenerationType.Constant)
                        ConstantList.Add(c);
            }
            if (ConstantList.Count != 0)
            {
                foreach (Schematix.FSM.My_Signal signal in ConstantList)
                {
                    res.Append("\t constant " + signal.name + " : " + signal.Type);
                    if (signal.Type_inf.avaliable == true)
                    {
                        res.Append("_Vector (" + signal.Type_inf.range1.ToString());
                        if (signal.Type_inf.to)
                            res.Append(" to ");
                        else
                            res.Append(" downto ");
                        res.Append(signal.Type_inf.range2.ToString() + " )");
                    }
                    if (string.IsNullOrEmpty(signal.Default_Value) != true)
                        res.Append(" := " + signal.Default_Value);
                    res.Append("; \n");
                }
            }

            //добавляем обьявление пременных
            foreach (Schematix.FSM.My_Signal signal in graph.Signals)
            {
                if (signal is Schematix.FSM.My_Constant)
                    continue;
                res.Append("\t signal " + signal.name + " : " + signal.Type);
                if (signal.Type_inf.avaliable == true)
                {
                    res.Append("_Vector (" + signal.Type_inf.range1.ToString());
                    if (signal.Type_inf.to)
                        res.Append(" to ");
                    else
                        res.Append(" downto ");
                    res.Append(signal.Type_inf.range2.ToString() + " )");
                }
                if (string.IsNullOrEmpty(signal.Default_Value) != true)
                    res.Append(" := " + signal.Default_Value);
                res.Append("; \n");
            }

            return res.ToString();
        }

        private string Generate_Entity()
        {
            StringBuilder res = new StringBuilder();
            res.Append("entity " + graph.VHDLModule.EntityName + " is \n");
            //пишем константы генерации

            List<Schematix.FSM.My_Constant> GenericConstantList = new List<Schematix.FSM.My_Constant>();
            foreach (Schematix.FSM.My_Constant c in graph.Constants)
            {
                if (c.Gen_Type == My_Constant.GenerationType.Generic)
                    GenericConstantList.Add(c);
            }
            if (GenericConstantList.Count != 0)
            {
                res.Append("\n\n \t generic \n\t(\n");
                foreach (My_Constant c in GenericConstantList)
                {
                    res.Append("\t\t" + c.name + " : " + c.Type + " ");
                    if (c.Type_inf.avaliable == true)
                    {
                        res.Append("_Vector(" + c.Type_inf.range1.ToString());
                        if (c.Type_inf.to)
                            res.Append(" to ");
                        else
                            res.Append(" downto ");
                        res.Append(c.Type_inf.range2.ToString() + " )");
                    }
                    if (string.IsNullOrEmpty(c.Default_Value) != true)
                        res.Append(" := " + c.Default_Value);
                    int index = GenericConstantList.IndexOf(c);
                    if (index != GenericConstantList.Count - 1)
                    {
                        res.Append(";");
                    }

                    res.Append("\n");
                }
                res.Append("\t);\n");
            }

            //записываем порты
            res.Append("\t port(\n");
            //печатам инфрмацию о порте
            foreach (Schematix.FSM.My_Port port in graph.Ports)
            {
                res.Append("\t\t" + port.name + "  :  ");
                res.Append(port.Direction.ToString() + " ");

                if (port.Type_inf.avaliable == true)
                {
                    if (port.Type_inf.to == true)
                    {
                        res.Append(port.Type + " (" + port.Type_inf.range1.ToString() + " to " + port.Type_inf.range2.ToString() + ")");
                    }
                    else
                    {
                        res.Append(port.Type + " (" + port.Type_inf.range2.ToString() + " downto " + port.Type_inf.range1.ToString() + ")");
                    }
                }
                else
                    res.Append(port.Type);

                int index = graph.Ports.IndexOf(port);
                if (graph.Ports.Count > index + 1)
                    res.Append(";");
                res.Append("\n");
            }
            res.Append("\t ); end; \n\n");
            return res.ToString();
        }

        private string Generate_process1()
        {
            StringBuilder res = new StringBuilder();

            string arguments = "State";
            if (graph.Reset != null)
            {
                if (graph.Reset.res_type == My_Reset.Reset_Type.Asynchonous)
                    arguments += ", " + graph.Reset.signal;
            }
            My_Port ClockedPort = graph.ClockPort;
            if (ClockedPort != null)
                arguments += ", " + ClockedPort.name;

            res.Append("begin \n\n \t process (" + arguments + ") is \n");
            res.Append("\t begin \n");
            //действия во время сброса
            if (graph.Reset != null)
            {
                if (graph.Reset.res_type == My_Reset.Reset_Type.Asynchonous)
                {
                    res.Append("\tif " + graph.Reset.condition + " then \n");
                    res.Append("\t\t State <= " + graph.Reset.state.name + ";\n");
                    if (ClockedPort != null)
                        res.Append("\telsif rising_edge(" + ClockedPort.name + ") then\n");
                    else
                        res.Append("\telse\n");
                }
                else
                {
                    if (ClockedPort != null)
                        res.Append("if rising_edge(" + ClockedPort.name + ") then\n");
                    res.Append("if " + graph.Reset.condition + " then\n");
                    res.Append("\t\tState <= " + graph.Reset.state.name + ";\n");
                    res.Append("\telse\n");
                }
            }
            res.Append("\t\t case State is \n");
            //дальше перебераем все состояния и смотрим какие дуги из них выходят...
            foreach (Schematix.FSM.My_State state in graph.States)
            {
                res.Append("\t\t\t when " + state.name + " => \n");
                //перебираем все выходящие дуги...

                List<Schematix.FSM.My_Line> out_lines = new List<My_Line>();
                //это список линий, которые выходят из данного состояния
                //первым делом нужно построить данный список
                out_lines = graph.Lines.ToList().FindAll(r => r.state_begin == state);

                //теперь нам нужно отсортировать список по приоритетам
                out_lines.Sort((r1, r2) => r1.priority.CompareTo(r2.priority));

                if (out_lines.Count != 0)
                {
                    bool start = false;
                    foreach (Schematix.FSM.My_Line line in out_lines)
                    {
                        //пишем условие
                        if (string.IsNullOrEmpty(line.condition) == false)
                        {
                            if (start == false)
                            {
                                start = true;
                                res.Append("\t\t\t if " + line.condition + "\n");
                            }
                            else
                            {
                                res.Append("\t\t\t elsif " + line.condition + "\n");
                            }
                            res.Append("\t\t\t\t then State_Next <= " + line.state_end.name + "; \n");
                        }
                        else
                        {
                            if (start == false)
                            {
                                res.Append("\t\t\t\t State_Next <= " + line.state_end.name + "; \n");
                            }
                            else
                            {
                                res.Append("\t\t\t else \n");
                                res.Append("\t\t\t\t State_Next <= " + line.state_end.name + "; \n");
                            }
                        }
                        if (string.IsNullOrEmpty(line.Action) == false)
                        {
                            res.Append("\t\t-- действия при переходе\n");
                            res.Append("\t\t\t" + line.Action + "\n");
                            res.Append("\t\t------------------------\n");
                        }
                    }
                    if (start == true)
                        res.Append("\t\t\t end if; \n");
                }
            }
            res.Append("\t\t end case; \n");
            if (graph.Reset != null)
            {
                if(graph.Reset.res_type == My_Reset.Reset_Type.Synchonous)
                    res.Append("\t\t end if;\n \t\t end if;\n");
                else
                    res.Append("\t\t end if;\n");
            }
            //res.Append("\t end process; \n\n");
            return res.ToString();
        }

        private string Generate_process2()
        {
            StringBuilder res = new StringBuilder();
            My_Port ClockedPort = graph.ClockPort;
            string argument = "";
            if(ClockedPort != null)
                argument= ClockedPort.name;
            //res.Append("\t process (" + argument + ") is \n");
            //res.Append("\t begin \n");
            res.Append("\t\t if rising_edge(" + argument + ")\n");
            res.Append("\t\t\t then State <= State_Next; \n");
            res.Append("\t\t end if; \n");
            //res.Append("\t end process; \n\n");
            return res.ToString();
        }

        private string Generate_process3()
        {
            StringBuilder res = new StringBuilder();
            My_Port ClockedPort = graph.ClockPort;
            //string argument = "";
            //if (ClockedPort != null)
            //    argument = ClockedPort.name;
            //res.Append("\t process (" + argument + ") is \n");
            //res.Append("\t begin \n");

            res.Append("\t\t case State is \n");
            //перебираем список состояний
            foreach (Schematix.FSM.My_State state in graph.States)
            {
                res.Append("\t\t\t when " + state.name + " => \n");
                if (string.IsNullOrEmpty(state.ActivityInput) == false)
                {
                    res.Append("\t\t\t" + state.ActivityInput + "\n");
                }
                if (string.IsNullOrEmpty(state.condition) == false)
                {
                    res.Append("\t\t\t" + state.condition + "\n");
                }
                if (string.IsNullOrEmpty(state.ActivityExit) == false)
                {
                    res.Append("\t\t\t" + state.ActivityExit + "\n");
                }
            }

            res.Append("\t\t end case; \n");
            res.Append("\t end process; \n");

            return res.ToString();
        }

        public string GenerateCode()
        {
            use = Generate_Use();
            entity = Generate_Entity();
            declaration = Generate_Declaration();
            process1 = Generate_process1();
            process2 = Generate_process2();
            process3 = Generate_process3();

            StringBuilder res = new StringBuilder();
            res.Append(use);
            res.Append(entity + "\n\n");
            res.Append("architecture " + graph.VHDLModule.ArchitectureName + " of " + graph.VHDLModule.EntityName + " is \n");
            res.Append(declaration);
            res.Append(process1);
            res.Append(process2);
            res.Append(process3);
            res.Append("end " + graph.VHDLModule.ArchitectureName + ";");

            return res.ToString();
        }        
    }
}