using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Attack {

	public string name;
	public int minDamage;
	public int maxDamage;
	public float chargetime;
	public int maxPresses;
	public Vector2[] hitboxSizes;
	public Vector2[] hitboxPositions;
	public int minStun;
	public int maxStun;
	public int grapple;
	public Vector2[] grappleMovement;
	public bool disgruntles;

	public Attack(){

	}
}
