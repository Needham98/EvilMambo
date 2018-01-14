using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class CombatSystemTests {

	[Test]
	public void CombatSystemTestsCharacterDamage() {
		CombatCharacter c1 = new CombatCharacter (100, 100, 100, 100, new SimpleAttack (20, 20, "melee"));
		CombatCharacter c2 = new CombatCharacter (100, 100, 100, 100, new SimpleAttack (20, 20, "melee"));

		Assert.AreEqual (100, c1.health);
		List<CombatCharacter> l = new List<CombatCharacter> ();
		l.Add (c1);
		c2.basicAttack.doAbility(l, c2);
		Assert.AreEqual (80, c1.health);

	}
}
