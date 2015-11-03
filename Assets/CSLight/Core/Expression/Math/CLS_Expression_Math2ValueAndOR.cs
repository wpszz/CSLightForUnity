using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_Math2ValueAndOr : ICLS_Expression
    {
        public CLS_Expression_Math2ValueAndOr(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
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
            CLS_Content.Value result = new CLS_Content.Value();
            result.type = typeof(bool);

            bool bleft = (bool)listParam[0].ComputeValue(content).value;

            if (mathop == '&')
            {
                if (!bleft)
                {
                    result.value = false;
                }
                else
                {
                    result.value = bleft && (bool)listParam[1].ComputeValue(content).value;
                }
            }
            else if (mathop == '|')
            {
                if (bleft)
                {
                    result.value = true;
                }
                else
                {
                    result.value = bleft || (bool)listParam[1].ComputeValue(content).value;
                }
            }

            content.OutStack(this);
            return result;

        }

        public char mathop;

        public override string ToString()
        {
            return "Math2ValueAndOr: a" + mathop + "b";
        }
    }
}