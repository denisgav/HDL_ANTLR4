using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using DataContainer;

namespace Schematix.Waveform.TestBench
{
    public partial class TestBench_Dialog : Form
    {
        //test
        private StringBuilder sb;
        
        private string fileName;
        TestBenchModel tbm = null;

        /// <summary>
        /// Путь к созданному файлу TestBench
        /// </summary>
        private string testBenchFileName;
        public string TestBenchFileName
        {
            get { return testBenchFileName; }
        }

       
        public TestBench_Dialog(string fileName, string entityName, string archName)
        {
            InitializeComponent();
         
            groupBox1.SetBounds(groupBox1.Bounds.X,groupBox1.Bounds.Y,groupBox1.Bounds.Width,310);
            groupBox2.Visible = false;
            //button2.Enabled =true;
            button3.Enabled = true;
            button5.Enabled = true;
            //comboBox2.Enabled = false;
            ClearElements();

            textBoxFileName.Text = entityName + "_tb";
            
                this.fileName = fileName;
                //textBox1.Text = this.fileName;
                tbm = new TestBenchModel(fileName,entityName,archName);
                tbm.FillPortsListView(listView1, entityName, "");
                //tbm.FillEntityCombobox(comboBox1);
                //tbm.FillArchCombobox(comboBox2, comboBox1.SelectedItem.ToString());
           


            //button2.Enabled = true;
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
                ClearElements();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    textBox1.Text = fileName;
                    tbm = new TestBenchModel(fileName);
                    tbm.FillEntityCombobox(comboBox1);
                    tbm.FillArchCombobox(comboBox2, comboBox1.SelectedItem.ToString());
                }
              
                
                button2.Enabled = true;
            

        }*/
       
        private void ClearElements()
        {
            //comboBox1.Items.Clear();
            //comboBox1.Items.Clear();
            listView1.Items.Clear();
            listView1.Visible = false;
        }
       /* private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            tbm.FillArchCombobox(comboBox2,comboBox1.SelectedItem.ToString());
            listView1.Items.Clear();
            button2.Enabled = true;
        }*/

       /* private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                tbm.FillPortsListView(listView1, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
                button2.Enabled = false;
            }
            catch (Exception exc)
            {
                tbm.FillPortsListView(listView1, comboBox1.SelectedItem.ToString(), "");
                button2.Enabled = false;
            }
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            foreach(System.Windows.Forms.ListViewItem it in listView1.Items)
                it.Checked = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.CheckedListViewItemCollection check = listView1.CheckedItems;
            /*StringBuilder sb = new StringBuilder();
            foreach (System.Windows.Forms.ListViewItem item in check)
            {
                sb.Append(item.SubItems[0].Text).Append("\n");
            }*/
            groupBox1.Visible = false;
            
            tbm.GetSelectListView(listView1);
            tbm.FillPortsGeneratorListView(listView2);

            groupBox2.SetBounds(groupBox1.Bounds.X, groupBox1.Bounds.Y, groupBox1.Bounds.Width, 310);
            groupBox2.Visible = true;
            this.Text = "Step2";
            button3.Visible = false;
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            tbm.SetGenerators(listView2);
            tbm.FillPortsGeneratorListView(listView2);
            /*int counter = 0;
            foreach (System.Windows.Forms.ListViewItem li in listView2.Items)
            {
                if (li.SubItems[1].Text != "")
                {
                    counter++;
                }
                
            }
            if (counter == listView2.Items.Count)
                button5.Enabled = true;
             */
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //button3.Enabled = (listView1.CheckedItems.Count > 0);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TimeInterval ti;

                switch (comboBox3.Text)
                {
                    case "s": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.s);
                        break;
                    case "us": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.us);
                        break;
                    case "ps": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.ps);
                        break;
                    case "ns": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.ns);
                        break;
                    case "ms": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.ms);
                        break;
                    case "fs": ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.fs);
                        break;
                    default: ti = new TimeInterval(Convert.ToInt32(numericUpDown1.Value), TimeUnit.ns); //default TimeUnit!!!
                        break;
                }

                tbm.GenerateTestBench(ti);
                //richTextBox1.Text = tbm.TestBench;
                if (string.IsNullOrEmpty(textBoxFileName.Text) == false)
                {
                    string fileName =  System.IO.Path.GetDirectoryName(tbm.FileName) + "\\" + textBoxFileName.Text + ".vhdl";
                    StreamWriter writer = File.CreateText(fileName);
                    tbm.SaveTestBenchToFile(ti, writer);
                    /*tbm.GenerateTestBench(ti);
                    writer.Write( tbm.TestBench);*/
                    writer.Close();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    testBenchFileName = fileName;
                    Close();
                }

        }
        
    }
}
