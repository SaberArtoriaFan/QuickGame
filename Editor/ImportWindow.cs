using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#region
//saber战棋
#endregion
namespace Saber 
{
    public class ImportWindow
    {
        [MenuItem("Tools/Saber/ImportAll",false,-1)]
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
                    IOUtil.CopyFilesAndDirs(dir, p);
                    Debug.Log("结束导入");
                }else if (n == "Packages~")
                {
                    string targetPath = Application.dataPath.Substring(0, Application.dataPath.Length - ("Assets").Length);
                    targetPath= Path.GetFullPath(Path.Combine(targetPath,"Packages"));
                    Debug.Log($"开始导入,源文件夹:{dir},\n目标文件夹:{targetPath}");
                    IOUtil.CopyFilesAndDirs(dir, targetPath);
                    Debug.Log("结束导入");
                }

            }
            AssetDatabase.Refresh();

        }
    }

}
