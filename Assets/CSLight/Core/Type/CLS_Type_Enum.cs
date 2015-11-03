using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_Enum : ICLS_Type
    {
        public string keyword
        {
            get;
            private set;
        }

        public CLType type
        {
            get;
            private set;
        }

        public object DefValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICLS_TypeFunction function
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type sysType;

        public CLS_Type_Enum(Type type, string keyword = null)
        {
            this.type = type;
            this.keyword = keyword == null ? type.Name : keyword;
            this.sysType = type;
        }

        public ICLS_Value MakeValue(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(CLS_Content content, object src, CLType targetType)
        {
            Type targetSysType = (Type)targetType;

            if (targetSysType.IsEnum)
                return Enum.ToObject(targetSysType, src);
            else if (targetSysType == typeof(int))
                return System.Convert.ToInt32(src);
            else if (targetSysType == typeof(uint))
                return System.Convert.ToUInt32(src);
            else if (targetSysType == typeof(short))
                return System.Convert.ToInt16(src);
            else if (targetSysType == typeof(ushort))
                return System.Convert.ToUInt16(src);
            else if (targetSysType == typeof(long))
                return System.Convert.ToInt64(src);
            else if (targetSysType == typeof(ulong))
                return System.Convert.ToUInt64(src);

            throw new NotImplementedException();
        }

        public object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returnType)
        {
            returnType = type;

            int tLeft = System.Convert.ToInt32(left);
            int tRight = System.Convert.ToInt32(right.value);

            switch (code)
            {
                case '+':
                    return tLeft + tRight;
                case '-':
                    return tLeft - tRight;
                case '*':
                    return tLeft * tRight;
                case '/':
                    return tLeft / tRight;
                case '%':
                    return tLeft % tRight;
                case '<':
                    return tLeft << tRight;
                case '>':
                    return tLeft >> tRight;
                case '&':
                    return tLeft & tRight;
                case '|':
                    return tLeft | tRight;
                case '~':
                    return ~tLeft;
                case '^':
                    return tLeft ^ tRight;
            }

            throw new NotImplementedException();
        }

        public bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
        {
            int tLeft = System.Convert.ToInt32(left);
            int tRight = System.Convert.ToInt32(right.value);

            switch (code)
            {
                case logictoken.equal:
                    return tLeft == tRight;
                case logictoken.not_equal:
                    return tLeft != tRight;
                case logictoken.more:
                    return tLeft > tRight;
                case logictoken.more_equal:
                    return tLeft >= tRight;
                case logictoken.less:
                    return tLeft < tRight;
                case logictoken.less_equal:
                    return tLeft <= tRight;
            }

            throw new NotImplementedException();
        }
    }
}
