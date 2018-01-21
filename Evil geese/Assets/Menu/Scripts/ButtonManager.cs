using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	//starts a new game, called when the new game button on the menu is pressed
	public void NewGameBtn(string newGameScene){
		GameStateManager state = GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
		state.state = new GameState ();
		state.availibleCharacters.Add (CombatCharacterFactory.CombatCharacterPresets.BobbyBard);
		state.currentTeam.Add (CombatCharacterFactory.CombatCharacterPresets.BobbyBard);
		state.availibleCharacters.Add (CombatCharacterFactory.CombatCharacterPresets.CharlieCleric);
		state.currentTeam.Add (CombatCharacterFactory.CombatCharacterPresets.CharlieCleric);
		state.availibleCharacters.Add (CombatCharacterFactory.CombatCharacterPresets.MabelMage);
		state.currentTeam.Add (CombatCharacterFactory.CombatCharacterPresets.MabelMage);
		SceneManager.LoadScene (newGameScene);
	}

	// quits the game, called when the exit game button is pressed
	public void ExitGameBtn(){
		Application.Quit ();
	}
}
