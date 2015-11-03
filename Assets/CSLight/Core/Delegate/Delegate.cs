using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class DeleFunction   // 指向脚本中的函数
    {
        public SType calltype;
        public SInstance callthis;
        public string function;

        public DeleFunction(SType calltype, SInstance callthis, string function)
        {
            this.calltype = calltype;
            this.callthis = callthis;
            this.function = function;
        }

        public Delegate cacheFunction(Delegate dele)
        {
            if (dele == null)
            {
                if (callthis != null)
                {
                    Delegate v = null;
                    callthis.cacheDeles.TryGetValue(function, out v);
                    return v;
                }
                else
                {
                    return calltype.functions[function].staticDele;
                }
            }
            else
            {
                if (callthis != null)
                {
                    callthis.cacheDeles[function] = dele;
                }
                else
                {
                    calltype.functions[function].staticDele = dele;
                }
                return dele;
            }
        }
    }

    public class DeleLambda     // 指向Lambda表达式
    {
        public List<Type> paramTypes = new List<Type>();
        public List<string> paramNames = new List<string>();
        public CLS_Content content;
        public ICLS_Expression expr_func;

        public DeleLambda(CLS_Content content, IList<ICLS_Expression> param, ICLS_Expression func)
        {
            this.content = content.Clone();
            this.expr_func = func;
            for (int i = 0, count = param.Count; i < count; i++)
            {
                CLS_Expression_GetValue v1 = param[i] as CLS_Expression_GetValue;
                if (v1 != null)
                {
                    paramTypes.Add(null);
                    paramNames.Add(v1.value_name);
                    continue;
                }
                CLS_Expression_Define v2 = param[i] as CLS_Expression_Define;
                if (v2 != null)
                {
                    paramTypes.Add(v2.value_type);
                    paramNames.Add(v2.value_name);
                    continue;
                }
                throw new Exception("DeleLambda 参数不正确");
            }
        }
    }
}
