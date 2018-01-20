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

	public Dictionary<string, List<Sprite>> combatSprites;

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

	// calculates how much of a modifiyer a given damage type should have when deffending
	public float damageTakenModifier(string damageType){
		float returnValue = 1f;
		foreach (CombatEffect effect in effectList) {
			if (effect.type == CombatEffect.effectType.damageTakenModifier && effect.damageType == damageType) {
				returnValue *= effect.modifier;
			}
		}
		return returnValue;
	}

	// calculates how much of a modifiyer a given damage type should have when attacking
	public float damageDealtModifier(string damageType){
		float returnValue = 1f;
		foreach (CombatEffect effect in effectList) {
			if (effect.type == CombatEffect.effectType.damageDealtModifier && effect.damageType == damageType ) {
				returnValue *= effect.modifier;
			}
		}
		return returnValue;
	}

	public void addEffect(CombatEffect effect){
		effectList.Add (effect.copy());
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

	//changes the set of frames used by the CombatEntity assosiated with this CombatCharacter
	public void updateEntityAnimation(string animSet){
		if (entity != null && entity.animLoop != null) {
			entity.animLoop.animationTime = 0f;
			combatSprites.TryGetValue (animSet,  out entity.animLoop.frames);
			if ((entity.animLoop.frames == null || entity.animLoop.frames.Count == 0) && animSet != "base") {
				Debug.Log ("animation set \"" + animSet + "\" not found, reverting to \"base\" set");
				combatSprites.TryGetValue ("base",  out entity.animLoop.frames);
			}
		}
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
