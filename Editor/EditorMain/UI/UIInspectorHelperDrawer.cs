using Saber.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
#region
//池骋
#endregion
[CustomPropertyDrawer(typeof(UIInspectorHelper))]
public class UIInspectorHelperDrawer : PropertyDrawer
{
    bool isExpand = false;
    float elementHeigh = 25;
    float originHeigh = 18;
    Dictionary<string, GameObject> dict = null;
    void InitDict(SerializedProperty property)
    {
        if (dict != null) return;
        //Debug.Log($"序列化对象:{property.serializedObject.targetObject},类型:{(property.serializedObject.targetObject).GetType()}方法:{(property.serializedObject.targetObject).GetType().GetMethod("OnEditorShow")}");
        dict = (property.serializedObject.targetObject).GetType().GetMethod("OnEditorShow").Invoke(property.serializedObject.targetObject, null) as Dictionary<string, GameObject>;
    }
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        InitDict(property);
        return base.CreatePropertyGUI(property);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //var count = property.FindPropertyRelative("items").arraySize + 1;
        InitDict(property);
        var count = dict.Count + 1;
        // Debug.Log("cOUNT" + count);
        return isExpand ? originHeigh + count * elementHeigh + 10 : originHeigh;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InitDict(property);
        Rect toggle = position;
        if (isExpand)
            toggle = new Rect(position.x, position.y, position.width, originHeigh);

        //position.y+= elementHeigh;
        //position = new Rect(position.x, position.y - 50, position.width, position.height);
        EditorGUI.BeginProperty(position, label, property);
        // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        //不让indentLevel层级影响到同一行的绘制，因为PropertyDrawer在很多地方都有可能被用到，可能出现嵌套使用
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        // Debug.Log(toggle);
        isExpand = EditorGUI.Toggle(toggle, "展示元素缓存", isExpand);
        if (isExpand)
        {
            //property.serializedObject
            EditorGUI.BeginDisabledGroup(true);
            foreach (var item in dict)
            {
                var item_position = new Rect(position.x+35, position.y + elementHeigh, 100, 20);
                EditorGUI.LabelField(item_position, item.Key);
                item_position = new Rect(position.x+position.width/2, position.y + elementHeigh, 100, 20);
                EditorGUI.ObjectField(item_position, item.Value, typeof(GameObject),false);
                position.y += elementHeigh;
            }
            EditorGUI.EndDisabledGroup() ;

            var btnPos = new Rect(position.width / 2 - 50, position.y + elementHeigh, position.width, position.height);

            if (EditorGUI.LinkButton(btnPos, "选中脚本生成代码位置"))
            {
                var fileName = $"{(property.serializedObject.targetObject).GetType()}_Params";
                var paths= AssetDatabase.FindAssets($"{fileName} t:TextAsset",new string[] { "Assets"});
                var path = string.Empty;
                foreach(var m in paths)
                {
                    string v = AssetDatabase.GUIDToAssetPath(m);
                    if (System.IO.Path.GetExtension(v)==".cs")
                    { path = v; break; }
                }
                Debug.Log($"filename:{fileName},pathcount:{paths[0]},path:{path}");
                //var path = property.FindPropertyRelative("scriptsSavePath").stringValue;
                path = Path.GetFullPath(path);
                var pref = Path.GetFullPath(Application.dataPath);
                path = path.Replace(pref, "Assets");
                UIScriptsGenerate.OpenPrefabPath(path);
            }
        }


        EditorGUI.indentLevel = indent;
        //isExpand = false;
        EditorGUI.EndProperty();
        // base.OnGUI(position, property, label);
    }
}
