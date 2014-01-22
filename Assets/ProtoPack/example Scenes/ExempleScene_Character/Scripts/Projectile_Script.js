
var Ragdoll : GameObject;
var ImpactForce : int = 1000;
var UpForce : int = 0;
var MoveSpeed : float = 10.0;
var ProjectileRange : int = 20;

private var InitialPosition : Vector3;



function Start(){

		InitialPosition = transform.position;
}


function Update () 
{
		//Move projectile
		transform.position += transform.forward * MoveSpeed * Time.deltaTime;
		
		//Max range
		var distance = Vector3.Distance(InitialPosition, transform.position);
		if (distance > ProjectileRange) Destroy (gameObject);
}


function OnCollisionEnter(collision : Collision) {


	if (collision.gameObject.tag == "skeleton") {

			//Create a Ragdoll
			var MaRagdoll = GameObject.Instantiate(Ragdoll, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
			MaRagdoll.name = "Rag" + Character_Script.score;
			
			//Apply a force to the ragdoll
			var Impact : Vector3 = ImpactForce * transform.forward + Vector3(0,UpForce,0);
			var MonPointImpact : GameObject = MaRagdoll.Find("/" + MaRagdoll.name + "/biped");
			MonPointImpact.rigidbody.AddForce(Impact);
			
			//Projectile destruction
			Destroy (gameObject);

	}
}

