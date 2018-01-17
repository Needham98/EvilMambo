using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, ISerializationCallbackReceiver{
	public string startSelectorVarName; // the name of the gameState variable that controls which of the above dialogs will be used
	public Dictionary<string, DialogElement> dialogData = new Dictionary<string, DialogElement>(); // the dictionary that contains all the dialog elements that might be used in this dialog

	//serialization variables
	[HideInInspector]
	[SerializeField]
	List<string> dialogDataKeys;
	[HideInInspector]
	[SerializeField]
	List<DialogElement> dialogDataValues;

	//variables used when dialog is active
	GameStateManager state;
	DialogElement currentDialog;
	GameObject dialogCanvasPrefab;
	GameObject dialogCanvasObj;
	List<GameObject> buttonObjs;



	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		dialogCanvasPrefab = (GameObject) Resources.Load ("DialogCanvas");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (currentDialog != null) {
			Transform panelTransform = dialogCanvasObj.transform.Find ("Panel");
			panelTransform.Find ("CharName").gameObject.GetComponent<UnityEngine.UI.Text> ().text = currentDialog.speekerName;
			string dialogText = currentDialog.dialogText;

			//replace "%ItemType" with the count of that item in inventory

			foreach (InventoryItems.itemTypes itemType in Enum.GetValues(typeof(InventoryItems.itemTypes))){
				dialogText = dialogText.Replace ("%" + itemType.ToString (), state.getItem (itemType).ToString ());
			}


			panelTransform.Find ("MainText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = dialogText;
			if (buttonObjs == null) {
				buttonObjs = new List<GameObject> ();
				GameObject buttonRefrence = panelTransform.Find ("DialogButton").gameObject;
				List<int> optionsVaild = new List<int> ();
				for (int i = 0; i < currentDialog.optionConditional.Count; i++) {
					if (currentDialog.optionConditional [i].evaluate ()) {
						optionsVaild.Add (i);
					}
				}

				float maxOffset = 990f; // the maximum spread between buttons
				int currentButtonCount = 0;
				foreach (int currentOption in optionsVaild){
					GameObject button = Instantiate (buttonRefrence, panelTransform);
					string optionPointer = currentDialog.optionPointer [currentOption];
					string optionDescription = currentDialog.optionDescription [currentOption];
					button.GetComponent<Button> ().onClick.AddListener (delegate {doDialogOption(optionPointer);});
					button.SetActive (true);
					button.transform.Find ("Text").gameObject.GetComponent<UnityEngine.UI.Text> ().text = optionDescription;
					Vector3 newButtonPos = button.transform.localPosition * 1; // multiply by one to force derefrence
					if (optionsVaild.Count > 1) {
						newButtonPos.x += maxOffset - maxOffset * ((currentButtonCount)/(float)(optionsVaild.Count-1));
					}else{
						newButtonPos.x += maxOffset;
					}
					button.transform.localPosition = newButtonPos;
					buttonObjs.Add (button);
					currentButtonCount++;
				}
			}
		}
	}

	public void beginDialog(){
		if (state == null) {
			state = GameStateManager.getGameStateManager ();
		}
		if (dialogCanvasPrefab == null) {
			dialogCanvasPrefab = (GameObject) Resources.Load ("DialogCanvas");
		}

		state.movementEnabled = false;
		string dialogName;
		if (startSelectorVarName.Length == 0) {
			dialogName = "start";
		}else{
			dialogName = state.getGameVar (startSelectorVarName);
		}
		if (dialogName == "") {
			Debug.Log ("the gameVar \"" + startSelectorVarName + "\" is undefined, using the \"start\" dialog");
			dialogName = "start";
		}
		dialogCanvasObj = Instantiate (dialogCanvasPrefab);
		doDialogOption (dialogName);
		}

	public void doDialogOption(string option){
		if (option == "" || option == "None") {// end dialog if dialog option is a dialog cancel option
			state.movementEnabled = true;
			currentDialog = null;
			Destroy (dialogCanvasObj);
			buttonObjs = null;
			return;
		}
		if (buttonObjs != null) {
			foreach (GameObject obj in buttonObjs) {
				Destroy (obj);
			}
		}
		buttonObjs = null;
		if (!dialogData.ContainsKey (option)) {
			Debug.Log ("Dialog error, node: \"" + option + "\" does not exist, ending dialog");
			doDialogOption (""); // ends dialog, see above
		}else{
			currentDialog = dialogData [option];
		}
		foreach (DialogAction action in currentDialog.actions) {
			action.doAction ();
		}
	}

	public void OnBeforeSerialize(){
		dialogDataKeys = new List<string> ();
		dialogDataValues = new List<DialogElement> ();
		foreach (string key in dialogData.Keys) {
			dialogDataKeys.Add (key);
			dialogDataValues.Add (dialogData [key]);
		}
	}

	public void OnAfterDeserialize(){
		dialogData = new Dictionary<string, DialogElement> ();
		for (int i = 0; i < dialogDataKeys.Count; i++) {
			dialogData.Add (dialogDataKeys [i], dialogDataValues [i]);
		}
	}
}

