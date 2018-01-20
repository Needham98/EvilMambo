using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CombatEffect {
	public int turnsRemaining;
	public enum effectType {abilityPerTurn, damageDealtModifier, damageTakenModifier}
	public effectType type;
	CombatAbility ability;
	public string damageType;
	public float modifier;

	public CombatEffect(effectType type, int turnsRemaining, CombatAbility ability = null, string damageType = "", float modifier = 0f){
		this.turnsRemaining = turnsRemaining;
		this.type = type;
		if (type == effectType.abilityPerTurn) {
			this.ability = ability;
		}
		if (type == effectType.damageDealtModifier || type == effectType.damageTakenModifier) {
			this.damageType = damageType;
			this.modifier = modifier;
		}
	}

	public void doEffect(CombatCharacter character){
		if (type == effectType.abilityPerTurn) {
			List<CombatCharacter> characterList = new List<CombatCharacter> ();
			characterList.Add (character);
			ability.doAbility (characterList, character);
		}
	}

	public CombatEffect copy(){
		return (CombatEffect) this.MemberwiseClone ();
	}
}
