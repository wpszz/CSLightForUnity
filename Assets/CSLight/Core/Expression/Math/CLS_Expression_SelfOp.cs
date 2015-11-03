using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_SelfOp : ICLS_Expression
    {
        public CLS_Expression_SelfOp(int tbegin, int tend, int lbegin, int lend)
        {
            tokenBegin = tbegin;
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
            get
            {
                return null;
            }
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

            CLS_Content.Value left = content.Get(value_name);
            ICLS_Type type = content.environment.GetType(left.type);
            CLType returntype;
            left.value = type.Math2Value(content, mathop, left.value, CLS_Content.Value.One, out returntype);
            //left.value = type.ConvertTo(content, left.value, v.type);
            //content.Set(value_name, left.value);

            content.OutStack(this);

            return content.Get(value_name);
        }

        public string value_name;
        public char mathop;
  
        public override string ToString()
        {
            return value_name + mathop + mathop;
        }
    }
}