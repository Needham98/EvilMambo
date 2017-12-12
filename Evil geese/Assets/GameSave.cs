using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameSave {
	const string savePath = "/saves/"; // root path for saves

	public static void saveState(string saveName, GameState state)
	{	
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream saveFile = File.Create(savePath + saveName);
		bf.Serialize (saveFile, state);
		saveFile.Close();
	}

	public static GameState loadState(string saveName){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream loadFile = File.OpenRead (savePath + saveName);
		GameState state = (GameState)bf.Deserialize (loadFile);
		loadFile.Close ();
		return state;
	}
}
