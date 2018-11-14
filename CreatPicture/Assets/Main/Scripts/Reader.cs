using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
public class Reader : MonoBehaviour
{
    
    //public string text;
    public static Reader instance;
    private void Awake()
    {
        instance    = this;

    }
    public float min    = 0, max   = 0;
    public int width;

    [HideInInspector]
    public List<int> datas;
    public void Read(string text, int index)
    {
        datas.Clear();
        string[] lines  = File.ReadAllLines(Application.dataPath + "/Main/Data/" + text + ".txt");
        int length   = lines[0].Split(',').Length;

        for (int i   = 0; i < lines.Length; i++)
        {
            string[] strs   = lines[i].Split(',');
            float data   = float.Parse(strs[index]);
           
             datas.Add((int)(data)/1000+150);//测试
            // datas.Add((int)(data)/100+1500);
            // datas.Add((int)(data) / 200 + 3000);//第四组数据
            // datas.Add((int)(data) / 120000 + 1500);//第五组数据
            //  datas.Add((int)(data) / 40 + 1500);//第八，九组数据   
            // datas.Add((int)(data) / 50 + 3000);//第十一，十二组数据     

        }
    }

    public int scale;
    public int offset;
    public int offsetPic;
    public List<List<float>> GetData(string filePath,string saveName,int hight = 600)
    {
        int k = 1;
        // string text = Application.streamingAssetsPath + "/Config/" + fileName + ".txt";
        string text = filePath;
       string[] lines   = File.ReadAllLines(text);

        int length   = lines[0].Split(',').Length;
        max = float.Parse(lines[0].Split(',')[1])*k;
        min = max;
        List<List<float>> pictureDatas  = new List<List<float>>();
        for (int i  = 0; i < length; i++)
        {
            pictureDatas.Add(new List<float>());
        }
        for (int i   = 0; i < lines.Length; i++)
        {
            string[] strs   = lines[i].Split(',');

            for (int j   = 1; j < strs.Length; j++)
            {
                float data  = float.Parse(strs[j])*k;
                if (data > max)
                {
                    max   = data;
                }
                else if (data < min)
                {
                    min   = data;
                }
                pictureDatas[j - 1].Add(data);
            }
        }
        if (max - min < 10)
        {
            k = 100000;
        }
        int _min,_max;
        _min = (int)(min*k);
        _max = (int)(max*k);

      int hightCount=  FormatMaxAndMin(ref _min, ref _max);

        //图片整体的缩放值
         scale  = (int)((_max - _min) / hight);
        //归整至1 2 5  
       // scale = FormatInt(scale);
       // scale *= 2;
        //图片整体的偏移值
        offset = 0;
        offsetPic = (hight / hightCount);

        int condition = _min + offsetPic * offset * scale;
        if (condition < 0)
        {
            while (condition <= 0)
            {
                offset++;
                condition = _min + offsetPic * offset * scale;
            }
            offset--;
        }
        else if (condition > 0)
        {
            while (condition >= 0)
            {
                offset--;
                condition = _min + offsetPic * offset * scale;
            }
            offset++;
        }
        width = pictureDatas[0].Count;
       int widthCount= FormatWidth(ref width);

         PictureItemData pic = new PictureItemData();
        pic.texture = saveName;
        pic.maxValue = _max;
        pic.minValue = _min;
        pic.hightCount = hightCount;
        pic.pictureCount = pictureDatas.Count;
        pic.maxTime = (int)width;
        pic.widthCount = widthCount;
        pic.realTime = pictureDatas[0].Count;
        if (k == 1)
        {
            pic.k = 1;
        }else
        {
            pic.k = 6;
        }
           
       WriteXml("texture_" + saveName, pic);
      
        for(int i=0;i< pictureDatas.Count; i++)
        {
            for (int j = 0; j < pictureDatas[i].Count; j++)
            {
                pictureDatas[i][j] *= k;
            }
        }
        return pictureDatas;
    }
    /// <summary>
    /// 对时间线进行预处理
    /// </summary>
    /// <param name="num"></param>
    public int FormatWidth(ref int max )
    {
        //获取波动范围
        int length = max;
        //区间的大小
        int _scale = length / 3;
        _scale = FormatScale(_scale);
       
        int maxHead = max / _scale;
        if (max >= 0)
        {
            maxHead++;
            max = maxHead * _scale;
        }
        else
        {
            max = maxHead * _scale;
        }
        return (int)(maxHead);

    }

    /// <summary>
    /// 预处理最大值和最小值
    /// </summary>
    int FormatMaxAndMin(ref int min, ref int max )
    {
        //获取波动范围
        int length = max - min;
        //区间的大小
        int _scale = length / 4;
        _scale = FormatScale(_scale);
        int minHead = min / _scale;
        if (min >= 0)
        {
            min = minHead * _scale;
        }else
        {
            minHead--;
            min = minHead * _scale;
        }
        int maxHead = max / _scale;
        if (max >= 0)
        {
            maxHead++;
            max = maxHead * _scale;
        }
        else
        {
            max = maxHead * _scale;
        }
        return (int)(maxHead - minHead);
    }
 
    /// <summary>
    /// 取整数值的间距
    /// </summary>
  public int FormatScale(int num)
    {
        //获取位数
        int length = Tools. GetLength(num);
        int _scale = (int)System.Math.Pow(10, length - 1);
        if (_scale <= 0)
            _scale = 1;
        num = num / _scale;
        num ++;
        num *= _scale;
        return num;
    }

    int FormatInt(int num)
    {
        int length = Tools. GetLength(num);
        int _scale = (int)System.Math.Pow(10, length-1);
       if(num>5 * _scale)
        {
            num = 10 * _scale;
        }
        else if (num > 2* _scale)
        {
            num = 5 * _scale;
        }
        else 
        {
            num = 2* _scale;
        }
        return num;
    }
   
   
    public Dictionary<string,List< PictureItemData>> ReadXmlForDic()
    {
        Dictionary<string, List<PictureItemData>> dic = new Dictionary<string, List<PictureItemData>>();
        
        string path = Application.streamingAssetsPath + "/Manifest/manifest.xml";

        XDocument xd = XDocument.Load(path);

        foreach (XElement xe in xd.Root.Elements())
        {
            PictureItemData pic = new PictureItemData();
            pic.texture = xe.Element("texture").Attribute("value").Value;
            pic.mainTitle = xe.Element("MainTitle").Attribute("value").Value;
            pic.VTitle = xe.Element("VTitle").Attribute("value").Value;
            pic.minValue = int.Parse(xe.Element("Min").Attribute("value").Value);
            pic.maxValue = int.Parse(xe.Element("Max").Attribute("value").Value);
            pic.hightCount = int.Parse(xe.Element("hightCount").Attribute("value").Value);
            pic.pictureCount = int.Parse(xe.Element("pictureCount").Attribute("value").Value);
            pic.key = xe.Element("Key").Attribute("value").Value;
            pic.k = int.Parse(xe.Element("K").Attribute("value").Value);
            pic.maxTime = int.Parse(xe.Element("Time").Attribute("value").Value);
            pic.widthCount = int.Parse(xe.Element("widthCount").Attribute("value").Value);
            pic.realTime = int.Parse(xe.Element("RealTime").Attribute("value").Value);

            if (!dic.ContainsKey(pic.key))
            {
                List<PictureItemData> pics = new List<PictureItemData>();
                dic.Add(pic.key, pics);
            }
            dic[pic.key].Add(pic);
        }
        return dic;
    }
    /// <summary>
    /// 修改xml的配置信息
    /// </summary>
    /// <returns></returns>
    public void WriteXml(string picname,string attributeName,string value)
    {
        List<PictureItemData> picDatas = new List<PictureItemData>();
        string path = Application.streamingAssetsPath + "/Manifest/manifest.xml";

        XDocument xd = XDocument.Load(path);

        xd.Root.Element(picname).Element(attributeName).Attribute("value").SetValue(value);
        xd.Save(path);
    }
    public void WriteXml(string picname,PictureItemData data)
    {
        List<PictureItemData> picDatas = new List<PictureItemData>();
        string path = Application.streamingAssetsPath + "/Manifest/manifest.xml";

        XDocument xd = XDocument.Load(path);
        xd.Root.Element(picname).Element("texture").Attribute("value").SetValue(data.texture);
        xd.Root.Element(picname).Element("Min").Attribute("value").SetValue(data.minValue.ToString());
        xd.Root.Element(picname).Element("Max").Attribute("value").SetValue(data.maxValue.ToString());
        xd.Root.Element(picname).Element("hightCount").Attribute("value").SetValue(data.hightCount.ToString());
        xd.Root.Element(picname).Element("pictureCount").Attribute("value").SetValue(data.pictureCount.ToString());
        xd.Root.Element(picname).Element("K").Attribute("value").SetValue(data.k.ToString());
        xd.Root.Element(picname).Element("Time").Attribute("value").SetValue(data.maxTime.ToString());
        xd.Root.Element(picname).Element("widthCount").Attribute("value").SetValue(data.widthCount.ToString());
        xd.Root.Element(picname).Element("RealTime").Attribute("value").SetValue(data.realTime.ToString());
        xd.Save(path);
    }
}
public class PictureItemData
{
    public string mainTitle;
    public string VTitle ;
    public int minValue;
    public int maxValue;
    public int maxTime;
    public string texture;
    /// <summary>
    /// 切割份数
    /// </summary>
    public int hightCount;
    /// <summary>
    /// x,y,z
    /// </summary>
    public int pictureCount;
    public string key;
    /// <summary>
    /// 缩放底数
    /// </summary>
    public int k;
    /// <summary>
    /// 时间的切割份数
    /// </summary>
    public int widthCount;
    /// <summary>
    /// 图片的实际长度
    /// </summary>
    public int realTime;

}
