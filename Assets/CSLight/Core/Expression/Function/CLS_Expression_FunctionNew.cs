using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_FunctionNew: ICLS_Expression
    {
        public CLS_Expression_FunctionNew(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
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
            private set;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            private set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
            for (int i = 0, count = listParam.Count; i < count; i++)
            {
                _params.Add(listParam[i].ComputeValue(content));
            }
            CLS_Content.Value value = type.function.New(content, _params);
            CLS_Content.PoolParamList(_params);
            content.OutStack(this);
            return value;

        }
        public CSLE.ICLS_Type type;
  
        public override string ToString()
        {
            return "new|" + type.keyword + "(params[" + listParam.Count + ")";
        }
    }
}