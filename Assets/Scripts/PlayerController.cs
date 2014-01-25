using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float Gravity = 21f;	 //downward force
	public float TerminalVelocity = 20f;	//max downward speed
	public float JumpSpeed = 6f;
	public float MoveSpeed = 10f;
	
	public Vector3 MoveVector;
	public float VerticalVelocity;
	
	public CharacterController CharacterController;
	
	// Use this for initialization
	void Awake () 
	{
		CharacterController = gameObject.GetComponent("CharacterController") as CharacterController;
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkMovement();
		HandleActionInput();
		processMovement();
	}
	
	void checkMovement()
	{
		//move l/r
		var deadZone = 0.1f;
		VerticalVelocity = MoveVector.y;
		MoveVector = Vector3.zero;

		if(Input.GetAxis("L_XAxis_1_Mac") > deadZone || Input.GetAxis("L_XAxis_1_Mac") < -deadZone){
			MoveVector += new Vector3(Input.GetAxis("L_XAxis_1_Mac"), 0, 0);
		}
	}
	
	void HandleActionInput()
	{
		if (Input.GetAxis("L_YAxis_1_Mac") < 0){
			jump();
		}
	}
	
	void processMovement()
	{
		//transform moveVector into world-space relative to character rotation
		MoveVector = transform.TransformDirection(MoveVector);
		
		//normalize moveVector if magnitude > 1
		if(MoveVector.magnitude > 1){
			MoveVector = Vector3.Normalize(MoveVector);
		}
		
		//multiply moveVector by moveSpeed
		MoveVector *= MoveSpeed;
		
		//reapply vertical velocity to moveVector.y
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
		
		//apply gravity
		applyGravity();
		
		//move character in world-space
		CharacterController.Move(MoveVector * Time.deltaTime);
	}
	
	void applyGravity() 
	{
		if (MoveVector.y > -TerminalVelocity) {
			var x = MoveVector.x;
			var y = MoveVector.y - Gravity * Time.deltaTime;
			var z = MoveVector.z;

			MoveVector = new Vector3(x, y, z);
		}

		if(CharacterController.isGrounded && MoveVector.y < -1){
			MoveVector = new Vector3(MoveVector.x, (-1), MoveVector.z);
		}
	}
	
	public void jump()
	{
		if (CharacterController.isGrounded){
			VerticalVelocity = JumpSpeed;
		}
	}
}