using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Schematix_all;
using System.Windows.Forms;

namespace Schematix.Windows.MDIChild
{
    public class File
    {
        public string _name;
        public bool _saved;
        public bool _exist;
        private SchemaUserControl parent;

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        public bool exist
        {
            get
            {
                return this._exist;
            }
        }
        public File(SchemaUserControl parent)
        {
            this.parent = parent;

            this._name = "";
            this._saved = false;
            this._exist = false;
        }
        public bool Save()
        {
            FileStream fs = new FileStream(this._name, FileMode.Create, FileAccess.Write);
            bool result = this.parent.Save(fs);
            fs.Close();
            return result;
        }
        private bool Open()
        {
            FileStream fs = new FileStream(this._name, FileMode.Open, FileAccess.Read);
            bool result = this.parent.Open(fs);
            fs.Close();
            return result;
        }
    }
}
