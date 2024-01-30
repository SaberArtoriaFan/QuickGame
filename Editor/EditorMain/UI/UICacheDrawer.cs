using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region
//保持UTF-8
#endregion
namespace Saber.UI
{

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UICacheItem))]
    public class UICacheItemDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //property.o
            //var root= property.FindPropertyRelative("root").objectReferenceValue as GameObject;
            float addWidth = position.x - 50;
            position=new Rect(50,position.y,position.width+addWidth,20);
            EditorGUI.BeginProperty(position, label, property);
            //FocusType.Passive 使用Tab键切换时不会被选中，FocusType.Keyboard 使用Tab键切换时会被选中，很显然这里我们不需要label能被选中进行编辑 
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);
            //不让indentLevel层级影响到同一行的绘制，因为PropertyDrawer在很多地方都有可能被用到，可能出现嵌套使用
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var buttonRect = new Rect(position.x , position.y, 20, 20);
            position = new Rect(position.x+35, position.y, position.width, position.height);
            var nameRect = new Rect(position.x, position.y, 120, 20);
            var overviewRect = new Rect(position.x + 130, position.y, position.width-135, 20);
            var str = property.FindPropertyRelative("UIName").stringValue;
            if (EditorGUI.LinkButton(buttonRect, "O"))
            {
                GUIUtility.systemCopyBuffer = str;
                Debug.Log($"已复制到剪切板--->{str}");
            }
            EditorGUI.LabelField(nameRect,str);
            

            EditorGUI.BeginDisabledGroup(true);

            //.PropertyField(nameRect, , GUIContent.none);
            //EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
            //这里获取就行
            //EditorGUI.ObjectField(overviewRect,, typeof(GameObject));
            //EditorGUI.PropertyField(overviewRect, property.FindPropertyRelative("UIModel"), GUIContent.none);
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(UICache))]
    public class UICacheDrawer:PropertyDrawer
    {
        bool isExpand = false;
        float elementHeigh = 25;
        float originHeigh = 18;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var count=property.FindPropertyRelative("items").arraySize+1;
           // Debug.Log("cOUNT" + count);
            return isExpand? originHeigh + count * elementHeigh+10:originHeigh;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            Rect toggle = position;
            if (isExpand)
                toggle = new Rect(position.x, position.y , position.width, originHeigh);

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
                var items = property.FindPropertyRelative("items");

                foreach (SerializedProperty item in items)
                {
                    position = new Rect(position.x, position.y + elementHeigh, position.width, position.height);

                    EditorGUI.PropertyField(position, item, GUIContent.none);
                }
                var btnPos = new Rect(position.width/2-50, position.y + elementHeigh, position.width, position.height);

                if (EditorGUI.LinkButton(btnPos, "选中脚本生成代码位置"))
                {
                    var path = property.FindPropertyRelative("scriptsSavePath").stringValue;
                    path=Path.GetFullPath(path);
                    var pref = Path.GetFullPath(Application.dataPath);
                    path= path.Replace(pref, "Assets");
                    UIScriptsGenerate.OpenPrefabPath(path);
                }
            }


            EditorGUI.indentLevel = indent;
            //isExpand = false;
            EditorGUI.EndProperty();
           // base.OnGUI(position, property, label);
        }
    }
#endif
}
