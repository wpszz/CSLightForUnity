using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_ULong : CLS_Type_Number<ulong>
    {
        public CLS_Type_ULong()
            : base("ulong")
        {

        }

        public override object DefValue
        {
            get { return (ulong)0; }
        }
    }
}
