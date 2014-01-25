using UnityEngine;
using System.Collections;

public class GamePadInput : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("A_1_Mac")) {
			Debug.Log("Hello player 1");
		}
	}
}
