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

        [MenuItem("GD/File/将两个文件拆散叠加")]
        public static void AddFile()
        {
            string filePath = Application.dataPath + "/GD/Add";
            string outPath = Application.dataPath + "/GD/Out";
            string[] files = Directory.GetFiles(filePath);
            int length=-1;
            List<List<string[]>> filesData = new List<List<string[]>>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".txt"))
                {
                    string[] lines = File.ReadAllLines(files[i]);
                    List<string[]> lineList = new List<string[]>();

                    for(int j = 0; j < lines.Length; j++)
                    {
                        lineList.Add(lines[j].Split(','));
                    }
                    filesData.Add(lineList);
                    ///取出最短的数据行
                    if (length < 0)
                    {
                        length = lines.Length;
                    }
                    else if(length> lines.Length)
                    {
                        length = lines.Length;
                    }
                }
            }
           // List<string[]> outFilesData = new List<string[]>();
            
            for(int index = 1; index <= 3; index++)
            {
                string[] outLines = new string[length];
                ///重新赋值
                for (int i = 0; i < filesData.Count; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        outLines[j]  +=  "," + filesData[i][j][index];
                    }
                }
                File.WriteAllLines(outPath + "/" + index + ".txt", outLines);
                DataBase.Refresh();
            }
        }
        [MenuItem("GD/File/将文件放大")]
        public static void ScaleFile()
        {
            string filePath = Application.dataPath + "/GD/Scale";
            string outPath = Application.dataPath + "/GD/Out";
            string[] files = Directory.GetFiles(filePath);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".txt"))
                {
                    string[] lines = File.ReadAllLines(files[i]);
                    int length = lines.Length;
                    string[] outFile = new string[(length-1)*100+1];
                    
                    for(int j=0;j<length-1; j++)
                    {
                        string[] strs = lines[j].Split(',');
                        float num1Start = float.Parse(strs[0]);
                        float num2Start= float.Parse(strs[1]);
                       
                        string[] strs2 = lines[j+1].Split(',');
                        float num1End = float.Parse(strs2[0]);
                        float num2End = float.Parse(strs2[1]);

                        float scale1 = (num1End - num1Start) / 100;
                        float scale2 = (num2End - num2Start) / 100;

                        for(int k=0;k<100;k++)
                        {
                            num1Start += k * scale1;
                            num2Start += k * scale2;
                            outFile[j * 100 + k] = num1Start + "," + scale2;

                        }
                    }
                    outFile[(length - 1) * 100] = lines[length - 1];
                    File.WriteAllLines(outPath + "/1.txt", outFile);
                    DataBase.Refresh();
                }

            }
            }
        [MenuItem("GD/File/读取html")]
        public static void Test()
        {
            string filePath = Application.dataPath + "/GD/html.html";

            string context = File.ReadAllText(filePath);

            Debug.Log(context);

        }

    }
}

