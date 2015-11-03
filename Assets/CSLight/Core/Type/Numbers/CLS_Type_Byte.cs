using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Byte : CLS_Type_Number<byte>
    {
        public CLS_Type_Byte()
            : base("byte")
        {
        }
        public override object DefValue
        {
            get { return (byte)0; }
        }
    }
}
