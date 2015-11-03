using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public class CLS_Expression_Yield : ICLS_Expression
    {
        public CLS_Expression_Yield(int tbegin,int tend,int lbegin,int lend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
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
            CLS_Content.Value srcVal = listParam[0].ComputeValue(content);
            // 不能直接修改srcVal
            CLS_Content.Value val = new CLS_Content.Value();
            val.type = srcVal.type;
            val.value = srcVal.value;
            val.breakBlock = srcVal.breakBlock;
            val.breakBlock += 10;
#if UNITY_EDITOR
            if (val.value is System.Collections.IEnumerator)
                throw new Exception("Dont support yield return IEnumerator, please use StartCoroutine(IEnumerator) instead: " + content.DumpStack());
#endif
            return val;
        }

        public override string ToString()
        {
            return "Yield|" + listParam[0].ToString();
        }
    }
}