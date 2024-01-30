//using Saber.UI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//#region
////池骋
//#endregion
//[CustomPropertyDrawer(typeof(UIResourcesAttribute))]
//public class UIResourcesDrawer : PropertyDrawer
//{
//    float ovrrideHeight = 20;
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return base.GetPropertyHeight(property, label)+ ovrrideHeight;
//    }
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        Debug.Log("Rect" + position);
//        EditorGUI.LabelField(position, "你好");
//        position.y += ovrrideHeight;
//        base.OnGUI(position, property, label);
//    }
//}
