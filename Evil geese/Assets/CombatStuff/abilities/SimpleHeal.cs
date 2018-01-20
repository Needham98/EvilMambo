using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHeal : CombatAbility {
	int maxHeal;
	int minHeal;
	string ownName;
	public string abilityName {
		get {return ownName;} 
	}
	int ownCost;
	public int minTargets { get { return 1; } }
	public int maxTargets { get { return 1; } }
	public int energyCost { get { return ownCost; } }
	public bool isAssist { get { return true; } }

	public SimpleHeal (int minHeal, int maxHeal, int energyCost = 0, string abilityName = "Stab"){
		if (maxHeal < minHeal) {
			throw new ArgumentException("maxHeal must not be less than minHeal");
		}
		this.maxHeal = maxHeal;
		this.minHeal = minHeal;
		this.ownName = abilityName;
		this.ownCost = energyCost;
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
			int heal = UnityEngine.Random.Range (minHeal, maxHeal + 1);
			c.takeDamage ((int) -(heal * user.damageDealtModifier("heal")), "heal");
		} 
	}
}
