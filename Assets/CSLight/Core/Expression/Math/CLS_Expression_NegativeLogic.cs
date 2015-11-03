using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_NegativeLogic : ICLS_Expression
    {
        public CLS_Expression_NegativeLogic(int tbegin, int tend, int lbegin, int lend)
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

            retVal.type = oriVal.type;
            retVal.breakBlock = oriVal.breakBlock;
            retVal.value = !(bool)oriVal.value;

            content.OutStack(this);

            return retVal;
        }

        public override string ToString()
        {
            return "!a" ;
        }
    }
}