using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GDEditor
{
    /// <summary>
    /// 脚本格式生成器
    /// </summary>
    public class ScriptFormat : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            string formatPath = Application.dataPath+"/GD/Editor/Format.cs";
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string[] directorys = path.Split('/');
                string scriptName = directorys[directorys.Length - 1];
                scriptName = scriptName.Replace(".cs", "");

                string allText = File.ReadAllText(formatPath);
                //更换脚本名称
                allText = allText.Replace("Format",scriptName );
                //添加脚本的创建时间
                allText = allText.Replace("*创建时间*", System.DateTime.Now.ToString());
                File.WriteAllText(path, allText);
                DataBase.Refresh();
            }
        }
    }
    public class DataBase
    {
        public static void Refresh()
        {
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

        }

    }
}
