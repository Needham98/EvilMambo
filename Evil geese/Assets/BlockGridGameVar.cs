using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//blocks and a grid position based on a game variable
public class BlockGridGameVar : MonoBehaviour {
	GameObject gameController;
	Grid movementGrid;
	GameStateManager state;

	public int x;
	public int y;
	public string gameVar;
	public string value;
	public bool invert;
	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		movementGrid = gameController.GetComponent<Grid> ();
	}
	
	// Update is called once per frame
	void Update () {
		string currentValue = state.getGameVar (gameVar);
		GridPosition gPos = movementGrid.getPosition (x, y);
		if (gPos == null) {
			gPos = new GridPosition (x, y);
		}
		gPos.blocked = invert ^ (value == currentValue);
		movementGrid.clearPosition (x, y);
		movementGrid.setPosition (gPos);
	}
}
