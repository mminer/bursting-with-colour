using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public int playerNumber = 1;
	public float Gravity = 21f;	 //downward force
	public float TerminalVelocity = 20f;	//max downward speed
	public float JumpSpeed = 6f;
	public float MoveSpeed = 10f;
	
	public Vector3 MoveVector;
	public float VerticalVelocity;
	
	public CharacterController characterController;

	string jumpInputName;
	string moveInputName;
	string aInputName;
	string bInputName;
	string xInputName;
	string yInputName;
	
	// Use this for initialization
	void Awake () 
	{
		DetermineInputs();
	}

	void DetermineInputs ()
	{
		var suffix = "Win";

		#if UNITY_STANDALONE_OSX
		suffix = "Mac";
		#endif

		jumpInputName = GenerateInputName("L_YAxis", suffix);
		moveInputName = GenerateInputName("L_XAxis", suffix);
		aInputName = GenerateInputName("A", suffix);
		bInputName = GenerateInputName("B", suffix);
		xInputName = GenerateInputName("X", suffix);
		yInputName = GenerateInputName("Y", suffix);
	}

	string GenerateInputName (string prefix, string suffix)
	{
		return prefix + "_" + playerNumber + "_" + suffix;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckColourSwitch();
		CheckMovement();
		HandleActionInput();
		ProcessMovement();
	}

	void CheckColourSwitch ()
	{
		if (Input.GetButtonDown(aInputName)) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Green);
		} else if (Input.GetButtonDown(bInputName)) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Red);
		} else if (Input.GetButtonDown(xInputName)) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Blue);
		} else if (Input.GetButtonDown(yInputName)) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Yellow);
		}
	}
	
	void CheckMovement()
	{
		//move l/r
		var deadZone = 0.1f;
		VerticalVelocity = MoveVector.y;
		MoveVector = Vector3.zero;

		if (Input.GetAxis(moveInputName) > deadZone || Input.GetAxis(moveInputName) < -deadZone){
			MoveVector += new Vector3(Input.GetAxis(moveInputName),0,0);
		}
	}
	
	void HandleActionInput()
	{
		if(Input.GetAxis(jumpInputName) < 0){
			Jump();
		}
	}
	
	void ProcessMovement()
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
		ApplyGravity();
		
		//move character in world-space
		characterController.Move(MoveVector * Time.deltaTime);
	}
	
	void ApplyGravity() 
	{
		if (MoveVector.y > -TerminalVelocity) {
			var x = MoveVector.x;
			var y = MoveVector.y - Gravity * Time.deltaTime;
			var z = MoveVector.z;

			MoveVector = new Vector3(x, y, z);
		}

		if(characterController.isGrounded && MoveVector.y < -1){
			MoveVector = new Vector3(MoveVector.x, (-1), MoveVector.z);
		}
	}
	
	public void Jump()
	{
		if (characterController.isGrounded){
			VerticalVelocity = JumpSpeed;
		}
	}
}