﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{

    public class CLS_Expression_IndexSetValue : ICLS_Expression
    {
        public CLS_Expression_IndexSetValue(int tbegin, int tend, int lbegin, int lend)
        {
           listParam= new List<ICLS_Expression>();
           this.tokenBegin = tbegin;
           this.tokenEnd = tend;
           lineBegin = lbegin;
           lineEnd = lend;
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
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            var parent = listParam[0].ComputeValue(content);
            var key = listParam[1].ComputeValue(content);
            var value = listParam[2].ComputeValue(content);
            var type = content.environment.GetType(parent.type);
            type.function.IndexSet(content, parent.value, key.value, value.value);
            content.OutStack(this);
            return null;
        }

        public override string ToString()
        {
            return "IndexSet[]=|";
        }
    }
}
