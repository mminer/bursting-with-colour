
var Object_Projectile_gun : GameObject;
var Sound_Projectile_gun : AudioClip;
var Object_Projectile_rifle : GameObject;
var Sound_Projectile_rifle : AudioClip;
var Object_Projectile_sword : GameObject;
var Sound_Projectile_sword : AudioClip;
var Object_Projectile_magic : GameObject;
var Sound_Projectile_magic : AudioClip;

var Weapon_gun1 : GameObject;
var Weapon_gun2 : GameObject;
var Weapon_rifle : GameObject;
var Weapon_sword : GameObject;
var Weapon_shield : GameObject;
var Weapon_wand : GameObject;
var Weapon_hat : GameObject;

private var weapon : int = 0;
private var weaponAnimation : String;

var MoveSpeed : float = 5.0;
private var ForwardDirection : Vector3 = Vector3.zero;
private var charController : CharacterController;
private var gravity : float = 9.81;
private var RunSpeed : float = 5.0;
private var CurrentAction : int = 0;

static var score : int = 0;

var ButtonSound : AudioClip;








/////////////////////////////////////////
/////////////// GUI /////////////////////
/////////////////////////////////////////



function OnGUI () {



		
		if(GUI.Button ( Rect(Screen.width - 60,10,50,20), "Back"))  {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (0);
		}
		
		
		GUI.Label ( Rect(10,10,300,20), "Score : " + score); 

		if(GUI.Button ( Rect(270,10,60,20), "1 Gun")) {
		
			audio.PlayOneShot(ButtonSound);
			ChangeWeapon(1);
		}
			
		if(GUI.Button ( Rect(340,10,60,20), "2 Guns")) {
		
			audio.PlayOneShot(ButtonSound);
			ChangeWeapon(2);
		}
		
		if(GUI.Button ( Rect(410,10,60,20), "Rifle")) {
		
			audio.PlayOneShot(ButtonSound);
			ChangeWeapon(3);
		}
		
		if(GUI.Button ( Rect(480,10,60,20), "Wand")) {
		
			audio.PlayOneShot(ButtonSound);
			ChangeWeapon(5);
		}
		
		if(GUI.Button ( Rect(550,10,60,20), "Sword")) {
		
			audio.PlayOneShot(ButtonSound);
			ChangeWeapon(4);
		}
}







/////////////////////////////////////////
///////PLAYER MANAGEMENT////////
/////////////////////////////////////////



function Start()
{
    charController = GetComponent(CharacterController);
	ChangeWeapon(2);
}




function Update () 
{

    if(charController.isGrounded == true)
    {
		
		//Moving forward
        if(Input.GetAxis("Vertical") > .1) {
		
			animation.CrossFade("run",0.3);
			RunSpeed = MoveSpeed;
		
		//Moving backward
        } else if (Input.GetAxis("Vertical") < -.1){
		
			animation.CrossFade("back",0.3);
			RunSpeed = MoveSpeed/2.0;
		
		//Idle & Turn
		} else {
			
			if(Input.GetAxis("Horizontal") && !Input.GetAxis("Vertical")) animation.CrossFade("turn",0.3);
			else animation.CrossFade("idle", 0.1);		
        }
		
		
        transform.eulerAngles.y += 2*Input.GetAxis("Horizontal");
        ForwardDirection = Vector3(0,0, Input.GetAxis("Vertical"));
        ForwardDirection = transform.TransformDirection(ForwardDirection);
		

    }
	
	//Action
	if (Input.GetButton ("Jump")) {
		
		if (CurrentAction == 0) {
				CurrentAction = 1;
				Action();
		}

	}
	
	
	if (CurrentAction == 0 ) animation.Blend(weaponAnimation);
	else if (CurrentAction == 1 ) animation.Blend(weaponAnimation + "_action", 1.0, 0.0);
	

    ForwardDirection.y -= gravity * Time.deltaTime;
    charController.Move(ForwardDirection * (Time.deltaTime * RunSpeed));
}








/////////////////////////////////////////
///////ACTIONS MANAGEMENT////////
/////////////////////////////////////////


function Action(){
	
	CreateProjectile();
	
	yield WaitForSeconds (animation[weaponAnimation + "_action"].length);
	CurrentAction = 0;

}


function CreateProjectile(){
		
		switch(weapon){
				
				case 1: // 1 Gun
				var MonProjectile1 = GameObject.Instantiate(Object_Projectile_gun, Weapon_gun1.transform.position, transform.rotation);
				audio.PlayOneShot(Sound_Projectile_gun);
				break;

				case 2: // 2 Guns
				var MonProjectile21 = GameObject.Instantiate(Object_Projectile_gun, Weapon_gun1.transform.position, transform.rotation);
				audio.PlayOneShot(Sound_Projectile_gun);
				yield WaitForSeconds (0.2);
				var MonProjectile22 = GameObject.Instantiate(Object_Projectile_gun, Weapon_gun2.transform.position, transform.rotation);
				audio.PlayOneShot(Sound_Projectile_gun);
				break;
				
				case 3: // Rifle
				var MonProjectile3 = GameObject.Instantiate(Object_Projectile_rifle, Weapon_rifle.transform.position, transform.rotation);
				audio.PlayOneShot(Sound_Projectile_rifle);
				break;
				
				case 4: // Sword
				var MonProjectile4 = GameObject.Instantiate(Object_Projectile_sword, Weapon_sword.transform.position, transform.rotation);
				audio.PlayOneShot(Sound_Projectile_sword);
				break;
				
				case 5: // Wand
				audio.PlayOneShot(Sound_Projectile_magic);
				yield WaitForSeconds (0.3);
				var MonProjectile5 = GameObject.Instantiate(Object_Projectile_magic, Weapon_wand.transform.position, transform.rotation);
				break;

		}

}







/////////////////////////////////////////
///////WEAPONS MANAGEMENT////////
/////////////////////////////////////////



function ChangeWeapon(newWeapon : int){

		if (weapon != newWeapon){
		
				MountWeapon(weapon, false);
				MountWeapon(newWeapon, true);
				weapon = newWeapon;
		
		}

}


function MountWeapon(WeaponNum : int, Action : boolean){

		if (WeaponNum != 0) {
		
				switch(WeaponNum){
				
						case 1:
						Weapon_gun1.renderer.enabled = Action;
						if (Action) weaponAnimation = "up_1gun";
						break;
						
						case 2:
						Weapon_gun1.renderer.enabled = Action;
						Weapon_gun2.renderer.enabled = Action;
						if (Action) weaponAnimation = "up_2guns";
						break;
						
						case 3:
						Weapon_rifle.renderer.enabled = Action;
						if (Action) weaponAnimation = "up_rifle";
						break;
						
						case 4:
						Weapon_sword.renderer.enabled = Action;
						Weapon_shield.renderer.enabled = Action;
						if (Action) weaponAnimation = "up_sword";
						break;
						
						case 5:
						Weapon_wand.renderer.enabled = Action;
						Weapon_hat.renderer.enabled = Action;
						if (Action) weaponAnimation = "up_wand";
						break;

				}
		}
}


















