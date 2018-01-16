using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	GameObject player;
	PlayerMovement movement;
	float movementThreshold = 0.8f; // magnitude of movment axis must be greater than this to move
	GameStateManager state;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		movement = player.GetComponent<PlayerMovement> ();
		state = GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (!state.movementEnabled) {
			return;
		}
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
		if (Input.GetButtonDown("Cancel")){
			PauseMenuManager.pause ();
		}
		if (Input.GetButtonDown("RosterOpen")){
			CharacterMenu.open();
		}
	}
}
