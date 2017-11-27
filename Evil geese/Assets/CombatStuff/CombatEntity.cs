using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : MonoBehaviour {
	public Vector3 restPosition;
	float moveSpeed = 6.0f; // the movment speed in units per second

	// Use this for initialization
	void Start () {
		restPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool moveAttack(){ // moves parent towards the attack position, returns true when at attack position
		if (Mathf.Abs (transform.position.x) <= moveSpeed * Time.deltaTime) {
			transform.position = new Vector3 (0.0f, transform.position.y, transform.position.z);
			return true;
		}
		float change = -Mathf.Sign (transform.position.x) * moveSpeed * Time.deltaTime;
		transform.position += new Vector3 (change, 0.0f);
		return false;

	}

	public bool moveRest(){ // moves parent towards the rest position, returns true when at rest position
		if ((transform.position - restPosition).magnitude <= moveSpeed * Time.deltaTime) {
			transform.position = restPosition;
			return true;
		}
		transform.position = Vector3.MoveTowards (transform.position, restPosition, moveSpeed * Time.deltaTime);
		return false;

	}
}
