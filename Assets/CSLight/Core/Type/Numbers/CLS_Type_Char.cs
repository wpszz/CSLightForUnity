using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Char : CLS_Type_Number<char>
    {
        public CLS_Type_Char()
            : base("char")
        {

        }

        public override object DefValue
        {
            get { return (char)0; }
        }
    }
}
