using UnityEngine;
using System.Collections;

public class EdgeGrab : MonoBehaviour {

	public enum EdgeSide{
		Left,
		Right
	}
	public EdgeSide edge;
	public float stageHeight;
	public bool occupied;
	public PlayerControls occupant;

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Bepis");
			PlayerControls playerControls = coll.gameObject.GetComponent<PlayerControls> ();
			if (occupied) {
				PlayerControls.EndEdge (occupant, this.gameObject.GetComponent<EdgeGrab>());
			}

			if (coll.gameObject.transform.position.y < stageHeight + .5 && coll.gameObject.transform.position.y > stageHeight - 1 && !playerControls.wasGrabbing && !occupied) {
				playerControls.attachedEdge = this.gameObject;
				playerControls.edgeGrabbing = true;
				occupant = coll.gameObject.GetComponent<PlayerControls> ();
				playerControls.PlayerControl = false;
				if (edge == EdgeSide.Right) {
					playerControls.facingNum = -1;
				} else if (edge == EdgeSide.Left) {
					playerControls.facingNum = 1;
				}
				playerControls.anim.SetBool ("edgeGrabbing", true);
				occupied = true;
				playerControls.timesJumped = 0;
				Debug.Log ("Edge");

			}
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Bepis");
			PlayerControls playerControls = coll.gameObject.GetComponent<PlayerControls> ();
			if (playerControls.attachedEdge == this.gameObject) {
				playerControls.attachedEdge = null;
				playerControls.wasGrabbing = false;
			}
		}
	}
}
