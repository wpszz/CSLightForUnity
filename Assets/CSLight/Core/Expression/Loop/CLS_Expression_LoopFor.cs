using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_LoopFor : ICLS_Expression
    {
        public CLS_Expression_LoopFor(int tbegin, int tend, int lbegin, int lend)
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

            ICLS_Expression expr_init = listParam[0];
            ICLS_Expression expr_continue = listParam[1];
            ICLS_Expression expr_step = listParam[2];
            ICLS_Expression expr_block = listParam[3];

            if (expr_init != null)
                expr_init.ComputeValue(content);

            for (;;)
            {
                if (expr_continue != null && !(bool)expr_continue.ComputeValue(content).value) 
                    break;

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

                if (expr_step != null)
                    expr_step.ComputeValue(content);
            }
            content.DepthRemove();
            content.OutStack(this);
            return vrt;
        }

        public override string ToString()
        {
            return "for(...)";
        }
    }
}