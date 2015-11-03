using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_DeleNonVoidAction<ReturnType, T, T1, T2> : RegHelper_Dele
    {
        /// <summary>
        /// 有返回值,同时带 3 个 参数的委托.
        /// </summary>
        /// <returns></returns>
        public delegate ReturnType NonVoidDelegate(T param0, T1 param1, T2 param2);

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
                dele = delegate(T param0, T1 param1, T2 param2)
                {
                    CLS_Content content = CLS_Content.NewContent(env);
#if UNITY_EDITOR
                    try{
#endif
                    content.CallThis = delefunc.callthis;
                    content.CallType = delefunc.calltype;
                    content.DefineAndSet(func._paramnames[0], func._paramtypes[0].type, param0);
                    content.DefineAndSet(func._paramnames[1], func._paramtypes[1].type, param1);
                    content.DefineAndSet(func._paramnames[2], func._paramtypes[2].type, param2);
                    CLS_Content.Value retValue = func.expr_runtime.ComputeValue(content);
                    CLS_Content.PoolContent(content);
                    return (ReturnType)retValue.value;
#if UNITY_EDITOR
                    }catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); return default(ReturnType);}
#endif
                };
            }
            else
            {
                dele = delegate(T param0, T1 param1, T2 param2) { return default(ReturnType); };
            }

            _dele = Delegate.CreateDelegate(this.type, dele.Target, dele.Method);
            return delefunc.cacheFunction(_dele);
        }

        public override Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            NonVoidDelegate dele;
            if (lambda.expr_func != null)
            {
                dele = delegate(T param0, T1 param1, T2 param2)
                {
#if UNITY_EDITOR
                    try{
#endif
                    lambda.content.DepthAdd();
                    lambda.content.DefineAndSet(lambda.paramNames[0], typeof(T), param0);
                    lambda.content.DefineAndSet(lambda.paramNames[1], typeof(T1), param1);
                    lambda.content.DefineAndSet(lambda.paramNames[2], typeof(T2), param2);
                    CLS_Content.Value retValue = lambda.expr_func.ComputeValue(lambda.content);
                    lambda.content.DepthRemove();
                    return (ReturnType)retValue.value;
#if UNITY_EDITOR
                    }catch (System.Exception ex) { lambda.content.environment.logger.Log_Error(ex.Message + "\n" + lambda.content.DumpStack() + ex); return default(ReturnType);}
#endif
                };
            }
            else
            {
                dele = delegate(T param0, T1 param1, T2 param2) { return default(ReturnType); };
            }

            return Delegate.CreateDelegate(this.type, dele.Target, dele.Method);
        }
    }
}