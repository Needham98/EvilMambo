using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogAction {
	public enum actionType{
		setGameVar,
		startCombat
	}
	public actionType ownActionType;
	public string gameVarName = "";
	public string gameVarValue = "";
	public List<CombatCharacterFactory.CombatCharacterPresets> combatEnemies;

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
		}
	}
}
