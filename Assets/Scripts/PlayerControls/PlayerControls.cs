using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

	//Playerstuff
	public GameObject player;
	public Rigidbody2D rbody;
	public CharacterStats characterStats;
	public Vector2 playerPos;
	public Animator anim;
	public AttackAction attackAction;

	//Stagestuff
	public bool gravityEnabled;
	public GameObject[] stageParts;
	public bool isGrounded;

	//Player Stats
	public float walkSpeed;
	public float runsSpeed;
	public float crouchSpeed;
	public float acceleration;
	public int jumps;
	public float jump;
	public float dJump;
	public Vector2 velocity = Vector2.zero;

	///Booleans
	public bool crouch;
	public bool canDJump = false;
	public bool canJump;
	public bool canWalk;
	public bool canCrawl;
	public bool moving;
	public bool PlayerControl;
	public bool attacking;
	public bool canAttack;

	bool sprint;
	bool detectcrouch;
	bool detectjump;
	//bool bigJump;
	float getHor;
	float getVer;

	///Numbers 
	public float crouchNum;
	public Vector2 movement_vector;
	public int timesJumped;
	public float gravity;
	public bool jumping;
	public int facingNum;

	void Start(){
		rbody = this.GetComponent<Rigidbody2D> ();
		attackAction = this.GetComponent<AttackAction> ();
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
		canAttack = true;
	}

	void Update(){
		//Taunts//
		if (!attacking) {
			if (Input.GetButtonDown ("UpTaunt") && isGrounded) {
				StartCoroutine (attackAction.Taunt ("up"));
			}
			if (Input.GetButtonDown ("SideTaunt") && isGrounded) {
				StartCoroutine (attackAction.Taunt ("side"));
			}
			if (Input.GetButtonDown ("DownTaunt") && isGrounded) {
				StartCoroutine (attackAction.Taunt ("down"));
			}
		}
		//Controls//
		detectcrouch = Input.GetButtonDown("Crouch");
		sprint = Input.GetButtonDown ("Sprint");
		detectjump = Input.GetButtonDown ("Jump");
		movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
		getHor = Input.GetAxisRaw ("Horizontal");
		getVer = Input.GetAxisRaw ("Vertical");
		crouchNum = getVer;

		//Physics Calc

		if (crouch) {
			canWalk = false;

			canJump = false;
		} else {
			canWalk = true;

			canJump = true;
		}

		if (crouch && isGrounded && !attacking) {
			crouch = true;
			anim.SetBool ("isCrouching", true);

		} else {
			crouch = false;
			anim.SetBool ("isCrouching", false);
		}

		if (canDJump == true && (detectjump) && PlayerControl && canJump) {

			velocity.y = dJump/2;
			timesJumped++;
			if (timesJumped >= jumps) {
				canDJump = false;
			}
		}

		if (crouchNum == -1) {
			crouch = true;
		} else {
			crouch = false;
		}

		if (movement_vector != Vector2.zero && !attacking) {
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool ("isWalking", false);
		}
		if (!attacking) {
			if (getHor != 0 || getVer != 0) {
				if (getHor == 1) {
					facingNum = 1;
				} else if (getHor == -1) {
					facingNum = -1;
				}
				moving = true;
			} else {
				moving = false;
			}
		}

		if (facingNum == -1) {
			GetComponent<SpriteRenderer> ().flipX = false;
		} else if (facingNum == 1) {
			GetComponent<SpriteRenderer> ().flipX = true;
		}
	}

	void FixedUpdate(){
		
		//Horizontal Movement//
		playerPos.x = rbody.position.x;

		// Pass all parameters to the character control script.
		if (sprint && PlayerControl && canWalk && !crouch && isGrounded && !attacking) {
			rbody.MovePosition (rbody.position + (movement_vector * runsSpeed * (Time.deltaTime / 2)));
			anim.SetBool ("isRunning", true);
			anim.SetBool ("isCrawling", false);
			Debug.Log ("Sprint");
		} else if (crouch && PlayerControl && canCrawl && crouch && isGrounded && !attacking) {
			anim.SetBool ("isCrawling", true);
			anim.SetBool ("isRunning", false);
			Debug.Log ("Crouch");
			//rbody.MovePosition (rbody.position + (movement_vector * crouchSpeed * (Time.deltaTime / 2)));
		} else if (PlayerControl && canWalk && !crouch && isGrounded && !attacking) {
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isCrawling", false);
			rbody.MovePosition (rbody.position + (movement_vector * walkSpeed * (Time.deltaTime / 2)));
		} else if (!canCrawl && isGrounded && crouch) {
			rbody.MovePosition (new Vector2 (rbody.position.x, rbody.position.y + (movement_vector.y * walkSpeed * (Time.deltaTime / 2F))));
		} else if(attacking){
			rbody.MovePosition (new Vector2 (rbody.position.x, rbody.position.y + (movement_vector.y * walkSpeed * (Time.deltaTime / 2F))));
		} else {
			rbody.MovePosition (rbody.position + (movement_vector * walkSpeed * (Time.deltaTime / 2F)));
		}

		if (isGrounded) {
			velocity = new Vector3 (0, 0, 0);
			velocity = transform.TransformDirection (velocity);
			//velocity *= jump;
			anim.SetBool ("IsJumping", false);

			if (detectjump && canJump && PlayerControl) {
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
		
		Collider2D collider = coll.collider;
		bool top = false;
		Debug.Log (coll.gameObject + ""+ collider.name);

		if (collider.tag == "Stage") {
			float angle = Vector2.Angle(collider coll.transform.position);
			if(Mathf.Approximately(angle, 0))// back
			if(Mathf.Approximately(angle, 180))// front
			if(Mathf.Approximately(angle, 90)){
		}

		if(coll.gameObject.tag == "Stage" && !jumping && top){
			isGrounded = true;
			anim.SetBool ("grounded", true);
			timesJumped = 1;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if(coll.gameObject.tag == "Stage"){
			isGrounded = false;
			anim.SetBool ("grounded", false);
			canDJump = true;
		}
	}
}
