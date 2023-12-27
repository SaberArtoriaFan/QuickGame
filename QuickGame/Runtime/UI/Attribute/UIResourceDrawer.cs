using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#region
//保持UTF-8
#endregion
namespace Saber.UI
{
    public enum ResourcesLoadType
    {
        Resources
    }
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class UIResourcesAttribute : PropertyAttribute
    {
        public readonly string address;
        public readonly ResourcesLoadType loadType;

        public UIResourcesAttribute(string address, ResourcesLoadType loadType=ResourcesLoadType.Resources)
        {
            this.address = address;
            this.loadType = loadType;
            //Debug.Log($"看看实力{address}");
        }
    }

    //public class UIResourceDrawer :PropertyDrawer
    //{
        
    //}
}
