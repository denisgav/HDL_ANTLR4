using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.EntityDrawning
{
    public class EntityDrawningInformationList
    {
        public List<EntityDrawningInfo> InfoList;
        public EntityDrawningInformationList()
        {
            InfoList = new List<EntityDrawningInfo>();
        }

        public void OpenFile(string FileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            InfoList = (List<Schematix.EntityDrawning.EntityDrawningInfo>)formatter.Deserialize(stream);
            stream.Close();
        }

        public void SaveFile(string FileName)
        {
            Stream stream = File.Create(FileName);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, InfoList);
            stream.Close();
        }
    }
}