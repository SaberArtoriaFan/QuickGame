using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Saber.UI;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
#region
//池骋
#endregion
namespace Saber
{
    public class UIAboutWindowOdin :OdinEditorWindow
    {
        private bool isCanChange = false;
        [ListDrawerSettings(DraggableItems = false,IsReadOnly =true, HideAddButton =true,HideRemoveButton = true,ShowFoldout =false)]
        [SerializeField]
        [ReadOnly]
        [ShowIf("@!isCanChange")]
        [LabelText("UI命名规则")]
        //[HideLabel]
        List<AboutItem> aboutItems = new List<AboutItem>();

        [ShowIf("@isCanChange")]
        [LabelText("UI命名规则")]
        [ListDrawerSettings( ShowFoldout = false,OnTitleBarGUI = nameof(TitleBarButtonDrawer))]
        [SerializeField]
        //[HideLabel]
        List<AboutItem> canChangeAboutItems = new List<AboutItem>();

        public bool IsCanChange { get => isCanChange; set
            {
                isCanChange = value;
                OnDisable();
                OnEnable();
                
            }
        }

        [MenuItem("GameObject/UI/About")]
        public static void SelectAbout()
        {
            var uw = EditorWindow.GetWindow<UIAboutWindowOdin>();
            //uw.IsCanChange = false;
            // var path = Path.Combine("Assets", "Resources", UIUtil.AboutPath);
            //uw.uIAbout = UIUtil.LoadAbout();
            //uw.Init();
        }
        [MenuItem("Saber/UI/About", false, 1)]
        public static void SelectAboutWindow()
        {
            var uw = EditorWindow.GetWindow<UIAboutWindowOdin>();
            uw.IsCanChange = true;
        }
        private void Refresh()
        {
            aboutItems.Clear();
            canChangeAboutItems.Clear();
            var about = UIUtil.LoadAbout();
            if (!this.isCanChange)
            {
                aboutItems.AddRange(about.aboutItems.Select((u)=>u.Clone()));
                foreach (var item in aboutItems)
                    item.isError = false;
            }
            else
            {
                canChangeAboutItems.AddRange(about.aboutItems.Select((u) => u.Clone()));
                foreach (var item in canChangeAboutItems)
                    item.isError = false;
            }
        }
        private void TitleBarButtonDrawer()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            { 
                Refresh();
            }
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.Save))
            {
                var about = UIUtil.LoadAbout();
                var origin = about.aboutItems.ToArray();
                about.aboutItems = this.canChangeAboutItems.ToArray();
                if (about.CheckRight())
                {
                    about.Save();
                    foreach(var item in canChangeAboutItems)
                        item.isError = false;
                    //Refresh();
                }
                else
                {
                    about.aboutItems = origin;
                    about.Save();
                }
            }

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            var about = UIUtil.LoadAbout();
            if (!this.isCanChange)
            {
                aboutItems.AddRange(about.aboutItems.Select((u) => u.Clone()));
                foreach (var item in aboutItems)
                    item.isError = false;
            }
            else
            {
                canChangeAboutItems.AddRange(about.aboutItems.Select((u) => u.Clone()));
                foreach (var item in canChangeAboutItems)
                    item.isError = false;
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            aboutItems.Clear();
            canChangeAboutItems.Clear();
        }
    }

}
