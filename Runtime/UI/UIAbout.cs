using Sirenix.OdinInspector;
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
        [LabelText("",true,Icon = SdfIconType.Apple),HorizontalGroup(125,LabelWidth =20)]
        public string ItemName;
        [HideLabel, HorizontalGroup,InfoBox("请输入正确的类型全名,例如:UnityEngine.GameObject,再点击保存",InfoMessageType.Error,VisibleIf ="@isError")]
        public string ClassFullName;
        [HideLabel, HorizontalGroup(125), ReadOnly]
        public string BehaviorName;

        [NonSerialized]
        public bool isError=false;
        //[HideLabel, HorizontalGroup(125), ShowIf("@string.IsNullOrEmpty(BehaviorName)")]
        //public UIBehaviorType BehaviorType;

        public AboutItem()
        {
            
        }

        //public AboutItem(string itemName, UIBehaviorType behaviorType, string classFullName)
        //{
        //    ItemName = itemName;
        //    BehaviorType = behaviorType;
        //    ClassFullName = classFullName;
        //}

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
            new AboutItem("Btn",typeof(Button).Name,typeof(Button).FullName),
            new AboutItem("Text",typeof(Text).Name,typeof(Text).FullName),
            new AboutItem("TMP",typeof(TMP_Text).Name,typeof(TMP_Text).FullName),
            new AboutItem("Img",typeof(Image).Name,typeof(Image).FullName),
            new AboutItem("RawImg",typeof(RawImage).Name,typeof(RawImage).FullName),
            new AboutItem("RT",typeof(RectTransform).Name,typeof(RectTransform).FullName),
            new AboutItem("SR",typeof(ScrollRect).Name,typeof(ScrollRect).FullName)

        };

    }
}
