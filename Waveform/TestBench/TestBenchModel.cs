using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Parser;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Schematix.Waveform;
using System.IO;
using Schematix.Waveform.UserControls;
using DataContainer;
using DataContainer.Generator;

namespace Schematix.Waveform.TestBench
{
    internal class TestBenchModel
    {
        private string fileName;//имя файла
        private string entityName, archName;
        private Entity entityCur;//выбранное entity
        private Parser.Parser parser;//парсер
        private List<PortItem> entity_ports = null;//порты выбранной entity
        private List<ArchitectureBlockDeclarativeItem> arch_signals = null;//сигнала архитектуры
        private List<PortGenerator> currentPorts = null;//
        private List<SignalGenerator> currentSignals = null;
        private List<PortGenerator> selectedPorts = null;
        private List<SignalGenerator> selectedSignals = null;
        private string testBench;

        public TestBenchModel(string fileName, string entityName, string archName)
        {
            this.fileName = fileName;
            this.entityName = entityName;
            this.archName = archName;
            currentPorts = new List<PortGenerator>();
            currentSignals = new List<SignalGenerator>();
            selectedPorts = new List<PortGenerator>();
            selectedSignals = new List<SignalGenerator>();

            parser = new Parser.Parser(fileName);
            parser.Parse();

        }

        public void FillEntityCombobox(System.Windows.Forms.ComboBox cb)
        {
            foreach (Entity en in parser.EntityList)
            {
                cb.Items.Add(en.name);
            }
            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }
        }

        public void FillArchCombobox(System.Windows.Forms.ComboBox cb, string entity)
        {
            cb.Items.Clear();
            foreach (Architecture arch in parser.ArchitectureList)
            {
                if (entity.Equals(arch.entity_name))
                {
                    cb.Items.Add(arch.name);
                }
            }
            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }
        }

        public void FillPortsListView(System.Windows.Forms.ListView lv, string entity, string arch)
        {
            lv.Items.Clear();
            foreach (Entity en in parser.EntityList)
            {
                if (entity.ToLower().Equals(en.name))
                {
                    entity_ports = en.Port_items;
                    entityCur = en;
                    break;
                }
            }
            if (!arch.Equals(""))
            {
                foreach (Architecture ar in parser.ArchitectureList)
                {
                    if (arch.Equals(ar.name))
                    {
                        arch_signals = ar.architecture_items;
                        break;
                    }
                }
            }
            AddEntities(lv, entity_ports);
            if (!arch.Equals(""))
            {
                //AddArchs(lv,arch_signals);
            }
            lv.Visible = true;
        }

        private void AddEntities(System.Windows.Forms.ListView lv, List<PortItem> eps)
        {
            currentPorts.Clear();
            int countEn = 0;
            foreach (PortItem pi in eps)
            {
                countEn = 0;
                foreach (string s in pi.id_list)
                {
                    if (pi.interface_mode != InterfaceMode._out && pi.interface_mode != InterfaceMode._not_defined && pi.interface_mode != InterfaceMode._buffer)
                    {
                        string[] mCList = new String[4];
                        mCList[0] = s;
                        mCList[1] = pi.interface_mode.ToString();
                        mCList[2] = pi.subtype.sub_type_indication_string;
                        if (pi.subtype.hasRange)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("( ").Append(pi.subtype.range.leftRange);
                            if (pi.subtype.range.isReverseRange)
                                sb.Append(" downto ");
                            else
                                sb.Append(" to ");
                            sb.Append(pi.subtype.range.rightRange).Append(" )");
                            mCList[3] = sb.ToString();
                        }
                        else
                            mCList[3] = "single";
                        lv.Items.Add(new System.Windows.Forms.ListViewItem(mCList));
                        //currentPorts.Add(new PortGenerator(entityCur,countEn, pi, null));
                        //countEn++;
                    }
                    currentPorts.Add(new PortGenerator(entityCur, countEn, pi, null));
                    countEn++;
                }

            }
            //System.Windows.Forms.MessageBox.Show(currentPorts.Count.ToString());
        }
        private void AddArchs(System.Windows.Forms.ListView lv, List<ArchitectureBlockDeclarativeItem> arcs)
        {
            int countArch = 0;
            foreach (ArchitectureBlockDeclarativeItem ai in arcs)
            {
                countArch = 0;
                foreach (string s in ai.id_list)
                {
                    string[] mCList = new String[4];
                    mCList[0] = s;
                    mCList[1] = ai.interface_type.ToString();
                    mCList[2] = ai.subtype.sub_type_indication_string;
                    if (ai.subtype.hasRange)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("(").Append(ai.subtype.range.leftRange);
                        if (ai.subtype.range.isReverseRange)
                            sb.Append("downto");
                        else
                            sb.Append("to");
                        sb.Append(ai.subtype.range.rightRange).Append(")");
                        mCList[3] = sb.ToString();
                    }
                    else
                        mCList[3] = "single";
                    lv.Items.Add(new System.Windows.Forms.ListViewItem(mCList));
                    currentSignals.Add(new SignalGenerator(countArch, ai, null));
                    countArch++;
                }
            }
        }
        public void GetSelectListView(System.Windows.Forms.ListView lv)
        {
            selectedPorts.Clear();
            System.Windows.Forms.ListView.CheckedListViewItemCollection check = lv.CheckedItems;
            List<string> selectNames = new List<string>();
            foreach (System.Windows.Forms.ListViewItem item in check)
            {
                //selectNames.Add(item.SubItems[0].Text);
                foreach (PortGenerator pg in currentPorts)
                {
                    if (pg.Name == item.SubItems[0].Text)
                        selectedPorts.Add(pg);
                }
                foreach (SignalGenerator sg in currentSignals)
                {
                    if (sg.Name == item.SubItems[0].Text)
                        selectedSignals.Add(sg);
                }
            }
        }

        public void FillPortsGeneratorListView(System.Windows.Forms.ListView lv)
        {
            lv.Items.Clear();
            foreach (PortGenerator pg in selectedPorts)
            {
                string[] mCList = new String[2];
                mCList[0] = pg.Name;
                if (pg.Generator != null)
                    mCList[1] = pg.Generator.ToString();
                else
                    mCList[1] = "";
                lv.Items.Add(new System.Windows.Forms.ListViewItem(mCList));
            }
            foreach (SignalGenerator sg in selectedSignals)
            {
                string[] mCList = new String[2];
                mCList[0] = sg.Name;
                if (sg.Generator != null)
                    mCList[1] = sg.Generator.ToString();
                else
                    mCList[1] = "";
                lv.Items.Add(new System.Windows.Forms.ListViewItem(mCList));
            }
        }

        public void SetGenerators(System.Windows.Forms.ListView lv)
        {
            GeneratorSettings set;
            GeneratorDialog dialog = new GeneratorDialog();


            foreach (PortGenerator pg in selectedPorts)
            {
                if (pg.Name == lv.SelectedItems[0].Text)
                {
                    switch (pg.Port.subtype.sub_type_indication_string.ToLower())
                    {
                        case "std_logic":
                            set = new GeneratorSettings(1, GeneratorSettings.GeneratedValue.EnumerableValue, GeneratorSettings.GeneratedValue.EnumerableValue, new List<object>(VHDL.builtin.StdLogic1164.STD_ULOGIC.Literals));
                            //dialog = new GeneratorDialog(set);
                            dialog.GenSettings = set;
                            dialog.ShowDialog();
                            pg.Generator = dialog.Generator;
                            break;
                        case "std_logic_vector":
                            set = new GeneratorSettings((uint)(Math.Abs(pg.Port.subtype.range.leftRange - pg.Port.subtype.range.rightRange) + 1), GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, GeneratorSettings.GeneratedValue.IntegerValue);
                            //set = new GeneratorSettings((uint)(Math.Abs(pg.Port.subtype.range.leftRange - pg.Port.subtype.range.rightRange) + 1), GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, GeneratorSettings.GeneratedValue.BoolArray);
                            //dialog = new GeneratorDialog(set);
                            dialog.GenSettings = set;
                            dialog.ShowDialog();
                            pg.Generator = dialog.Generator;
                            break;
                        case "bit":
                            set = new GeneratorSettings(1, GeneratorSettings.GeneratedValue.EnumerableValue, GeneratorSettings.GeneratedValue.EnumerableValue, new List<object>(VHDL.builtin.Standard.BIT.Literals));
                            //dialog = new GeneratorDialog(set);
                            //dialog = new GeneratorDialog(set);
                            dialog.GenSettings = set;
                            dialog.ShowDialog();
                            pg.Generator = dialog.Generator;
                            break;
                        case "bit_vector":
                            set = new GeneratorSettings((uint)(Math.Abs(pg.Port.subtype.range.leftRange - pg.Port.subtype.range.rightRange) + 1), GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, GeneratorSettings.GeneratedValue.BoolArray);
                            //dialog = new GeneratorDialog(set);
                            dialog.GenSettings = set;
                            dialog.ShowDialog();
                            pg.Generator = dialog.Generator;
                            break;
                        default: 
                            System.Windows.Forms.MessageBox.Show("Generators can work only with STD_LOGIC,STD_LOGIC_VECTOR,BIT,BIT_VECTOR types.");
                            return;
                            break;
                    }
                }
            }
        }

        public void GenerateTestBench(TimeInterval ti)
        {
            testBench = TestBenchUtil.GenerateTestbenchAsString(currentPorts, selectedPorts,archName, ti);
        }

        public void SaveTestBenchToFile(string saveFile)
        {
            StreamWriter writer = File.CreateText(saveFile);
            writer.Write(testBench);
            writer.Close();
        }

        public void SaveTestBenchToFile( TimeInterval ti, StreamWriter writer)
        {
            TestBenchUtil.GenerateTestbenchAsStream(currentPorts, selectedPorts, archName,ti, writer);
        }
        public void SaveTestBenchToFile(TimeInterval ti, String testbenchFileName)
        {
            StreamWriter sw = File.CreateText(testbenchFileName);
            TestBenchUtil.GenerateTestbenchAsStream(currentPorts, selectedPorts,archName, ti, sw);
            sw.Close();
        }

        public Entity Entity
        {
            get { return this.entityCur; }
            set { entityCur = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }



        public List<PortGenerator> CurrentPorts
        {
            get { return currentPorts; }
        }

        public List<PortGenerator> SelectedPorts
        {
            get { return selectedPorts; }
        }

        public string TestBench
        {
            get { return testBench; }
        }


    }
}
