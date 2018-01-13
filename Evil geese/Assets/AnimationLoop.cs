using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLoop : MonoBehaviour {
	public List<Sprite> frames;
	public float frameRate;
	public bool loop;
	public bool obeysPause = true;

	SpriteRenderer ren;
	public float animationTime = 0f;
	GameStateManager state;
	// Use this for initialization
	void Start () {
		state = GameStateManager.getGameStateManager ();
		try{
			ren = this.gameObject.GetComponent<SpriteRenderer> ();
		}catch (MissingComponentException){
			this.gameObject.AddComponent (typeof(SpriteRenderer));
			ren = this.gameObject.GetComponent<SpriteRenderer> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!(obeysPause && state.isPaused)) {
			animationTime += Time.deltaTime;
		}
		int nextFrame;
		if (loop) {
			nextFrame = Mathf.RoundToInt (animationTime * frameRate) % frames.Count;
		} else {
			nextFrame = Mathf.Min (Mathf.RoundToInt (animationTime * frameRate), frames.Count-1);
		}
		ren.sprite = frames [nextFrame];
	}
}