using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GDEditor
{
    public class FormatFile : Editor
    {

        [MenuItem("GD/File/删除文件中符合条件的行内容")]
        public static void DoFormat()
        {
            string[] dics = Directory.GetDirectories(Application.dataPath + "/GD/Ignore");
            string[] lineFormats = File.ReadAllLines(Application.dataPath + "/GD/Ignore/IgnoreLine.txt");
            string[] fileFormats = File.ReadAllLines(Application.dataPath + "/GD/Ignore/ValidFile.txt");
            for (int d = 0; d < dics.Length; d++)
            {
                string[] files = Directory.GetFiles(dics[d]);
                for (int i = 0; i < files.Length; i++)
                {
                    if (CheckFileType(files[i], fileFormats))
                    {
                        Format(files[i], lineFormats);
                    }
                }
            }
        }
        /// <summary>
        /// 删除文件中的符合格式的行
        /// </summary>
        /// <param name="fileName"></param>
        static void Format(string fileName, string[] format)
        {
            bool needChange = false;
            //string[] datas=   File.ReadAllLines(path + "/" + fileName + ".csv");
            string[] datas = File.ReadAllLines(fileName);
            List<int> index = new List<int>();
            for (int i = 0; i < datas.Length; i++)
            {

                if (datas[i] == "")
                {
                    Debug.Log("第" + i + "行是空白行！！！！");
                }
                if (CheckFormat(datas[i], format))
                {
                    needChange = true;
                }
                else
                {
                    index.Add(i);
                }
            }
            if (needChange)
            {
                ///创建新的文件
                string[] outData = new string[index.Count - 1];
                for (int i = 0; i < outData.Length; i++)
                {
                    outData[i] = datas[index[i]];
                }

                File.WriteAllLines(fileName, outData);
                File.AppendAllText(fileName, datas[index[outData.Length]]);
                DataBase.Refresh();
                Debug.Log("刷新完成");
            }
        }
        /// <summary>
        /// 判断需要删除的格式
        /// </summary>
        static bool CheckFormat(string str, string[] format)
        {
            for (int j = 0; j < format.Length; j++)
            {
                if (str.Equals(format[j]))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断文件的格式
        /// </summary>
        static bool CheckFileType(string str, string[] format)
        {

            for (int j = 0; j < format.Length; j++)
            {
                if (str.EndsWith(format[j]))
                {
                    return true;
                }
            }
            return false;
        }
        [MenuItem("GD/File/归整文件的行数")]
        public static void DoFileLineFormat()
        {
            string filePath = Application.dataPath + "/GD/Format/File";
            string[] files = Directory.GetFiles(filePath);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".csv"))
                {
                    DoFileLineFormat(files[i]);
                }
            }

            Debug.Log("归整完成");
        }
        /// <summary>
        /// 归整单个文件
        /// </summary>
        /// <param name="filePath"></param>
        static void DoFileLineFormat(string filePath)
        {
            string formatPath = Application.dataPath + "/GD/Format/标准/标准行数.csv";

            string[] datas = File.ReadAllLines(formatPath);

            string[] lines = File.ReadAllLines(filePath);
            int length = lines.Length;
            string str = lines[length - 1];
            Debug.Log(length + ":::::" + str);
            for (int i = 0; i < datas.Length; i++)
            {
                if (i < length)
                {

                    datas[i] = lines[i];
                }
                else
                {
                    datas[i] = str;
                }
            }
            File.WriteAllLines(filePath, datas);
            DataBase.Refresh();

        }
        [MenuItem("GD/File/检查文件的行数")]
        public static void CheckFileLine()
        {
            Dictionary<int, List<string>> keys = new Dictionary<int, List<string>>();
            string filePath = Application.dataPath + "/GD/Check";
            string[] files = Directory.GetFiles(filePath);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".csv"))
                {
                    int lineLength = File.ReadAllLines(files[i]).Length;
                    if (!keys.ContainsKey(lineLength))
                    {
                        List<string> names = new List<string>();
                        keys.Add(lineLength, names);
                    }
                    keys[lineLength].Add(files[i]);
                    Debug.Log(files[i]);
                }
            }

            foreach (int key in keys.Keys)
            {
                Debug.Log("行数为" + key + "的文件个数为" + keys[key].Count);
            }
            Debug.Log("检查文件的行数完成");
        }

        [MenuItem("GD/File/模板化每行的内容")]
        public static void FormatLine()
        {

        }

    }
}

