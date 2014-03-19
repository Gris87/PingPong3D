//
//  LoadAllLanguages.cs
//
// This class will load all languages and show all the keys/values
//
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class LoadAllLanguages : MonoBehaviour 
{
	private Dictionary<string,string> currentLanguageValues;
	private LanguageManager thisLanguageManager;
	private Vector2 valuesScrollPosition = Vector2.zero;
	private Vector2 languagesScrollPosition = Vector2.zero;

	void Start () 
	{
		thisLanguageManager = LanguageManager.Instance;
		
		string systemLanguage = thisLanguageManager.GetSystemLanguage();
		if(thisLanguageManager.IsLanguageSupported(systemLanguage))
		{
			thisLanguageManager.ChangeLanguage(systemLanguage);	
		}
		
		if(thisLanguageManager.AvailableLanguages.Count > 0)
		{
			currentLanguageValues = thisLanguageManager.GetTextDataBase();	
		}
		else
		{
			Debug.LogError("No languages are created!, Open the Smart Localization plugin at Window->Smart Localization and create your language!");
		}
	}
	
	void OnGUI() 
	{
		if(thisLanguageManager.IsInitialized)
		{
			GUILayout.Label("Current Language:" + thisLanguageManager.language);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Keys:", GUILayout.Width(460));
			GUILayout.Label("Values:", GUILayout.Width(460));
			GUILayout.EndHorizontal();
			
			valuesScrollPosition = GUILayout.BeginScrollView(valuesScrollPosition);
			foreach(KeyValuePair<string,string> languageValue in currentLanguageValues)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(languageValue.Key, GUILayout.Width(460));
				GUILayout.Label(languageValue.Value, GUILayout.Width(460));
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			
			languagesScrollPosition = GUILayout.BeginScrollView (languagesScrollPosition);
#if !UNITY_WP8
			foreach(CultureInfo language in thisLanguageManager.AvailableLanguagesCultureInfo)
			{
				if(GUILayout.Button(language.NativeName, GUILayout.Width(960)))
				{
					thisLanguageManager.ChangeLanguage(language.Name);
					currentLanguageValues = thisLanguageManager.GetTextDataBase();
				}
			}
#else
			foreach(string language in thisLanguageManager.AvailableLanguages)
			{
				if(GUILayout.Button(language, GUILayout.Width(960)))
				{
					thisLanguageManager.ChangeLanguage(language);
					currentLanguageValues = thisLanguageManager.GetTextDataBase();
				}
			}
#endif
			GUILayout.EndScrollView();
		}
	}
}
