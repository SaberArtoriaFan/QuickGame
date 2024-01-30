using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
using System.Text;
using Object = UnityEngine.Object;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    
    public static class UIScriptsGenerate
    {
        //选中文件夹
        //OpenPrefabPath("Assets");
        //选中文件
        //OpenPrefabPath("Assets/Cube.prefab");
        public static void OpenPrefabPath(string selectPath)
        {
#if UNITY_EDITOR
            //加载想要选中的文件/文件夹
            Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(selectPath);
            //在Project面板标记高亮显示
            UnityEditor.EditorGUIUtility.PingObject(obj);
            //在Project面板自动选中，并在Inspector面板显示详情
            UnityEditor.Selection.activeObject = obj;
#endif
        }
#if UNITY_EDITOR
        public static string Create(string className, UICache cache, string savePath="")
        {
            var about = UIUtil.LoadAbout();
            string path = about?.UIScriptTemplate;
            if(string.IsNullOrEmpty(savePath)||Directory.Exists(savePath)==false)
                savePath = about.UIScriptSavePrefix;
            if(string.IsNullOrEmpty(path))
            {
                Debug.LogError("无法创建脚本，脚本模板位置缺失");
                return string.Empty;
            }
            if (string.IsNullOrEmpty(savePath))
            {
                Debug.LogError("无法创建脚本，脚本保存位置为空");
                return string.Empty;
            }
            //path = Path.GetFullPath(path);
            //var pref = Path.GetFullPath(Application.dataPath);
            //path = path.Replace(pref, "Assets");
            Debug.Log($"path:{path}");
            var text= AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
            text= text.Replace("#UICLASSNAME#", className);
            text=SetContent(text, cache,about);
            savePath = Path.Combine(Application.dataPath, savePath, $"{className}_Params.cs");
            if(Directory.Exists(Path.GetDirectoryName(savePath))==false)
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.WriteAllText(savePath, text);
            cache.scriptsSavePath = savePath;
            //AssetDatabase.Refresh();
            return savePath;
        }

        private static string SetContent(string text, UICache cache,UIAbout about)
        {
            StringBuilder fieldSB= new StringBuilder();
            StringBuilder methodSB= new StringBuilder();
            StringBuilder editorSB= new StringBuilder();
            Dictionary<string,string> classNameDict=new Dictionary<string, string>();
            foreach (var v in about.aboutItems)
            {
                if(string.IsNullOrEmpty(v.BehaviorName))
                    classNameDict.TryAdd(v.BehaviorType.ToString(), v.ClassFullName);
                else
                    classNameDict.TryAdd(v.BehaviorName, v.ClassFullName);


            }
            editorSB.AppendLine("var dict=new System.Collections.Generic.Dictionary<string,UnityEngine.GameObject>();");
            for (int i=0; i < cache.items.Length; i++)
            {

                if (classNameDict.TryGetValue(cache.items[i].UIAbb,out var fullName))
                {
                    fieldSB.AppendLine($"\t[System.NonSerialized]public {fullName} {cache.items[i].UIName};");
                    //methodSB.AppendLine($"\t{cache.items[i].UIName} = cache.items[{i}].UIModel.GetComponent<{fullName}>();");
                    methodSB.AppendLine($"\t{cache.items[i].UIName} = transform.Find(\"{cache.items[i].UIPath}\").GetComponent<{fullName}>();");
                    editorSB.AppendLine($"\tdict.Add(\"{cache.items[i].UIName}\",transform.Find(\"{cache.items[i].UIPath}\").gameObject);");
                }
            }
            editorSB.AppendLine("return dict;");


            text = text.Replace("#CLASSCONTENT#", fieldSB.ToString());
            text = text.Replace("#METHONDCONTENT#", methodSB.ToString());
            text=text.Replace("#ONEDITOTSHOW#",editorSB.ToString());
            return text;
        }
#endif

    }
}
