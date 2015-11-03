using UnityEngine;
using UnityEditor;
using System;
using System.IO; 
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using CSLE;

public class ToCSLightEditor : EditorWindow 
{
    static string ScriptFolder          = Application.dataPath + "/CSLight/Editor/CSLogic";
    static string ResTemplatePath       = Application.dataPath + "/CSLight/Editor/Res/ToCSLightTemplate.txt";
    static string ResRulePath           = Application.dataPath + "/CSLight/Editor/Res/ToCSLightRule.txt";
    static string ToCSLightPathPrefix   = Application.dataPath + "/CSLight/ToCSLight/ToCSLight";

    //===============================================================================================================

    List<Type> m_listType = new List<Type>();
    Dictionary<string, Type> m_dictBuildType = new Dictionary<string, Type>();
    List<string> m_listGenericsFunction = new List<string>();

    Dictionary<string, object> m_jsonRule = null;
    Dictionary<string, object> m_jsonRuleAssembly = null;
    Dictionary<string, object> m_jsonRuleRemoveMember = null;

    List<Type> m_listRemoveTypeList = new List<Type>();

    Vector2 m_vScrollPos = Vector2.zero;
    Vector2 m_vScrollPos2 = Vector2.zero;
    Vector2 m_vScrollPos3 = Vector2.zero;

    Type m_curViewType = null;

    bool m_viewGenericsFunction = false;

    bool m_genConfig = false;

    [MenuItem("CSLight/ToCSLightEditor")]
	static void Init() 
	{
        ToCSLightEditor wnd = ScriptableObject.CreateInstance<ToCSLightEditor>();
		wnd.ShowUtility();

        wnd.titleContent = new GUIContent("ToCSLightEditor");
        wnd.position = new Rect(300, 100, Screen.currentResolution.width - 600, Screen.currentResolution.height - 200);

#if UNITY_ANDROID && !UNITY_5_2
        string dllPath = CombinePaths(EditorApplication.applicationContentsPath, "PlaybackEngines", "AndroidPlayer", "Managed", "UnityEngine.dll");
#elif UNITY_IPHONE
        string dllPath = CombinePaths(EditorApplication.applicationContentsPath, "PlaybackEngines", "iossupport", "Managed", "UnityEngine.dll");
#elif UNITY_WP8 || UNITY_WP_8_1
        string dllPath = CombinePaths(EditorApplication.applicationContentsPath, "PlaybackEngines", "wp8support", "Managed", "UnityEngine.dll"); 
#else
        string dllPath = typeof(UnityEngine.Application).Assembly.Location;
#endif
        System.Reflection.Assembly dllUnityEngine = System.Reflection.Assembly.ReflectionOnlyLoadFrom(dllPath);
        foreach (Type type in wnd.m_listType)
        {
            Type buildType = dllUnityEngine.GetType(type.FullName);
            if (buildType != null)
            {
                wnd.m_dictBuildType.Add(type.FullName, buildType);
            }
        }

        // 加载规则json
        string rule = System.IO.File.ReadAllText(ResRulePath, Encoding.UTF8);
        wnd.m_jsonRule = MiniJSON.Json.Deserialize(rule) as Dictionary<string, object>;
        wnd.m_jsonRuleAssembly = wnd.m_jsonRule["Assemblys"] as Dictionary<string, object>;
        wnd.m_jsonRuleRemoveMember = wnd.m_jsonRule["RemoveMember"] as Dictionary<string, object>;

        foreach (var pair in wnd.m_jsonRuleAssembly)
        {
            wnd.RunDllJsonRule(pair.Key, pair.Value as Dictionary<string, object>);
        }

        wnd.m_listType.Sort(delegate(Type small, Type big)
        {
            bool hasSmall = isFileExisted(ToCSLightPathPrefix + small.Name + ".cs");
            bool hasBig = isFileExisted(ToCSLightPathPrefix + big.Name + ".cs");
            if (hasSmall && !hasBig)
                return -1;
            if (!hasSmall && hasBig)
                return 1;
            return string.Compare(toCSLightName(small), toCSLightName(big));
        });

        if (Directory.Exists(ScriptFolder))
        {
            string[] filePaths = Directory.GetFiles(ScriptFolder, "*.cs", SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                StringReader sr = new StringReader(System.IO.File.ReadAllText(filePath, Encoding.UTF8));
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Contains(">("))
                    {
                        wnd.m_listGenericsFunction.Add(line);
                    }
                    line = sr.ReadLine();
                }
            }
        }
    }

    static string CombinePaths(params string[] paths)
    {
        string result = null;
        bool first = true;
        foreach (var e in paths)
        {
            if (first)
            {
                result = e;
                first = false;
                continue;
            }
            result = Path.Combine(result, e);
        }
        return result;
    }

    static List<T> TryGetList<T>(string key, Dictionary<string, object> dictObj)
    {
        object o;
        if (dictObj.TryGetValue(key, out o))
        {
            return o as List<T>;
        }
        return null;
    }

    static Dictionary<T1, T2> TryGetDict<T1, T2>(string key, Dictionary<string, object> dictObj)
    {
        object o;
        if (dictObj.TryGetValue(key, out o))
        {
            return o as Dictionary<T1, T2>;
        }
        return null;
    }

    void RunDllJsonRule(string dllName, Dictionary<string, object> dllJsonRule)
    {
        Assembly[] assemblys = System.AppDomain.CurrentDomain.GetAssemblies();

        Assembly assemblyFind = null;
        foreach (Assembly assembly in assemblys)
        {
            if ((assembly.GetName().Name + ".dll") == dllName)
            {
                assemblyFind = assembly;
                break;
            }
        }
        
        if (assemblyFind == null)
        {
            Debug.LogWarning("Can't find dll: " + dllName);
            return;
        }

        List<object> jsonRuleIncludeNameSpace = TryGetList<object>("IncludeNameSpace", dllJsonRule);
        List<object> jsonRuleIncludeType = TryGetList<object>("IncludeType", dllJsonRule);
        List<object> jsonRuleRemoveType = TryGetList<object>("RemoveType", dllJsonRule);
        List<object> jsonRuleRemoveTypeList = TryGetList<object>("RemoveTypeList", dllJsonRule);

        CollectTypesByNames(assemblyFind, jsonRuleIncludeType, m_listType);

        List<Type> removeTypes = new List<Type>();
        CollectTypesByNames(assemblyFind, jsonRuleRemoveType, removeTypes);

        CollectTypesByNamespaces(assemblyFind, jsonRuleIncludeNameSpace, removeTypes, m_listType);

        CollectTypesByNames(assemblyFind, jsonRuleRemoveTypeList, m_listRemoveTypeList);
    }

    void CollectTypesByNames(Assembly assembly, List<object> jsonRuleIncludeType, List<Type> outputTypes)
    {
        if (jsonRuleIncludeType == null || jsonRuleIncludeType.Count == 0)
            return;

        foreach (string typeName in jsonRuleIncludeType)
        {
            Type type = assembly.GetType(typeName);
            if (type == null)
            {
                string[] words = typeName.Split('.');
                int len = words.Length;
                string subTypeName = words[0];
                for (int i = 1; i < len; i++)
                {
                    if (assembly.GetType(subTypeName) != null)
                        subTypeName += "+" + words[i];  // 类中类
                    else
                        subTypeName += "." + words[i];  // 名空间
                }

                type = assembly.GetType(subTypeName);
                if (type == null)
                {
                    Debug.LogWarning("Can't find type: " + typeName + " from dll: " + assembly);
                    continue;
                }
            }
            if (!outputTypes.Contains(type))
                outputTypes.Add(type);
            else
                Debug.LogWarning("Store repeat type: " + type + " from dll: " + assembly);
        }
    }

    void CollectTypesByNamespaces(Assembly assembly, List<object> jsonRuleIncludeNameSpace, List<Type> removeTypes, List<Type> outputTypes)
    {
        if (jsonRuleIncludeNameSpace == null || jsonRuleIncludeNameSpace.Count == 0)
            return;

        foreach (Type type in assembly.GetTypes())
        {
            if (!jsonRuleIncludeNameSpace.Contains(type.Namespace))
                continue;

            if (removeTypes.Contains(type))
                continue;

            // 过滤不可外部访问的
            if (!type.IsVisible)
                continue;

            // 过滤泛型
            if (type.IsGenericType)
                continue;

            if (!outputTypes.Contains(type))
                outputTypes.Add(type);
            else
                Debug.LogWarning("Store repeat type: " + type + " from dll: " + assembly);
        }
    }

    //==============================================================================

	void OnGUI()
	{
        GUI.color = Color.green;
        GUILayout.Label("Type count = " + m_listType.Count + " " + (m_curViewType != null ? m_curViewType.Name : ""));
        GUI.color = Color.white;
        if (m_viewGenericsFunction)
        {
            m_vScrollPos3 = EditorGUILayout.BeginScrollView(m_vScrollPos3, true, true);
            foreach (string genericsFun in m_listGenericsFunction)
            {
                if (genericsFun.Contains("List"))
                    GUI.color = Color.green;
                else if (genericsFun.Contains("Dictionary"))
                    GUI.color = Color.red;
                else if (genericsFun.Contains("KeyValuePair"))
                    GUI.color = Color.yellow;
                else
                    GUI.color = Color.white;
                GUILayout.Label(genericsFun);
            }
            EditorGUILayout.EndScrollView();
            GUI.color = Color.white;
            if (GUILayout.Button("return", GUILayout.Height(48)))
            {
                m_viewGenericsFunction = false;
            }
        }
        else if (m_curViewType == null)
        {
            m_vScrollPos = EditorGUILayout.BeginScrollView(m_vScrollPos, true, true);
            foreach (Type type in m_listType)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.white;
                GUILayout.Label(type.ToString(), GUILayout.Width(300));
                GUI.color = Color.green;
                GUILayout.Label(toCSLightName(type), GUILayout.Width(300));
                GUI.color = Color.white;
                if (GUILayout.Button("view", GUILayout.Width(100)))
                {
                    m_curViewType = type;
                    break;
                }
                if (!isFileExisted(ToCSLightPathPrefix + type.Name + ".cs"))
                    GUI.color = Color.white;
                else
                    GUI.color = Color.green;
                if (GUILayout.Button("to CSLight", GUILayout.Width(200)))
                {
                    toCSLight(type, BindingFlags.FlattenHierarchy);
                    m_genConfig = true;
                }

                if (isStatic(type))
                {
                    GUI.color = Color.red;
                    GUILayout.Label("static", GUILayout.Width(300));
                }

                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            GUI.color = Color.white;
            if (GUILayout.Button("View Generics Function", GUILayout.Height(48)))
            {
                m_viewGenericsFunction = true;
            } 
            GUI.color = Color.green;
            if (GUILayout.Button("Gen config", GUILayout.Height(48)))
            {
                m_genConfig = true;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            m_vScrollPos2 = EditorGUILayout.BeginScrollView(m_vScrollPos2, true, true);

            // 静态方法
            GUI.color = Color.white;
            var sFuns = getMethods(m_curViewType, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var sFun in sFuns)
            {
                if (hasRefOrOut(sFun))
                    continue;
                GUILayout.Label(sFun.ToString(), GUILayout.Width(600));
            }
            // 运算符重载方法
            GUI.color = new Color(0.8f, 0.4f, 0f);
            foreach (var sFun in sFuns)
            {
                if (isObsolete(sFun))
                    continue;
                if (!isOperator(sFun))
                    continue;
                if (!isLeftValueSelf(sFun))
                    continue;
                GUILayout.Label(sFun.ToString(), GUILayout.Width(600));
            }
            // 静态成员
            GUI.color = Color.green;
            var sMems = getMembers(m_curViewType, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var sMem in sMems)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(sMem.ToString(), GUILayout.Width(600));
                GUILayout.Label(string.Format("{0}/{1}", canRead(sMem) ? "r" : "", canWrite(sMem) ? "w" : ""), GUILayout.Width(50));
                GUILayout.EndHorizontal();
            }
            // 构造方法
            GUI.color = new Color(1f, 0f, 0.4f);
            var constructors = m_curViewType.GetConstructors();
            foreach (var constructor in constructors)
            {
                GUILayout.Label(constructor.ToString(), GUILayout.Width(600));
            }
            // 实例方法
            GUI.color = Color.yellow;
            var mFuns = getMethods(m_curViewType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var mFun in mFuns)
            {
                if (hasRefOrOut(mFun))
                    continue;
                if (isNeedlessMemberFun(mFun))
                    continue;
                GUILayout.Label(mFun.ToString(), GUILayout.Width(600));
            }
            // 实例成员
            GUI.color = Color.cyan;
            var mMems = getMembers(m_curViewType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var mMem in mMems)
            {
                if (isIndex(mMem))
                    continue;
                GUILayout.Label(mMem.ToString(), GUILayout.Width(600));
            }
            // 实例this索引
            GUI.color = new Color(0f, 0.5f, 1f);
            foreach (var mMem in mMems)
            {
                if (isObsolete(mMem))
                    continue;
                if (!isIndex(mMem))
                    continue;
                GUILayout.Label(mMem.ToString(), GUILayout.Width(600));
            }

            EditorGUILayout.EndScrollView();
            GUI.color = Color.red;
            if (GUILayout.Button("return", GUILayout.Height(64)))
            {
                m_curViewType = null;
            }
        }
	}

    void Update()
    {
        if (m_genConfig)
        {
            m_genConfig = false;

            string path = ToCSLightPathPrefix + ".cs";
            string code = System.IO.File.ReadAllText(path, Encoding.UTF8);

            StringBuilder gen = new StringBuilder();

            foreach (Type type in m_listType)
            {
                string toName = toCSLightName(type);

                if (isFileExisted(ToCSLightPathPrefix + type.Name + ".cs"))
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new ToCSLight{0}());", type.Name);
                    gen.AppendLine();
                    toCSLight(type, BindingFlags.FlattenHierarchy, true);
                    continue;
                }

                if (!isDelegate(type))
                {
                    if (type.Name == toName)
                    {
                        gen.Append("\t\t");
                        gen.AppendFormat("clsEnv.RegType(new {1}(typeof({0})));", toName, type.IsEnum ? "CLS_Type_Enum" : "RegHelper_Type");
                        gen.AppendLine();
                    }
                    else
                    {
                        gen.Append("\t\t");
                        gen.AppendFormat("clsEnv.RegType(new {1}(typeof({0}), \"{0}\"));", toName, type.IsEnum ? "CLS_Type_Enum" : "RegHelper_Type");
                        gen.AppendLine();
                    }
                    continue;
                }

                MethodInfo invoke = type.GetMethod("Invoke");

                ParameterInfo[] ps = invoke.GetParameters();

                if (invoke.ReturnType == typeof(void) && ps.Length == 0)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleAction(typeof({0}), \"{0}\"));", toName);
                    gen.AppendLine();
                    continue; 
                }
                if (invoke.ReturnType == typeof(void) && ps.Length == 1)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleAction<{1}>(typeof({0}), \"{0}\"));", toName, toCSLightName(ps[0].ParameterType));
                    gen.AppendLine();
                    continue;
                }
                if (invoke.ReturnType == typeof(void) && ps.Length == 2)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleAction<{1}, {2}>(typeof({0}), \"{0}\"));", toName, toCSLightName(ps[0].ParameterType), toCSLightName(ps[1].ParameterType));
                    gen.AppendLine();
                    continue;
                }
                if (invoke.ReturnType == typeof(void) && ps.Length == 3)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleAction<{1}, {2}, {3}>(typeof({0}), \"{0}\"));", toName, toCSLightName(ps[0].ParameterType), toCSLightName(ps[1].ParameterType), toCSLightName(ps[2].ParameterType));
                    gen.AppendLine();
                    continue;
                }

                if (ps.Length == 0)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleNonVoidAction<{1}>(typeof({0}), \"{0}\"));", toName, toCSLightName(invoke.ReturnType));
                    gen.AppendLine();
                    continue;
                }
                if (ps.Length == 1)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleNonVoidAction<{1}, {2}>(typeof({0}), \"{0}\"));", toName, toCSLightName(invoke.ReturnType), toCSLightName(ps[0].ParameterType));
                    gen.AppendLine();
                    continue;
                }
                if (ps.Length == 2)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleNonVoidAction<{1}, {2}, {3}>(typeof({0}), \"{0}\"));", toName, toCSLightName(invoke.ReturnType), toCSLightName(ps[0].ParameterType), toCSLightName(ps[1].ParameterType));
                    gen.AppendLine();
                    continue;
                }
                if (ps.Length == 3)
                {
                    gen.Append("\t\t");
                    gen.AppendFormat("clsEnv.RegType(new RegHelper_DeleNonVoidAction<{1}, {2}, {3}, {4}>(typeof({0}), \"{0}\"));", toName, toCSLightName(invoke.ReturnType), toCSLightName(ps[0].ParameterType), toCSLightName(ps[1].ParameterType), toCSLightName(ps[2].ParameterType));
                    gen.AppendLine();
                    continue;
                }
            }

            bool removeDelegateList = (bool)m_jsonRule["RemoveDelegateList"];
            bool removeEnumList = (bool)m_jsonRule["RemoveEnumList"];

            // 数组/列表
            foreach (Type type in m_listType)
            {
                if (isStatic(type))
                    continue;

                if (isInheritFromList(type, m_listRemoveTypeList))
                    continue;

                if (removeDelegateList && isDelegate(type))
                    continue;

                if (removeEnumList && type.IsEnum)
                    continue;

                string toName = toCSLightName(type);
                gen.Append("\t\t");
                gen.AppendFormat("clsEnv.RegType(new CLS_Type_Array<{0}>(\"{0}[]\"));", toName);
                gen.AppendLine();

                gen.Append("\t\t");
                gen.AppendFormat("clsEnv.RegType(new CLS_Type_List<{0}>(\"List<{0}>\"));", toName);
                gen.AppendLine();
            }

            string startStr = "// gen start";
            string endStr = "// gen end";
            int startPos = code.IndexOf(startStr) + startStr.Length + 2;
            int endPos = code.IndexOf(endStr) - 2;
            code = code.Remove(startPos, endPos - startPos);
            code = code.Insert(startPos, gen.ToString());

            System.IO.File.WriteAllText(path, code, Encoding.UTF8);

            EditorUtility.DisplayDialog("finish", "success done!", "ok");
        }
    }

    //=======================辅助函数===========================

    MethodInfo[] getMethods(Type type, BindingFlags flags)
    {
        Type buildType = null;
        m_dictBuildType.TryGetValue(type.FullName, out buildType);
        object rule = null;
        m_jsonRuleRemoveMember.TryGetValue(type.FullName, out rule);

        var buildFuns = buildType != null ? buildType.GetMethods(flags) : null;
        var funs = type.GetMethods(flags);
        var list = new List<MethodInfo>();
        foreach (var fun in funs)
        {
            if (isObsolete(fun))
                continue;
            if (fun.IsSpecialName)
                continue;

            if (type.BaseType != null && type.BaseType.Name.Contains("MonoSingleton"))
            {
                if (fun.DeclaringType != type && fun.DeclaringType != type.BaseType)
                    continue;
            }

            if (buildType != null)
            {
                bool existed = false;
                foreach (var buildFun in buildFuns)
                {
                    if (buildFun.Name == fun.Name)
                    {
                        ParameterInfo[] ps1 = fun.GetParameters();
                        ParameterInfo[] ps2 = buildFun.GetParameters();
                        if (ps1.Length == ps2.Length)
                        {
                            bool sameParam = true;
                            for (int i = 0; i < ps1.Length; i++)
                            {
                                if (ps1[i].ParameterType.FullName != ps2[i].ParameterType.FullName)
                                {
                                    sameParam = false;
                                    break;
                                }
                            }

                            if (sameParam)
                            {
                                existed = true;
                                break;
                            }
                        }
                    }
                }
                if (!existed)
                    continue;
            }

            if (rule != null)
            {
                List<object> listRule = (List<object>)rule;
                if (listRule.Contains(fun.Name))
                    continue;
            }

            list.Add(fun);
        }

        list.Sort(delegate(MethodInfo small, MethodInfo big)
        {
            return small.Name.CompareTo(big.Name);
        });

        return list.ToArray();
    }

    MemberInfo[] getMembers(Type type, BindingFlags flags)
    {
        Type buildType = null;
        m_dictBuildType.TryGetValue(type.FullName, out buildType);
        object rule = null;
        m_jsonRuleRemoveMember.TryGetValue(type.FullName, out rule);

        var mems = type.GetMembers(flags);
        var list = new List<MemberInfo>();
        foreach (var mem in mems)
        {
            if (isObsolete(mem))
                continue;
            if (!isVal(mem))
                continue;

            if (type.BaseType != null && type.BaseType.Name.Contains("MonoSingleton"))
            {
                if (mem.DeclaringType != type && mem.DeclaringType != type.BaseType)
                    continue;
            }

            if (buildType != null)
            {
                if (buildType.GetMember(mem.Name, flags).Length == 0)
                    continue;
            }

            if (rule != null)
            {
                List<object> listRule = (List<object>)rule;
                if (listRule.Contains(mem.Name))
                    continue;
            }

            list.Add(mem);
        }

        list.Sort(delegate(MemberInfo small, MemberInfo big)
        {
            return small.Name.CompareTo(big.Name);
        });

        return list.ToArray();
    }

    //==================================================================

    static string toCSLightName(Type type)
    {
        if (type.IsArray)
        {
            Type elementType = type.GetElementType();
            return toCSLightName(elementType) + "[]";
        }
        if (type == typeof(bool))
            return "bool";
        if (type == typeof(byte))
            return "byte";
        if (type == typeof(sbyte))
            return "sbyte";
        if (type == typeof(short))
            return "short";
        if (type == typeof(ushort))
            return "ushort";
        if (type == typeof(int))
            return "int";
        if (type == typeof(uint))
            return "uint";
        if (type == typeof(long))
            return "long";
        if (type == typeof(ulong))
            return "ulong";
        if (type == typeof(float))
            return "float";
        if (type == typeof(double))
            return "double";
        if (type == typeof(string))
            return "string";
        if (type == typeof(object))
            return "object";
        if (type == typeof(UnityEngine.Object))
            return "UnityEngine.Object";
        if (type == typeof(UnityEngine.Random))
            return "UnityEngine.Random";
        if (type.IsGenericType)
        {
            Type[] ps = type.GetGenericArguments();
            string str = type.Name.Remove(type.Name.IndexOf('`'));
            str += "<";
            for (int i = 0; i < ps.Length; i++)
            {
                str += toCSLightName(ps[i]);
                if (i + 1 < ps.Length)
                    str += ", ";
            }
            str += ">";
            return str;
        }

        return (type.DeclaringType != null ? (toCSLightName(type.DeclaringType) + ".") : "") + type.Name;
    }

    static bool hasRefOrOut(MethodInfo method)
    {
        foreach (ParameterInfo param in  method.GetParameters())
        {
            if (param.ToString().Contains("&"))
                return true;   
            if (param.IsOut)  
                return true;
        }
        return false;
    }

    static bool hasDefaultParam(MethodBase method, out int startIndex)
    {
        startIndex = 0;
        foreach (ParameterInfo param in method.GetParameters())
        {
            if (param.IsOptional)
            //if ((param.Attributes & ParameterAttributes.HasDefault) != 0)
                return true;
            startIndex++;
        }
        return false;
    }

    static bool isParamsArray(ParameterInfo param)
    {
        foreach (Attribute attribute in param.GetCustomAttributes(false))
        {
            if (attribute is ParamArrayAttribute)
                return true;
        }
        return false;
    }

    static bool hasParamsArray(MethodInfo method, out int startIndex)
    {
        startIndex = 0;
        foreach (ParameterInfo param in method.GetParameters())
        {
            if (isParamsArray(param))
                return true;
            startIndex++;
        }
        return false;
    }

    static bool hasType(MethodInfo method, Type type)
    {
        foreach (ParameterInfo param in method.GetParameters())
        {
            if (param.ParameterType == type)
                return true;
        }
        return method.ReturnType == type;
    }

    static bool isFileExisted(string fileName)
    {
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
        return fileInfo.Exists;
    }

    static bool isConstructor(MethodBase method)
    {
        return method.IsSpecialName && method.Name == ".ctor";
    }

    static bool isOperator(MethodInfo method)
    {
        return method.IsSpecialName && method.Name.StartsWith("op_");
    }

    static bool isLeftValueSelf(MethodInfo method)
    {
        ParameterInfo[] ps = method.GetParameters();
        if (ps.Length > 0 && ps[0].ParameterType.Equals(method.DeclaringType))
            return true;
        return false;
    }

    static bool isNeedlessMemberFun(MethodInfo method)
    {
        if (method.Name == "GetType" ||
            method.Name == "GetHashCode")
            return true;
        return false;
    }

    static bool isNeedlessStaticFun(MethodInfo method)
    {
        if (method.Name == "Equals" ||
            method.Name == "ReferenceEquals")
            return true;
        return false;
    }

    static bool isVal(MemberInfo mem)
    {
        return mem.MemberType == MemberTypes.Field || mem.MemberType == MemberTypes.Property;
    }

    static bool isIndex(MemberInfo mem)
    {
        if (isVal(mem))
            return mem.Name == "Item" || mem.Name == "Chars";
        return false;
    }

    static bool isObsolete(MemberInfo mem)
    {
        foreach (object attr in mem.GetCustomAttributes(false))
        {
            if (attr is ObsoleteAttribute)
                return true;
            if (attr is EditorOnlyAttribute)
                return true;
        }
        return false;
    }

    static Type valType(MemberInfo fieldOrProperty)
    {
        Type vt = null;
        if (fieldOrProperty.MemberType == MemberTypes.Field)
            vt = (fieldOrProperty as FieldInfo).FieldType;
        else if (fieldOrProperty.MemberType == MemberTypes.Property)
            vt = (fieldOrProperty as PropertyInfo).PropertyType;
        return vt;
    }

    static bool canRead(MemberInfo fieldOrProperty)
    {
        if (fieldOrProperty.MemberType == MemberTypes.Field)
            return true;
        else if (fieldOrProperty.MemberType == MemberTypes.Property)
        {
            PropertyInfo property = fieldOrProperty as PropertyInfo;
            MethodInfo setMethod = property.GetGetMethod(false);
            return setMethod != null;
        }
        return false;
    }

    static bool canWrite(MemberInfo fieldOrProperty)
    {
        if (fieldOrProperty.MemberType == MemberTypes.Field)
            return !(fieldOrProperty as FieldInfo).IsLiteral && !(fieldOrProperty as FieldInfo).IsInitOnly;
        else if (fieldOrProperty.MemberType == MemberTypes.Property)
        {
            PropertyInfo property = fieldOrProperty as PropertyInfo;
            MethodInfo setMethod = property.GetSetMethod(false);
            return setMethod != null;
        }
        return false;
    }

    static bool hasIntaface(Type vt, Type i)
    {
        foreach (Type t in vt.GetInterfaces())
        {
            if (t == i)
                return true;
        }
        return false;
    }

    static bool isDelegate(Type vt)
    {
        return vt.IsSubclassOf(typeof(System.Delegate));
    }

    static bool isStatic(Type vt)
    {
        return vt.IsAbstract && vt.IsSealed;
    }

    static bool isInheritFrom(Type child, Type parent)
    {
        if (child == parent)
            return true;
        if (child.IsSubclassOf(parent))
            return true;
        if (parent.IsGenericType)
        {
            Type genType = parent.MakeGenericType(new Type[] { child });
            if (genType != null && child.IsSubclassOf(genType))
                return true;
        }
        return false;
    }

    static bool isInheritFromList(Type child, List<Type> parents)
    {
        foreach (Type parent in parents)
        {
            if (isInheritFrom(child, parent))
                return true;
        }
        return false;
    }

    static string covertTo(Type vt, string value)
    {
        if (vt == typeof(bool))
            return string.Format("Convert.ToBoolean({0})", value);
        if (vt == typeof(char))
            return string.Format("Convert.ToChar({0})", value);
        if (vt == typeof(byte))
            return string.Format("Convert.ToByte({0})", value);
        if (vt == typeof(short))
            return string.Format("Convert.ToInt16({0})", value);
        if (vt == typeof(ushort))
            return string.Format("Convert.ToUInt16({0})", value);
        if (vt == typeof(int))
            return string.Format("Convert.ToInt32({0})", value);
        if (vt == typeof(uint))
            return string.Format("Convert.ToUInt32({0})", value);
        if (vt == typeof(long))
            return string.Format("Convert.ToInt64({0})", value);
        if (vt == typeof(ulong))
            return string.Format("Convert.ToUInt64({0})", value);
        if (vt == typeof(float))
            return string.Format("Convert.ToSingle({0})", value);
        if (vt == typeof(double))
            return string.Format("Convert.ToDouble({0})", value);
        if (vt == typeof(object))
            return value;
        if (isDelegate(vt))
            return string.Format("({1} == null ? null: ({0})content.environment.GetType({1}.GetType()).ConvertTo(content, {1}, typeof({0})))", toCSLightName(vt), value);
        if (isStatic(vt))
            return value;
        return string.Format("(({0}){1})", toCSLightName(vt), value);
    }

    static string isMatch(Type vt, bool isAllConvertible, bool hasFloatAndDouble)
    {
        if (vt == typeof(double))
            return "IConvertible";

        if (!hasFloatAndDouble)
        {
            if (vt == typeof(float))
                return "IConvertible";
        }

        if (!isAllConvertible)
        {
            if (vt == typeof(bool) ||
                vt == typeof(char) ||
                vt == typeof(byte) ||
                vt == typeof(short) ||
                vt == typeof(ushort) ||
                vt == typeof(int) ||
                vt == typeof(uint) ||
                vt == typeof(long) ||
                vt == typeof(ulong) ||
                vt == typeof(float) ||
                vt == typeof(double))
                return "IConvertible";
        }
        return toCSLightName(vt);
    }

    static Dictionary<Type, int> dictTypePriority = null;
    static int compareTypePriority(Type big, Type small)
    {
        if (hasIntaface(big, typeof(IConvertible)) && !hasIntaface(small, typeof(IConvertible)))
            return -1;
        if (!hasIntaface(big, typeof(IConvertible)) && hasIntaface(small, typeof(IConvertible)))
            return 1;
        if (!hasIntaface(big, typeof(IConvertible)) && !hasIntaface(small, typeof(IConvertible)))
            return 0;

        if (dictTypePriority == null)
        {
            dictTypePriority = new Dictionary<Type, int>();
            int p = 0;
            dictTypePriority[typeof(bool)] = p++;
            dictTypePriority[typeof(char)] = p++;
            dictTypePriority[typeof(byte)] = p++;
            dictTypePriority[typeof(short)] = p++;
            dictTypePriority[typeof(ushort)] = p++;
            dictTypePriority[typeof(int)] = p++;
            dictTypePriority[typeof(uint)] = p++;
            dictTypePriority[typeof(long)] = p++;
            dictTypePriority[typeof(ulong)] = p++;
            dictTypePriority[typeof(float)] = p++;
            dictTypePriority[typeof(double)] = p++;
        }
        int p1, p2 = 0;
        dictTypePriority.TryGetValue(big, out p1);
        dictTypePriority.TryGetValue(small, out p2);
        if (p1 < p2)
            return -1;
        if (p1 > p2)
            return 1;
        return 0;
    }

    static bool isAllParamsSameCount(MethodBase[] funs)
    {
        int count = -1;
        foreach (var fun in funs)
        {
            if (count == -1)
                count = fun.GetParameters().Length;
            else if (count != fun.GetParameters().Length)
                return false;
        }
        return true;
    }

    static bool isAllParamsSameType(MethodBase[] funs)
    {
        string typeName = null;
        foreach (var fun in funs)
        {
            foreach (var p in fun.GetParameters())
            {
                if (typeName == null)
                    typeName = toCSLightName(p.ParameterType);
                else if (typeName != toCSLightName(p.ParameterType))
                    return false;
            }
        }
        return true;
    }

    static bool isAllParamsConvertible(MethodBase[] funs)
    {
        foreach (var fun in funs)
        {
            foreach (var p in fun.GetParameters())
            {
                if (!hasIntaface(p.ParameterType, typeof(IConvertible)))
                    return false;
            }
        }
        return true;
    }

    static bool hasFloatAndDoubleParams(MethodBase[] funs)
    {
        bool hasFloat = false;
        bool hasDouble = false;
        foreach (var fun in funs)
        {
            foreach (var p in fun.GetParameters())
            {
                if (p.ParameterType == typeof(float))
                {
                    if (hasDouble)
                        return true;
                    hasFloat = true;
                }
                if (p.ParameterType == typeof(double))
                {
                    if (hasFloat)
                        return true;
                    hasDouble = true;
                }
            }
        }
        return false;
    }

    static Dictionary<string, List<MethodInfo>> sameNameFunsOri(Type type, MethodInfo[] funs)
    {
        Dictionary<string, List<MethodInfo>> dictResult = new Dictionary<string, List<MethodInfo>>();
        foreach (MethodInfo fun in funs)
        {
            // 暂不支持decimal类型参数或返回值的方法
            if (hasType(fun, typeof(decimal)))
                continue;

            if (!dictResult.ContainsKey(fun.Name))
                dictResult.Add(fun.Name, new List<MethodInfo>());
            List<MethodInfo> list = dictResult[fun.Name];
            list.Add(fun);
        }
        return dictResult;
    }

    static Dictionary<string, List<MethodInfo>> sameNameFuns(Type type, MethodInfo[] funs)
    {
        Dictionary<string, List<MethodInfo>> dictResult = new Dictionary<string, List<MethodInfo>>();
        foreach (MethodInfo fun in funs)
        {
            // 暂不支持ref和out
            if (hasRefOrOut(fun))
                continue;
            // 暂不支持params参数
            int s;
            if (hasParamsArray(fun, out s))
                continue;
            // 暂不支持泛型方法
            if (fun.IsGenericMethod)
                continue;
            if (!fun.IsStatic && isNeedlessMemberFun(fun))
                continue;
            if (!fun.IsStatic && isStatic(type))
                continue;
            if (isStatic(type) && isNeedlessStaticFun(fun))
                continue;
            // 暂不支持decimal类型参数或返回值的方法
            if (hasType(fun, typeof(decimal)))
                continue;

            if (!dictResult.ContainsKey(fun.Name))
                dictResult.Add(fun.Name, new List<MethodInfo>());
            List<MethodInfo> list = dictResult[fun.Name];
            list.Add(fun);
        }

        foreach (var pair in dictResult)
        {
            pair.Value.Sort(delegate(MethodInfo small, MethodInfo big)
            {
                int s1, s2;
                if (!hasDefaultParam(small, out s1) && hasDefaultParam(big, out s2))
                    return -1;
                if (hasDefaultParam(small, out s1) && !hasDefaultParam(big, out s2))
                    return 1;
                if (!hasParamsArray(small, out s1) && hasParamsArray(big, out s2))
                    return -1;
                if (hasParamsArray(small, out s1) && !hasParamsArray(big, out s2))
                    return 1;
                if (small.GetParameters().Length < big.GetParameters().Length)
                    return -1;
                if (small.GetParameters().Length > big.GetParameters().Length)
                    return 1;

                // 根据参数类型排序
                ParameterInfo[] sps = small.GetParameters();
                ParameterInfo[] bps = big.GetParameters();
                for (int i = 0, len = sps.Length; i < len; i++)
                {
                    int ret = compareTypePriority(sps[i].ParameterType, bps[i].ParameterType);
                    if (ret != 0)
                        return ret;
                }

                return 0;
            });
        }

        return dictResult;
    }

    static string funCall(MethodBase fun, int paramCount = -1)
    {
        string code = (isConstructor(fun) ? toCSLightName(fun.DeclaringType) : fun.Name) + "(";
        ParameterInfo[] ps = fun.GetParameters();
        if (paramCount < 0)
            paramCount = ps.Length;
        for (int i = 0; i < paramCount; i++)
        {
            code += covertTo(ps[i].ParameterType, string.Format("_params[{0}].value", i));
            if (i + 1 < paramCount)
                code += ", ";
        }
        code += ")";
        return code;
    }

    static string funMatch(MethodBase fun, bool isAllSameCount, bool isAllSameType, bool isAllConvertible, bool hasFloatAndDouble, int paramCount = -1)
    {
        if (paramCount >= 0)
            return "if (_params.size == " + paramCount.ToString() + ")";

        string code = "";
        ParameterInfo[] ps = fun.GetParameters();
        paramCount = ps.Length;

        if (!isAllSameCount)
        {
            code = "_params.size == " + paramCount.ToString();
        }

        if (!isAllSameType)
        {
            for (int i = 0; i < paramCount; i++)
            {
                Type pt = ps[i].ParameterType;
                if (!pt.Equals(typeof(object)))
                {
                    if (!string.IsNullOrEmpty(code))
                        code += " && ";

                    if (!pt.IsClass)
                        code += string.Format("(_params[{0}].value is {1})", i, isMatch(pt, isAllConvertible, hasFloatAndDouble));
                    else
                        code += string.Format("(_params[{0}].value == null || _params[{0}].value is {1})", i, isMatch(pt, isAllConvertible, hasFloatAndDouble));
                }
            }
        }
        return "if (" + code + ")";
    }

    static string funBlock(List<MethodInfo> funs, Type type, bool isStatic, List<MethodInfo> funsOri)
    {
        string callThis = (isStatic ? "{0}.{1};" : "(({0})object_this).{1};");

        StringBuilder blockBuilder = new StringBuilder();
        Action<string, MethodInfo, int> action = (head, fun, paramCount) =>
        {
            if (fun.ReturnType.Equals(typeof(void)))
            {
                blockBuilder.AppendFormat(head + callThis, toCSLightName(type), funCall(fun, paramCount));
                blockBuilder.AppendLine();
                blockBuilder.AppendLine(head + "return CLS_Content.Value.Void;");
            }
            else
            {
                blockBuilder.AppendLine(head + "CLS_Content.Value val = new CLS_Content.Value();");
                blockBuilder.AppendFormat(head + "val.type = typeof({0});", toCSLightName(fun.ReturnType));
                blockBuilder.AppendLine();
                blockBuilder.AppendFormat(head + "val.value = " + callThis, toCSLightName(type), funCall(fun, paramCount));
                blockBuilder.AppendLine();
                blockBuilder.AppendLine(head + "return val;");
            }
        };

        int si;
        if (funsOri.Count == 1 && !hasDefaultParam(funsOri[0], out si))
        {
            action("\t\t\t\t", funs[0], funs[0].GetParameters().Length);
        }
        else
        {
            bool isAllSameCount = isAllParamsSameCount(funs.ToArray()) && funs.Count == funsOri.Count;
            bool isAllSameType = isAllParamsSameType(funs.ToArray()) && funs.Count == funsOri.Count;
            bool isAllConvertible = isAllParamsConvertible(funs.ToArray());
            bool hasFloatAndDouble = hasFloatAndDoubleParams(funs.ToArray());
            for (int i = 0; i < funs.Count; i++)
            {
                if (hasDefaultParam(funs[i], out si))
                {
                    int maxParamCount = funs[i].GetParameters().Length;
                    for (int j = si; j <= maxParamCount; j++)
                    {
                        blockBuilder.AppendFormat("\t\t\t\t" + "{1}{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble, j), i == 0 ? "" : "else ");
                        blockBuilder.AppendLine();
                        blockBuilder.AppendLine("\t\t\t\t" + "{");
                        action("\t\t\t\t\t", funs[i], j);
                        blockBuilder.AppendLine("\t\t\t\t" + "}");
                    }
                }
                else
                {
                    blockBuilder.AppendFormat("\t\t\t\t" + "{1}{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble), i == 0 ? "" : "else ");
                    blockBuilder.AppendLine();
                    blockBuilder.AppendLine("\t\t\t\t" + "{");
                    action("\t\t\t\t\t", funs[i], funs[i].GetParameters().Length);
                    blockBuilder.AppendLine("\t\t\t\t" + "}");
                }
            }
        }
        return blockBuilder.ToString();
    }

    static string funStructBlock(List<MethodInfo> funs, Type type, List<MethodInfo> funsOri)
    {
        StringBuilder blockBuilder = new StringBuilder();
        Action<string, MethodInfo, int> action = (head, fun, paramCount) =>
        {
            if (fun.ReturnType.Equals(typeof(void)))
            {
                blockBuilder.AppendFormat(head + "newVal.{0};", funCall(fun, paramCount));
                blockBuilder.AppendLine();
                blockBuilder.AppendLine(head + "retVal = CLS_Content.Value.Void;");
            }
            else
            {
                blockBuilder.AppendLine(head + "retVal = new CLS_Content.Value();");
                blockBuilder.AppendFormat(head + "retVal.type = typeof({0});", toCSLightName(fun.ReturnType));
                blockBuilder.AppendLine();
                blockBuilder.AppendFormat(head + "retVal.value = newVal.{0};", funCall(fun, paramCount));
                blockBuilder.AppendLine();
            }
        };

        int si;
        if (funsOri.Count == 1 && !hasDefaultParam(funsOri[0], out si))
        {
            action("\t\t\t\t", funs[0], funs[0].GetParameters().Length);
        }
        else
        {
            bool isAllSameCount = isAllParamsSameCount(funs.ToArray()) && funs.Count == funsOri.Count;
            bool isAllSameType = isAllParamsSameType(funs.ToArray()) && funs.Count == funsOri.Count;
            bool isAllConvertible = isAllParamsConvertible(funs.ToArray());
            bool hasFloatAndDouble = hasFloatAndDoubleParams(funs.ToArray());
            for (int i = 0; i < funs.Count; i++)
            {
                if (hasDefaultParam(funs[i], out si))
                {
                    int maxParamCount = funs[i].GetParameters().Length;
                    for (int j = si; j <= maxParamCount; j++)
                    {
                        blockBuilder.AppendFormat("\t\t\t\t" + "{1}{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble, j), i == 0 ? "" : "else ");
                        blockBuilder.AppendLine();
                        blockBuilder.AppendLine("\t\t\t\t" + "{");
                        action("\t\t\t\t\t", funs[i], j);
                        blockBuilder.AppendLine("\t\t\t\t" + "}");
                    }
                }
                else
                {
                    blockBuilder.AppendFormat("\t\t\t\t" + "{1}{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble), i == 0 ? "" : "else ");
                    blockBuilder.AppendLine();
                    blockBuilder.AppendLine("\t\t\t\t" + "{");
                    action("\t\t\t\t\t", funs[i], funs[i].GetParameters().Length);
                    blockBuilder.AppendLine("\t\t\t\t" + "}");
                }
            }
        }
        return blockBuilder.ToString();
    }

    static List<MethodInfo> twoOpFuns(MethodInfo[] sFuns, string op)
    {
        List<MethodInfo> list = new List<MethodInfo>();
        foreach (var sFun in sFuns)
        {
            if (isObsolete(sFun))
                continue;
            if (!isOperator(sFun))
                continue;
            if (!isLeftValueSelf(sFun))
                continue;
            if (sFun.GetParameters().Length != 2)
                continue;

            if (op == "+" && sFun.Name == "op_Addition")
                list.Add(sFun);
            if (op == "-" && sFun.Name == "op_Subtraction")
                list.Add(sFun);
            if (op == "*" && sFun.Name == "op_Multiply")
                list.Add(sFun);
            if (op == "/" && sFun.Name == "op_Division")
                list.Add(sFun);
            if (op == "%" && sFun.Name == "op_Modulus")
                list.Add(sFun);
            if (op == "==" && sFun.Name == "op_Equality")
                list.Add(sFun);
            if (op == "!=" && sFun.Name == "op_Inequality")
                list.Add(sFun);
            if (op == ">" && sFun.Name == "op_GreaterThan")
                list.Add(sFun);
            if (op == ">=" && sFun.Name == "op_GreaterThanOrEqual")
                list.Add(sFun);
            if (op == "<" && sFun.Name == "op_LessThan")
                list.Add(sFun);
            if (op == "<=" && sFun.Name == "op_LessThanOrEqual")
                list.Add(sFun);
        }
        return list;
    }

    static string twoOpCall(MethodInfo fun, string op)
    {
        ParameterInfo[] ps = fun.GetParameters();
        return string.Format("{0} {1} {2}", covertTo(ps[0].ParameterType, "left"), op, covertTo(ps[1].ParameterType, "right.value"));
    }

    static string twoOpMatch(MethodInfo fun)
    {
        ParameterInfo[] ps = fun.GetParameters();
        return string.Format("if (right.value is {0})", toCSLightName(ps[1].ParameterType));
    }

    static string twoOpLogic(string op)
    {
        if (op == "==")return "logictoken.equal";
        if (op == "!=") return "logictoken.not_equal";
        if (op == ">") return "logictoken.more";
        if (op == ">=") return "logictoken.more_equal";
        if (op == "<") return "logictoken.less";
        if (op == "<=") return "logictoken.less_equal";
        return op;
    }

    static string twoOpBlock(List<MethodInfo> funs, string op)
    {
        bool isLogic = (op == "==" || op == "!=" || op == ">" || op == "<" || op == ">=" || op == "<=");
        StringBuilder blockBuilder = new StringBuilder();
        if (funs.Count > 0)
        {
            if (!isLogic)
                blockBuilder.AppendFormat("\t\t" + "if (code == '{0}')", op);
            else
                blockBuilder.AppendFormat("\t\t" + "if (code == {0})", twoOpLogic(op));
            blockBuilder.AppendLine();
            blockBuilder.AppendLine("\t\t" + "{");
            if (funs.Count == 1 && (op != "+"))
            {
                if (!isLogic)
                {
                    blockBuilder.AppendFormat("\t\t\t" + "returntype = typeof({0});", toCSLightName(funs[0].ReturnType));
                    blockBuilder.AppendLine();
                }
                blockBuilder.AppendFormat("\t\t\t" + "return {0};", twoOpCall(funs[0], op));
                blockBuilder.AppendLine();
            }
            else
            {
                for (int i = 0; i < funs.Count; i++)
                {
                    blockBuilder.AppendFormat("\t\t\t" + "{0}", twoOpMatch(funs[i]));
                    blockBuilder.AppendLine();
                    blockBuilder.AppendLine("\t\t\t" + "{");
                    if (!isLogic)
                    {
                        blockBuilder.AppendFormat("\t\t\t\t" + "returntype = typeof({0});", toCSLightName(funs[i].ReturnType));
                        blockBuilder.AppendLine();
                    }
                    blockBuilder.AppendFormat("\t\t\t\t" + "return {0};", twoOpCall(funs[i], op));
                    blockBuilder.AppendLine();
                    blockBuilder.AppendLine("\t\t\t" + "}");
                }
            }
            blockBuilder.AppendLine("\t\t" + "}");
        }
        return blockBuilder.ToString();
    }

    static string newBlock(ConstructorInfo[] funs, Type type)
    {
        StringBuilder blockBuilder = new StringBuilder();
        Action<string, ConstructorInfo, int> action = (head, fun, paramCount) =>
        {
            blockBuilder.AppendLine(head + "CLS_Content.Value val = new CLS_Content.Value();");
            blockBuilder.AppendFormat(head + "val.type = typeof({0});", toCSLightName(type));
            blockBuilder.AppendLine();
            blockBuilder.AppendFormat(head + "val.value = new {0};", funCall(fun, paramCount));
            blockBuilder.AppendLine();
            blockBuilder.AppendLine(head + "return val;");
        };

        int si;
        if (funs.Length == 1 && !hasDefaultParam(funs[0], out si))
        {
            action("\t\t\t", funs[0], funs[0].GetParameters().Length);
        }
        else
        {
            bool isAllSameCount = isAllParamsSameCount(funs);
            bool isAllSameType = isAllParamsSameType(funs);
            bool isAllConvertible = isAllParamsConvertible(funs);
            bool hasFloatAndDouble = hasFloatAndDoubleParams(funs);
            for (int i = 0; i < funs.Length; i++)
            {
                if (hasDefaultParam(funs[i], out si))
                {
                    int maxParamCount = funs[i].GetParameters().Length;
                    for (int j = si; j <= maxParamCount; j++)
                    {
                        blockBuilder.AppendFormat("\t\t\t" + "{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble, j));
                        blockBuilder.AppendLine();
                        blockBuilder.AppendLine("\t\t\t" + "{");
                        action("\t\t\t\t", funs[i], j);
                        blockBuilder.AppendLine("\t\t\t" + "}");
                    }
                }
                else
                {
                    blockBuilder.AppendFormat("\t\t\t" + "{0}", funMatch(funs[i], isAllSameCount, isAllSameType, isAllConvertible, hasFloatAndDouble));
                    blockBuilder.AppendLine();
                    blockBuilder.AppendLine("\t\t\t" + "{");
                    action("\t\t\t\t", funs[i], funs[i].GetParameters().Length);
                    blockBuilder.AppendLine("\t\t\t" + "}");
                }
            }
        }
        return blockBuilder.ToString();
    }

    static MemberInfo[] sortMemberInfos(MemberInfo[] src, Type type)
    {
        List<MemberInfo> list = new List<MemberInfo>();
        list.AddRange(src);
        list.Sort(delegate(MemberInfo small, MemberInfo big)
        {
            if (small.DeclaringType == type && big.DeclaringType != type)
                return -1;
            if (small.DeclaringType != type && big.DeclaringType == type)
                return 1;

            return string.Compare(small.Name, big.Name);
        });
        return list.ToArray();
    }

    //=======================转换函数===========================
    void toCSLight(Type type, BindingFlags flags, bool smartInherit = false)
    {
        string code = System.IO.File.ReadAllText(ResTemplatePath, Encoding.UTF8);

        replaceTypeName(type, ref flags, ref code, smartInherit);
        replaceConvertTo(type, flags, ref code);
        replaceMathLogic(type, flags, ref code);
        replaceMath2Value(type, flags, ref code);
        replaceNew(type, flags, ref code);
        replaceStaticCall(type, flags, ref code);
        replaceStaticValueGet(type, flags, ref code);
        replaceStaticValueSet(type, flags, ref code);
        replaceMemberCall(type, flags, ref code);
        replaceMemberValueGet(type, flags, ref code);
        replaceMemberValueSet(type, flags, ref code);
        replaceIndexGet(type, flags, ref code);
        replaceIndexSet(type, flags, ref code);

        code = code.Replace("\r\n\r\n\r\n\r\n\r\n",             "\r\n\r\n");
        code = code.Replace("\r\n\r\n\r\n\r\n",                 "\r\n\r\n");
        code = code.Replace("\r\n\r\n\r\n",                     "\r\n\r\n");

        code = code.Replace("\n\n\n\n\n",                         "\n\n");
        code = code.Replace("\n\n\n\n",                         "\n\n");
        code = code.Replace("\n\n\n",                           "\n\n");

        code = code.Replace("\r\n\r\n\r\n        (",            "\r\n        (");
        code = code.Replace("\r\n\r\n\r\n        }",            "\r\n        }");
        code = code.Replace("\r\n\r\n\r\n    }",                "\r\n    }");
        code = code.Replace("\r\n\r\n\r\n            else",     "\r\n            else");
        code = code.Replace("\r\n\r\n        (",                "\r\n        (");
        code = code.Replace("\r\n\r\n        }",                "\r\n        }");
        code = code.Replace("\r\n\r\n    }",                    "\r\n    }");
        code = code.Replace("\r\n\r\n            else",         "\r\n            else");

        code = code.Replace("\n\n\n        (",                  "\n        (");
        code = code.Replace("\n\n\n        }",                  "\n        }");
        code = code.Replace("\n\n\n    }",                      "\n    }");
        code = code.Replace("\n\n\n            else",           "\n            else");
        code = code.Replace("\n\n        (",                    "\n        (");
        code = code.Replace("\n\n        }",                    "\n        }");
        code = code.Replace("\n\n    }",                        "\n    }");
        code = code.Replace("\n\n            else",             "\n            else");

        code = code.Replace("{\r\n\r\n\r\n",                    "{\r\n");
        code = code.Replace("{\r\n\r\n",                        "{\r\n");

        code = code.Replace("{\n\n\n",                          "{\n");
        code = code.Replace("{\n\n",                            "{\n");

        System.IO.File.WriteAllText(ToCSLightPathPrefix + type.Name + ".cs", code, Encoding.UTF8);
    }

    string removeString(string code, string startStr, string endStr)
    {
        int indexStart = code.IndexOf(startStr);
        int indexEnd = code.IndexOf(endStr) + endStr.Length;
        return code.Remove(indexStart, indexEnd - indexStart);
    }

    void replaceTypeName(Type type, ref BindingFlags flags, ref string code, bool smartInherit)
    {
        code = code.Replace("/*type_name*/", type.Name);
        code = code.Replace("/*type*/", toCSLightName(type));

        // 继承替换
        Type parent = smartInherit ? type.BaseType : null;

        if (parent == typeof(System.Object))
            parent = null;

        if (parent != null && !isFileExisted(ToCSLightPathPrefix + parent.Name + ".cs"))
            parent = null;

        if (parent == null)
        {
            code = code.Replace("/*inherit_type*/", "RegHelper_Type");
            code = code.Replace("/*inherit_fun*/", "RegHelper_TypeFunction");
        }
        else
        {
            code = code.Replace("/*inherit_type*/", "ToCSLight" + parent.Name);
            code = code.Replace("/*inherit_fun*/", "ToCSLight" + parent.Name + ".ToCSLight" + parent.Name + "_Fun");
            flags &= ~BindingFlags.FlattenHierarchy;
            if (flags == BindingFlags.Default)
                flags = BindingFlags.DeclaredOnly;
        }
    }

    void replaceConvertTo(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        blockBuilder.AppendFormat("\t\t" + "return {0};", covertTo(type, "src"));
        blockBuilder.AppendLine();
        code = code.Replace("//ConvertTo", blockBuilder.ToString());
    }

    void replaceMathLogic(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var sFuns = type.GetMethods(BindingFlags.Public | BindingFlags.Static | flags);
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "=="), "=="));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "!="), "!="));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, ">"), ">"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, ">="), ">="));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "<"), "<"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "<="), "<="));

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//MathLogic", blockBuilder.ToString());
            code = code.Replace("/*MathLogic start*/", "");
            code = code.Replace("/*MathLogic end*/", "");
        }
        else
        {
            code = removeString(code, "/*MathLogic start*/", "/*MathLogic end*/");
        }
    }

    void replaceMath2Value(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var sFuns = type.GetMethods(BindingFlags.Public | BindingFlags.Static | flags);
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "+"), "+"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "-"), "-"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "*"), "*"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "/"), "/"));
        blockBuilder.Append(twoOpBlock(twoOpFuns(sFuns, "%"), "%"));

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//Math2Value", blockBuilder.ToString());
            code = code.Replace("/*Math2Value start*/", "");
            code = code.Replace("/*Math2Value end*/", "");
        }
        else
        {
            code = removeString(code, "/*Math2Value start*/", "/*Math2Value end*/");
        }
    }

    void replaceNew(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        List<ConstructorInfo> constructors = new List<ConstructorInfo>();
        foreach (var constructor in type.GetConstructors())
        {
            if (isObsolete(constructor))
                continue;
            constructors.Add(constructor);
        }

        blockBuilder.Append(newBlock(constructors.ToArray(), type));

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//New", blockBuilder.ToString());
            code = code.Replace("/*New start*/", "");
            code = code.Replace("/*New end*/", "");

            if (constructors.Count <= 1)
            {
                code = removeString(code, "//one_constructor_start", "//one_constructor_end");
            }
            else
            {
                code = code.Replace("//one_constructor_start", "");
                code = code.Replace("//one_constructor_end", "");
            }
        }
        else
        {
            code = removeString(code, "/*New start*/", "/*New end*/");
        }
    }

    void replaceStaticCall(Type type, BindingFlags flags, ref string code)
    {
        bool hasCode = false;
        StringBuilder blockBuilder = new StringBuilder();
        var sFuns = getMethods(type, BindingFlags.Public | BindingFlags.Static | flags);
        Dictionary<string, List<MethodInfo>> dictFuns = sameNameFuns(type, sFuns);
        Dictionary<string, List<MethodInfo>> dictFunsOri = sameNameFunsOri(type, sFuns);
        foreach (var pair in dictFuns)
        {
            blockBuilder.AppendFormat("\t\t\t" + "{1}if (function == \"{0}\")", pair.Key, !hasCode ? "" : "else ");
            blockBuilder.AppendLine();
            blockBuilder.AppendLine("\t\t\t" + "{");
            blockBuilder.Append(funBlock(pair.Value, type, true, dictFunsOri[pair.Key]));
            blockBuilder.AppendLine("\t\t\t" + "}");
            hasCode = true;
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//StaticCall", blockBuilder.ToString());
            code = code.Replace("/*StaticCall start*/", "");
            code = code.Replace("/*StaticCall end*/", "");
        }
        else
        {
            code = removeString(code, "/*StaticCall start*/", "/*StaticCall end*/");
        }
    }

    void replaceStaticValueGet(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var sMems = getMembers(type, BindingFlags.Public | BindingFlags.Static | flags);
        foreach (var sMem in sMems)
        {
            if (!canRead(sMem))
                continue;
            blockBuilder.AppendFormat("\t\t\t" + "if (valuename == \"{0}\")", sMem.Name);
            blockBuilder.AppendLine();
            blockBuilder.AppendFormat("\t\t\t\t" + "return new CLS_Content.Value() {{ type = typeof({2}), value = {0}.{1} }};",
                toCSLightName(type), sMem.Name, toCSLightName(valType(sMem)));
            blockBuilder.AppendLine();
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//StaticValueGet", blockBuilder.ToString());
            code = code.Replace("/*StaticValueGet start*/", "");
            code = code.Replace("/*StaticValueGet end*/", "");
        }
        else
        {
            code = removeString(code, "/*StaticValueGet start*/", "/*StaticValueGet end*/");
        }
    }

    void replaceStaticValueSet(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var sMems = getMembers(type, BindingFlags.Public | BindingFlags.Static | flags);
        foreach (var sMem in sMems)
        {
            if (!canWrite(sMem))
                continue;

            blockBuilder.AppendFormat("\t\t\t" + "if (valuename == \"{0}\")", sMem.Name);
            blockBuilder.AppendLine();
            blockBuilder.AppendLine("\t\t\t" + "{");
            blockBuilder.AppendFormat("\t\t\t\t" + "{0}.{1} = {2};", toCSLightName(type), sMem.Name, covertTo(valType(sMem), "value"));
            blockBuilder.AppendLine();
            blockBuilder.AppendLine("\t\t\t\t" + "return;");
            blockBuilder.AppendLine("\t\t\t" + "}");
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//StaticValueSet", blockBuilder.ToString());
            code = code.Replace("/*StaticValueSet start*/", "");
            code = code.Replace("/*StaticValueSet end*/", "");
        }
        else
        {
            code = removeString(code, "/*StaticValueSet start*/", "/*StaticValueSet end*/");
        }
    }

    void replaceMemberCall(Type type, BindingFlags flags, ref string code)
    {
        bool hasCode = false;
        StringBuilder blockBuilder = new StringBuilder();
        GUI.color = Color.yellow;
        var mFuns = getMethods(type, BindingFlags.Public | BindingFlags.Instance | flags);
        Dictionary<string, List<MethodInfo>> dictFuns = sameNameFuns(type, mFuns);
        Dictionary<string, List<MethodInfo>> dictFunsOri = sameNameFunsOri(type, mFuns);
        foreach (var pair in dictFuns)
        {
            if (type.IsClass)
            {
                blockBuilder.AppendFormat("\t\t\t" + "{1}if (function == \"{0}\")", pair.Key, !hasCode ? "" : "else ");
                blockBuilder.AppendLine();
                blockBuilder.AppendLine("\t\t\t" + "{");
                blockBuilder.Append(funBlock(pair.Value, type, false, dictFunsOri[pair.Key]));
                blockBuilder.AppendLine("\t\t\t" + "}");
            }
            else
            {
                if (!hasCode)
                {
                    blockBuilder.AppendFormat("\t\t\t" + "{0} newVal = ({0})object_this;", toCSLightName(type));
                    blockBuilder.AppendLine();
                }
                blockBuilder.AppendFormat("\t\t\t" + "{1}if (function == \"{0}\")", pair.Key, !hasCode ? "" : "else ");
                blockBuilder.AppendLine();
                blockBuilder.AppendLine("\t\t\t" + "{");
                blockBuilder.Append(funStructBlock(pair.Value, type, dictFunsOri[pair.Key]));
                blockBuilder.AppendLine("\t\t\t" + "}");
            }
            hasCode = true;
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//MemberCall", blockBuilder.ToString());
            code = code.Replace("/*MemberCall start*/", "");
            code = code.Replace("/*MemberCall end*/", "");

            if (!hasCode || type.IsClass)
            {
                code = removeString(code, "//struct_start_MemberCall", "//struct_end_MemberCall");
                code = removeString(code, "//struct_start_MemberCall", "//struct_end_MemberCall");
            }
            else
            {
                code = code.Replace("//struct_start_MemberCall", "");
                code = code.Replace("//struct_end_MemberCall", "");
            }
        }
        else
        {
            code = removeString(code, "/*MemberCall start*/", "/*MemberCall end*/");
        }
    }

    void replaceMemberValueGet(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var mMems = getMembers(type, BindingFlags.Public | BindingFlags.Instance | flags);
        mMems = sortMemberInfos(mMems, type);
        foreach (var mMem in mMems)
        {
            if (!canRead(mMem))
                continue;
            if (isIndex(mMem))
                continue;

            blockBuilder.AppendFormat("\t\t\t" + "if (valuename == \"{0}\")", mMem.Name);
            blockBuilder.AppendLine();
            blockBuilder.AppendFormat("\t\t\t\t" + "return new CLS_Content.Value() {{ type = typeof({2}), value = (({0})object_this).{1} }};",
                toCSLightName(type), mMem.Name, toCSLightName(valType(mMem)));
            blockBuilder.AppendLine();
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//MemberValueGet", blockBuilder.ToString());
            code = code.Replace("/*MemberValueGet start*/", "");
            code = code.Replace("/*MemberValueGet end*/", "");
        }
        else
        {
            code = removeString(code, "/*MemberValueGet start*/", "/*MemberValueGet end*/");
        }
    }

    void replaceMemberValueSet(Type type, BindingFlags flags, ref string code)
    {
        bool hasCode = false;

        StringBuilder blockBuilder = new StringBuilder();
        var mMems = getMembers(type, BindingFlags.Public | BindingFlags.Instance | flags);
        mMems = sortMemberInfos(mMems, type);
        foreach (var mMem in mMems)
        {
            if (!canWrite(mMem))
                continue;
            if (isIndex(mMem))
                continue;

            if (type.IsClass)
            {
                blockBuilder.AppendFormat("\t\t\t" + "if (valuename == \"{0}\")", mMem.Name);
                blockBuilder.AppendLine();
                blockBuilder.AppendLine("\t\t\t" + "{");
                blockBuilder.AppendFormat("\t\t\t\t" + "(({0})object_this).{1} = {2};", toCSLightName(type), mMem.Name, covertTo(valType(mMem), "value"));
                blockBuilder.AppendLine();
                blockBuilder.AppendLine("\t\t\t\t" + "return;");
                blockBuilder.AppendLine("\t\t\t" + "}");
            }
            else
            {
                if (!hasCode)
                {
                    blockBuilder.AppendFormat("\t\t\t" + "{0} newVal = ({0})object_this;", toCSLightName(type));
                    blockBuilder.AppendLine();
                }
                blockBuilder.AppendFormat("\t\t\t" + "{1}if (valuename == \"{0}\")", mMem.Name, !hasCode ? "" : "else ");
                blockBuilder.AppendLine();
                blockBuilder.AppendFormat("\t\t\t\t" + "newVal.{0} = {1};", mMem.Name, covertTo(valType(mMem), "value"));
                blockBuilder.AppendLine();
            }
            hasCode = true;
        }

        if (blockBuilder.Length > 0)
        {
            code = code.Replace("//MemberValueSet", blockBuilder.ToString());
            code = code.Replace("/*MemberValueSet start*/", "");
            code = code.Replace("/*MemberValueSet end*/", "");

            if (!hasCode || type.IsClass)
            {
                code = removeString(code, "//struct_start_MemberValueSet", "//struct_end_MemberValueSet");
            }
            else
            {
                code = code.Replace("//struct_start_MemberValueSet", "");
                code = code.Replace("//struct_end_MemberValueSet", "");
            }
        }
        else
        {
            code = removeString(code, "/*MemberValueSet start*/", "/*MemberValueSet end*/");
        }
    }

    void replaceIndexGet(Type type, BindingFlags flags, ref string code)
    {
        StringBuilder blockBuilder = new StringBuilder();
        var mMems = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var mMem in mMems)
        {
            if (!isVal(mMem))
                continue;
            if (!canRead(mMem))
                continue;
            if (isObsolete(mMem))
                continue;
            if (!isIndex(mMem))
                continue;

            var gt = type.GetMethod("get_Item");
            if (gt == null)
                gt = type.GetMethod("get_Chars");
            blockBuilder.AppendFormat("\t\t\t" + "return new CLS_Content.Value() {{ type = typeof({2}), value = (({0})object_this)[{1}] }};",
                toCSLightName(type), covertTo(gt.GetParameters()[0].ParameterType, "key"), toCSLightName(valType(mMem)));
            blockBuilder.AppendLine();
            code = code.Replace("//IndexGet", blockBuilder.ToString());
            code = code.Replace("/*IndexGet start*/", "");
            code = code.Replace("/*IndexGet end*/", "");
            return;
        }
        code = removeString(code, "/*IndexGet start*/", "/*IndexGet end*/");
    }

    void replaceIndexSet(Type type, BindingFlags flags, ref string code)
    {
        // struct 类型受装箱拆箱限制，只能用反射来实现
        if (!type.IsClass)
        {
            code = removeString(code, "/*IndexSet start*/", "/*IndexSet end*/");
            return;
        }
        StringBuilder blockBuilder = new StringBuilder();
        var mMems = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var mMem in mMems)
        {
            if (!isVal(mMem))
                continue;
            if (!canWrite(mMem))
                continue;
            if (isObsolete(mMem))
                continue;
            if (!isIndex(mMem))
                continue;

            var gt = type.GetMethod("get_Item");
            if (gt == null)
                gt = type.GetMethod("get_Chars");
            blockBuilder.AppendFormat("\t\t\t" + "(({0})object_this)[{1}] = {2};",
                toCSLightName(type), covertTo(gt.GetParameters()[0].ParameterType, "key"), covertTo(valType(mMem), "value"));
            blockBuilder.AppendLine();
            code = code.Replace("//IndexSet", blockBuilder.ToString());
            code = code.Replace("/*IndexSet start*/", "");
            code = code.Replace("/*IndexSet end*/", "");
            return;
        }
        code = removeString(code, "/*IndexSet start*/", "/*IndexSet end*/");
    }
}
