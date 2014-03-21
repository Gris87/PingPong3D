//
// LocalizedGUIText.cs
//
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEngine;
using System.Collections;

public class LocalizedGUIText : MonoBehaviour
{
    public string localizedKey = "INSERT_KEY_HERE";

    void Start ()
    {
        //Subscribe to the change language event
        LanguageManager thisLanguageManager = LanguageManager.Instance;
        thisLanguageManager.OnChangeLanguage += OnChangeLanguage;

        //Run the method one first time
        OnChangeLanguage(thisLanguageManager);
    }

    void OnDestroy()
    {
        LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }

    void OnChangeLanguage(LanguageManager thisLanguageManager)
    {
        //Initialize all your language specific variables here
        this.guiText.text = LanguageManager.Instance.GetTextValue(localizedKey);
    }
}
