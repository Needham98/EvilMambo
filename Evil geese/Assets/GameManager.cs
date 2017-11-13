using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats{
	public string playerName = "";
	public int healthPoints = 100;
	public int strengthPoints = 1;
	public int defensePoints = 1;
	public string statModifierVar = "health"; //A name of a stat modifier for the char
}

public class GameManager : MonoBehaviour {
	//Inbetween level stat holders:
	public PlayerStats stats = new PlayerStats();
	public static GameManager instance = null; //Accessor for other scripts
	[HideInInspector] public bool playersTurn = true;

	// Firstly exec
	void Awake () {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;
		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);	

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}
	//What to do to begin game
	void InitGame(){

	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public static void CallbackInitialization(){
		//register the callback to be called everytime the scene is loaded
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//This is loaded each time a scene loads
	static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1){
		instance.InitGame ();
	}

	///<summary>Game over procedure</summary>
    ///<param name="finished">Whether game was finished.</param>
	public void GameOver(bool finished){
		
	}

}
