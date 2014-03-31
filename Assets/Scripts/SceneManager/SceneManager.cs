using UnityEngine;
using System.Collections.Generic;

public static class SceneManager
{
    private static Dictionary<string, object> mSceneArguments;

	/// <summary>
	/// Gets the scene arguments.
	/// </summary>
	/// <value>The scene arguments.</value>
	public static Dictionary<string, object> sceneArguments
    {
        get
        {
           return mSceneArguments;
        }
    }



	/// <summary>
	/// Loads the level by its name.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
    public static void LoadScene(string sceneName)
    {
		LoadScene(sceneName, new Dictionary<string, object>());
    }

	/// <summary>
	/// Loads the level with arguments by its name.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
	/// <param name="arguments">List of arguments.</param>
	public static void LoadScene(string sceneName, Dictionary<string, object> arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevel(sceneName);
    }



	/// <summary>
	/// Loads a level additively.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
    public static void LoadSceneAdditive(string sceneName)
    {
		LoadSceneAdditive(sceneName, new Dictionary<string, object>());
    }

	/// <summary>
	/// Loads a level with arguments additively.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
	/// <param name="arguments">List of arguments.</param>
	public static void LoadSceneAdditive(string sceneName, Dictionary<string, object> arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAdditive(sceneName);
    }



	/// <summary>
	/// Loads the level asynchronously in the background.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
    public static void LoadSceneAsync(string sceneName)
    {
		LoadSceneAsync(sceneName, new Dictionary<string, object>());
    }

	/// <summary>
	/// Loads the level with arguments asynchronously in the background.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
	/// <param name="arguments">List of arguments.</param>
	public static void LoadSceneAsync(string sceneName, Dictionary<string, object> arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAsync(sceneName);
    }



	/// <summary>
	/// Loads the level additively and asynchronously in the background.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
    public static void LoadSceneAdditiveAsync(string sceneName)
    {
		LoadSceneAdditiveAsync(sceneName, new Dictionary<string, object>());
    }

	/// <summary>
	/// Loads the level with arguments additively and asynchronously in the background.
	/// </summary>
	/// <param name="sceneName">Name of scene.</param>
	/// <param name="arguments">List of arguments.</param>
	public static void LoadSceneAdditiveAsync(string sceneName, Dictionary<string, object> arguments)
    {
        mSceneArguments=arguments;
        Application.LoadLevelAdditiveAsync(sceneName);
    }
}
