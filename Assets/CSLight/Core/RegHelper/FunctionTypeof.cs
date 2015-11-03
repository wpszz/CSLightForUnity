using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class FunctionTypeof : ICLS_Function
    {
        public string keyword
        {
            get { return "typeof"; }
        }

        public CLS_Content.Value Call(CLS_Content content, BetterList<CLS_Content.Value> param)
        {
            return new CLS_Content.Value() { type = typeof(Type), value = (Type)content.environment.GetTypeByKeyword((string)param[0].value).type };
        }
    }
}
