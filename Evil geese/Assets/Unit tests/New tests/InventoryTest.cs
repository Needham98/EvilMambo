using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

public class InventoryTest {

	public static void setupGameStateManagerForTests(){
		GameObject stateObject = new GameObject ();
		stateObject.tag = "GameStateManager";
		stateObject.AddComponent<GameStateManager> ();
	}

	[UnityTest]
	public IEnumerator InventoryTestSimplePasses() {
		// A simple test to check that inventory items return the correct attack when used
		//Based on Evil Geese's preexisting combat test and GameStateTest
		setupGameStateManagerForTests();
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState(); //reset the gamestate

		InventoryItems.itemTypes[] items =(InventoryItems.itemTypes[]) Enum.GetValues (typeof(InventoryItems.itemTypes));


		CombatCharacter c1 = new CombatCharacter (100, 100, 100, 100, new SimpleAttack (20, 20, "melee"));
		CombatCharacter c2 = new CombatCharacter (100, 100, 100, 100, InventoryItems.itemAbility(items[0]));

		Assert.AreEqual (100, c1.health);
		List<CombatCharacter> l = new List<CombatCharacter> ();
		l.Add (c1);
		c2.basicAttack.doAbility(l, c2);
		Assert.AreEqual (85, c1.health);
	
	}
		
}
