using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	float angle = 360f / 4 / Mathf.Rad2Deg;
	void Awake () {
//		_sample2();
	}
	// Use this for initialization
	void Start () {}
/**
* 画面更新
*/
	void Update () {
		if(angle * Mathf.Rad2Deg < 0) {
			return;
		}
		_moveCamera();
	}
/**
* カメラ回転
*/
	private void _moveCamera() {
		float radius = 125;
		GameObject target = (GameObject)GameObject.Find("board");
		Camera maincamera = gameObject.GetComponent<Camera>();
          Vector3 pos = target.transform.position;
          maincamera.transform.LookAt(pos);     // カメラをtargetの方向へ向かせるように設定する

          // オブジェクトの周りをカメラが円運動する
          maincamera.transform.position = new Vector3(
               pos.x/* + radius*/,
               pos.y + Mathf.Cos(angle) * radius,
               pos.z + Mathf.Sin(angle) * radius
          );
          Debug.Log(
          	"angle:"+Mathf.Rad2Deg * angle+
          	" y:"+(pos.y + Mathf.Cos(angle) * radius)+
          	" z:"+(pos.z + Mathf.Sin(angle) * radius)
          );
//          angle += 0.01f;
          angle -= 0.01f;
	}
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
