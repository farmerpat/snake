using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour {
	private MyPlayerController parentScript;

	void Start () {
		parentScript = transform.parent.gameObject.GetComponent<MyPlayerController> ();
		Debug.Log (parentScript);
	}

	void OnTriggerEnter2D (Collider2D other) {
		parentScript.OnChildTriggerEnter2D (GetComponent<Collider2D>(), other);

	}
}
