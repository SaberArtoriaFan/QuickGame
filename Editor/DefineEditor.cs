using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#region
//saber战棋
#endregion
public class DefineEditor 
{
    public static string SaberInitDefine = "SABER_INIT";
    /// <summary>
    /// [MenuItem("Saber/Define/[OFF]", false, 1000)]
    /// </summary>
    public static void SetAllTargetDefine()
    {
        foreach (var v in Enum.GetValues(typeof(BuildTargetGroup)))
        {
            SetDfineTargetGroup((BuildTargetGroup)v);
        }
    }
    static void SetDfineTargetGroup(BuildTargetGroup buildTarget)
    {
        // 动态获取 Android 平台的宏定义

        // 动态修改宏定义
        try
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget, out var defines);
            foreach (var define in defines)
            {
                if (define == SaberInitDefine) return;
            }
            string[] newDefine = new string[defines.Length + 1];
            Array.Copy(defines, 0, newDefine, 0, defines.Length);
            newDefine[defines.Length] = SaberInitDefine;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, newDefine);
        }
        catch(Exception e) {
            Debug.LogError($"在修改宏定义->{buildTarget.ToString()}时出错->{e}");
        }
    }

    //[MenuItem("TestTools/CheckDefineSymbols", false, 1000)]
    //public static void MenuTestDefineSymbols_Check()
    //{
    //    // 动态获取 Android 平台的宏定义
    //    string tmpSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

    //    string[] symbolArray = tmpSymbols.Split(';');

    //    for (int i = 0; i < symbolArray.Length; i++)
    //    {
    //        if(symbolArray[i] == "TEST_DEFINE_SYMBOLS_CHECK")
    //        {
    //            symbolArray[i] = "";
    //        }
    //    }

    //    tmpSymbols = "";
    //    for (int i = 0; i < symbolArray.Length; i++)
    //    {
    //        if (string.IsNullOrEmpty(symbolArray[i])) continue;

    //        if(tmpSymbols == "")
    //        {
    //            tmpSymbols += symbolArray[i];
    //        }
    //        else
    //        {
    //            tmpSymbols += ";";
    //            tmpSymbols += symbolArray[i];
    //        }
    //    }
        
    //    // 动态修改宏定义
    //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, tmpSymbols);
                
    //    // 取消勾选状态
    //    Menu.SetChecked("TestTools/CheckDefineSymbols", false);
    //}
    
}
