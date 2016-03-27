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
	public AnimCombat animCombat;

	//Stagestuff
	public bool gravityEnabled;
	public GameObject[] stageParts;
	public bool isGrounded;
	public GameObject attachedEdge;
	public Vector2 edgePosition;

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
	public bool edgeGrabbing;
	public bool canBeHit;
	public bool wasGrabbing;

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

	public enum Controller {
		j1,
		j2,
		j3,
		j4,
		k
	}
	public Controller controllerType;

	void Start(){
		rbody = this.GetComponent<Rigidbody2D> ();
		animCombat = this.GetComponent<AnimCombat> ();
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
			if (Input.GetButtonDown (controllerType+"Taunt") && isGrounded) {
				StartCoroutine (animCombat.Taunt ("down"));
			}
		}
		//Controls//
		if (PlayerControl) {
			detectjump = Input.GetButtonDown (controllerType+"Jump");
			movement_vector = new Vector2 (Input.GetAxisRaw (controllerType+"Horizontal"), 0);
			getHor = Input.GetAxisRaw (controllerType+"Horizontal");
			getVer = Input.GetAxisRaw (controllerType+"Vertical");
			if (getVer < 0) {
				detectcrouch = true;
			} else {
				detectcrouch = false;
			}
		} else {
			detectcrouch = false;
			detectjump = false;
			movement_vector = Vector2.zero;
			getHor = 0;
			getVer = 0;
		}
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
		if (!attacking && !edgeGrabbing && PlayerControl) {
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

		EdgeHandle (attachedEdge);
	}

	void EdgeHandle(GameObject edge){
		if (edgeGrabbing) {
			transform.SetParent (attachedEdge.transform);
			transform.localPosition = edgePosition;
			canBeHit = false;
			isGrounded = true;
			wasGrabbing = true;
			GetComponent<BoxCollider2D> ().enabled = false;
			if (detectjump) {
				EndEdge (this.GetComponent<PlayerControls> (), edge.GetComponent<EdgeGrab>());
				velocity.y = dJump/2;
				timesJumped++;
			}
		}
	}

	public static void EndEdge(PlayerControls playa , EdgeGrab edge){
		playa.attachedEdge.GetComponent<EdgeGrab> ().occupied = false;
		playa.edgeGrabbing = false;
		edge.occupant = null;
		playa.PlayerControl = true;
		playa.isGrounded = false;
		playa.anim.SetBool ("edgeGrabbing", false);
		playa.GetComponent<BoxCollider2D> ().enabled = true;
		playa.transform.SetParent (null);
		playa.canBeHit = true;
	}

	void FixedUpdate(){
		//Horizontal Movement//
		playerPos.x = rbody.position.x;

		// Pass all parameters to the character control script.
		if (crouch && PlayerControl && canCrawl && crouch && isGrounded && !attacking) {
			anim.SetBool ("isCrawling", true);
			anim.SetBool ("isRunning", false);
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
		
	void OnCollisionStay2D(Collision2D coll){
		
		if (coll.gameObject.tag == "Stage") {
			float stageHeight = coll.gameObject.GetComponent<StageMechanics> ().stageFloor;
			if (coll.contacts [0].point.y >= stageHeight) {
				timesJumped = 0;
				anim.SetBool ("grounded", true);
				isGrounded = true;
			} else {
				anim.SetBool ("grounded", false);
				isGrounded = false;
			}
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
