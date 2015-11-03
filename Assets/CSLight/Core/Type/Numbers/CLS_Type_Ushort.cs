using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_UShort : CLS_Type_Number<ushort>
    {
        public CLS_Type_UShort()
            : base("ushort")
        {

        }

        public override object DefValue
        {
            get { return (ushort)0; }
        }
    }
}
