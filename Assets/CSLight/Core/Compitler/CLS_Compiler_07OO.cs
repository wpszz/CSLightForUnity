using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        string m_curFileName;

        IList<ICLS_Type> _FileCompiler(string filename, IList<Token> tokens, bool embDeubgToken, ICLS_Environment env, bool onlyGotType = false)
        {
            m_curFileName = filename;

            List<ICLS_Type> typelist = new List<ICLS_Type>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ";")
                    continue;

                if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "class")
                {
                    string className = tokens[i + 1].text;

                    // 在这里检查继承
                    List<string> baseClassNames = null;
                    int ibegin = i + 2;
                    if (onlyGotType)
                    {
                        while (tokens[ibegin].text != "{")
                        {
                            ibegin++;
                        }
                    }
                    else
                    {
                        if (tokens[ibegin].text == ":")
                        {
                            baseClassNames = new List<string>();
                            ibegin++;
                        }
                        while (tokens[ibegin].text != "{")
                        {
                            if (tokens[ibegin].type == TokenType.TYPE)
                            {
                                baseClassNames.Add(tokens[ibegin].text);
                            }
                            ibegin++;
                        }
                    }

                    int iend = FindBlock(env, tokens, ibegin);
                    if (iend == -1)
                    {
                        env.logger.Log_Error("查找文件尾失败。");
                        return null;
                    }
                    
                    if (onlyGotType)
                    {
                        env.logger.Log("(scriptPreParser)findclass:" + className + "(" + ibegin + "," + iend + ")");

                    }
                    else
                    {
                        env.logger.Log("(scriptParser)findclass:" + className + "(" + ibegin + "," + iend + ")");
                    }

                    ICLS_Type type = Compiler_Class(env, className, baseClassNames, filename, tokens, ibegin, iend, embDeubgToken, onlyGotType, null);
                    if (type != null)
                    {
                        typelist.Add(type);
                    }

                    i = iend;
                    continue;
                }
            }

            return typelist;
        }

        ICLS_Type Compiler_Class(ICLS_Environment env, string classname, IList<string> baseTypeNames, string filename, IList<Token> tokens, int ibegin, int iend, bool EmbDebugToken, bool onlyGotType = false, IList<string> usinglist = null)
        {
            CLS_Type_Class sClass = env.GetTypeByKeywordQuiet(classname) as CLS_Type_Class;

            if (sClass == null)
                sClass = new CLS_Type_Class(classname, filename);

            sClass.compiled = false;
            (sClass.function as SType).functions.Clear();
            (sClass.function as SType).members.Clear();

            if (onlyGotType)
                return sClass;

            // 调试Token
            if (EmbDebugToken)
                (sClass.function as SType).EmbDebugToken(tokens);

            bool bStatic = false;
            int sortIndex = 0;

            for (int i = ibegin; i <= iend; i++)
            {
                if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "static")
                {
                    bStatic = true;
                    continue;
                }

                if (tokens[i].type == TokenType.ProtoIndex)
                {
                    sortIndex = int.Parse(tokens[i].text);
                    continue;
                }

                if (tokens[i].type == TokenType.TYPE || (tokens[i].type == TokenType.IDENTIFIER && tokens[i].text == classname))//发现类型
                {
                    ICLS_Type idtype = env.GetTypeByKeyword("null");
                    bool bctor = false;
                    if (tokens[i].type == TokenType.TYPE)
                    {
                        if (tokens[i].text == classname && tokens[i + 1].text == "(")
                        {
                            //构造函数
                            bctor = true;
                            i--;
                        }
                        else if (tokens[i + 1].text == "[" && tokens[i + 2].text == "]")
                        {
                            idtype = env.GetTypeByKeyword(tokens[i].text + "[]");
                            i += 2;
                        }
                        else if (tokens[i].text == "void")
                        {

                        }
                        else
                        {
                            idtype = env.GetTypeByKeyword(tokens[i].text);
                        }
                    }

                    if (tokens[i + 1].type == CSLE.TokenType.IDENTIFIER || bctor) //类型后面是名称
                    {
                        string idname = tokens[i + 1].text;
                        if (tokens[i + 2].type == CSLE.TokenType.PUNCTUATION && tokens[i + 2].text == "(")//参数开始,这是函数
                        {
                            logger.Log("发现函数:" + idname);
                            SType.Function func = new SType.Function();
                            func.bStatic = bStatic;

                            int funcparambegin = i + 2;
                            int funcparamend = FindBlock(env, tokens, funcparambegin);
                            if (funcparamend - funcparambegin > 1)
                            {
                                int start = funcparambegin + 1;
                                for (int j = funcparambegin + 1; j <= funcparamend; j++)
                                {
                                    if (tokens[j].text == "," || tokens[j].text == ")")
                                    {
                                        string ptype = "";
                                        for (int k = start; k <= j - 2; k++)
                                            ptype += tokens[k].text;
                                        var pid = tokens[j - 1].text;
                                        var type = env.GetTypeByKeyword(ptype);

                                        if (type == null)
                                        {
                                            throw new Exception(filename + ":不可识别的函数头参数:" + tokens[funcparambegin].ToString());
                                        }

                                        func._paramnames.Add(pid);
                                        func._paramtypes.Add(type);
                                        start = j + 1;
                                    }
                                }
                            }

                            int funcbegin = funcparamend + 1;
                            if (tokens[funcbegin].text == "{")
                            {
                                bool coroutine = tokens[i].text == "IEnumerator";

                                int funcend = FindBlock(env, tokens, funcbegin);
                                this.Compiler_Expression_Block(tokens, env, funcbegin, funcend, out func.expr_runtime);

                                // Unity协程判断
                                if (coroutine)
                                {
                                    int yieldCount = 0;
                                    if (func.expr_runtime is CLS_Expression_Yield)
                                    {
                                        yieldCount++;
                                    }
                                    else
                                    {
                                        foreach (var v1 in func.expr_runtime.listParam)
                                        {
                                            if (v1 is CLS_Expression_Yield)
                                            {
                                                yieldCount++;
                                            }
                                            else if (v1 is CLS_Expression_LoopIf)
                                            {
                                                if (v1.listParam[1] is CLS_Expression_Yield)
                                                {
                                                    yieldCount++;
                                                }
                                                else if (v1.listParam[1] is CLS_Expression_Block)
                                                {
                                                    if (v1.listParam[1].listParam[v1.listParam[1].listParam.Count - 1] is CLS_Expression_Yield)
                                                    {
                                                        yieldCount++;
                                                    }
                                                }
                                            }
                                            else if (v1 is CLS_Expression_LoopFor)
                                            {
                                                if (v1.listParam[3] is CLS_Expression_Yield)
                                                {
                                                    yieldCount++;
                                                }
                                                else if (v1.listParam[3] is CLS_Expression_Block)
                                                {
                                                    foreach (var v2 in v1.listParam[3].listParam)
                                                    {
                                                        if (v2 is CLS_Expression_Yield)
                                                        {
                                                            yieldCount++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    // 暂不支持复杂嵌套的yield表达式
                                    if (yieldCount != m_tempYieldCount)
                                    {
                                        throw new Exception(filename + "暂不支持复杂嵌套的yield表达式: " + tokens[funcbegin].ToString());
                                    }
                                    var expr = func.expr_runtime;
                                    func.expr_runtime = new CLS_Expression_Coroutine(expr.listParam, expr.tokenBegin, expr.tokenEnd, expr.lineBegin, expr.lineEnd);
                                }
                                m_tempYieldCount = 0;

                                // 判断是否是派生类的构造函数
                                if (baseTypeNames != null && baseTypeNames.Count > 0 && bctor)
                                {
                                    CLS_Expression_Function baseFun = new CLS_Expression_Function(funcbegin, funcend, tokens[funcbegin].line, tokens[funcbegin].line);
                                    baseFun.funcname = classname + "->base";

                                    func.addBaseFun(baseFun, funcbegin, funcend, tokens[funcbegin].line, tokens[funcend].line);
                                }

                                (sClass.function as SType).addFun(idname, func);

                                i = funcend;
                            }
                            else if (tokens[funcbegin].text == ";")
                            {
                                func.expr_runtime = null;

                                (sClass.function as SType).addFun(idname, func);

                                i = funcbegin;
                            }
                            else if (tokens[funcbegin].text == ":")
                            {
                                // 添加 :base()的构造函数 支持
                                int funcbegin2 = funcbegin;

                                for (; funcbegin2 < iend; funcbegin2++)
                                {
                                    if (tokens[funcbegin2].text == "{")
                                    {
                                        CLS_Expression_Function baseFun = (CLS_Expression_Function)this.Compiler_Expression_Function(tokens, env, funcbegin + 1, funcbegin2 - 1);
                                        baseFun.funcname = classname + "->base";

                                        int funcend2 = FindBlock(env, tokens, funcbegin2);
                                        this.Compiler_Expression_Block(tokens, env, funcbegin2, funcend2, out func.expr_runtime);

                                        func.addBaseFun(baseFun, funcbegin, funcend2, tokens[funcbegin].line, tokens[funcend2].line);

                                        (sClass.function as SType).addFun(idname, func);

                                        i = funcend2;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(filename + "不可识别的函数表达式" + tokens[funcbegin].ToString());
                            }
                        }
                        else if (tokens[i + 2].type == CSLE.TokenType.PUNCTUATION && tokens[i + 2].text == "{")//语句块开始，这是 getset属性
                        {
                            logger.Log("发现Get/Set:" + idname);

                            SType.Member member = new SType.Member();
                            member.bStatic = bStatic;
                            member.sortIndex = sortIndex;
                            member.type = idtype;

                            i += 3;
                            for (int count = 2; count > 0; count--)
                            {
                                if (tokens[i].text == "get")
                                {
                                    // get 函数定义
                                    if (tokens[i + 1].text == "{")
                                    {
                                        member.getFun = "get_" + idname;

                                        SType.Function getFunc = new SType.Function();
                                        getFunc.bStatic = bStatic;

                                        int funcbegin = i + 1;
                                        int funcend = FindBlock(env, tokens, funcbegin);
                                        this.Compiler_Expression_Block(tokens, env, funcbegin, funcend, out getFunc.expr_runtime);

                                        (sClass.function as SType).addFun(member.getFun, getFunc);

                                        i = funcend + 1;
                                    }
                                    else
                                    {
                                        i += 2; // get; 模式
                                    }
                                }
                                else if (tokens[i].text == "set")
                                {
                                    // set 函数定义
                                    if (tokens[i + 1].text == "{")
                                    {
                                        member.setFun = "set_" + idname;

                                        SType.Function setFunc = new SType.Function();
                                        setFunc.bStatic = bStatic;
                                        setFunc._paramnames.Add("value");
                                        setFunc._paramtypes.Add(member.type);

                                        int funcbegin = i + 1;
                                        int funcend = FindBlock(env, tokens, funcbegin);
                                        this.Compiler_Expression_Block(tokens, env, funcbegin, funcend, out setFunc.expr_runtime);

                                        (sClass.function as SType).addFun(member.setFun, setFunc);

                                        i = funcend + 1;
                                    }
                                    else
                                    {
                                        i += 2; // set; 模式
                                    }
                                }
                                else if (tokens[i].text == "}")
                                {
                                    // get/set块结束
                                    break;
                                }
                                else
                                {
                                    throw new Exception(filename + "不可识别的Get/Set" + tokens[i].ToString());
                                }
                            }

                            (sClass.function as SType).addMember(idname, member);   
                        }
                        else if (tokens[i + 2].type == CSLE.TokenType.PUNCTUATION && (tokens[i + 2].text == "=" || tokens[i + 2].text == ";"))//这是成员定义
                        {
                            logger.Log("发现成员定义:" + idname);

                            SType.Member member = new SType.Member();
                            member.bStatic = bStatic;
                            member.sortIndex = sortIndex;
                            member.type = idtype;

                            if (tokens[i + 2].text == "=")
                            {
                                int posend = 0;
                                for (int j = i; j < iend; j++)
                                {
                                    if (tokens[j].text == ";")
                                    {
                                        posend = j - 1;
                                        break;
                                    }
                                }

                                int jbegin = i + 3;
                                int jdep;
                                int jend = FindCodeAny(tokens, ref jbegin, out jdep);
                                if (jend < posend)
                                {
                                    jend = posend;
                                }
                                if (!Compiler_Expression(tokens, env, jbegin, jend, out member.expr_defvalue))
                                {
                                    logger.Log_Error("Get/Set定义错误");
                                }

                                i = jend;
                            }

                            (sClass.function as SType).addMember(idname, member);
                        }

                        bStatic = false;
                        sortIndex = 0;
                        continue;
                    }
                    else
                    {
                        throw new Exception(filename + "不可识别的表达式" + tokens[i].ToString());
                    }
                }
            }
            sClass.compiled = true;

            // 关联基类(目前只支持单继承)
            if (baseTypeNames != null && baseTypeNames.Count > 0)
            {
                SType sBaseType = env.GetTypeByKeywordQuiet(baseTypeNames[0]).function as SType;
                SType sType = sClass.function as SType;
                sType.BaseType = sBaseType;

                bool hasBaseFun = false;

                foreach (KeyValuePair<string, SType.Function> pair in sBaseType.functions)
                {
                    if (!sType.functions.ContainsKey(pair.Key))
                    {
                        if (sBaseType.Name.Equals(pair.Key))
                        {
                            hasBaseFun = true;
                            sType.functions.Add(sType.Name + "->base", pair.Value);
                        }
                        else
                        {
                            if (pair.Value.ownerType == null)
                                pair.Value.ownerType = sBaseType;
                            sType.functions.Add(pair.Key, pair.Value);
                        }
                    }
                }

                foreach (KeyValuePair<string, SType.Member> pair in sBaseType.members)
                {
                    if (!sType.members.ContainsKey(pair.Key))
                        sType.members.Add(pair.Key, pair.Value);
                }

                foreach (KeyValuePair<string, SType.Member> pair in sBaseType.propertys)
                {
                    if (!sType.propertys.ContainsKey(pair.Key))
                        sType.propertys.Add(pair.Key, pair.Value);
                }

                // 自动创建可以调用基类函数的构造函数
                if (!sType.functions.ContainsKey(sType.Name) && hasBaseFun)
                {
                    // 处理 自身没构造函数 -- 基类有构造函数  的情况
                    SType.Function func = new SType.Function();
                    func.bStatic = false;

                    CLS_Expression_Function baseFun = new CLS_Expression_Function(0, 0, 0, 0);
                    baseFun.funcname = sType.Name + "->base";
                    func.addBaseFun(baseFun, 0, 0, 0, 0);
                    sType.functions.Add(sType.Name, func);
                }
                else if (sType.functions.ContainsKey(sType.Name) && !hasBaseFun)
                {
                    // 处理 自身有构造函数 -- 基类没有构造函数  的情况
                    SType.Function func = new SType.Function();
                    func.bStatic = false;
                    sType.functions.Add(sType.Name + "->base", func);
                }
            }

            return sClass;
        }

        int FindBlock(ICLS_Environment env, IList<CSLE.Token> tokens, int start)
        {
            if (tokens[start].type != CSLE.TokenType.PUNCTUATION)
            {
                env.logger.Log_Error("(script)FindBlock 没有从符号开始");
            }
            string left = tokens[start].text;
            string right = "}";
            if (left == "{") right = "}";
            if (left == "(") right = ")";
            if (left == "[") right = "]";
            int depth = 0;
            for (int i = start; i < tokens.Count; i++)
            {
                if (tokens[i].type == CSLE.TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == left)
                    {
                        depth++;
                    }
                    else if (tokens[i].text == right)
                    {
                        depth--;
                        if (depth == 0)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
    }
}