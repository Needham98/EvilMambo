using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// stores the data for the various inventory item types
public static class InventoryItems {
	private static Inventory Inventory;
	public enum itemTypes{
		Beak,
		PlasticFork

	}

	public static string itemDisplayName(itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			return "Goose Beak";
		case itemTypes.PlasticFork:
			return "Plastic Fork";
		default:
			return "error: invalid item";
		}

	}

	public static string itemDescription (itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			return "It appears to the the beak of a goose";
		case itemTypes.PlasticFork:
			return "It's an ordinary plastic fork... why did you keep this?";
		default:
			return "error: No descrition exists for this item";	
		}
	}

	/// <summary>
	/// Checks if a item is in the inventory, uses their display name (new)
	/// </summary>
	/// <returns><c>true</c>, if in inventory was in item, <c>false</c> otherwise.</returns>
	/// <param name="itemType">Item type.</param>
	public static bool itemInInventory(itemTypes itemType){ //checks if the item is in the players inventory
		return Inventory.inInventory(itemDisplayName(itemType));
	}

	public static bool itemHasAbility(itemTypes itemType){
			return itemAbility (itemType) != null;
	}

	public static bool itemConsumedOnUse(itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			return false;
		case itemTypes.PlasticFork:
			return false;
		default:
			return false;
		}
	}
		
	public static CombatAbility itemAbility(itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			Debug.Log ("what does the beak do?");
			return new SimpleAttack (1, 1, "beak", 0, "Beak Poke");
		case itemTypes.PlasticFork:
			return new SimpleAttack (10, 20, "melee", 0, "Fork Stab");
		default:
			return null;
		}
	}
}
