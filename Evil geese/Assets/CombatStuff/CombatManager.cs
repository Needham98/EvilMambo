using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
	//the lists representing all the combatants
	public List<CombatCharacter> frendlyChars;// the list of characters controlled by the player
	public List<CombatCharacter> enemyChars;//the list of characters attacking the player
	List<CombatCharacter> attackChars;//the list of characters attacking on the current turn (always points to either the frendlyChars or enemyChars list)
	List<CombatCharacter> defendChars;// the list of characters deffending on the current turn (always points to the opposite list to attackChars)


	//the selector icon
	GameObject selectorObj; // the GameObject of the selector icon
	SpriteRenderer selectorRen;// the sprite renderer on said object


	//data representing the state of the current turn
	bool frendlyAttacking; // whose turn it is, true if frendlyChars, false if enemyChars
	public int attackerPos;// the position of the current attacker in the AttackChars list
	CombatCharacter attacker;// the current attacker
	List<CombatCharacter> attackTargets;// list containing the targets of the current attack
	CombatAbility attack; //the object representing the current attack
	enum turnStages {selecting, moving, attacking, returning} // the stages of a turn
	turnStages currentStage;
		

	void Start () {
		frendlyAttacking = true;
		attackerPos = 0;
		currentStage = turnStages.selecting;

		selectorObj = new GameObject ("Selector");
		selectorRen = selectorObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		selectorRen.sprite = Resources.Load<Sprite> ("Selector");
		selectorRen.enabled = false;

		GameObject combatEntityPrefab = Resources.Load<GameObject> ("CombatEntity");

		if (frendlyChars == null || frendlyChars.Count == 0) {
			frendlyChars = new List<CombatCharacter> ();
			frendlyChars.Add (new CombatCharacter (10, 10));// for testing
			frendlyChars.Add (new CombatCharacter (10, 10));// for testing
		}
		if (enemyChars == null || enemyChars.Count == 0) {
			enemyChars = new List<CombatCharacter> ();
			enemyChars.Add (new CombatCharacter (10, 10));// for testing
			enemyChars.Add (new CombatCharacter (10, 10));// for testing
		}

		Vector3 pos = new Vector3 (-4, 3); // arbitrary start position
		Vector3 offset = new Vector3(0,-2); // arbitrary offset
		foreach (CombatCharacter c in frendlyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			c.entity = obj.GetComponent<CombatEntity> ();
			pos += offset;
		}
		pos = new Vector3 (4, 3); // arbitrary start position 
		foreach (CombatCharacter c in enemyChars) {
			GameObject obj = GameObject.Instantiate(combatEntityPrefab, pos, new Quaternion());
			pos += offset;
			c.entity = obj.GetComponent<CombatEntity> ();
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

		case turnStages.moving:
			movingStage ();
			break;

		case turnStages.attacking:
			attackingStage ();
			break;

		case turnStages.returning:
			returningStage ();
			break;
		}
	}

	public void doAttack(){// function to be called when the "attack" button is hit
		if (currentStage == turnStages.selecting && frendlyAttacking) {
			//todo implement system for targeting enemy characters
			int toAttack = CombatCharacter.getFirstAlive(defendChars);
			attackTargets = new List<CombatCharacter>();
			attackTargets.Add(defendChars[toAttack]);

			attack = attacker.basicAttack;
			currentStage = turnStages.moving;
		}
	}

	void lose(){//todo handle losing
		Debug.Log("You Lose! Now implement losing.");
	}

	void win(){// todo handle winning
		Debug.Log ("You Win! Now implement winning.");
	}

	void selectionStage(){
		if (!frendlyAttacking) {
			//todo write better enemy combat logic
			int toAttack = CombatCharacter.getFirstAlive(defendChars);
			if (toAttack == -1) {
				lose ();
			} else {
				attackTargets = new List<CombatCharacter>();
				attackTargets.Add(defendChars[toAttack]);
				attack = attacker.basicAttack;
				currentStage = turnStages.moving;
			}
		}
	}

	void movingStage(){
		if (attacker.entity.moveAttack ()) {
			currentStage = turnStages.attacking;
		}
	}

	void attackingStage(){// todo add animations and delays to attack stage
		attack.DoAbility (attackTargets);
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
				attackerPos = CombatCharacter.getFirstAlive (defendChars);
			}
			if (attackerPos == -1) {
				Debug.Log ("somethings wrong, there's no one left to take a turn");
			}
			currentStage = turnStages.selecting;
		}
	}
		
}
