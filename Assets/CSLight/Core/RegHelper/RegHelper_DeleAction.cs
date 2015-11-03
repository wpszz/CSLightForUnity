using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_DeleAction : RegHelper_Dele
    {
        public RegHelper_DeleAction(Type type, string setkeyword)
            : base(type, setkeyword)
        {

        }

        public override Delegate CreateDelegate(ICLS_Environment env, DeleFunction delefunc)
        {
            Delegate _dele = delefunc.cacheFunction(null);
            if (_dele != null) 
                return _dele;

            var func = delefunc.calltype.functions[delefunc.function];
            Action dele;
            if (func.expr_runtime != null)
            {
                dele = () =>
                {
                    CLS_Content content = CLS_Content.NewContent(env);
#if UNITY_EDITOR
                    try{
#endif
                    content.CallThis = delefunc.callthis;
                    content.CallType = delefunc.calltype;
                    func.expr_runtime.ComputeValue(content);
                    CLS_Content.PoolContent(content);
#if UNITY_EDITOR
                    }catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                };
            }
            else
            {
                dele = () => { };
            }

            if (this.sysType != typeof(Action))
            {
                _dele = Delegate.CreateDelegate(this.sysType, dele.Target, dele.Method);
            }
            else
            {
                _dele = dele;
            }
            return delefunc.cacheFunction(_dele);
        }

        public override Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            Action dele;
            if (lambda.expr_func != null)
            {
                dele = () =>
                {
#if UNITY_EDITOR
                    try{
#endif
                    lambda.expr_func.ComputeValue(lambda.content);
#if UNITY_EDITOR
                    }catch (System.Exception ex) { lambda.content.environment.logger.Log_Error(ex.Message + "\n" + lambda.content.DumpStack() + ex); }
#endif
                };
            }
            else
            {
                dele = () => { };
            }

            if (this.sysType != typeof(Action))
            {
                return Delegate.CreateDelegate(this.sysType, dele.Target, dele.Method);
            }
            else
            {
                return dele;
            }
        }
    }
}
