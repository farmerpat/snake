using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
	private GameObject foodPellet;

	// snake cells' pixels per unit is 100
	// sprites hxw = 53x53
	// so, the width of the sprite is 53/100 = .53
	void Start () {
		foodPellet = (GameObject)Instantiate(Resources.Load("FoodPellet"));	
		foodPellet.transform.position = this.GetPelletPos ();

	}
	
	void Update () {
		if (foodPellet == null) {
			foodPellet = (GameObject)Instantiate(Resources.Load("FoodPellet"));	
			foodPellet.transform.position = this.GetPelletPos ();
			// place it in the grid somewhere not in the snake

		}
	}

	public void PelletConsumed () {
		Destroy (foodPellet);
		foodPellet = null;

	}

	private Vector3 GetPelletPos () {
		// calculate the width instead, or get it from the config,
		// which will specify what size sprite to used based on resolution
		// find as calculation of viewport
		// this is lame...
		// first calculate all possibles points in Start.
		// store them in a 2d list that represents the game board
		// randomly pick x and y indicies. if theres no snake there,
		// check again
		// a different way would be to have the grid basically keep
		// a copy of the game board. e.g. it would show the places
		// where the snake already is

		int xMul = Random.Range(-4, 5);
		int yMul = Random.Range (-2, 3);
		Vector2 tryFirstCorner = new Vector2 ();
		Vector2 tryOtherCorner = new Vector2 ();

		float halfLength = .55f / 2;
		float halfWidth = halfLength;
		tryFirstCorner.x = (xMul * .55f) + halfLength;
		tryFirstCorner.y = (yMul * .55f) + halfWidth;

		tryOtherCorner.x = (xMul * .55f) - halfLength;
		tryOtherCorner.y = (yMul * .55f) - halfWidth;

		Collider2D spaceOccupied = Physics2D.OverlapArea (tryFirstCorner, tryOtherCorner);

		while (spaceOccupied != null) {
			xMul = Random.Range(-4, 5);
		    yMul = Random.Range (-2, 3);

			tryFirstCorner.x = (xMul * .55f) + halfLength;
			tryFirstCorner.y = (yMul * .55f) + halfWidth;

			tryOtherCorner.x = (xMul * .55f) - halfLength;
			tryOtherCorner.y = (yMul * .55f) - halfWidth;

			spaceOccupied = Physics2D.OverlapArea (tryFirstCorner, tryOtherCorner);

		}

//		Debug.Log ("bounds:");
//		SpriteRenderer sr = foodPellet.GetComponent<SpriteRenderer> ();
//		Debug.Log (sr.sprite.bounds);
//		Debug.Log ("rect:");
//		Debug.Log (sr.sprite.rect);

		return new Vector3 (xMul * .55f, yMul * .55f, 0.0f);

		// make sure its in bounds and is not in the snake!
	}
}
