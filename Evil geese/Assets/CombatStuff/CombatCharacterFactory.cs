using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class CombatCharacterFactory {
	public enum CombatCharacterPresets{// always store an element from this list instead of a CombatCharacter if a variable needs to be serialized
		BobbyBard,
		CharlieCleric,
		MabelMage,
		SusanShapeShifter,
		WalterWizard,
		PamelaPaladin,
		Goose,
        //added enums by UnTtwo for new enemies
        Robot,
        Shrub,
        finalGoose,
        gooseBoss

	}

	public static CombatCharacter MakeCharacter(CombatCharacterPresets characterType){
		CombatCharacter newCharacter = null;
		int characterMaxHealth = GetCharacterMaxhealth (characterType);
		int characterMaxEnergy = GetCharacterMaxEnergy (characterType);
		CombatAbility basicAttack = getCharacterBasicAttack (characterType);
		switch (characterType){
		case CombatCharacterPresets.BobbyBard:
			newCharacter = new CombatCharacter (characterMaxHealth, characterMaxHealth / 2, characterMaxEnergy, characterMaxEnergy, basicAttack);
			break;
		default:
			newCharacter = new CombatCharacter(characterMaxHealth, characterMaxHealth, characterMaxEnergy, characterMaxEnergy, basicAttack);
			break;
		}
		List<CombatAbility> abilities = GetCharacterAbilities (characterType);
		foreach (CombatAbility ability in abilities) {
			newCharacter.AddAbility (ability);
		}
		newCharacter.combatSprites = getCharacterSprites (characterType);
		return newCharacter;
	}

	public static string GetCharacterName(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.BobbyBard:
			return "Bobby The Bard";
		case CombatCharacterPresets.CharlieCleric:
			return "Charlie The Cleric";
		case CombatCharacterPresets.MabelMage:
			return "Mabel The Mage";
		case CombatCharacterPresets.PamelaPaladin:
			return "Pamela The Paladin";
		case CombatCharacterPresets.WalterWizard:
			return "Walter The Wizard";
		case CombatCharacterPresets.SusanShapeShifter:
			return "Susan The ShapeShifter";
		case CombatCharacterPresets.Goose:
			return "Goose";
        case CombatCharacterPresets.Robot:
            return "Robot";
        case CombatCharacterPresets.Shrub:
            return "Shrub";
        case CombatCharacterPresets.finalGoose:
            return "finalGoose";
        case CombatCharacterPresets.gooseBoss:
            return "gooseBoss";


        }
		return "Character name not defined";
	}

	public static int GetCharacterMaxhealth(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.BobbyBard:
			return 70;
		case CombatCharacterPresets.CharlieCleric:
			return 100;
		case CombatCharacterPresets.MabelMage:
			return 130;
		case CombatCharacterPresets.PamelaPaladin:
			return 180;
		case CombatCharacterPresets.WalterWizard:
			return 100;
		case CombatCharacterPresets.SusanShapeShifter:
			return 100;
		case CombatCharacterPresets.Goose:
			return 50;
        case CombatCharacterPresets.Robot:
            return 80;
        case CombatCharacterPresets.Shrub:
           	return 80;
        case CombatCharacterPresets.finalGoose:
            return 250;
        case CombatCharacterPresets.gooseBoss:
            return 250;
        }
		return 1;
	}

	public static int GetCharacterMaxEnergy(CombatCharacterPresets characterType){
		switch (characterType) {
		case CombatCharacterPresets.BobbyBard:
			return 200;
		case CombatCharacterPresets.CharlieCleric:
			return 150;
		case CombatCharacterPresets.MabelMage:
			return 130;
		case CombatCharacterPresets.PamelaPaladin:
			return 60;
		case CombatCharacterPresets.WalterWizard:
			return 150;
		case CombatCharacterPresets.SusanShapeShifter:
			return 120;
        case CombatCharacterPresets.Shrub:
            return 100;
        case CombatCharacterPresets.finalGoose:
            return 200;
        case CombatCharacterPresets.gooseBoss:
            return 200;
        }
		return 1;
	}

	public static List<CombatAbility> GetCharacterAbilities (CombatCharacterPresets characterType){
		List<CombatAbility> abilities = new List<CombatAbility> ();
		switch (characterType) {
		case CombatCharacterPresets.BobbyBard:
			abilities.Add (new SimpleHeal (30, 50, 40, "Basic Heal"));
			abilities.Add( new GiveEffect (new CombatEffect( CombatEffect.effectType.damageDealtModifier, 3, damageType : "melee", modifier : 1.5f), 30, true, "Melee Buff"));
			break;
		case CombatCharacterPresets.CharlieCleric:
			abilities.Add (new SimpleHeal (30, 50, 40, "Basic Heal"));
			abilities.Add (new SimpleHeal (80, 100, 80, "Big Heal"));
			break;
		case CombatCharacterPresets.MabelMage:
			abilities.Add (new SimpleAttack (60, 80, "melee", 40, "Heavy Strike"));
			abilities.Add (new SimpleAttack (65, 75, "magic", 40, "Ethereal Projectile"));
			break;
		case CombatCharacterPresets.PamelaPaladin:
			abilities.Add (new SimpleAttack (60, 80, "melee", 40, "Heavy Strike"));
			abilities.Add (new SimpleAttack (80, 100, "melee", 80, "Epic Strike"));
			break;
		case CombatCharacterPresets.WalterWizard:
			abilities.Add (new SimpleAttack (60, 70, "fire", 30, "Fireball"));
			abilities.Add (new SimpleAttack (65, 100, "electric", 70, "Lightning Strike"));
			break;
		case CombatCharacterPresets.SusanShapeShifter:
			abilities.Add (new SelfEffect (new CombatEffect (CombatEffect.effectType.damageTakenModifier, 3, damageType : "melee", modifier : 0.2f), 40, "Skin of Steel"));
			abilities.Add (new SelfEffect (new CombatEffect (CombatEffect.effectType.damageDealtModifier, 3, damageType : "melee", modifier : 4f), 40, "Knives For Hands"));
			break;
		case CombatCharacterPresets.Goose:
			break;
        case CombatCharacterPresets.Robot:
            break;
        case CombatCharacterPresets.Shrub:
            break;
        case CombatCharacterPresets.finalGoose:
            break;
        case CombatCharacterPresets.gooseBoss:
            break;

        }
		return abilities;
	}

	public static CombatAbility getCharacterBasicAttack(CombatCharacterPresets characterType){
		switch (characterType) {
		default:
			return new SimpleAttack (20, 30, "melee", 0);
		}
	}

	public static Dictionary<string, List<Sprite>> getCharacterSprites(CombatCharacterPresets characterType){
		Dictionary<string, List<Sprite>> sprites = new Dictionary<string, List<Sprite>> ();
		List<Sprite> frames = new List<Sprite> ();
		switch (characterType) {
		case CombatCharacterPresets.BobbyBard:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 6/F4"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 6/F5"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 6/F6"));
			sprites.Add ("attack", frames);
			break;

		case CombatCharacterPresets.CharlieCleric:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 4/D4"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 4/D7"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 4/D6"));
			sprites.Add ("attack", frames);
			break;
		
		case CombatCharacterPresets.MabelMage:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 2/A4"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 2/A5"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 2/A6"));
			sprites.Add ("attack", frames);
			break;
		
		case CombatCharacterPresets.PamelaPaladin:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 5/E4"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 5/E5"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 5/E6"));
			sprites.Add ("attack", frames);
			break;
		
		case CombatCharacterPresets.SusanShapeShifter:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 3/B4"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 3/B5"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 3/B6"));
			sprites.Add ("attack", frames);
			break;
		
		case CombatCharacterPresets.WalterWizard:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 1/c21"));
			sprites.Add ("base", frames);
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 1/c4"));
			frames.Add (Resources.Load<Sprite> ("Sprites/Character 1/c6"));
			sprites.Add ("attack", frames);
			break;

		case CombatCharacterPresets.Goose:
			frames = new List<Sprite> ();
			frames.Add (Resources.Load<Sprite>("Sprites/Goose"));
			sprites.Add ("base", frames);
			break;

            case CombatCharacterPresets.Robot:
                frames = new List<Sprite>();
                frames.Add(Resources.Load<Sprite>("Sprites/Robot"));
                sprites.Add("base", frames);
                break;

            case CombatCharacterPresets.Shrub:
                frames = new List<Sprite>();
                frames.Add(Resources.Load<Sprite>("Sprites/Shrub"));
                sprites.Add("base", frames);
                break;

            case CombatCharacterPresets.finalGoose:
                frames = new List<Sprite>();
                frames.Add(Resources.Load<Sprite>("Sprites/finalGoose"));
                sprites.Add("base", frames);
                break;

            case CombatCharacterPresets.gooseBoss:
                frames = new List<Sprite>();
                frames.Add(Resources.Load<Sprite>("Sprites/gooseBoss"));
                sprites.Add("base", frames);
                break;
        }
		return sprites;
	
	}
}

