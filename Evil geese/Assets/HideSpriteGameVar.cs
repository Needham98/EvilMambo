using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hides the sprite on an object if a game variable equals any of a list of values
public class HideSpriteGameVar : MonoBehaviour {
	public string gameVariable;
	public List<string> Values;

	GameStateManager state;
	SpriteRenderer ownSpriteRenderer;

	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		ownSpriteRenderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool hide = false;
		foreach (string s in Values) {
			if (state.getGameVar (gameVariable) == s) {
				hide = true;
			}
		}

		ownSpriteRenderer.enabled = !hide;
	}
}
