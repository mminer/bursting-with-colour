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
        if (CrossPlatformInput.GetAxis(jumpInputName) < 0) {
            jump = true;
		}

		CheckColourSwitch();
    }

	void FixedUpdate ()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInput.GetAxis(moveInputName);

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
}
