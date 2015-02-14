using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Schematix.Dialogs.NewFileDialogWizard;
using csx;
using Schematix.Core;
using Schematix.FSM;

namespace Schematix.Windows.FSM
{
    class FSM_Utils
    {
        /// <summary>
        /// Создание порта по имеющимся данным с VHDL_Port
        /// </summary>
        /// <param name="info"></param>
        /// <param name="center_point"></param>
        /// <param name="core"></param>
        /// <returns></returns>
        public static My_Port CreatePort(VHDL_Port info, Point center_point, Constructor_Core core)
        {
            My_Port res = new My_Port(info.Name, info.Type, center_point, core);
            switch (info.Direction)
            {
                case VHDLPortDirection.In:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.In;
                    break;
                case VHDLPortDirection.Out:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.Out;
                    break;
                case VHDLPortDirection.InOut:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.InOut;
                    break;
                case VHDLPortDirection.Buffer:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.Buffer;
                    break;
            }
            return res;
        }

        /// <summary>
        /// Создание порта по имеющимся данным с Verilog_Port
        /// </summary>
        /// <param name="info"></param>
        /// <param name="center_point"></param>
        /// <param name="core"></param>
        /// <returns></returns>
        public static My_Port CreatePort(Verilog_Port info, Point center_point, Constructor_Core core)
        {
            My_Port res = new My_Port(info.Name, info.Type, center_point, core);

            switch (info.Direction)
            {
                case VerilogPortDirection.In:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.In;
                    break;
                case VerilogPortDirection.Out:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.Out;
                    break;
                case VerilogPortDirection.InOut:
                    res.Direction = Schematix.FSM.My_Port.PortDirection.InOut;
                    break;
            }

            return res;
        }

        #region Инициализация данных
        /// <summary>
        /// Иницализация данных при зоздании FSM с языком VHDL
        /// </summary>
        /// <param name="options"></param>
        /// <param name="vhdl_module"></param>
        public static void InitVHDLData(
                        FSM_OptionsHelper options,
                        VHDL_Module vhdl_module,
                        Constructor_Core core

            )
        {
            //загружаем данные VHDL
            //Добавляем порты
            List<My_Port> NewPortsFSM = new List<My_Port>();
            Point pt = new Point(50, 50);
            foreach (VHDL_Port p in vhdl_module.PortList)
            {
                My_Port new_port = CreatePort(p, pt, core);
                NewPortsFSM.Add(new_port);
                pt.Y += 40;
            }
            core.Graph.AddFigureRange(NewPortsFSM);
            //-----------------------------

            core.Graph.VHDLModule = vhdl_module;

            //добавляем состояния
            if (options.NumberOfStates != 0)
            {
                #region Add States
                switch (options.StatesLayout)
                {
                    case FSM_OptionsHelper.FSMStatesLayout.LinearHorisontal:
                        {
                            pt = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            int Distance = 100;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);
                                pt.X += Distance;
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMStatesLayout.LinearVertical:
                        {
                            pt = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            int Distance = 100;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);
                                pt.Y += Distance;
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMStatesLayout.Circular:
                        {
                            Point Start_Point = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            double angle = 0;
                            int Radius = (options.NumberOfStates / 4 + 1) * 50;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                pt.X = (int)(Radius * Math.Cos(angle)) + Radius + Start_Point.X;
                                pt.Y = (int)(Radius * Math.Sin(angle)) + Radius + Start_Point.Y;

                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);

                                angle += (2 * Math.PI / (double)options.NumberOfStates);
                            }
                        }
                        break;
                    default:
                        break;
                }
                #endregion;

                #region Add Transitions
                switch (options.Transition)
                {
                    case FSM_OptionsHelper.FSMTransition.None:
                        { }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Forward:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s1 = core.Graph.States[i];
                                My_State s2 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line = new My_Line(s1, s2, string.Format("L{0}", i), "", core);
                                core.Graph.AddFigure(line);
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Backward:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s2 = core.Graph.States[i];
                                My_State s1 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line = new My_Line(s1, s2, string.Format("L{0}", i), "", core);
                                core.Graph.AddFigure(line);
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Both:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s1 = core.Graph.States[i];
                                My_State s2 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line1 = new My_Line(s1, s2, string.Format("L{0}", 2 * i), "", core);
                                core.Graph.AddFigure(line1);
                                My_Line line2 = new My_Line(s2, s1, string.Format("L{0}", 2 * i + 1), "", core);
                                core.Graph.AddFigure(line2);
                                core.Graph.UpdateLinesAggle(s1, s2);
                            }
                        }
                        break;
                    default:
                        break;
                }
                #endregion

                #region AddReset
                #endregion
            }
            //-----------------------------
        }


        /// <summary>
        /// Иницализация данных при зоздании FSM с языком Verilog
        /// </summary>
        /// <param name="options"></param>
        /// <param name="verilog_module"></param>
        public static void InitVerilogData(
                        FSM_OptionsHelper options,
                        Verilog_Module verilog_module,
                        Constructor_Core core
            )
        {
            //загружаем данные Verilog
            //Добавляем порты
            List<My_Port> NewPortsFSM = new List<My_Port>();
            Point pt = new Point(50, 50);
            foreach (Verilog_Port p in verilog_module.PortList)
            {
                My_Port new_port = CreatePort(p, pt, core);
                NewPortsFSM.Add(new_port);
                pt.Y += 40;
            }
            core.Graph.AddFigureRange(NewPortsFSM);
            //-----------------------------

            core.Graph.VerilogModule = verilog_module;

            //добавляем состояния
            if (options.NumberOfStates != 0)
            {
                #region Add States
                switch (options.StatesLayout)
                {
                    case FSM_OptionsHelper.FSMStatesLayout.LinearHorisontal:
                        {
                            pt = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            int Distance = 100;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);
                                pt.X += Distance;
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMStatesLayout.LinearVertical:
                        {
                            pt = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            int Distance = 100;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);
                                pt.Y += Distance;
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMStatesLayout.Circular:
                        {
                            Point Start_Point = new Point(100, 70);
                            System.Drawing.Size size = new System.Drawing.Size(40, 40);
                            double angle = 0;
                            int Radius = (options.NumberOfStates / 4 + 1) * 50;
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                pt.X = (int)(Radius * Math.Cos(angle)) + Radius + Start_Point.X;
                                pt.Y = (int)(Radius * Math.Sin(angle)) + Radius + Start_Point.Y;

                                My_State state = new My_State(core, new Rectangle(pt, size), true);
                                state.name = string.Format("S{0}", i);
                                state.label_name.Text = string.Format("S{0}", i);
                                core.Graph.AddFigure(state);

                                angle += (2 * Math.PI / (double)options.NumberOfStates);
                            }
                        }
                        break;
                    default:
                        break;
                }
                #endregion;

                #region Add Transitions
                switch (options.Transition)
                {
                    case FSM_OptionsHelper.FSMTransition.None:
                        { }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Forward:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s1 = core.Graph.States[i];
                                My_State s2 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line = new My_Line(s1, s2, string.Format("L{0}", i), "", core);
                                core.Graph.AddFigure(line);
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Backward:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s2 = core.Graph.States[i];
                                My_State s1 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line = new My_Line(s1, s2, string.Format("L{0}", i), "", core);
                                core.Graph.AddFigure(line);
                            }
                        }
                        break;
                    case FSM_OptionsHelper.FSMTransition.Both:
                        {
                            for (int i = 0; i < options.NumberOfStates; i++)
                            {
                                My_State s1 = core.Graph.States[i];
                                My_State s2 = (i < (options.NumberOfStates - 1)) ? core.Graph.States[i + 1] : core.Graph.States[0];

                                My_Line line1 = new My_Line(s1, s2, string.Format("L{0}", 2 * i), "", core);
                                core.Graph.AddFigure(line1);
                                My_Line line2 = new My_Line(s2, s1, string.Format("L{0}", 2 * i + 1), "", core);
                                core.Graph.AddFigure(line2);
                                core.Graph.UpdateLinesAggle(s1, s2);
                            }
                        }
                        break;
                    default:
                        break;
                }
                #endregion

                #region AddReset
                #endregion
            }
            //-----------------------------
        }
        #endregion        
    }
}
