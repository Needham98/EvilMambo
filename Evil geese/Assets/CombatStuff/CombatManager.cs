using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//handles combat sequences
public class CombatManager : MonoBehaviour {
	//TODO visual representation of status effects (CombatEffects)

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
	GameObject WinPanel;// the pannel displayed after winning
	GameObject LosePanel;// the panel displayed after losing

	//data representing the state of the current turn
	bool frendlyAttacking; // whose turn it is, true if frendlyChars, false if enemyChars
	public int attackerPos;// the position of the current attacker in the AttackChars list
	CombatCharacter attacker;// the current attacker
	List<CombatCharacter> attackTargets;// list containing the targets of the current attack
	CombatAbility attack; //the object representing the current attack
	int targetsRemaining; // how many more targets need selecting for the current attack
	enum turnStages {selecting, targetSelection, moving, attacking, returning, win, lose} // the posible stages of a turn
	turnStages currentStage; // which of the above stages this combat encounter is currently in

	//data needed to reset the scene after combat has finished
	List<GameObject> sceneObjects; //the list of objects that were deactivated in the scene when combat began
	Vector3 startingCameraPosition;// the cameras position when combat began
	GameObject sceneCamera; // the camera in the scene

	//gameState data
	GameStateManager state;

	float timer = 0f; //timer used to time animations

	void Start () {
		canvasObj = this.transform.parent.gameObject;
		WinPanel = this.transform.parent.Find("WinPanel").gameObject;
		LosePanel = this.transform.parent.Find ("LosePanel").gameObject;
		abilitiesPanel = canvasObj.transform.Find ("AbilitiesPanel").gameObject;
		state = GameStateManager.getGameStateManager ();

		//hide non combat objects in the scene
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
		List<GameObject> gameObjectsToLeaveActive = new List<GameObject> ();
		foreach (GameObject obj in sceneObjects) {
			if (obj.transform.IsChildOf (sceneCamera.transform)) {
				gameObjectsToLeaveActive.Add (obj);
			}
			if (obj.tag == "GameStateManager") {
				gameObjectsToLeaveActive.Add (obj);
			}
		}
		foreach (GameObject obj in gameObjectsToLeaveActive) {
			sceneObjects.Remove(obj);
			obj.SetActive (true);
		}


		frendlyAttacking = true;
		attackerPos = 0;
		currentStage = turnStages.selecting;

		selectorObj = new GameObject ("Selector");
		selectorRen = selectorObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		selectorRen.sprite = Resources.Load<Sprite> ("Selector");
		selectorRen.enabled = false;

		GameObject combatEntityPrefab = Resources.Load<GameObject> ("CombatEntity");

		// create objects representing combatants
		Vector3 pos = new Vector3 (-4, 3); // arbitrary start position
		Vector3 offset = new Vector3(0,-2); // arbitrary offset
		foreach (CombatCharacter c in frendlyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			c.entity = obj.GetComponent<CombatEntity> ();
			c.entity.setupBars (false, true);
			c.updateEntityBars ();
			c.updateEntityAnimation ("base");
			pos += offset;
		}
		pos = new Vector3 (4, 3); // arbitrary start position 
		foreach (CombatCharacter c in enemyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			c.entity = obj.GetComponent<CombatEntity> ();
			c.entity.setupBars (true, false);
			c.updateEntityBars ();
			c.updateEntityAnimation ("base");
			pos += offset;
		}
		attacker = frendlyChars [attackerPos];
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

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
		
		case turnStages.lose:
			loseStage ();
			break;
		}
	}

	// function to be called when the "attack" button is hit
	public void doAttack(){
		if (currentStage == turnStages.selecting && frendlyAttacking) {
			attack = attacker.basicAttack;
			currentStage = turnStages.targetSelection;
			targetsRemaining = attack.maxTargets;
		}
	}

	// function called when the abilities button is pressed
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

	// function called when the items button is pressed
	public void doItems(){
		//uses the same panel as abilities
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

		List<InventoryItems.itemTypes> items = new List<InventoryItems.itemTypes> ();
		foreach (InventoryItems.itemTypes itemType in state.state.inventory.Keys) {
			if (state.state.inventory [itemType] > 0 && InventoryItems.itemHasAbility(itemType)) {
				items.Add (itemType);
			}
		}

		foreach (InventoryItems.itemTypes itemType in items) {
			GameObject newButtonObj = Instantiate (referenceButton, abilitiesPanel.transform);
			UnityEngine.UI.Button newButton = newButtonObj.GetComponent<UnityEngine.UI.Button> ();
			abilityButtonObjs.Add (newButtonObj);
			((RectTransform)newButtonObj.transform).offsetMin = newButtonPosMin;
			((RectTransform)newButtonObj.transform).offsetMax = newButtonPosMax;
			newButtonPosMin.y -= buttonSeparation;
			newButtonPosMax.y -= buttonSeparation;
			newButtonObj.SetActive (true);
			newButtonObj.transform.Find ("NameText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = InventoryItems.itemDisplayName(itemType);
			//TODO refactor name "CostText" to "SecondText" or other similar since it describes cost on abilities screen and count on items screen
			newButtonObj.transform.Find ("CostText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = state.state.inventory[itemType].ToString();
			InventoryItems.itemTypes tempValue = itemType;
			newButton.onClick.AddListener (delegate {selectItem(tempValue);});

		}


	}

	//hides the ablilities panel
	public void hideAbilities(){
		abilitiesPanel.SetActive (false);
	}

	// function called when the continue button on the victory screen is pressed
	public void doWin(){
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
				if (obj != null) {
					obj.SetActive (true);
				}
			}
			state.
			sceneCamera.transform.position = startingCameraPosition;
			Destroy (this.transform.parent.gameObject);
		}
	}

	// function called when the continue button on the defeat screen is pressed
	public void doLose(){
		SceneManager.LoadScene ("Scenes/MenuScene");
	}

	// function called when an item button on the items panel is pressed
	void selectItem(InventoryItems.itemTypes itemType){
		selectAbility (InventoryItems.itemAbility (itemType));
		if (InventoryItems.itemConsumedOnUse(itemType)) {
			state.changeItem (itemType, -1);
		}
	}

	// function called when an abililty button on abilities panel is pressed
	void selectAbility(CombatAbility ability){
		attack = ability;
		currentStage = turnStages.targetSelection;
		targetsRemaining = attack.maxTargets;
	}

	// function called when a target is clicked during the target selection phase
	void selectTarget(CombatCharacter target){
		if (attackTargets == null) {
			attackTargets = new List<CombatCharacter> ();
		}
		attackTargets.Add (target);
		targetsRemaining -= 1;
	}

	// gets rid of the target selectors from the target selection phase
	void removeTargetSelectors(){
		if (targetSelectorButtonObjs == null) {
			return;
		}
		foreach (GameObject obj in targetSelectorButtonObjs) {
			Destroy (obj);
		}
		targetSelectorButtonObjs = null;
	}

	//called when a losing state is detected
	void lose(){
		currentStage = turnStages.lose;
		Debug.Log("You Lose! Now implement losing.");
	}

	// called when a winning state is detected
	void win(){
		currentStage = turnStages.win;
	}

	//called in update() if the current stage is the selection stage
	void selectionStage(){
		if (!frendlyAttacking) {
			//TODO write better enemy combat logic
			int toAttack = CombatCharacter.getFirstAlive (defendChars);
			if (toAttack == -1) {
				lose ();
			} else {
				attackTargets = new List<CombatCharacter> ();
				attackTargets.Add (defendChars [toAttack]);
				attack = attacker.basicAttack;
				attacker.updateEntityAnimation ("move");
				currentStage = turnStages.moving;
			}
		} else {
			selectorRen.enabled = true;
			selectorObj.transform.position = attacker.entity.transform.position;
		}
	}

	//called in update() if the current stage is the target selection stage
	void targetSelectionStage(){
		selectorRen.enabled = false;
		hideAbilities ();
		if (attackTargets == null) {
			attackTargets = new List<CombatCharacter> ();
		}
		if (targetsRemaining == 0) {
			removeTargetSelectors ();
			currentStage = turnStages.moving;
			attacker.updateEntityAnimation ("move");
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

	//called in update() if the current stage is the movement stage
	void movingStage(){
		hideAbilities ();
		selectorRen.enabled = false;
		if (attacker.entity.moveAttack ()) {
			attacker.updateEntityAnimation ("attack");
			timer = 0f;
			currentStage = turnStages.attacking;
		}
	}

	//called in update() if the current stage is the attacking stage
	void attackingStage(){// TODO add animations and delays to attack stage
		if (timer > 1f) {
			attack.doAbility (attackTargets, attacker);
			attackTargets = null;
			currentStage = turnStages.returning;
			attacker.updateEntityAnimation ("return");
		}
	}

	//called in update() if the current stage is the returning stage
	void returningStage(){
		if (CombatCharacter.getFirstAlive (frendlyChars) == -1) {
			lose ();
		}
		if (CombatCharacter.getFirstAlive (enemyChars) == -1) {
			win ();
		}

		if (attacker.entity.moveRest()){
			attacker.updateEntityAnimation ("base");
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

	//called in update() if the current stage is the lose stage
	void loseStage(){
		LosePanel.SetActive (true);
	}

	//called in update() if the current stage is the win stage
	void winStage(){
		WinPanel.SetActive (true);
	}

	// used to activate combat
	public static void startCombat(List<CombatCharacterFactory.CombatCharacterPresets> enemies){
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
	}
}
