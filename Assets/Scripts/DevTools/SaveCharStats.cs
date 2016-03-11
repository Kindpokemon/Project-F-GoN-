using UnityEngine;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;

public class SaveCharStats : MonoBehaviour {

	public static CharacterStats playerStats;

	public static void SaveCharacterStats(GameObject player) {
		if (player.GetComponent<PlayerControls> () != null) {
			SaveCharStats.playerStats =  player.GetComponent<PlayerControls> ().characterStats;
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/" + player.name + ".pfgon");
			bf.Serialize (file, SaveCharStats.playerStats);
			file.Close ();
		} else {
			Debug.Log ("Michael! You're and idiot! This doesn't have a CharacterStats!");
		}
	}

	public static void LoadCharacterStats(string playerName, GameObject loadTarget){
		if (File.Exists (Application.persistentDataPath + "/" + playerName + ".pfgon")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + playerName + ".pfgon",FileMode.Open);
			SaveCharStats.playerStats = (CharacterStats)bf.Deserialize (file);
			if (loadTarget.GetComponent<PlayerControls> () != null) {
				loadTarget.GetComponent<PlayerControls> ().characterStats = SaveCharStats.playerStats;
			}
			file.Close();
		} else {
			Debug.Log ("File does not Exist!");
		}
	}
}
