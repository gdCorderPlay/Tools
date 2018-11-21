using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;
namespace GD
{

    /// <summary>
    /// 曲线面板
    /// </summary>
    public class ChartViewer : MonoBehaviour
    {
        public CanvasGroup group;
        public RawImage view;
        public GridLayoutGroup gridV;
        public GridLayoutGroup gridH;
        public Text mainTitle;
        public Text vTitle;
        public VItem[] vItems;
        public VItem[] hItems;
        public Image mask;
        public float maxCount;
        public Text scale;
        public GameObject rightTitle;
        public int hScale;

        public void Show()
        {
            //group.DOFade(1, 0.5f);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            //group.alpha = 0;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 切换曲线
        /// </summary>
        public virtual void ChangePicture(PictureItemData showPic)
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
            //设置x方向的信息
            gridH.cellSize = new Vector2((float)300 / (showPic.widthCount), 140);
            showNum = showPic.maxTime;
            showNum = showNum / (showPic.widthCount);
            for (int i = 0; i < showPic.widthCount; i++)
            {
                hItems[i].gameObject.SetActive(true);
                float vValue = (showNum * i) / (1000f* hScale);
                hItems[i].SetItem(vValue.ToString());
            }
            hItems[showPic.widthCount].gameObject.SetActive(true);
            hItems[showPic.widthCount].SetItem((showPic.maxTime / (1000f*hScale)).ToString());

            StartCoroutine(WWW_Tex("file://" + Application.streamingAssetsPath + "/Texture/" + showPic.texture + ".png"));

        }
        /// <summary>
        /// 更新显示的
        /// </summary>
        /// <param name="fillAmount"></param>
        public virtual void UpdatePicture(float fillAmount)
        {
            mask.fillAmount = (maxCount * fillAmount) / (hScale);
        }
        public virtual void UpdatePicture(int fillAmount)
        {
            mask.fillAmount = ((float)fillAmount * hScale) / (maxCount);
        }
        protected void HideItem()
        {
            for (int i = 0; i < vItems.Length; i++)
            {

                vItems[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected IEnumerator WWW_Tex(string url)
        {

            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && www.error == null)
            {
                // wwwTexture   value  value= www.texture;
                view.texture = www.texture;
            }
        }


    }

}