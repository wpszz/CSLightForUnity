﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        public void LogError(IList<Token> tlist, string text, int pos,int posend)
        {
            string str = "";
            for (int i = pos; i <= posend;i++ )
            {
                str += tlist[i].text;
            }
            logger.Log_Error(text + ": " + str + " " + m_curFileName + "(" + pos + "-" + posend + ")");
        }

        // 可以搞出Block
        public bool Compiler_Expression_Block(IList<Token> tlist, ICLS_Environment content, int pos, int posend, out ICLS_Expression value)
        {
            int begin = pos;
            value = null;
            List<ICLS_Expression> values = new List<ICLS_Expression>();
            int end = 0;
            do
            {
                if (tlist[begin].type == TokenType.COMMENT)
                {
                    begin++;
                    continue;
                }
                if (tlist[begin].type == TokenType.PUNCTUATION && tlist[begin].text == ";")
                {
                    begin++;
                    continue;
                }
                int bdep;
                //脱一次壳
                end = FindCodeInBlock(tlist, ref begin, out bdep);

                if (end > posend)
                {
                    end = posend;
                }
                int expend = end;
                int expbegin = begin;
                if (expbegin > expend) return true;
                if (bdep == 2) //编译块表达式
                {
                    expbegin++;
                    expend--;
                    ICLS_Expression subvalue;
                    bool bsucc = Compiler_Expression_Block(tlist,content, expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    ICLS_Expression subvalue;
                    bool bsucc = Compiler_Expression(tlist, content,expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        return false;
                    }
                }

                begin = end + 1;
            }
            while (begin <= posend);

            if (values.Count == 1)
            {
                value = values[0];
            }
            else if (values.Count > 1)
            {
                value = new CLS_Expression_Block(values, pos, end, tlist[pos].line, tlist[end].line);
            }
            return true;
        }

        // 不出Block,必须一次解析完,括号为优先级
        public bool Compiler_Expression(IList<Token> tlist, ICLS_Environment environment, int pos, int posend, out ICLS_Expression value)
        {
            if (pos > posend)
            {
                value = null;
                return false;
            }
            int begin = pos;
            value = null;
            List<ICLS_Expression> values = new List<ICLS_Expression>();
            do
            {
                if (tlist[begin].type == TokenType.COMMENT)
                {
                    begin++;
                    continue;
                }
                if (tlist[begin].type == TokenType.PUNCTUATION && tlist[begin].text == ";")
                {
                    begin++;
                    continue;
                }
                int bdep;
                //脱一次壳
                int end = FindCodeAny(tlist, ref begin, out bdep);

                if (end > posend)
                {
                    end = posend;
                }

                else if (end < posend)
                {
                    bool bMath = false;
                    for (int i = end + 1; i <= posend; i++)
                    {
                        if (tlist[i].type == TokenType.COMMENT) continue;
                        if (tlist[i].type == TokenType.PUNCTUATION && tlist[i].text == ";") continue;
                        bMath = true;
                        break;
                    }
                    if (bMath)
                    {
                        end = posend;
                        //如果表达式一次搞不完，那肯定是优先级问题
                        value = Compiler_Expression_Math(tlist,environment, begin, posend);
                        return true;
                    }
                }

                int expend = end;
                int expbegin = begin;
                if (expbegin > expend) return true;
                if (expend == expbegin)
                {
                    if (tlist[expbegin].type == TokenType.KEYWORD)
                    {
                        if (tlist[expbegin].text == "return")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_Return(tlist, environment, expbegin, expend);
                            if (null == subvalue) 
                                return false;
                            else
                                values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "break")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_Break(tlist, expbegin);
                            if (null == subvalue)
                                return false;
                            else
                                values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "continue")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_Continue(tlist, expbegin);
                            if (null == subvalue)
                                return false;
                            else
                                values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "true")
                        {
                            values.Add(new CLS_True());
                        }
                        else if (tlist[expbegin].text == "false")
                        {
                            values.Add(new CLS_False());
                        }
                        else if (tlist[expbegin].text == "null")
                        {
                            values.Add(new CLS_Null());
                        }
                    }
                    else
                    {
                        ICLS_Expression subvalue = Compiler_Expression_Value(tlist[expbegin],expbegin);
                        if (null == subvalue) 
                            return false;
                        else
                            values.Add(subvalue);
                    }
                }
                else if (bdep == 1) //深层表达式
                {
                    expbegin++;
                    expend--;
                    ICLS_Expression subvalue;
                    bool bsucc = Compiler_Expression(tlist, environment, expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        return false;
                    }
                }
                else             //尝试各种表达式
                {
                    bool bTest = false;
                    // 取反表达式
                    if (tlist[expbegin].type == TokenType.PUNCTUATION && tlist[expbegin].text == "-")
                    {
                        if (tlist[expend].type == TokenType.VALUE)
                        {
                            // 负数
                            if (expend == expbegin + 1)
                            {
                                ICLS_Expression subvalue = Compiler_Expression_SubValue(tlist[expend]);
                                if (null == subvalue) 
                                    return false;
                                else
                                    values.Add(subvalue);
                            }
                            else
                            {
                                ICLS_Expression subvalue = Compiler_Expression_Math(tlist, environment, begin, posend);
                                if (null == subvalue) 
                                    return false;
                                else
                                    values.Add(subvalue);
                            }
                        }
                        else
                        {
                            // 负数表达式
                            ICLS_Expression subvalue = Compiler_Expression_NegativeValue(tlist, environment, expbegin + 1, expend);
                            if (null == subvalue) 
                                return false;
                            else
                                values.Add(subvalue);
                        }
                        bTest = true;
                    }
                    // 按位取反表达式
                    else if (tlist[expbegin].type == TokenType.PUNCTUATION && tlist[expbegin].text == "~")
                    {
                        ICLS_Expression subvalue = Compiler_Expression_NegativeBit(tlist, environment, expbegin + 1, expend);
                        if (null == subvalue)
                            return false;
                        else
                            values.Add(subvalue);
                        bTest = true;
                    }
                    // 逻辑反表达式
                    else if (tlist[expbegin].type == TokenType.PUNCTUATION && tlist[expbegin].text == "!")
                    {
                        ICLS_Expression subvalue = Compiler_Expression_NegativeLogic(tlist, environment, expbegin + 1, expend);
                        if (null == subvalue) 
                            return false;
                        else
                            values.Add(subvalue);
                        bTest = true;
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.TYPE)
                    {
                        //定义表达式或者定义并赋值表达式
                        if (tlist[expbegin + 1].type == TokenType.IDENTIFIER)
                        {
                            if (expend == expbegin + 1)
                            {
                                //定义表达式
                                ICLS_Expression subvalue = Compiler_Expression_Define(tlist, environment, expbegin, expend);
                                if (null == subvalue) 
                                    return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else if (expend > expbegin + 2 && tlist[expbegin + 2].type == TokenType.PUNCTUATION && tlist[expbegin + 2].text == "=")
                            {
                                //定义并赋值表达式
                                ICLS_Expression subvalue = Compiler_Expression_DefineAndSet(tlist, environment, expbegin, expend);
                                if (null == subvalue) 
                                    return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else
                            {
                                LogError(tlist,"无法识别的表达式:", expbegin ,expend);
                                return false;
                            }
                        }
                        else if (tlist[expbegin + 1].text == "[" && tlist[expbegin + 2].text == "]" && tlist[expbegin + 3].type == TokenType.IDENTIFIER)//定义表达式或者定义并赋值表达式
                        {
                            if (expend == expbegin + 3)
                            {
                                //定义表达式
                                ICLS_Expression subvalue = Compiler_Expression_DefineArray(tlist, environment, expbegin, expend);
                                if (null == subvalue) return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else if (expend > expbegin + 4 && tlist[expbegin + 4].type == TokenType.PUNCTUATION && tlist[expbegin + 4].text == "=")
                            {
                                //定义并赋值表达式
                                ICLS_Expression subvalue = Compiler_Expression_DefineAndSetArray(tlist, environment, expbegin, expend);
                                if (null == subvalue) return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else
                            {
                                LogError(tlist, "无法识别的表达式:", expbegin, expend);
                                return false;
                            }
                        }
                        else if (tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == ".")
                        {
                            //静态调用表达式
                            ICLS_Expression subvalue = Compiler_Expression_Math(tlist, environment, expbegin, expend);
                            if (subvalue != null)
                            {
                                values.Add(subvalue);
                                bTest = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.IDENTIFIER)
                    {
                        if (expend == expbegin + 1)//一元表达式
                        {
                            ICLS_Expression subvalue = Compiler_Expression_MathSelf(tlist, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        if (!bTest && tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == "=")//赋值表达式
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Set(tlist, environment,expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                    }
                    if (!bTest && (tlist[expbegin].type == TokenType.IDENTIFIER || tlist[expbegin].type == TokenType.VALUE || tlist[expbegin].type == TokenType.STRING))
                    {
                        //算数表达式
                        ICLS_Expression subvalue = Compiler_Expression_Math(tlist, environment, expbegin, expend);
                        if (null != subvalue)
                        {
                            values.Add(subvalue);
                            bTest = true;
                        }
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.KEYWORD)
                    {
                        if (tlist[expbegin].text == "for")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_For(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "foreach")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_ForEach(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "while")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_While(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "do")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_Dowhile(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "if")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_If(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "return")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_Loop_Return(tlist, environment,expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "throw")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_FunctionThrow(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if(tlist[expbegin].text=="true"||tlist[expbegin].text=="false"||tlist[expbegin].text=="null")
                        {
                            //算数表达式
                            ICLS_Expression subvalue = Compiler_Expression_Math(tlist, environment, expbegin, expend);
                            if (null != subvalue)
                            {
                                values.Add(subvalue);
                                bTest = true;
                            }
                        }
                        else if(tlist[expbegin].text=="new")
                        {
                            //new 表达式
                            if (tlist[expbegin + 1].type == TokenType.TYPE)
                            {
                                ICLS_Expression subvalue = Compiler_Expression_FunctionNew(tlist, environment, pos, posend);
                                values.Add(subvalue);
                               bTest = true;
                            }
                        }
                        else if (tlist[expbegin].text == "yield")
                        {
                            ICLS_Expression subvalue = Compiler_Expression_FunctionYield(tlist, environment, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "try")
                        {
                            LogError(tlist, "try 表达式不支持也没必要", expbegin, expend);
                            return false;
                        }
                        else
                        {
                            LogError(tlist, "无法识别的表达式:", expbegin, expend);
                            return false;
                        }
                    }
                    if (!bTest)
                    {
                        LogError(tlist, "无法识别的表达式:", expbegin, expend);
                        return false;
                    }
                }
                begin = end + 1;
            }
            while (begin <= posend);
            if (values.Count == 1)
            {
                value = values[0];
            }
            else if (values.Count > 1)
            {
                LogError(tlist, "异常表达式", pos, posend);
            }
            return true;
        }
    }
}