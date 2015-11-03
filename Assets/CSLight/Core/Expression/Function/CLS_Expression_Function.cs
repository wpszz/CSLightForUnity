using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    // 委托/局部函数调用表达式
    public class CLS_Expression_Function : ICLS_Expression
    {
        public CLS_Expression_Function(int tbegin, int tend, int lbegin, int lend)
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
            set;
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

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
            for (int i = 0, count = listParam.Count; i < count; i++)
            {
                _params.Add(listParam[i].ComputeValue(content));
            }

            CLS_Content.Value retVal;
            SType.Function fun;

            if (content.CallType != null && content.CallType.functions.TryGetValue(funcname, out fun))
            {
                if (fun.bStatic)
                {
                    retVal = content.CallType.StaticCall(content, funcname, _params);
                }
                else
                {
                    retVal = content.CallType.MemberCall(content, content.CallThis, funcname, _params);
                }
            }
            else
            {
                retVal = content.Get(funcname);
                Delegate sysDele = retVal.value as Delegate;
                if (sysDele != null)
                {
                    object[] args = CLS_Content.ParamObjsArray[_params.size];
                    for (int i = 0; i < _params.size; i++)
                    {
                        args[i] = _params[i].value;
                    }
                    retVal = new CLS_Content.Value();
                    retVal.value = sysDele.DynamicInvoke(args);
                    if (retVal.value != null)
                        retVal.type = retVal.value.GetType();
                }
                else
                {
                    DeleFunction csleDele = retVal.value as DeleFunction;
                    if (csleDele.callthis != null)
                    {
                        retVal = csleDele.calltype.MemberCall(content, csleDele.callthis, csleDele.function, _params);
                    }
                    else
                    {
                        retVal = csleDele.calltype.StaticCall(content, csleDele.function, _params);
                    }
                }
            }
            CLS_Content.PoolParamList(_params);
            content.OutStack(this);
            return retVal;
        }

        public string funcname;

        public override string ToString()
        {
            return "LocalFunctionCall: " + funcname + "(params[" + listParam.Count + "])";
        }
    }
}