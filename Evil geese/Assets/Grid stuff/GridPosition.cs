using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridPosition : System.Object {
	public int x;
	public int y;
	public GameObject interactionObject;
	public GameObject stepOn;
	public bool blocked;

	public GridPosition(int x, int y, GameObject interactionObject = null, GameObject stepOn = null, bool blocked = false){
		this.x = x;
		this.y = y;
		this.interactionObject = interactionObject;
		this.stepOn = stepOn;
		this.blocked = blocked;
	}
	public static ulong hashablePosition (int x, int y){ 
		ulong hashable = (ulong)(uint) x;
		hashable += ((ulong)(uint) y) << 32;
		return hashable;
	}

	public ulong hashable(){
		return hashablePosition (this.x, this.y);
	}
}