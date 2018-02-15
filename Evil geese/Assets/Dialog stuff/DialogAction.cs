using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogAction {
	public enum actionType{
		setGameVar,
		startCombat,
		giveItem,
		setCharacterAvailibility,
        //added enum by UnTwo
        startMiniGame,
		buy
	}
	public actionType ownActionType;
	public string gameVarName = "";
	public string gameVarValue = "";
	public List<CombatCharacterFactory.CombatCharacterPresets> combatEnemies;
	public InventoryItems.itemTypes itemType;
	public int itemAmount;

	public CombatCharacterFactory.CombatCharacterPresets character;
	public bool charAvailible;

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
		case actionType.buy:
			if (state.setCurrency (-InventoryItems.itemCost (itemType))) {
				state.changeItem (itemType, itemAmount);
				break;
			} else {
				break;
			}
			
		case actionType.setCharacterAvailibility:
			if (charAvailible) {
				if (!state.availibleCharacters.Contains (character)) {
					state.availibleCharacters.Add (character);
				}
			} else {
				if (state.availibleCharacters.Contains(character)){
					state.availibleCharacters.Remove(character);
				}
				if (state.currentTeam.Contains (character)) {
					state.currentTeam.Remove (character);
				}
			}
			break;
            // added case for new enum UnTwo
            case actionType.startMiniGame:
                SceneManager.LoadScene("MiniGameMenu");
                break;
        }
	}
}
