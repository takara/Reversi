using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	void Awake () {
	  Camera cam = gameObject.GetComponent<Camera>();
	  float baseAspect = 720f/1280f;
	  float nowAspect = (float)Screen.height/(float)Screen.width;
	  float changeAspect;
	  
	  if(baseAspect>nowAspect){   
	   changeAspect = nowAspect/baseAspect;
	   cam.rect=new Rect((1-changeAspect)*0.5f,0,changeAspect,1);
	  }else{
	   changeAspect = baseAspect/nowAspect;
	   cam.rect=new Rect(0,(1-changeAspect)*0.5f,1,changeAspect);
	  }
	  Destroy(this);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
