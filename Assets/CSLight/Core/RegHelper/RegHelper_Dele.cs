using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_Dele : RegHelper_Type, ICLS_Type_Dele
    {
        public RegHelper_Dele(Type type, string setkeyword)
            : base(type, setkeyword)
        {

        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = null;

            Delegate rightDele = null;
            if (right.value is DeleFunction)
                rightDele = CreateDelegate(env.environment, right.value as DeleFunction);
            else if (right.value is DeleLambda)
                rightDele = CreateDelegate(env.environment, right.value as DeleLambda);
            else if (right.value is Delegate)
                rightDele = right.value as Delegate;

            if (rightDele != null)
            {
                Delegate leftDele = left as Delegate;
                if (left == null)
                    return rightDele;

                if (code == '+')
                    return Delegate.Combine(leftDele, rightDele);
                if (code == '-')
                    return Delegate.Remove(leftDele, rightDele);
            }

            throw new NotSupportedException("" + right.value);
        }

        public virtual Delegate CreateDelegate(ICLS_Environment env, DeleFunction delefunc)
        {
            throw new Exception("请重载实现功能");
        }

        public virtual Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            throw new Exception("请重载实现功能");
        }
    }
}
