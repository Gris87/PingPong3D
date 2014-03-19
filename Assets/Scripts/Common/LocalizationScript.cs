using UnityEngine;
using System.Collections;

public class LocalizationScript : MonoBehaviour
{
    public string token;

	// Use this for initialization
	void Start ()
    {
        TextMesh textMesh=GetComponent<TextMesh>();

        if (textMesh!=null)
        {
            textMesh.text=LanguageManager.Instance.GetTextValue(token);
        }
	}	
}
