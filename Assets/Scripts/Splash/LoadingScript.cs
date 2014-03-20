using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        guiText.fontSize=(int)(Screen.height*0.1);

        Options.load();

        SceneManager.LoadScene("GameMenu");
	}
}
