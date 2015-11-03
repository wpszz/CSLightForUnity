using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_LoopIf : ICLS_Expression
    {
        public CLS_Expression_LoopIf(int tbegin, int tend, int lbegin, int lend)
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

            CLS_Content.Value value = null;
            if ((bool)listParam[0].ComputeValue(content).value)
            {
                ICLS_Expression expr_block = listParam[1];
                if (expr_block != null)
                {
                    if (expr_block is CLS_Expression_Block)
                    {
                        value = expr_block.ComputeValue(content);
                    }
                    else
                    {
                        content.DepthAdd();
                        value = expr_block.ComputeValue(content);
                        content.DepthRemove();
                    }
                }
            }
            else if (listParam.Count > 2)
            {
                ICLS_Expression expr_elseif = listParam[2];
                if (expr_elseif != null)
                {
                    if (expr_elseif is CLS_Expression_Block)
                    {
                        value = expr_elseif.ComputeValue(content);
                    }
                    else
                    {
                        content.DepthAdd();
                        value = expr_elseif.ComputeValue(content);
                        content.DepthRemove();
                    }
                }
            }

            content.OutStack(this);
            return value;
        }

        public override string ToString()
        {
            return "if(...)";
        }
    }
}