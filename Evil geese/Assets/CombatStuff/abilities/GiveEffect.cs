using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GiveEffect : CombatAbility {
	string ownName;
	public string abilityName { get { return ownName; } }
	public int minTargets { get { return 1; } }
	public int maxTargets { get { return 1; } }
	int ownCost;
	public int energyCost { get { return ownCost; } }
	bool ownAssist;
	public bool isAssist { get {return ownAssist; } }
	CombatEffect ownEffect;


	public GiveEffect(CombatEffect effect, int energyCost, bool isAssist, string abilityName){
		ownName = abilityName;
		ownAssist = isAssist;
		ownCost = energyCost;
		ownEffect = effect;
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
			c.addEffect (ownEffect);
		} 
	}
		
}
