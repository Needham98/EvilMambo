using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour{
	public List<CombatCharacterFactory.CombatCharacterPresets> availibleCharacters { 
		get {return state.availibleCharacters;}
		set {state.availibleCharacters = value;}
	}
	public List<CombatCharacterFactory.CombatCharacterPresets> currentTeam { 
		get {return state.currentTeam;}
		set {state.currentTeam = value;}
	}
	public bool movementEnabled {
		get {return state.movementEnabled;}
		set {state.movementEnabled = value;}
	}
	public Dictionary<InventoryItems.itemTypes, int> inventory {
		get {return state.inventory;}
	}

	bool hasLoaded = false;
	public GameState state;

	// Use this for initialization
	void Start () {
		// if a GameStateManager already exists destroy this one;
		if (GameObject.FindGameObjectsWithTag ("GameStateManager").Length > 1) {
			Destroy (this.gameObject);
			return;
		}
		GameObject.DontDestroyOnLoad (this.gameObject);
		if (state == null) {
			Debug.Log ("the gamestate was null, creating new gamestate");
			state = new GameState ();
		}
		SceneManager.sceneLoaded += onSceneLoad;
	}

	void onSceneLoad(Scene scene, LoadSceneMode mode){
		if (hasLoaded) {
			hasLoaded = false;
			try{
				PlayerMovement movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
				movement.x = state.playerX;
				movement.y = state.playerY;
				movement.snapToGridPos();
			}catch (NullReferenceException e ){
				Debug.Log (e);
			}
		}
		movementEnabled = true;
	}

	public string getGameVar(string varName){
		try{	
			return state.gameStateVars[varName];
		} catch (KeyNotFoundException){
			return "";
		}
	}

	public void setGameVar(string varName, string varValue){
		if (state.gameStateVars.ContainsKey (varName)) {
			state.gameStateVars [varName] = varValue;
		} else {
			state.gameStateVars.Add (varName, varValue);
		}
	}

	public void changeItem(InventoryItems.itemTypes itemType, int amount){
		if (inventory.ContainsKey(itemType)){
			inventory[itemType] += amount;
		}else{
			inventory.Add(itemType, amount);
		}
	}

	public int getItem(InventoryItems.itemTypes itemType){
		if (inventory.ContainsKey(itemType)){
			return inventory [itemType];
		}else{
			return 0;
		}
	}

	public void saveState(string saveName){
		GameSave.saveState (saveName, state);
	}

	public void loadState(string saveName){
		state = GameSave.loadState (saveName);
		SceneManager.LoadScene (state.sceneName);
		hasLoaded = true;
	}

	public static GameStateManager getGameStateManager(){
		return GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
	}
		

}
