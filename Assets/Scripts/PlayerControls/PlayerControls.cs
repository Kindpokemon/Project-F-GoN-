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
	Animator anim;
	public bool PlayerControl;
	public bool canJump;
	public bool canWalk;
	public bool moving;
	public int facingNum;
	public bool canCrawl;

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
		anim = GetComponent<Animator> ();
		//Enable movement (for debug purposes)
		gravityEnabled = true;
		velocity = new Vector2 ();
		PlayerControl = true;
		canWalk = true;
		canJump = true;
	}


	void Update(){

		// Read the inputs.
		crouch = Input.GetKey(KeyCode.LeftControl);
		movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
		if (movement_vector != Vector2.zero) {
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool ("isWalking", false);
		}

		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {
			if (Input.GetAxisRaw ("Horizontal") == 1) {
				facingNum = 1;
			} else if (Input.GetAxisRaw ("Horizontal") == -1) {
				facingNum = -1;
			}
			moving = true;
		} else {
			moving = false;
		}

		if (facingNum == -1) {
			GetComponent<SpriteRenderer> ().flipX = false;
		} else if (facingNum == 1) {
			GetComponent<SpriteRenderer> ().flipX = true;
		}

		// Pass all parameters to the character control script.
		if (Input.GetKey (KeyCode.LeftShift) && PlayerControl && canWalk && !crouch && isGrounded) {
			rbody.MovePosition (rbody.position + (movement_vector * runsSpeed * (Time.deltaTime / 2)));
			anim.SetBool ("isRunning", true);
			anim.SetBool ("isCrawling", false);
		} else if (Input.GetKey (KeyCode.LeftControl) && PlayerControl && canWalk && canCrawl && crouch && isGrounded) {
			anim.SetBool ("isCrawling", true);
			anim.SetBool ("isRunning", false);
			rbody.MovePosition (rbody.position + (movement_vector * crouchSpeed * (Time.deltaTime / 2)));
		} else if (PlayerControl && canWalk && !crouch) {
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isCrawling", false);
			rbody.MovePosition (rbody.position + (movement_vector * walkSpeed * (Time.deltaTime / 2)));
		}

		if (Input.GetKey (KeyCode.LeftControl) && isGrounded) {
			crouch = true;
			anim.SetBool ("isCrouching", true);

		} else {
			crouch = false;
			anim.SetBool ("isCrouching", false);
		}

		if (canDJump == true && Input.GetKeyDown (KeyCode.Space) && PlayerControl && canJump) {

			velocity.y = dJump/2;
			timesJumped++;
			if (timesJumped >= jumps) {
				canDJump = false;
			}
		}

		if (isGrounded) {
			velocity = new Vector3 (0, 0, 0);
			velocity = transform.TransformDirection (velocity);
			velocity *= jump;
			anim.SetBool ("IsJumping", false);

			if (Input.GetKeyDown (KeyCode.Space) && canJump && PlayerControl) {
				velocity.y = jump / 2;//*(Time.deltaTime);
				canDJump = true;
				//timesJumped++;
			}
		} else {
			anim.SetBool ("IsJumping", true);
		}
		if (gravityEnabled && !isGrounded) {
			velocity.y -= (gravity / 1F) * Time.deltaTime;
		}
		rbody.AddForce (velocity*Time.deltaTime*50000);

	}
		
	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage" && !jumping){
			isGrounded = true;
			timesJumped = 1;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage"){
			isGrounded = false;
			canDJump = true;
		}
	}
}
