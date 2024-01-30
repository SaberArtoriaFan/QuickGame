using System.IO;
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
        GetFilesAndDirs(srcDir, destDir);
        Debug.Log($"初始化完成！-->{destDir}");
    }
    public static void MakeSure(string filePath)
    {
        var direPath = Path.GetDirectoryName(filePath);
        if (System.IO.Directory.Exists(direPath) == false)
            System.IO.Directory.CreateDirectory(direPath);
        if (System.IO.File.Exists(filePath) == false)
            System.IO.File.Create(filePath).Dispose();
    }
    public  static void GetFilesAndDirs(string srcDir, string destDir)
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
    public static void CopyDatasToTemp(string dataPath)
    {
        var tempPath = Path.Combine(Application.temporaryCachePath, "Luban", "Datas");
        if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
        GetFilesAndDirs(dataPath, tempPath);
    }
}
