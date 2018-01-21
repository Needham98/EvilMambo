using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// handles the character selection menu
public class CharacterMenu : MonoBehaviour {
	GameObject characterButtonPattern;
	GameObject abilityTextPattern;
	GameObject costTextPattern;
	GameObject characterNameText;
	GameObject healthText;
	GameObject energyText;

	CombatCharacterFactory.CombatCharacterPresets currentCharacter;
	GameStateManager state;

	List<GameObject> buttonObjects;
	List<GameObject> abilityTextObjects;
	List<GameObject> costTextObjects;


	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		characterButtonPattern = this.transform.Find ("CharacterSelectButton").gameObject;
		abilityTextPattern = this.transform.Find ("AbilityText").gameObject;
		costTextPattern = this.transform.Find ("CostText").gameObject;
		characterNameText = this.transform.Find ("CharacterNameText").gameObject;
		healthText = this.transform.Find ("HealthText").gameObject;
		energyText = this.transform.Find ("EnergyText").gameObject;

		buttonObjects = new List<GameObject> ();
		selectCharacter(state.availibleCharacters[0]);

		float buttonOffset = 0.0f;
		foreach (CombatCharacterFactory.CombatCharacterPresets character in state.availibleCharacters) {
			GameObject button = Instantiate (characterButtonPattern, this.transform);
			button.transform.Find ("Text").gameObject.GetComponent<Text> ().text = CombatCharacterFactory.GetCharacterName (character);
			button.SetActive (true);
			buttonObjects.Add (button);
			((RectTransform)(button.transform)).anchorMin -= new Vector2 (0f, buttonOffset);
			((RectTransform)(button.transform)).anchorMax -= new Vector2 (0f, buttonOffset);
			CombatCharacterFactory.CombatCharacterPresets tempValue = character;
			button.GetComponent<Button> ().onClick.AddListener (delegate {
				selectCharacter (tempValue);
				});
			buttonOffset += 0.08f;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void selectCharacter(CombatCharacterFactory.CombatCharacterPresets character){
		currentCharacter = character;
		characterNameText.GetComponent<Text> ().text = CombatCharacterFactory.GetCharacterName (character);
		healthText.GetComponent<Text> ().text = "Health: " + CombatCharacterFactory.GetCharacterMaxhealth (character).ToString ();
		energyText.GetComponent<Text> ().text = "Energy: " + CombatCharacterFactory.GetCharacterMaxEnergy (character).ToString ();

		if (abilityTextObjects != null) {
			foreach (GameObject o in abilityTextObjects) {
				Destroy (o);
			}
		}
		if (costTextObjects != null) {
			foreach (GameObject o in costTextObjects) {
				Destroy (o);
			}
		}

		abilityTextObjects = new List<GameObject> ();
		costTextObjects = new List<GameObject> ();

		float abilityOffset = 0f;
		foreach (CombatAbility ability in CombatCharacterFactory.GetCharacterAbilities(character)) {
			GameObject abilityText = Instantiate (abilityTextPattern, this.transform);
			GameObject costText = Instantiate (costTextPattern, this.transform);
			abilityTextObjects.Add (abilityText);
			costTextObjects.Add (costText);

			abilityText.SetActive (true);
			costText.SetActive (true);
			abilityText.GetComponent<Text> ().text = ability.abilityName;
			costText.GetComponent<Text> ().text = ability.energyCost.ToString() + " energy";

			((RectTransform)abilityText.transform).anchorMin -= new Vector2 (0, abilityOffset);
			((RectTransform)abilityText.transform).anchorMax -= new Vector2 (0, abilityOffset);
			((RectTransform)costText.transform).anchorMin -= new Vector2 (0, abilityOffset);
			((RectTransform)costText.transform).anchorMax -= new Vector2 (0, abilityOffset);
			abilityOffset += 0.05f;
		}

	}

	public void setCurrentCharacterPositionInTeam(int pos){
		if (pos >= state.currentTeam.Count) {
			if (!state.currentTeam.Contains (currentCharacter)) {
				state.currentTeam.Add (currentCharacter);
			}
			return;
		}

		if (state.currentTeam.Contains (currentCharacter)) {
			if (state.currentTeam.IndexOf (currentCharacter) == pos) {
				return;
			}

			CombatCharacterFactory.CombatCharacterPresets swapValue = state.currentTeam [pos];
			int otherPos = state.currentTeam.IndexOf (currentCharacter);
			state.currentTeam [otherPos] = swapValue;
			state.currentTeam [pos] = currentCharacter;

		} else {
			state.currentTeam [pos] = currentCharacter;
		}
	}

	public static void open(){
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.movementEnabled = false;
		Instantiate (Resources.Load ("CharRosterCanvas"));
	}

	public void close (){
		Destroy (this.transform.gameObject);
		state.movementEnabled = true;
	}
}
