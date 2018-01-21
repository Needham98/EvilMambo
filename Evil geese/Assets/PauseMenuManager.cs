using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// handles the pasue menu
public class PauseMenuManager : MonoBehaviour {
	GameStateManager state;
	PlayerMovement movement;
	public GameObject pausePanel;
	public GameObject savePanel;
	public GameObject loadPanel;

	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		state.movementEnabled = false;
		state.isPaused = true;
		movement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
	}

	// resumes the game
	public void resume(){
		state.movementEnabled = true;
		state.isPaused = false;
		Destroy (this.gameObject);
	}

	// sets the menu to the pause default pause menu state
	public void gotoPause(){
		pausePanel.SetActive (true);
		savePanel.SetActive (false);
		loadPanel.SetActive (false);
	}

	// sets the menu to the save game state
	public void gotoSave(){
		pausePanel.SetActive (false);
		savePanel.SetActive (true);
		loadPanel.SetActive (false);
	}

	// sets the menu to the load game state
	public void gotoLoad(){
		pausePanel.SetActive (false);
		savePanel.SetActive (false);
		loadPanel.SetActive (true);
	}

	// saves a copy of the current gameState in "<savename>.geese"
	public void save(string saveName){
		state.state.playerX = movement.x;
		state.state.playerY = movement.y;
		state.state.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		state.saveState (saveName);
	}

	// loads the gameState stored in "<savename>.geese"
	public void load(string saveName){
		state.loadState (saveName);
	}

	// quits to menu
	public void quit(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MenuScene");
	}

	// opens the pause menu
	public static void pause(){
		Instantiate (Resources.Load ("PauseCanvas"));
	}
}
