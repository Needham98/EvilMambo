using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Grid))]
public class GridManager : Editor {
	int tempx = 0;
	int tempy = 0;
	public override void OnInspectorGUI(){
		Grid myTarget = (Grid)target;
		myTarget.OnAfterDeserialize ();
		DrawDefaultInspector ();

		EditorGUILayout.BeginHorizontal (null);
		EditorGUILayout.PrefixLabel(new GUIContent("x:"));
		tempx = EditorGUILayout.IntField (tempx, null);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal (null);
		EditorGUILayout.PrefixLabel(new GUIContent("y:"));
		tempy = EditorGUILayout.IntField (tempy, null);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("add")) {
			myTarget.setPosition (new GridPosition (tempx, tempy, blocked : true));
		}
		if (GUILayout.Button ("remove")) {
			myTarget.clearPosition (tempx, tempy);
		}
		myTarget.OnBeforeSerialize ();
	}
}
