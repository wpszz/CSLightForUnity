using UnityEngine;
using UnityEditor;
using System.IO; 
using System.Collections.Generic;
using CSLE;

public class GenCSLightAssetBundle : EditorWindow
{
    static string ScriptFolder = Application.dataPath + "/CSLight/Editor/CSLogic";
    static string ScriptAssetBundleName = "scripts.unity3d";
    static string AssetBundleTempFolder = "Assets/AssetBundleTemp/";
    static string AssetBundlesRootFolderName = "AssetBundles";
    static string AssetBundlesOutputFolder
    {
        get
        {
            string path = Path.Combine(System.Environment.CurrentDirectory, AssetBundlesRootFolderName);
            return Path.Combine(path, GetPlatformFolderForEditor(EditorUserBuildSettings.activeBuildTarget));
        }
    }

    //===============================================================================================================

    Vector2 			m_vScrollPos = Vector2.zero;

	List<string> 		m_listPath = new List<string>();
	List<bool> 			m_listToggle = new List<bool>();
		
    bool                m_doGenerate = false;

#if UNITY_ANDROID
    static BuildTarget  s_buildTarget = BuildTarget.Android;
#elif UNITY_IPHONE
    static BuildTarget 	s_buildTarget = BuildTarget.iPhone;
#elif UNITY_WP8 || UNITY_WP_8_1
    static BuildTarget 	s_buildTarget = BuildTarget.WP8Player;
#else
    static BuildTarget  s_buildTarget = BuildTarget.StandaloneWindows;
#endif

    [MenuItem("CSLight/Build AssetBundle")]
	static void Init() 
	{
        GenCSLightAssetBundle wnd = ScriptableObject.CreateInstance<GenCSLightAssetBundle>();
		wnd.ShowUtility();

        wnd.titleContent = new GUIContent("GenCSLightAssetBundle");
		wnd.position = new Rect(500, 200, 800, 640);

        if (Directory.Exists(ScriptFolder))
        {
            string[] filePaths = Directory.GetFiles(ScriptFolder, "*.cs", SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                wnd.m_listPath.Add(filePath.Replace('\\', '/'));
                wnd.m_listToggle.Add(true);
            }
            wnd.m_listPath.Sort();
        }
	}

	void OnGUI()
	{
		m_vScrollPos = EditorGUILayout.BeginScrollView(m_vScrollPos);
		
		List<string> listPath = m_listPath;
		List<bool> listToggle = m_listToggle;
		
		GUIStyle iconStyle = new GUIStyle();
		iconStyle.margin = new RectOffset(4, 4, 2, 4);
		iconStyle.fixedWidth = 16.0f;
		iconStyle.fixedHeight = 16.0f;
		
		GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
		toggleStyle.margin = new RectOffset(4, 4, 2, 4);
		toggleStyle.fixedWidth = 16.0f;
		toggleStyle.fixedHeight = 16.0f;
		toggleStyle.contentOffset = new Vector2(20, 0);

		for (int i = 0; i < listPath.Count; i++)
		{			
			GUILayout.BeginHorizontal();
	
			listToggle[i] = GUILayout.Toggle(listToggle[i], "", toggleStyle);
            string simplePath = listPath[i].Replace(System.Environment.CurrentDirectory.Replace('\\', '/') + "/", "");
            GUILayout.Label(AssetDatabase.GetCachedIcon(simplePath), iconStyle);
			GUILayout.Label(listPath[i]);

			GUILayout.EndHorizontal();

			GUILayout.Space(1);
		}
		
		EditorGUILayout.EndScrollView();
		
		//========================================================================
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("All"))
		{
			for (int i = 0; i < listToggle.Count; i++)
			{
				listToggle[i] = true;
			}
		}
		
		if (GUILayout.Button("None"))
		{
			for (int i = 0; i < listToggle.Count; i++)
			{
				listToggle[i] = false;
			}		
		}
		
		if (GUILayout.Button("invert"))
		{
			for (int i = 0; i < listPath.Count; i++)
			{
				listToggle[i] = !listToggle[i];
			}		
		}

		s_buildTarget = (BuildTarget)EditorGUILayout.EnumPopup(s_buildTarget);

        if (GUILayout.Button("Generate..."))
		{
            m_doGenerate = true;
		}

		EditorGUILayout.EndHorizontal();
	}

    void Update()
    {
        if (m_doGenerate)
        {
            m_doGenerate = EditorUtility.DisplayDialog("", "Are you sure build?", "yes", "no");
        }

        if (m_doGenerate)
        {
            m_doGenerate = false;

            List<string> listPath = m_listPath;
            List<bool> listToggle = m_listToggle;

            List<string> tmpPaths = new List<string>();
            Dictionary<string, string> projectCodes = new Dictionary<string, string>();
            for (int i = 0; i < listPath.Count; i++)
            {
                if (listToggle[i])
                    projectCodes.Add(listPath[i], System.IO.File.ReadAllText(listPath[i]));
            }

            CheckFolder(AssetBundleTempFolder);

            if (projectCodes.Count > 0)
            {
                List<string> classNames = new List<string>();
                CSLE.CLS_Environment csleEnv = ToCSLight.CreateEnvironment();
                Dictionary<string, IList<CSLE.Token>> csleProject = csleEnv.Project_ParserToken(projectCodes);
                foreach (var pair in csleProject)
                {
                    string className = System.IO.Path.GetFileNameWithoutExtension(pair.Key);
                    string tmpPath = AssetBundleTempFolder + className + ".bytes";
                    using (var fs = System.IO.File.OpenWrite(tmpPath))
                    {
                        csleEnv.TokenToStream(pair.Value, fs);
                    }
                    tmpPaths.Add(tmpPath);
                    classNames.Add(className);
                }

                string cfgPath = AssetBundleTempFolder + "class.asset";
                StringHolder classHolder = ScriptableObject.CreateInstance<StringHolder>();
                classHolder.content = classNames.ToArray();
                tmpPaths.Add(cfgPath);
                AssetDatabase.CreateAsset(classHolder, cfgPath);
            }

            BuildByName(tmpPaths, ScriptAssetBundleName);

            EditorUtility.DisplayDialog("finish", "success done!", "ok");
        }
    }

    public void BuildByName(List<string> assetPaths, string assetBundleName)
    {
        CheckFolder(AssetBundleTempFolder);

        RemoveAssetBundleName(assetBundleName);

        AssetDatabase.Refresh();

        foreach (string assetPath in assetPaths)
        {
            SetAssetBundleName(assetPath, assetBundleName);
        }

        string keepPath1 = AssetBundlesOutputFolder + "/" + GetPlatformFolderForEditor(EditorUserBuildSettings.activeBuildTarget);
        string keepPath2 = keepPath1 + ".manifest";
        string tempPath1 = CopyFileToUnityTemp(keepPath1);
        string tempPath2 = CopyFileToUnityTemp(keepPath2);

        if (!Directory.Exists(AssetBundlesOutputFolder))
            Directory.CreateDirectory(AssetBundlesOutputFolder);

        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = assetBundleName;
        build.assetNames = assetPaths.ToArray();
        BuildPipeline.BuildAssetBundles(AssetBundlesOutputFolder, new AssetBundleBuild[] { build }, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        foreach (string assetPath in assetPaths)
        {
            RemoveFile(Application.dataPath + "/../" + assetPath);
        }

        RemoveAssetBundleName(assetBundleName);

        AssetDatabase.Refresh();

        if (!string.IsNullOrEmpty(tempPath1))
        {
            File.Copy(tempPath1, keepPath1, true);
            RemoveFolder(Path.GetDirectoryName(tempPath1));
        }
        if (!string.IsNullOrEmpty(tempPath2))
        {
            File.Copy(tempPath2, keepPath2, true);
            RemoveFolder(Path.GetDirectoryName(tempPath2));
        }

        RemoveFolder(AssetBundleTempFolder);

        AssetDatabase.Refresh();
    }

    static void CheckFolder(string filePath)
    {
        string dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    static void SetAssetBundleName(string assetPath, string assetBundleName)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath) as AssetImporter;
        if (assetImporter != null)
            assetImporter.assetBundleName = assetBundleName;
        else
            Debug.LogWarning("SetAssetBundleName failed: " + assetPath + " ---- " + assetBundleName);
    }

    static void RemoveAssetBundleName(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        foreach (string abName in AssetDatabase.GetAllAssetBundleNames())
        {
            if (abName == assetBundleName)
            {
                AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                break;
            }
        }
    }

    static string CopyFileToUnityTemp(string filePath)
    {
        if (!File.Exists(filePath))
            return "";

        string tempPath = System.Environment.CurrentDirectory + "/Temp/" + System.Guid.NewGuid() + "/" + Path.GetFileName(filePath);

        CheckFolder(tempPath);

        File.Copy(filePath, tempPath, true);

        return tempPath;
    }

    static void RemoveFile(string filePath)
    {
        try
        {
            System.IO.File.Delete(filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("RemoveFile failed: " + ex);
        }
    }

    static void RemoveFolder(string dirPath)
    {
        try
        {
            if (!Directory.Exists(dirPath))
                return;

            foreach (string file in Directory.GetFiles(dirPath))
            {
                File.Delete(file);
            }

            foreach (string subDir in Directory.GetDirectories(dirPath))
            {
                RemoveFolder(subDir);
            }

            Directory.Delete(dirPath);
        }
        catch (System.Exception)
        {
        }
    }

    static string GetPlatformFolderForEditor(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "IOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            case BuildTarget.WP8Player:
                return "WP8";
            default:
                return null;
        }
    }
}
