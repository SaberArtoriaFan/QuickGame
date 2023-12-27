using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    internal interface IUIDialog:IUIBase
    {
        bool IsOverWindow { get; }
    }
    public class UIDialog<T> : UIBase<T>,IUIDialog where T:UIDialog<T>
    {

        public virtual bool IsOverWindow => true;



    }
}
