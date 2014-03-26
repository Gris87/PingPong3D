using UnityEngine;
using System.Collections;

public static class SceneManager
{
    private static Hashtable mSceneArguments;
    public static Hashtable sceneArguments
    {
        get
        {
           return mSceneArguments;
        }
    }



    public static void LoadScene(string sceneName)
    {
        LoadScene(sceneName, new Hashtable());
    }

    public static void LoadScene(string sceneName, Hashtable arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevel(sceneName);
    }



    public static void LoadSceneAdditive(string sceneName)
    {
        LoadSceneAdditive(sceneName, new Hashtable());
    }

    public static void LoadSceneAdditive(string sceneName, Hashtable arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAdditive(sceneName);
    }



    public static void LoadSceneAsync(string sceneName)
    {
        LoadSceneAsync(sceneName, new Hashtable());
    }

    public static void LoadSceneAsync(string sceneName, Hashtable arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAsync(sceneName);
    }



    public static void LoadSceneAdditiveAsync(string sceneName)
    {
        LoadSceneAdditiveAsync(sceneName, new Hashtable());
    }

    public static void LoadSceneAdditiveAsync(string sceneName, Hashtable arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAdditiveAsync(sceneName);
    }
}
