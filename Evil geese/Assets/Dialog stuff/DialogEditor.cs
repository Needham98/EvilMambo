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
		Undo.RecordObject (ownDialogManager, "dialog change");
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
				for (int i = 0; i < currentDialogElement.optionPointer.Count; i++){
					if (currentDialogElement.optionConditional.Count < currentDialogElement.optionPointer.Count) {
						currentDialogElement.optionConditional.Add (new DialogConditional ());
					}

					EditorGUILayout.BeginHorizontal ();
					currentDialogElement.optionPointer [i] = EditorGUILayout.TextField (currentDialogElement.optionPointer [i]);
					currentDialogElement.optionDescription [i] = EditorGUILayout.TextField (currentDialogElement.optionDescription [i]);
					if (GUILayout.Button("Delete")){
						toDelete = i;
					}
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					bool showConditionalDetail = true;
					DialogConditional currentConditional = currentDialogElement.optionConditional [i];
					if (currentConditional.ownComparison == DialogConditional.comparisonTypes._false
						|| currentConditional.ownComparison == DialogConditional.comparisonTypes._true) {
						showConditionalDetail = false;
					}

					if (showConditionalDetail) {
						if (currentConditional.leftSideComparable == DialogConditional.comparableTypes.item) {
							currentConditional.leftItemType = (InventoryItems.itemTypes)EditorGUILayout.EnumPopup (currentConditional.leftItemType);
						}else{
							currentConditional.leftSide = EditorGUILayout.TextField(currentConditional.leftSide);
						}
					}

					currentConditional.ownComparison = (DialogConditional.comparisonTypes) EditorGUILayout.EnumPopup (currentConditional.ownComparison);

					if (showConditionalDetail) {
						if (currentConditional.rightSideComparable == DialogConditional.comparableTypes.item) {
							currentConditional.rightItemType = (InventoryItems.itemTypes)EditorGUILayout.EnumPopup (currentConditional.rightItemType);
						}else{
							currentConditional.rightSide = EditorGUILayout.TextField(currentConditional.rightSide);
						}
					}
					EditorGUILayout.EndHorizontal ();

					if (showConditionalDetail) {
						EditorGUILayout.BeginHorizontal ();
						currentConditional.leftSideComparable = (DialogConditional.comparableTypes)EditorGUILayout.EnumPopup (currentConditional.leftSideComparable);
						currentConditional.rightSideComparable = (DialogConditional.comparableTypes)EditorGUILayout.EnumPopup (currentConditional.rightSideComparable);
						EditorGUILayout.EndHorizontal ();
					}

					EditorGUILayout.LabelField ("");
				} 
				if (toDelete != -1) {
					currentDialogElement.optionPointer.RemoveAt (toDelete);
					currentDialogElement.optionDescription.RemoveAt (toDelete);
					currentDialogElement.optionConditional.RemoveAt (toDelete);
				}
				if (GUILayout.Button ("Add dialog option")) {
					currentDialogElement.optionPointer.Add ("");
					currentDialogElement.optionDescription.Add ("");
					currentDialogElement.optionConditional.Add(new DialogConditional());
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
					case DialogAction.actionType.giveItem:
						action.itemType = (InventoryItems.itemTypes)EditorGUILayout.EnumPopup (action.itemType);
						action.itemAmount = EditorGUILayout.IntField (action.itemAmount);
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
