using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    public class UINode 
    {
        public readonly int Order;
        readonly List<IUIBase> Values=new List<IUIBase>();
        readonly List<IUIBase> NoInfludeValues=new List<IUIBase>();
        public UINode deepNode;

        public UINode(int order)
        {
            Order = order;
        }
        /// <summary>
        /// 没有处理Open事件
        /// </summary>
        /// <param name="uIBase"></param>
        internal void Push(IUIBase uIBase)
        {
            if(uIBase.IsInfluence==false)
            {
                NoInfludeValues.Add(uIBase);
                if(uIBase.IsOpen==false)
                {
                    uIBase.transform.gameObject.SetActive(true);
                    uIBase.OnOpen();
                }
                return;
            }

            if (Values.Count > 0 && Values[0].IsOpen)
            {
                if (Values[0].IsCache)
                {
                    Values[0].OnClose();
                    UIManager.Instance.Cache(Values[0]);

                }
                else
                {
                    var v= Values[0];
                    v.OnClose();
                    GameObject.Destroy(v.transform.gameObject);
                }
                Values.RemoveAt(0);
            }

            Values.Insert(0, uIBase);
        }
        internal IUIBase Pop(bool showNext)
        {
            IUIBase v = null;
            if (Values.Count > 0)
            {
                v = Values[0];
         
                Values.RemoveAt(0);

                if (v.IsOpen)
                    v.OnClose();
                v.transform.gameObject.SetActive(false);
                if (showNext &&Values.Count>0)
                {
                    Values[0].transform.gameObject.SetActive(true);
                    Values[0].OnOpen();
                }

                if (v.IsCache)
                    UIManager.Instance.Cache(v);
                else
                    GameObject.Destroy(v.transform.gameObject);
            }
            return v;
        }
        public int Count=> Values.Count;
        internal UINode Hide()
        {
            bool isNullNode = true;
            for(int i=0;i<Values.Count; i++)
            {
                if (Values[i].IsOpen)
                {
                    Values[i].OnClose();
                    Values[i].transform.gameObject.SetActive(false);
                }
                if (Values[i].IsCache)
                {
                    isNullNode = false;
                    UIManager.Instance.Cache(Values[i]);
                }
                else
                {
                    GameObject.Destroy(Values[i].transform.gameObject);
                }
            }
            Values.Clear();
            if (isNullNode)
                return deepNode?.Hide();
            else
                return this;
        }

        internal void DestoryAll()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                var v = Values[i];
                if ( v== null) continue;
                if (v.IsOpen)
                    v.OnClose();
                GameObject.Destroy(v.transform.gameObject);
            }
            Values.Clear();

            for (int i = 0; i < NoInfludeValues.Count; i++)
            {
                var v = NoInfludeValues[i];
                if (v == null) continue;
                if (v.IsOpen)
                    v.OnClose();
                GameObject.Destroy(v.transform.gameObject);
            }
            NoInfludeValues.Clear();
            deepNode?.DestoryAll();

        }
    }
}
