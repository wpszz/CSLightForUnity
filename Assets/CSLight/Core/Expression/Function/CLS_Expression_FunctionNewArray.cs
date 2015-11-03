using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_FunctionNewArray : ICLS_Expression
    {
        public CLS_Expression_FunctionNewArray(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
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
            content.InStack(this);

            int arraySize = listParam[0] == null ? (listParam.Count - 1) : (int)listParam[0].ComputeValue(content).value;
            if (arraySize == 0)
                throw new Exception("不能创建0长度数组");

            int initValCount = listParam.Count - 1;
            object[] initVals = new object[initValCount];
            for (int i = 1; i < listParam.Count; i++)
            {
                initVals[i - 1] = listParam[i].ComputeValue(content).value;
            }

            CLS_Content.Value vcount = new CLS_Content.Value();
            vcount.type = typeof(int);
            vcount.value = arraySize;
            BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
            _params.Add(vcount);
            CLS_Content.Value newVal = type.function.New(content, _params);

            for (int i = 0; i < initValCount; i++)
            {
                type.function.IndexSet(content, newVal.value, i, initVals[i]);
            }

            CLS_Content.PoolParamList(_params);
            content.OutStack(this);
            return newVal;

        }
        public CSLE.ICLS_Type type;

        public override string ToString()
        {
            return "new|" + type.keyword + "(params[" + listParam.Count + ")";
        }
    }
}