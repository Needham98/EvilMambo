using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, ISerializationCallbackReceiver{
	public string startSelectorVarName; // the name of the gameState variable that controls which of the above dialogs will be used
	public Dictionary<string, DialogElement> dialogData; // the dictionary that contains all the dialog elements that might be used in this dialog

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
		currentDialog = null;
		state = GameStateManager.getGameStateManager ();
		dialogCanvasPrefab = (GameObject) Resources.Load ("DialogCanvas");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (currentDialog != null) {
			Transform panelTransform = dialogCanvasObj.transform.Find ("Panel");
			panelTransform.Find ("CharName").gameObject.GetComponent<UnityEngine.UI.Text> ().text = currentDialog.speekerName;
			panelTransform.Find ("MainText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = currentDialog.dialogText;
			//todo make dialog apear slowly (add one character at a time till the whole thing is displayed)
			if (buttonObjs == null) {
				buttonObjs = new List<GameObject> ();
				GameObject buttonRefrence = panelTransform.Find ("DialogButton").gameObject;
				float maxOffset = 990f; // the maximum spread between buttons
				for (int i = 0; i < currentDialog.optionPointer.Count; i++) {
					GameObject button = Instantiate (buttonRefrence, panelTransform);
					string optionPointer = currentDialog.optionPointer [i];
					string optionDescription = currentDialog.optionDescription [i];
					button.GetComponent<Button> ().onClick.AddListener (delegate {doDialogOption(optionPointer);});
					button.SetActive (true);
					button.transform.Find ("Text").gameObject.GetComponent<UnityEngine.UI.Text> ().text = optionDescription;
					Vector3 newButtonPos = button.transform.localPosition * 1; // multiply by one to force derefrence
					if (currentDialog.optionPointer.Count > 1) {
						newButtonPos.x += maxOffset - maxOffset * ((i)/(float)(currentDialog.optionPointer.Count-1));
					}else{
						newButtonPos.x += maxOffset;
					}
					button.transform.localPosition = newButtonPos;
					buttonObjs.Add (button);
				}
			}
		}
	}

	public void beginDialog(){
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
		doDialogOption (dialogName);
		dialogCanvasObj = Instantiate (dialogCanvasPrefab);
		}

	public void doDialogOption(string option){
		if (option == "") {
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

