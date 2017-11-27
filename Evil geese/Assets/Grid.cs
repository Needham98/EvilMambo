using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, ISerializationCallbackReceiver{
	public Dictionary<ulong, GridPosition> gridDictionary;
	//these are both for serialization
	public List<ulong> _keys;
	public List<GridPosition> _positions;

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

	// todo remove keys from serialization system since they can be calculated from the positions
	public void OnBeforeSerialize(){
		_keys = new List<ulong> ();
		_positions = new List<GridPosition> ();
		foreach (ulong key in gridDictionary.Keys) {
			_keys.Add (key);
			_positions.Add(gridDictionary[key]);
		}
	}

	public void OnAfterDeserialize(){
		gridDictionary = new Dictionary<ulong, GridPosition> ();
		for (int i = 0; i < _keys.Count; i += 1) {
			gridDictionary.Add (_keys [i], _positions [i]);
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		foreach (GridPosition pos in gridDictionary.Values) {
			Gizmos.DrawSphere (new Vector3 ((float)pos.x, (float)pos.y, 0.0f), 0.4f);
		}
	}
}
// todo move to own file
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