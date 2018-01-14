using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapButton : MonoBehaviour {
	GameStateManager state;
	public string varibleName;
	public bool defaultState;
	public string sceneName;
	public int playerX;
	public int playerY;

	// Use this for initialization
	void Start () {
		Awake ();
	}

	void Awake(){
		state = GameStateManager.getGameStateManager ();
		bool enabled;
		if (state.getGameVar (varibleName) == "") {
			enabled = defaultState;
		} else if (state.getGameVar (varibleName) == "true") {
			enabled = true;
		} else {
			enabled = false;
		}

		UnityEngine.UI.Button button = this.GetComponent<UnityEngine.UI.Button> ();
		button.interactable = enabled;
		button.onClick.AddListener (delegate {
			enterScene (sceneName, playerX, playerY);
		});
	}

	public void enterScene(string newScene, int playerX, int playerY){
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state.playerX = playerX;
		state.state.playerY = playerY;
		SceneManager.LoadScene (newScene);
	}
}
