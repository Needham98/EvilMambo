using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void NewGameBtn(string NewGameScene){
		GameStateManager state = GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
		state.state = new GameState ();
		state.availibleCharacters.Add (CombatCharacterFactory.CombatCharacterPresets.PlayerCharBasic);
		state.availibleCharacters.Add (CombatCharacterFactory.CombatCharacterPresets.PlayerCharBasic);
		state.currentTeam.Add (CombatCharacterFactory.CombatCharacterPresets.PlayerCharBasic);
		state.currentTeam.Add (CombatCharacterFactory.CombatCharacterPresets.PlayerCharBasic);
		SceneManager.LoadScene (NewGameScene);
	}

	public void ExitGameBtn(){
		Application.Quit ();
	}
}
