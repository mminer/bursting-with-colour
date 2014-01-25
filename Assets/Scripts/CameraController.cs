using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public bool continuousCameraMove = true;
	public float cameraSpeed = 0.5f;
	public float delayBeforeStart = 5f;
	public bool followTopPlayer = false;
	public float followPlayerMargin = 0f;

	public Transform player1;
	public Transform player2;

	private bool moving = false;
	private float farthestPlayer;

	void Start ()
	{
		if (continuousCameraMove)
			StartMoving();
	}

	IEnumerator DelayBeforeCamerMove(float delay)
	{
		yield return new WaitForSeconds(delay);
		moving = true;
	}

	public void StartMoving()
	{
		if (moving == false)
			StartCoroutine(DelayBeforeCamerMove(delayBeforeStart));
	}

	void Update ()
	{
		if (continuousCameraMove && moving)
		{
			transform.Translate(0f, cameraSpeed * Time.deltaTime, 0f);
		}
		else if (followTopPlayer)
		{
			UpdateFarthestPlayer ();
			if ((transform.position.y + followPlayerMargin) < farthestPlayer)
				transform.position = new Vector3(transform.position.x, farthestPlayer, transform.position.z);
		}
	}

	void UpdateFarthestPlayer()
	{
		if (player1 != null)
			if (player1.transform.position.y > farthestPlayer)
				farthestPlayer = player1.transform.position.y;
		if (player2 != null)
			if (player2.transform.position.y > farthestPlayer)
				farthestPlayer = player2.transform.position.y;
//		Debug.Log ("FartherPlayer Y is "+ farthestPlayer);
	}
}
