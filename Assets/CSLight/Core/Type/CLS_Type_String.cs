using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_String : ICLS_Type
    {
        public CLS_Type_String()
        {
            function = new RegHelper_TypeFunction(typeof(string));
        }

        public string keyword
        {
            get { return "string"; }
        }

        public CLType type
        {
            get { return typeof(string); }
        }

        public object DefValue
        {
            get { return null; }
        }

        public ICLS_Value MakeValue(object value)
        {
            return new CLS_Expression_Value<string>() { value = value };
        }

        public object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return src;
        }

        public object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = typeof(string);
            if (code == '+')
            {
                if (right.value == null)
                {
                    return (string)left + "null";
                }
                else
                {
                    return (string)left + right.value.ToString();
                }
            }
            throw new NotImplementedException();
        }

        public bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                return (string)left == (string)right.value;
            }
            else if(code == logictoken.not_equal)
            {
                return (string)left != (string)right.value;
            }
            throw new NotImplementedException();
        }

        public ICLS_TypeFunction function
        {
            get;
            private set;
        }
    }
}
