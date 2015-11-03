using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_MemberFind : ICLS_Expression
    {
        public CLS_Expression_MemberFind(int tbegin, int tend, int lbegin, int lend)
        {
           listParam= new List<ICLS_Expression>();
           this.tokenBegin = tbegin;
           this.tokenEnd = tend;
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
            CLS_Content.Value parent = listParam[0].ComputeValue(content);
            ICLS_Type type = content.environment.GetType(parent.type);
            CLS_Content.Value value = type.function.MemberValueGet(content, parent.value, membername, parent.breakBlock == 255);
            content.OutStack(this);
            return value;
        }
   
        public string membername;
   
        public override string ToString()
        {
            return "MemberFind: " + membername;
        }
    }
}