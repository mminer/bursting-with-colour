// Copyright (c) 2013 Brad Keys. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

[CustomEditor(typeof(SwitchTrigger))]
[CanEditMultipleObjects]
/// <summary>
/// A custom inspector for the Switch Trigger system.
/// </summary>
public class SwitchTriggerInspector : Editor 
{
	/// <summary>
	/// The indent used for UI alignment.
	/// </summary>
	float indent = 120;
	bool showActions = true;
	SwitchTrigger trigger;

	const string actionsLabel = 			"Actions";
	const string optionsLabel = 			"Options";
	const string addActionLabel = 			"Add Action";
	const string removeActionLabel = 		"Remove Action";
	const string componentDefaultName = 	"GameObject";
	const string targetLabel = 				"Target";
	const string tagLabel = 				"Permitted Tag";
	const string activateActionLabel = 		"Activated By";
	const string triggerActionLabel = 		"Action";
	const string timeDelayLabel = 			"Time Delay";
	const string secondsLabel = 			"Seconds";
	const string buttonKeyLabel = 			"Key";
	const string delayActionLabel = 		"After Time Delay";
	const string repeatableLabel = 			"Repeatable";
	const string activateAnotherLabel = 	"Activate Other Trigger";
	const string selectFunctionLabel = 		"Function";
	const string spawnPointLabel = 			"Spawn Point";
	const string spawnActionLabel = 		"Spawned Object Action";
	const string animClipLabel = 			"Animation Clip";
	const string animSpeedLabel = 			"Animation Speed";
	const string animRepeatLabel = 			"Repeat Animation";

	const string selectGameObjectWarning = 	"Choose a target GameObject to continue";
	const string selectComponentWarning = 	"Choose a component from the target GameObject";
	const string invalidAnimationWarning = 	"Target component must be an Animation.";
	const string invalidAudioWarning = 		"Target component must be an AudioSource.";

	/// <summary>
	/// Raises the inspector GUI event.
	/// </summary>
	override public void OnInspectorGUI () 
	{
		init ();

		DisplayOptions();
		DisplayActions();
		
		// Refresh the UI when something is changed
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}

	/// <summary>
	/// Initializes the GUI and GameObject.
	/// </summary>
	void init ()
	{
		trigger = target as SwitchTrigger;
		
		if (trigger.actions.Count == 0) {
			trigger.CreateAction();
		}
		
		// Add a BoxCollider set to be a trigger, when appropriate.
		if (trigger.collider == null) {
			if (trigger.triggeredBy != SwitchTrigger.TriggerType.None && trigger.triggeredBy != SwitchTrigger.TriggerType.Start) {
				trigger.gameObject.AddComponent(typeof(BoxCollider));
				trigger.collider.isTrigger = true;
				Debug.LogWarning("[Switch] The object " + trigger.gameObject.name + " had no collider attached, so we attached a box collider.");
			}
		}
	}

	void DisplayOptions ()
	{
		SelectActivatedByTag();
		SelectActivationAction();
		SelectTriggerOnce();
		SelectTimeDelay();
		SelectAnotherTrigger();
	}

	void DisplayActions ()
	{
		showActions = EditorGUILayout.Foldout(showActions, actionsLabel);

		// Hide this content if not folded
		if (!showActions) {
			return;
		}

		if (GUILayout.Button(addActionLabel, EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
			trigger.CreateAction();
		}

		GUILayout.Space(5);
		
		var actions = trigger.actions.ToArray();
		
		foreach (var act in actions) {
			SelectTarget(act);
			SelectTriggerAction(act);
			DisplayRemoveActionButton(act);
			GUILayout.Space(5);
		}
	}

	void DisplayRemoveActionButton (SwitchAction act)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(String.Empty, GUILayout.Width(indent));
			
			if (GUILayout.Button(removeActionLabel, EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
				trigger.RemoveAction(act);
			}
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Selects a target GameObject and Component to be manipulated.
	/// </summary>
	void SelectTarget (SwitchAction act)
	{
		SelectTargetGameObject(act);
		SelectTargetComponent(act);
	}

	/// <summary>
	/// Selects the target game object.
	/// </summary>
	void SelectTargetGameObject (SwitchAction act)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(targetLabel, GUILayout.Width(indent));
			act.targetGameObject = (GameObject)EditorGUILayout.ObjectField(act.targetGameObject , typeof(GameObject), true, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Selects the target component.
	/// </summary>
	void SelectTargetComponent (SwitchAction act)
	{
		if (act.targetGameObject != null) {
			if (act.components == null) {
				act.components = act.targetGameObject.GetComponents<Component>();
			}
		}
		
		if (act.components != null && act.components.Length > 0) {
			if (act.componentNames == null) {
				act.componentNames = new string[act.components.Length + 1];
				act.componentNames[0] = componentDefaultName;
				act.types = new Type[act.components.Length];
				
				for (int c = 0; c < act.components.Length; c++) {
					var t = act.components[c].GetType();
					act.types[c] = t;
					act.componentNames[c + 1] = t.Name;
				}
			}

			GUILayout.BeginHorizontal();
				GUILayout.Label(String.Empty, GUILayout.Width(indent));
				act.selectedComponentIndex = EditorGUILayout.Popup(act.selectedComponentIndex, act.componentNames, GUILayout.Width(indent));
			GUILayout.EndHorizontal();

			if (act.selectedComponentIndex <= 0) {
				act.targetComponent = null;
			} else {
				act.targetComponent = act.components[act.selectedComponentIndex - 1];
			}
		}
	}

	/// <summary>
	/// Selects the activated by tag that has permission to activate this trigger.
	/// </summary>
	void SelectActivatedByTag ()
	{
		// Choose who can activate the trigger
		if (trigger.tags == null) {
			trigger.tags = GetTags();
		} else if (trigger.tags.Length != UnityEditorInternal.InternalEditorUtility.tags.Length + 1) {
			trigger.tags = GetTags();
		}
		
		GUILayout.BeginHorizontal();
			GUILayout.Label(tagLabel, GUILayout.Width(indent));
			trigger.selectedTagIndex = EditorGUILayout.Popup(trigger.selectedTagIndex, trigger.tags, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Selects the action that activates this trigger.
	/// </summary>
	void SelectActivationAction ()
	{
		// Choose how the trigger is activated
		GUILayout.BeginHorizontal();
			GUILayout.Label(activateActionLabel, GUILayout.Width(indent));
			trigger.selectedTriggeredByIndex = EditorGUILayout.Popup(trigger.selectedTriggeredByIndex, SwitchTrigger.triggeredByNames, GUILayout.Width(indent));
		GUILayout.EndHorizontal();

		switch(SwitchTrigger.triggeredByNames[trigger.selectedTriggeredByIndex]) {
			case SwitchTrigger.enterTriggerLabel:		trigger.triggeredBy = SwitchTrigger.TriggerType.OnTriggerEnter;	break;
			case SwitchTrigger.stayTriggerLabel: 		trigger.triggeredBy = SwitchTrigger.TriggerType.OnTriggerStay; 	break;
			case SwitchTrigger.sceneTriggerLabel: 		trigger.triggeredBy = SwitchTrigger.TriggerType.Start; 			break;
			case SwitchTrigger.exitingTriggerLabel: 	trigger.triggeredBy = SwitchTrigger.TriggerType.OnTriggerExit; 	break;
			case SwitchTrigger.buttonTriggerLabel:		trigger.triggeredBy = SwitchTrigger.TriggerType.Button;			break;
			case SwitchTrigger.noneTriggerLabel: 		trigger.triggeredBy = SwitchTrigger.TriggerType.None; 			break;
		}

		// Display button select
		if (trigger.triggeredBy == SwitchTrigger.TriggerType.Button) {
			SelectButtonPress();
		}
	}
	
	/// <summary>
	/// Selects the action this trigger performs.
	/// </summary>
	void SelectTriggerAction (SwitchAction act)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(triggerActionLabel, GUILayout.Width(indent));
			act.selectedActionIndex = EditorGUILayout.Popup(act.selectedActionIndex, SwitchTrigger.action, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
		
		// Additional options, depending on action choice
		switch (SwitchTrigger.action[act.selectedActionIndex]) {
			case SwitchTrigger.functionActionLabel:		SelectFunction(act); 				break;
			case SwitchTrigger.destroyActionLabel:		SelectDestroy();					break;
			case SwitchTrigger.spawnActionLabel:		SelectSpawnObject(act);				break;
			case SwitchTrigger.playAnimActionLabel:		SelectAnimationAction(act); 		break;
			case SwitchTrigger.stopAnimActionLabel:		SelectStopAnimationAction(act);		break;
			case SwitchTrigger.pauseAnimActionLabel: 	SelectAnimationAction(act); 		break;
			case SwitchTrigger.resumeAnimActionLabel:	SelectAnimationAction(act);			break;
			case SwitchTrigger.playSoundActionLabel:	SelectSoundAction(act); 			break;
		}
	}

	#region Trigger Actions

	/// <summary>
	/// Selects the key used in the button press activator.
	/// </summary>
	void SelectButtonPress ()
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(buttonKeyLabel, GUILayout.Width(indent));
			trigger.inputKeyName = EditorGUILayout.TextField(trigger.inputKeyName, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
	}
	
	void SelectFunction (SwitchAction act)
	{
		if (act.targetGameObject == null) {
			ShowWarning(selectGameObjectWarning);
			return;
		} else if (act.targetComponent == null) {
			ShowWarning(selectComponentWarning);
			return;
		}
		
		if (act.callableMethods == null) {
			var type = act.types[act.selectedComponentIndex - 1];
			var methods = type.GetMethods();
			var callableMethods = new List<string>();
			
			foreach (var method in methods) {
				// Skip inherited methods
				if (method.Module.Name == "UnityEngine.dll") {
					continue;
				}
				
				// Skip functions that require parameters 
				if (method.GetParameters().Length > 0) {
					continue;	
				}
				
				// Skip getters
				if (method.IsSpecialName && method.Name.StartsWith("get_")) {
					continue;	
				}
				
				callableMethods.Add(method.Name);
			}
			
			act.callableMethods = callableMethods.ToArray();
		}
		
		if (act.callableMethods.Length == 0) {
			return;
		}
		
		GUILayout.BeginHorizontal();					
			GUILayout.Label(selectFunctionLabel, GUILayout.Width(indent));
			act.selectedFunctionIndex = EditorGUILayout.Popup(act.selectedFunctionIndex, act.callableMethods, GUILayout.Width(indent));
			act.functionName = act.callableMethods[act.selectedFunctionIndex];
		GUILayout.EndHorizontal();
	}
	
	void SelectDestroy ()
	{
		// Prevents trying to destroy something that has already been destroyed
		trigger.repeatable = false;
	}
	
	void SelectSpawnObject (SwitchAction act)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(spawnPointLabel, GUILayout.Width(indent));
			act.spawnPoint = (Transform)EditorGUILayout.ObjectField(act.spawnPoint, typeof(Transform), true, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
	}

	void SelectAnimationAction (SwitchAction act)
	{
		Animation anim;
		
		if (act.targetComponent is Animation) {
			anim = act.targetComponent as Animation;
		} else {
			ShowWarning(invalidAnimationWarning);
			return;
		}
		
		act.animations = GetAnimationNames(anim);
		
		GUILayout.BeginHorizontal();
			GUILayout.Label(animClipLabel, GUILayout.Width(indent));
			act.selectedAnimationClipIndex = EditorGUILayout.Popup(act.selectedAnimationClipIndex, act.animations, GUILayout.Width(indent));
		GUILayout.EndHorizontal();

		string action = SwitchTrigger.action[act.selectedActionIndex];

		if (action == SwitchTrigger.playAnimActionLabel || action == SwitchTrigger.resumeAnimActionLabel) {
			GUILayout.BeginHorizontal();
				GUILayout.Label(animSpeedLabel, GUILayout.Width(indent));
				act.animationSpeed = EditorGUILayout.FloatField(act.animationSpeed,  GUILayout.Width(indent));
			GUILayout.EndHorizontal();
		}

		if (action == SwitchTrigger.playAnimActionLabel) {
			GUILayout.BeginHorizontal();
				GUILayout.Label(animRepeatLabel, GUILayout.Width(indent));
				act.loopAnimation = EditorGUILayout.Toggle(act.loopAnimation, GUILayout.Width(25));
			GUILayout.EndHorizontal();
		}
	}

	void SelectStopAnimationAction (SwitchAction act)
	{
		if (!(act.targetComponent is Animation)) {
			ShowWarning(invalidAnimationWarning);
			return;
		}
	}

	void SelectSoundAction (SwitchAction act)
	{
		if (!(act.targetComponent is AudioSource)) {
			ShowWarning(invalidAudioWarning);
		}
	}

	#endregion

	/// <summary>
	/// Shows the time delay checkbox and delay time.
	/// </summary>
	void SelectTimeDelay ()
	{
		// Time related events
		GUILayout.BeginHorizontal();
			GUILayout.Label(timeDelayLabel, GUILayout.Width(indent));
			trigger.delayAction = EditorGUILayout.Toggle(trigger.delayAction, GUILayout.Width(25));
			
			if (trigger.delayAction) {
				GUILayout.Label(secondsLabel, GUILayout.Width(75));
				trigger.timeDelay = EditorGUILayout.FloatField(trigger.timeDelay,  GUILayout.Width(40));
			}
		GUILayout.EndHorizontal();
		
		// Behaviour of time delay
		SelectTimeDelayBehaviour();
	}

	/// <summary>
	/// Selects the time delay behaviour.
	/// </summary>
	void SelectTimeDelayBehaviour ()
	{
		if (trigger.delayAction) {
			GUILayout.BeginHorizontal();
				GUILayout.Label(delayActionLabel, GUILayout.Width(indent));
				trigger.selectedDelayedActionIndex = EditorGUILayout.Popup(trigger.selectedDelayedActionIndex, SwitchTrigger.timeBehaviours, GUILayout.Width(indent));
			GUILayout.EndHorizontal();
		}
	}

	/// <summary>
	/// Selects the trigger once option.
	/// </summary>
	void SelectTriggerOnce ()
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(repeatableLabel, GUILayout.Width(indent));
			trigger.repeatable = EditorGUILayout.Toggle(trigger.repeatable, GUILayout.Width(25));
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Selects another trigger option.
	/// </summary>
	void SelectAnotherTrigger ()
	{
		// Option to activate another trigger
		GUILayout.BeginHorizontal();
			GUILayout.Label(activateAnotherLabel, GUILayout.Width(indent));
			trigger.nextTrigger = (SwitchTrigger)EditorGUILayout.ObjectField(trigger.nextTrigger, typeof(SwitchTrigger), true, GUILayout.Width(indent));
		GUILayout.EndHorizontal();
	}

	#region Helper Methods
	
	/// <summary>
	/// Gets the tags.
	/// </summary>
	/// <returns>
	/// The tags.
	/// </returns>
	string[] GetTags ()
	{
		var newTags = UnityEditorInternal.InternalEditorUtility.tags;
		var combined = new List<string>();
		
		combined.Add("All Objects");
		
		foreach (var newTag in newTags) {
			combined.Add(newTag);
		}
		
		return combined.ToArray();
	}
	
	/// <summary>
	/// Gets the animation names.
	/// </summary>
	/// <returns>
	/// The animation names.
	/// </returns>
	/// <param name='anim'>
	/// Animation.
	/// </param>
	string[] GetAnimationNames (Animation anim) 
	{
		List<string> animations = new List<string>();
		
		foreach (AnimationState a in anim) {
			animations.Add(a.name);
		}

		animations.Sort();
		
		return animations.ToArray();
	}
	
	/// <summary>
	/// Shows a warning message.
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	void ShowWarning (string message)
	{
		var color = GUI.skin.label.normal.textColor;
		GUI.skin.label.normal.textColor = Color.red;
		GUILayout.Label(message);
		GUI.skin.label.normal.textColor = color;
	}

	#endregion
}