using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public void resume(){
		state.movementEnabled = true;
		state.isPaused = false;
		Destroy (this.gameObject);
	}

	public void gotoPause(){
		pausePanel.SetActive (true);
		savePanel.SetActive (false);
		loadPanel.SetActive (false);
	}

	public void gotoSave(){
		pausePanel.SetActive (false);
		savePanel.SetActive (true);
		loadPanel.SetActive (false);
	}

	public void gotoLoad(){
		pausePanel.SetActive (false);
		savePanel.SetActive (false);
		loadPanel.SetActive (true);
	}

	public void save(string saveName){
		state.state.playerX = movement.x;
		state.state.playerY = movement.y;
		state.state.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		state.saveState (saveName);
	}

	public void load(string saveName){
		state.loadState (saveName);
	}

	public void quit(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MenuScene");
	}

	public static void pause(){
		Instantiate (Resources.Load ("PauseCanvas"));
	}
}
