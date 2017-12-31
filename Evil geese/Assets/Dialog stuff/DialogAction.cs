using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogAction {
	public enum actionType{
		setGameVar,
		startCombat,
		giveItem
	}
	public actionType ownActionType;
	public string gameVarName = "";
	public string gameVarValue = "";
	public List<CombatCharacterFactory.CombatCharacterPresets> combatEnemies;
	public InventoryItems.itemTypes itemType;
	public int itemAmount;

	public DialogAction (){
		combatEnemies = new List<CombatCharacterFactory.CombatCharacterPresets> ();
	}

	public void doAction(){
		GameStateManager state = GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
		switch (ownActionType) {
		case actionType.setGameVar:
			state.setGameVar (gameVarName, gameVarValue);
			break;
		case actionType.startCombat:
			CombatManager.startCombat (combatEnemies);
			break;
		case actionType.giveItem:
			state.changeItem (itemType, itemAmount);
			break;
		}
	}
}
