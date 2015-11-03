using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public enum logictoken
    {
        less,           //<
        less_equal,     //<=
        more,           //>
        more_equal,     //>=
        equal,          //==
        not_equal       //!=

    }

    public interface ICLS_TypeFunction
    {
        CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params);
        CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params);
        CLS_Content.Value StaticValueGet(CLS_Content content, string valuename);
        void StaticValueSet(CLS_Content content, string valuename, object value);
        CLS_Content.Value MemberCall(CLS_Content content, object object_this, string func, BetterList<CLS_Content.Value> _params, bool isBaseCall = false);
        CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false);
        void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false);
        CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key);
        void IndexSet(CLS_Content content, object object_this, object key, object value);
    }

    public class CLType
    {
        static Dictionary<Type, CLType> dictTypeSystem = new Dictionary<Type, CLType>();
        static Dictionary<SType, CLType> dictTypeScript = new Dictionary<SType, CLType>();

        Type typeSystem;
        SType typeScript;

        public string Name
        {
            get
            {
                if (typeSystem != null)
                    return typeSystem.Name;
                else 
                    return typeScript.Name;
            }
        }

        private CLType(Type typeSystem)
        {
            this.typeSystem = typeSystem;
        }

        private CLType(SType typeScript)
        {
            this.typeScript = typeScript;
        }

        public static implicit operator Type(CLType ct)
        {
            return ct.typeSystem;
        }

        public static implicit operator SType(CLType ct)
        {
            if (ct == null)
                return null;
            return ct.typeScript;
        }

        public static implicit operator CLType(Type typeSystem)
        {
            CLType ct;
            if (dictTypeSystem.TryGetValue(typeSystem, out ct))
                return ct;
            ct = new CLType(typeSystem);
            dictTypeSystem[typeSystem] = ct;
            return ct;
        }

        public static implicit operator CLType(SType typeScript)
        {
            CLType ct;
            if (dictTypeScript.TryGetValue(typeScript, out ct))
                return ct;
            ct = new CLType(typeScript);
            dictTypeScript[typeScript] = ct;
            return ct;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface ICLS_Type
    {
        string keyword
        {
            get;
        }

        CLType type
        {
            get;
        }

        object DefValue
        {
            get;
        }
            
        ICLS_Value MakeValue(object value);

        // 自动转型能力
        object ConvertTo(CLS_Content content, object src, CLType targetType);

        // 数学计算能力
        object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returnType);

        // 逻辑计算能力
        bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right);

        ICLS_TypeFunction function
        {
            get;
        }
    }

    public interface ICLS_Type_Dele : ICLS_Type
    {
        Delegate CreateDelegate(ICLS_Environment env, DeleFunction lambda);

        Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda);
    }
}
