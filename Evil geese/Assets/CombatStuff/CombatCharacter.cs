using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CombatCharacter {
	public int maxHealth;
	public int health;
	public int maxEnergy;
	public int energy;
	public CombatAbility basicAttack;
	public List<CombatAbility> abilities;
	public CombatEntity entity;// the CombatEntity attached to the representation of this character
	public List<CombatEffect> effectList;

	//todo define spriteset system for combat characters
	public CombatCharacter(int maxHealth, int health,int maxEnergy, int energy, CombatAbility basicAttack = null){
		this.maxHealth = maxHealth;
		this.health = health;
		this.maxEnergy = maxEnergy;
		this.energy = energy;

		if (basicAttack == null){
			this.basicAttack = new SimpleAttack (8, 10, "melee");
		}
		else{
			this.basicAttack = basicAttack;
		}

		abilities = new List<CombatAbility> ();
		effectList = new List<CombatEffect> ();
	}

	public void AddAbility(CombatAbility ability){
		abilities.Add (ability);
	}
		
	public void takeDamage(int damage, string damageType = ""){
		health -= (int) (damage * damageTakenModifier (damageType));
		updateEntityBars ();
	}

	public float damageTakenModifier(string damageType){
		float returnValue = 1f;
		foreach (CombatEffect effect in effectList) {
			if (effect.type == CombatEffect.effectType.damageTakenModifier && effect.damageType.Equals(damageType) ) {
				returnValue *= effect.modifier;
			}
		}
		return returnValue;
	}

	public float damageDealtModifier(string damageType){
		float returnValue = 1f;
		foreach (CombatEffect effect in effectList) {
			if (effect.type == CombatEffect.effectType.damageDealtModifier && effect.damageType.Equals(damageType) ) {
				returnValue *= effect.modifier;
			}
		}
		return returnValue;
	}

	public void addEffect(CombatEffect effect){
		effectList.Add (effect);
	}

	public void doEffects(){
		for (int i = 0; i < effectList.Count; i++) {
			effectList[i].doEffect (this);
			effectList[i].turnsRemaining--;
			if (effectList [i].turnsRemaining == 0) {
				effectList.RemoveAt (i);
				i--;
			}
		}
	}

	public void updateEntityBars(){
		if (entity != null) {
			entity.setBars ((float) health / maxHealth, (float) energy / maxEnergy);
		}
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
