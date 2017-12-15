using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameState : ISerializationCallbackReceiver{
	public List<CombatCharacterFactory.CombatCharacterPresets> availibleCharacters;
	public List<CombatCharacterFactory.CombatCharacterPresets> currentTeam;
	public Dictionary<string, string> gameStateVars;
	[HideInInspector]
	[SerializeField]
	List<string> gameVarNames;
	[HideInInspector]
	[SerializeField]
	List<string> gameVarValues;
	public bool movementEnabled;
	public string sceneName;
	public int playerX;
	public int playerY;

	public GameState(){
		gameStateVars = new Dictionary<string, string> ();
		movementEnabled = true;
		availibleCharacters = new List<CombatCharacterFactory.CombatCharacterPresets> ();
		currentTeam = new List<CombatCharacterFactory.CombatCharacterPresets> ();
	}

	public void OnBeforeSerialize(){
		gameVarNames = new List<string> ();
		gameVarValues = new List<string> ();
		foreach (string name in gameStateVars.Keys) {
			gameVarNames.Add (name);
			gameVarValues.Add (gameStateVars [name]);
		}
	}

	public void OnAfterDeserialize(){
		gameStateVars = new Dictionary<string, string>();
		for (int i = 0; i < gameVarNames.Count; i++){
			gameStateVars.Add (gameVarNames [i], gameVarValues [i]);
		}
	}
}
