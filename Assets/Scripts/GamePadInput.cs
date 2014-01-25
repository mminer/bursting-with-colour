using UnityEngine;
using System.Collections;

public class GamePadInput : MonoBehaviour {

	public bool player1Jump = false;
	public float player1Left = 0f;
	public float player1Right = 0f;
	public bool player1pressA = false;
	public bool player1pressB = false;
	public bool player1pressX = false;
	public bool player1pressY = false;

	public bool player2Jump = false;
	public float player2Left = 0f;
	public float player2Right = 0f;
	public bool player2pressA = false;
	public bool player2pressB = false;
	public bool player2pressX = false;
	public bool player2pressY = false;

	void ResetInputs()
	{
		player1Jump = false;
		player1Left = 0f;
		player1Right = 0f;
		player1pressA = false;
		player1pressB = false;
		player1pressX = false;
		player1pressY = false;
		
		player2Jump = false;
		player2Left = 0f;
		player2Right = 0f;
		player2pressA = false;
		player2pressB = false;
		player2pressX = false;
		player2pressY = false;
	}

	// Update is called once per frame
	void Update () {

		ResetInputs();

		UpdatePlayer1Inputs();
		UpdatePlayer2Inputs();
		/*
		//PlayerOne
		//MovementLeft
		if ((Input.GetAxis ("L_XAxis_1_Mac") < 0) || (Input.GetAxis("L_XAxis_1_Win") < 0)) {
			Debug.Log("Player 1 Presses Left.");
		}
		//MovementRight
		if ((Input.GetAxis ("L_XAxis_1_Mac") > 0) || (Input.GetAxis("L_XAxis_1_Win") > 0)) {
			Debug.Log("Player 1 Presses Right.");
		}
		//Jump
		if ((Input.GetAxis ("L_YAxis_1_Mac") < 0) || (Input.GetAxis("L_YAxis_1_Win") < 0)) {
			Debug.Log("Player 1 Presses Up for Jump.");
		}
		//A
		if (Input.GetButtonDown ("A_1_Mac") || Input.GetButtonDown("A_1_Win")) {
			Debug.Log("Player 1 Presses A.");
		}
		//B
		if (Input.GetButtonDown ("B_1_Mac") || Input.GetButtonDown("B_1_Win")) {
			Debug.Log("Player 1 Presses B.");
		}
		//X
		if (Input.GetButtonDown ("X_1_Mac") || Input.GetButtonDown("X_1_Win")) {
			Debug.Log("Player 1 Presses X.");
		}
		//Y
		if (Input.GetButtonDown ("Y_1_Mac") || Input.GetButtonDown("Y_1_Win")) {
			Debug.Log("Player 1 Presses Y.");
		}
		*/
	}

	void UpdatePlayer1Inputs()
	{
		//MovementLeft
		float leftMovement = Input.GetAxis ("L_XAxis_1_Mac");
		if (leftMovement < 0) player1Left = leftMovement;
		else
		{
			leftMovement = Input.GetAxis("L_XAxis_1_Win");
			if (leftMovement < 0) player1Left = leftMovement;
		}
//		if ((Input.GetAxis ("L_XAxis_1_Mac") < 0) || (Input.GetAxis("L_XAxis_1_Win") < 0)) {
//			player1Left = 0f;
//		}

		//MovementRight
		float rightMovement = Input.GetAxis ("L_XAxis_1_Mac");
		if (rightMovement > 0) player1Right = rightMovement;
		else
		{
			leftMovement = Input.GetAxis("L_XAxis_1_Win");
			if (rightMovement > 0) player1Right = rightMovement;
		}
//		if ((Input.GetAxis ("L_XAxis_1_Mac") > 0) || (Input.GetAxis("L_XAxis_1_Win") > 0)) {
//			Debug.Log("Player 1 Presses Right.");
//		}

		//Jump
		if ((Input.GetAxis ("L_YAxis_1_Mac") < 0) || (Input.GetAxis("L_YAxis_1_Win") < 0)) {
			player1Jump = true;
		}
		//A
		if (Input.GetButton ("A_1_Mac") || Input.GetButton("A_1_Win")) {
			player1pressA = true;
		}
		//B
		if (Input.GetButton ("B_1_Mac") || Input.GetButton("B_1_Win")) {
			player1pressB = true;
		}
		//X
		if (Input.GetButton ("X_1_Mac") || Input.GetButton("X_1_Win")) {
			player1pressX = true;
		}
		//Y
		if (Input.GetButton ("Y_1_Mac") || Input.GetButton("Y_1_Win")) {
			player1pressY = true;
		}
	}

	void UpdatePlayer2Inputs()
	{
		//MovementLeft
		float leftMovement = Input.GetAxis ("L_XAxis_2_Mac");
		if (leftMovement < 0) player2Left = leftMovement;
		else
		{
			leftMovement = Input.GetAxis("L_XAxis_2_Win");
			if (leftMovement < 0) player2Left = leftMovement;
		}
		//		if ((Input.GetAxis ("L_XAxis_2_Mac") < 0) || (Input.GetAxis("L_XAxis_2_Win") < 0)) {
		//			player2Left = 0f;
		//		}
		
		//MovementRight
		float rightMovement = Input.GetAxis ("L_XAxis_2_Mac");
		if (rightMovement > 0) player2Right = rightMovement;
		else
		{
			leftMovement = Input.GetAxis("L_XAxis_2_Win");
			if (rightMovement > 0) player2Right = rightMovement;
		}
		//		if ((Input.GetAxis ("L_XAxis_2_Mac") > 0) || (Input.GetAxis("L_XAxis_2_Win") > 0)) {
		//			Debug.Log("Player 2 Presses Right.");
		//		}
		
		//Jump
		if ((Input.GetAxis ("L_YAxis_2_Mac") < 0) || (Input.GetAxis("L_YAxis_2_Win") < 0)) {
			player2Jump = true;
		}
		//A
		if (Input.GetButton ("A_2_Mac") || Input.GetButton("A_2_Win")) {
			player2pressA = true;
		}
		//B
		if (Input.GetButton ("B_2_Mac") || Input.GetButton("B_2_Win")) {
			player2pressB = true;
		}
		//X
		if (Input.GetButton ("X_2_Mac") || Input.GetButton("X_2_Win")) {
			player2pressX = true;
		}
		//Y
		if (Input.GetButton ("Y_2_Mac") || Input.GetButton("Y_2_Win")) {
			player2pressY = true;
		}
	}
}
