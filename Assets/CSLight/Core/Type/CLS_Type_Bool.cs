using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_Bool : ICLS_Type
    {
        public string keyword
        {
            get { return "bool"; }
        }

        public CLType type
        {
            get { return (typeof(bool)); }
        }

        public object DefValue
        {
            get { return false; }
        }

        public ICLS_Value MakeValue(object value)
        {
            if ((bool)value)
                return new CLS_True();
            return new CLS_False();
        }

        public object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return src;
        }

        public object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            throw new NotImplementedException();
        }

        public ICLS_TypeFunction function
        {
            get { throw new NotImplementedException(); }
        }
    }
}
