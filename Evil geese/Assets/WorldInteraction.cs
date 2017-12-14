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
			CombatManager.startCombat (enemies);
			break;

		case InteractionTypes.DialogStart:
			DialogManager dialogManager = this.GetComponentInParent<DialogManager> ();
			if (dialogManager == null) {
				Debug.Log ("Error: no DialogManager component found");
				return;
			}
			dialogManager.beginDialog ();
			break;
		}
	}

}
