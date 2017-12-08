using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteraction : MonoBehaviour {
	public enum InteractionTypes {CombatStart, DialogStart}
	public InteractionTypes ownInteractionType;
	public List<CombatCharacterFactory.CombatCharacterPresets> enemies;


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public void interact(){
		switch (ownInteractionType) {
		case InteractionTypes.CombatStart:
			GameObject combatObject = (GameObject) Instantiate (Resources.Load ("CombatCanvas"));
			CombatManager combatMan = combatObject.GetComponentInChildren<CombatManager> ();
			List<CombatCharacter> enemyCharList = new List<CombatCharacter> ();
			foreach (CombatCharacterFactory.CombatCharacterPresets charType in enemies) {
				enemyCharList.Add (CombatCharacterFactory.MakeCharacter (charType));
			}
			combatMan.enemyChars = enemyCharList;

			GameStateManager gameState = GameObject.FindGameObjectWithTag ("GameStateManager").GetComponent<GameStateManager> ();
			List<CombatCharacter> frendlyCharList = new List<CombatCharacter> ();
			foreach (CombatCharacterFactory.CombatCharacterPresets charType in gameState.currentTeam) {
				frendlyCharList.Add (CombatCharacterFactory.MakeCharacter (charType));
			}
			combatMan.frendlyChars = frendlyCharList;
			break;

		case InteractionTypes.DialogStart:
			//todo implement dialog system
			Debug.Log ("NOT IMPLEMENTED");
			break;
		}
	}

}
