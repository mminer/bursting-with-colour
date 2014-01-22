/*Car.js For original source, visit http://www.unifycommunity.com/ and look for the JCar script. Ported to Javascript and modified by Lincoln Green(http://www.binkworks.com/)*/

/*INFO
This is basically a modified version of the JCar script found on the unifycommunity wiki(also ported to JavaScript). There are quite a few problems with it, but as far as I have seen from searching the forums and wiki, there's not really a way to get good results out of wheel colliders(the standard method seems to be to implement your own). Try playing around with the setting and see if you can come up with anything. Have fun!*/

/*CAMERA FOLLOW
If you look at the car prefab, you will notice a couple extra objectsL Camera and CamFollow. The Camera is (oddly enough) the camera that follows the car. The CamFollow object is an empty gameobject that is flipped along the z-axis. The purpose of this object is to make it so the SmoothFollow script works. If you were to target the car itself rather than the CamFollow object, the camera would follow the car from the front than the back. Try setting the target of the Camera smoothfollow script to the car itself rather than the CamFollow object to see what I mean.

Enjoy!
*/


///////////////////////Fog

// Fog and Ambient light settings
RenderSettings.ambientLight = Color(0.8,0.8,0.8,1);
RenderSettings.fogDensity = 0.007;
RenderSettings.fogColor = Color(0,0.5,0.4,1);

// Enable fog
RenderSettings.fog = true;

///////////////////////


var wheelFR : Transform; // connect to Front Right Wheel transform
var wheelFL : Transform; // connect to Front Left Wheel transform
var wheelBR : Transform; // connect to Back Right Wheel transform
var wheelBL : Transform; // connect to Back Left Wheel transform
	
var suspensionDistance : float = 0.45; // amount of movement in suspension
var springs : float = 1000.0; // suspension springs
var dampers : float = 20; // how much damping the suspension has
var torque : float = 200; // the base power of the engine (per wheel, and before gears)
var brakeTorque : float = 2000; // the power of the braks (per wheel)
var wheelWeight : float = 4; // the weight of a wheel
var shiftCenter : Vector3 = Vector3(0.0, -1.0, 0.0); // offset of center of mass
var frontWheelRadius : float = 0.3;
var backWheelRadius : float = 0.5;

var maxSteerAngle : float = 15.0; // max angle of steering wheels

var idleRPM : float = 500.0; // idle rpm

private var currentGear : int = 1; //are we moving forward or in reverse
private var wheelRadius : float;

var ButtonSound : AudioClip;


	// every wheel has a wheeldata struct, contains useful wheel specific info
class WheelData {
	var transform : Transform;
	var go : GameObject;
	var col : WheelCollider;
	var startPos : Vector3;
	var rotation : float = 0.0;
	var maxSteer : float;
	var motor : boolean;
	var radius : float;
};

var wheels : WheelData[]; // array with the wheel data

// setup wheelcollider for given wheel data
// wheel is the transform of the wheel
// maxSteer is the angle in degrees the wheel can steer (0 for no steering)
// motor if wheel is driven by engine or not
function SetWheelParams(wheel : Transform, maxSteer : float, motor : boolean, rad : float) {
	var result : WheelData = new WheelData();	// the container of wheel specific data
																	// we create a new gameobject for the collider and move, transform it to match
																	// the position of the wheel it represents. This allows us to do transforms
																	// on the wheel itself without disturbing the collider.
	var go : GameObject = new GameObject("WheelCollider");
	go.transform.parent = transform;					// the car, not the wheel is parent
	go.transform.position = wheel.position; 		// match wheel pos
	
	var col : WheelCollider = go.AddComponent("WheelCollider");	// create the actual wheel collider in the collider game object
	col.motorTorque = 0.0;
	
	// store some useful references in the wheeldata object
	result.transform = wheel; // access to wheel transform 
	result.go = go; // store the collider game object
	result.col = col; // store the collider self
	result.startPos = go.transform.localPosition; // store the current local pos of wheel
	result.maxSteer = maxSteer; // store the max steering angle allowed for wheel
	result.motor = motor; // store if wheel is connected to engine
	result.radius = rad;
	
	return result; // return the WheelData
}

// Use this for initialization
function Start () {

	// 4 wheels, if needed different size just modify and modify
	// the wheels[...] block below.
	wheels = new WheelData[4];
	
	// we use 4 wheels, but you can change that easily if neccesary.
	// this is the only place that refers directly to wheelFL, ...
	// so when adding wheels, you need to add the public transforms,
	// adjust the array size, and add the wheels initialisation here.
	//the radius of the front wheels
	wheels[0] = SetWheelParams(wheelFR, maxSteerAngle, true, frontWheelRadius); //setup the wheels
	wheels[1] = SetWheelParams(wheelFL, maxSteerAngle, true, frontWheelRadius);
	wheels[2] = SetWheelParams(wheelBR, 0.0, false, backWheelRadius);
	wheels[3] = SetWheelParams(wheelBL, 0.0, false, backWheelRadius);
	
	// found out the hard way: some parameters must be set AFTER all wheel colliders
	// are created, like wheel mass, otherwise your car will act funny and will
	// flip over all the time.
	for (var w : WheelData in wheels) {
		var col : WheelCollider = w.col;
		col.suspensionDistance = suspensionDistance;
		var js : JointSpring = col.suspensionSpring;
		js.spring = springs;
		js.damper = dampers;			
		col.suspensionSpring = js;
		col.radius = w.radius;
		col.mass = wheelWeight;
					
		// see docs, haven't really managed to get this work
		// like i would but just try out a fiddle with it.
		var fc : WheelFrictionCurve = col.forwardFriction;
		fc.asymptoteValue = 10000.0;
		fc.extremumSlip = 2.0;
		fc.asymptoteSlip = 20.0;
		fc.stiffness = 0.03;
		col.forwardFriction = fc;
		fc = col.sidewaysFriction;
		fc.asymptoteValue = 15000.0;
		fc.asymptoteSlip = 1.0;
		fc.stiffness = 0.03;
		col.sidewaysFriction = fc;
	}
	
	// we move the center of mass (somewhere below the center works best.)
	rigidbody.centerOfMass += shiftCenter;		
}



var wantedRPM : float = 0.0; // rpm the engine tries to reach
var motorRPM : float = 0.0;
var killEngine : float = 0.0;



function Update() {
	
	//Engine sound
	audio.pitch = motorRPM / 1000.0 + 1.0;

}


// handle the physics of the engine
function FixedUpdate () {
	var delta : float = Time.fixedDeltaTime;
	
	var steer : float = Input.GetAxis("Horizontal"); // steering -1.0 .. 1.0
	var accel : float = Input.GetAxis("Vertical"); // accelerating -1.0 .. 1.0
	var brake : boolean = Input.GetButton("Jump");  // braking (true is brake)
	
	var reverse : boolean = false;
	// handle automatic shifting
	if (accel < 0.0) {
		if(currentGear == 1){
			brake = true;
			accel = 0.0;
			wantedRPM = 0.0;
		}
		currentGear = 0; // reverse
		
	} else if (accel > 0.0) {
		if(currentGear == 0){
			brake = true;
			accel = 0.0;
			wantedRPM = 0.0;
		}
		currentGear = 1; // go from reverse to first gear
	}

	if (accel < 0.0 && !reverse) {
		// if we try to decelerate we brake.
	}
		// the RPM we try to achieve.
	//wantedRPM = (3000.0 * accel) * 0.1 + wantedRPM * 0.9;
	wantedRPM = (3000.0 * accel);
	if (wantedRPM < 10) wantedRPM = 0;
	
	var rpm : float = 0.0;
	var motorizedWheels : float = 0;
	var floorContact : boolean = false;
	
	var col : WheelCollider;
	// calc rpm from current wheel speed and do some updating
	for (var w : WheelData in wheels) {
		var hit : WheelHit;
		col = w.col;
		
		// only calculate rpm on wheels that are connected to engine
		if (w.motor) {
			rpm += col.rpm;
			motorizedWheels++;
		}
		
		w.rotation = Mathf.Repeat(w.rotation - delta * col.rpm * 360.0 / 60.0, 360.0); // calculate the local rotation of the wheels from the delta time and rpm
		w.transform.localRotation = Quaternion.Euler(w.rotation, col.steerAngle, 0.0); // then set the local rotation accordingly (also adjust for steering)
		
		// let the wheels contact the ground, if no groundhit extend max suspension distance
		var lp : Vector3 = w.transform.localPosition;
		if (col.GetGroundHit(hit)) {
			lp.y -= Vector3.Dot(w.transform.position - hit.point, transform.up) - col.radius;
			floorContact = floorContact || (w.motor);
		}
		else {
			lp.y = w.startPos.y - suspensionDistance;
		}
		w.transform.localPosition = lp;
	}
	// calculate the actual motor rpm from the wheels connected to the engine
	// note we haven't corrected for gear yet.
	if (motorizedWheels > 1) {
		rpm = rpm / motorizedWheels;
	}
	
	// we do some delay of the change (should take delta instead of just 95% of
	// previous rpm, and also adjust or gears.
	
	motorRPM = 0.95 * motorRPM + 0.05 * Mathf.Abs(rpm * (currentGear == 1 ? 2 : -6)); //see below
	if (motorRPM > 3000.0) motorRPM = 3000.0;
	
	// calculate torque using gears
	var newTorque : float = torque * (currentGear == 1 ? 5 : -5); //if the current gear is 1, we need to add a force moving forward - otherwise backward
	for (var w : WheelData in wheels) {		// go set torque to the wheels

		col = w.col;
		
		// of course, only the wheels connected to the engine can get engine torque
			if (w.motor) {
			// only set torque if wheel goes slower than the expected speed
			if (Mathf.Abs(col.rpm) > Mathf.Abs(wantedRPM)) {
				// wheel goes too fast, set torque to 0
				col.motorTorque = 0;
			}
			else {
				// 
				var curTorque : float = col.motorTorque;
				col.motorTorque = curTorque * 0.7 + newTorque * 0.3;
			}
		}
		// check if we have to brake
 		
		// set steering angle
		col.steerAngle = steer * w.maxSteer;
	}
}






function OnGUI () {

		//Back to the Menu
		if(GUI.Button ( Rect(Screen.width - 60,10,50,20), "Back")){
				
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (0);
		}
		
}
