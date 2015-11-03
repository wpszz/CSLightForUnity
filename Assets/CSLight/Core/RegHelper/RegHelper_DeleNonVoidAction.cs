using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_DeleNonVoidAction<ReturnType> : RegHelper_Dele
    {
        /// <summary>
        /// 有返回值,同时不带参数的委托.
        /// </summary>
        /// <returns></returns>
        public delegate ReturnType NonVoidDelegate();

        public RegHelper_DeleNonVoidAction(Type type, string setkeyword)
            : base(type, setkeyword)
        {

        }

        public override Delegate CreateDelegate(ICLS_Environment env, DeleFunction delefunc)
        {
            Delegate _dele = delefunc.cacheFunction(null);
            if (_dele != null) 
                return _dele;

            var func = delefunc.calltype.functions[delefunc.function];
            NonVoidDelegate dele;
            if (func.expr_runtime != null)
            {
                dele = delegate()
                {
                    CLS_Content content = CLS_Content.NewContent(env);
#if UNITY_EDITOR
                    try{
#endif
                    content.CallThis = delefunc.callthis;
                    content.CallType = delefunc.calltype;
                    CLS_Content.Value retValue = func.expr_runtime.ComputeValue(content);
                    CLS_Content.PoolContent(content);
                    return (ReturnType)retValue.value;
#if UNITY_EDITOR
                    }catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); return default(ReturnType); }
#endif
                };
            }
            else
            {
                dele = delegate() { return default(ReturnType); };
            }

            _dele = Delegate.CreateDelegate(this.type, dele.Target, dele.Method);
            return delefunc.cacheFunction(_dele);
        }

        public override Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            NonVoidDelegate dele;
            if (lambda.expr_func != null)
            {
                dele = delegate()
                {
#if UNITY_EDITOR
                    try{
#endif
                    CLS_Content.Value retValue = lambda.expr_func.ComputeValue(lambda.content);
                    return (ReturnType)retValue.value;
#if UNITY_EDITOR
                    }catch (System.Exception ex) { lambda.content.environment.logger.Log_Error(ex.Message + "\n" + lambda.content.DumpStack() + ex); return default(ReturnType);}
#endif
                };
            }
            else
            {
                dele = delegate() { return default(ReturnType); };
            }

            return Delegate.CreateDelegate(this.type, dele.Target, dele.Method);
        }
    }
}
