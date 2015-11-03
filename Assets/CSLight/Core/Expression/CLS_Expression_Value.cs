using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public class CLS_Expression_Value<T> : ICLS_Expression, ICLS_Value
    {
        public CLType type
        {
            get { return typeof(T); }
        }

        private CLS_Content.Value cacheValue = new CLS_Content.Value() { type = typeof(T), value = default(T) };
        public object value
        {
            get { return cacheValue.value; }
            set { cacheValue.value = value; }
        }

        public override string ToString()
        {
            return type.Name + "|" + value;
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return cacheValue;
        }
    }

    public class CLS_Expression_Enum : ICLS_Expression, ICLS_Value
    {
        public CLS_Expression_Enum(Type type)
        {
            this.type = type;
            this.cacheValue = new CLS_Content.Value() { type = type };
        }

        public CLType type
        {
            get;
            private set;
        }

        private CLS_Content.Value cacheValue;
        public object value
        {
            get { return cacheValue.value; }
            set { cacheValue.value = value; }
        }

        public override string ToString()
        {
            return type.Name + "." + value;
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return cacheValue;
        }
    }

    public class CLS_Null : ICLS_Expression, ICLS_Value
    {
        public CLType type
        {
            get { return null; }
        }
      
        public object value
        {
            get { return null; }
        }

        public override string ToString()
        {
            return "null";
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return CLS_Content.Value.Null;
        }
    }

    public class CLS_True : ICLS_Expression, ICLS_Value
    {
        public CLType type
        {
            get { return typeof(bool); }
        }

        public object value
        {
            get { return true; }
        }

        public override string ToString()
        {
            return "true";
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return CLS_Content.Value.True;
        }
    }

    public class CLS_False : ICLS_Expression, ICLS_Value
    {
        public CLType type
        {
            get { return typeof(bool); }
        }

        public object value
        {
            get { return false; }
        }

        public override string ToString()
        {
            return "false";
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return CLS_Content.Value.False;
        }
    }

    public class CLS_Object : ICLS_Expression, ICLS_Value
    {
        public CLS_Object(Type type)
        {
            this.type = type;
        }

        public CLType type
        {
            get;
            private set;
        }

        public object value
        {
            get;
            set;
        }

        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public int tokenBegin
        {
            get;
            set;
        }

        public int tokenEnd
        {
            get;
            set;
        }

        public int lineBegin
        {
            get;
            set;
        }

        public int lineEnd
        {
            get;
            set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            return new CLS_Content.Value() { type = this.type, value = this.value };
        }
    }
}