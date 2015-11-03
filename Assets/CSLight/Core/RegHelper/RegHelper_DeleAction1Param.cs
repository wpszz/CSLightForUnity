﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_DeleAction<T> : RegHelper_Dele
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
            Action<T> dele;
            if (func.expr_runtime != null)
            {
                dele = (T param0) =>
                {
                    CLS_Content content = CLS_Content.NewContent(env);
#if UNITY_EDITOR
                    try{
#endif
                    content.CallThis = delefunc.callthis;
                    content.CallType = delefunc.calltype;
                    content.DefineAndSet(func._paramnames[0], func._paramtypes[0].type, param0);
                    func.expr_runtime.ComputeValue(content);
                    CLS_Content.PoolContent(content);
#if UNITY_EDITOR
                    }catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                };
            }
            else
            {
                dele = (T param0) => { };
            }

            if (this.sysType != typeof(Action<T>))
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
            Action<T> dele;
            if (lambda.expr_func != null)
            {
                dele = (T param0) =>
                {
#if UNITY_EDITOR
                    try{
#endif
                    lambda.content.DepthAdd();
                    lambda.content.DefineAndSet(lambda.paramNames[0], typeof(T), param0);
                    lambda.expr_func.ComputeValue(lambda.content);
                    lambda.content.DepthRemove();
#if UNITY_EDITOR
                    }catch (System.Exception ex) { lambda.content.environment.logger.Log_Error(ex.Message + "\n" + lambda.content.DumpStack() + ex); }
#endif
                };
            }
            else
            {
                dele = (T param0) => { };
            }

            if (this.sysType != typeof(Action<T>))
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
