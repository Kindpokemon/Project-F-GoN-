using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterStats {
	public string characterName;
	public BoxCollider2D hitbox;
	public Animator animationController;
	public int killPercent;
	public float walkSpeed;
	public float runSpeed;
	public float crouchSpeed;
	public float jumpHeight;
	public float dJumpHeight;
	public float acceleration;
	public float gravity;
	public int jumps;
	public List<Attack> moveSet;
	public List<NonAttack> otherMoves;

	public CharacterStats(){

	}
}
