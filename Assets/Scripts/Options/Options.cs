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

    public GUIStyle menuItemStyle;
    public GUIStyle menuSelectedItemStyle;

    private Vector2 scrollPosition;

    private State currentState;
    private int   currentItem;
    private int   itemsCount;

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

        goToOptionsList();
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
        menuItemStyle.fontSize=(int)(Screen.height*0.05);
        menuSelectedItemStyle.fontSize=(int)(Screen.height*0.05);

        float panelWidth  = Screen.width*0.9f;
        float panelHeight = Screen.height*0.9f;
        float rowHeight=Screen.height*0.1f;
        float rowOffset=rowHeight+Screen.height*0.025f;


        
        GUI.BeginGroup(new Rect(Screen.width*0.05f, Screen.height*0.05f, panelWidth, panelHeight));
        GUI.Box(new Rect(0, 0, panelWidth-1, panelHeight-1), "");

        scrollPosition=GUI.BeginScrollView(new Rect(panelWidth*0.01f, panelHeight*0.01f, panelWidth*0.98f, panelHeight*0.98f), scrollPosition, new Rect(0, 0, panelWidth*0.95f, rowHeight+(itemsCount-1)*rowOffset));

        switch(currentState)
        {
            case State.InOptionsList:     drawOptionsList    (panelWidth, panelHeight, rowHeight, rowOffset); break;
            case State.InGameOptions:     drawGameOptions    (panelWidth, panelHeight, rowHeight, rowOffset); break;
            case State.InSoundOptions:    drawSoundOptions   (panelWidth, panelHeight, rowHeight, rowOffset); break;
            case State.InVideoOptions:    drawVideoOptions   (panelWidth, panelHeight, rowHeight, rowOffset); break;
            case State.InControlsOptions: drawControlsOptions(panelWidth, panelHeight, rowHeight, rowOffset); break;
            default:
                Debug.LogError("Unknown state");
            break;
        }

        GUI.EndScrollView();

        GUI.EndGroup();
    }

    private void drawOptionsList(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;
        
        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Game",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goToGameOptions();
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Sound",    currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goToSoundOptions();
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Video",    currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goToVideoOptions();
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Controls", currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goToControlsOptions();
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Back",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goBack();
        }
    }

    private void drawGameOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;
        
        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Back",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goBack();
        }
    }

    private void drawSoundOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;
        
        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Back",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goBack();
        }
    }

    private void drawVideoOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;
        
        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Back",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goBack();
        }
    }

    private void drawControlsOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;
        
        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), "Back",     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            goBack();
        }
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
            goToOptionsList();
        }
    }

    private void goToOptionsList()
    {
        Debug.Log("Go to options list");
        
        scrollPosition = Vector2.zero;
        currentState   = State.InOptionsList;
        currentItem    = 0;
        itemsCount     = 5;
    }

    private void goToGameOptions()
    {
        Debug.Log("Go to game options");
        
        scrollPosition = Vector2.zero;
        currentState   = State.InGameOptions;
        currentItem    = 0;
        itemsCount     = 0;
    }

    private void goToSoundOptions()
    {
        Debug.Log("Go to sound options");
        
        scrollPosition = Vector2.zero;
        currentState   = State.InSoundOptions;
        currentItem    = 0;
        itemsCount     = 0;
    }

    private void goToVideoOptions()
    {
        Debug.Log("Go to video options");
        
        scrollPosition = Vector2.zero;
        currentState   = State.InVideoOptions;
        currentItem    = 0;
        itemsCount     = 0;
    }

    private void goToControlsOptions()
    {
        Debug.Log("Go to controls options");
        
        scrollPosition = Vector2.zero;
        currentState   = State.InControlsOptions;
        currentItem    = 0;
        itemsCount     = 0;
    }
}
