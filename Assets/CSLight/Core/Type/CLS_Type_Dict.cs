using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_Dict<TKey, TValue> : RegHelper_Type
    {
        public CLS_Type_Dict(string keyword)
        {
            this.type = typeof(Dictionary<TKey, TValue>);
            this.sysType = this.type;
            this.keyword = keyword;
            this.function = new CLS_Type_Dict_Fun(this.sysType);
        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return (Dictionary<TKey, TValue>)src;
        }

        public class CLS_Type_Dict_Fun : RegHelper_TypeFunction
        {
            public System.Type m_TKeyType;
            public System.Type m_TValueType;

            public CLS_Type_Dict_Fun(Type type)
                : base(type)
            {
                this.m_TKeyType = typeof(TKey);
                this.m_TValueType = typeof(TValue);
            }

            public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
            {
                CLS_Content.Value retVal = new CLS_Content.Value();
                if (_params.size > 0)
                {
                    if ((_params[0].value == null || _params[0].value is IDictionary))
			        {
                        retVal.value = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>)(_params[0].value));
			        }
                    else
                    {
                        retVal.value = new Dictionary<TKey, TValue>(Convert.ToInt32(_params[0].value));
                    }
                }
                else
                {
                    retVal.value = new Dictionary<TKey, TValue>();
                }
                retVal.type = type;
                return retVal;
            }

            public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
            {
                if (function == "Add")
                {
                    object value = _params[1].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TValueType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TValueType);
                    }
                    ((Dictionary<TKey, TValue>)object_this).Add((TKey)_params[0].value, (TValue)value);
                    return CLS_Content.Value.Void;
                }
                else if (function == "Remove")
                {
                    return new CLS_Content.Value() { type = typeof(bool), value = ((Dictionary<TKey, TValue>)object_this).Remove((TKey)_params[0].value) };
                }
                else if (function == "ContainsKey")
                {
                    return new CLS_Content.Value() { type = typeof(bool), value = ((Dictionary<TKey, TValue>)object_this).ContainsKey((TKey)_params[0].value) };
                }
                else if (function == "Clear")
                {
                    ((Dictionary<TKey, TValue>)object_this).Clear();
                    return CLS_Content.Value.Void;
                }
                return base.MemberCall(content, object_this, function, _params, isBaseCall);
            }

            public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
            {
                if (valuename == "Count")
                    return new CLS_Content.Value() { type = typeof(int), value = ((Dictionary<TKey, TValue>)object_this).Count };
                return base.MemberValueGet(content, object_this, valuename, isBaseCall);
            }

            public override CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
            {
                return new CLS_Content.Value() { type = typeof(TValue), value = ((Dictionary<TKey, TValue>)object_this)[(TKey)key] };
            }

            public override void IndexSet(CLS_Content content, object object_this, object key, object value)
            {
                if (value != null)
                {
                    System.Type valType = value.GetType();
                    if (valType != m_TValueType)
                        value = content.environment.GetType(valType).ConvertTo(content, value, m_TValueType);
                }
                ((Dictionary<TKey, TValue>)object_this)[(TKey)key] = (TValue)value;
            }
        }
    }
}
