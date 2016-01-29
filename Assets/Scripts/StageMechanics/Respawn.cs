using UnityEngine;
using System.Collections;

public class Respawn:MonoBehaviour {

	public Vector3 respawnspot;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PlayerControls> ().velocity = new Vector3 ();
			coll.gameObject.transform.position = respawnspot;
		} else if (coll.gameObject.tag == "Crate" || coll.gameObject.tag == "Weapon" || coll.gameObject.tag == "Useable") {
			GameObject.Destroy (coll.gameObject);
		}
	}
}
