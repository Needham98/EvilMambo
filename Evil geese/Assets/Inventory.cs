using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	//(New class which contains the players inventory)
	private string[] items; //this is the players inventory, it can only contain 5 items
	private int full;//this tracks if the inventory is full 

	// Use this for initialization
	void Start () {
		items = new string[5] {"","","","",""};
		full = 0;
	}
	
	/// <summary>
	/// is item in the inventory.
	/// </summary>
	/// <returns><c>true</c>, if inventory contains itemtype, <c>false</c> otherwise.</returns>
	/// <param name="itemtype">Itemtype.</param>
	public bool inInventory (string itemtype) {
		for (int i = 0; i < items.Length; i++) {
			if (items [i] == itemtype) {
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// Add itemtype to inventory.
	/// </summary>
	/// <param name="itemtype">Itemtype.</param>
	public void additem(string itemtype){
		if (full < 5) {
			for (int i = 0; i < 5; i++) {
				if (items [i] == "") {
					items [i] = itemtype;
					full += 1;
					return;
				}
			}
		} else {
			return; 
		}
	}
	/// <summary>
	/// Remove the specified itemtype.
	/// </summary>
	/// <param name="itemtype">Itemtype.</param>
	public void Remove(string itemtype){
		for (int i = 0; i < 5; i++) {
			if (items [i] == itemtype) {
				items [i] = "";
				full -= 1;
				return;
			}
		}
		return;
		}
}
