using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Long : RegHelper_Type
    {
        public CLS_Type_Long()
            : base(typeof(long), "long")
        {

        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            Type t = targetType;
            if (t == typeof(double))
                return (double)(long)(src);
            if (t == typeof(float))
                return (float)(long)(src);
            if (t == typeof(long))
                return src;
            if (t == typeof(ulong))
                return (ulong)(long)(src);
            if (t == typeof(int))
                return (int)(long)(src);
            if (t == typeof(uint))
                return (uint)(long)(src);
            if (t == typeof(short))
                return (short)(long)(src);
            if (t == typeof(ushort))
                return (ushort)(long)(src);
            if (t == typeof(sbyte))
                return (sbyte)(long)(src);
            if (t == typeof(byte))
                return (byte)(long)(src);
            if (t == typeof(char))
                return (char)(long)(src);
            if (t == typeof(int?))
                return (int?)(long)(src);
            if (t == typeof(uint?))
                return (uint?)(long)(src);
            return base.ConvertTo(env, src, targetType);
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            if (code == '+')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left + (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left + (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (long)left + (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (long)left + (float)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left + (uint)right.value;
                }
                else if (right.value is string)
                {
                    returntype = typeof(string);
                    return Convert.ToString(left) + Convert.ToString(right.value);
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left + Convert.ToInt64(right.value);
                }
            }
            else if (code == '-')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left - (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left - (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (long)left - (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (long)left - (float)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left - (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left - Convert.ToInt64(right.value);
                }
            }
            else if (code == '*')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left * (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left * (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (long)left * (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (long)left * (float)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left * (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left * Convert.ToInt64(right.value);
                }
            }
            else if (code == '/')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left / (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left / (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (long)left / (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (long)left / (float)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left / (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left / Convert.ToInt64(right.value);
                }
            }
            else if (code == '%')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left % (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left % (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (long)left % (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (long)left % (float)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left % (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left % Convert.ToInt64(right.value);
                }
            }
            else if (code == '<')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (long)left << (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left << Convert.ToInt32(right.value);
                }
            }
            else if (code == '>')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (long)left >> (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left >> Convert.ToInt32(right.value);
                }
            }
            else if (code == '&')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left & (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left & (int)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left & (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left & Convert.ToInt64(right.value);
                }
            }
            else if (code == '|')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left | (long)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left | Convert.ToInt64(right.value);
                }
            }
            else if (code == '~')
            {
                returntype = type;
                return ~(long)left;
            }
            else if (code == '^')
            {
                if (right.value is long)
                {
                    returntype = type;
                    return (long)left ^ (long)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (long)left ^ (int)right.value;
                }
                else if (right.value is uint)
                {
                    returntype = type;
                    return (long)left ^ (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (long)left ^ Convert.ToInt64(right.value);
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
                    return (long)left == (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left == (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left == (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left == (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left == (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left == Convert.ToInt64(right.value);
                }
            }
            else if (code == logictoken.not_equal)
            {
                if (right.value is int)
                {
                    return (long)left != (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left != (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left != (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left != (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left != (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left != Convert.ToInt64(right.value);
                }
            }
            else if (code == logictoken.less)
            {
                if (right.value is int)
                {
                    return (long)left < (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left < (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left < (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left < (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left < (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left < Convert.ToInt64(right.value);
                }
            }
            else if (code == logictoken.less_equal)
            {
                if (right.value is int)
                {
                    return (long)left <= (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left <= (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left <= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left <= (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left <= (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left <= Convert.ToInt64(right.value);
                }
            }
            else if (code == logictoken.more)
            {
                if (right.value is int)
                {
                    return (long)left > (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left > (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left > (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left > (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left > (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left > Convert.ToInt64(right.value);
                }
            }
            else if (code == logictoken.more_equal)
            {
                if (right.value is int)
                {
                    return (long)left >= (int)right.value;
                }
                else if (right.value is long)
                {
                    return (long)left >= (long)right.value;
                }
                else if (right.value is double)
                {
                    return (long)left >= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (long)left >= (float)right.value;
                }
                else if (right.value is uint)
                {
                    return (long)left >= (uint)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (long)left >= Convert.ToInt64(right.value);
                }
            }

            return base.MathLogic(env, code, left, right);
        }

        public override object DefValue
        {
            get { return (long)0; }
        }
    }
}
