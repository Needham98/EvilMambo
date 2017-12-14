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
		GUILayout.ExpandWidth (false);
		ownDialogManager = (DialogManager)target;
		DrawDefaultInspector ();
		currentMode = (modes) EditorGUILayout.EnumPopup(currentMode);
		switch (currentMode) {
		case modes.list:
			foreach (string s in ownDialogManager.dialogData.Keys) {
				if (GUILayout.Button(s)){
					currentMode = modes.search;
					dialogName = s;
				}
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
				EditorGUILayout.LabelField ("");
				EditorGUILayout.LabelField ("Dialog options");

				EditorGUILayout.BeginHorizontal ();
				EditorGUIUtility.labelWidth = 60;
				EditorGUILayout.LabelField ("Pointer");
				EditorGUIUtility.labelWidth = 60;
				EditorGUILayout.LabelField ("Description");
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
				EditorGUILayout.LabelField("");
				EditorGUILayout.LabelField ("Dialog actions");
				DialogAction actionToDelete = null;
				foreach (DialogAction action in currentDialogElement.actions) {
					action.ownActionType = (DialogAction.actionType)EditorGUILayout.EnumPopup (action.ownActionType);
					switch (action.ownActionType) {
					case DialogAction.actionType.setGameVar:
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Variable Name");
						action.gameVarName = EditorGUILayout.TextField (action.gameVarName);
						EditorGUILayout.EndHorizontal ();
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Variable Value");
						action.gameVarValue = EditorGUILayout.TextField (action.gameVarValue);
						EditorGUILayout.EndHorizontal ();
						break;
					
					case DialogAction.actionType.startCombat:
						int numberOfEnemies = action.combatEnemies.Count;
						numberOfEnemies = EditorGUILayout.IntSlider (action.combatEnemies.Count, 1, 5);
						if (numberOfEnemies > action.combatEnemies.Count) {
							for (int i = action.combatEnemies.Count; i < numberOfEnemies; i++) {
								action.combatEnemies.Add (CombatCharacterFactory.CombatCharacterPresets.PlayerCharBasic);
							}
						} else if (numberOfEnemies < action.combatEnemies.Count) {
							action.combatEnemies.RemoveRange (numberOfEnemies - 1, action.combatEnemies.Count - numberOfEnemies);
						}

						for (int i = 0; i < action.combatEnemies.Count; i++) {
							action.combatEnemies[i]	= (CombatCharacterFactory.CombatCharacterPresets) EditorGUILayout.EnumPopup(action.combatEnemies[i]);
						}

						break;
					}
					if (GUILayout.Button ("Delete")) {
						actionToDelete = action;
					}
					EditorGUILayout.LabelField ("");
				}
				if (actionToDelete != null) {
					currentDialogElement.actions.Remove (actionToDelete);
				}

				if (GUILayout.Button ("Add dialog action")) {
					currentDialogElement.actions.Add (new DialogAction ());
				}


			}
			break;
		case modes.add:
			dialogName = EditorGUILayout.TextField (dialogName);
			if (GUILayout.Button(new GUIContent("add"))) {
				DialogElement newDialog = new DialogElement();
				ownDialogManager.dialogData.Add(dialogName, newDialog);
				currentMode = modes.search;
			}
			break;
		}

		
	}

}
