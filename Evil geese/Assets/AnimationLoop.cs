using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// changes the sprite/image of its parent object in order to animate it
public class AnimationLoop : MonoBehaviour {
	public List<Sprite> frames;
	public float frameRate;
	public bool loop;
	public bool obeysPause = true;

	SpriteRenderer ren;
	Image img;
	public float animationTime = 0f;
	GameStateManager state;
	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		ren = this.gameObject.GetComponent<SpriteRenderer> ();
		img = this.gameObject.GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!(obeysPause && state.isPaused)) {
			animationTime += Time.deltaTime;
		}
		if (frames == null || frames.Count == 0){
			return;
		}
		int nextFrame;
		if (loop) {
			nextFrame = Mathf.RoundToInt (animationTime * frameRate) % frames.Count;
		} else {
			nextFrame = Mathf.Min (Mathf.RoundToInt (animationTime * frameRate), frames.Count-1);
		}
		if (ren != null) {
			ren.sprite = frames [nextFrame];
		} else {
			img.sprite = frames [nextFrame];
		}
	}
}