using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCollisionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int	count = 0;
		Dictionary<ulong, int> dict = new Dictionary<ulong,int> ();
		for (int x = -256; x < 256; x += 1) {
			for (int y = -256; y < 256; y += 1) {
				count += 1;
				ulong value = GridPosition.hashablePosition (x, y);
				if (dict.ContainsKey (value)) {
					Debug.Log (value);
				}
				dict.Add (value, 1);
			}
		}
		if (dict.Count == count) {
			Debug.Log ("Passed position collision test");
		} else {
			Debug.Log ("FAILURE: position collision test!");
			Debug.Log (dict.Count);
		}
	}
}
