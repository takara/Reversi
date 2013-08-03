using UnityEngine;
using System.Collections;

public class GO : MonoBehaviour {
/**
* ボードプレハブ
*/
	public GameObject boardPrefab;
/**
* 初期化
*/
	public void Start() {
		//GameObject board = (GameObject)
		Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
}
