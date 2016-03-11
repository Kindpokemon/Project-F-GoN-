using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Attack {

	//Basic Info
	public string name;
	public AnimCombat.AttackTypes attackType;
	public float chargetime;
	public float currentCharge;

	//Damage
	public int minDamage;
	public int maxDamage;
	public bool disgruntles;

	//Animations
	public string attackAnim; //Final Swing/Strike
	public string chargingAnim; //Charging
	public string summonAnim; //For Summoning STuff
	public string comboAnim; //For combo attacks

	//Object Info
	public Object[] AttackObjects;
	public Vector2[] positions;//Also used for moving projectiles
	public float[] betweenWaits;
	public enum SummonType{
		AtOnce,
		Consecutive
	}
	public SummonType summonType;

	//For Multiattacks
	public int maxPresses;
	public int currentPresses;
	public int finalDamage;
	public bool[] flipped;

	//If Stuns
	public int maxStun;
	public int minStun;

	//GRAPPLE
	public bool grapples;
	public bool grappleMoves;


	public Attack(){
		
	}
}
