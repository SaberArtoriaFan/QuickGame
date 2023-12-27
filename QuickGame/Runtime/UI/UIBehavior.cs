using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    public class UIBehavior 
    {
        public readonly TMP_Text tmpText; 
        public readonly RectTransform RT;
        public UIBehaviorType bhType;
        public UIBehavior(RectTransform rectTransform, UIBehaviorType behaviorType)
        {
            this.RT = rectTransform;
            this.bhType = behaviorType;

            switch (bhType)
            {
                case UIBehaviorType.TMP_Text:
                    tmpText = this.RT.GetComponent<TMP_Text>();
                    break;
            }
        }
    }
}
