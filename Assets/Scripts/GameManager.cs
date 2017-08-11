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

		return new Vector3 (xMul * .53f, yMul * .53f, 0.0f);

		// make sure its in bounds and is not in the snake!
	}
}
