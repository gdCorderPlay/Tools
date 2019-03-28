using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace GD
{
    /// <summary>
    /// 键入注释
    /// </summary>
    public class CreatAsset : ScriptableWizard
    {

        [MenuItem("GD/MyKey #a", false, 14)]
        public static void MyKey()
        {
            Debug.Log("myKey");
           // EditorApplication.hie
        }


    }
    //=======================================================
    // 作者：GD
    // 描述：这人懒得一逼
    // 始于11/14/2018 3:10:22 PM
    //=======================================================
}

