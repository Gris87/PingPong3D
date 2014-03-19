using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour
{
    private enum State
    {
        InOptionsList,
        InGameOptions,
        InSoundOptions,
        InVideoOptions,
        InControlsOptions
    }

    private State currentState=State.InOptionsList;

    #region Localization
    private string localizationBack;
    #endregion
    
    // Use this for initialization
    void Start ()
    {
        #region Localization
        LanguageManager languageManager=LanguageManager.Instance;
        
        localizationBack = languageManager.GetTextValue("OptionsScene.Back");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();
        }
    }
    
    void OnGUI()
    {
        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize=(int)(Screen.height*0.03);
        
        if (GUI.Button(new Rect(Screen.width*0.05f, Screen.height*0.05f, Screen.width*0.2f, Screen.height*0.1f), localizationBack, buttonStyle))
        {
            goBack();
        }

        float panelWidth  = Screen.width*0.9f;
        float panelHeight = Screen.height*0.75f;
        
        GUI.BeginGroup(new Rect(Screen.width*0.05f, Screen.height*0.2f, panelWidth, panelHeight));
        GUI.Box(new Rect(0, 0, panelWidth-1, panelHeight-1), "");

        switch(currentState)
        {
            case State.InOptionsList:     drawOptionsList    (panelWidth, panelHeight); break;
            case State.InGameOptions:     drawGameOptions    (panelWidth, panelHeight); break;
            case State.InSoundOptions:    drawSoundOptions   (panelWidth, panelHeight); break;
            case State.InVideoOptions:    drawVideoOptions   (panelWidth, panelHeight); break;
            case State.InControlsOptions: drawControlsOptions(panelWidth, panelHeight); break;
            default:
                Debug.LogError("Unknown state");
            break;
        }

        GUI.EndGroup();
    }

    private void drawOptionsList(float panelWidth, float panelHeight)
    {
    }

    private void drawGameOptions(float panelWidth, float panelHeight)
    {
    }

    private void drawSoundOptions(float panelWidth, float panelHeight)
    {
    }

    private void drawVideoOptions(float panelWidth, float panelHeight)
    {
    }

    private void drawControlsOptions(float panelWidth, float panelHeight)
    {
    }

    private void goBack()
    {
        if (currentState==State.InOptionsList)
        {
            Debug.Log("Go to game menu");
            
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            Debug.Log("Go to options list");
            
            currentState=State.InOptionsList;
        }
    }
}
