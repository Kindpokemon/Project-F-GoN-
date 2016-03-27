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
	public string normalName;
	public string specialName;
	public string horName;
	public string verName;

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
		Shield,
		GravityProjectile
	}

	void Start(){
		playerControls = gameObject.GetComponent<PlayerControls> ();
		charStats = playerControls.characterStats;
		normalName = playerControls.controllerType+"Normal";
		specialName = playerControls.controllerType+"Special";
		horName = playerControls.controllerType+"Horizontal";
		verName = playerControls.controllerType+"Vertical";
	}

	void Update(){

		if (!attacking) { //Heldstuffs
			if (Input.GetButton (normalName) && !stopClockA) {
				timeHeld += Time.deltaTime;
			} else {
				timeHeld = 0;
			}
			if (Input.GetButton (specialName) && !stopClockB) {
				specialHeld += Time.deltaTime;
			} else {
				specialHeld = 0;
			}

			if (timeHeld >= .25F && !stopClockA) { //Held A
				if (Input.GetAxisRaw (horName) == 1 || Input.GetAxisRaw (horName) == -1) {
					if (playerControls.characterStats.moveSet [3].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [3], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				} else if (Input.GetAxisRaw (verName) == 1) {

				} else if (Input.GetAxisRaw (verName) == -1) {
					if (playerControls.characterStats.moveSet [5].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [5], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				} else if(Input.GetAxisRaw (horName) == 0 && Input.GetAxisRaw (verName) == 0){
					if (playerControls.characterStats.moveSet [1].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [1], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				}
			}

			if (Input.GetButtonUp (normalName) && timeHeld < .25F) {
				if (Input.GetAxisRaw (horName) == 1 || Input.GetAxisRaw (horName) == -1) { //Side
					if (playerControls.characterStats.moveSet [2].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [2], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				} else if (Input.GetAxisRaw (verName) == 1) { //Up
					if (playerControls.characterStats.moveSet [6].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [6], normalName));
						stopClockA = false;
						timeHeld = 0;
					}	
				} else if (Input.GetAxisRaw (verName) == -1) { //Down
					if (playerControls.characterStats.moveSet [4].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [4], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				} else if(Input.GetAxisRaw (horName) == 0 && Input.GetAxisRaw (verName) == 0){
					if (playerControls.characterStats.moveSet [0].attackType == AttackTypes.Summon && playerControls.isGrounded) {
						StartCoroutine (Summon (playerControls.characterStats.moveSet [0], normalName));
						stopClockA = false;
						timeHeld = 0;
					}
				}

			}
			if (!playerControls.isGrounded && Input.GetButtonDown(normalName)) {
				if ((Input.GetAxisRaw (verName) == 1 && playerControls.facingNum == 1) || (Input.GetAxisRaw (verName) == -1 && playerControls.facingNum == -1)) { //fair

				} else if ((Input.GetAxisRaw (verName) == -1 && playerControls.facingNum == 1) || (Input.GetAxisRaw (verName) == 1 && playerControls.facingNum == -1)) { //Bair

				} else if (Input.GetAxisRaw (verName) == 1) { //Uair

				} else if (Input.GetAxisRaw (verName) == -1) { //Dair

				} else if(Input.GetAxisRaw (horName) == 0 && Input.GetAxisRaw (verName) == 0){ //Nair

				}
			}
			if (specialHeld >= .25F && !stopClockB) {
				if (Input.GetAxisRaw (horName) == 1 || Input.GetAxisRaw (horName) == -1) {

				} else if (Input.GetAxisRaw (verName) == 1) {

				} else if (Input.GetAxisRaw (verName) == -1) {

				} else if(Input.GetAxisRaw (horName) == 0 && Input.GetAxisRaw (verName) == 0){

				}
			}
			if (Input.GetButtonUp (specialName) && specialHeld < .25F) { //Non-Charging
				if (Input.GetAxisRaw (horName) == 1 || Input.GetAxisRaw (horName) == -1) { //Side
					
				} else if (Input.GetAxisRaw (verName) == 1) { //Up

				} else if (Input.GetAxisRaw (verName) == -1) { //Down

				} else if(Input.GetAxisRaw (horName) == 0 && Input.GetAxisRaw (verName) == 0){
					Debug.Log ("Gothor");
					if (playerControls.characterStats.moveSet [8].attackType == AttackTypes.Beam) {
						StartCoroutine (Beam (playerControls.characterStats.moveSet [8], specialName));
						Debug.Log ("Corut");
						stopClockB = false;
						specialHeld = 0;
						Debug.Log ("false0");
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
		int objectCount = 0;
		if (attack.chargeSummon) {
			objectCount = attack.summonedAtFirst;
		} else {
			objectCount = attack.AttackObjects.Length;
		}

		if (attack.summonType == Attack.SummonType.AtOnce) {
			for (int i = 0; i < objectCount; i++) {
				GameObject g = null;
				if (attack.randomSummon) {
					g = (GameObject)Instantiate (attack.AttackObjects [Random.Range(0,attack.AttackObjects.Length-1)]);
				} else {
					g = (GameObject)Instantiate (attack.AttackObjects [i]);
				}
				Animator a = g.transform.GetChild(0).GetComponent<Animator> ();
				objects.Add (g);
				anims.Add (a);
				g.transform.SetParent (playerControls.gameObject.transform);
				g.transform.localPosition = new Vector2 (attack.positions[i].x, attack.positions[i].y);
				if (attack.flipped[i] == true) {
					Vector3 thePos = g.transform.localPosition;
					if (thePos == Vector3.zero) {
						Vector3 theScale = g.transform.localScale;
						theScale.x *= -1;
						g.transform.localScale = theScale;
					} else {
						thePos.x *= -1;
						g.transform.localPosition = thePos;
					}
				}
				if (playerControls.facingNum == 1) {
					Vector3 thePos = g.transform.localPosition;
					if (thePos == Vector3.zero) {
						Vector3 theScale = g.transform.localScale;
						theScale.x *= -1;
						g.transform.localScale = theScale;
					} else {
						thePos.x *= -1;
						g.transform.localPosition = thePos;
					}
				}
				a.SetTrigger ("Move");
			}
		} else if (attack.summonType == Attack.SummonType.Consecutive) {
			GameObject g = (GameObject)Instantiate (attack.AttackObjects [0]);
			Animator a = g.transform.GetChild(0).GetComponent<Animator> ();
		}
		//Animation Summon
		if (attack.summonAnim != "") {
			playerControls.anim.Play (attack.summonAnim);
		}
		yield return new WaitForSeconds (attack.betweenWaits [0]);
		//Infinite holding
		if (attack.infiniteHold) {
			for (int l = 0; l < anims.Count; l++) {
				anims [l].SetTrigger ("InfiniteHold");
			}
			while (Input.GetButton (keyType)) {
				yield return new WaitForSeconds (attack.betweenWaits[2]);
			}
		} else if (attack.chargetime > 0) { //Limited Holding
			for (int l = 0; l < anims.Count; l++) {
				anims [l].SetBool ("Charging",true);
			}
			if (attack.chargingAnim != "") {
				playerControls.anim.Play (attack.chargingAnim);
			}
			int summonNumber = 0;
			if(attack.chargeSummon) {
				summonNumber = attack.summonedAtFirst;
			}
			while (Input.GetButton (keyType) && attack.currentCharge < attack.chargetime) {
				yield return new WaitForSeconds (attack.betweenWaits[2]);
				if (attack.currentCharge < attack.chargetime) {
					attack.currentCharge++;
				}
				if (attack.chargeSummon && summonNumber < attack.AttackObjects.Length-1) {
					for (int i = 0; i < attack.chargeSCount; i++) {
						GameObject g = (GameObject)Instantiate (attack.AttackObjects [summonNumber]);
						Animator a = g.transform.GetChild (0).GetComponent<Animator> ();
						objects.Add (g);
						anims.Add (a);
						g.transform.SetParent (playerControls.gameObject.transform);
						g.transform.localPosition = new Vector2 (attack.positions [summonNumber].x, attack.positions [summonNumber].y);
						if (attack.flipped [summonNumber] == true) {
							Vector3 thePos = g.transform.localPosition;
							if (thePos == Vector3.zero) {
								Vector3 theScale = g.transform.localScale;
								theScale.x *= -1;
								g.transform.localScale = theScale;
							} else {
								thePos.x *= -1;
								g.transform.localPosition = thePos;
							}
						}
						if (playerControls.facingNum == 1) {
							Vector3 thePos = g.transform.localPosition;
							if (thePos == Vector3.zero) {
								Vector3 theScale = g.transform.localScale;
								theScale.x *= -1;
								g.transform.localScale = theScale;
							} else {
								thePos.x *= -1;
								g.transform.localPosition = thePos;
							}
						}
						a.SetTrigger ("Move");
						summonNumber++;
					}
				}
			}
			for (int l = 0; l < anims.Count; l++) {
				anims [l].SetBool ("Charging",false);
			}
		}
		//Fire
		if (attack.attackAnim != "") {
			playerControls.anim.Play (attack.attackAnim);
		}
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
		attack.currentCharge = 0;
		attack.currentPresses = 0;
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
		bool animating = true;
		yield return new WaitForSeconds (attack.betweenWaits [0]);
		//Chargingshit
		if (!playerControls.isGrounded) {
			//a.SetTrigger ("Charge");
			playerControls.anim.Play (attack.chargingAnim);
			if (attack.minDamage < attack.maxDamage) {
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
		while (animating) {
			if (playerControls.anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !playerControls.anim.IsInTransition (0)) {
				animating = false;
			}
		}
		yield return new WaitForSeconds (attack.betweenWaits[1]);
		//End Attack
		playerControls.anim.SetTrigger ("AttackEnd");
		g.transform.SetParent (null);
		//Destroy Beamthing
		yield return new WaitForSeconds (attack.betweenWaits [2]);
		Destroy (g);
		//Enable attacking
		attacking = false;
		playerControls.PlayerControl = true;
		playerControls.attacking = false;
		yield break;
	}
}