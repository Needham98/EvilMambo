using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
	Dictionary<ulong, GridPosition> gridDictionary;
	public Grid(){
		this.gridDictionary = new Dictionary<ulong, GridPosition> ();
	}

	public GridPosition getPosition(int x, int y){
		try{
			return gridDictionary[GridPosition.hashablePosition(x,y)];
		} catch (KeyNotFoundException){
			return null;
		}
	}

	public void setPosition( GridPosition pos){
		gridDictionary.Add (pos.hashable (), pos);
	}

	public bool clearPosition(int x, int y){
		return gridDictionary.Remove (GridPosition.hashablePosition (x, y));
	}

	public bool clearPosition(GridPosition pos){
		return gridDictionary.Remove (pos.hashable ());
	}
}

public class GridPosition {
	public int x;
	public int y;
	public GameObject interactionObject;
	public GameObject stepOn;	

	public GridPosition(int x, int y, GameObject interactionObject = null, GameObject stepOn = null){
		this.x = x;
		this.y = y;
		this.interactionObject = interactionObject;
		this.stepOn = stepOn;
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