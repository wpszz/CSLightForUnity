/// C#Light/Evil
/// 原作者 疯光无限 版本见ICLS_Environment.version
/// https://github.com/lightszero/CSLightStudio
/// http://crazylights.cnblogs.com
/// 
/// 扩展版作者 身自在
/// https://github.com/wpszz/CSLightForUnity
/// 请勿删除此声明
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CSLE
{
    public class CLS_Environment : ICLS_Environment
    {
        Dictionary<CLType, ICLS_Type> dictTypes = new Dictionary<CLType, ICLS_Type>();
        Dictionary<string, ICLS_Type> dictTypeKeywords = new Dictionary<string, ICLS_Type>();
        Dictionary<string, ICLS_Function> dictFuns = new Dictionary<string, ICLS_Function>();

        public string version
        {
            get
            {
                return "1.0.1";
            }
        }

        public ICLS_Logger logger
        {
            get;
            private set;
        }

        ICLS_TokenParser tokenParser = null;

        ICLS_Expression_Compiler compiler = null;

        public CLS_Environment(ICLS_Logger logger)
        {
            this.logger = logger;
            this.tokenParser = new CLS_TokenParser(this.dictTypeKeywords);
            this.compiler = new CLS_Expression_Compiler(logger);

            RegType(new CLS_Type_Void());
            RegType(new CLS_Type_Null());
            RegType(new CLS_Type_Int());
            RegType(new CLS_Type_UInt());
            RegType(new CLS_Type_Float());
            RegType(new CLS_Type_Double());
            RegType(new CLS_Type_String());
            RegType(new CLS_Type_Bool());
            RegType(new CLS_Type_Lambda());
            RegType(new CLS_Type_Delegate());
            RegType(new CLS_Type_Byte());
            RegType(new CLS_Type_Char());
            RegType(new CLS_Type_UShort());
            RegType(new CLS_Type_Sbyte());
            RegType(new CLS_Type_Short());
            RegType(new CLS_Type_Long());
            RegType(new CLS_Type_ULong());

            RegType(new RegHelper_Type(typeof(object), "object"));
            RegType(new RegHelper_Type(typeof(List<>), "List"));
            RegType(new RegHelper_Type(typeof(Dictionary<,>), "Dictionary"));
            RegType(new RegHelper_Type(typeof(KeyValuePair<,>), "KeyValuePair"));
            RegType(new RegHelper_SInstance());

            // Unity 协程注册
            RegType(new RegHelper_Type(typeof(IEnumerator), "IEnumerator"));
            RegType(new RegHelper_Type(typeof(YieldInstruction), "YieldInstruction"));
            RegType(new RegHelper_Type(typeof(Coroutine), "Coroutine"));
            RegType(new RegHelper_Type(typeof(WaitForSeconds), "WaitForSeconds"));
            RegType(new RegHelper_Type(typeof(WaitForEndOfFrame), "WaitForEndOfFrame"));
            RegType(new RegHelper_Type(typeof(WaitForFixedUpdate), "WaitForFixedUpdate"));

            RegType(new CLS_Type_Dict<int, object>("Dictionary<int, object>"));
            RegType(new CLS_Type_KeyValuePair<int, object>("KeyValuePair<int, object>"));

            RegType(new CLS_Type_Dict<int, SInstance>("Dictionary<int, CSLE.SInstance>"));            // IOS防止JIT
            RegType(new CLS_Type_KeyValuePair<int, SInstance>("KeyValuePair<int, CSLE.SInstance>"));  // IOS防止JIT

            RegType(new CLS_Type_Dict<string, object>("Dictionary<string, object>"));
            RegType(new CLS_Type_KeyValuePair<string, object>("KeyValuePair<string, object>"));

            RegType(new CLS_Type_Dict<string, SInstance>("Dictionary<string, CSLE.SInstance>"));            // IOS防止JIT
            RegType(new CLS_Type_KeyValuePair<string, SInstance>("KeyValuePair<string, CSLE.SInstance>"));  // IOS防止JIT

            // 注册基础数组
            RegType(new CLS_Type_Array<int>("int[]"));
            RegType(new CLS_Type_Array<uint>("uint[]"));
            RegType(new CLS_Type_Array<float>("float[]"));
            RegType(new CLS_Type_Array<double>("double[]"));
            RegType(new CLS_Type_Array<string>("string[]"));
            RegType(new CLS_Type_Array<bool>("bool[]"));
            RegType(new CLS_Type_Array<Delegate>("Delegate[]"));
            RegType(new CLS_Type_Array<byte>("byte[]"));
            RegType(new CLS_Type_Array<char>("char[]"));
            RegType(new CLS_Type_Array<ushort>("ushort[]"));
            RegType(new CLS_Type_Array<sbyte>("sbyte[]"));
            RegType(new CLS_Type_Array<short>("short[]"));
            RegType(new CLS_Type_Array<long>("long[]"));
            RegType(new CLS_Type_Array<ulong>("ulong[]"));
            RegType(new CLS_Type_Array<object>("object[]"));
            RegType(new CLS_Type_Array<SInstance>("CSLE.SInstance[]"));         // IOS防止JIT

            // 注册基础列表
            RegType(new CLS_Type_List<int>("List<int>"));
            RegType(new CLS_Type_List<uint>("List<uint>"));
            RegType(new CLS_Type_List<float>("List<float>"));
            RegType(new CLS_Type_List<double>("List<double>"));
            RegType(new CLS_Type_List<string>("List<string>"));
            RegType(new CLS_Type_List<bool>("List<bool>"));
            RegType(new CLS_Type_List<Delegate>("List<Delegate>"));
            RegType(new CLS_Type_List<byte>("List<byte>"));
            RegType(new CLS_Type_List<char>("List<char>"));
            RegType(new CLS_Type_List<ushort>("List<ushort>"));
            RegType(new CLS_Type_List<sbyte>("List<sbyte>"));
            RegType(new CLS_Type_List<short>("List<short>"));
            RegType(new CLS_Type_List<long>("List<long>"));
            RegType(new CLS_Type_List<ulong>("List<ulong>"));
            RegType(new CLS_Type_List<object>("List<object>"));
            RegType(new CLS_Type_List<SInstance>("List<CSLE.SInstance>"));      // IOS防止JIT

            RegFunction(new FunctionTrace());
            RegFunction(new FunctionTypeof());
        }

        public void RegType(ICLS_Type regType)
        {
            if (regType.type != null)
            {
                if (dictTypes.ContainsKey(regType.type))
                {
                    // 允许多个关键字映射相同注册类型
                    //logger.Log_Warn("RegType repeat: type=" + regType.type);
                }
                else
                {
                    dictTypes.Add(regType.type, regType);
                }
            }

            if (!string.IsNullOrEmpty(regType.keyword))
            {
                if (dictTypeKeywords.ContainsKey(regType.keyword))
                {
                    logger.Log_Warn("RegType repeat: type key=" + regType.keyword);
                }
                else
                {
                    dictTypeKeywords.Add(regType.keyword, regType);
                }
            }
        }

        public ICLS_Type GetType(CLType type)
        {
            if (type == null)
                return dictTypeKeywords["null"];

            ICLS_Type iType;
            if (dictTypes.TryGetValue(type, out iType))
                return iType;

            logger.Log_Warn("类型未注册, 这里将自动注册一份匿名:" + type.ToString());
            RegType(new RegHelper_Type(type, ""));

            return dictTypes[type];
        }

        public ICLS_Type GetTypeByKeyword(string keyword)
        {
            ICLS_Type iType;
            if (dictTypeKeywords.TryGetValue(keyword, out iType))
                return iType;

            int len = keyword.Length;
            if (keyword[len - 1] == '>')
            {
                int iis = keyword.IndexOf('<');
                string typeName = keyword.Substring(0, iis);
                List<string> _types = new List<string>();
                int istart = iis + 1;
                int inow = istart;
                int dep = 0;
                while (inow < len)
                {
                    if (keyword[inow] == '<')
                    {
                        dep++;
                    }
                    if (keyword[inow] == '>')
                    {
                        dep--;
                        if (dep < 0)
                        {
                            _types.Add(keyword.Substring(istart, inow - istart));
                            break;
                        }
                    }

                    if (keyword[inow] == ',' && dep == 0)
                    {
                        _types.Add(keyword.Substring(istart, inow - istart));
                        istart = inow + 1;
                        inow = istart;
                        continue;
                    }

                    inow++;
                }

                iType = GetTypeByKeyword(typeName);
                if (iType != null)
                {
                    Type genericType = iType.type;
                    if (genericType != null && genericType.IsGenericTypeDefinition)
                    {
                        Type[] types = new Type[_types.Count];
                        for (int i = 0; i < types.Length; i++)
                        {
                            iType = GetTypeByKeyword(_types[i]);

                            Type rt = iType.type;
                            if (rt != null)
                                types[i] = rt;
                            else
                                types[i] = typeof(SInstance);
                        }
                        iType = new RegHelper_Type(genericType.MakeGenericType(types), keyword);
                        RegType(iType);
                        return iType;
                    }
                }
            }
            else if (keyword[len - 1] == ']')
            {
                string typeName = keyword.Substring(0, keyword.IndexOf('['));
                iType = GetTypeByKeyword(typeName);
                if (iType != null)
                {
                    Type rt = iType.type;
                    if (rt == null)
                        rt = typeof(SInstance);
                    iType = new RegHelper_Type(rt.MakeArrayType(), keyword);
                    RegType(iType);
                    return iType;
                }
            }

            logger.Log_Error("类型未注册: " + keyword);
            return null;
        }

        public ICLS_Type GetTypeByKeywordQuiet(string keyword)
        {
            ICLS_Type iType;
            if (dictTypeKeywords.TryGetValue(keyword, out iType))
                return iType;
            return null;
        }

        public void RegFunction(ICLS_Function func)
        {
            dictFuns[func.keyword] = func;
        }

        public ICLS_Function GetFunction(string name)
        {
            ICLS_Function func;
            if (dictFuns.TryGetValue(name, out func))
                return func;
            return null;
        }

        public ICLS_Expression Expr_CompilerToken(IList<Token> listToken, bool SimpleExpression = false)
        {
            return SimpleExpression ? compiler.Compiler_NoBlock(listToken, this) : compiler.Compiler(listToken, this);
        }

        public ICLS_Expression Expr_Optimize(ICLS_Expression old)
        {
            return compiler.Optimize(old, this);
        }

        public CLS_Content CreateContent()
        {
            return CLS_Content.NewContent(this);
        }

        public CLS_Content.Value Expr_Execute(ICLS_Expression expr, CLS_Content content = null)
        {
            if (content == null) 
                content = CreateContent();
            return expr.ComputeValue(content);
        }

        public IList<Token> ParserToken(string code)
        {
            return tokenParser.Parse(code);
        }

        public Dictionary<string, IList<Token>> Project_ParserToken(Dictionary<string, string> projectCodes)
        {
            Dictionary<string, IList<Token>> projectTokens = new Dictionary<string, IList<Token>>();

            // 预注册脚本类型
            foreach (var pair in projectCodes)
            {
                string className = System.IO.Path.GetFileNameWithoutExtension(pair.Key);
                this.RegType(new CLS_Type_Class(className, pair.Key));
            }

            // 解析代码
            foreach (var pair in projectCodes)
            {
                projectTokens[pair.Key] = ParserToken(pair.Value);
            }

            return projectTokens;
        }

        public void CompilerToken(string fileName, IList<Token> tokens, bool embDebugToken)
        {
            compiler.FileCompiler(this, fileName, tokens, embDebugToken);
        }

        public void Project_CompilerToken(Dictionary<string, IList<Token>> projectTokens, bool embDebugToken)
        {
            // 预注册脚本类型
            foreach (var pair in projectTokens)
            {
                string className = System.IO.Path.GetFileNameWithoutExtension(pair.Key);
                this.RegType(new CLS_Type_Class(className, pair.Key));
            }

            // 编译token
            foreach (var pair in projectTokens)
            {
                compiler.FileCompiler(this, pair.Key, pair.Value, embDebugToken);
            }
        }

        public void TokenToStream(IList<Token> tokens, System.IO.Stream outstream)
        {
            this.tokenParser.SaveTokenList(tokens, outstream);
        }

        public IList<Token> StreamToToken(System.IO.Stream instream)
        {
            return this.tokenParser.ReadTokenList(instream);
        }

        public void Project_PacketToStream(Dictionary<string, IList<Token>> project, System.IO.Stream outstream)
        {
            byte[] FileHead = System.Text.Encoding.UTF8.GetBytes("C#LE-DLL");
            outstream.Write(FileHead, 0, 8);
            UInt16 count = (UInt16)project.Count;
            outstream.Write(BitConverter.GetBytes(count), 0, 2);
            foreach (var pair in project)
            {
                byte[] pname = System.Text.Encoding.UTF8.GetBytes(pair.Key);
                outstream.WriteByte((byte)pname.Length);
                outstream.Write(pname, 0, pname.Length);
                this.tokenParser.SaveTokenList(pair.Value, outstream);
            }
        }

        public Dictionary<string, IList<Token>> Project_FromPacketStream(System.IO.Stream instream)
        {
            Dictionary<string, IList<Token>> project = new Dictionary<string, IList<Token>>();
            byte[] buf = new byte[8];
            instream.Read(buf, 0, 8);
            string filehead = System.Text.Encoding.UTF8.GetString(buf, 0, 8);
            if (filehead != "C#LE-DLL") return null;
            instream.Read(buf, 0, 2);
            UInt16 count = BitConverter.ToUInt16(buf, 0);
            for (int i = 0; i < count; i++)
            {
                int slen = instream.ReadByte();
                byte[] buffilename = new byte[slen];
                instream.Read(buffilename, 0, slen);
                string key = System.Text.Encoding.UTF8.GetString(buffilename, 0, slen);
                var tlist = tokenParser.ReadTokenList(instream);
                project[key] = tlist;
            }
            return project;
        }
    }
}
