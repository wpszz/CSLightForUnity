using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        ICLS_Expression CreateValueExpression(long val)
        {
            if (val > uint.MaxValue || val < int.MinValue)
            {
                CLS_Expression_Value<long> number = new CLS_Expression_Value<long>();
                number.value = val;
                return number;
            }
            else if (val > int.MaxValue)
            {
                CLS_Expression_Value<uint> number = new CLS_Expression_Value<uint>();
                number.value = (uint)val;
                return number;
            }
            else
            {
                CLS_Expression_Value<int> number = new CLS_Expression_Value<int>();
                number.value = (int)val;
                return number;
            }
        }

        public ICLS_Expression Compiler_Expression_Value(Token value, int pos)
        {
            if (value.type == TokenType.VALUE)
            {
                if (value.text.StartsWith("-0x") || value.text.StartsWith("-0X"))
                {
                    long lv = -Convert.ToInt64(value.text.Substring(1), 16);
                    return CreateValueExpression(lv);
                }
                else if (value.text.StartsWith("0x") || value.text.StartsWith("0X"))
                {
                    long lv = Convert.ToInt64(value.text, 16);
                    return CreateValueExpression(lv);
                }
                else if (value.text[value.text.Length - 1] == 'f')
                {
                    CLS_Expression_Value<float> number = new CLS_Expression_Value<float>();
                    number.value = float.Parse(value.text.Substring(0, value.text.Length - 1));
                    return number;
                }
                else if (value.text.Contains("."))
                {
                    CLS_Expression_Value<double> number = new CLS_Expression_Value<double>();
                    number.value = double.Parse(value.text);
                    return number;
                }
                else if (value.text.Contains("'"))
                {
                    CLS_Expression_Value<char> number = new CLS_Expression_Value<char>();
                    number.value = (char)value.text[1];
                    return number;
                }
                else if (value.text.StartsWith("-"))
                {
                    long lv = long.Parse(value.text);
                    return CreateValueExpression(lv);
                }
                else
                {
                    long lv = long.Parse(value.text);
                    return CreateValueExpression(lv);
                }
            }
            else if (value.type == TokenType.STRING)
            {
                CLS_Expression_Value<string> str = new CLS_Expression_Value<string>();
                str.value = value.text.Substring(1, value.text.Length - 2);
                return str;
            }
            else if (value.type == TokenType.IDENTIFIER)
            {
                CLS_Expression_GetValue getvalue = new CLS_Expression_GetValue(pos, pos, value.line, value.line);
                getvalue.value_name = value.text;
                return getvalue;
            }
            else if (value.type == TokenType.TYPE)
            {
                CLS_Expression_GetValue getvalue = new CLS_Expression_GetValue(pos, pos, value.line, value.line);
                int l = value.text.LastIndexOf('.');
                if (l >= 0)
                {
                    getvalue.value_name = value.text.Substring(l + 1);
                }
                else
                    getvalue.value_name = value.text;
                return getvalue;
            }
            logger.Log_Error("无法识别的简单表达式" + value);
            return null;
        }

        public ICLS_Expression Compiler_Expression_SubValue(Token value)
        {
            logger.Log_Error("已经整合到Compiler_Expression_Value处理" + value);
            return null;
        }

        public ICLS_Expression Compiler_Expression_NegativeValue(IList<Token> tlist, ICLS_Environment content, int pos, int posend)
        {
            ICLS_Expression subvalue;
            bool succ = Compiler_Expression(tlist, content, pos, posend, out subvalue);
            if (succ && subvalue != null)
            {
                CLS_Expression_NegativeValue v = new CLS_Expression_NegativeValue(pos, posend, tlist[pos].line, tlist[posend].line);
                v.listParam.Add(subvalue);
                return v;
            }
            else
            {
                LogError(tlist, "无法识别的负号表达式:", pos, posend);
                return null;
            }
        }

        public ICLS_Expression Compiler_Expression_NegativeBit(IList<Token> tlist, ICLS_Environment content, int pos, int posend)
        {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);
            if (expend2 != posend)
            {
                LogError(tlist, "无法识别的按位取反表达式:", expbegin, posend);
                return null;
            }
            else
            {
                ICLS_Expression subvalue;
                bool succ = Compiler_Expression(tlist, content, expbegin, expend2, out subvalue);
                if (succ && subvalue != null)
                {
                    CLS_Expression_NegativeBit v = new CLS_Expression_NegativeBit(pos, expend2, tlist[pos].line, tlist[expend2].line);
                    v.listParam.Add(subvalue);
                    return v;
                }
                else
                {
                    LogError(tlist, "无法识别的按位取反表达式:", expbegin, posend);
                    return null;
                }
            }
        }

        public ICLS_Expression Compiler_Expression_NegativeLogic(IList<Token> tlist, ICLS_Environment content, int pos, int posend)
        {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);

            if (expend2 > posend)
                expend2 = posend;

            ICLS_Expression subvalue;
            bool succ = Compiler_Expression(tlist, content, expbegin, expend2, out subvalue);
            if (succ && subvalue != null)
            {
                if (tlist[expbegin].text != "(" &&
                    (subvalue is CLS_Expression_Math2Value || subvalue is CLS_Expression_Math2ValueAndOr || subvalue is CLS_Expression_Math2ValueLogic))
                {
                    var pp = subvalue.listParam[0];
                    CLS_Expression_NegativeLogic v = new CLS_Expression_NegativeLogic(pp.tokenBegin, pp.tokenEnd, pp.lineBegin, pp.lineEnd);
                    v.listParam.Add(pp);
                    subvalue.listParam[0] = v;
                    return subvalue;
                }
                else
                {
                    CLS_Expression_NegativeLogic v = new CLS_Expression_NegativeLogic(pos, expend2, tlist[pos].line, tlist[expend2].line);
                    v.listParam.Add(subvalue);
                    return v;
                }
            }
            else
            {
                LogError(tlist, "无法识别的取反表达式:", expbegin, posend);
                return null;
            }
        }
    }
}