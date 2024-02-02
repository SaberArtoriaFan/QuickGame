//using Saber.UI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//#region
////池骋
//#endregion
//[CustomPropertyDrawer(typeof(AboutItem))]
//public class UIAboutItemDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //property.o
//        //var root= property.FindPropertyRelative("root").objectReferenceValue as GameObject;
//        float addWidth = position.x - 50;
//        position = new Rect(50, position.y, position.width + addWidth, 20);
//        EditorGUI.BeginProperty(position, label, property);
//        //FocusType.Passive 使用Tab键切换时不会被选中，FocusType.Keyboard 使用Tab键切换时会被选中，很显然这里我们不需要label能被选中进行编辑 
//        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);
//        //不让indentLevel层级影响到同一行的绘制，因为PropertyDrawer在很多地方都有可能被用到，可能出现嵌套使用
//        var indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;
//        //var buttonRect = new Rect(position.x, position.y, 20, 20);
//        position = new Rect(position.x + 35, position.y, position.width, position.height);
//        var nameRect = new Rect(position.x, position.y, position.width/4, 20);
//        var fullNameRect = new Rect(position.x + position.width / 4, position.y, position.width / 2, 20);
//        var behaviorRect = new Rect(position.x + position.width / 4*3, position.y, position.width / 4, 20);

//        var name = property.FindPropertyRelative("ItemName").stringValue;
//        var fullName = property.FindPropertyRelative("ClassFullName").stringValue;
//        var behavior = (UIBehaviorType)property.FindPropertyRelative("BehaviorType").enumValueIndex;

//        EditorGUI.LabelField(nameRect, name);
//        EditorGUI.LabelField(fullNameRect,fullName);
//        EditorGUI.EnumPopup(behaviorRect, behavior);


//        //EditorGUI.BeginDisabledGroup(true);

//        //.PropertyField(nameRect, , GUIContent.none);
//        //EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
//        //这里获取就行
//        //EditorGUI.ObjectField(overviewRect,, typeof(GameObject));
//        //EditorGUI.PropertyField(overviewRect, property.FindPropertyRelative("UIModel"), GUIContent.none);
//        //EditorGUI.EndDisabledGroup();

//        EditorGUI.indentLevel = indent;
//        EditorGUI.EndProperty();
//    }
//}
