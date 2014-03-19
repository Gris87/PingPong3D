using UnityEngine;
using System.Collections;

public class LocalizationScript : MonoBehaviour
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
            LanguageManager thisLanguageManager = LanguageManager.Instance;
            thisLanguageManager.OnChangeLanguage += OnChangeLanguage;
            
            //Run the method one first time
            OnChangeLanguage(thisLanguageManager);
        }
	}	

    void OnChangeLanguage(LanguageManager thisLanguageManager)
    {
        //Initialize all your language specific variables here
        textMesh.text=LanguageManager.Instance.GetTextValue(token);
    }
}
