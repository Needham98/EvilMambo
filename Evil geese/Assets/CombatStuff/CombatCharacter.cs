using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CombatCharacter {
	public int maxHealth;
	public int health;
	public CombatAbility basicAttack;
	public bool isControlable;
	public List<CombatAbility> abilities;
	public CombatEntity entity;// the CombatEntity attached to the representation of this character

	//todo define spriteset system for combat characters
	public CombatCharacter(int maxHealth, int health, bool isControlable = true, CombatAbility basicAttack = null){
		this.maxHealth = maxHealth;
		this.health = health;
		this.isControlable = isControlable;
		if (basicAttack == null){
			this.basicAttack = new MeleeAttack (8, 10);
		}
		else{
			this.basicAttack = basicAttack;
		}
		abilities = new List<CombatAbility> ();
	}

	public void AddAbility(CombatAbility ability){
		abilities.Add (ability);
	}

	// todo add system for damage calucation by damage type
	public void takeDamage(int damage, string type = ""){
		health -= damage;
	}

	public bool isAlive(){
		return health > 0;
	}
		
	public static int getFirstAlive (List<CombatCharacter> L){
		int i = 0;
		foreach (CombatCharacter c in L) {
			if (c.isAlive()) {
				return i;
			}
			i++;
		}
		return -1;
	}
}
