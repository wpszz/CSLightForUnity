using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_Array<T> : RegHelper_Type
    {
        public CLS_Type_Array(string keyword)
        {
            this.type = typeof(T[]);
            this.sysType = this.type;
            this.keyword = keyword;
            this.function = new CLS_Type_Array_Fun(this.type);
        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return (T[])src;
        }

        public class CLS_Type_Array_Fun : RegHelper_TypeFunction
        {
            public System.Type m_TType;

            public CLS_Type_Array_Fun(Type type)
                : base(type)
            {
                this.m_TType = typeof(T);
            }

            public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
            {
                CLS_Content.Value retVal = new CLS_Content.Value();
                retVal.value = new T[Convert.ToInt32(_params[0].value)];
                retVal.type = type;
                return retVal;
            }

            public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
            {
                if (valuename == "Length")
                    return new CLS_Content.Value() { type = typeof(int), value = ((T[])object_this).Length };
                // WP8不支持
                //if (valuename == "LongLength")
                //    return new CLS_Content.Value() { type = typeof(long), value = ((T[])object_this).LongLength };
                return base.MemberValueGet(content, object_this, valuename, isBaseCall);
            }

            public override CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
            {
                return new CLS_Content.Value() { type = typeof(T), value = ((T[])object_this)[Convert.ToInt32(key)] };
            }

            public override void IndexSet(CLS_Content content, object object_this, object key, object value)
            {
                if (value != null)
                {
                    System.Type valType = value.GetType();
                    if (valType != m_TType)
                        value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                }
                ((T[])object_this)[Convert.ToInt32(key)] = (T)value;
            }
        }
    }
}
