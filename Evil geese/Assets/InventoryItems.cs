using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryItems {
	public enum itemTypes{
		beak
	}

	public static string itemDisplayName(itemTypes itemType){
		switch (itemType) {
		case itemTypes.beak:
			return "Goose Beak";
		default:
			return "error: invalid item";
		}

	}

	public static string itemDescription (itemTypes itemType){
		switch (itemType) {
		case itemTypes.beak:
			return "It appears to the the beak of a goose";
		default:
			return "error: No descrition exists for this item";	
		}
	}

	public static bool itemHasAbility(itemTypes itemType){
		switch (itemType) {
		case itemTypes.beak:
			return true;
		default:
			return itemAbility (itemType) == null;
		}
	}

	public static bool itemConsumedOnUse(itemTypes itemType){
		switch (itemType) {
		case itemTypes.beak:
			return false;
		default:
			return false;
		}
	}
		
	public static CombatAbility itemAbility(itemTypes itemType){
		switch (itemType) {
		case itemTypes.beak:
			Debug.Log ("what does the beak do?");
			return new SimpleAttack (0, 0, "beak", 0, "the beak thing");
		default:
			return null;
		}
	}
}
