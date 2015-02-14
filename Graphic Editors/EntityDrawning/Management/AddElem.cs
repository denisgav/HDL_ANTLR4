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
    public partial class AddElem : Form
    {
        private Parser parser = new Schematix.EntityDrawning.Parser();
        public EntityDrawningInfo EntityDrawningInfo { get; set; }

        public AddElem()
        {
            InitializeComponent();
            LoadData();
        }

        public AddElem(string EDRPath)
        {
            InitializeComponent();
            textBoxProjectFile.Text = EDRPath;
            textBoxProjectFile.Enabled = false;
            buttonOpenProject.Enabled = false;
                
            LoadData();
        }

        private void LoadData()
        {
            imageList1.Images.Add(global::EntityDrawning.Resource.input_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.output_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.bidirectional_port);
            imageList1.Images.Add(global::EntityDrawning.Resource.buffer_port);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialogVHDL.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBoxVHDLFile.Text = openFileDialogVHDL.FileName;
                parser.Parsing(openFileDialogVHDL.FileName);
                if (parser.entities.Count != 0)
                {
                    comboBoxEntity.Items.Clear();
                    foreach (vhdEntity entity in parser.entities)
                    {
                        comboBoxEntity.Items.Add(entity.name);
                    }
                    comboBoxEntity.SelectedIndex = 0;
                }
            }
        }

        private void comboBoxEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = (string)comboBoxEntity.SelectedItem;
            foreach (vhdEntity entity in parser.entities)
            {
                if (entity.name.Equals(name))
                {
                    listViewPorts.Items.Clear();
                    foreach (vhdPort port in entity.ports)
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
                    break;
                }
            }
        }

        private void buttonOpenProject_Click(object sender, EventArgs e)
        {
            DialogResult res = saveFileDialogProject.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBoxProjectFile.Text = saveFileDialogProject.FileName;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            EntityDrawningInfo info = new EntityDrawningInfo();
            if(String.IsNullOrEmpty(textBoxVHDLFile.Text))
            {
                MessageBox.Show("You Must Set VHDL File", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (String.IsNullOrEmpty(textBoxProjectFile.Text))
            {
                MessageBox.Show("You Must Set Project File", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (String.IsNullOrEmpty((string)comboBoxEntity.SelectedItem))
            {
                MessageBox.Show("You Must Set Entity Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            string name = (string)comboBoxEntity.SelectedItem;
            foreach (vhdEntity entity in parser.entities)
            {
                if (entity.name.Equals(name))
                {
                    info.Entity = entity;
                    break;
                }
            }

            info.ProjectFileName = textBoxProjectFile.Text;
            info.VHDLFileName = textBoxVHDLFile.Text;

            this.EntityDrawningInfo = info;

            Close();
        }
    }
}
