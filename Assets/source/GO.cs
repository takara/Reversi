using UnityEngine;
using System.Collections;

public class GO : MonoBehaviour {
/**
* ???????
*/
	public GameObject boardPrefab;
/**
* ???
*/
	public void Start() {
		//GameObject board = (GameObject)
		Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
}
