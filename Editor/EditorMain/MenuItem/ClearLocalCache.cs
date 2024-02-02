using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#region
//池骋
#endregion
namespace Saber
{
    public class ClearLocalCache
    {
        [MenuItem("Saber/Function/Cache/ClearPlayerPrefs",false,1)]
        static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("清除PlayerPrefs成功");
        }
        [MenuItem("Saber/Function/Cache/ClearPersistent",false,2)]
        static void ClearPersistentDataPath()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
            directoryInfo.Delete(true);
            Debug.Log($"清除本地缓存{Application.persistentDataPath}成功");
        }
        [MenuItem("Saber/Function/Cache/ClearAllCache", false, 3)]
        static void ClearAllh()
        {
            ClearPlayerPrefs();
            ClearPersistentDataPath();
        }
    }
}

