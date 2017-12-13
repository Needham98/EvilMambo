using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
	public GameState state;

	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad (this.gameObject);
		if (state == null) {
			Debug.Log ("the gamestate was null, creating new gamestate");
			state = new GameState ();
		}
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

	public void saveState(string saveName){
		GameSave.saveState (saveName, state);
	}

	public void loadState(string saveName){
		state = GameSave.loadState (saveName);
	}
		

}
