using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimCombat : MonoBehaviour {

	public PlayerControls playerControls;
	public CharacterStats charStats;
	public bool attacking;
	public int comboNum;
	public float timeHeld;
	public float specialHeld;
	public float punchtime;
	public bool stopClockA;
	public bool stopClockB;


	public enum AttackTypes {
		//Neutrals
		//Downs
		//Ups
		//Side
		//Generic
		Punch,
		Summon,
		Beam,
		Laser,
		Teleport,
		Shield
	}

	void Start(){
		playerControls = gameObject.GetComponent<PlayerControls> ();
		charStats = playerControls.characterStats;
	}

	void Update(){

		if (!attacking) { //Heldstuffs
			if (Input.GetButton ("Normal") && !stopClockA) {
				timeHeld += Time.deltaTime;
			} else {
				timeHeld = 0;
			}
			if (Input.GetButton ("Special") && !stopClockB) {
				specialHeld += Time.deltaTime;
			} else {
				specialHeld = 0;
			}

			if (timeHeld >= .25F && !stopClockA) { //Held A
				if (Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1) {

				} else if (Input.GetAxisRaw ("Vertical") == 1) {

				} else if (Input.GetAxisRaw ("Vertical") == -1) {
					
				} else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0){

				}
			}

			if (Input.GetButtonUp ("Normal") && timeHeld < .25F) {
				Debug.Log("D");
				if (Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1) { //Side

				} else if (Input.GetAxisRaw ("Vertical") == 1) { //Up
						
				} else if (Input.GetAxisRaw ("Vertical") == -1) { //Down
					Debug.Log("O");
					if (playerControls.characterStats.moveSet [4].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						Debug.Log("T");
						StartCoroutine (Summon (playerControls.characterStats.moveSet [4], "Normal"));
						stopClockA = false;
						timeHeld = 0;
					}
				} else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0){
					
				}

			}
			if (specialHeld >= .25F && !stopClockB) {
				if (Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1) {

				} else if (Input.GetAxisRaw ("Vertical") == 1) {

				} else if (Input.GetAxisRaw ("Vertical") == -1) {

				} else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0){

				}
			}
			if (Input.GetButtonDown ("Special") && specialHeld < .25F) { //Non-Charging
				Debug.Log ("Got Das Booton");
				if (Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1) { //Side
					
				} else if (Input.GetAxisRaw ("Vertical") == 1) { //Up

				} else if (Input.GetAxisRaw ("Vertical") == -1) { //Down

				} else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0){
					if (playerControls.characterStats.moveSet [8].attackType == AttackTypes.Beam && playerControls.isGrounded) {
						StartCoroutine (Beam (playerControls.characterStats.moveSet [8], "Special"));
						stopClockB = false;
						specialHeld = 0;
					}
				}
			}
		}
	}

	public IEnumerator Taunt(string tauntName){
		playerControls.PlayerControl = false;
		if (tauntName == "up") {
			playerControls.anim.SetTrigger ("upTaunt");

		} else if (tauntName == "side") {
			playerControls.anim.SetTrigger ("sideTaunt");
		} else if (tauntName == "down") {
			playerControls.anim.SetTrigger ("downTaunt");
		}
		playerControls.canAttack = false;
		playerControls.attacking = true;
		attacking = true;
		yield return new WaitForSeconds (1.5F);
		playerControls.canAttack = true;
		playerControls.attacking = false;
		playerControls.PlayerControl = true;
		attacking = false;
	}

	//////Attacks

	public IEnumerator Punch(Attack attack, string keyType){
		//Disable Control
		playerControls.PlayerControl = false;
		playerControls.attacking = true;
		attacking = true;
		//Enable attacking
		attacking = false;
		playerControls.PlayerControl = true;
		playerControls.attacking = false;
		yield break;
	}

	public IEnumerator Summon(Attack attack, string keyType){
		//Disable Control
		playerControls.PlayerControl = false;
		playerControls.attacking = true;
		attacking = true;
		//Summon Object(s) and Orientate
		List<GameObject> objects = new List<GameObject>();
		List<Animator> anims = new List<Animator> ();
		if (attack.summonType == Attack.SummonType.AtOnce) {
			for (int i = 0; i < attack.AttackObjects.Length; i++) {
				GameObject g = (GameObject)Instantiate (attack.AttackObjects [i]);
				Animator a = g.transform.GetChild(0).GetComponent<Animator> ();
				objects.Add (g);
				anims.Add (a);
				g.transform.SetParent (playerControls.gameObject.transform);
				g.transform.localPosition = new Vector2 (attack.positions[0].x, attack.positions[0].y);
				if (attack.flipped[i] == true) {
					Vector3 theScale = g.transform.localScale;
					theScale.x *= -1;
					g.transform.localScale = theScale;
				}
				if (playerControls.facingNum == 1) {
					Vector3 theScale = g.transform.localScale;
					theScale.x *= -1;
					g.transform.localScale = theScale;
				}
				a.SetTrigger ("Move");
			}
		} else if (attack.summonType == Attack.SummonType.Consecutive) {
			GameObject g = (GameObject)Instantiate (attack.AttackObjects [0]);
			Animator a = g.transform.GetChild(0).GetComponent<Animator> ();
		}
		//Animation Summon
		playerControls.anim.Play (attack.summonAnim);
		yield return new WaitForSeconds (attack.betweenWaits [0]);
		//Fire
		playerControls.anim.Play (attack.attackAnim);
		for (int j = 0; j < anims.Count; j++) {
			anims [j].SetTrigger ("Fire");
		}
		yield return new WaitForSeconds (attack.betweenWaits [1]);
		int req = anims.Count;
		for (int k = 0; k < req; k++) {
			Destroy (objects [k]);
		}
		playerControls.anim.SetTrigger ("AttackEnd");
		//Enable attacking
		attacking = false;
		playerControls.PlayerControl = true;
		playerControls.attacking = false;
		yield break;
	}

	public IEnumerator Beam(Attack attack, string keyType){
		//Disable Control
		playerControls.PlayerControl = false;
		playerControls.attacking = true;
		attacking = true;
		//Summon Object and Orientate
		GameObject g = (GameObject)Instantiate (attack.AttackObjects [0]);
		Animator a = g.transform.GetChild(0).GetComponent<Animator> ();
		g.transform.SetParent (playerControls.gameObject.transform);
		g.transform.localPosition = new Vector2 (attack.positions[0].x, attack.positions[0].y);
		if (playerControls.facingNum == 1) {
			Vector3 theScale = g.transform.localScale;
			theScale.x *= -1;
			g.transform.localScale = theScale;
		}
		//Summon Animation
		playerControls.anim.Play (attack.summonAnim);
		a.SetTrigger ("Move");
		yield return new WaitForSeconds (attack.betweenWaits [0]);
		//Chargingshit
		if (attack.chargetime > 0) {
			a.SetTrigger ("Charge");
			playerControls.anim.Play (attack.chargingAnim);
			if (attack.minDamage == attack.maxDamage) {
				while (Input.GetButton (keyType)) {
					if (attack.currentPresses < attack.maxPresses) {
						attack.currentPresses++;
					}
					yield return new WaitForSeconds (.5f);
				}
			}
		}
		//Fire after charging
		playerControls.anim.Play (attack.attackAnim);
		a.SetTrigger ("Fire");
		bool animating = true;
		while (animating) {
			if (playerControls.anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !playerControls.anim.IsInTransition (0)) {
				animating = false;
			}
		}
		yield return new WaitForSeconds (.5f);
		//End Attack
		playerControls.anim.SetTrigger ("AttackEnd");
		g.transform.SetParent (null);
		//Destroy Beamthing
		yield return new WaitForSeconds (attack.betweenWaits [1]);
		Destroy (g);
		//Enable attacking
		attacking = false;
		playerControls.PlayerControl = true;
		playerControls.attacking = false;
		yield break;
	}
}