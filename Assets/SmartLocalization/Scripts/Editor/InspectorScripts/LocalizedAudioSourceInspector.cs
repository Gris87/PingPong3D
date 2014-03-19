//
// LocalizedAudioSourceInspector.cs
// 
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AudioSource))]
public class LocalizedAudioSourceInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedAudioSource audioObject = ((AudioSource)target).gameObject.GetComponent<LocalizedAudioSource>();
		if(audioObject != null)
		{
			selectedKey = audioObject.localizedKey;
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.AUDIO);
		
		if(!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
		{
			GameObject targetGameObject = ((AudioSource)target).gameObject;
			LocalizedAudioSource audioObject = targetGameObject.GetComponent<LocalizedAudioSource>();
			if(audioObject == null)
			{
				audioObject = targetGameObject.AddComponent<LocalizedAudioSource>();
			}
			
			audioObject.localizedKey = selectedKey;
		}
	}
}
