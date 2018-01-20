using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DialogSystemTests {
		
	[UnityTest]
	public IEnumerator DialogSystemConditionalConstantEvaluations() {
		GameStateTests.setupGameStateManagerForTests ();// dialog conditionals require a gameStateManager
		yield return null;
		DialogConditional cond = new DialogConditional ();
		cond.leftSideComparable = DialogConditional.comparableTypes.constant;
		cond.leftSide = "0";
		cond.rightSideComparable = DialogConditional.comparableTypes.constant;
		cond.rightSide = "1";
		cond.ownComparison = DialogConditional.comparisonTypes.le;

		Assert.True (cond.evaluate ());

		cond.rightSide = "-1";
		Assert.False (cond.evaluate ());
		cond.ownComparison = DialogConditional.comparisonTypes.ge;
		Assert.True (cond.evaluate ());
		cond.rightSide = "1";
		Assert.False (cond.evaluate ());
	}

	[UnityTest]
	public IEnumerator DialogSystemConditionalGamevarEvaluations() {
		GameStateTests.setupGameStateManagerForTests ();// dialog conditionals require a gameStateManager
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState (); // reset the gamestate
		DialogConditional cond = new DialogConditional ();
		cond.leftSideComparable = DialogConditional.comparableTypes.variable;
		cond.leftSide = "test";
		cond.rightSideComparable = DialogConditional.comparableTypes.constant;
		cond.rightSide = "testValue";
		cond.ownComparison = DialogConditional.comparisonTypes.eq;

		Assert.False(cond.evaluate ());

		state.setGameVar ("test", "1");
		Assert.False(cond.evaluate());
		state.setGameVar ("test", "testValue");
		Assert.True (cond.evaluate());
	}

	[UnityTest]
	public IEnumerator DialogSystemConditionalItemEvaluations() {
		GameStateTests.setupGameStateManagerForTests ();// dialog conditionals require a gameStateManager
		yield return null;
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state = new GameState (); // reset the gamestate
		DialogConditional cond = new DialogConditional ();
		cond.leftSideComparable = DialogConditional.comparableTypes.item;
		cond.leftItemType= InventoryItems.itemTypes.Beak;
		cond.rightSideComparable = DialogConditional.comparableTypes.constant;
		cond.rightSide = "1";
		cond.ownComparison = DialogConditional.comparisonTypes.eq;

		Assert.False(cond.evaluate ());
		cond.ownComparison = DialogConditional.comparisonTypes.lt;
		Assert.True (cond.evaluate ());
		state.changeItem(InventoryItems.itemTypes.Beak, 1);
		Assert.False (cond.evaluate ());
		cond.ownComparison = DialogConditional.comparisonTypes.eq;
		Assert.True (cond.evaluate());
	}

	[UnityTest]
	public IEnumerator DialogSystemConditionalStaticEvaluations() {
		GameStateTests.setupGameStateManagerForTests (); // dialog conditionals require a gameStateManager
		yield return null;
		DialogConditional cond = new DialogConditional ();
		cond.ownComparison = DialogConditional.comparisonTypes._true;

		Assert.True(cond.evaluate ());
		cond.ownComparison = DialogConditional.comparisonTypes._false;
		Assert.False(cond.evaluate());
	}
}
