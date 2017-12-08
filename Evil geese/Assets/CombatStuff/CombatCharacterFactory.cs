using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class CombatCharacterFactory {
	public enum CombatCharacterPresets{// always store an element from this list instead of a CombatCharacter if a variable needs to be serialized
		PlayerCharBasic 
	}

	public static CombatCharacter MakeCharacter(CombatCharacterPresets characterType){
		CombatCharacter newCharacter;
		switch (characterType){
		case CombatCharacterPresets.PlayerCharBasic:
			newCharacter = new CombatCharacter (100, 100, 100, 100, new SimpleAttack (20, 30, "melee", 0));
			newCharacter.AddAbility (new SimpleAttack (50, 60, "melee", 30, "Slash"));
			return newCharacter;
		
		}
		return null;
	}
}
