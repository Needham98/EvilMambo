using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
	//todo visual representation of status effects

	//the lists representing all the combatants
	public List<CombatCharacter> frendlyChars;// the list of characters controlled by the player
	public List<CombatCharacter> enemyChars;//the list of characters attacking the player
	List<CombatCharacter> attackChars;//the list of characters attacking on the current turn (always points to either the frendlyChars or enemyChars list)
	List<CombatCharacter> defendChars;// the list of characters deffending on the current turn (always points to the opposite list to attackChars)


	//UI Variables
	GameObject canvasObj; // the canvas used in the UI
	GameObject selectorObj; // the GameObject of the selector icon (used to show which character you're controlling)
	SpriteRenderer selectorRen;// the sprite renderer on said object
	GameObject abilitiesPanel; // the panel used as the background and parent object for the ability selection menu
	List<GameObject> abilityButtonObjs;// the buttons used in the abilities menu
	List<GameObject> targetSelectorButtonObjs;


	//data representing the state of the current turn
	bool frendlyAttacking; // whose turn it is, true if frendlyChars, false if enemyChars
	public int attackerPos;// the position of the current attacker in the AttackChars list
	CombatCharacter attacker;// the current attacker
	List<CombatCharacter> attackTargets;// list containing the targets of the current attack
	CombatAbility attack; //the object representing the current attack
	int targetsRemaining;
	enum turnStages {selecting, targetSelection, moving, attacking, returning, win, lose} // the posible stages of a turn
	turnStages currentStage;


	//win data
	public delegate void winAction(); //function that gets called when the player wins

	//data needed to reset the scene after combat has finished
	List<GameObject> sceneObjects; //the list of objects that were active in the scene when combat began
	Vector3 startingCameraPosition;// the cameras position when combat began
	GameObject sceneCamera; // the camera in the scene

	void Start () {
		canvasObj = this.transform.parent.gameObject;
		abilitiesPanel = canvasObj.transform.Find ("AbilitiesPanel").gameObject;

		sceneObjects = new List<GameObject> ();
		foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
			if (obj.tag == "MainCamera") {
				canvasObj.GetComponent<Canvas> ().worldCamera = obj.GetComponent<Camera> ();
				startingCameraPosition = obj.transform.position;
				sceneCamera = obj;
				obj.transform.position = new Vector3 (0f, 0f, -10f);
			} else if (!obj.transform.IsChildOf(this.transform.parent)){
				sceneObjects.Add (obj);
				obj.SetActive (false);
			}
		}

		frendlyAttacking = true;
		attackerPos = 0;
		currentStage = turnStages.selecting;

		selectorObj = new GameObject ("Selector");
		selectorRen = selectorObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		selectorRen.sprite = Resources.Load<Sprite> ("Selector");
		selectorRen.enabled = false;

		GameObject combatEntityPrefab = Resources.Load<GameObject> ("CombatEntity");

		Vector3 pos = new Vector3 (-4, 3); // arbitrary start position
		Vector3 offset = new Vector3(0,-2); // arbitrary offset
		foreach (CombatCharacter c in frendlyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			c.entity = obj.GetComponent<CombatEntity> ();
			c.entity.setupBars (false, true);
			pos += offset;
		}
		pos = new Vector3 (4, 3); // arbitrary start position 
		foreach (CombatCharacter c in enemyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			c.entity = obj.GetComponent<CombatEntity> ();
			c.entity.setupBars (true, false);
			pos += offset;
		}
		attacker = frendlyChars [attackerPos];
	}
	
	// Update is called once per frame
	void Update () {
		if (frendlyAttacking) {
			attackChars = frendlyChars;
			defendChars = enemyChars;
		}
		else
		{
			attackChars = enemyChars;
			defendChars = frendlyChars;
		}
		if (attackerPos < 0 || attackerPos > attackChars.Count) {
			Debug.Log ("invalid attackerPos");
			return;
		}else{
			attacker = attackChars [attackerPos];
		}
		switch (currentStage) {
		case turnStages.selecting:
			selectionStage ();
			break;
		
		case turnStages.targetSelection:
			targetSelectionStage ();
			break;

		case turnStages.moving:
			movingStage ();
			break;

		case turnStages.attacking:
			attackingStage ();
			break;

		case turnStages.returning:
			returningStage ();
			break;
		
		case turnStages.win:
			winStage ();
			break;
		
		}
	}

	public void doAttack(){// function to be called when the "attack" button is hit
		if (currentStage == turnStages.selecting && frendlyAttacking) {
			attack = attacker.basicAttack;
			currentStage = turnStages.targetSelection;
			targetsRemaining = attack.maxTargets;
		}
	}

	public void doAbilities(){
		if (currentStage != turnStages.selecting) {
			return; // do nothing
		}

		//clear old buttons
		if (abilityButtonObjs != null) {
			foreach (GameObject button in abilityButtonObjs) {
				GameObject.Destroy (button);
			}
		}

		abilityButtonObjs = new List<GameObject> ();
		abilitiesPanel.SetActive (true);
		GameObject referenceButton = abilitiesPanel.transform.Find ("ReferenceButton").gameObject;
		Vector2 newButtonPosMin = ((RectTransform) referenceButton.transform).offsetMin;
		Vector2 newButtonPosMax = ((RectTransform) referenceButton.transform).offsetMax;
		float buttonSeparation = 45; // distance apart the tops of the buttons should be in pixels
		foreach (CombatAbility a in attacker.abilities) {
			GameObject newButtonObj = Instantiate (referenceButton, abilitiesPanel.transform);
			UnityEngine.UI.Button newButton = newButtonObj.GetComponent<UnityEngine.UI.Button> ();
			abilityButtonObjs.Add (newButtonObj);
			((RectTransform)newButtonObj.transform).offsetMin = newButtonPosMin;
			((RectTransform)newButtonObj.transform).offsetMax = newButtonPosMax;
			newButtonPosMin.y -= buttonSeparation;
			newButtonPosMax.y -= buttonSeparation;
			newButtonObj.SetActive (true);
			newButtonObj.transform.Find ("NameText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = a.abilityName;
			newButtonObj.transform.Find ("CostText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = a.energyCost.ToString();
			CombatAbility tempValue = a; // necessary to deal with weird scoping
			newButton.onClick.AddListener (delegate {selectAbility(tempValue);});

		}


	}

	public void hideAbilities(){
		abilitiesPanel.SetActive (false);
	}
		
	void selectAbility(CombatAbility ability){
		attack = ability;
		currentStage = turnStages.targetSelection;
		targetsRemaining = attack.maxTargets;
	}

	void selectTarget(CombatCharacter target){
		if (attackTargets == null) {
			attackTargets = new List<CombatCharacter> ();
		}
		attackTargets.Add (target);
		targetsRemaining -= 1;
	}
		
	void removeTargetSelectors(){
		if (targetSelectorButtonObjs == null) {
			return;
		}
		foreach (GameObject obj in targetSelectorButtonObjs) {
			Destroy (obj);
		}
		targetSelectorButtonObjs = null;
	}

	void lose(){//todo handle losing
		currentStage = turnStages.lose;
		Debug.Log("You Lose! Now implement losing.");
	}

	void win(){// todo handle winning
		currentStage = turnStages.win;
		Debug.Log ("You Win! Now implement winning.");
	}

	void selectionStage(){
		if (!frendlyAttacking) {
			//todo write better enemy combat logic
			int toAttack = CombatCharacter.getFirstAlive (defendChars);
			if (toAttack == -1) {
				lose ();
			} else {
				attackTargets = new List<CombatCharacter> ();
				attackTargets.Add (defendChars [toAttack]);
				attack = attacker.basicAttack;
				currentStage = turnStages.moving;
			}
		} else {
			selectorRen.enabled = true;
			selectorObj.transform.position = attacker.entity.transform.position;
		}
	}

	void targetSelectionStage(){
		selectorRen.enabled = false;
		hideAbilities ();
		if (targetsRemaining == 0) {
			removeTargetSelectors ();
			currentStage = turnStages.moving;
			return;
		}
		if (targetSelectorButtonObjs == null) {
			targetSelectorButtonObjs = new List<GameObject> ();
			List<CombatCharacter> selectableCharacters;
			if (attack.isAssist) {
				selectableCharacters = attackChars;
			} else {
				selectableCharacters = defendChars;
			}

			foreach (CombatCharacter character in selectableCharacters){
				if (character.isAlive()) {
					GameObject newButtonObj = Instantiate (canvasObj.transform.Find ("ReferenceTargetButton").gameObject, canvasObj.transform);
					newButtonObj.transform.position = character.entity.transform.position;
					newButtonObj.SetActive (true);
					targetSelectorButtonObjs.Add (newButtonObj);
					UnityEngine.UI.Button newButton = newButtonObj.GetComponent<UnityEngine.UI.Button> ();
					CombatCharacter tempValue = character; // necessary to deal with weird scoping
					newButton.onClick.AddListener (delegate {
						selectTarget (tempValue);
					});
				}
			}
		}
	}

	void movingStage(){
		hideAbilities ();
		selectorRen.enabled = false;
		if (attacker.entity.moveAttack ()) {
			currentStage = turnStages.attacking;
		}
	}

	void attackingStage(){// todo add animations and delays to attack stage
		attack.doAbility (attackTargets, attacker);
		attackTargets = null;
		currentStage = turnStages.returning;
	}

	void returningStage(){
		if (CombatCharacter.getFirstAlive (frendlyChars) == -1) {
			lose ();
		}
		if (CombatCharacter.getFirstAlive (enemyChars) == -1) {
			win ();
		}

		if (attacker.entity.moveRest()){
			do {
				attackerPos++;
				if (attackerPos >= attackChars.Count){
					break;
				}
				attacker = attackChars[attackerPos];
			} while (!attacker.isAlive());

			if (attackerPos >= attackChars.Count) {
				frendlyAttacking = !frendlyAttacking;
				foreach (CombatCharacter character in defendChars) {
					character.doEffects ();
				}
				attackerPos = CombatCharacter.getFirstAlive (defendChars);
			}
			if (attackerPos == -1) {
				Debug.Log ("somethings wrong, there's no one left to take a turn");
			}
			currentStage = turnStages.selecting;
		}
	}

	void winStage(){
		if (abilitiesPanel != null) {
			Destroy (abilitiesPanel);
			Destroy (selectorObj);
			foreach (CombatCharacter character in frendlyChars) {
				Destroy (character.entity.transform.gameObject);
			}
			foreach (CombatCharacter character in enemyChars) {
				Destroy (character.entity.transform.gameObject);
			}
			foreach (GameObject obj in sceneObjects) {
				obj.SetActive (true);
			}
			sceneCamera.transform.position = startingCameraPosition;
			Destroy (this.transform.parent.gameObject);
		}
	}
}
