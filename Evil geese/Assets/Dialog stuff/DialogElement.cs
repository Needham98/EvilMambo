using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// a single node in a dialog tree/graph
public class DialogElement {
	public string speekerName; // the name of the character who is speeking (in the story)
	public string dialogText;// the text content of this dialog

	public List<string> optionDescription;// the options the user may see after this dialog
	public List<string> optionPointer;// and the names of the dialog elements (in DialogManager) that they point to
	public List<DialogConditional> optionConditional; // and the conditionals that govern whether they're shown

	public List<DialogAction> actions;// the actions taken at the end of this bit of dialog

	public DialogElement() {
		optionPointer = new List<string> ();
		optionDescription = new List<string> ();
		optionConditional = new List<DialogConditional> ();
		actions = new List<DialogAction> ();
	}



}
