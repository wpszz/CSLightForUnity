using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Type_List<T> : RegHelper_Type
    {
        public CLS_Type_List(string keyword)
        {
            this.type = typeof(List<T>);
            this.sysType = this.type;
            this.keyword = keyword;
            this.function = new CLS_Type_List_Fun(this.type);
        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return (List<T>)src;
        }

        public class CLS_Type_List_Fun : RegHelper_TypeFunction
        {
            public System.Type m_TType;

            public CLS_Type_List_Fun(Type type)
                : base(type)
            {
                this.m_TType = typeof(T);
            }

            public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
            {
                CLS_Content.Value retVal = new CLS_Content.Value();
                if (_params.size > 0)
                {
			        if ((_params[0].value == null || _params[0].value is IEnumerable))
			        {
                        retVal.value = new List<T>((IEnumerable<T>)(_params[0].value));
			        }
                    else
                    {
                        retVal.value = new List<T>(Convert.ToInt32(_params[0].value));
                    }
                }
                else
                {
                    retVal.value = new List<T>();
                }
                retVal.type = type;
                return retVal;
            }

            public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
            {
                if (function == "Add")
                {
                    object value = _params[0].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                    }
                    ((List<T>)object_this).Add((T)value);
                    return CLS_Content.Value.Void;
                }
                else if (function == "Remove")
                {
                    object value = _params[0].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                    }
                    return new CLS_Content.Value() { type = typeof(bool), value = ((List<T>)object_this).Remove((T)value) };
                }
                else if (function == "RemoveAt")
                {
                    ((List<T>)object_this).RemoveAt(Convert.ToInt32(_params[0].value));
                    return CLS_Content.Value.Void;
                }
                else if (function == "Contains")
                {
                    object value = _params[0].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                    }
                    return new CLS_Content.Value() { type = typeof(bool), value = ((List<T>)object_this).Contains((T)value) };
                }
                else if (function == "Clear")
                {
                    ((List<T>)object_this).Clear();
                    return CLS_Content.Value.Void;
                }
                else if (function == "IndexOf")
                {
                    object value = _params[0].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                    }
                    return new CLS_Content.Value() { type = typeof(int), value = ((List<T>)object_this).IndexOf((T)value) };
                }
                else if (function == "Insert")
                {
                    object value = _params[1].value;
                    if (value != null)
                    {
                        System.Type valType = value.GetType();
                        if (valType != m_TType)
                            value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                    }
                    ((List<T>)object_this).Insert(Convert.ToInt32(_params[0].value), (T)value);
                    return CLS_Content.Value.Void;
                }
                else if (function == "AddRange")
                {
                    ((List<T>)object_this).AddRange((IEnumerable<T>)_params[0].value);
                    return CLS_Content.Value.Void;
                }
                return base.MemberCall(content, object_this, function, _params, isBaseCall);
            }

            public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
            {
                if (valuename == "Count")
                    return new CLS_Content.Value() { type = typeof(int), value = ((List<T>)object_this).Count };
                if (valuename == "Capacity")
                    return new CLS_Content.Value() { type = typeof(int), value = ((List<T>)object_this).Capacity };
                return base.MemberValueGet(content, object_this, valuename, isBaseCall);
            }

            public override CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
            {
                return new CLS_Content.Value() { type = typeof(T), value = ((List<T>)object_this)[Convert.ToInt32(key)] };
            }

            public override void IndexSet(CLS_Content content, object object_this, object key, object value)
            {
                if (value != null)
                {
                    System.Type valType = value.GetType();
                    if (valType != m_TType)
                        value = content.environment.GetType(valType).ConvertTo(content, value, m_TType);
                }
                ((List<T>)object_this)[Convert.ToInt32(key)] = (T)value;
            }
        }
    }
}
