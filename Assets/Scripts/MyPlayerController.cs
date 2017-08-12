/*
 * in hindsight, it seems that making SnakeHead a child of Player may
 * not have been the best move. Player havinga public GameObject member
 * whose value was SnakeHead probalby makes more sense.
 * consider refactoring
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyPlayerController : MonoBehaviour {
	private GameObject head;
	public GameObject BodyCellPool;
	public GameObject BodyCell;
	public int moveDelay = 5;
	public float deadZoneSize = .1f;

	private float negativeInputTolerance;
	private float positiveInputTolerance;
	private float bodyUnit;
	// number of frames between movement
	private int frameCycle = 0;
	private string direction;
	private List<GameObject> body;
	private AudioSource audioSource;

	private bool addCell = false;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = deadZoneSize * -1;
		// calculate this based on the head hitbox, you fool!
		bodyUnit = .55f;
		head = GameObject.Find ("SnakeHead");
		body = new List<GameObject> ();
		audioSource = GetComponent<AudioSource> ();

		// randomly select starting direction
		int r = Random.Range(0, 4);

		switch (r) {
		case 0:
			direction = "up";
			break;
		case 1:
			direction = "down";
			break;
		case 2:
			direction = "left";
			break;
		case 3:
			direction = "right";
			break;

		}

		body.Add (head);
	}
	
	void Update () {
		frameCycle++;

		if (Input.GetKeyDown (KeyCode.UpArrow) || (Input.GetAxis("Vertical") > positiveInputTolerance)) {
			direction = "up";

		} else if (Input.GetKeyDown (KeyCode.DownArrow) || (Input.GetAxis("Vertical") < negativeInputTolerance)) {
			direction = "down";

		} else if (Input.GetKeyDown (KeyCode.LeftArrow) || (Input.GetAxis("Horizontal") < negativeInputTolerance)) {
			direction = "left";

		} else if (Input.GetKeyDown (KeyCode.RightArrow) || (Input.GetAxis("Horizontal") > positiveInputTolerance)) {
			direction = "right";

		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			addCell = true;

		}

		if (frameCycle == moveDelay) {
			frameCycle = 0;

			if (addCell) {
				addCell = false;
				this.SpawnBodyPart ();

			}

			this.AdvanceBody ();
		}
	}

	private void SpawnBodyPart () {
		// pull a cell from the snakeCellObjectPool
		// as an exercise in optimization
		// and place it at the same location of transform.pos
		// for now, just instantiate a new one and add it to the body list

		GameObject cell = (GameObject)Instantiate (BodyCell);

		// disable it first so that it doesn't show up in the middle of the viewport
		cell.SetActive (false);

		body.Add (cell);
	}

	private void AdvanceBody() {
		Vector3 nextPos = this.head.transform.position;
		Vector3 tempPos;

		switch (direction) {
		case "up":
			nextPos.y += this.bodyUnit;
			break;

		case "down":
			nextPos.y -= this.bodyUnit;
			break;

		case "left":
			nextPos.x -= this.bodyUnit;
			break;

		case "right":
			nextPos.x += this.bodyUnit;
			break;

		}

		// see if the head is going to collide with anything (use RayCast ?)
		// a la RogueLike tut MovingObject script...
		// boxCollider.enabled = false;
		// RaycastHit2D hit = Physics2D.Linecast (start, end, blockingLayer);
		// boxCollider.enabled = true;
		// if not, move the head one unit in the direction of direction,
		// move head of the other body parts to the position of
		// the body part before it
		// otherwise
		// if its going to collide with the border, lose
		// if its going to collide with a body part, lose
		// if its going to collide with food, add from the object pool to the body,
		// increase score, move
		// for now, just move
		for (int i = 0; i < body.Count; i++) {
			// try disabling the hit boxes
			// ... but what about the head colliding w/ body 

			tempPos = body[i].transform.position;
			body[i].transform.position = nextPos;

			// see if its disabled and enable it if so (e.g. a new body part)
			if (!body [i].activeSelf) {
				body[i].SetActive (true);
			}

			//body [i].GetComponent<BoxCollider2D> ().enabled = false;
			nextPos = tempPos;

		}

		// and then re-enabling them
		for (int i = 0; i < body.Count; i++) {
			//body [i].GetComponent<BoxCollider2D> ().enabled = true;

		}
	}

	public void OnChildTriggerEnter2D (Collider2D child, Collider2D other) {
		if (other.CompareTag ("Food")) {
			Destroy (other.gameObject);
			this.SpawnBodyPart ();

		} else if (other.CompareTag ("Player")) {
			// if there are still board spaces available, game over
			// if there aren't spaces available, win game
			// play appropriate sounds

			// play death sound
			audioSource.Play();
			Debug.Log ("i hit myself. rip");

		} else if (other.CompareTag ("Wall")) {
			// these dont exist yet so...
			Debug.Log ("i hit the wall");

		}
	}
}
