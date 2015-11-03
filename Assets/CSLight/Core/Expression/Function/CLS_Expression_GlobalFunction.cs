using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public class CLS_Expression_GlobalFunction : ICLS_Expression
    {
        public CLS_Expression_GlobalFunction(List<ICLS_Expression> listParam, int tbegin, int tend, int lbegin, int lend)
        {
            this.listParam = listParam;
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            this.lineBegin = lbegin;
            this.lineEnd = lend;
        }

        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
            for (int i = 0, count = listParam.Count; i < count; i++)
            {
                _params.Add(listParam[i].ComputeValue(content));
            }
            CLS_Content.Value retVal = content.environment.GetFunction(funcname).Call(content, _params);
            CLS_Content.PoolParamList(_params);
            content.OutStack(this);
            return retVal;
        }

        public string funcname;

        public override string ToString()
        {
            return "GlobalFunctionCall: " + funcname + "(params[" + listParam.Count + "])";
        }
    }
}
