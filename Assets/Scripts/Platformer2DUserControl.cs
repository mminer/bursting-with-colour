using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour 
{
	private Player player;
	private PlatformerCharacter2D character;
    private bool jump;
	
	string jumpInputName;
	string moveInputName;
	string aInputName;
	string bInputName;
	string xInputName;
	string yInputName;

	void Awake() 
	{
		character = GetComponent<PlatformerCharacter2D>();
		player = GetComponent<Player>();
		DetermineInputs();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
        if (CrossPlatformInput.GetAxis(jumpInputName) < -0.2f)
		{
            jump = true;
		}
		//keyboard input
		else
		{
			if ((player.playerID == 1) && (Input.GetKey (KeyCode.W)))
				jump = true;
			else if ((player.playerID == 2) && (Input.GetKey (KeyCode.UpArrow)))
				jump = true;
		}

		CheckColourSwitch();
    }

	void FixedUpdate ()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInput.GetAxis(moveInputName);
		if (h == 0)
		{
			if ((player.playerID == 1) && Input.GetKey(KeyCode.A))
				h = -1f;
			else if ((player.playerID == 2) && Input.GetKey(KeyCode.LeftArrow))
				h = -1f;
			else if ((player.playerID == 1) && Input.GetKey(KeyCode.D))
				h = 1f;
			else if ((player.playerID == 2) && Input.GetKey(KeyCode.RightArrow))
				h = 1f;
		}
		// Pass all parameters to the character control script.
		character.Move(h, crouch, jump);

        // Reset the jump input once it has been used.
	    jump = false;
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
		return prefix + "_" + player.playerID + "_" + suffix;
	}

	void CheckColourSwitch ()
	{
		if (Input.GetButtonDown(aInputName) || ((player.playerID == 1) && Input.GetKey(KeyCode.F)) || ((player.playerID == 2) && Input.GetKey(KeyCode.Keypad1))) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Green);
		}
		else if (Input.GetButtonDown(bInputName) || ((player.playerID == 1) && Input.GetKey(KeyCode.G)) || ((player.playerID == 2) && Input.GetKey(KeyCode.Keypad2))) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Red);
		}
		else if (Input.GetButtonDown(xInputName) || ((player.playerID == 1) && Input.GetKey(KeyCode.H)) || ((player.playerID == 2) && Input.GetKey(KeyCode.Keypad3))) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Blue);
		}
		else if (Input.GetButtonDown(yInputName) || ((player.playerID == 1) && Input.GetKey(KeyCode.J)) || ((player.playerID == 2) && Input.GetKey(KeyCode.KeypadEnter))) {
			LayerManager.ToggleLayer(gameObject, LayerColor.Yellow);
		}
	}
}
