using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_Block : ICLS_Expression
    {
        public CLS_Expression_Block(int tbegin,int tend,int lbegin,int lend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public CLS_Expression_Block(List<ICLS_Expression> _params, int tbegin, int tend, int lbegin, int lend)
        {
            listParam = _params;
            tokenBegin = tbegin;
            tokenEnd = tend;
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
            content.DepthAdd();
            CLS_Content.Value value = null;
            for (int i = 0, count = listParam.Count; i < count; i++)
            {
                value = listParam[i].ComputeValue(content);
                if (value != null && value.breakBlock != 0)
                    break;
            }
            content.DepthRemove();
            content.OutStack(this);
            return value;
        }

        public override string ToString()
        {
            return "Block|";
        }
    }
}