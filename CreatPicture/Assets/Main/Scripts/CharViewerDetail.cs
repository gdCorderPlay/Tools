using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GD
{
    /// <summary>
    /// 曲线图的细节放大显示
    /// </summary>
    public class CharViewerDetail : ChartViewer
    {
        public VItem timeLine;

        private int realTime;
        public override void ChangePicture(PictureItemData showPic)
        {
            HideItem();
            if (showPic.pictureCount > 2)
            {
                rightTitle.SetActive(true);
            }
            else
            {
                rightTitle.SetActive(false);
            }
            hScale = showPic.hScale;
            maxCount = showPic.maxTime;
            realTime = showPic.realTime;
            mainTitle.text = showPic.mainTitle;
            vTitle.text = showPic.VTitle;

            gridV.cellSize = new Vector2(300, (float)140 / (showPic.hightCount));

            int showNum = showPic.maxValue - showPic.minValue;
            showNum = showNum / (showPic.hightCount);
            //设置幂次方
            scale.text = (Tools.GetLength(showNum) - showPic.k).ToString();
            //设置y方向的信息
            int hValue;
            for (int i = 0; i < showPic.hightCount; i++)
            {
                vItems[i].gameObject.SetActive(true);
                hValue = (showPic.maxValue - showNum * i) / Tools.Floor(showNum);
                vItems[i].SetItem(hValue.ToString());
            }
            vItems[showPic.hightCount].gameObject.SetActive(true);
            hValue = showPic.minValue / Tools.Floor(showNum);
            vItems[showPic.hightCount].SetItem(hValue.ToString());
            ////设置x方向的信息
            //gridH.cellSize = new Vector2((float)300 / (showPic.widthCount), 140);
            //showNum = showPic.maxTime;
            //showNum = showNum / (showPic.widthCount);
            //for (int i = 0; i < showPic.widthCount; i++)
            //{
            //    hItems[i].gameObject.SetActive(true);
            //    float vValue = (showNum * i) / 10000f;
            //    hItems[i].SetItem(vValue.ToString());
            //}
            //hItems[showPic.widthCount].gameObject.SetActive(true);
            //hItems[showPic.widthCount].SetItem((showPic.maxTime / 10000f).ToString());

            StartCoroutine(WWW_Tex("file://" + Application.streamingAssetsPath + "/Texture/" + showPic.texture + ".png"));
        }

        public override void UpdatePicture(int fillAmount)
        {
            if (fillAmount > realTime)
                fillAmount = (int)realTime;
            float rate = (float)fillAmount * 10 / maxCount;
            mask.fillAmount = rate;

            timeLine.SetItem(fillAmount.ToString());
            RectTransform rt = timeLine.GetComponent<RectTransform>();
            RectTransform rtView=   view.GetComponent<RectTransform>();
            if (rate <= 1)
            {
                rt.anchoredPosition = new Vector2(300 * rate, 0);
                rtView.anchoredPosition = Vector2.zero;
            }
            else
            {
                rt.anchoredPosition = new Vector2(300, 0);
                rtView.anchoredPosition = new Vector2(-300* (rate-1), 0);
            }
        }

    }
 
}

