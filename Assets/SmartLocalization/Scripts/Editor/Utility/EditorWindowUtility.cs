//
//  EditorWindowUtility.cs
//
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using UnityEditor;

public class EditorWindowUtility
{
#region Show / Hide Smart Localization Windows
	/// <summary>
	/// Returns true if the window should show, returns false if not.
	/// If false, it will draw an editor label that says the window is unavailable
	/// </summary>
	public static bool ShowWindow()
	{
		if(Application.isPlaying)
		{
			GUILayout.Label ("Smart Localization is not available in play mode", EditorStyles.boldLabel);
			return false;
		}
		else
		{
			return true;
		}
	}
#endregion
}