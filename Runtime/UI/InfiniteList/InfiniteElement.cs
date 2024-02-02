using HT.InfiniteList;
using UnityEngine;

namespace Saber.UI
{
    /// <summary>
    /// 无限列表元素
    /// </summary>
    public class InfiniteElement : MonoBehaviour
    {
        private RectTransform _uiTransform;

        public virtual bool IsUseSelfHeigh => false;
        public virtual float ElementHeigh { get; }
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
        /// 更新显示数据
        /// </summary>
        /// <param name="scrollRect">无限列表滚动视野</param>
        /// <param name="data">无限列表数据</param>
        public virtual void OnUpdateData(InfiniteListSR scrollRect, InfiniteListData data)
        {

        }

        /// <summary>
        /// 清理显示数据
        /// </summary>
        public virtual void OnClearData()
        {

        }
    }
}