using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Short : CLS_Type_Number<short>
    {
        public CLS_Type_Short()
            : base("short")
        {

        }

        public override object DefValue
        {
            get { return (short)0; }
        }
    }
}
