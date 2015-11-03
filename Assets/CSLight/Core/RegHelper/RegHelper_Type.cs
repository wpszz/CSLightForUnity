using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_TypeFunction : ICLS_TypeFunction
    {
        protected class CacheMethod
        {
            public ConstructorInfo constructor;
            public MethodInfo method;
            public FieldInfo field;
            public sbyte paramArrayStart = -1;
            public sbyte defaultParamStart = -1;
        }

        protected Type type;

        protected static Dictionary<ICLS_Expression, CacheMethod> s_cacheMethod = new Dictionary<ICLS_Expression, CacheMethod>();
        protected static Dictionary<ICLS_Expression, CacheMethod> s_cacheGet = new Dictionary<ICLS_Expression, CacheMethod>();
        protected static Dictionary<ICLS_Expression, CacheMethod> s_cacheSet = new Dictionary<ICLS_Expression, CacheMethod>();

        public RegHelper_TypeFunction(Type type)
        {
            this.type = type;
        }

        public virtual CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
            Type[] pts = CLS_Content.ParamTypesArray[_params.size];
            object[] ps = CLS_Content.ParamObjsArray[_params.size];
            CLS_Content.Value tempValue;
            for (int i = 0; i < _params.size; i++)
            {
                tempValue = _params[i];
                if (tempValue.type == null)
                {
                    pts[i] = typeof(object);
                }
                else if ((SType)tempValue.type != null)
                {
                    pts[i] = typeof(SInstance);
                }
                else if ((Type)tempValue.type != null)
                {
                    pts[i] = tempValue.type;
                }
                else
                {
                    pts[i] = typeof(object);
                }
                ps[i] = tempValue.value;
            }

            CacheMethod cache;
            if (!s_cacheMethod.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();
                cache.constructor = this.type.GetConstructor(pts);
                s_cacheMethod[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.constructor != null)
            {
                retVal.value = cache.constructor.Invoke(ps);
                retVal.type = this.type;
            }
            else
            {
                retVal.value = Activator.CreateInstance(this.type);
                retVal.type = this.type;
            }
            return retVal;
        }

        public virtual CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
            Type[] pts = CLS_Content.ParamTypesArray[_params.size];
            object[] ps = CLS_Content.ParamObjsArray[_params.size];
            CLS_Content.Value tempValue;
            for (int i = 0; i < _params.size; i++)
            {
                tempValue = _params[i];
                if (tempValue.type == null)
                {
                    pts[i] = typeof(object);
                }
                else if ((SType)tempValue.type != null)
                {
                    pts[i] = typeof(SInstance);
                }
                else if ((Type)tempValue.type != null)
                {
                    pts[i] = tempValue.type;
                }
                else
                {
                    pts[i] = typeof(object);
                }
                ps[i] = tempValue.value;
            }

            CacheMethod cache;
            if (!s_cacheMethod.TryGetValue(content.CallExpression, out cache))
            {
                cache = FindMethod(content, type, function, pts, ps);
                if (cache == null)
                    throw new NotImplementedException("静态函数不存在或参数不匹配:" + type.ToString() + "." + function);
                s_cacheMethod[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.defaultParamStart >= 0)
            {
                ParameterInfo[] pis = cache.method.GetParameters();
                int len = pis.Length;
                int lenOld = ps.Length;
                object[] newPs = CLS_Content.ParamObjsArray[len];
                for (int i = 0; i < len; i++)
                {
                    if (i < lenOld)
                        newPs[i] = ps[i];
                    else
                        newPs[i] = pis[i].DefaultValue;
                }
                retVal.value = cache.method.Invoke(null, newPs);
                retVal.type = cache.method.ReturnType;
            }
            else if (cache.paramArrayStart >= 0)
            {
                int lenOld = ps.Length;
                object[] newPs = new object[cache.paramArrayStart + 1];
                object[] newPs2 = new object[lenOld - cache.paramArrayStart];
                newPs[cache.paramArrayStart] = newPs2;
                for (int i = 0; i < lenOld; i++)
                {
                    if (i < cache.paramArrayStart)
                        newPs[i] = ps[i];
                    else
                        newPs2[i - cache.paramArrayStart] = ps[i];
                }
                retVal.value = cache.method.Invoke(null, newPs);
                retVal.type = cache.method.ReturnType;
            }
            else
            {
                retVal.value = cache.method.Invoke(null, ps);
                retVal.type = cache.method.ReturnType;
            }
            return retVal;
        }

        public virtual CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
            CacheMethod cache;
            if (!s_cacheGet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.field = type.GetField(valuename);
                if (cache.field == null)
                {
                    cache.method = type.GetMethod("get_" + valuename);
                    if (cache.method == null)
                    {
                        Type baseType = type.BaseType;
                        while (baseType != null)
                        {
                            cache.field = baseType.GetField(valuename);
                            if (cache.field == null)
                            {
                                cache.method = baseType.GetMethod("get_" + valuename);
                                if (cache.method != null)
                                    break;
                            }
                            else
                                break;
                            baseType = baseType.BaseType;
                        }
                        if (cache.field == null && cache.method == null)
                            throw new NotImplementedException("静态属性不存在:" + type.ToString() + "." + valuename);
                    }
                }

                s_cacheGet[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.field != null)
            {
                retVal.value = cache.field.GetValue(null);
                retVal.type = cache.field.FieldType;
            }
            else
            {
                retVal.value = cache.method.Invoke(null, null);
                retVal.type = cache.method.ReturnType;
            }
            return retVal;
        }

        public virtual void StaticValueSet(CLS_Content content, string valuename, object value)
        {
            CacheMethod cache;
            if (!s_cacheSet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.field = type.GetField(valuename);
                if (cache.field == null)
                {
                    cache.method = type.GetMethod("set_" + valuename);
                    if (cache.method == null)
                    {
                        Type baseType = type.BaseType;
                        while (baseType != null)
                        {
                            cache.field = baseType.GetField(valuename);
                            if (cache.field == null)
                            {
                                cache.method = baseType.GetMethod("set_" + valuename);
                                if (cache.method != null)
                                    break;
                            }
                            else
                                break;
  
                            baseType = baseType.BaseType;
                        }
                        if (cache.field == null && cache.method == null)
                            throw new NotImplementedException("静态属性不存在:" + type.ToString() + "." + valuename);
                    }
                }

                s_cacheSet[content.CallExpression] = cache;
            }

            if (cache.field != null)
            {
                if (value != null)
                {
                    Type vType = value.GetType();
                    if (vType != cache.field.FieldType)
                        value = content.environment.GetType(vType).ConvertTo(content, value, cache.field.FieldType);
                }
                cache.field.SetValue(null, value); ;
            }
            else
            {
                if (value != null)
                {
                    Type vType = value.GetType();
                    Type pType = cache.method.GetParameters()[0].ParameterType;
                    if (vType != pType)
                        value = content.environment.GetType(vType).ConvertTo(content, value, pType);
                }
                object[] ps = CLS_Content.ParamObjsArray[1];
                ps[0] = value;
                cache.method.Invoke(null, ps);
            }
        }

        public virtual CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
#if UNITY_EDITOR
            if (object_this == null)
                throw new NotImplementedException("MemberCall实例 = null:" + type.ToString() + "." + function);
#endif
            Type[] pts = CLS_Content.ParamTypesArray[_params.size];
            object[] ps = CLS_Content.ParamObjsArray[_params.size];
            CLS_Content.Value tempValue;
            for (int i = 0; i < _params.size; i++)
            {
                tempValue = _params[i];
                if (tempValue.type == null)
                {
                    pts[i] = typeof(object);
                }
                else if ((SType)tempValue.type != null)
                {
                    pts[i] = typeof(SInstance);
                }
                else if ((Type)tempValue.type != null)
                {
                    pts[i] = tempValue.type;
                }
                else
                {
                    pts[i] = typeof(object);
                }
                ps[i] = tempValue.value;
            }

            CacheMethod cache;
            if (!s_cacheMethod.TryGetValue(content.CallExpression, out cache))
            {
                cache = FindMethod(content, type, function, pts, ps);
                if (cache == null)
                    throw new NotImplementedException("实例函数不存在或参数不匹配:" + type.ToString() + "." + function);
                s_cacheMethod[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.defaultParamStart >= 0)
            {
                ParameterInfo[] pis = cache.method.GetParameters();
                int len = pis.Length;
                int lenOld = ps.Length;
                object[] newPs = CLS_Content.ParamObjsArray[len];
                for (int i = 0; i < len; i++)
                {
                    if (i < lenOld)
                        newPs[i] = ps[i];
                    else
                        newPs[i] = pis[i].DefaultValue;
                }
                retVal.value = cache.method.Invoke(object_this, newPs);
                retVal.type = cache.method.ReturnType;
            }
            else if (cache.paramArrayStart >= 0)
            {
                int lenOld = ps.Length;
                object[] newPs = new object[cache.paramArrayStart + 1];
                object[] newPs2 = new object[lenOld - cache.paramArrayStart];
                newPs[cache.paramArrayStart] = newPs2;
                for (int i = 0; i < lenOld; i++)
                {
                    if (i < cache.paramArrayStart)
                        newPs[i] = ps[i];
                    else
                        newPs2[i - cache.paramArrayStart] = ps[i];
                }
                retVal.value = cache.method.Invoke(object_this, newPs);
                retVal.type = cache.method.ReturnType;
            }
            else
            {
                retVal.value = cache.method.Invoke(object_this, ps);
                retVal.type = cache.method.ReturnType;
            }
            return retVal;
        }

        public virtual CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
#if UNITY_EDITOR
            if (object_this == null)
                throw new NotImplementedException("MemberValueGet实例 = null:" + type.ToString() + "." + valuename);
#endif
            CacheMethod cache;
            if (!s_cacheGet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.field = type.GetField(valuename);
                if (cache.field == null)
                {
                    cache.method = type.GetMethod("get_" + valuename);
                    if (cache.method == null)
                    {
                        Type baseType = type.BaseType;
                        while (baseType != null)
                        {
                            cache.field = baseType.GetField(valuename);
                            if (cache.field == null)
                            {
                                cache.method = baseType.GetMethod("get_" + valuename);
                                if (cache.method != null)
                                    break;
                            }
                            else
                                break;
                            baseType = baseType.BaseType;
                        }
                        if (cache.field == null && cache.method == null)
                            throw new NotImplementedException("实例属性不存在:" + type.ToString() + "." + valuename);
                    }
                }

                s_cacheGet[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.field != null)
            {
                retVal.value = cache.field.GetValue(object_this);
                retVal.type = cache.field.FieldType;
            }
            else
            {
                retVal.value = cache.method.Invoke(object_this, null);
                retVal.type = cache.method.ReturnType;
            }
            return retVal;
        }

        public virtual void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
#if UNITY_EDITOR
            if (object_this == null)
                throw new NotImplementedException("MemberValueSet实例 = null:" + type.ToString() + "." + valuename);
#endif
            CacheMethod cache;
            if (!s_cacheSet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.field = type.GetField(valuename);
                if (cache.field == null)
                {
                    cache.method = type.GetMethod("set_" + valuename);
                    if (cache.method == null)
                    {
                        Type baseType = type.BaseType;
                        while (baseType != null)
                        {
                            cache.field = baseType.GetField(valuename);
                            if (cache.field == null)
                            {
                                cache.method = baseType.GetMethod("set_" + valuename);
                                if (cache.method != null)
                                    break;
                            }
                            else
                                break;

                            baseType = baseType.BaseType;
                        }
                        if (cache.field == null && cache.method == null)
                            throw new NotImplementedException("实例属性不存在:" + type.ToString() + "." + valuename);
                    }
                }

                s_cacheSet[content.CallExpression] = cache;
            }

            if (cache.field != null)
            {
                if (value != null)
                {
                    Type vType = value.GetType();
                    if (vType != cache.field.FieldType)
                        value = content.environment.GetType(vType).ConvertTo(content, value, cache.field.FieldType);
                }
                cache.field.SetValue(object_this, value);
            }
            else
            {
                if (value != null)
                {
                    Type vType = value.GetType();
                    Type pType = cache.method.GetParameters()[0].ParameterType;
                    if (vType != pType)
                        value = content.environment.GetType(vType).ConvertTo(content, value, pType);
                }
                object[] ps = CLS_Content.ParamObjsArray[1];
                ps[0] = value;
                cache.method.Invoke(object_this, ps);
            }
        }

        public virtual CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
        {
            CacheMethod cache;
            if (!s_cacheGet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.method = type.GetMethod("get_Item");
                if (cache.method == null)
                {
                    cache.method = type.GetMethod("GetValue", new Type[] { typeof(int) });
                    if (cache.method == null)
                        throw new NotImplementedException("IndexGet不存在:" + type.ToString() + "." + key);
                }

                cache.defaultParamStart = (sbyte)(cache.method.Name == "get_Item" ? 0 : -1);
                s_cacheGet[content.CallExpression] = cache;
            }

            CLS_Content.Value retVal = new CLS_Content.Value();
            if (cache.defaultParamStart == 0)
            {
                object[] ps = CLS_Content.ParamObjsArray[1];
                ps[0] = key;
                retVal.value = cache.method.Invoke(object_this, ps);
                retVal.type = cache.method.ReturnType;
            }
            else
            {
                object[] ps = CLS_Content.ParamObjsArray[1];
                ps[0] = key;
                retVal.value = cache.method.Invoke(object_this, ps);
                retVal.type = type.GetElementType();
            }
            return retVal;
        }

        public virtual void IndexSet(CLS_Content content, object object_this, object key, object value)
        {
            CacheMethod cache;
            if (!s_cacheSet.TryGetValue(content.CallExpression, out cache))
            {
                cache = new CacheMethod();

                cache.method = type.GetMethod("set_Item");
                if (cache.method == null)
                {
                    cache.method = type.GetMethod("SetValue", new Type[] { typeof(object), typeof(int) });
                    if (cache.method == null)
                        throw new NotImplementedException("IndexSet不存在:" + type.ToString() + "." + key);
                }

                cache.defaultParamStart = (sbyte)(cache.method.Name == "set_Item" ? 0 : -1);
                s_cacheSet[content.CallExpression] = cache;
            }

            if (cache.defaultParamStart == 0)
            {
                object[] ps = CLS_Content.ParamObjsArray[2];
                ps[0] = key;
                ps[1] = value;
                cache.method.Invoke(object_this, ps);
            }
            else
            {
                object[] ps = CLS_Content.ParamObjsArray[2];
                ps[0] = value;
                ps[1] = key;
                cache.method.Invoke(object_this, ps);
            }
        }

        // 泛型方法查找
        protected static MethodInfo FindGenericMethod(Type type, string function, Type[] pts, object[] ps, Type[] gtypes, out int defaultParamStart, out int paramArrayStart)
        {
            defaultParamStart = -1;
            paramArrayStart = -1;

            List<MethodInfo> listMethods = getMethodsSorted(type, function, true);
            for (int i = 0, count = listMethods.Count; i < count; i++)
            {
                MethodInfo tmpMethod = listMethods[i];
                if (isMatchParams(tmpMethod, pts, ps, out defaultParamStart, out paramArrayStart))
                    return tmpMethod.MakeGenericMethod(gtypes);
            }
            return null;
        }

        // 方法查找
        protected static CacheMethod FindMethod(CLS_Content content, Type type, string function, Type[] pts, object[] ps)
        {
            MethodInfo md;
            try
            {
                md = type.GetMethod(function, pts);
            }
            catch (System.Exception)
            {
                md = null;
            }
            if (md != null)
            {
                CacheMethod cache = new CacheMethod();
                cache.method = md;
                return cache;
            }

            int defaultParamStart = -1;
            int paramArrayStart = -1;

            if (function[function.Length - 1] == '>')
            {
                string[] sf = function.Split(new char[] { '<', ',', '>' }, StringSplitOptions.RemoveEmptyEntries);
                string tfunc = sf[0];
                Type[] gtypes = new Type[sf.Length - 1];
                for (int i = 1; i < sf.Length; i++)
                {
                    gtypes[i - 1] = content.environment.GetTypeByKeyword(sf[i]).type;
                }
                md = FindGenericMethod(type, tfunc, pts, ps, gtypes, out defaultParamStart, out paramArrayStart);
                if (md != null)
                {
                    CacheMethod cache = new CacheMethod();
                    cache.method = md;
                    cache.defaultParamStart = (sbyte)defaultParamStart;
                    cache.paramArrayStart = (sbyte)paramArrayStart;
                    return cache;
                }
                return null;
            }

            List<MethodInfo> listMethods = getMethodsSorted(type, function, false);
            for (int i = 0, count = listMethods.Count; i < count; i++)
            {
                MethodInfo tmpMethod = listMethods[i];
                if (isMatchParams(tmpMethod, pts, ps, out defaultParamStart, out paramArrayStart))
                {
                    CacheMethod cache = new CacheMethod();
                    cache.method = tmpMethod;
                    cache.defaultParamStart = (sbyte)defaultParamStart;
                    cache.paramArrayStart = (sbyte)paramArrayStart;
                    return cache;
                }
            }

            Type baseType = type.BaseType;
            while (baseType != null)
            {
                CacheMethod cache = FindMethod(content, baseType, function, pts, ps);
                if (cache != null)
                    return cache;
                baseType = baseType.BaseType;
            }

            return null;
        }

        protected static List<MethodInfo> getMethodsSorted(Type type, string function, bool genericMethod)
        {
            List<MethodInfo> listMethods = new List<MethodInfo>();
            MethodInfo[] tmpMethods = type.GetMethods();
            for (int i = 0, len = tmpMethods.Length; i < len; i++)
            {
                MethodInfo tmpMethod = tmpMethods[i];
                if (tmpMethod.Name == function && tmpMethod.IsGenericMethodDefinition == genericMethod)
                    listMethods.Add(tmpMethod);
            }

            listMethods.Sort(delegate(MethodInfo small, MethodInfo big)
            {
                int smallParamCount = small.GetParameters().Length;
                int bigParamCount = big.GetParameters().Length;
                if (smallParamCount < bigParamCount)
                    return -1;
                if (smallParamCount > bigParamCount)
                    return 1;
                return 0;
            });

            return listMethods;
        }

        protected static bool isMatchParams(MethodInfo method, Type[] pts, object[] ps, out int defaultParamStart, out int paramArrayStart)
        {
            defaultParamStart = -1;
            paramArrayStart = -1;

            bool isGeneric = method.IsGenericMethodDefinition;
            int paramCount = pts.Length;
            ParameterInfo[] tmpParams = method.GetParameters();
            int tmpParamCount = tmpParams.Length;
            if (paramCount < tmpParamCount)
            {
                for (int i = 0; i < tmpParamCount; i++)
                {
                    ParameterInfo tmpParam = tmpParams[i];
                    if (i < paramCount)
                    {
                        if (isMatchType(pts[i], ps[i], tmpParam.ParameterType, isGeneric))
                            continue;
                    }
                    else if (isDefaultParam(tmpParam))
                    {
                        defaultParamStart = i;
                        paramArrayStart = -1;
                        return true;
                    }
                    return false;
                }
                return true;
            }
            else
            {
                for (int i = 0; i < paramCount; i++)
                {
                    if (i < tmpParamCount)
                    {
                        ParameterInfo tmpParam = tmpParams[i];
                        if (isParamsArray(tmpParam))
                        {
                            defaultParamStart = -1;
                            paramArrayStart = i;
                            return true;
                        }
                        if (isMatchType(pts[i], ps[i], tmpParam.ParameterType, isGeneric))
                            continue;
                    }
                    return false;
                }
                return paramCount == tmpParamCount;
            }
        }

        protected static bool isMatchType(Type srcType, object srcValue, Type targetType, bool isGeneric)
        {
            if (srcType == targetType)
                return true;
            if (srcType.IsSubclassOf(targetType))
                return true;
            if (targetType.IsValueType)
            {
                if (srcValue is IConvertible && hasIntaface(targetType, typeof(IConvertible)))
                    return true;
            }
            else
            {
                if (srcValue == null)
                    return true;
            }
            // 判断是否是泛型参数
            if (isGeneric && targetType.IsGenericParameter)
                return true;
            return false;
        }

        protected static bool isParamsArray(ParameterInfo param)
        {
            foreach (Attribute attribute in param.GetCustomAttributes(false))
            {
                if (attribute is ParamArrayAttribute)
                    return true;
            }
            return false;
        }

        protected static bool isDefaultParam(ParameterInfo param)
        {
            return param.IsOptional;
            //return (param.Attributes & ParameterAttributes.HasDefault) != 0;
        }

        protected static bool hasIntaface(Type vt, Type i)
        {
            foreach (Type t in vt.GetInterfaces())
            {
                if (t == i)
                    return true;
            }
            return false;
        }
    }

    public class RegHelper_Type : ICLS_Type
    {
        public RegHelper_Type()
        {

        }

        public RegHelper_Type(Type type, string keyword = null)
        {
            this.type = type;
            this.keyword = keyword == null ? type.Name : keyword;
            this.sysType = type;
            this.function = new RegHelper_TypeFunction(type);
        }

        public string keyword
        {
            get;
            protected set;
        }

        public CLType type
        {
            get;
            protected set;
        }

        public Type sysType;

        public virtual ICLS_Value MakeValue(object value)
        {
            CLS_Object retVal = new CLS_Object(type);
            retVal.value = value;
            return retVal;
        }

        public virtual object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            Type targetSysType = (Type)targetType;

            if (sysType == targetSysType) 
                return src;

            if (targetSysType == null)
                return src;

            return src;
        }

        public virtual object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returnType)
        {
            if (code == '+')
            {
                returnType = typeof(string);

                if (left == null)
                    return "null" + right.value;

                if (right.value == null)
                    return left.ToString() + "null";

                return left.ToString() + right.value;
            }

            throw new NotImplementedException("未实现算术运算: " + code);
        }

        public virtual bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                if (left == null)
                    return left == right.value;
                return left.Equals(right.value);
            }
            else if (code == logictoken.not_equal)
            {
                if (left == null)
                    return left != right.value;
                return !left.Equals(right.value);
            }

            throw new NotImplementedException("未实现逻辑运算: " + code);
        }

        public ICLS_TypeFunction function
        {
            get;
            protected set;
        }

        public virtual object DefValue
        {
            get { return null; }
        }
    }
}
