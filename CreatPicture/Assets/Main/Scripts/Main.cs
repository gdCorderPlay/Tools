using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace GD
{
    public class Main : MonoBehaviour
    {


        // public Texture2D texture;
        public Color[] color;
        public Color bg;
        public int width;
        public int hight;
        private int origin;
        public string text;
        public int index;
        private string path;
        Vector3 _pos1;

        private Dictionary<int, Vector3> pointDic;
        private void Start()
        {
            pointDic = new Dictionary<int, Vector3>();
            _pos1 = new Vector3(0f, 0.5f, 0);
            //for (int i   value  value= 0; i < 200000; i++)
            //{
            //    int random   value  value= Random.Range(0, 2);
            //    switch (random)
            //    {
            //        case 0:
            //            _pos1 +  value  value= new Vector3(0.01f / 1000, 0.01f / 10, 0);
            //            break;
            //        case 1:
            //            _pos1 +  value  value= new Vector3(0.01f / 1000, -0.01f / 10, 0);
            //            break;
            //    }
            //    pointDic.Add(i, _pos1);

            //}
        }
        bool running;
        public void OnClick()
        {
            if (!running)
                StartCoroutine(MakeAPictures());
        }
        IEnumerator MakeAPicture()
        {
            running = true;
            yield return 0;
            Reader.instance.Read(text, index);
            path = Application.dataPath + "/Main/Out/" + text + "_" + index + ".png";
            Texture2D texture1 = new Texture2D(width, hight, TextureFormat.RGBA32, false);
            ///背景底色
            Color[] pixels = texture1.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = bg;
            }
            texture1.SetPixels(pixels);

            int jianGe = 10;
            int start = (int)(Reader.instance.datas[origin]);
            for (int i = jianGe; i < Reader.instance.datas.Count; i += jianGe)
            {
                int end = (int)(Reader.instance.datas[origin + i]);
                Render(i / jianGe, ref start, end, texture1);
            }

            //保存图片
            byte[] data = texture1.EncodeToPNG();
            File.WriteAllBytes(path, data);
            AssetDatabase.Refresh();
            running = false;
            Debug.Log("图片生成完成");
        }


        IEnumerator MakeAPictures()
        {
            running = true;
            yield return 0;
            string _path = Application.streamingAssetsPath + "/Config";
            // string[] files = Directory.GetFiles(_path);
            string[] directories = Directory.GetDirectories(_path);
            for (int i = 0; i < directories.Length; i++)
            {
                string head = directories[i].Split('_')[1];
                string[] files = Directory.GetFiles(directories[i]);
                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j].EndsWith(".txt")|| files[j].EndsWith(".csv"))
                    {
                        string str = files[j].Substring(directories[i].Length + 1);
                        str = str.Trim('.', 't', 'x');
                        str = head + "_" + str;
                        List<List<float>> pics = Reader.instance.GetData(files[j], str, hight);
                        // Debug.Log(str);
                        Creat(str, pics, Reader.instance.scale, Reader.instance.offset, Reader.instance.offsetPic, (int)Reader.instance.width);

                    }
                }

            }
        }
        void Creat(string name, List<List<float>> data, int scale, int offset, int offsetPic, int dataWidth)
        {
            if (scale <= 0)
            {
                scale = 1;
            }

            // Debug.Log(name);
            string _path = Application.streamingAssetsPath + "/Texture/" + name + ".png";
            Texture2D texture1 = new Texture2D(width, hight, TextureFormat.RGBA32, false);
            ///背景底色
            Color[] pixels = texture1.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = bg;
            }
            texture1.SetPixels(pixels);
            // int jianGe = 10;
            float jianGe = (float)dataWidth / width;
            //if (jianGe <= 0)
            //    jianGe = 1;
            for (int i = 0; i < data.Count; i++)
            {
                // Debug.Log(data[i].Count);
                if (data[i].Count > origin)
                {
                    //  int start = (int)(data[i][origin]);
                    int start = (int)(data[i][origin]) / (int)scale + (int)(offsetPic * offset);
                    for (float j = jianGe; j < data[i].Count; j += jianGe)
                    {
                        int end = (int)(data[i][origin + (int)j]) / (int)scale + (int)(offsetPic * offset);
                        Render((int)(j / jianGe), ref start, end, texture1, color[i]);
                    }
                }
            }

            // datas.Add((int)(data) / 1000 + 150);//测试
            ////保存图片
            byte[] picData = texture1.EncodeToPNG();
            File.WriteAllBytes(_path, picData);
            AssetDatabase.Refresh();
            Debug.Log("图片生成完成" + name + "ofsset" + offset);

        }
        private void Render(int x, ref int start, int end, Texture2D texture1)
        {
            if (start < end)
            {
                for (int y = start; y <= end; y++)
                {
                    RenderPoint(x, y, 4, texture1);
                    //texture1.SetPixel(x, y, color);
                }
            }
            else
            {

                for (int y = start; y >= end; y--)
                {
                    RenderPoint(x, y, 4, texture1);
                    //texture1.SetPixel(x, y, color);
                }
            }
            start = end;
        }
        private void Render(int x, ref int start, int end, Texture2D texture1, Color _color)
        {
            if (start < end)
            {
                for (int y = start; y <= end; y++)
                {
                    RenderPoint(x, y, 1, texture1, _color);
                    //texture1.SetPixel(x, y, color);
                }
            }
            else
            {

                for (int y = start; y >= end; y--)
                {
                    RenderPoint(x, y, 1, texture1, _color);
                    //texture1.SetPixel(x, y, color);
                }
            }
            start = end;
        }
        private void RenderPoint(int x, int y, int count, Texture2D texture1)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    texture1.SetPixel(x + i, y + j, color[0]);
                }
            }


        }
        private void RenderPoint(int x, int y, int count, Texture2D texture1, Color _color)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    texture1.SetPixel(x + i, (int)(y + j), _color);
                }
            }


        }
    }

}
