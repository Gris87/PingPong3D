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
    private Rect              saveDialogRect;
    private GUIStyle          saveTextStyle;

    private State currentState;
    private int   currentItem;
    private int   itemsCount;
    private bool  modified;
    private bool  askSaving;

    #region Options
    private static string language="en";
    #endregion

    #region Localization
    private string localizationGame;
    private string localizationSound;
    private string localizationVideo;
    private string localizationControls;
    private string localizationBack;
    private string localizationLanguage;
    private string localizationSettingsChanged;
    private string localizationDoYouWantToSaveChanges;
    private string localizationOK;
    private string localizationCancel;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Localization
        LanguageManager languageManager=LanguageManager.Instance;
        languageManager.OnChangeLanguage+=OnChangeLanguage;
        OnChangeLanguage(languageManager);
        #endregion

        #region Create text styles
        saveTextStyle=new GUIStyle();

        saveTextStyle.alignment=TextAnchor.UpperLeft;
        saveTextStyle.wordWrap=true;
        saveTextStyle.clipping=TextClipping.Clip;
        saveTextStyle.fontSize=(int)(Screen.height*0.04);
        saveTextStyle.normal.textColor=Color.white;
        #endregion


        List<CultureInfo> availableLanguages=languageManager.AvailableLanguagesCultureInfo;
        string[] languages=new string[availableLanguages.Count];

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            languages[i]=availableLanguages[i].EnglishName;
        }

        languageScroller=new SelectionScroller(languages, 0, settingsModified, scrollerLeftTexture, scrollerRightTexture);



        modified  = false;
        askSaving = false;

        goToOptionsList(0);
    }

    void OnChangeLanguage(LanguageManager languageManager)
    {
        localizationGame                   = languageManager.GetTextValue("OptionsScene.Game");
        localizationSound                  = languageManager.GetTextValue("OptionsScene.Sound");
        localizationVideo                  = languageManager.GetTextValue("OptionsScene.Video");
        localizationControls               = languageManager.GetTextValue("OptionsScene.Controls");
        localizationBack                   = languageManager.GetTextValue("OptionsScene.Back");
        localizationLanguage               = languageManager.GetTextValue("OptionsScene.Language");
        localizationSettingsChanged        = languageManager.GetTextValue("OptionsScene.SettingsChanged");
        localizationDoYouWantToSaveChanges = languageManager.GetTextValue("OptionsScene.DoYouWantToSaveChanges");
        localizationOK                     = languageManager.GetTextValue("OptionsScene.OK");
        localizationCancel                 = languageManager.GetTextValue("OptionsScene.Cancel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();
        }
        else
        if (!Utils.isTouchDevice && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentItem>0)
            {
                --currentItem;
            }
        }
        else
        if (!Utils.isTouchDevice && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentItem<itemsCount-1)
            {
                ++currentItem;
            }
        }
        else
        if (
            !Utils.isTouchDevice
            &&
            (
             Input.GetKeyDown(KeyCode.Return)
             ||
             Input.GetKeyDown(KeyCode.KeypadEnter)
            )
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
        if (askSaving)
        {
            GUIStyle windowStyle=new GUIStyle(GUI.skin.window);
            windowStyle.fontSize=(int)(Screen.height*0.05);

            saveDialogRect=GUI.ModalWindow(0, saveDialogRect, drawSaveDialog, localizationSettingsChanged, windowStyle);
        }

        menuItemStyle.fontSize=(int)(Screen.height*0.05);
        menuSelectedItemStyle.fontSize=(int)(Screen.height*0.05);
        
        float panelWidth  = Screen.width*0.9f;
        float panelHeight = Screen.height*0.9f;
        float rowHeight   = Screen.height*0.1f;
        float rowOffset   = rowHeight+Screen.height*0.025f;
        
        
        
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

    private void drawSaveDialog(int id)
    {
        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize=(int)(Screen.height*0.03);

        GUI.Label(new Rect(saveDialogRect.width*0.05f, saveDialogRect.height*0.25f, saveDialogRect.width*0.9f, saveDialogRect.height*0.4f), localizationDoYouWantToSaveChanges, saveTextStyle);

        if (GUI.Button(new Rect(saveDialogRect.width*0.1f, saveDialogRect.height*0.7f, saveDialogRect.width*0.3f, saveDialogRect.height*0.2f), localizationOK, buttonStyle))
        {
            applyChanges();
            goBack();
        }

        if (GUI.Button(new Rect(saveDialogRect.width*0.6f, saveDialogRect.height*0.7f, saveDialogRect.width*0.3f, saveDialogRect.height*0.2f), localizationCancel, buttonStyle))
        {
            modified  = false;
            askSaving = false;

            goBack();
        }
    }

    private void drawOptionsList(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (drawButton(localizationGame, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;

        if (drawButton(localizationSound, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;

        if (drawButton(localizationVideo, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;

        if (drawButton(localizationControls, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawGameOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.35f, rowHeight), localizationLanguage,  (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        languageScroller.draw(new Rect(panelWidth*0.4f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawSoundOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawVideoOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawControlsOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private bool drawButton(string text, float panelWidth, float panelHeight, float rowHeight, float rowOffset, int cur)
    {
        return GUI.Button(new Rect(0, rowOffset*cur, panelWidth*0.95f, rowHeight), text, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
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
        // Nothing
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

    private void settingsModified()
    {
        modified=true;
    }

    private void goBack()
    {
        if (currentState==State.InOptionsList)
        {
            Debug.Log("Go to game menu");

            save();

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
        if (modified)
        {
            askSaving=!askSaving;

            if (askSaving)
            {
                saveDialogRect=new Rect(Screen.width*0.3f, Screen.height*0.3f, Screen.width*0.4f, Screen.height*0.4f);
            }
        }
        else
        {
            Debug.Log("Go to options list");
            
            scrollPosition = Vector2.zero;
            currentState   = State.InOptionsList;
            currentItem    = index;
            itemsCount     = 5;
        }
    }

    private void goToGameOptions()
    {
        Debug.Log("Go to game options");

        scrollPosition = Vector2.zero;
        currentState   = State.InGameOptions;
        currentItem    = 0;
        itemsCount     = 2;

        updateLanguageScroller();
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

    public void applyChanges()
    {
        modified  = false;
        askSaving = false;
    
        switch(currentState)
        {
            case State.InOptionsList:
                Debug.LogError("Never reach this code");
            break;
            case State.InGameOptions:     applyChangesInGameOptions    (); break;
            case State.InSoundOptions:    applyChangesInSoundOptions   (); break;
            case State.InVideoOptions:    applyChangesInVideoOptions   (); break;
            case State.InControlsOptions: applyChangesInControlsOptions(); break;
            default:
                Debug.LogError("Unknown state");
            break;
        }

        save();
    }
    
    private void applyChangesInGameOptions()
    {
        LanguageManager languageManager=LanguageManager.Instance;

        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        int languageIndex=languageScroller.getCurrentIndex();

        if (languageIndex<0 || languageIndex>=availableLanguages.Count)
        {
            Debug.LogError("Array index out of bounds");
            return;
        }

        string selectedLanguage=availableLanguages[languageIndex].Name;

        if (!language.Equals(selectedLanguage))
        {
            language=selectedLanguage;
            languageManager.ChangeLanguage(language);
        }
    }
    
    private void applyChangesInSoundOptions()
    {
    }
    
    private void applyChangesInVideoOptions()
    {
    }
    
    private void applyChangesInControlsOptions()
    {
    }

    private void updateLanguageScroller()
    {
        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        int languageIndex=0;
        
        for (int i=0; i<availableLanguages.Count; ++i)
        {            
            if (language.Equals(availableLanguages[i].Name))
            {
                languageIndex=i;
                break;
            }
        }
        
        languageScroller.setCurrentIndex(languageIndex);
    }

    public static void load()
    {
        Debug.Log("Loading settings");
        IniFile iniFile=new IniFile("Settings");

        LanguageManager languageManager=LanguageManager.Instance;
        language=iniFile.Get("Language", languageManager.GetSystemLanguage());

        if (!languageManager.IsLanguageSupported(language))
        {
            language="en";
        }

        Debug.Log("Application language: "+language);
        languageManager.ChangeLanguage(language);
    }

    public static void save()
    {
        IniFile iniFile=new IniFile("Settings");
        
        iniFile.Set("Language", language, "Application language");

        iniFile.save("Settings");
    }
}
