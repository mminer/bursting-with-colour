using UnityEngine;
using System.Collections;

[System.Serializable]
public class SwitchAction 
{
	/// <summary>
	/// A target object that is ultimately affected by actions of this trigger.
	/// </summary>
	Object objectToTrigger;
	
	[SerializeField] GameObject _targetGameObject;
	
	/// <summary>
	/// The target game object used to define objectToTrigger
	/// </summary>
	public GameObject targetGameObject 
	{
		get {
			return _targetGameObject;
		}
		set {
			// Reset indexes and selected values when changing target GameObject
			if (value != _targetGameObject) {
				components = null;
				componentNames = null;
				types = null;
				selectedComponentIndex = 0;
				selectedAnimationClipIndex = 0;
			}

			_targetGameObject = value;
		}
	}
	
	[SerializeField] Component _targetComponent;
	
	/// <summary>
	/// The target component that is optionally used to define objectToTrigger.
	/// </summary>
	public Component targetComponent 
	{
		get {
			return _targetComponent;
		}
		set {
			// Reset indexes and selected values when changing target Component
			if (value != _targetComponent) {
				callableMethods = null;
				selectedFunctionIndex = 0;
			}

			_targetComponent = value;
		}
	}
	
	/// <summary>
	/// Names of a GameObject's components, used for display.
	/// </summary>
	public string[] componentNames;

	/// <summary>
	/// The components on the target GameObject.
	/// </summary>
	public Component[] components;

	// Index of the selected Component
	public int selectedComponentIndex;

	// Index of selected action
	public int selectedActionIndex;
	
	#region Function Calling

	int _selectedFunctionIndex;
	
	public int selectedFunctionIndex {
		get {
			return _selectedFunctionIndex;
		}
		set {
			if (callableMethods == null) {
				SetSelectedFunctionIndex(0);
				return;
			}

			// Index didn't change
			if (value == _selectedFunctionIndex) {
				// Store the method name for sanity check reference
				if (selectedFunctionName == null || selectedFunctionName.Length == 0) {
					selectedFunctionName = callableMethods[_selectedFunctionIndex];
				// Check if the index is the same, but the method  name has changed
				} else {
					// We lost the method name!
					if (callableMethods[_selectedFunctionIndex] != selectedFunctionName) {
						// See if we can find its new index, the function might have been moved in the script
						for (int i = 0; i < callableMethods.Length; i++) {
							if (callableMethods[i] == selectedFunctionName) {
								SetSelectedFunctionIndex(i);
								return;
							}
						}
						
						// Unable to find the method name
						SetSelectedFunctionIndex(0);
						Debug.LogWarning("[Switch Trigger] GameObject:" + targetGameObject.name + brokenFunctionWarning);
					} 
				}
				// New index
			} else {
				SetSelectedFunctionIndex(value);
			}
		}
	}

	void SetSelectedFunctionIndex (int index)
	{
		_selectedFunctionIndex = index;

		if (callableMethods == null) {
			selectedFunctionName = string.Empty;
		} else {
			selectedFunctionName = callableMethods[index];
		}
	}
	
	// Sanity check reference
	string selectedFunctionName;

	/// <summary>
	/// A public function that can be called from the target objectToTrigger.
	/// </summary>
	public string functionName;

	/// <summary>
	///  The types of the components listed for the target GameObject. Used for invoking methods.
	/// </summary>
	public System.Type[] types;

	public string[] callableMethods;

	const string brokenFunctionWarning = " - A function used in a trigger was renamed or removed, breaking the connection.";

	#endregion

	#region Spawning Objects

	Object spawnedObject;

	/// <summary>
	/// If this trigger spawns a new object, this is a reference to its trigger and is activated once spawned.
	/// </summary>
	public SwitchTrigger spawnedObjectAction;
	
	/// <summary>
	/// If this trigger spawns a new object, this is a reference to where it will spawn.
	/// </summary>
	public Transform spawnPoint;

	#endregion

	#region Animations

	public int selectedAnimationClipIndex; 	// Index of selected animation clip

	/// <summary>
	/// Animation names the target objectToTrigger contains.
	/// </summary>
	public string[] animations;
	
	/// <summary>
	/// Whether a played animation should loop.
	/// </summary>
	public bool loopAnimation;
	
	/// <summary>
	/// The animation speed.
	/// </summary>
	public float animationSpeed = 1f;
	
	/// <summary>
	/// A reference to the original animation speed.
	/// </summary>
	public float pausedAnimationSpeed = 1f;

	#endregion

	public void DetermineTarget ()
	{
		// Determine if we are targeting a GameObject or Component
		if (targetComponent != null) {
			objectToTrigger = targetComponent;
		} else {
			objectToTrigger = targetGameObject;
		}
	}

	public void DoAction (string actionName)
	{
		// It's possible the object no longer exists
		if (objectToTrigger == null) {
			return;
		}

		// Make sure Animations have at least 1 clip
		if (objectToTrigger is Animation && !HasAnimationClips()) {
			return;
		}

		switch (actionName) {
			case SwitchTrigger.enableActionLabel: 		DoEnable(true); 		break;
			case SwitchTrigger.disableActionLabel: 		DoEnable(false);		break;
			case SwitchTrigger.toggleActionLabel:		DoToggleEnable();		break;
			case SwitchTrigger.spawnActionLabel:		DoSpawnObject();		break;
			case SwitchTrigger.functionActionLabel:		DoCallFunction();  		break;
			case SwitchTrigger.destroyActionLabel: 		DoDestroy();			break;
			case SwitchTrigger.playAnimActionLabel:		DoPlayAnimation();		break;
			case SwitchTrigger.stopAnimActionLabel:		DoStopAnimation();		break;
			case SwitchTrigger.pauseAnimActionLabel: 	DoPauseAnimation();		break;
			case SwitchTrigger.resumeAnimActionLabel:	DoResumeAnimation();	break;
			case SwitchTrigger.playSoundActionLabel:	DoPlaySound();			break;
		}
	}

	void DoEnable (bool enable)
	{
		if (objectToTrigger is MonoBehaviour) {
			(objectToTrigger as MonoBehaviour).enabled = enable;
		} else if (objectToTrigger is GameObject) {
			(objectToTrigger as GameObject).SetActive(enable);
		}  else if (objectToTrigger is Behaviour) {
			(objectToTrigger as Behaviour).enabled = enable;
		}
	}

	void DoToggleEnable ()
	{
		if (objectToTrigger is GameObject) {
			var go = objectToTrigger as GameObject;
			go.SetActive(!go.activeSelf);
		} else if (objectToTrigger is MonoBehaviour) {
			var mono = objectToTrigger as MonoBehaviour;
			mono.enabled = !mono.enabled;
		} else if (objectToTrigger is Behaviour) {
			var behav = objectToTrigger as Behaviour;
			behav.enabled = !behav.enabled;
		}
	}
	
	void DoSpawnObject ()
	{
		GameObject.Instantiate(objectToTrigger, spawnPoint.position, spawnPoint.rotation);
	}
	
	void DoCallFunction ()
	{
		if (objectToTrigger is MonoBehaviour) {
			(objectToTrigger as MonoBehaviour).Invoke(functionName, 0.0f);
		}  else if (objectToTrigger is Component) {
			(objectToTrigger as Component).BroadcastMessage(functionName);
		}
	}
	
	void DoDestroy ()
	{
		GameObject.Destroy(objectToTrigger);
	}
	
	void DoPlayAnimation () 
	{
		Animation anim = objectToTrigger as Animation;
		SetAnimationSpeed(anim, animationSpeed);
		anim.clip = anim[animations[selectedAnimationClipIndex]].clip;
		SwitchTrigger.RunCoroutine(StartAnimation(anim));
	}

	bool HasAnimationClips ()
	{
		// Make sure Animations have at least 1 clip
		if (objectToTrigger is Animation) {
			var anim = objectToTrigger as Animation;
			
			if (anim.GetClipCount() == 0) {
				return false;
			}
		}

		return true;
	}
	
	void DoStopAnimation ()
	{
		(objectToTrigger as Animation).Stop();
	}
	
	void DoPauseAnimation ()
	{
		PauseAnimation(objectToTrigger as Animation, true);
	}
	
	
	/// <summary>
	/// Resumes the animation.
	/// </summary>
	void DoResumeAnimation () 
	{
		pausedAnimationSpeed = animationSpeed;
		PauseAnimation(objectToTrigger as Animation, false);
	}
	
	void DoPlaySound ()
	{
		var sound = objectToTrigger as AudioSource;
		sound.Play();
	}
	
	#region Helper Functions
	
	/// <summary>
	/// Starts the animation.
	/// </summary>
	/// <returns>
	/// The animation.
	/// </returns>
	IEnumerator StartAnimation (Animation anim) 
	{
		if (anim == null) {
			return false;
		}

		anim.Play();
		
		if (loopAnimation) {
			yield return new WaitForSeconds(anim.clip.length);
			SwitchTrigger.RunCoroutine(StartAnimation(anim));
		}
	}
	
	/// <summary>
	/// Pauses or resumes an animation.
	/// </summary>
	/// <param name='anim'>
	/// Animation.
	/// </param>
	/// <param name='pause'>
	/// Pause.
	/// </param>
	void PauseAnimation (Animation anim, bool pause)
	{		
		// Pause
		if (pause) {
			pausedAnimationSpeed = anim[animations[selectedAnimationClipIndex]].speed;
			SetAnimationSpeed(anim, 0);
			// Resume
		} else {
			SetAnimationSpeed(anim, pausedAnimationSpeed);
		}
	}
	
	
	void SetAnimationSpeed(Animation anim, float speed)
	{
		anim[animations[selectedAnimationClipIndex]].speed = speed;
	}
	
	#endregion
}
