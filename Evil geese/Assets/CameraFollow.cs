using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Vector2 minPos;
	public Vector2 maxPos;

	GameObject player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.activeSelf) {
			float newX = Mathf.Clamp (player.transform.position.x, minPos.x, maxPos.x);
			float newY = Mathf.Clamp (player.transform.position.y, minPos.y, maxPos.y);
			this.transform.position = new Vector3 (newX, newY, this.transform.position.z);
		}
	}
}
