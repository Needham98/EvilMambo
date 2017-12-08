using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour {
	Transform ownTransform;
	public int x;// current x position on grid system
	public int y;// current y position on grid system
	bool moving = false;
	public GameObject stepObject; // for event triggers
	GameObject MCP; // GameController
	public Vector2 movedir = Vector2.up; // for animations
	Grid movementGrid;
	float moveSpeed = 3.5f; // speed of movement in tiles per second

	void Start () {
		ownTransform = GetComponent<Transform> ();
		MCP = GameObject.FindGameObjectWithTag ("GameController");
		movementGrid = (Grid) MCP.GetComponent<Grid> ();
	}


	void Update () {
		if (moving) {
			float distance = (ownTransform.position - new Vector3 (x, y, ownTransform.position.z)).magnitude;
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
		x = newx;
		y = newy;
		moving = true;
		movedir = dir.normalized;
		return true;
	}

	public void interact(){
		GridPosition pos = movementGrid.getPosition(x + (int) movedir.x, y + (int) movedir.y);
		if (pos == null){
			return;
		}
		GameObject interactionObject = pos.interactionObject;
		WorldInteraction interaction = interactionObject.GetComponent<WorldInteraction> ();
		if (interaction == null) {
			return;
		}
		interaction.interact ();
	}
}
