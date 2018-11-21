using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLTest : MonoBehaviour {

	public Material mat;
	private bool isDraw;

	Vector3 _pos1;

	private Dictionary<int,Vector3> pointDic;

	void Start()
	{
		pointDic = new Dictionary<int, Vector3> ();
		_pos1 = new Vector3(0f,0.5f,0);

		for (int i = 0; i < 200000; i++) 
		{
			int random = Random.Range (0, 2);
			switch (random) {
			case 0:
				_pos1 += new Vector3 (0.01f/1000, 0.01f/10, 0);
				break;
			case 1:
				_pos1 += new Vector3 (0.01f/1000, -0.01f/10, 0);
				break;
			}
			pointDic.Add (i, _pos1);

		}
	}


	void OnPostRender() 
	{
		if (!mat)
		{
			Debug.LogError("没有材质球!");
			return;
		}

			mat.SetPass (0); //刷新当前材质
			GL.LoadPixelMatrix ();//设置pixelMatrix
			GL.Color (Color.red);
			GL.LoadOrtho ();
			GL.Begin (GL.LINES);
		for (int i = 0; i < pointDic.Count - 1; i++) 
		{

			Vector3 start = pointDic[i];
			Vector3 end = pointDic[i+1];
			GL.Vertex (start);
			GL.Vertex (end);
		}
		GL.End ();


	}

}