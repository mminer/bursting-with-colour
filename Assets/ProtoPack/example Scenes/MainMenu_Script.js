
var ButtonSound : AudioClip;
var Background : Texture2D;

function OnGUI () {

		//Background
		GUI.DrawTexture (Rect(0,0,800,450), Background);
		
		//Additional information
		GUI.Label ( Rect(50,100,290,200), "The ProtoPack 3D provides all the art pieces you need to create quickly any prototype using Unity3D. Objects, textures, scenes, scripts and sounds of this demo (and a lot more) are include in the ProtoPack 3D.");
		
		//Version
		GUI.Label ( Rect(10,425,100,20), "v 1.3");
		
		//Animations Button
		if(GUI.Button ( Rect(100,220,200,20), "Animations Demo Scene")) {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (4);
		}
		
		//Car Demo Button
		if(GUI.Button ( Rect(100,250,200,20), "Car Demo Scene")) {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (1);
		}
		
		
		//Plane Demo Button
		if(GUI.Button ( Rect(100,280,200,20), "Plane Demo Scene")) {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (2);
		}
		
		//Survival Demo
		if(GUI.Button ( Rect(100,310,200,20), "Character Demo Scene")) {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (3);
		}
		
		//Ragdoll Demo
		if(GUI.Button ( Rect(100,340,200,20), "Ragdoll Demo Scene")) {
		
				audio.PlayOneShot(ButtonSound);
				Application.LoadLevel (5);
		}

}

