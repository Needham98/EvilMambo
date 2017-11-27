using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CombatAbility {
	int minTargets {get;}
	int maxTargets {get;}
	bool isAssist {get;}
	string abilityName {get; set;}

	void DoAbility (List<CombatCharacter> targets);

}
