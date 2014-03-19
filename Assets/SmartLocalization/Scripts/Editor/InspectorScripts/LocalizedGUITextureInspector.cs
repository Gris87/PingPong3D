//
// LocalizedGUITextureInspector.cs
// 
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GUITexture))]
public class LocalizedGUITextureInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedGUITexture textObject = ((GUITexture)target).gameObject.GetComponent<LocalizedGUITexture>();
		if(textObject != null)
		{
			selectedKey = textObject.localizedKey;
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.TEXTURE);
		
		if(!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
		{
			GameObject targetGameObject = ((GUITexture)target).gameObject;
			LocalizedGUITexture textObject = targetGameObject.GetComponent<LocalizedGUITexture>();
			if(textObject == null)
			{
				textObject = targetGameObject.AddComponent<LocalizedGUITexture>();
			}
			
			textObject.localizedKey = selectedKey;
		}
	}
}
