using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#region
//saber战棋
#endregion
public class IOUtil 
{
   public static void CopyFilesAndDirs(string srcDir, string destDir)
    {
        if (!Directory.Exists(srcDir))
        {
            Debug.LogError("拷贝源文件夹不存在！！！" + srcDir);
            return;
        }
        //bool isOn = false;
        //bool isNew = true;
        //if (!Directory.Exists(destDir))//若目标文件夹不存在
        //{
        //    isOn = true;
        //}
        //else
        //{
        //    if (EditorUtility.DisplayDialog("目录已经存在", "是否强制覆盖？\nConfig\\Datas下的内容将会全部保存", "是", "否"))
        //    {
        //        isOn = true;
        //        isNew = false;


        //        var dataPath = Path.Combine(destDir, "Config", "Datas");
        //        CopyDatasToTemp(dataPath);
        //        //var tempPath = Path.GetDirectoryName(destDir);
        //        //DirectoryInfo destDirInfo=new DirectoryInfo(destDir);
        //        //destDirInfo.MoveTo(tempPath);
        //        Directory.Delete(destDir, true);
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        GetFilesAndDirs(srcDir, destDir);
        Debug.Log($"初始化完成！-->{destDir}");
        //if (isNew == false)
        //{
        //    var tempPath = Path.Combine(Application.temporaryCachePath, "Luban", "Datas");
        //    if (Directory.Exists(tempPath))
        //        Directory.Delete(tempPath, true);
        //}
    }
   public  static void CopyDatasToTemp(string dataPath)
    {
        var tempPath = Path.Combine(Application.temporaryCachePath, "Luban", "Datas");
        if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
        GetFilesAndDirs(dataPath, tempPath);
    }
    public  static void GetFilesAndDirs(string srcDir, string destDir)
    {
        ////原有数据，就不要覆盖了
        //if (isNew == false && Path.GetFileName(destDir) == "Datas" && Path.GetFileName(Path.GetDirectoryName(destDir)) == "Config")
        //{
        //    var targetPath = Path.Combine(Application.temporaryCachePath, "Luban", "Datas");
        //    targetPath = Path.GetFullPath(targetPath);
        //    if (!Directory.Exists(targetPath))
        //    {
        //        Debug.LogError($"存放资料的临时文件夹不见了,你或许去回收站还能找到!{targetPath}");
        //        return;
        //    }
        //    //DirectoryInfo dataInfo=new DirectoryInfo(targetPath);
        //    //dataInfo.MoveTo(destDir);
        //    Debug.Log("转移数据" + targetPath);
        //    //Directory.Delete(targetPath,true);
        //    //return;
        //    srcDir = targetPath;

        //}

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
