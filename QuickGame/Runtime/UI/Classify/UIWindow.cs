using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    internal interface IUIWindow:IUIBase
    {

    }
    public class UIWindow<T> :UIBase<T>,IUIWindow where T:UIWindow<T>
    {
        
    }
}
