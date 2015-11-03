using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class FunctionTrace : ICLS_Function
    {
        public string keyword
        {
            get { return "trace"; }
        }

        public CLS_Content.Value Call(CLS_Content content, BetterList<CLS_Content.Value> param)
        {
            string output = "trace:";
            for (int i = 0; i < param.size; i++)
            {
                output += " " + param[i].value;
            }
            UnityEngine.Debug.Log(output);
            //content.environment.logger.Log(output);
            return CLS_Content.Value.Void;
        }
    }

}
