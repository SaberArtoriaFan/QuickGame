using System;
using UnityEngine;
#region
//saber战棋
#endregion
namespace Saber.UI
{
    [Serializable]
    public class UICache
    {
        public GameObject root;
        public UICacheItem[] items;
        public string scriptsSavePath;
        public void ShutDown()
        {
            foreach (var item in items)
            {
                item.UIName = null;
                item.UIPath = null;
                //item.UIModel = null;
            }
        }
    }
    [Serializable]
    public class UICacheItem
    {
        public string UIName;
        public string UIPath;
        public string UIAbb;
        public GameObject UIModel;
    }
}


