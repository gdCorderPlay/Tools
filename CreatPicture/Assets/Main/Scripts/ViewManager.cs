using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GD
{
    public class ViewManager : MonoBehaviour
    {

        public List<ChartViewer> views;
        private Dictionary<string, List<PictureItemData>> keyToPic;


        /// <summary>
        /// 切换曲线
        /// </summary>
        /// <param name="key"></param>
        public void ChangePicture(string key)
        {
            Debug.Log("*******************" + key);
            if (keyToPic == null)
                keyToPic = Reader.instance.ReadXmlForDic();
          
            if (!keyToPic.ContainsKey(key))
                return;
           // Debug.Log(key);
            List<PictureItemData> showPics = keyToPic[key];
            for (int i = 0; i < views.Count; i++)
            {
                views[i].Hide();
            }
            for (int i = 0; i < showPics.Count; i++)
            {
                views[i].Show();
                views[i].ChangePicture(showPics[i]);
            }

        }

        public void UpdatePicture(int fillAmount)
        {

            for (int i = 0; i < views.Count; i++)
            {
                views[i].UpdatePicture(fillAmount);
            }
        }

        private void Start()
        {

            // keyToPic = Reader.instance.ReadXmlForDic();
            // ChangePicture("01");

        }



       public int amount = 0;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                amount += 10;
                UpdatePicture(amount);
            }



        }
    }


}