using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleCamera : MonoBehaviour {

	public Vector3 cameraLocation;
	public List<GameObject> players;
	public Vector2 player1 = Vector2.zero;
	public Vector2 player2 = Vector2.zero;
	public Vector2 player3 = Vector2.zero;
	public Vector2 player4 = Vector2.zero;
	public Camera myCam;
	public float distanceAway;
	public int playerCount;

	// Use this for initialization
	void Start () {
		players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;
		Debug.Log (players.Count + ", ");
		for (int i = players.Count; i < 4; i++) {
			Debug.Log (i);
			players.Add(new GameObject());
		}

		myCam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (players [0] != null) {
			player1 = players [0].transform.position;
		}
		if (players [1] != null) {
			player2 = players [1].transform.position;
		}
		if (players [2] != null) {
			player3 = players [2].transform.position;
		}
		if (players [3] != null) {
			player4 = players [3].transform.position;
		}
		cameraLocation.x = (player1.x+player2.x+player3.x+player4.x)/playerCount;
		cameraLocation.y = (player1.y+player2.y+player3.y+player4.y)/playerCount;
		cameraLocation.z = distanceAway;

		myCam.transform.position = Vector3.Lerp (myCam.transform.position, cameraLocation, 1f);


	}
}
