using UnityEngine;
using System.Collections;

public class GO : MonoBehaviour {
	public GameObject boardPrefab;
	private GameObject board;
	private Reversi rev;
	private Reversi.typeOfPiece human_piece;

	// Use this for initialization
	public void Start() {
		board = (GameObject)Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		rev = (Reversi)board.GetComponent("Reversi");
		human_piece = Reversi.typeOfPiece.White;
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
					if(hit.collider.gameObject != board ) {
						Debug.Log("not board");
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
				rev.printByAscii();
			}
		} else {
			//Debug.Log("cpu:"+rev.turnIs());
			rev.think(3,ref x,ref y);
			if(rev.canPlacePiece(x, y)) {
				rev.placePiece(x, y);
				gui_update();
				Debug.Log("cpu x:"+x+" y:"+y);
			} else {
				Debug.Log("can not place x:"+x+" y:"+y);
			}
			rev.printByAscii();
		}
	}
/**
*
*/
	private void gui_update() {
		if(rev.turnIs() == Reversi.typeOfPiece.Black) {
			((GUIText)GameObject.Find("GUITurn").GetComponent("GUIText")).text = "Now Black turn Black";
		} else {
			((GUIText)GameObject.Find("GUITurn").GetComponent("GUIText")).text = "Now Black turn White";
		}
		((GUIText)GameObject.Find("GUIBlackNum").GetComponent("GUIText")).text = "Black : " + rev.numOfBlack();
		((GUIText)GameObject.Find("GUIWhiteNum").GetComponent("GUIText")).text = "White : " + rev.numOfWhite();
		((GUIText)GameObject.Find("GUIMessage").GetComponent("GUIText")).text = "Places of you can put are " + rev.numOfCanPlacePieces();
		//rev.printByAscii();
	}
/**
*
*/
	private void print_board() {
		rev.printByAscii();
		string s;
		s = "";
		for(int i2=0; i2<8; i2++){
			for(int j2=0; j2<8; j2++) {
				if( true == rev.canPlacePiece(i2, j2) ) {
					s += (char)('a'+i2);
					s += (char)('1'+j2);
					s += " ";
				}
			}
		}
		((GUIText)GameObject.Find("GUIMessage").GetComponent("GUIText")).text = s;
	}
}
