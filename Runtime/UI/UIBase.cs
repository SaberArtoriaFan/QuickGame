using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    public enum UIBehaviorType:byte
    {
        Defeault,
        Button,
        Img,
        Text,
        TMP_Text,
        RawImage,
        RectTransform,
        ScrollRect

    }
    //[Serializable]
    //public class SerUIBaseData
    //{
    //    public SerUIItem[] items;
    //}
    //[Serializable]
    //public class SerUIItem
    //{
    //    public string path;
    //    public byte type;
    //}
    internal interface IUIBase
    {
        Transform transform { get; }
        void OnOpen();
        void OnClose();
        void OnDestroy();
        void Init();
        void SetParams(params object[] args);
        int Order { get; }
        bool IsCache { get; }
        bool IsOpen { get; }
        /// <summary>
        /// 是不是影响其他Node
        /// </summary>
        bool IsInfluence { get; }
    }
    [Serializable]
    public class UIInspectorHelper
    {
        public UIInspectorHelper()
        {
        }
    }


    //[RequireComponent(typeof(RectTransform))]
    public class UIBase<T> :MonoBehaviour,IUIBase where T:UIBase<T> 
    {
        //[Disable, HideInInspector,SerializeField]
        //private TextAsset configJson;
        //[SerializeField]
        //protected UICache cache;
        //protected bool 
        [SerializeField] protected UIInspectorHelper m_Inspector = new UIInspectorHelper();

        bool isRunning= false;

        //protected Dictionary<int,UIBehavior> behaviorDict= new Dictionary<int,UIBehavior>();

        public virtual int Order => 0;

        public virtual bool IsCache => false;

        public virtual bool IsInfluence => true;

        public bool IsOpen { get; protected set; }

        //public UIBehavior this[string name]
        //{
        //    get
        //    {
        //        return this[UIUtil.StringToHash(name)];
        //    }
        //}
        //public UIBehavior this[int name]
        //{
        //    get
        //    {
        //        if (behaviorDict.TryGetValue(name, out var res)) return res;
        //        else return null;
        //    }
        //}

        //public T1 BH<T1>()
        //{
        //    return null;
        //}


        protected  void Awake()
        {
            //LoadAllLocal();
            OnScriptsGenerate();
            Init();
            //cache.ShutDown();
            //cache = null;
        }
        public virtual void Init() { }
        public virtual void OnScriptsGenerate() { }
        public virtual void OnDestroy()
        {

        }
        public virtual void SetParams(params object[] args)
        {

        }
        public virtual void OnOpen()
        {
            this.IsOpen = true;
        }
        public virtual void OnClose()
        {
            this.IsOpen = false;
        }
        //protected void LoadAllLocal()
        //{
        //    if (cache == null)
        //    {
        //        Debug.LogError("UI缓存未生成,请生成缓存,本次临时生成配置，请在Editor模式下自行生成");
        //        isRunning = true;
                
        //        RegisterAll();
        //        isRunning = false;
        //    }
        //    foreach (var v in cache.items)
        //    {
        //        if (string.IsNullOrEmpty(v.UIPath))
        //        //    behaviorDict.TryAdd(UIUtil.StringToHash(v.UIName), new UIBehavior(v.UIModel.transform as RectTransform, (UIBehaviorType)v.UIType));
        //        //else
        //        {
        //            Debug.LogError($"保存的预制体出错啦！是不是你更改了？路径--》{v.UIPath}\n已经帮你运行时生成了，请务必结束后手动生成！！！");
        //            cache = null;
        //            LoadAllLocal();
        //            break;
        //        }
        //    }
        //}
        //protected void LoadAllJson()
        //{
        //    SerUIBaseData data = null;
        //    if (configJson == null)
        //    {
        //        var dataName = GetDataFullPath(UIManager.About);
        //        data = JsonUtil.ReadData<SerUIBaseData>(dataName);
        //    }
        //    else
        //    {
        //        data = JsonUtility.FromJson<SerUIBaseData>(configJson.text);
        //        //释放对他的引用
        //        configJson = null;
        //    }

        //    foreach (var item in data.items)
        //    {
        //        var itemName = Path.GetFileName(item.path);
        //        var itemUI = new UIBehavior(FindRT(transform, item.path) as RectTransform, (UIBehaviorType)item.type);
        //        behaviorDict.Add(UIUtil.StringToHash(itemName), itemUI);
        //    }
        //}


        [Button(SdfIconType.Sun, "生成配置")]
        protected void RegisterAll()
        {
            if (Application.isPlaying&&isRunning==false)
            {
                Debug.LogError("请勿在运行中生成UI配置");
                return;
            }

            List<UICacheItem> cacheItems= new List<UICacheItem>();

            var about=UIUtil.LoadAbout();
            var split = about.Split;
            HashSet<string> map=new HashSet<string>();
            Type type = typeof(UIAbout);
            //SerUIBaseData data=new SerUIBaseData();
            //List<SerUIItem> list=new List<SerUIItem> ();
            //System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var v in about.aboutItems)
            {     
                if(map.Contains(v.ItemName)==false)
                    map.Add(v.ItemName);
            }
            string prefixPath = "";
            Action<Transform, string,string> action = (tr,n, s) =>
            {
                string name = tr.name;
                int index=name.IndexOf(split);
                if (index <= 0) return;
                string prefix=name.Substring(0, index);
                if(map.Contains(prefix))
                {
                    //list.Add(new SerUIItem()
                    //{
                    //    path = s,
                    //    type = GetUIType(prefix)
                    //});
                    s = s.Replace("\\", "/");
                    cacheItems.Add(new UICacheItem() { UIName=n,UIPath=s,/*UIModel=tr.gameObject,*/UIAbb=UIUtil.GetUIType(prefix,about)});
                }
            };
            foreach(Transform tr in transform)
                FindChilren(tr, prefixPath, action);

            var cache = new UICache();
            cache.root = this.gameObject;
            cache.items = cacheItems.ToArray();

            //data.items = list.ToArray();
#if UNITY_EDITOR
            //JsonUtil.Save(GetDataFullPath(about),data,false);
            //this.configJson = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine("Assets", about.UIDataSavePrefix, $"{typeof(T).Name}Data.json"));
            //EditorUtility.SetDirty(this.configJson);

            UIScriptsGenerate.Create(typeof(T).Name, cache);
            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            //Debug.Log($"生成成功--->路径:{GetDataFullPath(about)}");
            Debug.Log($"生成成功--->路径:{this.GetType().Name}");

#endif
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;
            //if (this.cache == null)
            //    RegisterAll();
        }

        protected byte GetUIType(string prefix)
        {
            return 0;
        }
        protected void FindChilren(Transform root,string prefixPath,Action<Transform,string,string> action)
        {
            prefixPath=Path.Combine(prefixPath, root.name);
            action(root, root.name,prefixPath);
            foreach (Transform child in root)
                FindChilren(child,prefixPath,action);
        }
        protected Transform FindRT(Transform root,string path)
        {
            if (string.IsNullOrEmpty(path)) return root;
            int index= path.IndexOf(Path.DirectorySeparatorChar);
            string childName = string.Empty;
            //小于0就说明没有分隔符了，剩下的就是目标
            if (index < 0) childName = path;
            else childName= path.Substring(0, index);
           
            var child = root.Find(childName);
            if (index<0) return child;
            return FindRT(child, path.Substring(index + 1));
        }
        /// <summary>
        /// 完整路径
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public string GetDataFullPath(UIAbout about)
        {
            return Path.Combine(Application.dataPath, about.UIDataSavePrefix, $"{typeof(T).Name}Data.json");
        }
    }
}
