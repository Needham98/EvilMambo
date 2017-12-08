using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
	public List<CombatCharacterFactory.CombatCharacterPresets> availibleCharacters;
	public List<CombatCharacterFactory.CombatCharacterPresets> currentTeam;

	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad (this.gameObject);
	}

	public void saveState(string saveName)
	{	
		//todo save system
		Debug.Log ("saving not implemented");
	}

}
