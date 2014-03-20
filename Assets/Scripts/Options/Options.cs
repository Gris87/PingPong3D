using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

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

    public GUIStyle  menuItemStyle;
    public GUIStyle  menuSelectedItemStyle;
    public Texture2D scrollerLeftTexture;
    public Texture2D scrollerRightTexture;

    private Vector2           scrollPosition;
    private SelectionScroller languageScroller;

    private State currentState;
    private int   currentItem;
    private int   itemsCount;

    #region Options
    private string language;
    #endregion

    #region Localization
    private string localizationGame;
    private string localizationSound;
    private string localizationVideo;
    private string localizationControls;
    private string localizationBack;
    private string localizationLanguage;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Localization
        LanguageManager languageManager=LanguageManager.Instance;

        localizationGame     = languageManager.GetTextValue("OptionsScene.Game");
        localizationSound    = languageManager.GetTextValue("OptionsScene.Sound");
        localizationVideo    = languageManager.GetTextValue("OptionsScene.Video");
        localizationControls = languageManager.GetTextValue("OptionsScene.Controls");
        localizationBack     = languageManager.GetTextValue("OptionsScene.Back");
        localizationLanguage = languageManager.GetTextValue("OptionsScene.Language");
        #endregion



        List<CultureInfo> availableLanguages=languageManager.AvailableLanguagesCultureInfo;
        string[] languages=new string[availableLanguages.Count];

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            languages[i]=availableLanguages[i].EnglishName;
        }

        languageScroller=new SelectionScroller(languages, language, scrollerLeftTexture, scrollerRightTexture);



        goToOptionsList(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();
        }
        else
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentItem>0)
            {
                --currentItem;
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentItem<itemsCount-1)
            {
                ++currentItem;
            }
        }
        else
        if (
            Input.GetKeyDown(KeyCode.Return)
            ||
            Input.GetKeyDown(KeyCode.KeypadEnter)
           )
        {
            selectItem(currentItem);
        }
        else
        {
            controlItem(currentItem);
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

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationGame,     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationSound,    currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationVideo,    currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationControls, currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationBack,     currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }
    }

    private void drawGameOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.35f, rowHeight), localizationLanguage,  currentItem==cur ? menuSelectedItemStyle : menuItemStyle);
        languageScroller.draw(new Rect(panelWidth*0.4f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationBack, currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }
    }

    private void drawSoundOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationBack, currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }
    }

    private void drawVideoOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationBack, currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }
    }

    private void drawControlsOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), localizationBack, currentItem==cur ? menuSelectedItemStyle : menuItemStyle))
        {
            selectItem(cur);
        }
    }

    private void controlItem(int index)
    {
        switch(currentState)
        {
            case State.InOptionsList:     controlItemInOptionsList    (index); break;
            case State.InGameOptions:     controlItemInGameOptions    (index); break;
            case State.InSoundOptions:    controlItemInSoundOptions   (index); break;
            case State.InVideoOptions:    controlItemInVideoOptions   (index); break;
            case State.InControlsOptions: controlItemInControlsOptions(index); break;
            default:
                Debug.LogError("Unknown state");
            break;
        }
    }

    private void controlItemInOptionsList(int index)
    {
        // Nothing
    }

    private void controlItemInGameOptions(int index)
    {
        switch(index)
        {
            case 0: languageScroller.control(); break;
            case 1:
                // Nothing
            break;
            default:
                Debug.LogError("Don't know how to handle it");
            break;
        }
    }

    private void controlItemInSoundOptions(int index)
    {
    }

    private void controlItemInVideoOptions(int index)
    {
    }

    private void controlItemInControlsOptions(int index)
    {
    }

    private void selectItem(int index)
    {
        if (index<0 || index>=itemsCount-1)
        {
            goBack();
            return;
        }

        switch(currentState)
        {
            case State.InOptionsList:     selectItemInOptionsList    (index); break;
            case State.InGameOptions:     selectItemInGameOptions    (index); break;
            case State.InSoundOptions:    selectItemInSoundOptions   (index); break;
            case State.InVideoOptions:    selectItemInVideoOptions   (index); break;
            case State.InControlsOptions: selectItemInControlsOptions(index); break;
            default:
                Debug.LogError("Unknown state");
            break;
        }
    }

    private void selectItemInOptionsList(int index)
    {
        switch(index)
        {
            case 0: goToGameOptions();     break;
            case 1: goToSoundOptions();    break;
            case 2: goToVideoOptions();    break;
            case 3: goToControlsOptions(); break;
            default:
                Debug.LogError("Don't know how to handle it");
            break;
        }
    }

    private void selectItemInGameOptions(int index)
    {
    }

    private void selectItemInSoundOptions(int index)
    {
    }

    private void selectItemInVideoOptions(int index)
    {
    }

    private void selectItemInControlsOptions(int index)
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
            switch(currentState)
            {
                case State.InGameOptions:     goToOptionsList(0); break;
                case State.InSoundOptions:    goToOptionsList(1); break;
                case State.InVideoOptions:    goToOptionsList(2); break;
                case State.InControlsOptions: goToOptionsList(3); break;
                default:
                    Debug.LogError("Unknown state");
                break;
            }
        }
    }

    private void goToOptionsList(int index)
    {
        Debug.Log("Go to options list");

        scrollPosition = Vector2.zero;
        currentState   = State.InOptionsList;
        currentItem    = index;
        itemsCount     = 5;
    }

    private void goToGameOptions()
    {
        Debug.Log("Go to game options");

        scrollPosition = Vector2.zero;
        currentState   = State.InGameOptions;
        currentItem    = 0;
        itemsCount     = 2;
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
