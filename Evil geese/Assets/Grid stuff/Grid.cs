using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO refactor Grid to GridManager
public class Grid : MonoBehaviour, ISerializationCallbackReceiver{
	public Dictionary<ulong, GridPosition> gridDictionary;
	//these are both for serialization
	public List<ulong> _keys;
	public List<GridPosition> _positions;

	public Grid(){
		this.gridDictionary = new Dictionary<ulong, GridPosition> ();
	}

	// gets the gridPosition at a set of coordinates if one exists
	public GridPosition getPosition(int x, int y){
		try{
			return gridDictionary[GridPosition.hashablePosition(x,y)];
		} catch (KeyNotFoundException){
			return null;
		}
	}

	// sets a grid position
	public void setPosition( GridPosition pos){
		gridDictionary.Add (pos.hashable (), pos);
	}

	// clears a grid postion at a set of coordinates
	public bool clearPosition(int x, int y){
		return gridDictionary.Remove (GridPosition.hashablePosition (x, y));
	}

	// clears a gridposition
	public bool clearPosition(GridPosition pos){
		return gridDictionary.Remove (pos.hashable ());
	}
		
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