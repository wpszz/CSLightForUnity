using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        public ICLS_Expression Compiler_Expression_Math(IList<Token> tlist, ICLS_Environment environment, int pos, int posend)
        {
            IList<int> sps = SplitExpressionWithOp(tlist, pos, posend);
            if (sps == null)
                LogError(tlist, "SplitExpressionWithOp return null", pos, posend);
            int oppos = GetLowestMathOp(tlist, sps);
            if (oppos < 0)
            {
                if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(")//函数表达式
                {
                    CLS_Expression_Function func = (CLS_Expression_Function)Compiler_Expression_Function(tlist, environment, pos, posend);
                    if (func != null)
                    {
                        if (environment.GetFunction(func.funcname) != null)
                        {
                            CLS_Expression_GlobalFunction globalFunc = new CLS_Expression_GlobalFunction(func.listParam, func.tokenBegin, func.tokenEnd, func.lineBegin, func.lineEnd);
                            globalFunc.funcname = func.funcname;
                            return globalFunc;
                        }
                    }
                    return func;
                }
                return null;
            }
            Token tkCur = tlist[oppos];
            if (tkCur.text == "=>")
            {
                return Compiler_Expression_Lambda(tlist, environment, pos, posend);
            }
            else if (tkCur.text == "." && pos == oppos - 1 && tlist[pos].type == TokenType.TYPE)
            {
                int right = oppos + 1;
                int rightend = posend;

                ICLS_Expression valueright;
                bool succ2 = Compiler_Expression(tlist, environment, right, rightend, out valueright);
                if (succ2)
                {
                    CLS_Expression_GetValue vg = valueright as CLS_Expression_GetValue;
                    CLS_Expression_Function vf = valueright as CLS_Expression_Function;
                    if (vg != null)
                    {
                        // 优化枚举常量表达式
                        try
                        {
                            System.Type sysType = environment.GetTypeByKeyword(tlist[pos].text).type;
                            if (sysType != null && sysType.IsEnum)
                            {
                                CLS_Expression_Enum enumVal = new CLS_Expression_Enum(sysType);
                                enumVal.tokenBegin = pos;
                                enumVal.tokenEnd = rightend;
                                enumVal.lineBegin = tlist[pos].line;
                                enumVal.lineEnd = tlist[rightend].line;
                                enumVal.value = Enum.Parse(sysType, vg.value_name);
                                return enumVal;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Log_Warn("Enum expression: " + ex.Message);
                        }
                        CLS_Expression_StaticFind value = new CLS_Expression_StaticFind(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.staticmembername = vg.value_name;
                        value.type = environment.GetTypeByKeyword(tlist[pos].text);
                        return value;
                    }
                    else if (vf != null)
                    {
                        CLS_Expression_StaticFunction value = new CLS_Expression_StaticFunction(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.functionName = vf.funcname;
                        value.type = environment.GetTypeByKeyword(tlist[pos].text);
                        value.listParam.AddRange(vf.listParam.ToArray());
                        return value;
                    }
                    else if (valueright is CLS_Expression_SelfOp)
                    {
                        CLS_Expression_SelfOp vr = valueright as CLS_Expression_SelfOp;
                        CLS_Expression_StaticMath value = new CLS_Expression_StaticMath(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.type = environment.GetTypeByKeyword(tlist[pos].text);
                        value.staticmembername = vr.value_name;
                        value.mathop = vr.mathop;
                        return value;
                    }
                    else
                    {
                        throw new Exception("不可识别的表达式:" + tkCur.ToString());
                    }
                }
                else
                {
                    throw new Exception("不可识别的表达式:" + tkCur.ToString());
                }
            }
            else
            {
                int left = pos;
                int leftend = oppos - 1;
                int right = oppos + 1;
                int rightend = posend;
                if (tkCur.text == "(")
                {
                    int offset = 0;
                    // 如果是(Type[])
                    if (tlist[oppos + 2].text == "[" && tlist[oppos + 3].text == "]")
                        offset = 5;
                    // 普通强转类型(Type) (Type.Type) (Type.Type.Type)
                    else if (tlist[oppos + 2].text == ")")
                        offset = 3;
                    else
                    {
                        LogError(tlist, "暂不支持该语法，可以删除多余的括号", pos, posend);
                        return null;
                    }
                    if (offset > 0)
                    {
                        ICLS_Expression v;
                        if (!Compiler_Expression(tlist, environment, oppos + offset, posend, out v))
                        {
                            LogError(tlist, "编译表达式失败", right, rightend);
                            return null;
                        }
                        CLS_Expression_TypeConvert convert = new CLS_Expression_TypeConvert(pos, posend, tlist[pos].line, tlist[posend].line);
                        convert.listParam.Add(v);
                        convert.targettype = environment.GetTypeByKeyword(tlist[oppos + 1].text).type;
                        return convert;
                    }
                }
                ICLS_Expression valueleft;
                bool succ1 = Compiler_Expression(tlist, environment, left, leftend, out valueleft);
                ICLS_Expression valueright;
                if (tkCur.text == "[")
                {
                    Token tkEnd = tlist[rightend];

                    if (tkEnd.text == "++" || tkEnd.text == "--")
                    {
                        rightend -= 2;
                    }
                    else
                    {
                        rightend--;
                    }

                    if (!Compiler_Expression(tlist, environment, right, rightend, out valueright))
                    {
                        LogError(tlist, "编译表达式失败", right, rightend);
                        return null;
                    }

                    CLS_Expression_IndexFind value = new CLS_Expression_IndexFind(left, rightend, tlist[left].line, tlist[rightend].line);
                    value.listParam.Add(valueleft);
                    value.listParam.Add(valueright);

                    if (tkEnd.text == "++" || tkEnd.text == "--")
                    {
                        CLS_Expression_SelfOpWithValue selfOpExp = new CLS_Expression_SelfOpWithValue(left, posend, tlist[left].line, tlist[posend].line);
                        selfOpExp.listParam.Add(value);
                        selfOpExp.listParam.Add(new CLS_Expression_Value<int>() { value = 1 });
                        selfOpExp.mathop = tkEnd.text[0];
                        return selfOpExp;
                    }
                    return value;
                }
                else if (tkCur.text == "as")
                {
                    CLS_Expression_TypeConvert convert = new CLS_Expression_TypeConvert(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    convert.listParam.Add(valueleft);
                    convert.targettype = environment.GetTypeByKeyword(tlist[oppos + 1].text).type;
                    return convert;
                }
                else if (tkCur.text == "is")
                {
                    CLS_Expression_TypeCheck check = new CLS_Expression_TypeCheck(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    check.listParam.Add(valueleft);
                    check.targettype = environment.GetTypeByKeyword(tlist[oppos + 1].text).type;
                    return check;
                }
                bool succ2 = Compiler_Expression(tlist, environment, right, rightend, out valueright);
                if (succ1 && succ2 && valueright != null && valueleft != null)
                {
                    if (tkCur.text == "=")
                    {
                        //member set
                        CLS_Expression_MemberFind mfinde = valueleft as CLS_Expression_MemberFind;
                        CLS_Expression_StaticFind sfinde = valueleft as CLS_Expression_StaticFind;
                        CLS_Expression_IndexFind ifinde = valueleft as CLS_Expression_IndexFind;
                        if (mfinde != null)
                        {
                            CLS_Expression_MemberSetValue value = new CLS_Expression_MemberSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.membername = mfinde.membername;
                            value.listParam.Add(mfinde.listParam[0]);
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else if (sfinde != null)
                        {
                            CLS_Expression_StaticSetValue value = new CLS_Expression_StaticSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.staticmembername = sfinde.staticmembername;
                            value.type = sfinde.type;
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else if (ifinde != null)
                        {
                            CLS_Expression_IndexSetValue value = new CLS_Expression_IndexSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(ifinde.listParam[0]);
                            value.listParam.Add(ifinde.listParam[1]);
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else
                        {
                            throw new Exception("非法的Member Set表达式: " + valueleft + " file: " + m_curFileName);
                        }
                    }
                    else if (tkCur.text == ".")
                    {
                        //FindMember
                        CLS_Expression_GetValue vg = valueright as CLS_Expression_GetValue;
                        CLS_Expression_Function vf = valueright as CLS_Expression_Function;
                        if (vg != null)
                        {
                            CLS_Expression_MemberFind value = new CLS_Expression_MemberFind(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.membername = vg.value_name;
                            return value;
                        }
                        else if (vf != null)
                        {
                            CLS_Expression_MemberFunction value = new CLS_Expression_MemberFunction(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.functionName = vf.funcname;
                            value.listParam.Add(valueleft);
                            value.listParam.AddRange(vf.listParam.ToArray());
                            return value;
                        }
                        else if (valueright is CLS_Expression_SelfOp)
                        {
                            CLS_Expression_SelfOp vr = valueright as CLS_Expression_SelfOp;
                            CLS_Expression_MemberMath value = new CLS_Expression_MemberMath(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.membername = vr.value_name;
                            value.mathop = vr.mathop;
                            return value;
                        }
                        throw new Exception("不可识别的表达式" + valueleft + "." + valueright);
                    }
                    else if (tkCur.text == "+=" || tkCur.text == "-=" || tkCur.text == "*=" || tkCur.text == "/=" || tkCur.text == "%=")
                    {
                        CLS_Expression_SelfOpWithValue value = new CLS_Expression_SelfOpWithValue(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        value.mathop = tkCur.text[0];
                        return value;
                    }
                    else if (tkCur.text == ">>=" || tkCur.text == "<<=" || tkCur.text == "&=" || tkCur.text == "|=" || tkCur.text == "^=")
                    {
                        CLS_Expression_SelfOpWithValue value = new CLS_Expression_SelfOpWithValue(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        value.mathop = tkCur.text[0];
                        return value;
                    }
                    else if (tkCur.text == "&&" || tkCur.text == "||")
                    {
                        CLS_Expression_Math2ValueAndOr value = new CLS_Expression_Math2ValueAndOr(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        value.mathop = tkCur.text[0];
                        return value;
                    }
                    else if (tkCur.text == ">" || tkCur.text == ">=" || tkCur.text == "<" || tkCur.text == "<=" || tkCur.text == "==" || tkCur.text == "!=")
                    {
                        CLS_Expression_Math2ValueLogic value = new CLS_Expression_Math2ValueLogic(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        logictoken token = logictoken.not_equal;
                        if (tkCur.text == ">")
                        {
                            token = logictoken.more;
                        }
                        else if (tkCur.text == ">=")
                        {
                            token = logictoken.more_equal;
                        }
                        else if (tkCur.text == "<")
                        {
                            token = logictoken.less;
                        }
                        else if (tkCur.text == "<=")
                        {
                            token = logictoken.less_equal;
                        }
                        else if (tkCur.text == "==")
                        {
                            token = logictoken.equal;
                        }
                        else if (tkCur.text == "!=")
                        {
                            token = logictoken.not_equal;
                        }
                        value.mathop = token;
                        return value;
                    }
                    else
                    {
                        char mathop = tkCur.text[0];
                        if (mathop == '?')
                        {
                            CLS_Expression_Math3Value value = new CLS_Expression_Math3Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);

                            CLS_Expression_Math2Value vvright = valueright as CLS_Expression_Math2Value;
                            if (vvright.mathop != ':')
                                throw new Exception("三元表达式异常" + tkCur.ToString());
                            value.listParam.Add(vvright.listParam[0]);
                            value.listParam.Add(vvright.listParam[1]);
                            return value;
                        }
                        else
                        {
                            CLS_Expression_Math2Value value = new CLS_Expression_Math2Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.listParam.Add(valueright);
                            value.mathop = mathop;
                            return value;
                        }
                    }
                }
                else
                {
                    LogError(tlist, "编译表达式失败", right, rightend);
                }
            }
            return null;
        }

        public ICLS_Expression Compiler_Expression_MathSelf(IList<Token> tlist, int pos, int posend)
        {
            CLS_Expression_SelfOp value = new CLS_Expression_SelfOp(pos, posend, tlist[pos].line, tlist[posend].line);
            value.value_name = tlist[pos].text;
            value.mathop = tlist[pos + 1].text[0];
            return value;
        }
    }
}
