using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HT.InfiniteList;
namespace Saber.UI
{
    /// <summary>
    /// 无限列表滚动视野
    /// </summary>
    public class InfiniteListSR : ScrollRect
    {
        /// <summary>
        /// 元素模板
        /// </summary>
        public GameObject ElementTemplate;
        /// <summary>
        /// 元素排列方向
        /// </summary>
        public Direction ListingDirection = Direction.Vertical;
        /// <summary>
        /// 元素高度
        /// </summary>
        public int Height = 20;
        /// <summary>
        /// 元素之间的间隔
        /// </summary>
        public int Interval = 5;

        private List<InfiniteListData> _datas = new List<InfiniteListData>();
        private HashSet<InfiniteListData> _dataIndexs = new HashSet<InfiniteListData>();
        private Dictionary<InfiniteListData, InfiniteElement> _displayElements = new Dictionary<InfiniteListData, InfiniteElement>();
        private HashSet<InfiniteListData> _invisibleList = new HashSet<InfiniteListData>();
        private Queue<InfiniteElement> _elementsPool = new Queue<InfiniteElement>();
        private RectTransform _uiTransform;

        /// <summary>
        /// UGUI变换组件
        /// </summary>
        public RectTransform UITransform
        {
            get
            {
                if (_uiTransform == null)
                {
                    _uiTransform = GetComponent<RectTransform>();
                }
                return _uiTransform;
            }
        }
        /// <summary>
        /// 当前数据数量
        /// </summary>
        public int DataCount
        {
            get
            {
                return _datas.Count;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener((value) => { RefreshScrollView(); });
        }

        /// <summary>
        /// 添加一条新的数据到无限列表尾部
        /// </summary>
        /// <param name="data">无限列表数据</param>
        public void AddData(InfiniteListData data)
        {
            if (_dataIndexs.Contains(data))
            {
                Debug.LogWarning("添加数据至无限列表失败：列表中已存在该数据 " + data.ToString());
                return;
            }

            _datas.Add(data);
            _dataIndexs.Add(data);

            RefreshScrollContent();
        }
        /// <summary>
        /// 添加多条新的数据到无限列表尾部
        /// </summary>
        /// <typeparam name="T">无限列表数据类型</typeparam>
        /// <param name="datas">无限列表数据</param>
        public void AddDatas<T>(T[] datas) where T : InfiniteListData
        {
            for (int i = 0; i < datas.Length; i++)
            {
                if (_dataIndexs.Contains(datas[i]))
                {
                    Debug.LogWarning("添加数据至无限列表失败：列表中已存在该数据 " + datas[i].ToString());
                    continue;
                }

                _datas.Add(datas[i]);
                _dataIndexs.Add(datas[i]);
            }

            RefreshScrollContent();
        }
        /// <summary>
        /// 添加多条新的数据到无限列表尾部
        /// </summary>
        /// <typeparam name="T">无限列表数据类型</typeparam>
        /// <param name="datas">无限列表数据</param>
        public void AddDatas<T>(List<T> datas) where T : InfiniteListData
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (_dataIndexs.Contains(datas[i]))
                {
                    Debug.LogWarning("添加数据至无限列表失败：列表中已存在该数据 " + datas[i].ToString());
                    continue;
                }

                _datas.Add(datas[i]);
                _dataIndexs.Add(datas[i]);
            }

            RefreshScrollContent();
        }
        /// <summary>
        /// 移除一条无限列表数据
        /// </summary>
        /// <param name="data">无限列表数据</param>
        public void RemoveData(InfiniteListData data)
        {
            if (_dataIndexs.Contains(data))
            {
                _datas.Remove(data);
                _dataIndexs.Remove(data);

                if (_displayElements.ContainsKey(data))
                {
                    RecycleElement(_displayElements[data]);
                    _displayElements.Remove(data);
                }

                RefreshScrollContent();
            }
            else
            {
                Debug.LogWarning("从无限列表中移除数据失败：列表中不存在该数据 " + data.ToString());
            }
        }
        /// <summary>
        /// 清除所有的无限列表数据
        /// </summary>
        public void ClearData()
        {
            _datas.Clear();
            _dataIndexs.Clear();

            foreach (var element in _displayElements)
            {
                RecycleElement(element.Value);
            }
            _displayElements.Clear();

            RefreshScrollContent();
        }

        protected float GetDatasHeight()
        {
            float len = 0;
            for(int i=0;i< _datas.Count;i++)
            {
                var v= _datas[i];
                len += v.DataHeight > 0 ? v.DataHeight : Height;
                len += Interval;
            }
            return len;
        }
        /// <summary>
        /// 刷新滚动列表内容
        /// </summary>
        protected void RefreshScrollContent()
        {
            if (ListingDirection == Direction.Vertical)
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x,GetDatasHeight());
            }
            else
            {
                content.sizeDelta = new Vector2(GetDatasHeight(), content.sizeDelta.y);
            }

            RefreshScrollView();
        }
        /// <summary>
        /// 刷新滚动视图
        /// </summary>
        protected void RefreshScrollView()
        {
            if (ListingDirection == Direction.Vertical)
            {
                float contentY = content.anchoredPosition.y;
                float viewHeight = UITransform.sizeDelta.y;

                ClearInvisibleVerticalElement(contentY, viewHeight);

                //int originIndex = (int)(contentY / (Height + Interval));
                int originIndex = 0;
                float h = 0;
                foreach (var v in _datas)
                {
                    h += (v.DataHeight > 0 ? v.DataHeight : Height);
                    h += Interval;
                    if (h > contentY)
                        break;
                    originIndex++;
                }
                if (originIndex < 0) originIndex = 0;
                float viewY = 0;
                for (int i = originIndex; i < _datas.Count; i++)
                {
                    InfiniteListData data = _datas[i];
                    float height = data.DataHeight > 0 ? data.DataHeight : Height;
                    float realTopEdge = viewY + contentY;
                    float realBottomEdge = viewY - height;
                    float origin = viewY;
                    viewY -= (height + Interval);

                    //只判断了顶大于底，没有判断底大于顶
                    if (realTopEdge > -viewHeight|| realBottomEdge <= 0)
                    {
                        //元素下边界是否和上边界有重叠部分？

                        if (_displayElements.ContainsKey(data))
                        {
                            _displayElements[data].UITransform.anchoredPosition = new Vector2(0, origin);
                            continue;
                        }

                        InfiniteElement element = ExtractIdleElement();
                        element.UITransform.anchoredPosition = new Vector2(0, origin);
                        element.OnUpdateData(this, data);
                        _displayElements.Add(data, element);

                        if (realBottomEdge>0)
                        {
                            Debug.Log($"断开判断，到" + i);

                            break;
                        }
       
                    }

                }
            }
            else
            {
                float contentX = content.anchoredPosition.x;
                float viewWidth = UITransform.sizeDelta.x;

                ClearInvisibleHorizontalElement(contentX, viewWidth);

                //int originIndex = (int)(-contentX / (Height + Interval));
                int originIndex = 0;
                float h = 0;
                foreach (var v in _datas)
                {
                    h += (v.DataHeight > 0 ? v.DataHeight : Height);
                    h += Interval;
                    if (h > -contentX)
                        break;
                    originIndex++;
                }
                if (originIndex < 0) originIndex = 0;
                float viewX = 0;
                for (int i = originIndex; i < _datas.Count; i++)
                {
                    InfiniteListData data = _datas[i];
                    float height = data.DataHeight > 0 ? data.DataHeight : Height;
                    float realX = viewX + contentX;
                    float origin = viewX;
                    viewX += (height + Interval);

                    if (realX < viewWidth)
                    {
                        if (_displayElements.ContainsKey(data))
                        {
                            _displayElements[data].UITransform.anchoredPosition = new Vector2(origin, 0);
                            continue;
                        }


                        InfiniteElement element = ExtractIdleElement();
                        element.UITransform.anchoredPosition = new Vector2(origin, 0);
                        element.OnUpdateData(this, data);
                        _displayElements.Add(data, element);
                    }
                    else
                    {
                        Debug.Log($"断开判断，到" + i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 清理并回收看不见的元素（垂直模式）
        /// </summary>
        /// <param name="contentY">滚动视图内容位置y</param>
        /// <param name="viewHeight">滚动视图高度</param>
        private void ClearInvisibleVerticalElement(float contentY, float viewHeight)
        {
            foreach (var element in _displayElements)
            {
                float topEdge = element.Value.UITransform.anchoredPosition.y + contentY;
                var height = element.Value.IsUseSelfHeigh ? element.Value.ElementHeigh : Height;
                float bottleEdge = topEdge - height;

                if (bottleEdge >= 0 || topEdge > -viewHeight)
                {
                    continue;
                }
                else
                {
                    _invisibleList.Add(element.Key);
                }
            }
            foreach (var item in _invisibleList)
            {
                RecycleElement(_displayElements[item]);
                _displayElements.Remove(item);
            }
            _invisibleList.Clear();
        }
        /// <summary>
        /// 清理并回收看不见的元素（水平模式）
        /// </summary>
        /// <param name="contentX">滚动视图内容位置x</param>
        /// <param name="viewWidth">滚动视图宽度</param>
        private void ClearInvisibleHorizontalElement(float contentX, float viewWidth)
        {
            foreach (var element in _displayElements)
            {
                float realX = element.Value.UITransform.anchoredPosition.x + contentX;
                var height = element.Value.IsUseSelfHeigh ? element.Value.ElementHeigh : Height;
                if (realX > -height && realX < viewWidth)
                {
                    continue;
                }
                else
                {
                    _invisibleList.Add(element.Key);
                }
            }
            foreach (var item in _invisibleList)
            {
                RecycleElement(_displayElements[item]);
                _displayElements.Remove(item);
            }
            _invisibleList.Clear();
        }
        /// <summary>
        /// 提取一个空闲的无限列表元素
        /// </summary>
        /// <returns>无限列表元素</returns>
        private InfiniteElement ExtractIdleElement()
        {
            if (_elementsPool.Count > 0)
            {
                InfiniteElement element = _elementsPool.Dequeue();
                element.gameObject.SetActive(true);
                return element;
            }
            else
            {
                GameObject element = Instantiate(ElementTemplate, content);
                element.SetActive(true);
                return element.GetComponent<InfiniteElement>();
            }
        }
        /// <summary>
        /// 回收一个无用的无限列表元素
        /// </summary>
        /// <param name="element">无限列表元素</param>
        private void RecycleElement(InfiniteElement element)
        {
            element.OnClearData();
            element.gameObject.SetActive(false);
            _elementsPool.Enqueue(element);
        }

        /// <summary>
        /// 方向
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 水平
            /// </summary>
            Horizontal,
            /// <summary>
            /// 垂直
            /// </summary>
            Vertical
        }
    }
}