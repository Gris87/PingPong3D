using UnityEngine;
using System;
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
    public Texture2D checkboxOnTexture;
    public Texture2D checkboxOffTexture;

    private Vector2             scrollPosition;
    private Rect                saveDialogRect;
    private GUIStyle            saveTextStyle;
    private ControlSetter       selectedControlSetter;
    private SelectionScroller   languageScroller;
    private BigHorizontalSlider masterVolumeSlider;
    private BigHorizontalSlider musicVolumeSlider;
    private BigHorizontalSlider effectsVolumeSlider;
    private BigCheckBox         showFPSCheckBox;
    private SelectionScroller   qualityScroller;
    private SelectionScroller   resolutionScroller;
    private BigCheckBox         fullScreenCheckBox;
    private List<ControlSetter> controlSetters;

    private State currentState;
    private int   currentItem;
    private int   itemsCount;
    private bool  modified;
    private bool  askSaving;

    #region Options

    #region Language
    private static string mLanguage="en";
    public static string language
    {
        get
        {
            return mLanguage;
        }
    }
    #endregion

    #region Master volume
    private static float mMasterVolume=1;
    public static float masterVolume
    {
        get
        {
            return mMasterVolume;
        }
    }
    #endregion

    #region Music volume
    private static float mMusicVolume=1;
    public static float musicVolume
    {
        get
        {
            return mMusicVolume;
        }
    }
    #endregion

    #region Effects volume
    private static float mEffectsVolume=1;
    public static float effectsVolume
    {
        get
        {
            return mEffectsVolume;
        }
    }
    #endregion
    
    #region Show FPS
    private static bool mShowFPS=false;
    public static bool showFPS
    {
        get
        {
            return mShowFPS;
        }
    }
    #endregion

    #region Quality
    private static int mQuality=QualitySettings.GetQualityLevel();
    public static int quality
    {
        get
        {
            return mQuality;
        }
    }
    #endregion

    #region Resolution
    private static string mResolution="800x600";
    public static string resolution
    {
        get
        {
            return mResolution;
        }
    }
    #endregion
    
    #region Full screen
    private static bool mFullScreen=true;
    public static bool fullScreen
    {
        get
        {
            return mFullScreen;
        }
    }
    #endregion

    #region Keys
    public static List<InputControl.KeyMapping> keys
    {
        get
        {
            return InputControl.getKeys();
        }
    }
    #endregion

    #endregion

    #region Localization
    private string       localizationGame;
    private string       localizationSound;
    private string       localizationVideo;
    private string       localizationControls;
    private string       localizationBack;
    private string       localizationLanguage;
    private string       localizationMasterVolume;
    private string       localizationMusicVolume;
    private string       localizationEffectsVolume;
    private string       localizationShowFPS;
    private string       localizationQuality;
    private string       localizationResolution;
    private string       localizationFullScreen;
    private List<string> localizationKeys;
    private string       localizationSettingsChanged;
    private string       localizationDoYouWantToSaveChanges;
    private string       localizationOK;
    private string       localizationCancel;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Create text styles
        saveTextStyle=new GUIStyle();

        saveTextStyle.alignment=TextAnchor.UpperLeft;
        saveTextStyle.wordWrap=true;
        saveTextStyle.clipping=TextClipping.Clip;
        saveTextStyle.normal.textColor=Color.white;
        #endregion

        #region Game
        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        string[] languages=new string[availableLanguages.Count];

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            languages[i]=availableLanguages[i].EnglishName;
        }

        languageScroller=new SelectionScroller(languages, 0, scrollerLeftTexture, scrollerRightTexture);
        languageScroller.setModifiedFunction(settingsModified);
        #endregion

        #region Sound
        masterVolumeSlider  = new BigHorizontalSlider(mMasterVolume,  0f, 1f);
        musicVolumeSlider   = new BigHorizontalSlider(mMusicVolume,   0f, 1f);
        effectsVolumeSlider = new BigHorizontalSlider(mEffectsVolume, 0f, 1f);

        masterVolumeSlider.setModifiedFunction (settingsModified);
        musicVolumeSlider.setModifiedFunction  (settingsModified);
        effectsVolumeSlider.setModifiedFunction(settingsModified);
        #endregion

        #region Video
        showFPSCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        showFPSCheckBox.setModifiedFunction(settingsModified);

        qualityScroller=new SelectionScroller(null, 0, scrollerLeftTexture, scrollerRightTexture);
        qualityScroller.setModifiedFunction(settingsModified);

        Resolution[] availableResolutions=Screen.resolutions;
        string[] resolutions=new string[availableResolutions.Length];
        
        for (int i=0; i<availableResolutions.Length; ++i)
        {
            Resolution resolution=availableResolutions[i];
            
            resolutions[i]=resolution.width.ToString()+"x"+resolution.height.ToString()+" : "+resolution.refreshRate.ToString()+" Hz";
        }
        
        resolutionScroller=new SelectionScroller(resolutions, 0, scrollerLeftTexture, scrollerRightTexture);
        resolutionScroller.setModifiedFunction(settingsModified);
        
        fullScreenCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        fullScreenCheckBox.setModifiedFunction(settingsModified);
        #endregion

        #region Controls
        selectedControlSetter=null;
        controlSetters=new List<ControlSetter>();

        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            controlSetters.Add(new ControlSetter());
        }

        localizationKeys=new List<string>();
        #endregion

        #region Localization
        LanguageManager languageManager=LanguageManager.Instance;
        languageManager.OnChangeLanguage+=OnChangeLanguage;
        OnChangeLanguage(languageManager);
        #endregion

        modified  = false;
        askSaving = false;

        goToOptionsList(0);
    }

    void OnChangeLanguage(LanguageManager languageManager)
    {
        #region Localization
        localizationGame                   = languageManager.GetTextValue("OptionsScene.Game");
        localizationSound                  = languageManager.GetTextValue("OptionsScene.Sound");
        localizationVideo                  = languageManager.GetTextValue("OptionsScene.Video");
        localizationControls               = languageManager.GetTextValue("OptionsScene.Controls");
        localizationBack                   = languageManager.GetTextValue("OptionsScene.Back");
        localizationLanguage               = languageManager.GetTextValue("OptionsScene.Language");
        localizationMasterVolume           = languageManager.GetTextValue("OptionsScene.MasterVolume");
        localizationMusicVolume            = languageManager.GetTextValue("OptionsScene.MusicVolume");
        localizationEffectsVolume          = languageManager.GetTextValue("OptionsScene.EffectsVolume");
        localizationShowFPS                = languageManager.GetTextValue("OptionsScene.ShowFPS");
        localizationQuality                = languageManager.GetTextValue("OptionsScene.Quality");
        localizationResolution             = languageManager.GetTextValue("OptionsScene.Resolution");
        localizationFullScreen             = languageManager.GetTextValue("OptionsScene.FullScreen");
        localizationSettingsChanged        = languageManager.GetTextValue("OptionsScene.SettingsChanged");
        localizationDoYouWantToSaveChanges = languageManager.GetTextValue("OptionsScene.DoYouWantToSaveChanges");
        localizationOK                     = languageManager.GetTextValue("OptionsScene.OK");
        localizationCancel                 = languageManager.GetTextValue("OptionsScene.Cancel");



        string[] qualities=QualitySettings.names;

        for (int i=0; i<qualities.Length; ++i)
        {
            qualities[i]                   = languageManager.GetTextValue("OptionsScene."+qualities[i]);
        }

        qualityScroller.setItems(qualities);



        localizationKeys.Clear();

        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            localizationKeys.Add(languageManager.GetTextValue("OptionsScene."+key.name));
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedControlSetter!=null)
        {
            if (InputControl.GetKeyDown(KeyCode.Escape))
            {
                selectedControlSetter.setSelectedKey(KeyCode.None);
                selectedControlSetter=null;

                modified=true;
            }
        }
        else
        {
            if (InputControl.GetKeyDown(KeyCode.Escape))
            {
                goBack();
            }
            else
            if (InputControl.GetKeyDown(KeyCode.UpArrow))
            {
                if (!askSaving && currentItem>0)
                {
                    --currentItem;
                }
            }
            else
            if (InputControl.GetKeyDown(KeyCode.DownArrow))
            {
                if (!askSaving && currentItem<itemsCount-1)
                {
                    ++currentItem;
                }
            }
            else
            if (
                InputControl.GetKeyDown(KeyCode.Return)
                ||
                InputControl.GetKeyDown(KeyCode.KeypadEnter)
               )
            {
                if (askSaving)
                {
                    applyChanges();
                    goBack();
                }
                else
                {
                    selectItem(currentItem);
                }
            }
            else
            {
                if (!askSaving)
                {
                    controlItem(currentItem);
                }
            }
        }
    }

    void OnGUI()
    {
        if (selectedControlSetter!=null && Event.current.type!=EventType.Repaint && Event.current.type!=EventType.Layout)
        {
            Event.current.Use();
        }

        if (askSaving)
        {
            GUIStyle windowStyle=new GUIStyle(GUI.skin.window);
            windowStyle.fontSize=(int)(Screen.height*0.045);

            saveDialogRect=GUI.ModalWindow(0, saveDialogRect, drawSaveDialog, localizationSettingsChanged, windowStyle);
        }

        menuItemStyle.fontSize=(int)(Screen.height*0.05);
        menuSelectedItemStyle.fontSize=(int)(Screen.height*0.05);

        float boxWidth    = Screen.width*0.9f;
        float boxHeight   = Screen.height*0.9f;
        float panelWidth  = boxWidth*0.93f;
        float panelHeight = boxHeight*0.98f;
        float rowHeight   = Screen.height*0.1f;
        float rowOffset   = rowHeight+Screen.height*0.025f;



        GUI.BeginGroup(new Rect(Screen.width*0.05f, Screen.height*0.05f, boxWidth, boxHeight));
        GUI.Box(new Rect(0, 0, boxWidth-1, boxHeight-1), "");

        scrollPosition=GUI.BeginScrollView(new Rect(boxWidth*0.02f, boxHeight*0.01f, boxWidth*0.96f, panelHeight), scrollPosition, new Rect(0, 0, panelWidth, rowHeight+(itemsCount-1)*rowOffset));

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

        saveTextStyle.fontSize=(int)(Screen.height*0.04);



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

        if (!Utils.isTouchDevice)
        {
            ++cur;
            
            if (drawButton(localizationControls, panelWidth, panelHeight, rowHeight, rowOffset, cur))
            {
                selectItem(cur);
            }
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

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationLanguage, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        languageScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawSoundOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationMasterVolume, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        masterVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationMusicVolume,  (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        musicVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationEffectsVolume, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        effectsVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawVideoOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationShowFPS,    (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        showFPSCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));
                
        ++cur;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationQuality,    (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
        qualityScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        if (!Utils.isTouchDevice)
        {
            ++cur;
            
            GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationResolution, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
            resolutionScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));
            
            ++cur;
            
            GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationFullScreen, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
            fullScreenCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));
        }
        
        ++cur;

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private void drawControlsOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        for (int i=0; i<controlSetters.Count; ++i)
        {
            GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationKeys[i], (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);

            if (controlSetters[i].draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight)))
            {
                selectedControlSetter=controlSetters[i];
            }
            
            ++cur;
        }

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

    private bool drawButton(string text, float panelWidth, float panelHeight, float rowHeight, float rowOffset, int cur)
    {
        return GUI.Button(new Rect(0, rowOffset*cur, panelWidth, rowHeight), text, (!Utils.isTouchDevice && currentItem==cur) ? menuSelectedItemStyle : menuItemStyle);
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
        switch(index)
        {
            case 0: masterVolumeSlider.control();  break;
            case 1: musicVolumeSlider.control();   break;
            case 2: effectsVolumeSlider.control(); break;
            case 3:
                // Nothing
            break;
            default:
                Debug.LogError("Don't know how to handle it");
            break;
        }
    }

    private void controlItemInVideoOptions(int index)
    {
        switch(index)
        {
            case 0: showFPSCheckBox.control();    break;            
            case 1: qualityScroller.control();    break;
            case 2: resolutionScroller.control(); break;
            case 3: fullScreenCheckBox.control(); break;
            case 4:
                // Nothing
            break;
            default:
                Debug.LogError("Don't know how to handle it");
            break;
        }
    }

    private void controlItemInControlsOptions(int index)
    {
        // Nothing
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
        // Nothing
    }

    private void selectItemInVideoOptions(int index)
    {
        switch(index)
        {
            case 0: 
                showFPSCheckBox.setChecked(!showFPSCheckBox.isChecked());
                modified=true;
            break;            
            case 1: 
                // Nothing
            break;
            case 2: 
                // Nothing
            break;
            case 3: 
                fullScreenCheckBox.setChecked(!fullScreenCheckBox.isChecked());
                modified=true;
            break;
            default:
                Debug.LogError("Don't know how to handle it");
            break;
        }
    }
    
    private void selectItemInControlsOptions(int index)
    {
        controlSetters[index].setKeyPressed(0);
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
            itemsCount     = Utils.isTouchDevice? 4 : 5;
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
        itemsCount     = 4;

        masterVolumeSlider.setValue (mMasterVolume);
        musicVolumeSlider.setValue  (mMusicVolume);
        effectsVolumeSlider.setValue(mEffectsVolume);
    }

    private void goToVideoOptions()
    {
        Debug.Log("Go to video options");

        scrollPosition = Vector2.zero;
        currentState   = State.InVideoOptions;
        currentItem    = 0;
        itemsCount     = Utils.isTouchDevice? 3 : 5;

        showFPSCheckBox.setChecked(mShowFPS);
        updateResolutionScroller();
        fullScreenCheckBox.setChecked(mFullScreen);
        qualityScroller.setCurrentIndex(mQuality);
    }

    private void goToControlsOptions()
    {
        Debug.Log("Go to controls options");

        scrollPosition = Vector2.zero;
        currentState   = State.InControlsOptions;
        currentItem    = 0;
        itemsCount     = controlSetters.Count+1;

        int cur=0;

        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            controlSetters[cur].setKeys(key);

            ++cur;
        }
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

        if (!mLanguage.Equals(selectedLanguage))
        {
            mLanguage=selectedLanguage;
            languageManager.ChangeLanguage(mLanguage);
        }
    }

    private void applyChangesInSoundOptions()
    {
        mMasterVolume  = masterVolumeSlider.getValue();
        mMusicVolume   = musicVolumeSlider.getValue();
        mEffectsVolume = effectsVolumeSlider.getValue();
    }

    private void applyChangesInVideoOptions()
    {
        bool aShowFPS=showFPSCheckBox.isChecked();
        
        if (mShowFPS!=aShowFPS)
        {
            mShowFPS=aShowFPS;
            FPSCounter.isOn=mShowFPS;
        }

        int qualityIndex=qualityScroller.getCurrentIndex();
        
        if (mQuality!=qualityIndex)
        {
            mQuality=qualityIndex;
            QualitySettings.SetQualityLevel(mQuality);
        }

        if (!Utils.isTouchDevice)
        {
            string resolution=resolutionScroller.getSelectedItem();
            bool aFullScreen=fullScreenCheckBox.isChecked();
            
            if (!mResolution.Equals(resolution) || mFullScreen!=aFullScreen)
            {
                mResolution = resolution;
                mFullScreen = aFullScreen;

                changeResolution();
            }
        }
    }

    private void applyChangesInControlsOptions()
    {
        int cur=0;
        
        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            key.set(controlSetters[cur].getKeys());

            ++cur;
        }
    }

    private void updateLanguageScroller()
    {
        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        int languageIndex=0;

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            if (mLanguage.Equals(availableLanguages[i].Name))
            {
                languageIndex=i;
                break;
            }
        }

        languageScroller.setCurrentIndex(languageIndex);
    }

    private void updateResolutionScroller()
    {
        Resolution[] availableResolutions=Screen.resolutions;
        int resolutionIndex=0;
        
        for (int i=0; i<availableResolutions.Length; ++i)
        {
            Resolution resolution=availableResolutions[i];
            string resolutionString=resolution.width.ToString()+"x"+resolution.height.ToString()+" : "+resolution.refreshRate.ToString()+" Hz";

            if (mResolution.Equals(resolutionString))
            {
                resolutionIndex=i;
                break;
            }
            else
            if (resolutionString.StartsWith(mResolution))
            {
                resolutionIndex=i;
            }
        }

        resolutionScroller.setCurrentIndex(resolutionIndex);
    }

    private static void changeResolution()
    {
        try
        {
            string resolution=mResolution;

            if (resolution.EndsWith("Hz", StringComparison.OrdinalIgnoreCase))
            {
                resolution=resolution.Remove(resolution.Length-2).Trim();
            }

            int index  = resolution.IndexOf('x');
            int index2 = resolution.LastIndexOf(':');

            int width  = 800;
            int height = 600;
            int rate   = 0;

            width      = Convert.ToInt32(resolution.Substring(0, index).Trim());

            if (index2>=0)
            {
                height = Convert.ToInt32(resolution.Substring(index+1, index2-index-1).Trim());
                rate   = Convert.ToInt32(resolution.Substring(index2+1).Trim());
            }
            else
            {
                height = Convert.ToInt32(resolution.Substring(index+1).Trim());
            }

            Screen.SetResolution(width, height, mFullScreen, rate);
        }   
        catch (Exception)
        {
            string temp = mResolution;

            mResolution = "800x600";
            mFullScreen = true;

            Screen.SetResolution(800, 600, true);
            Debug.LogError("Impossible to set resolution: "+temp);
        }        
    }

    public static void load()
    {
        Debug.Log("Loading settings");

        IniFile iniFile=new IniFile("Settings");

        if (iniFile.Count()==0)
        {
            save();
            iniFile.load("Settings");
        }

        #region Game
        LanguageManager languageManager=LanguageManager.Instance;
        mLanguage=iniFile.Get("Game.Language", languageManager.GetSystemLanguage());

        if (!languageManager.IsLanguageSupported(mLanguage))
        {
            mLanguage="en";
        }

        Debug.Log("Application language: "+mLanguage);
        languageManager.ChangeLanguage(mLanguage);
        #endregion

        #region Sound
        mMasterVolume  = iniFile.Get("Sound.MasterVolume",  1f);
        mMusicVolume   = iniFile.Get("Sound.MusicVolume",   1f);
        mEffectsVolume = iniFile.Get("Sound.EffectsVolume", 1f);

        Debug.Log("Master volume:  "+mMasterVolume.ToString());
        Debug.Log("Music volume:   "+mMusicVolume.ToString());
        Debug.Log("Effects volume: "+mEffectsVolume.ToString());
        #endregion

        #region Video
        mShowFPS        = iniFile.Get("Video.ShowFPS",    false);
        mQuality        = iniFile.Get("Video.Quality",    QualitySettings.GetQualityLevel());

        if (!Utils.isTouchDevice)
        {
            mResolution = iniFile.Get("Video.Resolution", "800x600");
            mFullScreen = iniFile.Get("Video.FullScreen", true);
        }

        Debug.Log("Show FPS:       "+mShowFPS.ToString());
        FPSCounter.isOn=mShowFPS;
        
        Debug.Log("Video quality:  "+mQuality.ToString());
        QualitySettings.SetQualityLevel(mQuality);

        if (!Utils.isTouchDevice)
        {
            Debug.Log("Resolution:     "+mResolution);
            Debug.Log("Full screen:    "+mFullScreen.ToString());

            changeResolution();
        }
        #endregion

        #region Controls        
        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            KeyCode primaryKey;
            KeyCode secondaryKey;



            try
            {
                primaryKey   = (KeyCode)Enum.Parse(typeof(KeyCode), iniFile.Get("Controls."+key.name+".Primary",   "None"));
            }
            catch(Exception)
            {
                primaryKey   = KeyCode.None;
            }

            try
            {
                secondaryKey = (KeyCode)Enum.Parse(typeof(KeyCode), iniFile.Get("Controls."+key.name+".Secondary", "None"));
            }
            catch(Exception)
            {
                secondaryKey = KeyCode.None;
            }



            key.set(primaryKey, secondaryKey);
        }
        #endregion
    }

    public static void save()
    {
        #region Get available languages
        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        string languagesList="";

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            if (i>0)
            {
                languagesList=languagesList+", ";
            }

            languagesList=languagesList+availableLanguages[i].Name;
        }
        #endregion



        IniFile iniFile=new IniFile();

        #region Game
        iniFile.Set("Game.Language",        mLanguage,      "Application language: "+languagesList);
        #endregion

        #region Sound
        iniFile.Set("Sound.MasterVolume",   mMasterVolume,  "Master volume: 0-1");
        iniFile.Set("Sound.MusicVolume",    mMusicVolume,   "Music volume: 0-1");
        iniFile.Set("Sound.EffectsVolume",  mEffectsVolume, "Effects volume: 0-1");
        #endregion

        #region Video
        iniFile.Set("Video.ShowFPS",        mShowFPS,       "Show FPS: True/False");
        iniFile.Set("Video.Quality",        mQuality,       "Video quality: 0-"+QualitySettings.names.Length.ToString()+" ("+QualitySettings.names.Length.ToString()+" - Custom)");

        if (!Utils.isTouchDevice)
        {
            iniFile.Set("Video.Resolution", mResolution,    "Screen resolution: WIDTH x HEIGHT : RATE Hz");
            iniFile.Set("Video.FullScreen", mFullScreen,    "Full screen mode: True/False");
        }
        #endregion

        #region Controls
        bool firstKey=true;

        foreach (InputControl.KeyMapping key in InputControl.getKeys())
        {
            if (firstKey)
            {
                firstKey=false;
                iniFile.Set("Controls."+key.name+".Primary", key.primaryCode.ToString(), "Controls");
            }
            else
            {
                iniFile.Set("Controls."+key.name+".Primary", key.primaryCode.ToString());
            }

            iniFile.Set("Controls."+key.name+".Secondary",   key.secondaryCode.ToString());
        }
        #endregion

        iniFile.save("Settings");
    }
}
