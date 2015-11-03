using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_UInt : RegHelper_Type
    {
        public CLS_Type_UInt()
            : base(typeof(uint), "uint")
        {

        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            Type t = targetType;
            if (t == typeof(double))
                return (double)(uint)(src);
            if (t == typeof(float))
                return (float)(uint)(src);
            if (t == typeof(long))
                return (long)(uint)(src);
            if (t == typeof(ulong))
                return (ulong)(uint)(src);
            if (t == typeof(int))
                return (int)(uint)(src);
            if (t == typeof(uint))
                return src;
            if (t == typeof(short))
                return (short)(uint)(src);
            if (t == typeof(ushort))
                return (ushort)(uint)(src);
            if (t == typeof(sbyte))
                return (sbyte)(uint)(src);
            if (t == typeof(byte))
                return (byte)(uint)(src);
            if (t == typeof(char))
                return (char)(uint)(src);
            if (t == typeof(int?))
                return (int?)(uint)(src);
            if (t == typeof(uint?))
                return (uint?)(uint)(src);
            return base.ConvertTo(env, src, targetType);
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            if (code == '+')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left + (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left + (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (uint)left + (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (uint)left + (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left + (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left + (ulong)right.value;
                }
                else if (right.value is string)
                {
                    returntype = typeof(string);
                    return Convert.ToString(left) + Convert.ToString(right.value);
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left + Convert.ToUInt32(right.value);
                }
            }
            else if (code == '-')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left - (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left - (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (uint)left - (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (uint)left - (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left - (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left - (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left - Convert.ToUInt32(right.value);
                }
            }
            else if (code == '*')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left * (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left * (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (uint)left * (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (uint)left * (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left * (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left * (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left * Convert.ToUInt32(right.value);
                }
            }
            else if (code == '/')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left / (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left / (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (uint)left / (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (uint)left / (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left / (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left / (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left / Convert.ToUInt32(right.value);
                }
            }
            else if (code == '%')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left % (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left % (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = typeof(double);
                    return (uint)left % (double)right.value;
                }
                else if (right.value is float)
                {
                    returntype = typeof(float);
                    return (uint)left % (float)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left % (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left % (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left % Convert.ToUInt32(right.value);
                }
            }
            else if (code == '<')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (uint)left << (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left << Convert.ToInt32(right.value);
                }
            }
            else if (code == '>')
            {
                if (right.value is int)
                {
                    returntype = type;
                    return (uint)left >> (int)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left >> Convert.ToInt32(right.value);
                }
            }
            else if (code == '&')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left & (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left & (int)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left & (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left & (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left & Convert.ToUInt32(right.value);
                }
            }
            else if (code == '|')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left | (uint)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left | (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left | (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left | Convert.ToUInt32(right.value);
                }
            }
            else if (code == '~')
            {
                returntype = type;
                return ~(uint)left;
            }
            else if (code == '^')
            {
                if (right.value is uint)
                {
                    returntype = type;
                    return (uint)left ^ (uint)right.value;
                }
                else if (right.value is int)
                {
                    returntype = typeof(long);
                    return (uint)left ^ (int)right.value;
                }
                else if (right.value is long)
                {
                    returntype = typeof(long);
                    return (uint)left ^ (long)right.value;
                }
                else if (right.value is ulong)
                {
                    returntype = typeof(ulong);
                    return (uint)left ^ (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (uint)left ^ Convert.ToUInt32(right.value);
                }
            }
            return base.Math2Value(env, code, left, right, out returntype);
        }

        public override bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                if (right.value is uint)
                {
                    return (uint)left == (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left == (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left == (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left == (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left == (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left == (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left == Convert.ToUInt32(right.value);
                }
            }
            else if (code == logictoken.not_equal)
            {
                if (right.value is uint)
                {
                    return (uint)left != (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left != (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left != (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left != (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left != (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left != (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left != Convert.ToUInt32(right.value);
                }
            }
            else if (code == logictoken.less)
            {
                if (right.value is uint)
                {
                    return (uint)left < (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left < (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left < (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left < (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left < (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left < (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left < Convert.ToUInt32(right.value);
                }
            }
            else if (code == logictoken.less_equal)
            {
                if (right.value is uint)
                {
                    return (uint)left <= (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left <= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left <= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left <= (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left <= (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left <= (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left <= Convert.ToUInt32(right.value);
                }
            }
            else if (code == logictoken.more)
            {
                if (right.value is uint)
                {
                    return (uint)left > (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left > (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left > (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left > (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left > (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left > (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left > Convert.ToUInt32(right.value);
                }
            }
            else if (code == logictoken.more_equal)
            {
                if (right.value is uint)
                {
                    return (uint)left >= (uint)right.value;
                }
                else if (right.value is int)
                {
                    return (uint)left >= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (uint)left >= (double)right.value;
                }
                else if (right.value is float)
                {
                    return (uint)left >= (float)right.value;
                }
                else if (right.value is long)
                {
                    return (uint)left >= (long)right.value;
                }
                else if (right.value is ulong)
                {
                    return (uint)left >= (ulong)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (uint)left >= Convert.ToUInt32(right.value);
                }
            }

            return base.MathLogic(env, code, left, right);
        }

        public override object DefValue
        {
            get { return (uint)0; }
        }
    }
}
