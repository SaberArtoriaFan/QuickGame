using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonUtil
{
    //读取文件
    public static string ReadData(string fileName)
    {
        //string类型的数据常量
        string readData;
        //获取到路径
        string fileUrl = fileName.EndsWith(".json") ? fileName : fileName + ".json";
        //Debug.Log($"ReadPath->{fileUrl}");

        if (File.Exists(fileUrl))
        {
            //读取文件
            using (StreamReader sr = File.OpenText(fileUrl))
            {
                //数据保存
                readData = sr.ReadToEnd();
                sr.Close();
            }
            //Debug.Log($"正在从{fileUrl}读取文件,内容为{readData}");

            //返回数据
            return readData;
        }
        else
            return null;


    }
    public static T ReadData<T>(string fileName) where T : class
    {
        string s = ReadData(fileName);
        if (string.IsNullOrEmpty(s)) return null;
        return JsonUtility.FromJson<T>(s);
    }
    //通过文件名称保存数据到json文件中，存储的路径为persistentDataPath
    public static void Save(string fileName, object value, bool isStrict = true)
    {

        string json = JsonUtility.ToJson(value);
        string filepath = fileName.EndsWith(".json") ? fileName : fileName + ".json";
        Debug.Log("Auto save，Content:" + json + $"\nPath:{filepath}");
        if (Directory.Exists(Path.GetDirectoryName(filepath)) == false)
        {
            if (isStrict)
            {
                Debug.LogError($"Path Not Find-->{filepath}");
                return;
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }
        }

        if (!File.Exists(filepath))
        {
            File.Create(filepath).Dispose();
            //Debug.Log(filepath);
        }

        using (StreamWriter sw = new StreamWriter(filepath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
    }
}