using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    public class UIAboutWindow: EditorWindow
    {
        UIAbout uIAbout;
        List<(string,string)>list=new List<(string, string)>();

        string newAbout_Abb;
        string newAbout_FullClassName;
        UIBehaviorType behaviorType;
        AboutItem aboutItem = new AboutItem();

        public UIAboutWindow()
        {
            position = new Rect(this.position.xMin, position.yMin, 800, 500);


        }
        void Init()
        {
            //Type type = typeof(UIAbout);
            //SerUIBaseData data=new SerUIBaseData();
            //List<SerUIItem> list=new List<SerUIItem> ();
            //System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var v in uIAbout.aboutItems)
            {
                 list.Add((v.BehaviorName, v.ItemName));
            }
        }
#if UNITY_EDITOR
        //[MenuItem("GameObject/UI/About")]
        //public static void SelectAbout()
        //{
        //    var uw= EditorWindow.GetWindow<UIAboutWindow>();
        //   // var path = Path.Combine("Assets", "Resources", UIUtil.AboutPath);
        //    uw.uIAbout = UIUtil.LoadAbout();
        //    uw.Init();
        //}
        //[MenuItem("Saber/UI/About",false,1)]
        //public static void SelectAboutWindow()
        //{
        //    SelectAbout();
        //}
#endif
        void DrawAddAboutItem()
        {
            GUILayout.BeginHorizontal();
            //aboutItem = (AboutItem)EditorGUILayout.ObjectField(aboutItem, typeof(AboutItem), false);
            GUILayout.EndHorizontal();
        }
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            //绘制标题
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("UI元素命名规则");
            GUI.skin.label.fontSize = 18;
            GUILayout.Label($"预制体位置--->{UIUtil.AboutPath}");

            GUI.skin.label.fontSize = 15;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUILayout.Space(50);

            foreach (var f in list)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(150);
                GUILayout.Label(f.Item1 + "---->",GUILayout.Width(300));
                GUILayout.Label(f.Item2);
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(30);

            DrawAddAboutItem();

            GUILayout.EndVertical();
        }


    }
}
