using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Attack {

	//Basic Info
	public string name;
	public AttackAction.AttackTypes attackType;
	public int minDamage;
	public int maxDamage;
	public float chargetime;
	public bool disgruntles;
	public string animName;

	//Object Info
	public Object[] AttackObjects;
	public Vector2[] hitboxSizes;
	public Vector2[] hitboxPositions;//Also used for moving projectiles
	public float[] betweenWaits;
	public float[] moveSpeeds;
	public List<Quaternion> objectRotation;

	//For Multiattacks
	public int maxPresses;
	public int attackTimes;
	public int finalDamage;

	//If Stuns
	public int maxStun;
	public int minStun;

	//GRAPPLE
	public int grapple;
	public bool grappleMoves;
	public Vector2[] grappleMovement;


	public Attack(){
		
	}
}
