using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

	public GameObject player;
	public bool crouch;
	public float walkSpeed;
	public float runsSpeed;
	public float crouchSpeed;
	public float acceleration;
	public GameObject stage;
	public Vector2 movement_vector;
	public Rigidbody2D rbody;
	public float jump;
	public float dJump;
	public float gravity;
	public bool canDJump = false;
	public Vector2 velocity = Vector3.zero;
	public bool isGrounded = false;
	public bool jumping;

	void Start(){
		rbody = this.GetComponent<Rigidbody2D> ();
	}


	void Update(){

		// Read the inputs.
		crouch = Input.GetKey(KeyCode.LeftControl);
		movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
		if (movement_vector != Vector2.zero) {

		}

		// Pass all parameters to the character control script.
		if(Input.GetKey(KeyCode.LeftShift)){
			rbody.MovePosition (rbody.position + movement_vector * runsSpeed * Time.deltaTime/2);
		} else {
			rbody.MovePosition (rbody.position + movement_vector * walkSpeed * Time.deltaTime/2);
		}

		if (canDJump == true && Input.GetKeyDown (KeyCode.Space)) {

			velocity.y = dJump;
			canDJump = false;

		}

		if (isGrounded) {
			velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, 0);
			velocity = transform.TransformDirection(velocity);
			velocity *= runsSpeed;

			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("Hopped");
				velocity.y = jump;
				canDJump = true;
			}
		}

		velocity.y -= gravity * Time.deltaTime;
		rbody.AddForce (velocity*Time.deltaTime);

	}
		
	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage" && !jumping){
			isGrounded = true;
		} else if(!jumping) {
			isGrounded = false;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage"){
			isGrounded = false;
		}
	}
}
