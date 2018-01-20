using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//used to control the GameObject that represents a combatCharacter in combat sequences
public class CombatEntity : MonoBehaviour {
	public Vector3 restPosition;
	float moveSpeed = 6.0f; // the movment speed in units per second
	GameObject healthBar;
	GameObject healthBarMask;
	Vector3 healthBarMaskInitalScale; // yeah I know it's a long name, but it's very descriptive
	GameObject energyBar;
	GameObject energyBarMask;
	Vector3 energyBarMaskInitalScale;

	public AnimationLoop animLoop;

	void Awake() {		
		Start (); // because instantiating an object doesn't instantly call Start but does call awake

		animLoop = this.gameObject.AddComponent<AnimationLoop> ();
		animLoop.loop = true;
		animLoop.frameRate = 12f;
	}

	void Start () {
		restPosition = transform.position;
		if (healthBar == null) {
			healthBar = Instantiate (Resources.Load<GameObject> ("HealthBar"));
			energyBar = Instantiate (Resources.Load<GameObject> ("EnergyBar"));
			healthBarMask = healthBar.transform.Find ("HealthBarMask").gameObject;
			energyBarMask = energyBar.transform.Find ("EnergyBarMask").gameObject;
			healthBarMaskInitalScale = healthBarMask.transform.localScale;
			energyBarMaskInitalScale = energyBarMask.transform.localScale;
		}
	}
	
	void OnDestroy(){
		Destroy (healthBar);
		Destroy (healthBarMask);
		Destroy (energyBar);
		Destroy (energyBarMask);
	}

	public void setupBars(bool placeRight, bool showEnergy){
		Vector3 pos = restPosition;
		if (placeRight) {
			pos += new Vector3 (2.0f, 0);
		}else{
			pos -= new Vector3(2.0f,0);
		}
		if (showEnergy) {
			healthBar.transform.position = pos + new Vector3 (0, 0.25f);
			energyBar.transform.position = pos - new Vector3 (0, 0.25f);
		} else {
			healthBar.transform.position = pos;
			energyBar.SetActive (false);
		}
	}

	public void setBars(float healthBarScale, float energyBarScale){
		Vector3 newHealthBarScale = healthBarMaskInitalScale * 1; // mulitply by one to force dereference
		newHealthBarScale.x = healthBarMaskInitalScale.x * healthBarScale; 
		healthBarMask.transform.localScale = newHealthBarScale;

		Vector3 newEnergyBarScale = energyBarMaskInitalScale * 1; // muliply by one to force dereference
		newEnergyBarScale.x = energyBarMaskInitalScale.x * energyBarScale;
		energyBarMask.transform.localScale = newEnergyBarScale;

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
