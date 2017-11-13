using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Grid))]
[CanEditMultipleObjects]
public class GridManager : Editor {
	int tempx = 0;
	int tempy = 0;
	GridPosition pos;
	public override void OnInspectorGUI(){
		Grid myTarget = (Grid)target;
		myTarget.OnAfterDeserialize ();
		//DrawDefaultInspector ();

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
		if (GUILayout.Button ("search")) {
			pos = myTarget.getPosition (tempx, tempy);
		}
		if (pos != null) {
			EditorGUILayout.BeginHorizontal (null);
			EditorGUILayout.PrefixLabel(new GUIContent("x:"));
			pos.x = EditorGUILayout.IntField (pos.x, null);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal (null);
			EditorGUILayout.PrefixLabel(new GUIContent("y:"));
			pos.y = EditorGUILayout.IntField (pos.y, null);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal (null);
			EditorGUILayout.PrefixLabel(new GUIContent("blocked:"));
			pos.blocked = EditorGUILayout.Toggle (pos.blocked, null);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal (null);
			EditorGUILayout.PrefixLabel(new GUIContent("interaction:"));
			pos.interactionObject = (GameObject) EditorGUILayout.ObjectField (pos.interactionObject, null, allowSceneObjects: true);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal (null);
			EditorGUILayout.PrefixLabel(new GUIContent("step on:"));
			pos.stepOn = (GameObject) EditorGUILayout.ObjectField (pos.stepOn, null, allowSceneObjects: true);
			EditorGUILayout.EndHorizontal ();

		}
		myTarget.OnBeforeSerialize ();
	}
}
