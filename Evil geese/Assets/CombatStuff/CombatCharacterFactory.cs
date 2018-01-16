using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class CombatCharacterFactory {
	public enum CombatCharacterPresets{// always store an element from this list instead of a CombatCharacter if a variable needs to be serialized
		PlayerCharBasic,
		PlayerCharTwo
	}

	public static CombatCharacter MakeCharacter(CombatCharacterPresets characterType){
		CombatCharacter newCharacter = null;
		int characterMaxHealth = GetCharacterMaxhealth (characterType);
		int characterMaxEnergy = GetCharacterMaxEnergy (characterType);
		switch (characterType){
		case CombatCharacterPresets.PlayerCharBasic:
			newCharacter = new CombatCharacter (characterMaxHealth, characterMaxHealth, 100, 100, new SimpleAttack (20, 30, "melee", 0));
			break;
		
		case CombatCharacterPresets.PlayerCharTwo:
			newCharacter = new CombatCharacter (characterMaxHealth, characterMaxHealth, 120, 120, new SimpleAttack (30, 40, "melee", 0));
			break;
		}
		List<CombatAbility> abilities = GetCharacterAbilities (characterType);
		foreach (CombatAbility ability in abilities) {
			newCharacter.AddAbility (ability);
		}
		return newCharacter;
	}

	public static string GetCharacterName(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.PlayerCharBasic:
			return "Test Character One";
		case CombatCharacterPresets.PlayerCharTwo:
			return "Test Character Two";
		}
		return "Character name not defined";
	}

	public static int GetCharacterMaxhealth(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.PlayerCharBasic:
			return 100;
		case CombatCharacterPresets.PlayerCharTwo:
			return 120;
		}
		return 1;
	}

	public static int GetCharacterMaxEnergy(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.PlayerCharBasic:
			return 100;
		case CombatCharacterPresets.PlayerCharTwo:
			return 120;
		}
		return 1;
	}

	public static List<CombatAbility> GetCharacterAbilities (CombatCharacterPresets characterType){
		List<CombatAbility> abilities = new List<CombatAbility> ();
		switch (characterType) {
		case CombatCharacterPresets.PlayerCharBasic:
			abilities.Add (new SimpleAttack (50, 60, "melee", 30, "Slash"));
			break;
		case CombatCharacterPresets.PlayerCharTwo:
			abilities.Add (new SimpleAttack (60, 70, "melee", 30, "Big slash"));
			abilities.Add (new SimpleAttack (80, 90, "melee", 40, "Really big slash"));
			break;
		}
		return abilities;
	}

	//TODO add GetCharacterBasicAttack
}
