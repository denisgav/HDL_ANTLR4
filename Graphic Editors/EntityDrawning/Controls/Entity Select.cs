using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using csx;

namespace Schematix.EntityDrawning
{
    public partial class Entity_Select : Form
    {
        private My_FileAnalyzer analyzer;
        public Entity_Select(My_FileAnalyzer analyzer)
        {
            InitializeComponent();
            this.analyzer = analyzer;
            LoadData();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string name = (string)comboBoxEntity.SelectedItem;
            vhdEntity selected_entity = null;
            foreach (vhdEntity entty in analyzer.entities)
            {
                if (entty.name.Equals(name))
                {
                    selected_entity = entty;
                    break;
                }
            }
            analyzer.SelectedEntity = selected_entity;
            this.Close();
        }

        private void LoadData()
        {
            foreach (vhdEntity entty in analyzer.entities)
                comboBoxEntity.Items.Add(entty.name);
            comboBoxEntity.SelectedIndex = 0;

            imageList1.Images.Add(global::EntityDrawning.Resource.input_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.output_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.bidirectional_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.buffer_port);
        }

        private void comboBoxEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewPorts.Items.Clear();
            string name = (string)comboBoxEntity.SelectedItem;
            vhdEntity selected_entity = null;
            foreach (vhdEntity entty in analyzer.entities)
            {
                if (entty.name.Equals(name))
                {
                    selected_entity = entty;
                    break;
                }
            }

            foreach (vhdPort port in selected_entity.ports)
            {
                ListViewItem item = new ListViewItem(port.name);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, port.inout.ToString()));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, port.type.ToString()));
                switch (port.inout)
                {
                    case portInOut.In:
                        item.ImageIndex = 0;
                    break;

                    case portInOut.Out:
                        item.ImageIndex = 1;
                    break;

                    case portInOut.InOut:
                     item.ImageIndex = 2;
                    break;

                    case portInOut.Buffer:
                        item.ImageIndex = 3;
                    break;
                }
                listViewPorts.Items.Add(item);
            }
        }
    }
}
