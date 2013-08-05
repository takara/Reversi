using UnityEngine;
using System.Collections;

/**
* リバーシークラス
*/
public class boardB : MonoBehaviour {
	private GameObject[,] marks;
	private Reversi rev;
	public GameObject prefubMark;
	private Reversi.typeOfPiece human_piece;

/**
* 起動時初期化
*/
	public void Awake() {
		rev = new Reversi();
		rev.setCallback(putMark);
		marks = new GameObject[8,8];
		name = "board";
	}
/**
* 初期化（開始時）
*/
	public void Start () {
		rev.init();
		human_piece = Reversi.typeOfPiece.Black;
		gui_update();
	}
	// Update is called once per frame
	public void Update() {
		if(rev.numOfEmpty() < 1) {
			return;
		}
		int x = -1, y = -1;
		if(rev.numOfCanPlacePieces() == 0){
			rev.pass();
			if(rev.numOfCanPlacePieces() == 0){
				// End
			}
		}
		if(rev.turnIs() == human_piece) {
			//Debug.Log("human:"+rev.turnIs());
			if(Input.GetMouseButtonDown(0)) {
				Vector3 clickDeltaPosition = Input.mousePosition;
				Ray ray = Camera.main.ScreenPointToRay(clickDeltaPosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)) {
				
					if(hit.collider.gameObject.name != "board(Clone)" ) {
						Debug.Log("not board:"+hit.collider.gameObject.name);
						return;
					}
					//Debug.Log(hit.point);
					//Debug.Log(hit.point.x);
					x = (int)(hit.point.x + 40) / 10;
					y = 7 - (int)(hit.point.z + 40) / 10;
					Debug.Log("x:"+x+" y:"+y);
					if(rev.canPlacePiece(x, y)) {
						rev.placePiece(x, y);
						gui_update();
					} else {
						Debug.Log("can not place x:"+x+" y:"+y);
						rev.canPlacePiece(x, y,true);
					}
				}
				//rev.printByAscii();
			}
		} else {
			//Debug.Log("cpu:"+rev.turnIs());
			rev.think(5,ref x,ref y);
			if(rev.canPlacePiece(x, y)) {
				rev.placePiece(x, y);
				gui_update();
				Debug.Log("cpu x:"+x+" y:"+y);
			} else {
				Debug.Log("can not place x:"+x+" y:"+y);
			}
			//rev.printByAscii();
		}
	}
/**
*
*/
	private void gui_update() {
		if(rev.turnIs() == Reversi.typeOfPiece.Black) {
			((GUIText)GameObject.Find("GUITurn").GetComponent("GUIText")).text = "Now turn Black";
		} else {
			((GUIText)GameObject.Find("GUITurn").GetComponent("GUIText")).text = "Now turn White";
		}
		((GUIText)GameObject.Find("GUIBlackNum").GetComponent("GUIText")).text = "Black : " + rev.numOfBlack();
		((GUIText)GameObject.Find("GUIWhiteNum").GetComponent("GUIText")).text = "White : " + rev.numOfWhite();
		((GUIText)GameObject.Find("GUIMessage").GetComponent("GUIText")).text = "Places of you can put are " + rev.numOfCanPlacePieces();
		//rev.printByAscii();
	}
/**
* 
*/
	private void putMark(int x,int y,Reversi.typeOfPiece typ) {
			//Debug.Log("putMark() x:"+ x + " y:"+y+" "+typ);
			//Debug.Log("board.putMark():"+rev.statusIs(x,y));
			if(rev.statusIs(x,y) == Reversi.typeOfPiece.Empty) {
				marks[x,y] = (GameObject)Instantiate(
					prefubMark, 
					new Vector3(x*10-30 -5, 2, 40 - (y*10) -5), 
					Quaternion.identity);
			}
			if(typ == Reversi.typeOfPiece.Black) {
				marks[x,y].transform.rotation = Quaternion.Euler(0, 0, 180);
			} else {
				marks[x,y].transform.rotation = Quaternion.Euler(0, 0, 0);
			}
	}
}
