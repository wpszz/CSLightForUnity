using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    // 将SInstance的调用指向脚本类
    public class RegHelper_SInstance : RegHelper_Type
    {
        public RegHelper_SInstance()
        {
            this.type = typeof(SInstance);
            this.sysType = this.type;
            this.keyword = "CSLE.SInstance";
            this.function = new RegHelper_SInstanceFunction(this.type);
        }

        public override ICLS_Value MakeValue(object value)
        {
            throw new NotImplementedException("CSLE.SInstance.MakeValue");
        }

        public override object ConvertTo(CLS_Content env, object src, CLType targetType)
        {
            return src;
        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returnType)
        {
            throw new NotImplementedException("CSLE.SInstance.Math2Value");
        }

        public override bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            throw new NotImplementedException("CSLE.SInstance.MathLogic");
        }

        public class RegHelper_SInstanceFunction : RegHelper_TypeFunction
        {
            public RegHelper_SInstanceFunction(Type type)
                : base(type)
            {
            }

            public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
            {
                throw new NotImplementedException("CSLE.SInstance.New");
            }

            public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
            {
                throw new NotImplementedException("CSLE.SInstance.StaticCall");
            }

            public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
            {
                throw new NotImplementedException("CSLE.SInstance.StaticValueGet");
            }

            public override void StaticValueSet(CLS_Content content, string valuename, object value)
            {
                throw new NotImplementedException("CSLE.SInstance.StaticValueSet");
            }

            public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
            {
                return ((SInstance)object_this).type.MemberCall(content, object_this, function, _params, isBaseCall);
            }

            public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
            {
                return ((SInstance)object_this).type.MemberValueGet(content, object_this, valuename, isBaseCall);
            }

            public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
            {
                ((SInstance)object_this).type.MemberValueSet(content, object_this, valuename, value, isBaseCall);
            }

            public override CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
            {
                throw new NotImplementedException("CSLE.SInstance.IndexGet");
            }

            public override void IndexSet(CLS_Content content, object object_this, object key, object value)
            {
                throw new NotImplementedException("CSLE.SInstance.IndexSet");
            }
        }
    }

}
