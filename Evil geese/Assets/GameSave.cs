using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
// used to save a gameState
public static class GameSave {
	static string savePath = Application.dataPath + "/" ; // root path for saves
	const string saveExtension = ".geese";

	public static void saveState(string saveName, GameState state)
	{	
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream saveFile = File.Create(savePath + saveName + saveExtension);
		bf.Serialize (saveFile, state);
		saveFile.Close();
	}

	public static GameState loadState(string saveName){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream loadFile = File.OpenRead (savePath + saveName + saveExtension);
		GameState state = (GameState)bf.Deserialize (loadFile);
		loadFile.Close ();
		return state;
	}
}
