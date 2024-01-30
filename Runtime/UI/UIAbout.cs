using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    [Serializable]
    public class AboutItem
    {
        public string ItemName;
        public string ClassFullName;
        public string BehaviorName;
        public UIBehaviorType BehaviorType;

        public AboutItem(string itemName, UIBehaviorType behaviorType, string classFullName)
        {
            ItemName = itemName;
            BehaviorType = behaviorType;
            ClassFullName = classFullName;
        }

        public AboutItem(string itemName,  string behaviorName, string classFullName)
        {
            ItemName = itemName;
            ClassFullName = classFullName;
            BehaviorName = behaviorName;
        }
    }
    public class UIAbout : ScriptableObject
    {
        public char Split = '_';

        public string UI_Button => aboutItems[0].ItemName;
        public string UI_Text=> aboutItems[1].ItemName;
        public string UI_TMP_Text => aboutItems[2].ItemName;
        public string UI_Image => aboutItems[3].ItemName;

        public string UIDataSavePrefix = Path.Combine("UI", "Data");

        public string UIScriptTemplate;
        public string UIScriptSavePrefix = Path.Combine("UI", "Scripts");

        public AboutItem[] aboutItems = new AboutItem[]
        {
            new AboutItem("Btn",UIBehaviorType.Button,typeof(Button).FullName),
            new AboutItem("Text",UIBehaviorType.Text,typeof(Text).FullName),
            new AboutItem("TMP",UIBehaviorType.TMP_Text,typeof(TMP_Text).FullName),
            new AboutItem("Img",UIBehaviorType.Img,typeof(Image).FullName),
            new AboutItem("RawImg",UIBehaviorType.RawImage,typeof(RawImage).FullName),
            new AboutItem("RT",UIBehaviorType.RectTransform,typeof(RectTransform).FullName),
            new AboutItem("SR",UIBehaviorType.ScrollRect,typeof(ScrollRect).FullName)

        };

    }
}
