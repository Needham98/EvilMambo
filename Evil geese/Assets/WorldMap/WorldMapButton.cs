using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapButton : MonoBehaviour, ICanvasRaycastFilter {
	GameStateManager state;
	public string varibleName;
	public bool defaultState;
	public string sceneName;
	public int playerX;
	public int playerY;
	Image ownImage;
	GameObject ownText;
	Camera sceneCamera;


	// Use this for initialization
	void Start () {
	}

	void Awake(){
		sceneCamera = FindObjectOfType<Camera> ();
		ownText = this.transform.Find ("Text").gameObject;
		ownImage = this.GetComponent<Image> ();
		state = GameStateManager.getGameStateManager ();
		bool enabled;
		if (state.getGameVar (varibleName) == "") {
			enabled = defaultState;
		} else if (state.getGameVar (varibleName) == "true") {
			enabled = true;
		} else {
			enabled = false;
		}

		UnityEngine.UI.Button button = this.GetComponent<UnityEngine.UI.Button> ();
		button.interactable = enabled;
		ownText.SetActive (enabled);
		button.onClick.AddListener (delegate {
			enterScene (sceneName, playerX, playerY);
			});
	}

	public void enterScene(string newScene, int playerX, int playerY){
		GameStateManager state = GameStateManager.getGameStateManager ();
		state.state.playerX = playerX;
		state.state.playerY = playerY;
		SceneManager.LoadScene (newScene);
		state.hasLoaded = true;
	}

	//used to allow transparent sections of buttons to not count as part of the button, (part of ICanvasRaycastFilter interface)
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera){
		//assumes that the world map fills the entire screen
		Texture2D tex = (Texture2D) ownImage.mainTexture;
		int x = (int) Mathf.Floor ((sp.x /(sceneCamera.pixelWidth)) * ownImage.mainTexture.width);
		int y = (int) Mathf.Floor ((sp.y /(sceneCamera.pixelHeight)) * ownImage.mainTexture.height);
		Debug.Log (x.ToString () + ", " + y.ToString ());

		return tex.GetPixel(x,y).a > 0;
	}
}
