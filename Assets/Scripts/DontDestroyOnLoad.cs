using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

	static GameObject thisGO;

	void Awake () {
		if (thisGO != null) {
			Destroy(gameObject);
			return;
		}

		thisGO = gameObject;
		DontDestroyOnLoad(gameObject);
	}
}
