using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// stores the data for the various inventory item types
public static class InventoryItems {
	private static Inventory Inventory;
	public enum itemTypes{
		Beak,
		PlasticFork,
		// new items
		KeyCard,
		CateringCard,
		YusuJumper,
		ElectionFlier,
		ReturnTicket

	}

	public static string itemDisplayName(itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			return "Goose Beak";
		case itemTypes.PlasticFork:
			return "Plastic Fork";
		case itemTypes.KeyCard:
			return "Key Card";
		case itemTypes.CateringCard:
			return "Catering Card";
		case itemTypes.YusuJumper:
			return "YUSU Jumper";
		case itemTypes.ElectionFlier:
			return "Election Flier";
		case itemTypes.ReturnTicket:
			return "Return Ticket";
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
		case itemTypes.KeyCard:
			return "It's a University Key Card, apparently you don't need this";
		case itemTypes.CateringCard:
			return "It's a Catering Card, food served 17:00 - 19:30";
		case itemTypes.YusuJumper:
			return "A University of York themed Jumper";
		case itemTypes.ElectionFlier:
			return "A Flier for the YUSU elections, seems like a waste of paper";
		case itemTypes.ReturnTicket:
			return "It's a 3 month old Return Ticket";
		default:
			return "error: No descrition exists for this item";
		}
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
		case itemTypes.KeyCard:
			return false;
		case itemTypes.CateringCard:
			return false;
		default:
			return true;
		}
	}
		
	public static CombatAbility itemAbility(itemTypes itemType){
		switch (itemType) {
		case itemTypes.Beak:
			Debug.Log ("what does the beak do?");
			return new SimpleAttack (15, 15, "beak", 0, "Beak Poke");
		case itemTypes.PlasticFork:
			return new SimpleAttack (10, 20, "melee", 0, "Fork Stab");
		case itemTypes.KeyCard:
			return new SimpleAttack (10, 25, "melee", 0, "KeyCard Swing");
		case itemTypes.CateringCard:
			return new SimpleHeal (10,30,0,"Food");
		case itemTypes.ElectionFlier:
			return new SimpleAttack(0,100,"melee",0,"Canvasing");
		case itemTypes.YusuJumper:
			return new SimpleAttack (15, 35, "melee", 0, "Smother by merchandising");
		case itemTypes.ReturnTicket:
			return new SimpleAttack (20, 30, "melee", 0, "Torn Sorrow");
		default:
			return null;
		}
	}
}
