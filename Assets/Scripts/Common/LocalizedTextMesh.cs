using UnityEngine;
using System.Collections;

public class LocalizedTextMesh : MonoBehaviour
{
    public string token;

    private TextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        textMesh=GetComponent<TextMesh>();

        if (textMesh!=null)
        {
            //Subscribe to the change language event
            LanguageManager languageManager = LanguageManager.Instance;
            languageManager.OnChangeLanguage += OnChangeLanguage;

            //Run the method one first time
            OnChangeLanguage(languageManager);
        }
    }

    void OnChangeLanguage(LanguageManager languageManager)
    {
        //Initialize all your language specific variables here
        textMesh.text=languageManager.GetTextValue(token);
    }
}
