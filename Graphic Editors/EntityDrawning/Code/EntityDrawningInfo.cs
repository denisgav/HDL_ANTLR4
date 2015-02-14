using System;
using System.Runtime.Serialization.Formatters.Binary;
using csx;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class EntityDrawningInfo
    {
        public string VHDLFileName { get; set; }
        public string ProjectFileName { get; set; }
        public vhdEntity Entity { get; set; }

        public EntityDrawningInfo() { }

        public EntityDrawningInfo(string VHDLFileName, string ProjectFileName, vhdEntity Entity)
        {
            this.Entity = Entity;
            this.ProjectFileName = ProjectFileName;
            this.VHDLFileName = VHDLFileName;
        }

        public bool IsCorrect()
        {
            try
            {
                Parser parser = new Parser();
                parser.Parsing(VHDLFileName);
                vhdEntity find_entity = null;
                foreach (vhdEntity entity in parser.entities)
                {
                    if(entity.name.Equals(Entity.name)) //Мы нашли подходящее entity
                    {
                        find_entity = entity;
                        break;
                    }
                }

                if(find_entity == null)
                    return false;

                //Теперь проверяем порты
                if(Entity.ports.Count != find_entity.ports.Count)
                    return false;

                foreach(vhdPort port in find_entity.ports)
                {
                    bool res = false;
                    foreach(vhdPort port2 in Entity.ports)
                    {
                        if((port.name == port2.name) && (port.bus == port2.bus) && (port.inout == port2.inout) && (port.leftBound == port2.leftBound) && (port.rightBound == port2.rightBound) && (port.type == port2.type))
                        {
                            res = true;
                            break;
                        }
                    }
                    if(res == false)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Parsing Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Parsing Error.", ex);
                return false;
            }
        }
    }
}