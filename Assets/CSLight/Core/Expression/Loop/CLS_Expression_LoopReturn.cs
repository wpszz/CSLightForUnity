using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_LoopReturn : ICLS_Expression
    {
        public CLS_Expression_LoopReturn(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
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
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            CLS_Content.Value rv;
            if (listParam.Count > 0 && listParam[0] != null)
            {
                rv = listParam[0].ComputeValue(content);
            }
            else
            {
                rv = new CLS_Content.Value();
                rv.type = typeof(void);
            }
            rv.breakBlock = 3;
            content.OutStack(this);
            return rv;
        }

        public override string ToString()
        {
            return "return ...";
        }
    }
}