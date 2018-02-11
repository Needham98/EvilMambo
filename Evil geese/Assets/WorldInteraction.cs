using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// represents an interaction with the game world, used in gridPosition
public class WorldInteraction : MonoBehaviour {
    
	public enum InteractionTypes {CombatStart, DialogStart, SceneChange}
	public InteractionTypes ownInteractionType;
	public List<CombatCharacterFactory.CombatCharacterPresets> enemies;
	public string sceneName;
	public int playerX;
	public int playerY;


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	// interacts with this world interaction
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
		
		case InteractionTypes.SceneChange:
			GameStateManager state = GameStateManager.getGameStateManager ();
			state.state.playerX = playerX;
			state.state.playerY = playerY;
			SceneManager.LoadScene (sceneName);
			break;
                       
		}
	}

}
