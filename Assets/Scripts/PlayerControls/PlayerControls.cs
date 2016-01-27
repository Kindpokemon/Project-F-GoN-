using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	public GameObject player;
	public bool jump;
	public bool crouch;
	public float walkSpeed;
	public float runsSpeed;
	public float crouchSpeed;
	public float jumpHeight;
	public GameObject stage;
	public Vector2 movement_vector;
	public Rigidbody2D rbody;

	void Start(){
		rbody = this.GetComponent<Rigidbody2D> ();
	}


	void Update()
	{
		if (!jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			jump = Input.GetKeyDown(KeyCode.Space);
		}
	}


	void FixedUpdate() {
		// Read the inputs.

		crouch = Input.GetKey(KeyCode.LeftControl);
		movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
		if (movement_vector != Vector2.zero) {

		}

		// Pass all parameters to the character control script.
		if(Input.GetKey(KeyCode.LeftShift)){
			rbody.MovePosition (rbody.position + movement_vector * runsSpeed * Time.deltaTime/2);
		} else {
			rbody.MovePosition (rbody.position + movement_vector * walkSpeed * Time.deltaTime/2);
		}
	}
}
