using UnityEngine;
using System.Collections;

public class GO : MonoBehaviour {
	public GameObject boardPrefab;
	private GameObject board;

	// Use this for initialization
	public void Start() {
		board = (GameObject)Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
/*	private void print_board() {
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
	}*/
}
