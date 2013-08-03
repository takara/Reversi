using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	void Awake () {
//		_sample2();
	}
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
/**
* サンプル
*
* どこかのサイトで拾ったコード
*/
	private void _sample1() {
		Camera cam = gameObject.GetComponent<Camera>();
		float baseAspect = 720f/1280f;
		float nowAspect = (float)Screen.height/(float)Screen.width;
		float changeAspect;
  
		if(baseAspect>nowAspect){   
	 		changeAspect = nowAspect/baseAspect;
	 		cam.rect=new Rect((1-changeAspect)*0.5f,0,changeAspect,1);
		} else {
	 		changeAspect = baseAspect/nowAspect;
	 		cam.rect=new Rect(0,(1-changeAspect)*0.5f,1,changeAspect);
		}
		Destroy(this);
	}
/**
* サンプル
*/
	private void _sample2() {
		Camera cam = gameObject.GetComponent<Camera>();
		float baseAspect = 1280f/720f;
		float nowAspect = (float)Screen.height/(float)Screen.width;
		float changeAspect;
Debug.Log("cam:"+cam.rect);
		changeAspect = nowAspect/baseAspect;  
 		cam.rect=new Rect((1-changeAspect)*0.5f,0,changeAspect,1);
/*		if(baseAspect>nowAspect){   
	 		changeAspect = nowAspect/baseAspect;
	 		cam.rect=new Rect((1-changeAspect)*0.5f,0,changeAspect,1);
		} else {
	 		changeAspect = baseAspect/nowAspect;
	 		cam.rect=new Rect(0,(1-changeAspect)*0.5f,1,changeAspect);
		}*/
		Destroy(this);
	}
}
