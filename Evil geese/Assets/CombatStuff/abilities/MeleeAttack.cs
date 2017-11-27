using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : CombatAbility {
	int maxDamage;
	int minDamage;
	string ownName;
	public string abilityName {
		get {return ownName;} 
		set {ownName = abilityName;}
		}
	public int minTargets { get { return 1; } }
	public int maxTargets { get { return 1; } }
	public bool isAssist { get { return false; } }

	public MeleeAttack (int minDamage, int maxDamage, string abilityName = "Stab"){
		if (maxDamage < minDamage) {
			throw new ArgumentException("maxDamage must not be less than minDamage");
		}
		this.maxDamage = maxDamage;
		this.minDamage = minDamage;
		this.abilityName = abilityName;
	}

	public void DoAbility (List<CombatCharacter> targets){
		if (targets.Count > maxTargets || targets.Count < minTargets) {
			throw new ArgumentException (string.Format ("invalid target count: {C0}, acceptable range: {C1}-{C2}", targets.Count, minTargets, maxTargets));
		}
		foreach (CombatCharacter c in targets) {
			int damage = UnityEngine.Random.Range (minDamage, maxDamage + 1);
			c.takeDamage (damage, "melee");
		} 
	}
}
