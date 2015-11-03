using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Int : RegHelper_Type
    {
        public CLS_Type_Int()
            : base(typeof(int), "int")
        {

        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            Type t = targetType;
            if (t == typeof(double))
                return (double)(int)(src);
            if (t == typeof(float))
                return (float)(int)(src);
            if (t == typeof(long))
                return (long)(int)(src);
            if (t == typeof(ulong))
                return (ulong)(int)(src);
            if (t == typeof(int))
                return src;
            if (t == typeof(uint))
                return (uint)(int)(src);
            if (t == typeof(short))
                return (short)(int)(src);
            if (t == typeof(ushort))
                return (ushort)(int)(src);
            if (t == typeof(sbyte))
                return (sbyte)(int)(src);
            if (t == typeof(byte))
                return (byte)(int)(src);
            if (t == typeof(char))
                return (char)(int)(src);
            if (t == typeof(int?))
                return (int?)(int)(src);
            if (t == typeof(uint?))
                return (uint?)(int)(src);
            return base.ConvertTo(env, src, targetType);
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            if (code == '+')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left + (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (int)left + (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (int)left + (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left + (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left + (uint)right.value;
                }
                else if (right.value is string)
                {
                    returntype = typeof(string);
                    return Convert.ToString(left) + Convert.ToString(right.value);
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left + Convert.ToInt32(right.value);
                }
            }
            else if (code == '-')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left - (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (int)left - (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (int)left - (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left - (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left - (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left - Convert.ToInt32(right.value);
                }
            }
            else if (code == '*')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left * (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (int)left * (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (int)left * (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left * (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left * (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left * Convert.ToInt32(right.value);
                }
            }
            else if (code == '/')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left / (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (int)left / (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (int)left / (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left / (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left / (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left / Convert.ToInt32(right.value);
                }
            }
            else if (code == '%')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left % (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (int)left % (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (int)left % (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left % (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left % (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left % Convert.ToInt32(right.value);
                }
            }
            else if (code == '<')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left << (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left << Convert.ToInt32(right.value);
                }
            }
            else if (code == '>')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left >> (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left >> Convert.ToInt32(right.value);
                }
            }
            else if (code == '&')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left & (int)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left & (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left & (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left & Convert.ToInt32(right.value);
                }
            }
            else if (code == '|')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left | (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left | Convert.ToInt32(right.value);
                }
            }
            else if (code == '~')
            {
                returntype = type;
                return ~(int)left;
            }
            else if (code == '^')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (int)left ^ (int)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (int)left ^ (long)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = typeof(long);
                    return (int)left ^ (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (int)left ^ Convert.ToInt32(right.value);
                }
            }
            return base.Math2Value(env, code, left, right, out returntype);
        }

        public override bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                if (right.value is int)
                {
                    return (int)left == (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left == (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left == (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left == (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left == (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left == Convert.ToInt32(right.value);
                }
            }
            else if (code == logictoken.not_equal)
            {
                if (right.value is int)
                {
                    return (int)left != (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left != (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left != (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left != (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left != (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left != Convert.ToInt32(right.value);
                }
            }
            else if (code == logictoken.less)
            {
                if (right.value is int)
                {
                    return (int)left < (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left < (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left < (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left < (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left < (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left < Convert.ToInt32(right.value);
                }
            }
            else if (code == logictoken.less_equal)
            {
                if (right.value is int)
                {
                    return (int)left <= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left <= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left <= (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left <= (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left <= (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left <= Convert.ToInt32(right.value);
                }
            }
            else if (code == logictoken.more)
            {
                if (right.value is int)
                {
                    return (int)left > (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left > (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left > (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left > (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left > (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left > Convert.ToInt32(right.value);
                }
            }
            else if (code == logictoken.more_equal)
            {
                if (right.value is int)
                {
                    return (int)left >= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (int)left >= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (int)left >= (float)right.value;
                }
                else if (right.value is long)
                {
                    return (int)left >= (long)right.value;
                }
                else if (right.value is uint)
                {
                    return (int)left >= (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (int)left >= Convert.ToInt32(right.value);
                }
            }

            return base.MathLogic(env, code, left, right);
        }

        public override object DefValue
        {
            get { return (int)0; }
        }
    }
}
