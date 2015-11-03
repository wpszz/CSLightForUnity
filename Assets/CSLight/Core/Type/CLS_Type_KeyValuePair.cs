using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_KeyValuePair<TKey, TValue> : RegHelper_Type
    {
        public CLS_Type_KeyValuePair(string keyword)
        {
            this.type = typeof(KeyValuePair<TKey, TValue>);
            this.sysType = this.type;
            this.keyword = keyword;
            this.function = new CLS_Type_KeyValuePair_Fun(this.sysType);
        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return (KeyValuePair<TKey, TValue>)src;
        }

        public class CLS_Type_KeyValuePair_Fun : RegHelper_TypeFunction
        {
            public System.Type m_TKeyType;
            public System.Type m_TValueType;

            public CLS_Type_KeyValuePair_Fun(Type type)
                : base(type)
            {
                this.m_TKeyType = typeof(TKey);
                this.m_TValueType = typeof(TValue);
            }

            public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
            {
                if (valuename == "Key")
                    return new CLS_Content.Value() { type = typeof(TKey), value = ((KeyValuePair<TKey, TValue>)object_this).Key };
                if (valuename == "Value")
                    return new CLS_Content.Value() { type = typeof(TValue), value = ((KeyValuePair<TKey, TValue>)object_this).Value };
                return base.MemberValueGet(content, object_this, valuename, isBaseCall);
            }
        }
    }
}
