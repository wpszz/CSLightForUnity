using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Sbyte : CLS_Type_Number<sbyte>
    {
        public CLS_Type_Sbyte()
            : base("sbyte")
        {

        }

        public override object DefValue
        {
            get { return (sbyte)0; }
        }
    }
}
