#region Defines

// Touch devices
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
#define TOUCH_DEVICE
#endif

/* FPSCounter package required. Import from Assets Store: TODO */
#define PACKAGE_FPS_COUNTER
/* IniFile package required. Import from Assets Store: TODO */
#define PACKAGE_INI_FILE
/* InputControl package required. Import from Assets Store: TODO */
#define PACKAGE_INPUT_CONTROL
/* SmartLocalization package required. Import from Assets Store: http://u3d.as/content/cry-wolf-studios/smart-localization/4ie */
#define PACKAGE_SMART_LOCALIZATION

// TODO: Comment/Uncomment required options
#region Available options

#region Menu

#define MENU_GAME
#define MENU_AUDIO
#define MENU_VIDEO

#if !TOUCH_DEVICE
#define MENU_CONTROLS
#endif

#endregion

#region Game
#if MENU_GAME

#if PACKAGE_SMART_LOCALIZATION
#define OPTION_LANGUAGE
#endif

#define OPTION_DIFFICULTY
#define OPTION_BLOOD
#define OPTION_USE_HINTS
#define OPTION_AUTOSAVE

#endif
#endregion

#region Audio
#if MENU_AUDIO
#define OPTION_SOUND
#define OPTION_MASTER_VOLUME
#define OPTION_MUSIC_VOLUME
#define OPTION_VOICE_VOLUME
#define OPTION_EFFECTS_VOLUME
#define OPTION_SUBTITLES
#endif
#endregion

#region Video
#if MENU_VIDEO

#if PACKAGE_FPS_COUNTER
#define OPTION_SHOW_FPS
#endif

#if !TOUCH_DEVICE
#define OPTION_FULL_SCREEN_AND_RESOLUTION
#endif

#define OPTION_QUALITY

#endif
#endregion

#region Controls
#if MENU_CONTROLS

#if PACKAGE_INPUT_CONTROL
#define MENU_DEFINE_KEYS
#endif

#define OPTION_ALWAYS_RUN
#define OPTION_AUTO_AIM
#define OPTION_MOUSE_SENSITIVITY
#define OPTION_INVERT_MOUSE_Y

#if PACKAGE_INPUT_CONTROL
#define OPTION_INPUT_DEVICE
#endif

#endif
#endregion

#endregion

#endregion

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// <see cref="Options"/> is a script for displaying and handling game options.
/// </summary>
public class Options : MonoBehaviour
{
// TODO: Defaults

    /// <summary>
    /// List of states.
    /// </summary>
    private enum State
    {
        InOptionsList

#if MENU_GAME
        ,InGameOptions
#endif

#if MENU_AUDIO
        ,InAudioOptions
#endif

#if MENU_VIDEO
        ,InVideoOptions
#endif

#if MENU_CONTROLS
        ,InControlsOptions
#endif

#if MENU_DEFINE_KEYS
        ,InDefineKeys
#endif
    }

    /// <summary>
    /// Where do we need to go when save dialog will be closed.
    /// </summary>
    private enum NextStep
    {
        ToBack

        #if MENU_DEFINE_KEYS
        ,ToDefineKeys
        #endif
    }

#if OPTION_DIFFICULTY
    private const int difficultyCount=3;
#endif

#if OPTION_MOUSE_SENSITIVITY
    private const float mouseSensitivityMaximum=2;
#endif

    public GUIStyle  menuItemStyle;
    public GUIStyle  menuSelectedItemStyle;
    public Texture2D scrollerLeftTexture;
    public Texture2D scrollerRightTexture;
    public Texture2D checkboxOnTexture;
    public Texture2D checkboxOffTexture;

    private Vector2  scrollPosition;
    private Rect     saveDialogRect;
    private GUIStyle saveTextStyle;

    private State    currentState;
    private NextStep nextStep;
    private int      currentItem;
    private int      itemsCount;
    private bool     modified;
    private bool     askSaving;

    #region Options UI

    #region Game
#if OPTION_LANGUAGE
    private SelectionScroller   languageScroller;
#endif

#if OPTION_DIFFICULTY
    private SelectionScroller   difficultyScroller;
#endif

#if OPTION_BLOOD
    private BigCheckBox         bloodCheckBox;
#endif

#if OPTION_USE_HINTS
    private BigCheckBox         useHintsCheckBox;
#endif

#if OPTION_AUTOSAVE
    private BigCheckBox         autosaveCheckBox;
#endif
    #endregion

    #region Audio
#if OPTION_SOUND
    private BigCheckBox         soundCheckBox;
#endif

#if OPTION_MASTER_VOLUME
    private BigHorizontalSlider masterVolumeSlider;
#endif

#if OPTION_MUSIC_VOLUME
    private BigHorizontalSlider musicVolumeSlider;
#endif

#if OPTION_VOICE_VOLUME
    private BigHorizontalSlider voiceVolumeSlider;
#endif

#if OPTION_EFFECTS_VOLUME
    private BigHorizontalSlider effectsVolumeSlider;
#endif

#if OPTION_SUBTITLES
    private BigCheckBox         subtitlesCheckBox;
#endif
    #endregion

    #region Video
#if OPTION_SHOW_FPS
    private BigCheckBox         showFPSCheckBox;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
    private BigCheckBox         fullScreenCheckBox;
    private SelectionScroller   resolutionScroller;
#endif

#if OPTION_QUALITY
    private SelectionScroller   qualityScroller;
#endif
    #endregion

    #region Controls
#if MENU_DEFINE_KEYS
    private List<ControlSetter> controlSetters;
    private ControlSetter       selectedControlSetter;
#endif

#if OPTION_ALWAYS_RUN
    private BigCheckBox         alwaysRunCheckBox;
#endif

#if OPTION_AUTO_AIM
    private BigCheckBox         autoAimCheckBox;
#endif

#if OPTION_MOUSE_SENSITIVITY
    private BigHorizontalSlider mouseSensitivitySlider;
#endif

#if OPTION_INVERT_MOUSE_Y
    private BigCheckBox         invertMouseYCheckBox;
#endif

#if OPTION_INPUT_DEVICE
    private SelectionScroller   inputDeviceScroller;
#endif
    #endregion

    #endregion

    #region Options

    #region Game

    #region Language
#if OPTION_LANGUAGE
    private static string mLanguage="en";

    /// <summary>
    /// Gets the game language.
    /// </summary>
    /// <value>Game language.</value>
    public static string language
    {
        get
        {
            return mLanguage;
        }
    }
#endif
    #endregion

    #region Difficulty
#if OPTION_DIFFICULTY
    private static int mDifficulty=difficultyCount/2;

    /// <summary>
    /// Gets the game difficulty.
    /// </summary>
    /// <value>Game difficulty.</value>
    public static int difficulty
    {
        get
        {
            return mDifficulty;
        }
    }
#endif
    #endregion

    #region Blood
#if OPTION_BLOOD
    private static bool mBlood=true;

    /// <summary>
    /// Gets a value indicating that blood is visible.
    /// </summary>
    /// <value><c>true</c> if blood is visible; otherwise, <c>false</c>.</value>
    public static bool blood
    {
        get
        {
            return mBlood;
        }
    }
#endif
    #endregion

    #region Use hints
#if OPTION_USE_HINTS
    private static bool mUseHints=true;

    /// <summary>
    /// Gets a value indicating that hints are visible.
    /// </summary>
    /// <value><c>true</c> if hints are visible; otherwise, <c>false</c>.</value>
    public static bool useHints
    {
        get
        {
            return mUseHints;
        }
    }
#endif
    #endregion

    #region Autosave
#if OPTION_AUTOSAVE
    private static bool mAutosave=true;

    /// <summary>
    /// Gets a value indicating that autosave feature is enabled.
    /// </summary>
    /// <value><c>true</c> if autosave is enabled; otherwise, <c>false</c>.</value>
    public static bool autosave
    {
        get
        {
            return mAutosave;
        }
    }
#endif
    #endregion

    #endregion

    #region Audio

    #region Sound
#if OPTION_SOUND
    private static bool mSound=true;

    /// <summary>
    /// Gets a value indicating that any sound should be played.
    /// </summary>
    /// <value><c>true</c> if sound is enabled; otherwise, <c>false</c>.</value
    public static bool sound
    {
        get
        {
            return mSound;
        }
    }
#endif
    #endregion

    #region Master volume
#if OPTION_MASTER_VOLUME
    private static float mMasterVolume=1;

    /// <summary>
    /// Gets the master volume.
    /// </summary>
    /// <value>Master volume.</value>
    public static float masterVolume
    {
        get
        {
            return mMasterVolume;
        }
    }
#endif
    #endregion

    #region Music volume
#if OPTION_MUSIC_VOLUME
    private static float mMusicVolume=1;

    /// <summary>
    /// Gets the music volume.
    /// </summary>
    /// <value>Music volume.</value>
    public static float musicVolume
    {
        get
        {
            return mMusicVolume;
        }
    }
#endif
    #endregion

    #region Voice volume
#if OPTION_VOICE_VOLUME
    private static float mVoiceVolume=1;

    /// <summary>
    /// Gets the voice volume.
    /// </summary>
    /// <value>Voice volume.</value>
    public static float voiceVolume
    {
        get
        {
            return mVoiceVolume;
        }
    }
#endif
    #endregion

    #region Effects volume
#if OPTION_EFFECTS_VOLUME
    private static float mEffectsVolume=1;

    /// <summary>
    /// Gets the effects volume.
    /// </summary>
    /// <value>Effects volume.</value>
    public static float effectsVolume
    {
        get
        {
            return mEffectsVolume;
        }
    }
#endif
    #endregion

    #region Subtitles
#if OPTION_SUBTITLES
    private static bool mSubtitles=true;

    /// <summary>
    /// Gets a value indicating that subtitles are visible.
    /// </summary>
    /// <value><c>true</c> if subtitles are visible; otherwise, <c>false</c>.</value>
    public static bool subtitles
    {
        get
        {
            return mSubtitles;
        }
    }
#endif
    #endregion

    #endregion

    #region Video

    #region Show FPS
#if OPTION_SHOW_FPS
    private static bool mShowFPS=false;

    /// <summary>
    /// Gets a value indicating that FPS is visible.
    /// </summary>
    /// <value><c>true</c> if FPS is visible; otherwise, <c>false</c>.</value>
    public static bool showFPS
    {
        get
        {
            return mShowFPS;
        }
    }
#endif
    #endregion

#if OPTION_FULL_SCREEN_AND_RESOLUTION
    #region Full screen
    private static bool mFullScreen=true;

    /// <summary>
    /// Gets a value indicating that full screen mode is enabled.
    /// </summary>
    /// <value><c>true</c> if full screen mode is enabled; otherwise, <c>false</c>.</value>
    public static bool fullScreen
    {
        get
        {
            return mFullScreen;
        }
    }
    #endregion

    #region Resolution
    private static string mResolution="800x600";

    /// <summary>
    /// Gets the screen resolution.
    /// </summary>
    /// <value>Screen resolution.</value>
    public static string resolution
    {
        get
        {
            return mResolution;
        }
    }
    #endregion
#endif

    #region Quality
#if OPTION_QUALITY
    private static int mQuality=0;

    /// <summary>
    /// Gets the video quality. Use QualitySettings to change predefined qualities.
    /// </summary>
    /// <value>Video quality.</value>
    public static int quality
    {
        get
        {
            return mQuality;
        }
    }
#endif
    #endregion

    #endregion

    #region Controls

    #region Keys
#if MENU_DEFINE_KEYS
    /// <summary>
    /// Gets the list of keys.
    /// </summary>
    /// <value>List of keys.</value>
    public static List<KeyMapping> keys
    {
        get
        {
            return InputControl.getKeys();
        }
    }
#endif
    #endregion

    #region Always run
#if OPTION_ALWAYS_RUN
    private static bool mAlwaysRun=false;

    /// <summary>
    /// Gets a value indicating that player don't need to hold button for running.
    /// </summary>
    /// <value><c>true</c> if player run always; otherwise, <c>false</c>.</value>
    public static bool alwaysRun
    {
        get
        {
            return mAlwaysRun;
        }
    }
#endif
    #endregion

    #region Auto aim
#if OPTION_AUTO_AIM
    private static bool mAutoAim=false;

    /// <summary>
    /// Gets a value indicating that mouse will automatically aim to the target.
    /// </summary>
    /// <value><c>true</c> if auto aim is enabled; otherwise, <c>false</c>.</value>
    public static bool autoAim
    {
        get
        {
            return mAutoAim;
        }
    }
#endif
    #endregion

    #region Mouse sensitivity
#if OPTION_MOUSE_SENSITIVITY
    private static float mMouseSensitivity=1f;

    /// <summary>
    /// Gets the mouse sensitivity.
    /// </summary>
    /// <value>Mouse sensitivity.</value>
    public static float mouseSensitivity
    {
        get
        {
            return mMouseSensitivity;
        }
    }
#endif
    #endregion

    #region Invert mouse Y
#if OPTION_INVERT_MOUSE_Y
    private static bool mInvertMouseY=false;

    /// <summary>
    /// Gets a value indicating that mouse Y axis is inverted.
    /// </summary>
    /// <value><c>true</c> if mouse Y axis is inverted; otherwise, <c>false</c>.</value>
    public static bool invertMouseY
    {
        get
        {
            return mInvertMouseY;
        }
    }
#endif
    #endregion

    #region Input device
#if OPTION_INPUT_DEVICE
    /// <summary>
    /// Gets the preferred input device.
    /// </summary>
    /// <value>Preferred input device.</value>
    public static InputDevice inputDevice
    {
        get
        {
            return InputControl.preferredInputDevice;
        }
    }
#endif
    #endregion

    #endregion

    #endregion

    #region Localization

    private string       localizationBack;

    #region Menu
#if MENU_GAME
    private string       localizationGame;
#endif

#if MENU_AUDIO
    private string       localizationAudio;
#endif

#if MENU_VIDEO
    private string       localizationVideo;
#endif

#if MENU_CONTROLS
    private string       localizationControls;
#endif
    #endregion

    #region Game
#if OPTION_LANGUAGE
    private string       localizationLanguage;
#endif

#if OPTION_DIFFICULTY
    private string       localizationDifficulty;
#endif

#if OPTION_BLOOD
    private string       localizationBlood;
#endif

#if OPTION_USE_HINTS
    private string       localizationUseHints;
#endif

#if OPTION_AUTOSAVE
    private string       localizationAutosave;
#endif
    #endregion

    #region Audio
#if OPTION_SOUND
    private string       localizationSound;
#endif

#if OPTION_MASTER_VOLUME
    private string       localizationMasterVolume;
#endif

#if OPTION_MUSIC_VOLUME
    private string       localizationMusicVolume;
#endif

#if OPTION_VOICE_VOLUME
    private string       localizationVoiceVolume;
#endif

#if OPTION_EFFECTS_VOLUME
    private string       localizationEffectsVolume;
#endif

#if OPTION_SUBTITLES
    private string       localizationSubtitles;
#endif
    #endregion

    #region Video
#if OPTION_SHOW_FPS
    private string       localizationShowFPS;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
    private string       localizationFullScreen;
    private string       localizationResolution;
#endif

#if OPTION_QUALITY
    private string       localizationQuality;
#endif
    #endregion

    #region Controls
#if MENU_DEFINE_KEYS
    private string       localizationDefineKeys;
    private List<string> localizationKeys;
#endif

#if OPTION_ALWAYS_RUN
    private string       localizationAlwaysRun;
#endif

#if OPTION_AUTO_AIM
    private string       localizationAutoAim;
#endif

#if OPTION_MOUSE_SENSITIVITY
    private string       localizationMouseSensitivity;
#endif

#if OPTION_INVERT_MOUSE_Y
    private string       localizationInvertMouseY;
#endif

#if OPTION_INPUT_DEVICE
    private string       localizationInputDevice;
#endif
    #endregion

    #region Save dialog
    private string       localizationSettingsChanged;
    private string       localizationDoYouWantToSaveChanges;
    private string       localizationOK;
    private string       localizationCancel;
    #endregion

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
#if OPTION_LANGUAGE
        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        string[] languages=new string[availableLanguages.Count];

        for (int i=0; i<availableLanguages.Count; ++i)
        {
            languages[i]=availableLanguages[i].EnglishName;
        }

        languageScroller=new SelectionScroller(languages, 0, scrollerLeftTexture, scrollerRightTexture);
        languageScroller.setModifiedFunction(settingsModified);
#endif

#if OPTION_DIFFICULTY
        difficultyScroller=new SelectionScroller(null, 0, scrollerLeftTexture, scrollerRightTexture);
        difficultyScroller.setModifiedFunction(settingsModified);
#endif

#if OPTION_BLOOD
        bloodCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        bloodCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_USE_HINTS
        useHintsCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        useHintsCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_AUTOSAVE
        autosaveCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        autosaveCheckBox.setModifiedFunction(settingsModified);
#endif
        #endregion

        #region Audio
#if OPTION_SOUND
        soundCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        soundCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_MASTER_VOLUME
        masterVolumeSlider  = new BigHorizontalSlider(mMasterVolume,  0f, 1f);
        masterVolumeSlider.setModifiedFunction (settingsModified);
#endif

#if OPTION_MUSIC_VOLUME
        musicVolumeSlider   = new BigHorizontalSlider(mMusicVolume,   0f, 1f);
        musicVolumeSlider.setModifiedFunction  (settingsModified);
#endif

#if OPTION_VOICE_VOLUME
        voiceVolumeSlider   = new BigHorizontalSlider(mVoiceVolume,   0f, 1f);
        voiceVolumeSlider.setModifiedFunction(settingsModified);
#endif

#if OPTION_EFFECTS_VOLUME
        effectsVolumeSlider = new BigHorizontalSlider(mEffectsVolume, 0f, 1f);
        effectsVolumeSlider.setModifiedFunction(settingsModified);
#endif

#if OPTION_SUBTITLES
        subtitlesCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        subtitlesCheckBox.setModifiedFunction(settingsModified);
#endif
        #endregion

        #region Video
#if OPTION_SHOW_FPS
        showFPSCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        showFPSCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        fullScreenCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        fullScreenCheckBox.setModifiedFunction(settingsModified);

        Resolution[] availableResolutions=Screen.resolutions;
        string[] resolutions=new string[availableResolutions.Length];

        for (int i=0; i<availableResolutions.Length; ++i)
        {
            Resolution resolution=availableResolutions[i];

            resolutions[i]=resolution.width.ToString()+"x"+resolution.height.ToString()+" : "+resolution.refreshRate.ToString()+" Hz";
        }

        resolutionScroller=new SelectionScroller(resolutions, 0, scrollerLeftTexture, scrollerRightTexture);
        resolutionScroller.setModifiedFunction(settingsModified);
#endif

#if OPTION_QUALITY
        qualityScroller=new SelectionScroller(null, 0, scrollerLeftTexture, scrollerRightTexture);
        qualityScroller.setModifiedFunction(settingsModified);
#endif
        #endregion

        #region Controls
#if MENU_DEFINE_KEYS
        selectedControlSetter=null;
        controlSetters=new List<ControlSetter>();

        for (int i=0; i<InputControl.getKeys().Count; ++i)
        {
            ControlSetter controlSetter=new ControlSetter();
            controlSetter.setModifiedFunction(settingsModified);
            controlSetters.Add(controlSetter);
        }

        localizationKeys=new List<string>();
#endif

#if OPTION_ALWAYS_RUN
        alwaysRunCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        alwaysRunCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_AUTO_AIM
        autoAimCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        autoAimCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_MOUSE_SENSITIVITY
        mouseSensitivitySlider=new BigHorizontalSlider(mMouseSensitivity, 0f, mouseSensitivityMaximum);
        mouseSensitivitySlider.setModifiedFunction(settingsModified);
#endif

#if OPTION_INVERT_MOUSE_Y
        invertMouseYCheckBox=new BigCheckBox(checkboxOnTexture, checkboxOffTexture);
        invertMouseYCheckBox.setModifiedFunction(settingsModified);
#endif

#if OPTION_INPUT_DEVICE
        inputDeviceScroller=new SelectionScroller(null, 0, scrollerLeftTexture, scrollerRightTexture);
        inputDeviceScroller.setModifiedFunction(settingsModified);
#endif
        #endregion

        #region Localization
#if PACKAGE_SMART_LOCALIZATION
        LanguageManager languageManager=LanguageManager.Instance;
        languageManager.OnChangeLanguage+=OnChangeLanguage;
        OnChangeLanguage(languageManager);
#else
        OnChangeLanguage();
#endif
        #endregion

        modified  = false;
        askSaving = false;

        goToOptionsList(0);
    }

#region Localization
#if PACKAGE_SMART_LOCALIZATION
    /// <summary>
    /// Language changed listener.
    /// </summary>
    /// <param name="another">LanguageManager instance.</param>
    void OnChangeLanguage(LanguageManager languageManager)
#else
    /// <summary>
    /// Language changed listener.
    /// </summary>
    void OnChangeLanguage()
#endif
    {
        localizationBack                   = localize("Back");

        #region Menu
#if MENU_GAME
        localizationGame                   = localize("Game");
#endif

#if MENU_AUDIO
        localizationAudio                  = localize("Audio");
#endif

#if MENU_VIDEO
        localizationVideo                  = localize("Video");
#endif

#if MENU_CONTROLS
        localizationControls               = localize("Controls");
#endif
        #endregion

        #region Game
#if OPTION_LANGUAGE
        localizationLanguage               = localize("Language");
#endif

#if OPTION_DIFFICULTY
        localizationDifficulty             = localize("Difficulty");

        // --------------------------------------

        string[] difficulties=new string[difficultyCount];

        for (int i=0; i<difficulties.Length; ++i)
        {
            difficulties[i]                = localize("Difficulty"+(i+1).ToString());
        }

        difficultyScroller.items=difficulties;
#endif

#if OPTION_BLOOD
        localizationBlood                  = localize("Blood");
#endif

#if OPTION_USE_HINTS
        localizationUseHints               = localize("UseHints");
#endif

#if OPTION_AUTOSAVE
        localizationAutosave               = localize("Autosave");
#endif
        #endregion

        #region Audio
#if OPTION_SOUND
        localizationSound                  = localize("Sound");
#endif

#if OPTION_MASTER_VOLUME
        localizationMasterVolume           = localize("MasterVolume");
#endif

#if OPTION_MUSIC_VOLUME
        localizationMusicVolume            = localize("MusicVolume");
#endif

#if OPTION_VOICE_VOLUME
        localizationVoiceVolume            = localize("VoiceVolume");
#endif

#if OPTION_EFFECTS_VOLUME
        localizationEffectsVolume          = localize("EffectsVolume");
#endif

#if OPTION_SUBTITLES
        localizationSubtitles              = localize("Subtitles");
#endif
        #endregion

        #region Video
#if OPTION_SHOW_FPS
        localizationShowFPS                = localize("ShowFPS");
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        localizationFullScreen             = localize("FullScreen");
        localizationResolution             = localize("Resolution");
#endif

#if OPTION_QUALITY
        localizationQuality                = localize("Quality");

        // --------------------------------------

        string[] qualities=QualitySettings.names;

        for (int i=0; i<qualities.Length; ++i)
        {
            qualities[i]                   = localize(qualities[i]);
        }

        qualityScroller.items=qualities;
#endif
        #endregion

        #region Controls
#if MENU_DEFINE_KEYS
        localizationDefineKeys             = localize("DefineKeys");

        // --------------------------------------

        localizationKeys.Clear();

        foreach (KeyMapping key in InputControl.getKeys())
        {
            localizationKeys.Add(localize(key.name));
        }
#endif

#if OPTION_ALWAYS_RUN
        localizationAlwaysRun              = localize("AlwaysRun");
#endif

#if OPTION_AUTO_AIM
        localizationAutoAim                = localize("AutoAim");
#endif

#if OPTION_MOUSE_SENSITIVITY
        localizationMouseSensitivity       = localize("MouseSensitivity");
#endif

#if OPTION_INVERT_MOUSE_Y
        localizationInvertMouseY           = localize("InvertMouseY");
#endif

#if OPTION_INPUT_DEVICE
        localizationInputDevice            = localize("InputDevice");

        // --------------------------------------

        string[] inputDevices=Enum.GetNames(typeof(InputDevice));

        for (int i=0; i<inputDevices.Length; ++i)
        {
            inputDevices[i]                = localize(inputDevices[i]);
        }

        inputDeviceScroller.items=inputDevices;
#endif
        #endregion

        #region Save dialog
        localizationSettingsChanged        = localize("SettingsChanged");
        localizationDoYouWantToSaveChanges = localize("DoYouWantToSaveChanges");
        localizationOK                     = localize("OK");
        localizationCancel                 = localize("Cancel");
        #endregion
    }

    /// <summary>
    /// Returns translation for specified token.
    /// </summary>
    /// <returns>Translated string.</returns>
    /// <param name="token">Translation token.</param>
    private string localize(string token)
    {
        string res="";

        if (token==null || token.Equals(""))
        {
            return res;
        }

#if PACKAGE_SMART_LOCALIZATION
        res=LanguageManager.Instance.GetTextValue("Options."+token);

        if (res!=null && !res.Equals(""))
        {
            return res;
        }
#endif

        if (token.Equals("ShowFPS"))
        {
            return "Show FPS";
        }

        if (token.Equals("OK"))
        {
            return "OK";
        }

        res=token[0].ToString();

        for (int i=1; i<token.Length; ++i)
        {
            if (token[i]>='A' && token[i]<='Z')
            {
                res=res+" "+((char)(token[i]+'a'-'A')).ToString();
            }
            else
            {
                res=res+token[i];
            }
        }

        return res;
    }
#endregion

    // Update is called once per frame
    void Update()
    {
#if MENU_DEFINE_KEYS
        if (selectedControlSetter!=null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectedControlSetter.setSelectedKey(null);
                selectedControlSetter=null;
            }
            else
            {
                CustomInput input=InputControl.currentInput();

                if (input!=null)
                {
                    selectedControlSetter.setSelectedKey(input);
                    selectedControlSetter=null;
                }
            }
        }
        else
        {
#endif
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                goBack();
            }
            else
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!askSaving && currentItem>0)
                {
                    --currentItem;

                    float rowHeight   = Screen.height*0.1f;
                    float rowOffset   = rowHeight+Screen.height*0.025f;

                    float itemTop=currentItem*rowOffset;

                    if (scrollPosition.y>itemTop)
                    {
                        scrollPosition.y=itemTop;
                    }
                }
            }
            else
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!askSaving && currentItem<itemsCount-1)
                {
                    ++currentItem;

                    float boxHeight   = Screen.height*0.9f;
                    float panelHeight = boxHeight*0.98f;
                    float rowHeight   = Screen.height*0.1f;
                    float rowOffset   = rowHeight+Screen.height*0.025f;

                    float itemBottom=currentItem*rowOffset+rowHeight;

                    if (scrollPosition.y+panelHeight<itemBottom)
                    {
                        scrollPosition.y=itemBottom-panelHeight;
                    }
                }
            }
            else
            if (
                Input.GetKeyDown(KeyCode.Return)
                ||
                Input.GetKeyDown(KeyCode.KeypadEnter)
               )
            {
                if (askSaving)
                {
                    applyChanges();
                    goToNextStep();
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
                    controlItem();
                }
            }
#if MENU_DEFINE_KEYS
        }
#endif
    }

    void OnGUI()
    {
#if MENU_DEFINE_KEYS
        if (selectedControlSetter!=null && Event.current.type!=EventType.Repaint && Event.current.type!=EventType.Layout)
        {
            Event.current.Use();
        }
#endif

        if (askSaving)
        {
            GUIStyle windowStyle=new GUIStyle(GUI.skin.window);
            windowStyle.fontSize=(int)(Screen.height*0.045);

            saveDialogRect=GUI.ModalWindow(0, saveDialogRect, drawSaveDialog, localizationSettingsChanged, windowStyle);
        }

        menuItemStyle.fontSize         = (int)(Screen.height*0.05);
        menuSelectedItemStyle.fontSize = (int)(Screen.height*0.05);

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

#if MENU_GAME
            case State.InGameOptions:     drawGameOptions    (panelWidth, panelHeight, rowHeight, rowOffset); break;
#endif

#if MENU_AUDIO
            case State.InAudioOptions:    drawAudioOptions   (panelWidth, panelHeight, rowHeight, rowOffset); break;
#endif

#if MENU_VIDEO
            case State.InVideoOptions:    drawVideoOptions   (panelWidth, panelHeight, rowHeight, rowOffset); break;
#endif

#if MENU_CONTROLS
            case State.InControlsOptions: drawControlsOptions(panelWidth, panelHeight, rowHeight, rowOffset); break;
#endif

#if MENU_DEFINE_KEYS
            case State.InDefineKeys:      drawDefineKeys     (panelWidth, panelHeight, rowHeight, rowOffset); break;
#endif

            default:
                Debug.LogError("Unknown state");
            break;
        }

        GUI.EndScrollView();

        GUI.EndGroup();
    }

    /// <summary>
    /// Draw function for save dialog.
    /// </summary>
    /// <param name="id">Window ID.</param>
    private void drawSaveDialog(int id)
    {
        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);

        buttonStyle.fontSize   = (int)(Screen.height*0.03);
        saveTextStyle.fontSize = (int)(Screen.height*0.04);



        GUI.Label(new Rect(saveDialogRect.width*0.05f, saveDialogRect.height*0.25f, saveDialogRect.width*0.9f, saveDialogRect.height*0.4f), localizationDoYouWantToSaveChanges, saveTextStyle);

        if (GUI.Button(new Rect(saveDialogRect.width*0.1f, saveDialogRect.height*0.7f, saveDialogRect.width*0.3f, saveDialogRect.height*0.2f), localizationOK, buttonStyle))
        {
            applyChanges();
            goToNextStep();
        }

        if (GUI.Button(new Rect(saveDialogRect.width*0.6f, saveDialogRect.height*0.7f, saveDialogRect.width*0.3f, saveDialogRect.height*0.2f), localizationCancel, buttonStyle))
        {
            modified  = false;
            askSaving = false;

            goToNextStep();
        }
    }

    /// <summary>
    /// Draws options list in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawOptionsList(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

#if MENU_GAME
        if (drawButton(localizationGame, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;
#endif

#if MENU_AUDIO
        if (drawButton(localizationAudio, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;
#endif

#if MENU_VIDEO
        if (drawButton(localizationVideo, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;
#endif

#if MENU_CONTROLS
        if (drawButton(localizationControls, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;
#endif

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }

#if MENU_GAME
    /// <summary>
    /// Draws game options in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawGameOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

#if OPTION_LANGUAGE
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationLanguage, currentTextStyle(cur));
        languageScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_DIFFICULTY
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationDifficulty, currentTextStyle(cur));
        difficultyScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_BLOOD
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationBlood,    currentTextStyle(cur));
        bloodCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_USE_HINTS
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationUseHints,  currentTextStyle(cur));
        useHintsCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_AUTOSAVE
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationAutosave,  currentTextStyle(cur));
        autosaveCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }
#endif

#if MENU_AUDIO
    /// <summary>
    /// Draws audio options in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawAudioOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

#if OPTION_SOUND
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationSound,        currentTextStyle(cur));
        soundCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_MASTER_VOLUME
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationMasterVolume, currentTextStyle(cur));
        masterVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;
#endif

#if OPTION_MUSIC_VOLUME
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationMusicVolume,  currentTextStyle(cur));
        musicVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;
#endif

#if OPTION_VOICE_VOLUME
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationVoiceVolume,  currentTextStyle(cur));
        voiceVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;
#endif

#if OPTION_EFFECTS_VOLUME
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationEffectsVolume, currentTextStyle(cur));
        effectsVolumeSlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;
#endif

#if OPTION_SUBTITLES
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationSubtitles,     currentTextStyle(cur));
        subtitlesCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }
#endif

#if MENU_VIDEO
    /// <summary>
    /// Draws video options in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawVideoOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

#if OPTION_SHOW_FPS
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationShowFPS,    currentTextStyle(cur));
        showFPSCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationFullScreen, currentTextStyle(cur));
        fullScreenCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;

        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationResolution, currentTextStyle(cur));
        resolutionScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_QUALITY
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationQuality,    currentTextStyle(cur));
        qualityScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }
#endif

#if MENU_CONTROLS
    /// <summary>
    /// Draws controls options in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawControlsOptions(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

#if MENU_DEFINE_KEYS
        if (drawButton(localizationDefineKeys, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }

        ++cur;
#endif

#if OPTION_ALWAYS_RUN
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationAlwaysRun,        currentTextStyle(cur));
        alwaysRunCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_AUTO_AIM
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationAutoAim,          currentTextStyle(cur));
        autoAimCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_MOUSE_SENSITIVITY
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationMouseSensitivity, currentTextStyle(cur));
        mouseSensitivitySlider.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight), !askSaving);

        ++cur;
#endif

#if OPTION_INVERT_MOUSE_Y
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationInvertMouseY,     currentTextStyle(cur));
        invertMouseYCheckBox.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

#if OPTION_INPUT_DEVICE
        GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationInputDevice,      currentTextStyle(cur));
        inputDeviceScroller.draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight));

        ++cur;
#endif

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Draws keys definition in specified location.
    /// </summary>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    private void drawDefineKeys(float panelWidth, float panelHeight, float rowHeight, float rowOffset)
    {
        int cur=0;

        for (int i=0; i<controlSetters.Count; ++i)
        {
            GUI.Label(new Rect(0, rowOffset*cur, panelWidth*0.4f, rowHeight), localizationKeys[i], currentTextStyle(cur));

            if (controlSetters[i].draw(new Rect(panelWidth*0.45f, rowOffset*cur, panelWidth*0.55f, rowHeight)))
            {
                selectedControlSetter=controlSetters[i];
            }
            else
            {
                if (selectedControlSetter==controlSetters[i])
                {
                    selectedControlSetter=null;
                }
            }

            ++cur;
        }

        if (drawButton(localizationBack, panelWidth, panelHeight, rowHeight, rowOffset, cur))
        {
            selectItem(cur);
        }
    }
#endif

    /// <summary>
    /// Draws button in specified location.
    /// </summary>
    /// <returns>True if button was pressed.</returns>
    /// <param name="text">Button contents.</param>
    /// <param name="panelWidth">Panel width.</param>
    /// <param name="panelHeight">Panel height.</param>
    /// <param name="rowHeight">Row height.</param>
    /// <param name="rowOffset">Distance between 2 rows (Including rowHeight).</param>
    /// <param name="cur">Current row. Used to check is it button selected or not.</param>
    private bool drawButton(string text, float panelWidth, float panelHeight, float rowHeight, float rowOffset, int cur)
    {
        return GUI.Button(new Rect(0, rowOffset*cur, panelWidth, rowHeight), text, currentTextStyle(cur));
    }

    /// <summary>
    /// Returns button style for current row.
    /// </summary>
    /// <returns>Button style.</returns>
    /// <param name="cur">Current row. Used to check is it button selected or not.</param>
    private GUIStyle currentTextStyle(int cur)
    {
#if TOUCH_DEVICE
        return menuItemStyle;
#else
        return currentItem==cur ? menuSelectedItemStyle : menuItemStyle;
#endif
    }

    /// <summary>
    /// Handles user interaction.
    /// </summary>
    private void controlItem()
    {
        switch(currentState)
        {
            case State.InOptionsList:     controlItemInOptionsList    (); break;

#if MENU_GAME
            case State.InGameOptions:     controlItemInGameOptions    (); break;
#endif

#if MENU_AUDIO
            case State.InAudioOptions:    controlItemInAudioOptions   (); break;
#endif

#if MENU_VIDEO
            case State.InVideoOptions:    controlItemInVideoOptions   (); break;
#endif

#if MENU_CONTROLS
            case State.InControlsOptions: controlItemInControlsOptions(); break;
#endif

#if MENU_DEFINE_KEYS
            case State.InDefineKeys:      controlItemInDefineKeys     (); break;
#endif

            default:
                Debug.LogError("Unknown state");
            break;
        }
    }

    /// <summary>
    /// Handles user interaction in options list.
    /// </summary>
    private void controlItemInOptionsList()
    {
        // Nothing
    }

#if MENU_GAME
    /// <summary>
    /// Handles user interaction in game options.
    /// </summary>
    private void controlItemInGameOptions()
    {
        int cur=0;

#if OPTION_LANGUAGE
        if (cur==currentItem)
        {
            languageScroller.control();
            return;
        }

        ++cur;
#endif

#if OPTION_DIFFICULTY
        if (cur==currentItem)
        {
            difficultyScroller.control();
            return;
        }

        ++cur;
#endif

#if OPTION_BLOOD
        if (cur==currentItem)
        {
            bloodCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_USE_HINTS
        if (cur==currentItem)
        {
            useHintsCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_AUTOSAVE
        if (cur==currentItem)
        {
            autosaveCheckBox.control();
            return;
        }

        ++cur;
#endif

        if (cur==currentItem)
        {
            // Nothing
            return;
        }

        Debug.LogError("Don't know how to handle it");
    }
#endif

#if MENU_AUDIO
    /// <summary>
    /// Handles user interaction in audio options.
    /// </summary>
    private void controlItemInAudioOptions()
    {
        int cur=0;

#if OPTION_SOUND
        if (cur==currentItem)
        {
            soundCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_MASTER_VOLUME
        if (cur==currentItem)
        {
            masterVolumeSlider.control();
            return;
        }

        ++cur;
#endif

#if OPTION_MUSIC_VOLUME
        if (cur==currentItem)
        {
            musicVolumeSlider.control();
            return;
        }

        ++cur;
#endif

#if OPTION_VOICE_VOLUME
        if (cur==currentItem)
        {
            voiceVolumeSlider.control();
            return;
        }

        ++cur;
#endif

#if OPTION_EFFECTS_VOLUME
        if (cur==currentItem)
        {
            effectsVolumeSlider.control();
            return;
        }

        ++cur;
#endif

#if OPTION_SUBTITLES
        if (cur==currentItem)
        {
            subtitlesCheckBox.control();
            return;
        }

        ++cur;
#endif

        if (cur==currentItem)
        {
            // Nothing
            return;
        }

        Debug.LogError("Don't know how to handle it");
    }
#endif

#if MENU_VIDEO
    /// <summary>
    /// Handles user interaction in video options.
    /// </summary>
    private void controlItemInVideoOptions()
    {
        int cur=0;

#if OPTION_SHOW_FPS
        if (cur==currentItem)
        {
            showFPSCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        if (cur==currentItem)
        {
            fullScreenCheckBox.control();
            return;
        }

        ++cur;

        if (cur==currentItem)
        {
            resolutionScroller.control();
            return;
        }

        ++cur;
#endif

#if OPTION_QUALITY
        if (cur==currentItem)
        {
            qualityScroller.control();
            return;
        }

        ++cur;
#endif

        if (cur==currentItem)
        {
            // Nothing
            return;
        }

        Debug.LogError("Don't know how to handle it");
    }
#endif

#if MENU_CONTROLS
    /// <summary>
    /// Handles user interaction in controls options.
    /// </summary>
    private void controlItemInControlsOptions()
    {
        int cur=0;

#if MENU_DEFINE_KEYS
        if (cur==currentItem)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_ALWAYS_RUN
        if (cur==currentItem)
        {
            alwaysRunCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_AUTO_AIM
        if (cur==currentItem)
        {
            autoAimCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_MOUSE_SENSITIVITY
        if (cur==currentItem)
        {
            mouseSensitivitySlider.control();
            return;
        }

        ++cur;
#endif

#if OPTION_INVERT_MOUSE_Y
        if (cur==currentItem)
        {
            invertMouseYCheckBox.control();
            return;
        }

        ++cur;
#endif

#if OPTION_INPUT_DEVICE
        if (cur==currentItem)
        {
            inputDeviceScroller.control();
            return;
        }

        ++cur;
#endif

        if (cur==currentItem)
        {
            // Nothing
            return;
        }

        Debug.LogError("Don't know how to handle it");
    }
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Handles user interaction in keys definition.
    /// </summary>
    private void controlItemInDefineKeys()
    {
        // Nothing
    }
#endif

    /// <summary>
    /// Select item by index.
    /// </summary>
    /// <param name="index">Item index.</param>
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

#if MENU_GAME
            case State.InGameOptions:     selectItemInGameOptions    (index); break;
#endif

#if MENU_AUDIO
            case State.InAudioOptions:    selectItemInAudioOptions   (index); break;
#endif

#if MENU_VIDEO
            case State.InVideoOptions:    selectItemInVideoOptions   (index); break;
#endif

#if MENU_CONTROLS
            case State.InControlsOptions: selectItemInControlsOptions(index); break;
#endif

#if MENU_DEFINE_KEYS
            case State.InDefineKeys:      selectItemInDefineKeys     (index); break;
#endif

            default:
                Debug.LogError("Unknown state");
            break;
        }
    }

    /// <summary>
    /// Select item by index in options list.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInOptionsList(int index)
    {
        int cur=0;

#if MENU_GAME
        if (cur==index)
        {
            goToGameOptions();
            return;
        }

        ++cur;
#endif

#if MENU_AUDIO
        if (cur==index)
        {
            goToAudioOptions();
            return;
        }

        ++cur;
#endif

#if MENU_VIDEO
        if (cur==index)
        {
            goToVideoOptions();
            return;
        }

        ++cur;
#endif

#if MENU_CONTROLS
        if (cur==index)
        {
            goToControlsOptions();
            return;
        }

        ++cur;
#endif

        Debug.LogError("Don't know how to handle it (cur="+cur.ToString()+")");
    }

#if MENU_GAME
    /// <summary>
    /// Select item by index in game options.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInGameOptions(int index)
    {
        int cur=0;

#if OPTION_LANGUAGE
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_DIFFICULTY
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_BLOOD
        if (cur==index)
        {
            bloodCheckBox.isChecked=!bloodCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_USE_HINTS
        if (cur==index)
        {
            useHintsCheckBox.isChecked=!useHintsCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_AUTOSAVE
        if (cur==index)
        {
            autosaveCheckBox.isChecked=!autosaveCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

        Debug.LogError("Don't know how to handle it (cur="+cur.ToString()+")");
    }
#endif

#if MENU_AUDIO
    /// <summary>
    /// Select item by index in audio options.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInAudioOptions(int index)
    {
        int cur=0;

#if OPTION_SOUND
        if (cur==index)
        {
            soundCheckBox.isChecked=!soundCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_MASTER_VOLUME
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_MUSIC_VOLUME
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_VOICE_VOLUME
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_EFFECTS_VOLUME
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_SUBTITLES
        if (cur==index)
        {
            subtitlesCheckBox.isChecked=!subtitlesCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

        Debug.LogError("Don't know how to handle it (cur="+cur.ToString()+")");
    }
#endif

#if MENU_VIDEO
    /// <summary>
    /// Select item by index in video options.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInVideoOptions(int index)
    {
        int cur=0;

#if OPTION_SHOW_FPS
        if (cur==index)
        {
            showFPSCheckBox.isChecked=!showFPSCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        if (cur==index)
        {
            fullScreenCheckBox.isChecked=!fullScreenCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;

        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_QUALITY
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

        Debug.LogError("Don't know how to handle it (cur="+cur.ToString()+")");
    }
#endif

#if MENU_CONTROLS
    /// <summary>
    /// Select item by index in controls options.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInControlsOptions(int index)
    {
        int cur=0;

#if MENU_DEFINE_KEYS
        if (cur==index)
        {
            goToDefineKeys();
            return;
        }

        ++cur;
#endif

#if OPTION_ALWAYS_RUN
        if (cur==index)
        {
            alwaysRunCheckBox.isChecked=!alwaysRunCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_AUTO_AIM
        if (cur==index)
        {
            autoAimCheckBox.isChecked=!autoAimCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_MOUSE_SENSITIVITY
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

#if OPTION_INVERT_MOUSE_Y
        if (cur==index)
        {
            invertMouseYCheckBox.isChecked=!invertMouseYCheckBox.isChecked;
            modified=true;

            return;
        }

        ++cur;
#endif

#if OPTION_INPUT_DEVICE
        if (cur==index)
        {
            // Nothing
            return;
        }

        ++cur;
#endif

        Debug.LogError("Don't know how to handle it (cur="+cur.ToString()+")");
    }
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Select item by index in keys definition.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void selectItemInDefineKeys(int index)
    {
        // Nothing
    }
#endif

    /// <summary>
    /// Handler for modification events.
    /// </summary>
    private void settingsModified()
    {
        modified=true;
    }

    /// <summary>
    /// Continue movement when save dialog will be closed.
    /// </summary>
    private void goToNextStep()
    {
        switch(nextStep)
        {
            case NextStep.ToBack:       goBack();         break;

            #if MENU_DEFINE_KEYS
            case NextStep.ToDefineKeys: goToDefineKeys(); break;
            #endif

            default:
                Debug.LogError("Unknown next step");
            break;
        }
    }

    /// <summary>
    /// Move to the upper level.
    /// </summary>
    private void goBack()
    {
        if (modified)
        {
            askSaving=!askSaving;

            if (askSaving)
            {
                saveDialogRect=new Rect(Screen.width*0.3f, Screen.height*0.3f, Screen.width*0.4f, Screen.height*0.4f);
                nextStep=NextStep.ToBack;
            }
        }
        else
        {
            if (currentState==State.InOptionsList)
            {
                Debug.Log("Go to game menu");

                save();

                SendMessage("OnOptionsClosed");
            }
#if MENU_DEFINE_KEYS
            else
            if (currentState==State.InDefineKeys)
            {
                goToControlsOptions();
            }
#endif
            else
            {
                goToOptionsList((int)currentState-1);
            }
        }
    }

    /// <summary>
    /// Move to options list and highlight item by index.
    /// </summary>
    /// <param name="index">Item index.</param>
    private void goToOptionsList(int index)
    {
        Debug.Log("Go to options list");

        scrollPosition = Vector2.zero;
        currentState   = State.InOptionsList;
        currentItem    = index;
        itemsCount     = 1;

#if MENU_GAME
        ++itemsCount;
#endif

#if MENU_AUDIO
        ++itemsCount;
#endif

#if MENU_VIDEO
        ++itemsCount;
#endif

#if MENU_CONTROLS
        ++itemsCount;
#endif
    }

#if MENU_GAME
    /// <summary>
    /// Move to game options.
    /// </summary>
    private void goToGameOptions()
    {
        Debug.Log("Go to game options");

        scrollPosition = Vector2.zero;
        currentState   = State.InGameOptions;
        currentItem    = 0;
        itemsCount     = 1;

#if OPTION_LANGUAGE
        ++itemsCount;
        updateLanguageScroller();
#endif

#if OPTION_DIFFICULTY
        ++itemsCount;
        difficultyScroller.currentIndex=mDifficulty;
#endif

#if OPTION_BLOOD
        ++itemsCount;
        bloodCheckBox.isChecked=mBlood;
#endif

#if OPTION_USE_HINTS
        ++itemsCount;
        useHintsCheckBox.isChecked=mUseHints;
#endif

#if OPTION_AUTOSAVE
        ++itemsCount;
        autosaveCheckBox.isChecked=mAutosave;
#endif
    }
#endif

#if MENU_AUDIO
    /// <summary>
    /// Move to audio options.
    /// </summary>
    private void goToAudioOptions()
    {
        Debug.Log("Go to sound options");

        scrollPosition = Vector2.zero;
        currentState   = State.InAudioOptions;
        currentItem    = 0;
        itemsCount     = 1;

#if OPTION_SOUND
        ++itemsCount;
        soundCheckBox.isChecked=mSound;
#endif

#if OPTION_MASTER_VOLUME
        ++itemsCount;
        masterVolumeSlider.value=mMasterVolume;
#endif

#if OPTION_MUSIC_VOLUME
        ++itemsCount;
        musicVolumeSlider.value=mMusicVolume;
#endif

#if OPTION_VOICE_VOLUME
        ++itemsCount;
        voiceVolumeSlider.value=mVoiceVolume;
#endif

#if OPTION_EFFECTS_VOLUME
        ++itemsCount;
        effectsVolumeSlider.value=mEffectsVolume;
#endif

#if OPTION_SUBTITLES
        ++itemsCount;
        subtitlesCheckBox.isChecked=mSubtitles;
#endif
    }
#endif

#if MENU_VIDEO
    /// <summary>
    /// Move to video options.
    /// </summary>
    private void goToVideoOptions()
    {
        Debug.Log("Go to video options");

        scrollPosition = Vector2.zero;
        currentState   = State.InVideoOptions;
        currentItem    = 0;
        itemsCount     = 1;

#if OPTION_SHOW_FPS
        ++itemsCount;
        showFPSCheckBox.isChecked=mShowFPS;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        itemsCount+=2;

        fullScreenCheckBox.isChecked=mFullScreen;
        updateResolutionScroller();
#endif

#if OPTION_QUALITY
        ++itemsCount;
        qualityScroller.currentIndex=mQuality;
#endif
    }
#endif

#if MENU_CONTROLS
    /// <summary>
    /// Move to controls options.
    /// </summary>
    private void goToControlsOptions()
    {
        Debug.Log("Go to controls options");

        scrollPosition = Vector2.zero;
        currentState   = State.InControlsOptions;
        currentItem    = 0;
        itemsCount     = 1;

#if MENU_DEFINE_KEYS
        ++itemsCount;
#endif

#if OPTION_ALWAYS_RUN
        ++itemsCount;
        alwaysRunCheckBox.isChecked=mAlwaysRun;
#endif

#if OPTION_AUTO_AIM
        ++itemsCount;
        autoAimCheckBox.isChecked=mAutoAim;
#endif

#if OPTION_MOUSE_SENSITIVITY
        ++itemsCount;
        mouseSensitivitySlider.value=mMouseSensitivity;
#endif

#if OPTION_INVERT_MOUSE_Y
        ++itemsCount;
        invertMouseYCheckBox.isChecked=mInvertMouseY;
#endif

#if OPTION_INPUT_DEVICE
        ++itemsCount;
        inputDeviceScroller.currentIndex=(int)inputDevice;
#endif
    }
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Move to keys definition.
    /// </summary>
    private void goToDefineKeys()
    {
        if (modified)
        {
            askSaving=!askSaving;

            if (askSaving)
            {
                saveDialogRect=new Rect(Screen.width*0.3f, Screen.height*0.3f, Screen.width*0.4f, Screen.height*0.4f);
                nextStep=NextStep.ToDefineKeys;
            }
        }
        else
        {
            Debug.Log("Go to define keys");

            scrollPosition = Vector2.zero;
            currentState   = State.InDefineKeys;
            currentItem    = 0;
            itemsCount     = controlSetters.Count+1;

            int cur=0;

            foreach (KeyMapping key in InputControl.getKeys())
            {
                controlSetters[cur].keyMapping=key;

                ++cur;
            }
        }
    }
#endif

    /// <summary>
    /// Apply changes that was made.
    /// </summary>
    public void applyChanges()
    {
        modified  = false;
        askSaving = false;

        switch(currentState)
        {
            case State.InOptionsList:
                Debug.LogError("Never reach this code");
            break;

#if MENU_GAME
            case State.InGameOptions:     applyChangesInGameOptions    (); break;
#endif

#if MENU_AUDIO
            case State.InAudioOptions:    applyChangesInAudioOptions   (); break;
#endif

#if MENU_VIDEO
            case State.InVideoOptions:    applyChangesInVideoOptions   (); break;
#endif

#if MENU_CONTROLS
            case State.InControlsOptions: applyChangesInControlsOptions(); break;
#endif

#if MENU_DEFINE_KEYS
            case State.InDefineKeys:      applyChangesInDefineKeys     (); break;
#endif

            default:
                Debug.LogError("Unknown state");
            break;
        }

        save();
    }

#if MENU_GAME
    /// <summary>
    /// Apply changes that was made in game options.
    /// </summary>
    private void applyChangesInGameOptions()
    {
#if OPTION_LANGUAGE
        LanguageManager languageManager=LanguageManager.Instance;

        List<CultureInfo> availableLanguages=LanguageManager.Instance.AvailableLanguagesCultureInfo;
        int languageIndex=languageScroller.currentIndex;

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
#endif

#if OPTION_DIFFICULTY
        int difficultyIndex=difficultyScroller.currentIndex;

        if (mDifficulty!=difficultyIndex)
        {
            mDifficulty=difficultyIndex;
        }
#endif

#if OPTION_BLOOD
        bool aBlood=bloodCheckBox.isChecked;

        if (mBlood!=aBlood)
        {
            mBlood=aBlood;
        }
#endif

#if OPTION_USE_HINTS
        bool aUseHints=useHintsCheckBox.isChecked;

        if (mUseHints!=aUseHints)
        {
            mUseHints=aUseHints;
        }
#endif

#if OPTION_AUTOSAVE
        bool aAutosave=autosaveCheckBox.isChecked;

        if (mAutosave!=aAutosave)
        {
            mAutosave=aAutosave;
        }
#endif
    }
#endif

#if MENU_AUDIO
    /// <summary>
    /// Apply changes that was made in audio options.
    /// </summary>
    private void applyChangesInAudioOptions()
    {
#if OPTION_SOUND
        bool aSound=soundCheckBox.isChecked;

        if (mSound!=aSound)
        {
            mSound=aSound;
        }
#endif

#if OPTION_MASTER_VOLUME
        float aMasterVolume=masterVolumeSlider.value;

        if (mMasterVolume!=aMasterVolume)
        {
            mMasterVolume=aMasterVolume;
        }
#endif

#if OPTION_MUSIC_VOLUME
        float aMusicVolume=musicVolumeSlider.value;

        if (mMusicVolume!=aMusicVolume)
        {
            mMusicVolume=aMusicVolume;
        }
#endif

#if OPTION_VOICE_VOLUME
        float aVoiceVolume=voiceVolumeSlider.value;

        if (mVoiceVolume!=aVoiceVolume)
        {
            mVoiceVolume=aVoiceVolume;
        }
#endif

#if OPTION_EFFECTS_VOLUME
        float aEffectsVolume=effectsVolumeSlider.value;

        if (mEffectsVolume!=aEffectsVolume)
        {
            mEffectsVolume=aEffectsVolume;
        }
#endif

#if OPTION_SUBTITLES
        bool aSubtitles=subtitlesCheckBox.isChecked;

        if (mSubtitles!=aSubtitles)
        {
            mSubtitles=aSubtitles;
        }
#endif

        // ------------------------------------------

        setAudioListenerVolume();
    }
#endif

#if MENU_VIDEO
    /// <summary>
    /// Apply changes that was made in video options.
    /// </summary>
    private void applyChangesInVideoOptions()
    {
#if OPTION_SHOW_FPS
        bool aShowFPS=showFPSCheckBox.isChecked;

        if (mShowFPS!=aShowFPS)
        {
            mShowFPS=aShowFPS;
            FPSCounter.isOn=mShowFPS;
        }
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
        bool   aFullScreen = fullScreenCheckBox.isChecked;
        string resolution  = resolutionScroller.selectedItem;

        if (mFullScreen!=aFullScreen || !mResolution.Equals(resolution))
        {
            mFullScreen = aFullScreen;
            mResolution = resolution;

            changeResolution();
        }
#endif

#if OPTION_QUALITY
        int qualityIndex=qualityScroller.currentIndex;

        if (mQuality!=qualityIndex)
        {
            mQuality=qualityIndex;
            QualitySettings.SetQualityLevel(mQuality);
        }
#endif
    }
#endif

#if MENU_CONTROLS
    /// <summary>
    /// Apply changes that was made in controls options.
    /// </summary>
    private void applyChangesInControlsOptions()
    {
#if OPTION_ALWAYS_RUN
        bool aAlwaysRun=alwaysRunCheckBox.isChecked;

        if (mAlwaysRun!=aAlwaysRun)
        {
            mAlwaysRun=aAlwaysRun;
        }
#endif

#if OPTION_AUTO_AIM
        bool aAutoAim=autoAimCheckBox.isChecked;

        if (mAutoAim!=aAutoAim)
        {
            mAutoAim=aAutoAim;
        }
#endif

#if OPTION_MOUSE_SENSITIVITY
        float aMouseSensitivity=mouseSensitivitySlider.value;

        if (mMouseSensitivity!=aMouseSensitivity)
        {
            mMouseSensitivity=aMouseSensitivity;

#if PACKAGE_INPUT_CONTROL
            InputControl.mouseSensitivity=mMouseSensitivity;
#endif
        }
#endif

#if OPTION_INVERT_MOUSE_Y
        bool aInvertMouseY=invertMouseYCheckBox.isChecked;

        if (mInvertMouseY!=aInvertMouseY)
        {
            mInvertMouseY=aInvertMouseY;

#if PACKAGE_INPUT_CONTROL
            InputControl.invertMouseY=mInvertMouseY;
#endif
        }
#endif

#if OPTION_INPUT_DEVICE
        int inputDeviceIndex=inputDeviceScroller.currentIndex;

        if ((int)inputDevice!=inputDeviceIndex)
        {
            InputControl.preferredInputDevice=(InputDevice)inputDeviceIndex;
        }
#endif
    }
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Apply changes that was made in keys definition.
    /// </summary>
    private void applyChangesInDefineKeys()
    {
        int cur=0;

        foreach (KeyMapping key in InputControl.getKeys())
        {
            key.set(controlSetters[cur].keyMapping);

            ++cur;
        }
    }
#endif

#if OPTION_LANGUAGE
    /// <summary>
    /// Sets value to languageScroller.
    /// </summary>
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

        languageScroller.currentIndex=languageIndex;
    }
#endif

    /// <summary>
    /// Sets the global volume.
    /// </summary>
    private static void setAudioListenerVolume()
    {
        float masterVolume=1f;

#if OPTION_SOUND
        if (!mSound)
        {
            masterVolume=0f;
        }
#endif

#if OPTION_MASTER_VOLUME
        masterVolume=mMasterVolume*masterVolume;
#endif

        if (AudioListener.volume!=masterVolume)
        {
            AudioListener.volume=masterVolume;
        }
    }

#if OPTION_FULL_SCREEN_AND_RESOLUTION
    /// <summary>
    /// Sets value to resolutionScroller.
    /// </summary>
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

        resolutionScroller.currentIndex=resolutionIndex;
    }

    /// <summary>
    /// Apply new screen resolution and full screen mode.
    /// </summary>
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
#endif

#if MENU_DEFINE_KEYS
    /// <summary>
    /// Convert input text representation to <see cref="CustomInput"/>.
    /// </summary>
    /// <returns>Converted input.</returns>
    /// <param name="value">Input text representation.</param>
    private static CustomInput StringToCustomInput(string value)
    {
        CustomInput res=null;

        res=JoystickInput.FromString(value);

        if (res!=null)
        {
            return res;
        }

        res=MouseInput.FromString(value);

        if (res!=null)
        {
            return res;
        }

        res=KeyboardInput.FromString(value);

        return res;
    }
#endif

    /// <summary>
    /// Load options.
    /// </summary>
    public static void load()
    {
        Debug.Log("Loading settings");

#if PACKAGE_INI_FILE
        IniFile iniFile=new IniFile("Settings");

        if (iniFile.count()==0)
        {
            save();
            iniFile.load("Settings");
        }
#endif

        #region Game
#if OPTION_LANGUAGE
        LanguageManager languageManager=LanguageManager.Instance;
#if PACKAGE_INI_FILE
        mLanguage=iniFile.get          ("Game.Language", languageManager.GetSystemLanguage());
#else
        mLanguage=PlayerPrefs.GetString("Game.Language", languageManager.GetSystemLanguage());
#endif

        if (!languageManager.IsLanguageSupported(mLanguage))
        {
            mLanguage="en";
        }

        Debug.Log("Application language: "+mLanguage);
        languageManager.ChangeLanguage(mLanguage);
#endif

#if OPTION_DIFFICULTY
#if PACKAGE_INI_FILE
        mDifficulty=iniFile.get       ("Game.Difficulty", difficultyCount/2);
#else
        mDifficulty=PlayerPrefs.GetInt("Game.Difficulty", difficultyCount/2);
#endif

        Debug.Log("Difficulty:           "+mDifficulty.ToString());
#endif

#if OPTION_BLOOD
#if PACKAGE_INI_FILE
        mBlood=iniFile.get       ("Game.Blood", true);
#else
        mBlood=PlayerPrefs.GetInt("Game.Blood", 1)==1;
#endif

        Debug.Log("Blood:                "+mBlood.ToString());
#endif

#if OPTION_USE_HINTS
#if PACKAGE_INI_FILE
        mUseHints=iniFile.get       ("Game.UseHints", true);
#else
        mUseHints=PlayerPrefs.GetInt("Game.UseHints", 1)==1;
#endif

        Debug.Log("Use hints:            "+mUseHints.ToString());
#endif

#if OPTION_AUTOSAVE
#if PACKAGE_INI_FILE
        mAutosave=iniFile.get       ("Game.Autosave", true);
#else
        mAutosave=PlayerPrefs.GetInt("Game.Autosave", 1)==1;
#endif

        Debug.Log("Autosave:             "+mAutosave.ToString());
#endif
        #endregion

        #region Audio
#if OPTION_SOUND
#if PACKAGE_INI_FILE
        mSound=iniFile.get       ("Audio.Sound", true);
#else
        mSound=PlayerPrefs.GetInt("Audio.Sound", 1)==1;
#endif

        Debug.Log("Sound:          "+mSound.ToString());
#endif

#if OPTION_MASTER_VOLUME
#if PACKAGE_INI_FILE
        mMasterVolume=iniFile.get         ("Audio.MasterVolume", 1f);
#else
        mMasterVolume=PlayerPrefs.GetFloat("Audio.MasterVolume", 1f);
#endif

        Debug.Log("Master volume:  "+mMasterVolume.ToString());
#endif

#if OPTION_MUSIC_VOLUME
#if PACKAGE_INI_FILE
        mMusicVolume=iniFile.get         ("Audio.MusicVolume", 1f);
#else
        mMusicVolume=PlayerPrefs.GetFloat("Audio.MusicVolume", 1f);
#endif

        Debug.Log("Music volume:   "+mMusicVolume.ToString());
#endif

#if OPTION_VOICE_VOLUME
#if PACKAGE_INI_FILE
        mVoiceVolume=iniFile.get         ("Audio.VoiceVolume", 1f);
#else
        mVoiceVolume=PlayerPrefs.GetFloat("Audio.VoiceVolume", 1f);
#endif

        Debug.Log("Voice volume:   "+mVoiceVolume.ToString());
#endif

#if OPTION_EFFECTS_VOLUME
#if PACKAGE_INI_FILE
        mEffectsVolume=iniFile.get         ("Audio.EffectsVolume", 1f);
#else
        mEffectsVolume=PlayerPrefs.GetFloat("Audio.EffectsVolume", 1f);
#endif

        Debug.Log("Effects volume: "+mEffectsVolume.ToString());
#endif

#if OPTION_SUBTITLES
#if PACKAGE_INI_FILE
        mSubtitles=iniFile.get       ("Audio.Subtitles", true);
#else
        mSubtitles=PlayerPrefs.GetInt("Audio.Subtitles", 1)==1;
#endif

        Debug.Log("Subtitles:      "+mSubtitles.ToString());
#endif

        setAudioListenerVolume();
        #endregion

        #region Video
#if OPTION_SHOW_FPS
#if PACKAGE_INI_FILE
        mShowFPS=iniFile.get       ("Video.ShowFPS", false);
#else
        mShowFPS=PlayerPrefs.GetInt("Video.ShowFPS", 0)==1;
#endif

        Debug.Log("Show FPS:       "+mShowFPS.ToString());
        FPSCounter.isOn=mShowFPS;
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
#if PACKAGE_INI_FILE
        mFullScreen=iniFile.get          ("Video.FullScreen", true);
        mResolution=iniFile.get          ("Video.Resolution", "800x600");
#else
        mFullScreen=PlayerPrefs.GetInt   ("Video.FullScreen", 1)==1;
        mResolution=PlayerPrefs.GetString("Video.Resolution", "800x600");
#endif

        Debug.Log("Full screen:    "+mFullScreen.ToString());
        Debug.Log("Resolution:     "+mResolution);

        changeResolution();
#endif

#if OPTION_QUALITY
#if PACKAGE_INI_FILE
        mQuality=iniFile.get       ("Video.Quality", QualitySettings.GetQualityLevel());
#else
        mQuality=PlayerPrefs.GetInt("Video.Quality", QualitySettings.GetQualityLevel());
#endif
        if (mQuality>=QualitySettings.names.Length)
        {
            // TODO: Add custom quality
            mQuality=0;
        }

        Debug.Log("Video quality:  "+mQuality.ToString());
        QualitySettings.SetQualityLevel(mQuality);
#endif
        #endregion

        #region Controls
#if MENU_DEFINE_KEYS
        foreach (KeyMapping key in InputControl.getKeys())
        {
#if PACKAGE_INI_FILE
            key.primaryInput   = StringToCustomInput(iniFile.get          ("Controls."+key.name+".Primary",   key.primaryInput.ToString()));
            key.secondaryInput = StringToCustomInput(iniFile.get          ("Controls."+key.name+".Secondary", key.secondaryInput.ToString()));
            key.thirdInput     = StringToCustomInput(iniFile.get          ("Controls."+key.name+".Third",     key.thirdInput.ToString()));
#else
            key.primaryInput   = StringToCustomInput(PlayerPrefs.GetString("Controls."+key.name+".Primary",   key.primaryInput.ToString()));
            key.secondaryInput = StringToCustomInput(PlayerPrefs.GetString("Controls."+key.name+".Secondary", key.secondaryInput.ToString()));
            key.thirdInput     = StringToCustomInput(PlayerPrefs.GetString("Controls."+key.name+".Third",     key.thirdInput.ToString()));
#endif

            Debug.Log("Key: "+key.name+" ; Primary   = "+key.primaryInput.ToString());
            Debug.Log("Key: "+key.name+" ; Secondary = "+key.secondaryInput.ToString());
            Debug.Log("Key: "+key.name+" ; Third     = "+key.thirdInput.ToString());
        }
#endif

#if OPTION_ALWAYS_RUN
#if PACKAGE_INI_FILE
        mAlwaysRun=iniFile.get       ("Controls.AlwaysRun", false);
#else
        mAlwaysRun=PlayerPrefs.GetInt("Controls.AlwaysRun", 0)==1;
#endif

        Debug.Log("Always run:        "+mAlwaysRun.ToString());
#endif

#if OPTION_AUTO_AIM
#if PACKAGE_INI_FILE
        mAutoAim=iniFile.get       ("Controls.AutoAim", false);
#else
        mAutoAim=PlayerPrefs.GetInt("Controls.AutoAim", 0)==1;
#endif

        Debug.Log("Auto aim:          "+mAutoAim.ToString());
#endif

#if OPTION_MOUSE_SENSITIVITY
#if PACKAGE_INI_FILE
        mMouseSensitivity=iniFile.get         ("Controls.MouseSensitivity", 1f);
#else
        mMouseSensitivity=PlayerPrefs.GetFloat("Controls.MouseSensitivity", 1f);
#endif

        Debug.Log("Mouse sensitivity: "+mouseSensitivity.ToString());

#if PACKAGE_INPUT_CONTROL
        InputControl.mouseSensitivity=mMouseSensitivity;
#endif
#endif

#if OPTION_INVERT_MOUSE_Y
#if PACKAGE_INI_FILE
        mInvertMouseY=iniFile.get       ("Controls.InvertMouseY", false);
#else
        mInvertMouseY=PlayerPrefs.GetInt("Controls.InvertMouseY", 0)==1;
#endif

        Debug.Log("Invert mouse Y:    "+mInvertMouseY.ToString());

#if PACKAGE_INPUT_CONTROL
        InputControl.invertMouseY=mInvertMouseY;
#endif
#endif

#if OPTION_INPUT_DEVICE
#if PACKAGE_INI_FILE
        InputControl.preferredInputDevice=(InputDevice)iniFile.get       ("Controls.InputDevice", (int)InputDevice.Any);
#else
        InputControl.preferredInputDevice=(InputDevice)PlayerPrefs.GetInt("Controls.InputDevice", (int)InputDevice.Any);
#endif

        Debug.Log("Input device:      "+InputControl.preferredInputDevice.ToString());
#endif
        #endregion
    }

    /// <summary>
    /// Save options.
    /// </summary>
    public static void save()
    {
#if PACKAGE_INI_FILE
        #region Get available languages
#if PACKAGE_SMART_LOCALIZATION
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
#endif
        #endregion



        IniFile iniFile=new IniFile();
#endif

        #region Game
#if OPTION_LANGUAGE
#if PACKAGE_INI_FILE
        iniFile.set          ("Game.Language", mLanguage, "Application language: "+languagesList);
#else
        PlayerPrefs.SetString("Game.Language", mLanguage);
#endif
#endif

#if OPTION_DIFFICULTY
#if PACKAGE_INI_FILE
        iniFile.set       ("Game.Difficulty", mDifficulty, "Difficulty: 0-"+(difficultyCount-1).ToString());
#else
        PlayerPrefs.SetInt("Game.Difficulty", mDifficulty);
#endif
#endif

#if OPTION_BLOOD
#if PACKAGE_INI_FILE
        iniFile.set       ("Game.Blood", mBlood, "Show blood: True/False");
#else
        PlayerPrefs.SetInt("Game.Blood", mBlood ? 1 : 0);
#endif
#endif

#if OPTION_USE_HINTS
#if PACKAGE_INI_FILE
        iniFile.set       ("Game.UseHints", mUseHints, "Use hints: True/False");
#else
        PlayerPrefs.SetInt("Game.UseHints", mUseHints ? 1 : 0);
#endif
#endif

#if OPTION_AUTOSAVE
#if PACKAGE_INI_FILE
        iniFile.set       ("Game.Autosave", mAutosave, "Enable autosave: True/False");
#else
        PlayerPrefs.SetInt("Game.Autosave", mAutosave ? 1 : 0);
#endif
#endif
        #endregion

        #region Audio
#if OPTION_SOUND
#if PACKAGE_INI_FILE
        iniFile.set       ("Audio.Sound", mSound, "Sound: True/False");
#else
        PlayerPrefs.SetInt("Audio.Sound", mSound ? 1 : 0);
#endif
#endif

#if OPTION_MASTER_VOLUME
#if PACKAGE_INI_FILE
        iniFile.set         ("Audio.MasterVolume", mMasterVolume, "Master volume: 0-1");
#else
        PlayerPrefs.SetFloat("Audio.MasterVolume", mMasterVolume);
#endif
#endif

#if OPTION_MUSIC_VOLUME
#if PACKAGE_INI_FILE
        iniFile.set         ("Audio.MusicVolume", mMusicVolume, "Music volume: 0-1");
#else
        PlayerPrefs.SetFloat("Audio.MusicVolume", mMusicVolume);
#endif
#endif

#if OPTION_VOICE_VOLUME
#if PACKAGE_INI_FILE
        iniFile.set         ("Audio.VoiceVolume", mVoiceVolume, "Voice volume: 0-1");
#else
        PlayerPrefs.SetFloat("Audio.VoiceVolume", mVoiceVolume);
#endif
#endif

#if OPTION_EFFECTS_VOLUME
#if PACKAGE_INI_FILE
        iniFile.set         ("Audio.EffectsVolume", mEffectsVolume, "Effects volume: 0-1");
#else
        PlayerPrefs.SetFloat("Audio.EffectsVolume", mEffectsVolume);
#endif
#endif

#if OPTION_SUBTITLES
#if PACKAGE_INI_FILE
        iniFile.set       ("Audio.Subtitles", mSubtitles, "Subtitles: True/False");
#else
        PlayerPrefs.SetInt("Audio.Subtitles", mSubtitles ? 1 : 0);
#endif
#endif
        #endregion

        #region Video
#if OPTION_SHOW_FPS
#if PACKAGE_INI_FILE
        iniFile.set       ("Video.ShowFPS", mShowFPS, "Show FPS: True/False");
#else
        PlayerPrefs.SetInt("Video.ShowFPS", mShowFPS ? 1 : 0);
#endif
#endif

#if OPTION_FULL_SCREEN_AND_RESOLUTION
#if PACKAGE_INI_FILE
        iniFile.set          ("Video.FullScreen", mFullScreen,    "Full screen mode: True/False");
        iniFile.set          ("Video.Resolution", mResolution,    "Screen resolution: WIDTH x HEIGHT : RATE Hz");
#else
        PlayerPrefs.SetInt   ("Video.FullScreen", mFullScreen ? 1 : 0);
        PlayerPrefs.SetString("Video.Resolution", mResolution);
#endif
#endif

#if OPTION_QUALITY
#if PACKAGE_INI_FILE
		iniFile.set       ("Video.Quality", mQuality, "Video quality: 0-"+QualitySettings.names.Length.ToString()+" ("+QualitySettings.names.Length.ToString()+" - Custom)");
#else
        PlayerPrefs.SetInt("Video.Quality", mQuality);
#endif
#endif
        #endregion

        #region Controls
#if MENU_DEFINE_KEYS
#if PACKAGE_INI_FILE
        bool firstKey=true;
#endif

        foreach (KeyMapping key in InputControl.getKeys())
        {
#if PACKAGE_INI_FILE
            if (firstKey)
            {
                firstKey=false;
                iniFile.set("Controls."+key.name+".Primary", key.primaryInput.ToString(), "Controls");
            }
            else
            {
                iniFile.set("Controls."+key.name+".Primary", key.primaryInput.ToString());
            }

            iniFile.set("Controls."+key.name+".Secondary",   key.secondaryInput.ToString());
            iniFile.set("Controls."+key.name+".Third",       key.thirdInput.ToString());
#else
            PlayerPrefs.SetString("Controls."+key.name+".Primary",   key.primaryInput.ToString());
            PlayerPrefs.SetString("Controls."+key.name+".Secondary", key.secondaryInput.ToString());
            PlayerPrefs.SetString("Controls."+key.name+".Third",     key.thirdInput.ToString());
#endif
        }
#endif

#if OPTION_ALWAYS_RUN
#if PACKAGE_INI_FILE
        iniFile.set       ("Controls.AlwaysRun", mAlwaysRun, "Always run: True/False");
#else
        PlayerPrefs.SetInt("Controls.AlwaysRun", mAlwaysRun ? 1 : 0);
#endif
#endif

#if OPTION_AUTO_AIM
#if PACKAGE_INI_FILE
        iniFile.set       ("Controls.AutoAim", mAutoAim, "Auto aim: True/False");
#else
        PlayerPrefs.SetInt("Controls.AutoAim", mAutoAim ? 1 : 0);
#endif
#endif

#if OPTION_MOUSE_SENSITIVITY
#if PACKAGE_INI_FILE
        iniFile.set         ("Controls.MouseSensitivity", mMouseSensitivity, "Mouse sensitivity: 0-"+mouseSensitivityMaximum.ToString());
#else
        PlayerPrefs.SetFloat("Controls.MouseSensitivity", mMouseSensitivity);
#endif
#endif

#if OPTION_INVERT_MOUSE_Y
#if PACKAGE_INI_FILE
        iniFile.set       ("Controls.InvertMouseY", mInvertMouseY, "Invert mouse Y: True/False");
#else
        PlayerPrefs.SetInt("Controls.InvertMouseY", mInvertMouseY ? 1 : 0);
#endif
#endif

#if OPTION_INPUT_DEVICE
#if PACKAGE_INI_FILE
        iniFile.set       ("Controls.InputDevice", (int)inputDevice, "Input device: 0-"+(Enum.GetNames(typeof(InputDevice)).Length-1).ToString());
#else
        PlayerPrefs.SetInt("Controls.InputDevice", (int)inputDevice);
#endif
#endif
        #endregion

#if PACKAGE_INI_FILE
        iniFile.save("Settings");
#endif
    }
}
