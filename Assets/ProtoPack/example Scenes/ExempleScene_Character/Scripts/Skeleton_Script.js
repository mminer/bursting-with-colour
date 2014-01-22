
var Player : Transform;

private var NbrSpawnPoint : int = 12;
private var MoveSpeed : float = 0.3;
private var RotationSpeed : float = 0.5;




function Update () 
{
		//Moving the character
		transform.position += transform.forward * MoveSpeed * Time.deltaTime;
		
		//Stand up
		transform.rotation.x = 0;
		transform.rotation.z = 0;
		
		//Face the player
		MyDestination = Player.position;
		transform.rotation = Quaternion.Slerp(transform.rotation , Quaternion.LookRotation(MyDestination - transform.position), RotationSpeed * Time.deltaTime);
}


function OnCollisionEnter(collision : Collision) {


		if (collision.gameObject.tag == "projectile") {
		
				//increase the score
				Character_Script.score += 1;

				//Move the character to a random spawn point
				SP = Random.Range(1,NbrSpawnPoint+1);
				var MySP : GameObject = GameObject.Find("/SpawnPoints/SpawPoint" + SP);
				transform.position = MySP.transform.position;
				transform.rotation = MySP.transform.rotation;
		}

}











