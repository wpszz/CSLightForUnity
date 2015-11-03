using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_SelfOpWithValue : ICLS_Expression
    {
        public CLS_Expression_SelfOpWithValue(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            private set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            private set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);

            ICLS_Expression leftExp = listParam[0];

            CLS_Content.Value left = leftExp.ComputeValue(content);
            CLS_Content.Value right = listParam[1].ComputeValue(content);
            ICLS_Type type = content.environment.GetType(left.type);

            CLType returntype;
            left.value = type.Math2Value(content, mathop, left.value, right, out returntype);
            //left.value = type.ConvertTo(content, left.value, left.type);

            if (leftExp is CLS_Expression_MemberFind)
            {
                CLS_Expression_MemberFind f = leftExp as CLS_Expression_MemberFind;

                CLS_Content.Value parent = f.listParam[0].ComputeValue(content);
                ICLS_Type ptype = content.environment.GetType(parent.type);
                ptype.function.MemberValueSet(content, parent.value, f.membername, left.value);
            }
            else if (leftExp is CLS_Expression_StaticFind)
            {
                CLS_Expression_StaticFind f = leftExp as CLS_Expression_StaticFind;
                f.type.function.StaticValueSet(content, f.staticmembername, left.value);
            }
            else if (leftExp is CLS_Expression_IndexFind)
            {
                CLS_Expression_IndexFind f = leftExp as CLS_Expression_IndexFind;

                CLS_Content.Value parent = f.listParam[0].ComputeValue(content);
                CLS_Content.Value key = f.listParam[1].ComputeValue(content);
                ICLS_Type ptype = content.environment.GetType(parent.type);
                ptype.function.IndexSet(content, parent.value, key.value, left.value);
            }

            content.OutStack(this);

            return null;
        }

        //public string value_name;
        public char mathop;

        public override string ToString()
        {
            return "a " + mathop + "= b";
        }
    }
}