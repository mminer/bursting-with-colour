var LaunchSound : AudioClip;
var ButtonSound : AudioClip;

var POV1 : Transform;
var POV2 : Transform;

private var buzy : int = 1;
private var Force : String = "-";


function Start(){

	yield WaitForSeconds (1.0);
	buzy = 0;

}


function Launch(){

	
	//Apply a force to the ragdoll
	
	var ForwardForce = Random.Range(11000,14000);
	var UpForce = Random.Range(7000,9000);
	
	Force = "( 0, " + UpForce + ", -" + ForwardForce + " )";
	
	var Launch : Vector3 = Vector3(0,UpForce, -ForwardForce);
	var ImpactPoint : GameObject = GameObject.Find("biped");
	ImpactPoint.rigidbody.AddForce(Launch);
	
	yield WaitForSeconds (3.0);
	buzy = 2;

}



function OnGUI () {

		//Back to the Menu
		if(GUI.Button ( Rect(Screen.width - 60,10,50,20), "Back")){
				
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (0);
		}
		
		//Force
		GUI.Label ( Rect(10,400,300,40), "Force: " + Force);
		
		//Buttons
		if (buzy == 0){
		
				if(GUI.Button ( Rect(Screen.width/2.0 - 50,400,100,40), "LAUNCH")){
						
						audio.PlayOneShot(LaunchSound);
						buzy = 1;
						Launch();
				}
		} else if (buzy == 2){
		
				if(GUI.Button ( Rect(Screen.width/2.0 - 50,400,100,40), "RESET")){
						
						audio.PlayOneShot(ButtonSound);
						Application.LoadLevel (5);
				}
		}
		
		
		//Cameras
		if (buzy != 2) {
		
				if(GUI.Button ( Rect(10,10,100,20), "Camera1")) {
						
						audio.PlayOneShot(ButtonSound);
						Camera.main.GetComponent ("SmoothFollow").enabled = true;
				
				}
				
				if(GUI.Button ( Rect(120,10,100,20), "Camera2")) {
						
						audio.PlayOneShot(ButtonSound);
						Camera.main.GetComponent ("SmoothFollow").enabled = false;
						Camera.main.transform.position = POV1.position;
						Camera.main.transform.rotation = POV1.rotation;
				
				}
				
				if(GUI.Button ( Rect(230,10,100,20), "Camera3")) {
						
						audio.PlayOneShot(ButtonSound);
						Camera.main.GetComponent ("SmoothFollow").enabled = false;
						Camera.main.transform.position = POV2.position;
						Camera.main.transform.rotation = POV2.rotation;
				
				}
		
		}
}




