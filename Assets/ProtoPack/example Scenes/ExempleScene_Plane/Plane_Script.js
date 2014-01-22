/*Plane.js by Lincoln Green(http://www.binkworks.com/)*/

/*SETUP INSTRUCTIONS FOR SCRIPT:
	There are some oddities with the default rotation of some of the objects in the protopack, one of these being that the plane's nose points backward along the Z-axis. To circumvent this, I have placed the plane model from the protopack into an empty gameobject and then flipped the plane's z-object. This empty gameobject is the object that contains this script. This allows us to use transform.Translate(0, 0, 1) to move forward, rather than backward. 
	To get the propeller of the plane to spin, you need to drag the propeller object into the 'propeller' slot of this script.*/

/*ABOUT
	The first thing that is done in the Update() function is rotating the propeller. This is for display only - you can remove it without killing your plane.

The next thing we do is define a defaultTilt Quaternion. Basically what this vector does is take the rotation of the plane and sets the x and z values to 0 so the plane is not nosing up or down and is not turning. Note that this does not affect the plane itself in any way - it is just an unused Quaternion(until later).

The next thing we do is read input and modify the eulerAngles of the transform based on input. Left/Right arrows turn the airplane, so we rotate along the y-axis. Up/Down arrows nose the plane up and down, so we rotate along the x axis. Note that there is some funky business going on with the up arrow - it is commented below.

If none of the arrows keys are pressed, then we smoothly rotate the plane back to it's original rotation(the defaultTilt).

Have fun!
*/


///////////////////////Fog

// Fog and Ambient light settings
RenderSettings.ambientLight = Color(0.8,0.8,0.8,1);
RenderSettings.fogDensity = 0.007;
RenderSettings.fogColor = Color(0,0.5,0.4,1);

// Enable fog
RenderSettings.fog = true;

///////////////////////


var propeller : Transform; //the prop object
var speed : float = 50; //the speed we move along just to stay in the air
var turnSpeed : float = 50; //how fast to turn
var climbSpeed : float = 5; //how fast the plane rotates up
var climbLimit : float = 40; //how far the plane can rotate to move up
var diveSpeed : float = 5; //how fast the plane rotates down
var diveLimit : float = 320; //how far the plane can rotate to nose down
var topLimit : float = 100; //how far the plane can go up
var bottomLimit : float = 5;//how far the plane can go down

var ButtonSound : AudioClip;

private var defaultTilt : Quaternion; //the rotation to return to when a key is not pressed

function Start(){
	//if they don't give us a propeller, (try to) assign it automatically
	if(!propeller){
		if(transform.Find("Propeller")){
			propeller = transform.Find("Propeller");
		} else {
			Debug.Log("No propeller assigned");
		}
	}
}

function Update () {

	//Engine sound
	audio.pitch = speed / 20.0;


	if(propeller)
		propeller.Rotate(0.0, 0.0, 250 * Time.deltaTime); //rotate the prop before doing anything else	
	
	defaultTilt = transform.rotation; //if a key is not being pressed we will move back to this rotation
	defaultTilt.x = 0; //and we don't want to be turning if a key isn't held down
	defaultTilt.z = 0;
	
	if(Input.GetAxis("Horizontal") > 0.0){ //if the right arrow is pressed
		transform.eulerAngles.y += turnSpeed * Time.deltaTime; //and then turn the plane
	}
	
	if(Input.GetAxis("Horizontal") < 0.0){ //if the left arrow is pressed
		transform.eulerAngles.y -= turnSpeed * Time.deltaTime;
	}
	
	if(Input.GetAxis("Vertical") < 0.0){ //if the down arrow is pressed
		if(transform.eulerAngles.x < climbLimit || transform.eulerAngles.x > 275){ /*We perform the second check becuase if the user as pressed the down key, the eulerAngles.x variable will have been decreased. Since the variable starts out at 0, decreasing it behaves as though the variable starts at 360(i.e if the plane's rotation is at 0 and you subtract 10 from that, you will end up with a rotation of 350).*/
			transform.eulerAngles.x += climbSpeed * 5 * Time.deltaTime; //nose up
		}
	}
	
	if(Input.GetAxis("Vertical") > 0.0){ //if the up arrow is pressed		
		if(transform.eulerAngles.x > diveLimit || transform.eulerAngles.x < 275){
			transform.eulerAngles.x -= diveSpeed * 5 * Time.deltaTime; //nose down
		}
	}
	
	if(!(Input.GetAxis("Vertical") || Input.GetAxis("Horizontal"))){ //if an arrow isn't being pressed
		transform.rotation = Quaternion.Slerp(transform.rotation, defaultTilt, 5 * Time.deltaTime); //then return to the default rotation
	}
	
	if (transform.position.y<bottomLimit) transform.position.y = bottomLimit; //constraints
	if (transform.position.y>topLimit) transform.position.y = topLimit; //constraints
	 
	transform.Translate(0, 0, speed * Time.deltaTime); //and move us
}



var bip : AudioClip;

function OnTriggerEnter (other : Collider) {
   
   audio.PlayOneShot(bip);
   
}




function OnGUI () {

		//Back to the Menu
		if(GUI.Button ( Rect(Screen.width - 60,10,50,20), "Back")){
				
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (0);
		}
		
}