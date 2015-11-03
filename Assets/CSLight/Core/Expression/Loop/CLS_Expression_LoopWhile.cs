using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_LoopWhile : ICLS_Expression
    {
        public CLS_Expression_LoopWhile(int tbegin, int tend, int lbegin, int lend)
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
            set;
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
            set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            content.DepthAdd();

            CLS_Content.Value vrt = null;

            ICLS_Expression expr_while = listParam[0];
            ICLS_Expression expr_block = listParam[1];

            while ((bool)expr_while.ComputeValue(content).value)
            {
                if (expr_block != null)
                {
                    CLS_Content.Value v = expr_block.ComputeValue(content);
                    if (v != null)
                    {
                        if (v.breakBlock > 2)
                            vrt = v;
                        if (v.breakBlock > 1)
                            break;
                    }
                }
            }
            content.DepthRemove();
            content.OutStack(this);
            return vrt;
        }

        public override string ToString()
        {
            return "while(...)";
        }
    }
}