using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
//stores the majority of data relating to the current game state
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

	public Dictionary<InventoryItems.itemTypes, int> inventory;
	[HideInInspector]
	[SerializeField]
	List<InventoryItems.itemTypes> inventoryTypes;
	[HideInInspector]
	[SerializeField]
	List<int> inventoryValues;

	public bool movementEnabled;
	public bool isPaused;
	public string sceneName;
	public int playerX;
	public int playerY;

	public int currency; //new variable added to enable a shop

	public GameState(){
		gameStateVars = new Dictionary<string, string> ();
		movementEnabled = true;
		isPaused = false;
		availibleCharacters = new List<CombatCharacterFactory.CombatCharacterPresets> ();
		currentTeam = new List<CombatCharacterFactory.CombatCharacterPresets> ();
		inventory = new Dictionary<InventoryItems.itemTypes, int> ();
		currency = 10;
	}

	public void OnBeforeSerialize(){
		gameVarNames = new List<string> ();
		gameVarValues = new List<string> ();
		foreach (string name in gameStateVars.Keys) {
			gameVarNames.Add (name);
			gameVarValues.Add (gameStateVars [name]);
		}

		inventoryTypes = new List<InventoryItems.itemTypes> ();
		inventoryValues = new List<int> ();
		foreach (InventoryItems.itemTypes itemType in inventory.Keys) {
			inventoryTypes.Add (itemType);
			inventoryValues.Add (inventory [itemType]);
		}
	}

	public void OnAfterDeserialize(){
		gameStateVars = new Dictionary<string, string>();
		for (int i = 0; i < gameVarNames.Count; i++){
			gameStateVars.Add (gameVarNames [i], gameVarValues [i]);
		}
		inventory = new Dictionary<InventoryItems.itemTypes, int> ();
		for (int i = 0; i < inventoryTypes.Count; i++) {
			inventory.Add (inventoryTypes [i], inventoryValues [i]);
		}

	}
}
