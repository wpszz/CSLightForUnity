using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class SType : ICLS_TypeFunction
    {
        public SType(string keyword, string filename)
        {
            this.Name = keyword;
            this.filename = filename;
        }

        public string filename
        {
            get;
            private set;
        }

        public IList<Token> tokenlist
        {
            get;
            private set;
        }

        public void EmbDebugToken(IList<Token> tokens)
        {
            this.tokenlist = tokens;
        }

        public string Name
        {
            get;
            private set;

        }

        public SType BaseType
        {
            get;
            set;
        }

        #region Script IMPL

        public CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
            if (staticMemberContent == null)
                NewStatic(content.environment);

            SInstance scriptInstance = new SInstance();
            scriptInstance.type = this;

            foreach (var pair in this.members)
            {
                Member member = pair.Value;
                if (!member.bStatic)
                {
                    CLS_Content.Value val = new CLS_Content.Value();
                    val.type = member.type.type;
                    if (member.expr_defvalue == null)
                    {
                        val.value = member.type.DefValue;
                    }
                    else
                    {
                        CLS_Content.Value value = member.expr_defvalue.ComputeValue(staticMemberContent);
                        if (member.type.type != value.type)
                        {
                            val.value = content.environment.GetType(value.type).ConvertTo(content, value.value, member.type.type);
                        }
                        else
                        {
                            val.value = value.value;
                        }
                    }
                    scriptInstance.member[pair.Key] = val;
                }
            }

            CLS_Content.Value thisVal = new CLS_Content.Value() { type = this, value = scriptInstance };

            // 加入this关键字
            scriptInstance.member["this"] = thisVal;

            // 加入base关键字
            if (BaseType != null)
                scriptInstance.member["base"] = new CLS_Content.Value() { type = BaseType, value = scriptInstance, breakBlock = 255 };

            // 调用构造函数
            if (this.functions.ContainsKey(this.Name))
                MemberCall(content, scriptInstance, this.Name, _params);

            return thisVal;
        }

        void NewStatic(ICLS_Environment env)
        {
            staticMemberContent = CLS_Content.NewContent(env);
            staticMemberValues = new Dictionary<string, CLS_Content.Value>();

            // 先初始化基类静态数据
            if (BaseType != null)
            {
                BaseType.NewStatic(env);

                // 拷贝基类的静态数据
                foreach (var pair in BaseType.staticMemberValues)
                {
                    staticMemberValues.Add(pair.Key, pair.Value);
                }
            }

            // 添加自身的静态成员数据
            foreach (var pair in this.members)
            {
                if (pair.Value.bStatic && !staticMemberValues.ContainsKey(pair.Key))
                {
                    CLS_Content.Value val = new CLS_Content.Value();
                    val.type = pair.Value.type.type;
                    if (pair.Value.expr_defvalue == null)
                    {
                        val.value = pair.Value.type.DefValue;
                    }
                    else
                    {
                        CLS_Content.Value value = pair.Value.expr_defvalue.ComputeValue(staticMemberContent);
                        if (pair.Value.type.type != value.type)
                        {
                            val.value = env.GetType(value.type).ConvertTo(staticMemberContent, value.value, pair.Value.type.type);
                        }
                        else
                        {
                            val.value = value.value;
                        }
                    }
                    staticMemberValues[pair.Key] = val;
                }
            }
        }

        public CLS_Content.Value StaticCall(CLS_Content contentParent, string func, BetterList<CLS_Content.Value> _params)
        {
            if (staticMemberContent == null)
                NewStatic(contentParent.environment);

            // 静态函数判断
            Function fun;
            if (this.functions.TryGetValue(func, out fun))
            {
#if UNITY_EDITOR
                if (!fun.bStatic)
                    throw new Exception("成员函数必须通过实例来调用: " + this.Name + "." + func);
#endif
                CLS_Content.Value value = null;
                if (fun.expr_runtime != null)
                {
                    CLS_Content content = CLS_Content.NewContent(contentParent.environment);
#if UNITY_EDITOR
                    contentParent.InStackContent(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    content.CallType = this;
                    content.CallThis = null;

                    for (int i = 0, count = fun._paramtypes.Count; i < count; i++)
                    {
                        content.DefineAndSet(fun._paramnames[i], fun._paramtypes[i].type, _params[i].value);
                    }

                    value = fun.expr_runtime.ComputeValue(content);
                    if (value != null)
                        value.breakBlock = 0;
#if UNITY_EDITOR
                    contentParent.OutStackContent(content);
#endif
                    CLS_Content.PoolContent(content);
                }
                return value;
            }

            // 委托判断
            CLS_Content.Value smDeleVal;
            if (this.staticMemberValues.TryGetValue(func, out smDeleVal))
            {
                Delegate dele = smDeleVal.value as Delegate;
                if (dele != null)
                {
                    CLS_Content.Value value = new CLS_Content.Value();
                    value.type = null;
                    object[] objs = CLS_Content.ParamObjsArray[_params.size];
                    for (int i = 0; i < _params.size; i++)
                    {
                        objs[i] = _params[i].value;
                    }
                    value.value = dele.DynamicInvoke(objs);
                    if (value.value != null)
                        value.type = value.value.GetType();
                    value.breakBlock = 0;
                    return value;
                }
            }

            throw new NotImplementedException("未实现静态函数: " + this.Name + "." + func);
        }

        public CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
            if (staticMemberContent == null)
                NewStatic(content.environment);

            CLS_Content.Value sV;
            if (this.staticMemberValues.TryGetValue(valuename, out sV))
                return sV;

            // 判断是否有同名的Get属性
            Member property;
            if (this.propertys.TryGetValue(valuename, out property))
            {
                if (property.getFun != null)
                {
                    return this.StaticCall(content, property.getFun, null);
                }
                throw new NotImplementedException("静态属性无get权限: " + this.Name + "." + valuename);
            }

#if UNITY_EDITOR
            // 判断是否有同名的委托函数
            Function fun;
            if (!this.functions.TryGetValue(valuename, out fun))
                throw new NotImplementedException("未实现静态字段: " + this.Name + "." + valuename);
            if (!fun.bStatic)
                throw new Exception("成员委托必通过实例来获取: " + this.Name + "." + valuename);
#endif
            CLS_Content.Value v = new CLS_Content.Value();
            v.type = typeof(DeleFunction);
            v.value = new DeleFunction(this, null, valuename);
            return v;
        }

        public bool TryStaticValueGet(CLS_Content content, string valuename, out CLS_Content.Value outVal)
        {
            if (staticMemberContent == null)
                NewStatic(content.environment);

            if (this.staticMemberValues.TryGetValue(valuename, out outVal))
                return true;

            // 判断是否有同名的Get属性
            Member property;
            if (this.propertys.TryGetValue(valuename, out property))
            {
                if (property.getFun != null)
                {
                    outVal = this.StaticCall(content, property.getFun, null);
                    return true;
                }
                throw new NotImplementedException("静态属性无get权限: " + this.Name + "." + valuename);
            }

            // 判断是否有同名的委托函数
            Function fun;
            if (this.functions.TryGetValue(valuename, out fun))
            {
#if UNITY_EDITOR
                if (!fun.bStatic)
                    throw new Exception("成员委托必通过实例来获取: " + this.Name + "." + valuename);
#endif
                outVal = new CLS_Content.Value();
                outVal.type = typeof(DeleFunction);
                outVal.value = new DeleFunction(this, null, valuename);
                return true;
            }
            outVal = null;
            return false;
        }

        public void StaticValueSet(CLS_Content content, string valuename, object value)
        {
            if (staticMemberContent == null)
                NewStatic(content.environment);

            CLS_Content.Value sV;
            if (this.staticMemberValues.TryGetValue(valuename, out sV))
            {
                sV.value = value;
                sV.FixValueType(content);
                return;
            }

            // 判断是否有同名的Set属性
            Member property;
            if (this.propertys.TryGetValue(valuename, out property))
            {
                if (property.setFun != null)
                {
                    BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
                    CLS_Content.Value hideValue = new CLS_Content.Value() { type = property.type.type, value = value };
                    hideValue.FixValueType(content);
                    _params.Add(hideValue);
                    this.StaticCall(content, property.setFun, _params);
                    CLS_Content.PoolParamList(_params);
                    return;
                }
                throw new NotImplementedException("属性无set权限: " + this.Name + "." + valuename);
            }

            throw new NotImplementedException("未实现静态字段: " + this.Name + "." + valuename);
        }

        public bool TryStaticValueSet(CLS_Content content, string valuename, object value)
        {
            if (staticMemberContent == null)
                NewStatic(content.environment);

            CLS_Content.Value sV;
            if (this.staticMemberValues.TryGetValue(valuename, out sV))
            {
                sV.value = value;
                sV.FixValueType(content);
                return true;
            }

            // 判断是否有同名的Set属性
            Member property;
            if (this.propertys.TryGetValue(valuename, out property))
            {
                if (property.setFun != null)
                {
                    BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
                    CLS_Content.Value hideValue = new CLS_Content.Value() { type = property.type.type, value = value };
                    hideValue.FixValueType(content);
                    _params.Add(hideValue);
                    this.StaticCall(content, property.setFun, _params);
                    CLS_Content.PoolParamList(_params);
                    return true;
                }
                throw new NotImplementedException("属性无set权限: " + this.Name + "." + valuename);
            }

            return false;
        }

        public CLS_Content.Value MemberCall(CLS_Content contentParent, object object_this, string func, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            SInstance callThis = object_this as SInstance;

            // 成员函数判断
            Function fun;
            if (this.functions.TryGetValue(func, out fun))
            {
#if UNITY_EDITOR
                if (fun.bStatic)
                    throw new Exception("不能通过实例来调用静态函数: " + this.Name + "." + func);
#endif
                CLS_Content.Value value = null;

                SType callType = this;

                if (isBaseCall)
                {
                    SType tempType = contentParent.CallType.BaseType;
                    SType.Function tempFun;
                    while (tempType != null)
                    {
                        if (tempType.functions.TryGetValue(func, out tempFun))
                        {
                            if (tempFun.ownerType == null || tempFun.ownerType == tempType)
                            {
                                callType = tempType;
                                fun = tempFun;
                                break;
                            }
                        }
                        tempType = tempType.BaseType;
                    }
                }
                else
                {
                    if (callType != callThis.type)
                    {
                        SType tempType = callThis.type;
                        Function tempFun;
                        while (tempType != null)
                        {
                            if (tempType.functions.TryGetValue(func, out tempFun))
                            {
                                if (tempFun.ownerType == null || tempFun.ownerType == tempType)
                                {
                                    callType = tempType;
                                    fun = tempFun;
                                    break;
                                }
                            }
                            tempType = tempType.BaseType;
                        }
                    }
                    else if (fun.ownerType != null && fun.ownerType != callType)
                    {
                        callType = fun.ownerType;
                    }
                }

                if (fun.expr_runtime != null)
                {
                    CLS_Content content = CLS_Content.NewContent(contentParent.environment);
#if UNITY_EDITOR
                    contentParent.InStackContent(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    content.CallType = callType;
                    content.CallThis = callThis;

                    for (int i = 0, count = fun._paramtypes.Count; i < count; i++)
                    {
                        content.DefineAndSet(fun._paramnames[i], fun._paramtypes[i].type, _params[i].value);
                    }

                    value = fun.expr_runtime.ComputeValue(content);
                    if (value != null)
                        value.breakBlock = 0;
#if UNITY_EDITOR
                    contentParent.OutStackContent(content);
#endif
                    CLS_Content.PoolContent(content);
                }

                return value;
            }

            // 委托判断
            CLS_Content.Value mDeleVal;
            if (callThis.member.TryGetValue(func, out mDeleVal))
            {
                Delegate dele = mDeleVal.value as Delegate;
                if (dele != null)
                {
                    CLS_Content.Value value = new CLS_Content.Value();
                    value.type = null;
                    object[] objs = CLS_Content.ParamObjsArray[_params.size];
                    for (int i = 0; i < _params.size; i++)
                    {
                        objs[i] = _params[i].value;
                    }
                    value.value = dele.DynamicInvoke(objs);
                    if (value.value != null)
                        value.type = value.value.GetType();
                    value.breakBlock = 0;
                    return value;
                }
            }
            throw new NotImplementedException("未实现成员函数: " + this.Name + "." + func);
        }

        public CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
            SInstance sin = object_this as SInstance;
            CLS_Content.Value mV;
            if (sin.member.TryGetValue(valuename, out mV))
                return mV;

            // 判断是否有同名的Get属性
            Member property;
            if (sin.type.propertys.TryGetValue(valuename, out property))
            {
                if (property.getFun != null)
                {
                    return this.MemberCall(content, sin, property.getFun, null, isBaseCall);
                }
                throw new NotImplementedException("属性无get权限: " + this.Name + "." + valuename);
            }

            // 判断是否有同名的委托函数
            if (sin.type.functions.ContainsKey(valuename))
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.type = typeof(DeleFunction);
                v.value = new DeleFunction(this, sin, valuename);
                return v;
            }

            throw new NotImplementedException("未实现成员字段: " + this.Name + "." + valuename);
        }

        public bool TryMemberValueGet(CLS_Content content, SInstance sin, string valuename, out CLS_Content.Value outVal)
        {
            if (sin.member.TryGetValue(valuename, out outVal))
                return true;

            // 判断是否有同名的Get属性
            Member property;
            if (sin.type.propertys.TryGetValue(valuename, out property))
            {
                if (property.getFun != null)
                {
                    if (property.bStatic)
                        outVal = this.StaticCall(content, property.getFun, null);
                    else
                        outVal = this.MemberCall(content, sin, property.getFun, null);
                    return true;
                }
                throw new NotImplementedException("属性无get权限: " + this.Name + "." + valuename);
            }

            // 判断是否有同名的委托函数
            if (sin.type.functions.ContainsKey(valuename))
            {
                outVal = new CLS_Content.Value();
                outVal.type = typeof(DeleFunction);
                outVal.value = new DeleFunction(this, sin, valuename);
                return true;
            }

            outVal = null;
            return false;
        }

        public void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
            SInstance sin = object_this as SInstance;
            CLS_Content.Value mV;
            if (sin.member.TryGetValue(valuename, out mV))
            {
                mV.value = value;
                mV.FixValueType(content);
                return;
            }

            // 判断是否有同名的Set属性
            Member property;
            if (sin.type.propertys.TryGetValue(valuename, out property))
            {
                if (property.setFun != null)
                {
                    BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
                    CLS_Content.Value hideValue = new CLS_Content.Value() { type = property.type.type, value = value };
                    hideValue.FixValueType(content);
                    _params.Add(hideValue);
                    this.MemberCall(content, object_this, property.setFun, _params, isBaseCall);
                    CLS_Content.PoolParamList(_params);
                    return;
                }
                throw new NotImplementedException("属性无set权限: " + this.Name + "." + valuename);
            }

            throw new NotImplementedException("未实现成员赋值字段: " + this.Name + "." + valuename);
        }

        public bool TryMemberValueSet(CLS_Content content, SInstance sin, string valuename, object value)
        {
            CLS_Content.Value mV;
            if (sin.member.TryGetValue(valuename, out mV))
            {
                mV.value = value;
                mV.FixValueType(content);
                return true;
            }

            // 判断是否有同名的Set属性
            Member property;
            if (sin.type.propertys.TryGetValue(valuename, out property))
            {
                if (property.setFun != null)
                {
                    BetterList<CLS_Content.Value> _params = CLS_Content.NewParamList();
                    CLS_Content.Value hideValue = new CLS_Content.Value() { type = property.type.type, value = value };
                    hideValue.FixValueType(content);
                    _params.Add(hideValue);
                    if (property.bStatic)
                        this.StaticCall(content, property.setFun, _params);
                    else
                        this.MemberCall(content, sin, property.setFun, _params);
                    CLS_Content.PoolParamList(_params);
                    return true;
                }
                throw new NotImplementedException("属性无set权限: " + this.Name + "." + valuename);
            }

            return false;
        }

        public CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
        {
            throw new NotImplementedException();
        }

        public void IndexSet(CLS_Content content, object object_this, object key, object value)
        {
            throw new NotImplementedException();
        }
        #endregion

        public class Function
        {
            public SType ownerType;
            public bool bStatic;
            public List<string> _paramnames = new List<string>();
            public List<ICLS_Type> _paramtypes = new List<ICLS_Type>();
            public ICLS_Expression expr_runtime;
            public Delegate staticDele;

            // 添加基类函数
            public void addBaseFun(ICLS_Expression baseFun, int tbegin, int tend, int lbegin, int lend)
            {
                if (expr_runtime == null)
                {
                    expr_runtime = baseFun;
                }
                else if (!(expr_runtime is CLS_Expression_Block))
                {
                    CLS_Expression_Block baseBlock = new CLS_Expression_Block(tbegin, tend, lbegin, lend);
                    baseBlock.listParam.Add(baseFun);
                    baseBlock.listParam.Add(expr_runtime);
                    expr_runtime = baseBlock;
                }
                else
                {
                    expr_runtime.listParam.Insert(0, baseFun);
                }
            }
        }

        public class Member
        {
            public ICLS_Type type;
            public bool bStatic;
            public int sortIndex;
            public ICLS_Expression expr_defvalue;
            public string getFun;
            public string setFun;
        }

        public Dictionary<string, Function> functions = new Dictionary<string, Function>();
        public Dictionary<string, Member> members = new Dictionary<string, Member>();
        public Dictionary<string, Member> propertys = new Dictionary<string, Member>();

        public CLS_Content staticMemberContent = null;
        public Dictionary<string, CLS_Content.Value> staticMemberValues = null;

        public List<string> sortProtoFieldKeys = null;

        // 添加函数
        public void addFun(string funName, Function fun)
        {
            // 目前脚本类只支持一个同名方法
            if (functions.ContainsKey(funName))
                throw new Exception("目前脚本类只支持一个同名方法: " + Name + "." + funName);
            else
                functions.Add(funName, fun);
        }

        // 添加成员/属性
        public void addMember(string memberName, Member member)
        {
            if (member.getFun == null && member.setFun == null)
            {
                // 目前脚本类只支持一个同名成员
                if (members.ContainsKey(memberName))
                    throw new Exception("目前脚本类只支持一个同名成员: " + Name + "." + member);
                else
                    members.Add(memberName, member);
            }
            else
            {
                // 目前脚本类只支持一个同名成员
                if (propertys.ContainsKey(memberName))
                    throw new Exception("目前脚本类只支持一个同名成员: " + Name + "." + member);
                else
                    propertys.Add(memberName, member);
            }
        }

        // 是否是基类
        public bool isBaseType(SType targetType)
        {
            if (BaseType == null)
                return false;

            if (BaseType == targetType)
                return true;

            return BaseType.isBaseType(targetType);
        }
    }

    public class SInstance
    {
        public SType type;
        public Dictionary<string, CLS_Content.Value> member = new Dictionary<string, CLS_Content.Value>();//成员
        public Dictionary<string, Delegate> cacheDeles = new Dictionary<string, Delegate>();
    }

    public class CLS_Type_Class : ICLS_Type
    {
        public CLS_Type_Class(string keyword, string filename = null)
        {
            this.keyword = keyword;
            this.compiled = false;
            this.type = new SType(keyword, filename);
            this.function = (SType)type;
        }

        public string keyword
        {
            get;
            private set;
        }

        public bool compiled
        {
            get;
            set;
        }

        public CLType type
        {
            get;
            private set;
        }

        public object DefValue
        {
            get { return null; }
        }

        public ICLS_TypeFunction function
        {
            get;
            protected set;
        }

        public ICLS_Value MakeValue(object value)
        {
            throw new NotImplementedException();
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
            if (code == logictoken.equal)
            {
                return left == right.value;
            }
            else if (code == logictoken.not_equal)
            {
                return left != right.value;
            }
            throw new NotImplementedException();
        }
    }
}
