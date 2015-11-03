using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    class CLS_Type_Void : ICLS_Type
    {
        public string keyword
        {
            get { return "void"; }
        }

        public CLType type
        {
            get { return null; }
        }

        public object DefValue
        {
            get { return null; }
        }

        public ICLS_Value MakeValue(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            throw new NotImplementedException();
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
