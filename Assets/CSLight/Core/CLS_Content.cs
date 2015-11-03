using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class CLS_Content
    {
        public class Value
        {
            public CLType type;
            public object value;

            /* 块标志
             * 1:continue 
             * 2:break 
             * 3:return 
             * 12:yield break  
             * 13:yield return xxx  
             * 255:base.function/member
            */
            public byte breakBlock = 0;

            public static Value One = new Value() { type = typeof(int), value = (int)1 };
            public static Value OneMinus = new Value() { type = typeof(int), value = (int)-1 };
            public static Value Null = new Value() { type = null, value = null };
            public static Value Void = new Value() { type = typeof(void), value = null };
            public static Value Break = new Value() { breakBlock = 2 };
            public static Value Continue = new Value() { breakBlock = 1 };
            public static Value True = new Value() { type = typeof(bool), value = true };
            public static Value False = new Value() { type = typeof(bool), value = false };

            public override string ToString()
            {
                return "<" + type + ">" + value;
            }

            // 修正值的类型为type
            public void FixValueType(CLS_Content content)
            {
                if (value != null)
                {
                    Type sysT = type;
                    if (sysT != null)
                    {
                        Type vT = value.GetType();
                        if (sysT != vT)
                        {
                            value = content.environment.GetType(vT).ConvertTo(content, value, type);
                        }
                    }
                }
            }
        }

        public ICLS_Environment environment;

        public SType CallType;

        public SInstance CallThis;

        public string CallFileName
        {
            get
            {
                if (this.CallType != null)
                {
                    if (!string.IsNullOrEmpty(this.CallType.filename))
                        return "(" + this.CallType.filename + ")";
                    else
                        return "(" + this.CallType.Name + ")";
                }
                return "(unknown file)";
            }
        }

        public BetterList<CLS_Content> stackContent = new BetterList<CLS_Content>();

        public BetterList<ICLS_Expression> stackExpr = new BetterList<ICLS_Expression>();

        public BetterList<BetterList<string>> stackTempValueName = new BetterList<BetterList<string>>();

        public Dictionary<string, Value> dictValues = new Dictionary<string, Value>();

        public ICLS_Expression CallExpression
        {
            get
            {
                return stackExpr[stackExpr.size - 1];
            }
        }

        protected static BetterList<BetterList<string>> s_tempValueNamePool = new BetterList<BetterList<string>>();
        protected static BetterList<CLS_Content> s_contentPool = new BetterList<CLS_Content>();
        public static int FreeContentCount { get { return s_contentPool.size; } }
        public static CLS_Content NewContent(ICLS_Environment environment)
        {
            if (s_contentPool.size > 0)
            {
                CLS_Content content = s_contentPool.Pop();
                content.environment = environment;
                return content;
            }
            else
            {
                CLS_Content content = new CLS_Content();
                content.environment = environment;
                return content;
            }
        }

        public static void PoolContent(CLS_Content content)
        {
            content.CallType = null;
            content.CallThis = null;
            content.stackContent.Clear();
            content.stackExpr.Clear();
            content.stackTempValueName.Clear();
            content.dictValues.Clear();
            s_contentPool.Add(content);
        }

        protected static BetterList<BetterList<CLS_Content.Value>> s_paramListPool = new BetterList<BetterList<CLS_Content.Value>>();
        public static int FreeParamListCount { get { return s_paramListPool.size; } }
        public static BetterList<CLS_Content.Value> NewParamList()
        {
            if (s_paramListPool.size > 0)
            {
                return s_paramListPool.Pop();
            }
            else
            {
                return new BetterList<CLS_Content.Value>();
            }
        }

        public static void PoolParamList(BetterList<CLS_Content.Value> paramList)
        {
            paramList.Clear();
            s_paramListPool.Add(paramList);
        }

        public static Type[][] ParamTypesArray = new Type[][]
        {
            new Type[0], new Type[1], new Type[2], new Type[3], new Type[4], new Type[5], new Type[6], new Type[7], new Type[8], new Type[9], new Type[10],
        };
        public static object[][] ParamObjsArray = new object[][]
        {
            new object[0], new object[1], new object[2], new object[3], new object[4], new object[5], new object[6], new object[7], new object[8], new object[9], new object[10],
        };

        // 禁止外部创建，统一使用CLS_Content.NewContent
        private CLS_Content(){}

        public CLS_Content Clone()
        {
            CLS_Content content = CLS_Content.NewContent(environment);
            content.CallThis = this.CallThis;
            content.CallType = this.CallType;
            foreach (var pair in this.dictValues)
            {
                content.dictValues.Add(pair.Key, pair.Value);
            }
            return content;
        }

        public override string ToString()
        {
            return DumpStack(null);
        }

        public void InStackContent(CLS_Content content)
        {
            stackContent.Add(content);
        }

        public void OutStackContent(CLS_Content content)
        {
            stackContent.Pop();
        }

        public void InStack(ICLS_Expression expr)
        {
            stackExpr.Add(expr);
        }

        public void OutStack(ICLS_Expression expr)
        {
            stackExpr.Pop();
        }

		public string DumpValue()
		{
			string svalues = "";
            foreach (var subc in this.stackContent)
            {
                svalues += subc.DumpValue();
            }
            svalues += "DumpValue:" + this + "\n";
            foreach(var pair in this.dictValues)
            {
                svalues += "V:" + pair.Key + "=" + pair.Value.ToString()+"\n";
            }
			return svalues;
		}

		public string DumpStack(IList<Token> tokenlist = null)
        {
			string dumpInfo = "";

            if (this.CallType != null && this.CallType.tokenlist != null)
            {
                tokenlist = this.CallType.tokenlist;
            }

            foreach (var subc in this.stackContent)
            {
                dumpInfo += subc.DumpStack(tokenlist);
            }

            if (tokenlist == null)
                return dumpInfo;

            dumpInfo += "CSLightDumpStack:" + this.CallFileName;

            if (stackExpr.size == 0)
                return dumpInfo + "\n";

            ICLS_Expression exp = CallExpression;

            if (exp != null)
            {
                dumpInfo += " [line=" + exp.lineBegin + "] ";

                int tlCount = tokenlist != null ? tokenlist.Count : 0;
                for (int i = exp.tokenBegin; i <= exp.tokenEnd; i++)
                {
                    if (i >= tlCount)
                        break;
                    dumpInfo += tokenlist[i].text;
                }
            }

            dumpInfo += "\n";

            return dumpInfo;
        }

		public string Dump(IList<Token> tokenlist=null)
		{
			string str = DumpValue();
			str += DumpStack(tokenlist);
			return str;
		}

        public void Define(string name,CLType type)
        {
            Value val = new Value();
            val.type = type;
            dictValues[name] = val;

            if (stackTempValueName.size > 0)
                stackTempValueName[stackTempValueName.size - 1].Add(name);//暂存临时变量
        }

        public void DefineAndSet(string name, CLType type, object value)
        {
            Value val = new Value();
            val.type = type;
            val.value = value;
            val.FixValueType(this);
            dictValues[name] = val;

            if (stackTempValueName.size > 0)
                stackTempValueName[stackTempValueName.size - 1].Add(name);//暂存临时变量
        }

        public void Set(string name, object value)
        {
            // 优先上下文变量
            Value val;
            if (dictValues.TryGetValue(name, out val))
            {
                val.value = value;
                val.FixValueType(this);
                return;
            }

            // 成员查找
            if (CallThis != null && CallThis.type.TryMemberValueSet(this, CallThis, name, value))
                return;

            // 静态成员查找
            if (CallType != null && CallType.TryStaticValueSet(this, name, value))
                return;
        }

        public Value Get(string name)
        {
            // 优先上下文查找
            Value val;
            if (dictValues.TryGetValue(name, out val))
                return val;

            // 成员查找
            if (CallThis != null && CallThis.type.TryMemberValueGet(this, CallThis, name, out val))
                return val;

            // 静态成员查找
            if (CallType != null && CallType.TryStaticValueGet(this, name, out val))
                return val;

            return null;
        }

        // 控制临时变量作用域，进栈
        public void DepthAdd()
        {
            if (s_tempValueNamePool.size > 0)
                stackTempValueName.Add(s_tempValueNamePool.Pop());
            else
                stackTempValueName.Add(new BetterList<string>());
        }

        // 控制临时变量作用域，出栈
        public void DepthRemove()
        {
            BetterList<string> topValueNames = stackTempValueName.Pop();
            for (int i = 0; i < topValueNames.size; i++)
            {
                dictValues.Remove(topValueNames[i]);
            }
            topValueNames.Clear();
            s_tempValueNamePool.Add(topValueNames);
        }
           
    }
}
