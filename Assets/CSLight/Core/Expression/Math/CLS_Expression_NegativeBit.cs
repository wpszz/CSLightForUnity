using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_NegativeBit : ICLS_Expression
    {
        public CLS_Expression_NegativeBit(int tbegin, int tend, int lbegin, int lend)
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

            CLS_Content.Value oriVal = listParam[0].ComputeValue(content);

            CLS_Content.Value retVal = new CLS_Content.Value();

            ICLS_Type type = content.environment.GetType(oriVal.type);
            retVal.value = type.Math2Value(content, '~', oriVal.value, CLS_Content.Value.One, out retVal.type);

            content.OutStack(this);

            return retVal;
        }

        public override string ToString()
        {
            return "~a" ;
        }
    }
}