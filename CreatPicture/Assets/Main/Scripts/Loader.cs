using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Loader : MonoBehaviour {

    public string Url;
    public RawImage image;
    private void Start()
    {

       // StartCoroutine(Init());
      // StartCoroutine(WWW_Tex("file://"+Application.streamingAssetsPath + "/Texture/1_1.png"));

    }

    Texture2D wwwTexture;
    IEnumerator WWW_Tex(string url)
    {
        yield return new WaitForSeconds(1f);
        float time  = Time.time;
        WWW www   = new WWW(url);
        yield return www;
        if (www.isDone && www.error   ==null)
        {
           // wwwTexture   value  value= www.texture;
            image.texture  = www.texture;
        }
        Debug.Log(Time.time - time);
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(1f);
        float time  = Time.time;
        byte[] data = File.ReadAllBytes(Application.streamingAssetsPath + "/1_1.png");
        wwwTexture   = new Texture2D(15000, 6000);
        wwwTexture.LoadImage(data);
        image.texture   = wwwTexture;
        yield return null;
        Debug.Log(Time.time-time);
    }
}
