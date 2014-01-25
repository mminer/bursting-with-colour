using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public int playerNumber = 1;
	public float gravity = 21f;	 			// downward force
	public float terminalVelocity = 20f;	// max downward speed
	public float jumpSpeed = 6f;			// jump height
	public float moveSpeed = 10f;
	
	public Vector3 moveVector;
	public float verticalVelocity;
	
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
		verticalVelocity = moveVector.y;
		moveVector = Vector3.zero;

		if (Input.GetAxis(moveInputName) > deadZone || Input.GetAxis(moveInputName) < -deadZone) {
			moveVector += new Vector3(Input.GetAxis(moveInputName), 0, 0);
		}
	}
	
	void HandleActionInput()
	{
		if (Input.GetAxis(jumpInputName) < 0) {
			Jump();
		}
	}
	
	void ProcessMovement()
	{
		//transform moveVector into world-space relative to character rotation
		moveVector = transform.TransformDirection(moveVector);
		
		//normalize moveVector if magnitude > 1
		if (moveVector.magnitude > 1) {
			moveVector = Vector3.Normalize(moveVector);
		}
		
		//multiply moveVector by moveSpeed
		moveVector *= moveSpeed;
		
		//reapply vertical velocity to moveVector.y
		moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);
		
		//apply gravity
		ApplyGravity();
		
		//move character in world-space
		characterController.Move(moveVector * Time.deltaTime);
	}
	
	void ApplyGravity() 
	{
		if (moveVector.y > -terminalVelocity) {
			var x = moveVector.x;
			var y = moveVector.y - gravity * Time.deltaTime;
			var z = moveVector.z;

			moveVector = new Vector3(x, y, z);
		}

		if (characterController.isGrounded && moveVector.y < -1) {
			moveVector = new Vector3(moveVector.x, (-1), moveVector.z);
		}
	}
	
	public void Jump()
	{
		if (characterController.isGrounded) {
			verticalVelocity = jumpSpeed;
		}
	}
}