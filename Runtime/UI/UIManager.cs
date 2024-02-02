using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    /// <summary>
    /// 负责管理弹窗和层级
    /// </summary>
    public class UIManager : SingleCase<UIManager>
    {
        UIAbout about;
        private Canvas mainCanvas;

        internal Dictionary<Type, IUIBase> uICache=new Dictionary<Type, IUIBase>();
        UINode windowRootNode;
        UINode dialogRootNode;

        public UIManager()
        {
            InitAbout();
            FindMainCanvas();
        }


        public static UIAbout About => Instance.about;

        public Canvas MainCanvas { get 
            {
                if (mainCanvas == null)
                    FindMainCanvas();
                return mainCanvas;
            } }
        RectTransform windowRoot;
        RectTransform dialogRoot;
        private void FindMainCanvas()
        {
            var canvases = GameObject.FindObjectsOfType<Canvas>();
            for(int i= 0; i < canvases.Length; i++)
            {
                if (canvases[i].name.Contains("Main"))
                {
                   mainCanvas = canvases[i];
                    break;
                }
                if(mainCanvas==null)
                    mainCanvas = canvases[i];
            }
            if(mainCanvas)
            {
                var go = new GameObject("WindowRoot", typeof(RectTransform));
                go.transform.SetParent(mainCanvas.transform);
                windowRoot =go.GetComponent<RectTransform>();
                windowRoot.localScale = Vector3.one;
                windowRoot.anchorMin = new Vector2(0, 0);
                windowRoot.anchorMax = new Vector2(1, 1);
                windowRoot.offsetMin = new Vector2(0, 0);
                windowRoot.offsetMax = new Vector2(0, 0);
                go = new GameObject("DialogRoot", typeof(RectTransform));
                go.transform.SetParent(mainCanvas.transform);
                dialogRoot=go.GetComponent<RectTransform>();
                dialogRoot.localScale = Vector3.one;
                dialogRoot.anchorMin = new Vector2(0, 0);
                dialogRoot.anchorMax = new Vector2(1, 1);
                dialogRoot.offsetMin = new Vector2(0, 0);
                dialogRoot.offsetMax = new Vector2(0, 0);

            }
            else
            {
                windowRoot = null;
                dialogRoot = null;
                Debug.LogError("无法在场景中找到[Canvas],请确保场景中有Canvas,并且可以命名带有'Main'指定主画布");
            }
        }

        private void InitAbout()
        {
            about = UIUtil.LoadAbout();
        }
        internal void Cache<T>(T t)where T : IUIBase
        {
            Type type= t.GetType();
            if (uICache.TryAdd(type, t)==false)
                GameObject.Destroy(t.transform.gameObject);
            else
                t.transform.gameObject.SetActive(false);
        }
        /// <summary>
        /// 如果不忽略层级，可能只是将层加入UI栈中，不会立刻开启
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isIgnoreOrder"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T OpenDialog<T>(bool isIgnoreOrder = true, params object[] paras) where T:UIDialog<T>
        {
            return OpenUI<T>(dialogRoot, isIgnoreOrder, paras);
        }
        public T OpenWindow<T>(bool isIgnoreOrder = true,params object[] paras) where T:UIWindow<T>
        {
            return OpenUI<T>(windowRoot, isIgnoreOrder, paras);
        }
        /// <summary>
        /// 会销毁所有缓存UI
        /// </summary>
        public void CloseAllWindow()
        {
            windowRootNode.DestoryAll();
            windowRootNode = null;
        }
        /// <summary>
        /// 会销毁所有缓存UI
        /// </summary>
        public void CloseAllDialog()
        {
            dialogRootNode.DestoryAll();
            dialogRootNode = null;
        }
        public void CloseTopWindow(bool showNext)
        {
            windowRootNode.Pop(showNext);
            if (showNext && windowRootNode.Count == 0)
            {
                while (windowRootNode!=null&& windowRootNode.Count > 0)
                {
                    windowRootNode = windowRootNode.deepNode;
                    windowRootNode.Pop(true);
                }
            }
        }

        public void CloseTopDialog(bool showNext)
        {
            dialogRootNode.Pop(showNext);
            if (showNext && dialogRootNode.Count == 0)
            {
                while (dialogRootNode != null && dialogRootNode.Count > 0)
                {
                    dialogRootNode = dialogRootNode.deepNode;
                    dialogRootNode.Pop(true);
                }
            }
        }
        T OpenUI<T>(RectTransform root,bool isIgnoreOrder = true,params object[] paras) where T : UIBase<T>
            {
            T ui = null;
            Type type = typeof(T);
            if (uICache.TryGetValue(type, out var v) == false)
            {
                var resource = type.GetCustomAttribute<UIResourcesAttribute>();
                var url = resource?.address;
                if (resource == null || string.IsNullOrEmpty(url))
                {
                    Debug.LogError($"UI 预制体路径为空,请给{typeof(T).FullName}类添加属性[UIResource]设置资源路径");
                    return null;
                }
                switch (resource.loadType)
                {
                    case ResourcesLoadType.Resources:
                        ui = Resources.Load<T>(url);
                        break;
                }
                if(ui== null)
                    Debug.LogError($"没有加载到UI预制体,加载方式为:{resource.loadType.ToString()},路径为:{resource.address}");
                else
                    //实例化
                    ui = GameObject.Instantiate(ui, root);
            }
            else
            {
                ui = (T)v;
                uICache.Remove(type);
            }


            if (ui)
            {

                //加载到画布
                //添加到UI栈里
                //初始化
                ui.SetParams(paras);
                SortUI(ui, isIgnoreOrder);
                
            }
            
            return ui;
        }
        private void SortUI<T>(T ui,bool isIgnoreOrder) where T : UIBase<T>
        {
            UINode rootNode = null;
            if(ui is IUIDialog)
            {
                rootNode = dialogRootNode;
            }
            else if(ui is IUIWindow)
            {
                rootNode = windowRootNode;

            }
            if (rootNode == null)
            {
                rootNode = new UINode(ui.Order);
                //return;
            }

            if (isIgnoreOrder||ui.IsInfluence==false||rootNode.Order == ui.Order)
            {
                rootNode.Push(ui);
                ui.gameObject.SetActive(true);
                ui.OnOpen();
            }
            else if (rootNode.Order > ui.Order)
            {
                UINode newNode = new UINode(ui.Order);
                newNode.deepNode = rootNode.Hide();
                newNode.Push(ui);
                ui.gameObject.SetActive(true);
                ui.OnOpen();
                rootNode = newNode;
            }
            else
            {
                ui.gameObject.SetActive(false);
                UINode targetNode = rootNode;
                while (targetNode.deepNode != null && targetNode.deepNode.Order < ui.Order )
                    targetNode = targetNode.deepNode;
                if (targetNode.deepNode == null)
                {
                    //创建了节点，但是没有打开这个界面
                    UINode newNode = new UINode(ui.Order);
                    newNode.Push(ui);
                    targetNode.deepNode = newNode;
                }
                else
                {
                    if (targetNode.deepNode.Order == ui.Order)
                        targetNode.Push(ui);
                    else
                    {
                        UINode newNode = new UINode(ui.Order);
                        newNode.Push(ui);
                        newNode.deepNode = targetNode.deepNode;
                        targetNode.deepNode = newNode;
                    }
                }

            }
            if (ui is IUIDialog)
            {
                dialogRootNode = rootNode; 
            }
            else if (ui is IUIWindow)
            {
                windowRootNode=rootNode;

            }
        }
    }
}
