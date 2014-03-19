//
// LocalizedAudioSource.cs
//
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;

public class LocalizedAudioSource : MonoBehaviour 
{
	public string localizedKey = "INSERT_KEY_HERE";
	public AudioClip thisAudioClip;
	private AudioSource thisAudioSource;
	
	void Start () 
	{
		//Subscribe to the change language event
		LanguageManager thisLanguageManager = LanguageManager.Instance;
		thisLanguageManager.OnChangeLanguage += OnChangeLanguage;
		
		//Get the audio source
		thisAudioSource = this.audio;
		
		//Run the method one first time
		OnChangeLanguage(thisLanguageManager);
	}
	
	void OnChangeLanguage(LanguageManager thisLanguageManager)
	{
		//Initialize all your language specific variables here
		thisAudioClip = thisLanguageManager.GetAudioClip(localizedKey);
		
		if(thisAudioSource != null)
		{
			thisAudioSource.clip = thisAudioClip;
		}
	}
}
