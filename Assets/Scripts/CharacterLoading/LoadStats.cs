using UnityEngine;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;

public static class LoadStats {

	public static List<CharacterStats> playableCharacters = new List<CharacterStats> ();
	public static int characterCount ;
	//public static List<Stage> playableStages = new List<Stage> ();

	public static void LoadCharacters() {
		string[] fileList = Directory.GetFiles (Application.persistentDataPath + "/StreamingAssets/Mods/Characters", "*.txt");
		for (int i = 0; i < fileList.Length; i++) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/StreamingAssets/Mods/Characters/" + fileList [i] + ".fgon", FileMode.Open);
			LoadStats.playableCharacters.Add ((CharacterStats)bf.Deserialize(file));
			file.Close ();
		}
	}

	public static void SaveCharacter(string characterName, int characterNumber){ //Only developers or modders should use this. You need to know what you're doing!
		if (File.Exists (Application.persistentDataPath + "/StreamingAssets/Mods/Characters/" + characterName + ".fgon")) {
			File.Delete (Application.persistentDataPath + "/StreamingAssets/Mods/Characters/" + characterName + ".fgon");
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/StreamingAssets/Mods/Characters/" + characterName + ".fgon");
		bf.Serialize (file, LoadStats.playableCharacters [characterNumber]);
		file.Close();
	}
}
