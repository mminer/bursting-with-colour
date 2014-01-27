using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.
	
	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	
	
	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	Transform airControlCheckTop;							// A position marking where to check if the player is colliding in the air.
	Transform airControlCheckBottom;	
	float airRadius = .4f;								// Radius of the overlap circle to determine if air control should be lost
	bool loseAirControl;
	bool loseAirControlRight;								// Boolean to take away air control abilities
	bool loseAirControlLeft;
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.
	bool jumped;
	float jumpStartTime;
	bool landed;
	float landTime;
	bool canJump;

	void Awake ()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		airControlCheckTop = transform.Find("AirControlCheck_Top");
		airControlCheckBottom = transform.Find("AirControlCheck_Bottom");
		anim = GetComponent<Animator>();
	}
	
	void FixedUpdate ()
	{
		// Give the player some time to get off the ground
		if (jumped && Time.time - jumpStartTime > 0.1f) {
			jumped = false;
		}

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		if (!landed && grounded) {
			landed = true;
			rigidbody2D.velocity = Vector2.zero;
		}

		loseAirControl = Physics2D.OverlapCircle(airControlCheckTop.position, airRadius, whatIsGround);

		if (!loseAirControl) {
			loseAirControl = Physics2D.OverlapCircle(airControlCheckBottom.position, airRadius, whatIsGround);
		}
		
		if (loseAirControl) {
			if (facingRight) {
				loseAirControlRight = true;
				loseAirControlLeft = false;
			} else {
				loseAirControlLeft = true;
				loseAirControlRight = false;
			}
		} else {
			loseAirControlLeft = false;
			loseAirControlRight = false;
		}
		
		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	}
	
	public void Move (float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch && anim.GetBool("Crouch")) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if ( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround)) {
				crouch = true;
			}
		}
		
		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);
		
		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !facingRight) {
			// ... flip the player.
			Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
		} else if(move < 0 && facingRight) {
			// ... flip the player.
			Flip();
		}
		
		//only control the player if grounded or airControl is turned on
		if (grounded || (airControl && !loseAirControlRight)) {
			if (loseAirControl && !grounded) {
				if (move < 1 && loseAirControlLeft) {
					return;
				} else if (move > 0 && loseAirControlRight) {
					return;
				}
			}
			
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move * crouchSpeed : move);
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));
			
			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}
		
		// If the player should jump...
		if (grounded && jump && !jumped) {
			Jump();
		}
	}
	
	void Jump ()
	{
		// Add a vertical force to the player.
		anim.SetBool("Ground", false);
		rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		jumpStartTime = Time.time;
		jumped = true;
		landed = false;
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}