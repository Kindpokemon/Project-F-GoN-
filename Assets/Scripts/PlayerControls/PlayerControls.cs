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
	public int jumps;
	public int timesJumped;
	public bool gravityEnabled;
	public CharacterStats characterStats;

	void Start(){
		rbody = this.GetComponent<Rigidbody2D> ();
		//Transfer stats to character
		walkSpeed = characterStats.walkSpeed;
		runsSpeed = characterStats.runSpeed;
		crouchSpeed = characterStats.crouchSpeed;
		acceleration = characterStats.acceleration;
		jump = characterStats.jumpHeight;
		dJump = characterStats.dJumpHeight;
		gravity = characterStats.gravity;
		jumps = characterStats.jumps;
		//Enable movement (for debug purposes)
		gravityEnabled = true;
		velocity = new Vector2 ();
	}


	void Update(){

		// Read the inputs.
		crouch = Input.GetKey(KeyCode.LeftControl);
		movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
		if (movement_vector != Vector2.zero) {

		}

		// Pass all parameters to the character control script.
		if(Input.GetKey(KeyCode.LeftShift)){
			rbody.MovePosition (rbody.position + (movement_vector * runsSpeed*(Time.deltaTime/2)));
		} else {
			rbody.MovePosition (rbody.position + (movement_vector * walkSpeed*(Time.deltaTime/2)));
		}

		if (canDJump == true && Input.GetKeyDown (KeyCode.Space)) {

			velocity.y = dJump/2;
			timesJumped++;
			if (timesJumped >= jumps) {
				canDJump = false;
			}
		}

		if (isGrounded) {
			velocity = new Vector3 (0, 0, 0);
			velocity = transform.TransformDirection(velocity);
			velocity *= jump;

			if (Input.GetKeyDown (KeyCode.Space)) {
				velocity.y = jump/2;//*(Time.deltaTime);
				canDJump = true;
				timesJumped++;
			}
		}
		if (gravityEnabled) {
			velocity.y -= (gravity / 1F) * Time.deltaTime;
		}
		rbody.AddForce (velocity*Time.deltaTime*50000);

	}
		
	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage" && !jumping){
			isGrounded = true;
			timesJumped = 0;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage"){
			isGrounded = false;
			canDJump = true;
		}
	}
}
