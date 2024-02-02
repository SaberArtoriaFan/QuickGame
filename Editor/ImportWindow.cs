using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
#region
//saber战棋
#endregion
namespace Saber 
{
    public class ImportWindow
    {
        public static string InitKey = "Saber_QuickGame_Init";

        [MenuItem("Saber/ImportAll",false,-1)]
        public static void ImportAll()
        {
            string path = Path.Combine("Packages", "com.saber.qgframe");
            if (!Directory.Exists(path))
            {
                path = Path.Combine("Packages", "QuickGame");
                if (!Directory.Exists(path))
                    Debug.LogError($"不存在路径{path},请使用packageManager导入本包或本地导入");
            }
            
            var dires = Directory.GetDirectories(path);
            foreach ( var dir in dires )
            {
                var n = Path.GetFileName(dir);
                var dirPath = Path.GetFullPath(dir);

                if (n == "Move~")
                {
                    string p = Path.GetFullPath(Application.dataPath);
                    Debug.Log($"开始导入,源文件夹:{dir},\n目标文件夹:{p}");
                    CopyFilesAndDirs(dir, p);
                    Debug.Log("结束导入");
                }else if (n == "Packages~")
                {
                    string targetPath = Application.dataPath.Substring(0, Application.dataPath.Length - ("Assets").Length);
                    targetPath= Path.GetFullPath(Path.Combine(targetPath,"Packages"));
                    Debug.Log($"开始导入,源文件夹:{dir},\n目标文件夹:{targetPath}");
                    CopyFilesAndDirs(dir, targetPath);
                    Debug.Log("结束导入");
                }

            }
            DefineEditor.SetAllTargetDefine();
            AssetDatabase.Refresh();

        }

        [InitializeOnLoadMethod]
        public static void FlagToInit()
        {
#if SABER_INIT
            return;
#endif
            if (EditorUtility.DisplayDialog("欢迎使用SaberQuickGame", "第一次导入请初始化", "OK"))
            {
                ImportAll();
            }
        }

        public static void CopyFilesAndDirs(string srcDir, string destDir)
        {
            if (!Directory.Exists(srcDir))
            {
                Debug.LogError("拷贝源文件夹不存在！！！" + srcDir);
                return;
            }
            GetFilesAndDirs(srcDir, destDir);
            Debug.Log($"初始化完成！-->{destDir}");
        }

        public static void GetFilesAndDirs(string srcDir, string destDir)
        {
            string newPath;
            FileInfo fileInfo;
            Directory.CreateDirectory(destDir);//创建目标文件夹                                                  
            string[] files = Directory.GetFiles(srcDir);//获取源文件夹中的所有文件完整路径
            foreach (string path in files)          //遍历文件     
            {
                fileInfo = new FileInfo(path);
                newPath = Path.Combine(destDir, fileInfo.Name);
                Debug.Log($"复制:{path}-->{newPath}");
                File.Copy(path, newPath, true);
            }
            string[] dirs = Directory.GetDirectories(srcDir);
            foreach (string path in dirs)        //遍历文件夹
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                string newDir = Path.Combine(destDir, directory.Name);
                GetFilesAndDirs(path, newDir);
            }
        }
    }

}
