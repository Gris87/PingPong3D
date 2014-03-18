using UnityEngine;
using System.Collections;

public class SceneManager
{
    private static Hashtable sceneArguments;

    public static void LoadScene(string sceneName)
    {
        LoadScene(sceneName, new Hashtable());
    }

    public static void LoadScene(string sceneName, Hashtable arguments)
    {
        sceneArguments=arguments;
        Application.LoadLevel(sceneName);
    }

    public static Hashtable GetSceneArguments()
    {
        return sceneArguments;
    }
}
