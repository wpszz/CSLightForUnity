using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Double : RegHelper_Type
    {
        public CLS_Type_Double()
            : base(typeof(double), "double")
        {

        }
  
        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            Type t = targetType;
            if (t == typeof(double))
                return src;
            if (t == typeof(float))
                return (float)(double)(src);
            if (t == typeof(long))
                return (long)(double)(src);
            if (t == typeof(ulong))
                return (ulong)(double)(src);
            if (t == typeof(int))
                return (int)(double)(src);
            if (t == typeof(uint))
                return (uint)(double)(src);
            if (t == typeof(short))
                return (short)(double)(src);
            if (t == typeof(ushort))
                return (ushort)(double)(src);
            if (t == typeof(sbyte))
                return (sbyte)(double)(src);
            if (t == typeof(byte))
                return (byte)(double)(src);
            if (t == typeof(char))
                return (char)(double)(src);
            return base.ConvertTo(env, src, targetType);
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            if (code == '+')
            {
                if (right.value is float)
                {
                    returntype = type;
                    return (double)left + (float)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (double)left + (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = type;
                    return (double)left + (double)right.value;
                }
                else if (right.value is string)
                {
                    returntype = typeof(string);
                    return Convert.ToString(left) + Convert.ToString(right.value);
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (double)left + Convert.ToSingle(right.value);
                }
            }
            else if (code == '-')
            {
                if (right.value is float)
                {
                    returntype = type;
                    return (double)left - (float)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (double)left - (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = type;
                    return (double)left - (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (double)left - Convert.ToSingle(right.value);
                }
            }
            else if (code == '*')
            {
                if (right.value is float)
                {
                    returntype = type;
                    return (double)left * (float)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (double)left * (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = type;
                    return (double)left * (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (double)left * Convert.ToSingle(right.value);
                }
            }
            else if (code == '/')
            {
                if (right.value is float)
                {
                    returntype = type;
                    return (double)left / (float)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (double)left / (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = type;
                    return (double)left / (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (double)left / Convert.ToSingle(right.value);
                }
            }
            else if (code == '%')
            {
                if (right.value is float)
                {
                    returntype = type;
                    return (double)left % (float)right.value;
                }
                else if (right.value is int)
                {
                    returntype = type;
                    return (double)left % (int)right.value;
                }
                else if (right.value is double)
                {
                    returntype = type;
                    return (double)left % (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    returntype = type;
                    return (double)left % Convert.ToSingle(right.value);
                }
            }

            return base.Math2Value(env, code, left, right, out returntype);
        }

        public override bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                if (right.value is float)
                {
                    return (double)left == (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left == (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left == (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left == Convert.ToSingle(right.value);
                }
            }
            else if (code == logictoken.not_equal)
            {
                if (right.value is float)
                {
                    return (double)left != (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left != (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left != (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left != Convert.ToSingle(right.value);
                }
            }
            else if (code == logictoken.less)
            {
                if (right.value is float)
                {
                    return (double)left < (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left < (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left < (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left < Convert.ToSingle(right.value);
                }
            }
            else if (code == logictoken.less_equal)
            {
                if (right.value is float)
                {
                    return (double)left <= (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left <= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left <= (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left <= Convert.ToSingle(right.value);
                }
            }
            else if (code == logictoken.more)
            {
                if (right.value is float)
                {
                    return (double)left > (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left > (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left > (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left > Convert.ToSingle(right.value);
                }
            }
            else if (code == logictoken.more_equal)
            {
                if (right.value is float)
                {
                    return (double)left >= (float)right.value;
                }
                else if (right.value is int)
                {
                    return (double)left >= (int)right.value;
                }
                else if (right.value is double)
                {
                    return (double)left >= (double)right.value;
                }
                else if (right.value is IConvertible)
                {
                    return (double)left >= Convert.ToSingle(right.value);
                }
            }

            return base.MathLogic(env, code, left, right);
        }

        public override object DefValue
        {
            get { return (double)0; }
        }
    }
}
