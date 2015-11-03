using UnityEngine;
using UnityEditor;
using System;
using System.IO; 
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using CSLE;

class WindowCommands : EditorWindow
{
    [MenuItem("CSLight/自定义脚本")]
    static void Init()
    {
        EditorWindow.GetWindow<WindowCommands>().Show();
    }

    protected string titleName
    {
        get
        {
            return "自定义脚本";
        }
    }

    protected float width
    {
        get
        {
            return 600;
        }
    }
    protected float height
    {
        get
        {
            return 400;
        }
    }

    protected bool isReady
    {
        get
        {
            return CSLightMng.hasInstance;
        }
    }

    protected string m_editScript = "";

    protected List<string> m_collectCmds = null;

    protected string m_collectPath = System.Environment.CurrentDirectory + "/commands.json";

    protected Vector2 m_scrollPos = Vector2.zero;

    void Awake()
    {
        titleContent = new GUIContent(titleName, AssetDatabase.GetCachedIcon("Assets/CSLight/Editor/Tools/WindowCommands.cs"));
        position = new Rect((Screen.currentResolution.width - width) / 2, (Screen.currentResolution.height - height) / 2, width, height);
    }

    void Update()
    {
        this.Repaint();
    }

    void OnGUI()
    {
        if (isReady)
        {
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, false, false);

            if (m_collectCmds == null || m_collectCmds.Count == 0)
            {
                LoadColloct();
            }

            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.green;
            bool run = GUILayout.Button("执行自定义脚本", GUILayout.Height(24f));
            GUI.color = Color.white;
            if (m_collectCmds.Count > 0)
            {
                string[] arrs = new string[m_collectCmds.Count];
                for (int i = 0; i < m_collectCmds.Count; i++)
                {
                    arrs[i] = m_collectCmds[i].Split('\n')[0].Replace(" ", "").Replace("/", "_");
                }
                int collectIndex = EditorGUILayout.Popup(0, arrs, GUI.skin.FindStyle("Popup"));
                if (collectIndex > 0)
                {
                    m_editScript = m_collectCmds[collectIndex];
                }
            }
            if (m_collectCmds.Contains(m_editScript))
            {
                GUI.color = Color.red;
                if (GUILayout.Button("取消收藏", GUILayout.Height(24f)))
                    ColloctScript(false);
                GUI.color = Color.white;
            }
            else
            {
                if (GUILayout.Button("收藏", GUILayout.Height(24f)))
                    ColloctScript(true);
            }
            EditorGUILayout.EndHorizontal();

            m_editScript = EditorGUILayout.TextArea(m_editScript, GUILayout.Height(120));

            if (run)
            {
                if (!string.IsNullOrEmpty(m_editScript))
                    CSLightMng.instance.Execute(m_editScript);
            }

            EditorGUILayout.EndScrollView();
        }
        else
        {
            GUI.color = Color.red;
            GUILayout.Label("资源未准备, 请先启动游戏...");
        }
    }

    void LoadColloct()
    {
        m_collectCmds = new List<string>();
        m_collectCmds.Add("脚本收藏列表");
        try
        {
            if (File.Exists(m_collectPath))
            {
                List<object> list = MiniJSON.Json.Deserialize(System.IO.File.ReadAllText(m_collectPath, Encoding.UTF8)) as List<object>;

                foreach (object obj in list)
                {
                    m_collectCmds.Add(obj.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    void ColloctScript(bool colloct)
    {
        if (!string.IsNullOrEmpty(m_editScript))
        {
            if (m_collectCmds[0] == m_editScript)
                return;

            if (colloct)
            {
                if (!m_collectCmds.Contains(m_editScript))
                {
                    m_collectCmds.Add(m_editScript);

                    string first = m_collectCmds[0];
                    m_collectCmds.RemoveAt(0);
                    m_collectCmds.Sort();
                    m_collectCmds.Insert(0, first);

                    System.IO.StreamWriter sw = System.IO.File.CreateText(m_collectPath);
                    sw.Write(MiniJSON.Json.Serialize(m_collectCmds));
                    sw.Close();
                }
            }
            else
            {
                if (m_collectCmds.Contains(m_editScript))
                {
                    m_collectCmds.Remove(m_editScript);

                    System.IO.StreamWriter sw = System.IO.File.CreateText(m_collectPath);
                    sw.Write(MiniJSON.Json.Serialize(m_collectCmds));
                    sw.Close();
                }
            }
        }
    }
}
