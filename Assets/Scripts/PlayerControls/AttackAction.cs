﻿using UnityEngine;
using System.Collections;

public class AttackAction : MonoBehaviour {

	public PlayerControls playerControls;
	public CharacterStats charStats;
	public bool attacking;
	public int comboNum;
	public float timeHeld;
	public float punchtime;
	public bool stopClockA;
	public bool stopClockB;


	public enum AttackTypes {
		//Neutrals
		ComboPunch,
		ComboSummon,
		ChargeUp,
		//Downs
		CrouchSwipe,
		CrouchSummon,
		Teleport,
		//Sides
		Grab,

		//Grabstuff
		Fthrow,
		Bthrow,
		Uthrow,
		Dstomp,
		Pummel,

		//General
		WeaponSummon,
		Spike

	}

	public AttackTypes attackTypes;

	void Start(){
		playerControls = gameObject.GetComponent<PlayerControls> ();
		charStats = playerControls.characterStats;
	}
	////////////////////Update

	void Update(){
		if (!attacking) {
			if (Input.GetButton ("Normal") && !stopClockA) {
				timeHeld += Time.deltaTime;
			}
			if (timeHeld >= .25F && !stopClockA) {
			
			}

			if (Input.GetButtonUp ("Normal")) {
				if (timeHeld < .25F) {
					if (Input.GetAxisRaw ("Horizontal") == 1) {

					} else if (Input.GetAxisRaw ("Horizontal") == -1) {

					} else if (Input.GetAxisRaw ("Vertical") == 1) {

					} else if (Input.GetAxisRaw ("Vertical") == -1) {

					} else {
						if (playerControls.characterStats.moveSet [0].attackType == AttackTypes.ComboPunch && playerControls.isGrounded) {
							StartCoroutine (ComboPunch (playerControls.characterStats.moveSet [0]));
							stopClockA = true;
						} else if (playerControls.characterStats.moveSet [0].attackType == AttackTypes.ComboSummon && playerControls.isGrounded) {
							Debug.Log ("Shouldwork...");
							StartCoroutine (ComboSummon (playerControls.characterStats.moveSet [0]));
							stopClockA = true;
						}

					}
				}
				timeHeld = 0;
				stopClockA = false;


			}
			if (Input.GetButtonDown ("Special")) {

			}
		}

	}


	public IEnumerator Taunt(string tauntName){
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
		attacking = false;
	}

	////////////////////Attacks

	public void HandleAttack(Attack attack, int combo){
		if (attack.attackType == AttackTypes.ComboPunch) {
			
		}
		if (attack.attackType == AttackTypes.ComboSummon) {
		}

		if (attack.attackType == AttackTypes.ChargeUp) {
			StartCoroutine (ChargeUp (attack));
		}
	}

	public IEnumerator ComboPunch(Attack attack){
		return null;
	}

	public IEnumerator ComboSummon(Attack attack){
		print("HYA, KYA");
		playerControls.attacking = true;
		if (attack.attackTimes > 1) {

		} else if (attack.attackTimes == 1) {
			GameObject g = GameObject.Instantiate (attack.AttackObjects[0]);
			g.transform.localPosition = Vector2.zero;
			g.SetActive (true);
			g.transform.SetParent (playerControls.gameObject.transform);
			for (int i = 0; i < attack.hitboxPositions.Length; i++) {
				g.transform.localPosition = Vector2.Lerp (g.transform.localPosition, new Vector2(attack.hitboxPositions [i].x * -(playerControls.facingNum),attack.hitboxPositions[i].y), 1);
				yield return new WaitForSeconds (attack.betweenWaits [i]);
				Debug.Log ("Spem?");
			}
			Destroy (g);
		} else {
			Debug.Log ("Attack Failed");
		}
		playerControls.attacking = false;
		yield break;
	}

	public IEnumerator ChargeUp(Attack attack){

		return null;
	}

}
