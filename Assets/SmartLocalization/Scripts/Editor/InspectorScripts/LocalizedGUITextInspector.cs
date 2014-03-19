//
// LocalizedGUITextInspector.cs
// 
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GUIText))]
public class LocalizedGUITextInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedGUIText textObject = ((GUIText)target).gameObject.GetComponent<LocalizedGUIText>();
		if(textObject != null)
		{
			selectedKey = textObject.localizedKey;
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
		
		if(!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
		{
			GameObject targetGameObject = ((GUIText)target).gameObject;
			LocalizedGUIText textObject = targetGameObject.GetComponent<LocalizedGUIText>();
			if(textObject == null)
			{
				textObject = targetGameObject.AddComponent<LocalizedGUIText>();
			}
			
			textObject.localizedKey = selectedKey;
		}
	}
}
