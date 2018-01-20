using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GridSystemTests {

	[Test]
	public void GridSystemCollisions() {
		GameObject o = new GameObject ();
		Grid g = o.AddComponent<Grid> ();
		Assert.IsNull (g.getPosition (0, 0));
		Assert.IsNull (g.getPosition (1, 1));
		g.setPosition (new GridPosition (0, 0,blocked:true));

		Assert.IsNotNull (g.getPosition (0, 0));
		Assert.IsNull (g.getPosition (1, 1));

		g.clearPosition (0, 0);
		Assert.IsNull(g.getPosition(0,0));
	}

}
