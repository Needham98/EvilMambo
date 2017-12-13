using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DialogManager))]
[CanEditMultipleObjects]
public class DialogEditor : Editor{
	DialogManager ownDialogManager;
	DialogElement currentDialogElement;
	string dialogName = "";
	enum modes {
		search,
		list,
		add
		}

	modes currentMode = modes.list;

	public override void OnInspectorGUI(){
		ownDialogManager = (DialogManager)target;
		DrawDefaultInspector ();
		currentMode = (modes) EditorGUILayout.EnumPopup(currentMode);
		switch (currentMode) {
		case modes.list:
			foreach (string s in ownDialogManager.dialogData.Keys) {
				EditorGUILayout.LabelField (s, null);
			}
			break;
		case modes.search:
			dialogName = EditorGUILayout.TextField (dialogName);
			if (ownDialogManager.dialogData.ContainsKey (dialogName)) {
				currentDialogElement = ownDialogManager.dialogData [dialogName];
				if (GUILayout.Button ("Delete")) {
					ownDialogManager.dialogData.Remove (dialogName);
				}

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Speeker ");
				currentDialogElement.speekerName = EditorGUILayout.TextField (currentDialogElement.speekerName);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.LabelField ("Text");
				currentDialogElement.dialogText = EditorGUILayout.TextArea (currentDialogElement.dialogText);
				//todo make this a bit clearer in the editor
				EditorGUILayout.LabelField ("");
				EditorGUILayout.LabelField ("Dialog options");
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Pointer");
				EditorGUILayout.LabelField ("Description");
				EditorGUILayout.LabelField ("");
				EditorGUILayout.EndHorizontal ();
				int toDelete = -1;
				for (int i = 0; i < currentDialogElement.optionPointer.Count; i++) {
					EditorGUILayout.BeginHorizontal ();
					currentDialogElement.optionPointer [i] = EditorGUILayout.TextField (currentDialogElement.optionPointer [i]);
					currentDialogElement.optionDescription [i] = EditorGUILayout.TextField (currentDialogElement.optionDescription [i]);
					if (GUILayout.Button("Delete")){
						toDelete = i;
					}
					EditorGUILayout.EndHorizontal ();
				} 
				if (toDelete != -1) {
					currentDialogElement.optionPointer.RemoveAt (toDelete);
					currentDialogElement.optionDescription.RemoveAt (toDelete);
				}
				if (GUILayout.Button ("Add dialog option")) {
					currentDialogElement.optionPointer.Add ("");
					currentDialogElement.optionDescription.Add ("");
				}

				//todo add actions editor


			}
			break;
		case modes.add:
			dialogName = EditorGUILayout.TextField (dialogName);
			if (GUILayout.Button(new GUIContent("add"))) {
				DialogElement newDialog = new DialogElement();
				ownDialogManager.dialogData.Add(dialogName, newDialog);
			}
			break;
		}

		
	}

}
