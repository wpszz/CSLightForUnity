using UnityEngine;
using System.IO;
using System.Collections;

public class Main : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // 加载脚本资源包
        string rootPath = Path.Combine(System.Environment.CurrentDirectory, "AssetBundles");
        rootPath = Path.Combine(rootPath, GetPlatformFolder(Application.platform));
        rootPath = rootPath.Replace("\\", "/");

        WWW scriptsManifestWWW = new WWW(string.Format("file://{0}/scripts.unity3d.manifest?dt={1}", rootPath, System.DateTime.UtcNow.Ticks));
        yield return scriptsManifestWWW;
        if (!string.IsNullOrEmpty(scriptsManifestWWW.error))
        {
            Debug.LogError("请先执行主界面菜单CSLight/Build AssetBundle: " + scriptsManifestWWW.error);
            yield break;
        }

        string scriptsHash128 = scriptsManifestWWW.text.Substring(scriptsManifestWWW.text.IndexOf("Hash: ") + 6, 32);
        WWW scriptsWWW = WWW.LoadFromCacheOrDownload(string.Format("file://{0}/scripts.unity3d", rootPath), Hash128.Parse(scriptsHash128), 0);
        yield return scriptsWWW;
        if (!string.IsNullOrEmpty(scriptsWWW.error))
        {
            Debug.LogError("请先执行主界面菜单CSLight/Build AssetBundle: " + scriptsWWW.error);
            yield break;
        }

        CSLightMng.instance.InitializeFromAssetBundle(scriptsWWW.assetBundle);

        // 运行脚本入口
        yield return CSLightMng.instance.RunScriptEntry("CSMapTestHelloWorld", null);
    }

    string GetPlatformFolder(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.OSXWebPlayer:
                return "WebPlayer";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            case RuntimePlatform.WP8Player:
                return "WP8";
            default:
                return null;
        }
    }
}