using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public interface ICLS_Function
    {
        string keyword
        {
            get;
        }

        CLS_Content.Value Call(CLS_Content content, BetterList<CLS_Content.Value> param);
    }
}
