//using Cysharp.Threading.Tasks;
using Saber.UI;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
#region
//保持UTF-8
#endregion
public static class UIUtil
{
    /// <summary>
    /// 默认Resource加载
    /// </summary>
    public static readonly string AboutPath = Path.Combine("UI","About.asset");

    //public static UIAbout About=
    //public static async UniTask LocalScaleSize(this TMP_Text text, Vector3 target,float duration,bool isLoop)
    //{
    //    if (duration <= 0) return;
    //    Vector3 origin = text.transform.localScale;
    //    float timer = 0;
    //    while (timer<duration)
    //    {
    //        var deltaTime = Time.deltaTime;
    //        timer += deltaTime;
    //        text.transform.localScale=Vector3.Lerp(text.transform.localScale, target, deltaTime/duration);
    //        await UniTask.WaitForSeconds(deltaTime);
    //    }
    //    text.transform.localScale=target;
    //    if (isLoop)
    //        text.LocalScaleSize(origin, duration, isLoop);
    //}


    public static UIAbout LoadAbout()
    {
        UIAbout res = null;
#if UNITY_EDITOR
        var path = Path.Combine("Assets", "Resources", AboutPath);
        res = AssetDatabase.LoadAssetAtPath<UIAbout>(path);
        if (res == null)
        {
            var asset = ScriptableObject.CreateInstance<UIAbout>();
            asset.UIScriptTemplate = UIUtil.FindFile(Application.dataPath, "UIGenerateClass.bytes")?.FullName;
            if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            AssetDatabase.CreateAsset(asset, path);//在传入的路径中创建资源
            AssetDatabase.SaveAssets(); //存储资源
            AssetDatabase.Refresh(); //刷新
            res = asset;
        }
        //return res;
#else
        res = Resources.Load<UIAbout>(AboutPath);
#endif

        return res;
    }
    public static int StringToHash(string name)=>Animator.StringToHash(name);

    /// <summary>
    /// 需要带拓展名的
    /// </summary>
    /// <param name="rootDire"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static FileInfo FindFile(string rootDire,string fileName)
    {
        //Debug.Log(rootDire);
        if (Directory.Exists(rootDire) == false) return null;
        DirectoryInfo root= new DirectoryInfo(rootDire);
        foreach(var v in root.GetFiles())
        {
            //Debug.Log($"filname{v.Name},targetname{fileName}");
            if(v.Name==fileName)
                return v;
        }
        foreach(var v in root.GetDirectories())
        {
            var res= FindFile(v.FullName, fileName);
            if (res != null) return res;
        }
        return null;
    }

    internal static string GetUIType(string uIName, UIAbout about)
    {
        foreach (var v in about.aboutItems)
        {
            if (v.ItemName == uIName)
            {
                if (string.IsNullOrEmpty(v.BehaviorName))
                    return v.BehaviorType.ToString();
                else
                    return v.BehaviorName;
            }
        }
        return null;
    }
}
