// Copyright (c) 2013 Brad Keys. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An advanced Unity trigger system.
/// </summary>
public class SwitchTrigger : MonoBehaviour 
{
	/// <summary>
	/// The actions this trigger will perform.
	/// </summary>
	public List<SwitchAction> actions = new List<SwitchAction>();

	/// <summary>
	/// The method in which this trigger becomes active.
	/// </summary>
	public TriggerType triggeredBy;
	
	/// <summary>
	/// Methods to activate this trigger.
	/// </summary>
	public enum TriggerType { OnTriggerEnter, OnTriggerStay, Button, Start, OnTriggerExit, None };
	
	/// <summary>
	/// A time delay before this trigger performs its action, once it has been activated.
	/// </summary>
	public float timeDelay;
	
	/// <summary>
	/// Whether or not this trigger is a delayed action.
	/// </summary>
	public bool delayAction = false;
	
	/// <summary>
	/// Whether or not this trigger can only be activated once in its lifetime.
	/// </summary>
	public bool repeatable = true;

	/// <summary>
	/// The name of the input key that will activate this trigger on button press.
	/// </summary>
	public string inputKeyName = "";

	/// <summary>
	/// A tag that has permission to activate this trigger.
	/// </summary>
	public string[] tags;

	public const string enterTriggerLabel = "Entering Trigger";
	public const string stayTriggerLabel = "Staying On Trigger";
	public const string exitingTriggerLabel = "Exiting Trigger";
	public const string buttonTriggerLabel = "Button Press";
	public const string sceneTriggerLabel = "On Awake";
	public const string noneTriggerLabel = "None";
	
	/// <summary>
	/// The triggered by method names. Used in the custom inspector.
	/// </summary>
	[System.NonSerialized]
	public static string[] triggeredByNames = new string[] {	enterTriggerLabel, 
																stayTriggerLabel, 
																exitingTriggerLabel,
																buttonTriggerLabel, 
																sceneTriggerLabel,
																noneTriggerLabel
															};

	public const string enableActionLabel = "Enable";
	public const string disableActionLabel = "Disable";
	public const string toggleActionLabel = "Toggle (Enable or Disable)";
	public const string destroyActionLabel = "Destroy";
	public const string functionActionLabel = "Call Function";
	public const string spawnActionLabel = "Spawn Object";
	public const string playAnimActionLabel = "Play Animation";
	public const string stopAnimActionLabel = "Stop Animation";
	public const string pauseAnimActionLabel = "Pause Animation";
	public const string resumeAnimActionLabel = "Resume Animation";
	public const string playSoundActionLabel = "Play Sound";

	/// <summary>
	/// The action names. Used in the custom inspector.
	/// </summary>
	public static string[] action = new string[] { 	enableActionLabel, 
													disableActionLabel, 
													toggleActionLabel,
													destroyActionLabel, 
													functionActionLabel, 
													spawnActionLabel, 
													playAnimActionLabel,
													stopAnimActionLabel,
													pauseAnimActionLabel,
													resumeAnimActionLabel,
													playSoundActionLabel
												};

	const string delayActivateTriggerLabel = "Activate This Trigger";
	const string delayDeactivateTriggerLabel = "Deactivate This Trigger";
	const string delayReplayTriggerLabel = "Replay This Trigger";
	const string delayActivateOtherLabel = "Activate Other Trigger";

	/// <summary>
	/// The delayed actions that can be performed. Used in the custom inspector.
	/// </summary>
	[System.NonSerialized]
	public static string[] timeBehaviours = new string[] {	delayActivateTriggerLabel, 
															delayDeactivateTriggerLabel,
															delayReplayTriggerLabel,
															delayActivateOtherLabel
												 		};
	public SwitchTrigger nextTrigger;
	
	#region indexes for custom inspector drop-down selections
	public int selectedTriggeredByIndex; 	// Index of selected trigger-by method
	public int selectedDelayedActionIndex; 	// Index of selected time-delay action
	public int selectedTagIndex; 			// Index of the selected tag
	#endregion

	bool isActive;
	bool hasTriggered;

	public static SwitchTrigger instance;
	
	public SwitchTrigger () {}
	
	/// <summary>
	/// Runs when this instance is initialized.
	/// </summary>
	void Awake () 
	{
		if (instance == null) {
			instance = this;
		}

		// Throw a warning if trying to use a trigger without a collider
		if (collider == null && triggeredBy != TriggerType.None && triggeredBy != TriggerType.Start) {
			Debug.LogWarning("The object " + gameObject.name + " is trying to use a trigger but does not have a collider attached.");
		}
		
		// Ensure the collider is set to isTrigger = true, when appropriate
		if (collider != null && !collider.isTrigger) {
			if (triggeredBy != TriggerType.None && triggeredBy != TriggerType.Start) {
				collider.isTrigger = true;	
			}
		}
	}
	
	/// <summary>
	/// Runs afer the Awake function, during initialization.
	/// </summary>
	void Start () 
	{
		if (triggeredBy == TriggerType.Start) {
			Activate();
		}
	}
	
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name='other'>
	/// Other.
	/// </param>
	void OnTriggerEnter (Collider other) 
	{
		if (HasPermissionToActivate(other.tag)) {
			if (triggeredBy == TriggerType.OnTriggerEnter || triggeredBy == TriggerType.OnTriggerStay) {
				Activate();
			}
		}
	}
	
	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name='other'>
	/// Other.
	/// </param>
	void OnTriggerStay (Collider other) 
	{
		if (HasPermissionToActivate(other.tag)) {
			if (triggeredBy == TriggerType.Button) {
				isActive = true;
			}
		}
	}
	
	/// <summary>
	/// Checks for player input to activate this trigger.
	/// </summary>
	void OnGUI () 
	{
		if (triggeredBy == TriggerType.Button && isActive) {
			Event e = Event.current;

			if (e.isKey) {				
				string key = e.character.ToString().ToLower();

				if (key == inputKeyName.ToLower()) {
					if (Input.GetKeyDown(key)) {
						Activate();
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name='other'>
	/// Other.
	/// </param>
	void OnTriggerExit (Collider other) 
	{
		if (HasPermissionToActivate(other.tag)) {
			if (triggeredBy == TriggerType.OnTriggerStay || triggeredBy == TriggerType.Button) {
				Deactivate();
			} else if (triggeredBy == TriggerType.OnTriggerExit) {
				Activate();
			}
		}
	}

	public void ActivateAfterDelay (float delay)
	{
		Invoke("Activate", delay);
	}
	
	/// <summary>
	/// Activates this trigger.
	/// </summary>
	public void Activate () 
	{
		if (hasTriggered && !repeatable) {
			return;
		}

		foreach (var act in actions) {
			act.DetermineTarget();
		}

		hasTriggered = true;
		
		if (enabled) {
			isActive = true;
			
			if (delayAction) {
				switch (timeBehaviours[selectedDelayedActionIndex]) {
					case delayActivateTriggerLabel:
						Invoke("DoActions", timeDelay);
						break;
					case delayDeactivateTriggerLabel:
						Invoke("Deactivate", timeDelay);
						break;
					case delayReplayTriggerLabel:
						DoActions();
						Invoke("Activate", timeDelay);
						break;
					default:
						DoActions();
						break;
				}
			} else {
				DoActions();
			}
		}
	}
	
	/// <summary>
	/// Deactivates this trigger (only affects button presses).
	/// </summary>
	public void Deactivate () 
	{
		isActive = false;
	}
	
	/// <summary>
	/// Determines whether the object attempting to activate this trigger has permission.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has permission; otherwise, <c>false</c>.
	/// </returns>
	bool HasPermissionToActivate (string tag)
	{
		bool permission = (tags[selectedTagIndex] == "All Objects" || tag == tags[selectedTagIndex]);		
		return permission;
	}
	
	/// <summary>
	/// Performs this trigger's actions.
	/// </summary>
	void DoActions ()
	{
		foreach (var act in actions) {
			act.DoAction(action[act.selectedActionIndex]);
		}

		if (nextTrigger != null) {
			// Check if a time delay was specified before activating the next trigger
			if (delayAction && timeBehaviours[selectedDelayedActionIndex] == delayActivateOtherLabel) {
				nextTrigger.ActivateAfterDelay(timeDelay);
			} else {
				nextTrigger.Activate();
			}
		}
	}

	public void CreateAction ()
	{
		actions.Add(new SwitchAction());
	}

	public void RemoveAction (SwitchAction act)
	{
		actions.Remove(act);
	}

	public static Coroutine RunCoroutine (IEnumerator coroutine)
	{
		return instance.StartCoroutine(coroutine);
	}
}