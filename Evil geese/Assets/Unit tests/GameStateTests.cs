using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameStateTests {

	public static void setupGameStateManagerForTests(){
		GameObject stateObject = new GameObject ();
		stateObject.tag = "GameStateManager";
		stateObject.AddComponent<GameStateManager> ();
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator GameStateInitalizedEmpty() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		setupGameStateManagerForTests();
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState(); //reset the gamestate

		Assert.NotNull (state);
		Assert.True (state.inventory.Count == 0);
		Assert.True (state.getGameVar ("") == "");
	}

	[UnityTest]
	public IEnumerator GameStateRetainsGameVars(){
		setupGameStateManagerForTests();
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();

		Assert.AreEqual ("", state.getGameVar("test"));
		state.setGameVar ("test", "testValue");
		Assert.AreEqual ("testValue", state.getGameVar ("test"));
	}

	[UnityTest]
	public IEnumerator GameStateRetainsInventoryItems(){
		setupGameStateManagerForTests();
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState(); //reset the gamestate

		InventoryItems.itemTypes[] items =(InventoryItems.itemTypes[]) Enum.GetValues (typeof(InventoryItems.itemTypes));

		foreach (InventoryItems.itemTypes item  in items) {
			Assert.AreEqual (0, state.getItem(item));
			state.changeItem (item, 1);
			Assert.AreEqual (1, state.getItem(item));
		}
	}

	[UnityTest]
	public IEnumerator GameStateSavesCorrectly(){
		setupGameStateManagerForTests();
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState(); //reset the gamestate
		SceneManager.LoadScene ("Scenes/ComputerScience");// has to be in a scene included in the build options for loading to work
		yield return null;

		InventoryItems.itemTypes[] items =(InventoryItems.itemTypes[]) Enum.GetValues (typeof(InventoryItems.itemTypes));

		foreach (InventoryItems.itemTypes item  in items) {
			state.changeItem (item, 1);
		}
		state.setGameVar ("test", "testValue");
		state.state.sceneName = SceneManager.GetActiveScene ().name;
		state.saveState ("testSave");// save

		SceneManager.LoadScene (SceneManager.GetSceneAt (0).name);// change scenes to the first scene in the build settings
		state.state = new GameState(); //reset the gamestate

		Assert.AreEqual("", state.getGameVar("test"));// check it was reset
		state.loadState ("testSave");// load

		Assert.AreEqual ("ComputerScience", SceneManager.GetActiveScene ().name);// check that it loaded the correct scene
		Assert.AreEqual ("testValue", state.getGameVar ("test"));//check gamevars were loaded

		foreach (InventoryItems.itemTypes item  in items) {
			Assert.AreEqual (1, state.getItem(item)); //check inventory was loaded
		}

	}


}
