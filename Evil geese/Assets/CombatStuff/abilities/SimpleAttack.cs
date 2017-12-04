using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SimpleAttack : CombatAbility {
	int maxDamage;
	int minDamage;
	string ownName;
	string damageType;
	public string abilityName {
		get {return ownName;} 
		}
	int ownCost;
	public int minTargets { get { return 1; } }
	public int maxTargets { get { return 1; } }
	public int energyCost { get { return ownCost; } }
	public bool isAssist { get { return false; } }

	public SimpleAttack (int minDamage, int maxDamage,string damageType, int energyCost = 0, string abilityName = "Stab"){
		if (maxDamage < minDamage) {
			throw new ArgumentException("maxDamage must not be less than minDamage");
		}
		this.maxDamage = maxDamage;
		this.minDamage = minDamage;
		this.ownName = abilityName;
		this.ownCost = energyCost;
		this.damageType = damageType;
	}

	public void doAbility (List<CombatCharacter> targets, CombatCharacter user){
		if (user.energy < energyCost) {
			throw new ArgumentException ("this ability can't be used because the user doesn't have enough energy");
		}
		if (targets.Count > maxTargets || targets.Count < minTargets) {
			throw new ArgumentException (string.Format ("invalid target count: {C0}, acceptable range: {C1}-{C2}", targets.Count, minTargets, maxTargets));
		}
		user.energy -= energyCost;
		user.updateEntityBars ();
		foreach (CombatCharacter c in targets) {
			int damage = UnityEngine.Random.Range (minDamage, maxDamage + 1);
			c.takeDamage ((int) (damage * user.damageDealtModifier(damageType)), damageType);
		} 
	}

	public void OnBeforeSerialize(){
	}

	public void OnAfterDeserialize(){
	}

}
