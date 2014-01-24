using UnityEngine;
using System.Collections;

public class SwitchDemo : MonoBehaviour 
{
	public Transform triggersRoot;			// A root GameObject to place the triggers in (organizatio)
	public Transform triggerSpawnPoint;		// The place where we spawn example triggers
	public GameObject[] exampleTriggers;	// An array of example triggers to show off
	GameObject currentTrigger;				// The trigger currently being demoed

	void OnGUI ()
	{
		DisplayInstructions();
		DisplayExampleTriggers();
	}

	void DisplayInstructions ()
	{
		GUILayout.Label("1. Select a trigger to test. \n2. Press 'e' to test the trigger.");
	}

	void DisplayExampleTriggers ()
	{
		foreach (var trigger in exampleTriggers) {
			if (GUILayout.Button(trigger.name, GUILayout.Width(150))) {
				SwitchExampleTrigger(trigger);
			}
		}
	}

	void SwitchExampleTrigger (GameObject trigger)
	{
		if (currentTrigger != null) {
			DestroyImmediate(currentTrigger);
		}

		currentTrigger = Instantiate(trigger, triggerSpawnPoint.position, Quaternion.identity) as GameObject;
		currentTrigger.transform.parent = triggersRoot;
	}
}	
