using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.CommonProperties
{
    [Serializable()]
    public class FSMOptions: GraphicsOptions, IOptions, ISerializable
    {
        #region constructors
        public FSMOptions()
            : base() { }
        public FSMOptions(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt) { }
        #endregion
        #region IOptions Members

        public void LoadData(Options options)
        {
            options.SetOptionsData(this);
        }

        public void Accept(Options options)
        {
            options.GetOptionsData(this);
        }

        public override void SetDefault()
        {
            base.SetDefault();
        }

        #endregion

        #region ISerializable Members

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
