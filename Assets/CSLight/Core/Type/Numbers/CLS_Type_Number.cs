using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Number<T> : RegHelper_Type
    {
        public CLS_Type_Number(string key)
            : base(typeof(T), key)
        {

        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            decimal srcValue = GetDecimalValue(typeof(T), src);
            return DecimalToTargetType(targetType, srcValue);
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            decimal finalValue = 0;
            switch (code)
            {
                case '+':
                    finalValue = GetDecimalValue(typeof(T), left) + GetDecimalValue(right.type, right.value);
                    break;
                case '-':
                    finalValue = GetDecimalValue(typeof(T), left) - GetDecimalValue(right.type, right.value);
                    break;
                case '*':
                    finalValue = GetDecimalValue(typeof(T), left) * GetDecimalValue(right.type, right.value);
                    break;
                case '/':
                    finalValue = GetDecimalValue(typeof(T), left) / GetDecimalValue(right.type, right.value);
                    break;
                case '%':
                    finalValue = GetDecimalValue(typeof(T), left) % GetDecimalValue(right.type, right.value);
                    break;
                case '<':
                    finalValue = (long)(Convert.ToInt64(left) << Convert.ToInt32(right.value));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                case '>':
                    finalValue = (long)(Convert.ToInt64(left) >> Convert.ToInt32(right.value));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                case '&':
                    finalValue = (long)(Convert.ToInt64(left) & Convert.ToInt64(right.value));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                case '|':
                    finalValue = (long)(Convert.ToInt64(left) | Convert.ToInt64(right.value));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                case '~':
                    finalValue = (long)(~Convert.ToInt64(left));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                case '^':
                    finalValue = (long)(Convert.ToInt64(left) ^ Convert.ToInt64(right.value));
                    returntype = typeof(T);
                    return DecimalToTargetType(returntype, finalValue);
                default:
                    throw new Exception("Invalid Math2Value opCode = " + code);
            }

            Type rightSysType = right.type;
            if (sysType == typeof(double) || rightSysType == typeof(double))
            {
                returntype = typeof(double);
            }
            else if (sysType == typeof(float) || rightSysType == typeof(float))
            {
                returntype = typeof(float);
            }
            else if (sysType == typeof(ulong) || rightSysType == typeof(ulong))
            {
                returntype = typeof(ulong);
            }
            // int 和 uint 结合会返回 long.
            else if (sysType == typeof(long) || rightSysType == typeof(long) ||
                     sysType == typeof(int) && rightSysType == typeof(uint) ||
                     sysType == typeof(uint) && rightSysType == typeof(int))
            {
                returntype = typeof(long);
            }
            // uint 和 非int 结合会返回 uint.
            else if (sysType == typeof(uint) && rightSysType != typeof(int) ||
                     sysType != typeof(int) && rightSysType == typeof(uint))
            {
                returntype = typeof(uint);
            }
            // 其他统一返回 int 即可，在C#类型系统中，即使是两个 ushort 结合返回的也是 int 类型。
            else
            {
                returntype = typeof(int);
            }
            return DecimalToTargetType(returntype, finalValue);
        }

        public override bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            decimal leftValue = GetDecimalValue(typeof(T), left);
            decimal rightValue = GetDecimalValue(right.type, right.value);
            switch (code)
            {
                case logictoken.equal:
                    return leftValue == rightValue;
                case logictoken.less:
                    return leftValue < rightValue;
                case logictoken.less_equal:
                    return leftValue <= rightValue;
                case logictoken.more:
                    return leftValue > rightValue;
                case logictoken.more_equal:
                    return leftValue >= rightValue;
                case logictoken.not_equal:
                    return leftValue != rightValue;
                default:
                    throw new Exception("Invalid MathLogic opCode = " + code);
            }
        }

        protected static decimal GetDecimalValue(Type type, object value)
        {
            if (type == typeof(double))
                return (decimal)Convert.ToDouble(value);
            if (type == typeof(float))
                return (decimal)Convert.ToSingle(value);
            if (type == typeof(long))
                return Convert.ToInt64(value);
            if (type == typeof(ulong))
                return Convert.ToUInt64(value);
            if (type == typeof(int))
                return Convert.ToInt32(value);
            if (type == typeof(uint))
                return Convert.ToUInt32(value);
            if (type == typeof(short))
                return Convert.ToInt16(value);
            if (type == typeof(ushort))
                return Convert.ToUInt16(value);
            if (type == typeof(sbyte))
                return Convert.ToSByte(value);
            if (type == typeof(byte))
                return Convert.ToByte(value);
            if (type == typeof(char))
                return Convert.ToChar(value);
            throw new Exception("unknown decimal type...");
        }

        protected static object DecimalToTargetType(Type type, decimal value)
        {
            if (type == typeof(double))
                return (double)value;
            if (type == typeof(float))
                return (float)value;

            if (type == typeof(long))
                return value > long.MaxValue ? (long)Convert.ToUInt64(value) : Convert.ToInt64(value);
            if (type == typeof(ulong))
                return value > long.MaxValue ? Convert.ToUInt64(value) : (ulong)Convert.ToInt64(value);

            long vl = value > long.MaxValue ? (long)Convert.ToUInt64(value) : Convert.ToInt64(value);
            if (type == typeof(int))
                return (int)(vl & 0xffffffff);
            if (type == typeof(uint))
                return (uint)(vl & 0xffffffff);
            if (type == typeof(short))
                return (short)(vl & 0xffff);
            if (type == typeof(ushort))
                return (ushort)(vl & 0xffff);
            if (type == typeof(sbyte))
                return (sbyte)(vl & 0xff);
            if (type == typeof(byte))
                return (byte)(vl & 0xff);
            if (type == typeof(char))
                return (char)(vl & 0xff);

            if (type == typeof(int?))
                return (int?)(vl & 0xffffffff);
            if (type == typeof(uint?))
                return (uint?)(vl & 0xffffffff);

            throw new Exception("unknown target type...");
        }
    }
}
