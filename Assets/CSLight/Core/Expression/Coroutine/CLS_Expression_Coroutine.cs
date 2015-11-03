using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Expression_Coroutine : ICLS_Expression
    {
        public CLS_Expression_Coroutine(List<ICLS_Expression> _params, int tbegin, int tend, int lbegin, int lend)
        {
            listParam = _params;
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
            return new CLS_Content.Value() { type = typeof(IEnumerator), value = CustomCoroutine(content.Clone()) };
        }

        IEnumerator CustomCoroutine(CLS_Content content)
        {
            content.InStack(this);
            content.DepthAdd();
            CLS_Content.Value retVal = null;
            ICLS_Expression exp = null;
            for (int i = 0, count = listParam.Count; i < count; i++)
            {
                exp = listParam[i];
                CLS_Expression_LoopFor expLoopFor = exp as CLS_Expression_LoopFor;
                if (expLoopFor != null)
                {
                    content.InStack(expLoopFor);
                    content.DepthAdd();

                    ICLS_Expression expr_init = expLoopFor.listParam[0];
                    ICLS_Expression expr_continue = expLoopFor.listParam[1];
                    ICLS_Expression expr_step = expLoopFor.listParam[2];
                    ICLS_Expression expr_block = expLoopFor.listParam[3];

#if UNITY_EDITOR
                    try
                    {
#endif
                    if (expr_init != null)
                        expr_init.ComputeValue(content);
#if UNITY_EDITOR
                    }
                    catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                    for (;;)
                    {
#if UNITY_EDITOR
                        try
                        {
#endif
                        if (expr_continue != null && !(bool)expr_continue.ComputeValue(content).value)
                            break;
#if UNITY_EDITOR
                        }
                        catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                        if (expr_block != null)
                        {
                            if (expr_block is CLS_Expression_Block)
                            {
                                content.InStack(expr_block);
                                content.DepthAdd();
                                for (int j = 0, count2 = expr_block.listParam.Count; j < count2; j++)
                                {
#if UNITY_EDITOR
                                    try
                                    {
#endif
                                    retVal = expr_block.listParam[j].ComputeValue(content);
#if UNITY_EDITOR
                                    }
                                    catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                                    if (retVal != null)
                                    {
                                        if (retVal.breakBlock == 12)
                                        {
                                            CLS_Content.PoolContent(content);
                                            yield break;
                                        }
                                        else if (retVal.breakBlock == 13)
                                            yield return retVal.value;
                                        else if (retVal.breakBlock > 1)
                                            break;
                                    }
                                }
                                content.DepthRemove();
                                content.OutStack(expr_block);
                            }
                            else
                            {
#if UNITY_EDITOR
                                try
                                {
#endif
                                retVal = expr_block.ComputeValue(content);
#if UNITY_EDITOR
                                }
                                catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                                if (retVal != null)
                                {
                                    if (retVal.breakBlock == 12)
                                    {
                                        CLS_Content.PoolContent(content);
                                        yield break;
                                    }
                                    else if (retVal.breakBlock == 13)
                                        yield return retVal.value;
                                    else if (retVal.breakBlock > 1)
                                        break;
                                }
                            }
                        }
#if UNITY_EDITOR
                        try
                        {
#endif
                        if (expr_step != null)
                            expr_step.ComputeValue(content);
#if UNITY_EDITOR
                        }
                        catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                    }

                    content.DepthRemove();
                    content.OutStack(expLoopFor);
                }
                else
                {
#if UNITY_EDITOR
                    try
                    {
#endif
                    retVal = exp.ComputeValue(content);
#if UNITY_EDITOR
                    }
                    catch (System.Exception ex) { content.environment.logger.Log_Error(ex.Message + "\n" + content.DumpStack() + ex); }
#endif
                    if (retVal != null)
                    {
                        if (retVal.breakBlock == 12)
                        {
                            CLS_Content.PoolContent(content);
                            yield break;
                        }
                        else if (retVal.breakBlock == 13)
                            yield return retVal.value;
                    }
                }
            }
            CLS_Content.PoolContent(content);
        }

        public override string ToString()
        {
            return "Coroutine|";
        }
    }
}