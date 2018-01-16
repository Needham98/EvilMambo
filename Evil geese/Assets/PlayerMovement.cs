using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	Transform ownTransform;
	public int x;// current x position on grid system
	public int y;// current y position on grid system
	bool moving = false;
	public GameObject stepObject; // for event triggers
	WorldInteraction stepObjectWorldInteraction;
	GameObject gameController; // GameController
	public Vector2 movedir = Vector2.up; // for animations
	Grid movementGrid;
	float moveSpeed = 3.5f; // speed of movement in tiles per second

	public List<Sprite> upAnimationLoop;
	public List<Sprite> downAnimationLoop;
	public List<Sprite> leftAnimationLoop;
	public List<Sprite> rightAnimationLoop;
	AnimationLoop anim;

	void Start () {
		ownTransform = this.transform;
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		movementGrid = (Grid) gameController.GetComponent<Grid> ();
		anim = this.gameObject.GetComponent<AnimationLoop> ();
	}


	void Update () {
		if (moving) {
			float distance = (ownTransform.position - new Vector3 (x, y, ownTransform.position.z)).magnitude;

			if (distance < 0.3 &&  stepObjectWorldInteraction != null) {
				stepObjectWorldInteraction.interact ();
			}

			float xDir = (float) x - ownTransform.position.x;
			float yDir = (float) y - ownTransform.position.y;
			if (distance < moveSpeed * Time.deltaTime * 1.1) { // 1.1 is a magic number to prevent bugs from floating point errors
				ownTransform.position = new Vector3 ((float) x, (float) y, ownTransform.position.z);
				moving = false;
			} else {
				ownTransform.position += new Vector3 (xDir, yDir).normalized * moveSpeed * Time.deltaTime;
			}
	}
	}

	public bool move (Vector2 dir){
		int newx = x + (int) dir.normalized.x;
		int newy = y + (int) dir.normalized.y;
		GridPosition gridPos = movementGrid.getPosition(newx, newy);
		if (moving) {
			if ((new Vector2(ownTransform.position.x, ownTransform.position.y) - new Vector2 (newx, newy)).magnitude > 1.02) {
				return false;
			}
		}
		if (gridPos != null) {
			if (gridPos.blocked) {
				movedir = dir.normalized;
				return false;
			}
			stepObject = gridPos.stepOn;
		} else {
			stepObject = null;
		}
		if (stepObject != null) {
			stepObjectWorldInteraction = stepObject.GetComponent<WorldInteraction> ();
		}

		x = newx;
		y = newy;
		moving = true;
		movedir = dir.normalized;

		if (movedir == Vector2.up) {
			anim.frames = upAnimationLoop;
		} else if (movedir == Vector2.down) {
			anim.frames = downAnimationLoop;
		} else if (movedir == Vector2.left) {
			anim.frames = leftAnimationLoop;
		} else if (movedir == Vector2.right) {
			anim.frames = rightAnimationLoop;
		}

		return true;
	}

	public void interact(){
		GridPosition pos = movementGrid.getPosition(x + (int) movedir.x, y + (int) movedir.y);
		if (pos == null){
			return;
		}
		GameObject interactionObject = pos.interactionObject;
		WorldInteraction interaction;
		try{
			interaction = interactionObject.GetComponent<WorldInteraction> ();
		}catch (NullReferenceException){
			return;
		}
		interaction.interact ();
	}

	public void snapToGridPos(){
		ownTransform = this.transform;
		ownTransform.position = new Vector3 ((float)x, (float)y, ownTransform.position.z);
	}
}
