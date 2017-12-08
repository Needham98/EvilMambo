using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	GameObject player;
	PlayerMovment movement;
	float movementThreshold = 0.8f; // magnitude of movment axis must be greater than this to move

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		movement = player.GetComponent<PlayerMovment> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Vertical") > movementThreshold) {
			movement.move (Vector2.up);
		}
		if (Input.GetAxis ("Vertical") < -movementThreshold) {
			movement.move (Vector2.down);
		}
		if (Input.GetAxis ("Horizontal") > movementThreshold) {
			movement.move (Vector2.right);
		}
		if (Input.GetAxis ("Horizontal") < -movementThreshold) {
			movement.move (Vector2.left);
		}
		if (Input.GetButtonDown ("Submit")) {
			movement.interact();
		}
	}
}
