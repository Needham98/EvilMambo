using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public string playerName = "Herobrine";
	public int health = 100;
	public int strength = 1;
	public int defense = 1;
//TODO
	public string statModifier = "health"; //A name of a stat modifier for the char
	//public <> skin;

	// Use this for initialization
	protected void Start () {
		LoadPlayer ();
	}
	
	
	// Update is called once per frame
	private void Update () {
		//if (GameManager.instance.playersTurn == false)
		//	return;
		/*
		int horizontal = 0;  	//Used to store the horizontal move direction.
		int vertical = 0;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		vertical = (int) (Input.GetAxisRaw ("Vertical"));

		tryMove (horizontal, vertical);*/
	}

//TODO;
	private void tryMove(int x, int y){
	}
	///<summary>
	///Gets inbetween level values for player
	///</summary>
	private void LoadPlayer(){
		playerName = GameManager.instance.stats.playerName;
		health = GameManager.instance.stats.healthPoints;
		strength = GameManager.instance.stats.strengthPoints;
		defense = GameManager.instance.stats.defensePoints;
		statModifier = GameManager.instance.stats.statModifierVar;
	}

	private void onDisable(){
		SavePlayer ();
	}

	///<summary>
	///Saves inbetween level values for player
	///</summary>
	private void SavePlayer(){
		GameManager.instance.stats.playerName = playerName;
		GameManager.instance.stats.healthPoints = health;
		GameManager.instance.stats.strengthPoints = strength;
		GameManager.instance.stats.defensePoints = defense;
		GameManager.instance.stats.statModifierVar = statModifier;
	}

	private void PlayerDead(){
		if (health <= 0){
			GameManager.instance.GameOver(false);
		}
	}
}
