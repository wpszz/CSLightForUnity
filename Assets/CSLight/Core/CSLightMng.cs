// 开启以下宏可以在编辑器里强制使用资源包加载
//#define EDITOR_FORCE_ASSET_BUNDLE

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using CSLE;

public partial class CSLightMng : MonoBehaviour
{
    private static CSLightMng s_instance;
    public static CSLightMng instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameObject("CSLightMng").AddComponent<CSLightMng>();
                GameObject.DontDestroyOnLoad(s_instance.gameObject);
            }
            return s_instance;
        }
    }

    public static bool hasInstance
    {
        get
        {
            return s_instance != null;
        }
    }

#if UNITY_EDITOR
    protected bool m_scriptTokenDebug = true;
#else
    protected bool m_scriptTokenDebug = false;
#endif

    protected AssetBundle m_clsAssetBundle = null;
    protected CLS_Environment m_clsEnv = null;
    protected CLS_Content m_clsContent = null;

    // 当前脚本入口
    protected SInstance m_clsCurEntry = null;
    public SInstance currentEntry
    {
        get
        {
            return m_clsCurEntry;
        }
    }

    // 脚本更新频率(次数/每秒)
    protected float m_scriptUpdateGap = 0.2f;
    protected float m_scriptLastUpdateTime = 0f;
    public float sriptUpdateRate
    {
        get
        {
            return 1.0f / m_scriptUpdateGap;
        }
        set
        {
            if (value > 0)
                m_scriptUpdateGap = 1.0f / value;
            else
                m_scriptUpdateGap = 0.2f;

            m_scriptLastUpdateTime = Time.time - m_scriptUpdateGap + 0.2f;
        }
    }

    // 从资源包初始化脚本环境
    public void InitializeFromAssetBundle(AssetBundle scriptsAssetBundle)
    {
        m_clsAssetBundle = scriptsAssetBundle;

#if UNITY_EDITOR
        float timeStart = Time.realtimeSinceStartup;
        uint monoStart = Profiler.GetMonoUsedSize();
#endif

        // 获取默认的脚本实例
        m_clsEnv = ToCSLight.CreateEnvironment();
        m_clsContent = m_clsEnv.CreateContent();

        // 预注册脚本类
#if UNITY_EDITOR && !EDITOR_FORCE_ASSET_BUNDLE
        string rootPath = Application.dataPath + "/CSLight/Editor/CSLogic";
        string[] files = System.IO.Directory.GetFiles(rootPath, "*.cs", System.IO.SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(file);
            m_clsEnv.RegType(new CLS_Type_Class(className, file.Replace('\\', '/')));
        }
#else
        StringHolder classHolder = m_clsAssetBundle.LoadAsset("class", typeof(StringHolder)) as StringHolder;
        foreach (string className in classHolder.content)
        {
            m_clsEnv.RegType(new CLS_Type_Class(className, className));
        }
#endif

#if UNITY_EDITOR
        Debug.Log("script init cost time: " + (Time.realtimeSinceStartup - timeStart));
        Debug.Log(string.Format("script init cost memory: {0:0.00}MB", (Profiler.GetMonoUsedSize() - monoStart) / (1024f * 1024f)));
        timeStart = Time.realtimeSinceStartup;
#endif
    }

    // 运行脚本入口
    public Coroutine RunScriptEntry(string clsEntryName, params object[] startParams)
    {
        this.Clear();

        MonoBehaviour behaviour = new GameObject("ScriptEntryBehaviour").AddComponent<MonoBehaviour>();
        return behaviour.StartCoroutine(RunScriptEntryCoroutine(clsEntryName, behaviour, startParams));
    }
    IEnumerator RunScriptEntryCoroutine(string clsEntryName, MonoBehaviour behaviour, params object[] startParams)
    {
        m_clsContent = m_clsEnv.CreateContent();
        m_clsCurEntry = (SInstance)this.NewInstance(clsEntryName);

        this.SetMember(m_clsCurEntry, "behaviour", behaviour);

        yield return behaviour.StartCoroutine((IEnumerator)this.Call(m_clsCurEntry, "Start", startParams));

        this.Call(m_clsCurEntry, "UpdatePrevious");
    }

    // 执行简单的表达式
    public object Eval(string script)
    {
        IList<Token> tokens = null;
#if UNITY_EDITOR
        try
        {
#endif
            if (m_clsEnv == null)
            {
                Debug.LogWarning("please run project script first !");
                return null;
            }

            tokens = m_clsEnv.ParserToken(script);                              //词法分析
            RuntimeCompilerTokens(tokens);
            ICLS_Expression expr = m_clsEnv.Expr_CompilerToken(tokens, true);   //语法分析,简单表达式(一条语句)
            CLS_Content.Value value = m_clsEnv.Expr_Execute(expr, m_clsContent);//执行表达式
            return value == null ? null : value.value;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message + "\n" + m_clsContent.DumpStack(tokens) + ex.StackTrace);
            return null;
        }
#endif
    }

    // 执行一段脚本
    public object Execute(string script)
    {
        IList<Token> tokens = null;
#if UNITY_EDITOR
        try
        {
#endif
            if (m_clsEnv == null)
            {
                Debug.LogWarning("please run project script first !");
                return null;
            }

            tokens = m_clsEnv.ParserToken(script);                                  //词法分析
            RuntimeCompilerTokens(tokens);
            ICLS_Expression expr = m_clsEnv.Expr_CompilerToken(tokens, false);      //语法分析，语法块
            CLS_Content.Value value = m_clsEnv.Expr_Execute(expr, m_clsContent);    //执行表达式
            return value == null ? null : value.value;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message + "\n" + m_clsContent.DumpStack(tokens) + ex);
            return null;
        }
#endif
    }

    // 打印当前调用堆栈
    public string DumpStack(System.Exception ex)
    {
        if (ex != null)
        {
            string desc = ex.Message;
            desc += "\n";
            if (m_clsContent != null)
                desc += m_clsContent.DumpStack();
            desc += ex;
            return desc;
        }
        else
        {
            if (m_clsContent != null)
                return m_clsContent.DumpStack();
            return "";
        }
    }

    // 即时编译token
    protected void RuntimeCompilerTokens(IList<Token> tokens, List<string> types = null)
    {
        for (int i = 0, count = tokens.Count; i < count; i++)
        {
            Token token = tokens[i];
            if (token.type == TokenType.TYPE)
            {
                string className = token.text;
                CLS_Type_Class stype = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
                if (stype != null && !stype.compiled)
                {
                    if (types != null && types.Contains(className))
                        continue;
                    RuntimeCompilerClass(className, types);
                }
            }
        }
    }

    // 即时编译class
    protected void RuntimeCompilerClass(string className, List<string> types = null)
    {
        CLS_Type_Class stype = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
        if (stype == null || stype.compiled)
            return;

        if (types == null)
            types = new List<string>();
        if (!types.Contains(className))
            types.Add(className);

        IList<Token> tokens;
#if UNITY_EDITOR && !EDITOR_FORCE_ASSET_BUNDLE
        string code = System.IO.File.ReadAllText(((SType)stype.function).filename);
        tokens = m_clsEnv.ParserToken(code);
#else
        TextAsset codeText = m_clsAssetBundle.LoadAsset(className) as TextAsset;
        using (MemoryStream ms = new MemoryStream(codeText.bytes))
        {
            tokens = m_clsEnv.StreamToToken(ms);
        }
#endif
        RuntimeCompilerTokens(tokens, types);
        m_clsEnv.CompilerToken(className, tokens, m_scriptTokenDebug);
    }

    //======================================================================

    // 设置全局变量
    public void SetGlobalValue(string name, object val)
    {
        if (m_clsEnv == null)
        {
            Debug.LogWarning("please run project script first !");
            return;
        }

        m_clsContent.DefineAndSet(name, val.GetType(), val);
    }

    public void ClearGlobalValue()
    {
        if (m_clsEnv == null)
        {
            Debug.LogWarning("please run project script first !");
            return;
        }

        m_clsContent = m_clsEnv.CreateContent();
    }

    // 调用脚本实例成员函数
    public object Call(SInstance scriptInstance, string funName, params object[] _params)
    {
        if (scriptInstance == null)
            return null;

#if UNITY_EDITOR
        try
        {

            if (!scriptInstance.type.functions.ContainsKey(funName))
            {
                Debug.LogWarning(string.Format("Call({0}.{1}): fun not found!", scriptInstance.type.Name, funName));
                return null;
            }
#endif
            BetterList<CLS_Content.Value> paramList = ScriptParamConvert(_params);
            CLS_Content.Value retVal = scriptInstance.type.MemberCall(m_clsContent, scriptInstance, funName, paramList);
            CLS_Content.PoolParamList(paramList);
            return retVal != null ? retVal.value : null;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return null;
        }
#endif
    }

    // 获取脚本实例成员
    public object GetMember(SInstance scriptInstance, string memName, object val)
    {
        if (scriptInstance == null)
            return null;

#if UNITY_EDITOR
        try
        {

            if (!scriptInstance.type.members.ContainsKey(memName))
            {
                Debug.LogWarning(string.Format("GetMember({0}.{1}): mem not found!", scriptInstance.type.Name, memName));
                return null;
            }
#endif
            return scriptInstance.type.MemberValueGet(m_clsContent, scriptInstance, memName).value;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return null;
        }
#endif
    }

    // 赋值脚本实例成员
    public void SetMember(SInstance scriptInstance, string memName, object val)
    {
        if (scriptInstance == null)
            return;

#if UNITY_EDITOR
        try
        {

            if (!scriptInstance.type.members.ContainsKey(memName))
            {
                Debug.LogWarning(string.Format("SetMember({0}.{1}): mem not found!", scriptInstance.type.Name, memName));
                return;
            }
#endif
            scriptInstance.type.MemberValueSet(m_clsContent, scriptInstance, memName, val);
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return;
        }
#endif
    }

    // new脚本实例
    public object NewInstance(string className, params object[] _params)
    {
#if UNITY_EDITOR
        try
        {

            if (string.IsNullOrEmpty(className))
                return null;
#endif
            CLS_Type_Class type = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
            if (type == null)
            {
                Debug.LogWarning(string.Format("NewInstance({0}): class not found!", className));
                return null;
            }

            if (!type.compiled)
                RuntimeCompilerClass(className);

            BetterList<CLS_Content.Value> paramList = ScriptParamConvert(_params);
            CLS_Content.Value retVal = type.function.New(m_clsContent, paramList);
            CLS_Content.PoolParamList(paramList);
            return retVal != null ? retVal.value : null;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return null;
        }
#endif
    }

    // 调用脚本静态函数
    public object CallStatic(string className, string funName, params object[] _params)
    {
#if UNITY_EDITOR
        try
        {

            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(funName))
                return null;
#endif
            CLS_Type_Class type = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
            if (type == null)
            {
                Debug.LogWarning(string.Format("CallStatic({0}.{1}): class not found!", className, funName));
                return null;
            }

            if (!type.compiled)
                RuntimeCompilerClass(className);

            BetterList<CLS_Content.Value> paramList = ScriptParamConvert(_params);
            CLS_Content.Value retVal = type.function.StaticCall(m_clsContent, funName, paramList);
            CLS_Content.PoolParamList(paramList);
            return retVal != null ? retVal.value : null;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return null;
        }
#endif
    }

    // 获取脚本静态成员
    public object GetMemberStatic(string className, string memName)
    {
#if UNITY_EDITOR
        try
        {

            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(memName))
                return null;
#endif
            CLS_Type_Class type = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
            if (type == null)
            {
                Debug.LogWarning(string.Format("CallStatic({0}.{1}): class not found!", className, memName));
                return null;
            }

            if (!type.compiled)
                RuntimeCompilerClass(className);

            return type.function.StaticValueGet(m_clsContent, memName).value;
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return null;
        }
#endif
    }

    // 赋值脚本静态成员
    public void SetMemberStatic(string className, string memName, object val)
    {
#if UNITY_EDITOR
        try
        {

            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(memName))
                return;
#endif
            CLS_Type_Class type = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
            if (type == null)
            {
                Debug.LogWarning(string.Format("CallStatic({0}.{1}): class not found!", className, memName));
                return;
            }
            type.function.StaticValueSet(m_clsContent, memName, val);
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(DumpStack(ex));
            return;
        }
#endif
    }

    protected BetterList<CLS_Content.Value> ScriptParamConvert(params object[] _params)
    {
        if (_params == null)
            return CLS_Content.NewParamList();

        BetterList<CLS_Content.Value> listParam = CLS_Content.NewParamList();
        int paramCount = _params.Length;
        for (int i = 0; i < paramCount; i++)
        {
            object param = _params[i];
            if (param == null)
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.type = typeof(object);
                v.value = null;
                listParam.Add(v);
            }
            else
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.type = param.GetType();
                v.value = param;
                listParam.Add(v);
            }
        }
        return listParam;
    }

    //=====================================================

    void Clear()
    {
        if (m_clsCurEntry != null)
        {
            this.Call(m_clsCurEntry, "Clear");
            m_clsCurEntry = null;
        }
    }

    void Update()
    {
        if (Time.time - m_scriptLastUpdateTime < m_scriptUpdateGap)
            return;
        m_scriptLastUpdateTime = Time.time;

        if (m_clsCurEntry != null)
        {
            this.Call(m_clsCurEntry, "Update");
        }
    }
}
