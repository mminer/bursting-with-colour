
var Character1 : GameObject;
var Character2 : GameObject;
var Character3 : GameObject;

var Cam1 : GameObject;
var Cam2 : GameObject;
var Cam3 : GameObject;

private var BaseAnimation : String = "dance";
private var TopAnimation : String = "";

var ButtonSound : AudioClip;






function Start(){
	
	HideObjects(Character1);
	HideObjects(Character2);
	HideObjects(Character3);	
}






function Update () {


	Character1.animation.CrossFade(BaseAnimation, 0.1);
	Character2.animation.CrossFade(BaseAnimation, 0.1);
	Character3.animation.CrossFade(BaseAnimation, 0.1);

	if (TopAnimation != "") {
			Character1.animation.Blend(TopAnimation, 1.0, 0.0);
			Character2.animation.Blend(TopAnimation, 1.0, 0.0);
			Character3.animation.Blend(TopAnimation, 1.0, 0.0);
	}
}










function OnGUI () {


		//Back to the Main Menu
		if(GUI.Button ( Rect(Screen.width - 60,10,50,20), "Back"))  {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (0);
		}
		
		//Cameras
		if(GUI.Button ( Rect(10,10,100,20), "Camera1")) {
				
				audio.PlayOneShot(ButtonSound);
				camera.transform.position = Cam1.transform.position;
				camera.transform.rotation = Cam1.transform.rotation;
		
		}
		
		if(GUI.Button ( Rect(120,10,100,20), "Camera2")) {
				
				audio.PlayOneShot(ButtonSound);
				camera.transform.position = Cam2.transform.position;
				camera.transform.rotation = Cam2.transform.rotation;
		
		}
		
		if(GUI.Button ( Rect(230,10,100,20), "Camera3")) {
				
				audio.PlayOneShot(ButtonSound);
				camera.transform.position = Cam3.transform.position;
				camera.transform.rotation = Cam3.transform.rotation;
		
		}
		
		
		//Regular Animations
		GUI.Label ( Rect(10,Screen.height - 125,120,20), "Regular Animations"); 

		if(GUI.Button ( Rect(140,Screen.height - 125,100,20), "Dance")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("dance", 1);
		}
		
		if(GUI.Button ( Rect(250,Screen.height - 125,100,20), "Zombi Walk")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("ZombiWalk", 1);
		}


		//Bottom Animations
		GUI.Label ( Rect(10,Screen.height - 100,120,20), "Bottom Animations"); 

		if(GUI.Button ( Rect(140,Screen.height - 100,100,20), "Back")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("back", 2);
		}
		if(GUI.Button ( Rect(250,Screen.height - 100,100,20), "Idle")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("idle", 2);
		}
		if(GUI.Button ( Rect(360,Screen.height - 100,100,20), "Jump")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("jump", 2);
		}
		if(GUI.Button ( Rect(470,Screen.height - 100,100,20), "Run")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("run", 2);
		}
		if(GUI.Button ( Rect(580,Screen.height - 100,100,20), "Turn")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("turn", 2);
		}
		if(GUI.Button ( Rect(690,Screen.height - 100,100,20), "Walk")) {
		
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("walk", 2);
		}
		

		//Top Animations
		GUI.Label ( Rect(10,Screen.height - 50,120,20), "Top Animations"); 

		if(GUI.Button ( Rect(140,Screen.height - 50,100,20), "1 gun")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_1gun", 3);
				ShowObject("gun1");
		}
		if(GUI.Button ( Rect(250,Screen.height - 50,100,20), "2 guns")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_2guns", 3);
				ShowObject("gun1");
				ShowObject("gun2");
		}
		if(GUI.Button ( Rect(360,Screen.height - 50,100,20), "Rifle")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_rifle", 3);
				ShowObject("rifle");
		}
		if(GUI.Button ( Rect(470,Screen.height - 50,100,20), "Sword")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_sword", 3);
				ShowObject("sword");
				ShowObject("shield");
		}
		if(GUI.Button ( Rect(580,Screen.height - 50,100,20), "Wand")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_wand", 3);
				ShowObject("wand");
		}
		if(GUI.Button ( Rect(690,Screen.height - 50,100,20), "Idle")) {
				audio.PlayOneShot(ButtonSound);
				CharacterAnimation("up_idle", 3);
		}			
}





function CharacterAnimation (AnimName : String, AnimaType : int){


		if (AnimaType != 2) {
				HideObjects(Character1);
				HideObjects(Character2);
				HideObjects(Character3);
		}
				
		if (AnimaType == 1){
		
				BaseAnimation = AnimName;
				TopAnimation = "";
				
		} else if (AnimaType == 2){
		
				BaseAnimation = AnimName;
				if (TopAnimation == "") {
					TopAnimation = "up_idle";
					HideObjects(Character1);
					HideObjects(Character2);
					HideObjects(Character3);
				}

				
		} else if (AnimaType == 3 && BaseAnimation != "dance" && BaseAnimation != "ZombiWalk") TopAnimation = AnimName;
		
		
}






function HideObjects(CharacterObject : GameObject){


	var MyGun1: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/R UpperArm/R Forearm/R Hand/gun1");
	MyGun1.renderer.enabled = false;
	
	var MyGun2: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/L UpperArm/L Forearm/L Hand/gun2");
	MyGun2.renderer.enabled = false;
	
	var MyRifle: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/R UpperArm/R Forearm/R Hand/rifle");
	MyRifle.renderer.enabled = false;
	
	var MyWand: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/R UpperArm/R Forearm/R Hand/wand");
	MyWand.renderer.enabled = false;
	
	var MySword: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/R UpperArm/R Forearm/R Hand/sword");
	MySword.renderer.enabled = false;
	
	var MyShield: GameObject = GameObject.Find("/" + CharacterObject.name + "/biped/Spine/Spine1/Spine2/L UpperArm/L Forearm/shield");
	MyShield.renderer.enabled = false;
	
	
}





function ShowObject(MyObject : String){
		
		
		var MyObjectPath : String;
		
		if (MyObject == "gun1" || MyObject == "rifle" || MyObject == "sword" || MyObject == "wand") MyObjectPath = "/biped/Spine/Spine1/Spine2/R UpperArm/R Forearm/R Hand/" + MyObject;
		else if (MyObject == "gun2") MyObjectPath = "/biped/Spine/Spine1/Spine2/L UpperArm/L Forearm/L Hand/" + MyObject;
		else if (MyObject == "shield") MyObjectPath = "/biped/Spine/Spine1/Spine2/L UpperArm/L Forearm/" + MyObject;

		var C1MyObject: GameObject = GameObject.Find("/Character1" + MyObjectPath);
		var C2MyObject: GameObject = GameObject.Find("/Character2" + MyObjectPath);
		var C3MyObject: GameObject = GameObject.Find("/Character3" + MyObjectPath);
		C1MyObject.renderer.enabled = true;
		C2MyObject.renderer.enabled = true;
		C3MyObject.renderer.enabled = true;

}

