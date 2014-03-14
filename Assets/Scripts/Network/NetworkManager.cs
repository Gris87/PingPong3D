using UnityEngine;
using System;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "GrisPingPong3D";
	private const int    port     = 46115;

	private GUIStyle centerTextStyle;

	private bool isServerMode;
	private bool serverInitializing;

	// Use this for initialization
	void Start()
	{
		centerTextStyle=new GUIStyle();

		centerTextStyle.alignment=TextAnchor.MiddleCenter;
		centerTextStyle.clipping=TextClipping.Overflow;
		centerTextStyle.fontSize=24;
		centerTextStyle.normal.textColor=Color.white;

		// -----------------------------------------------------------

		Hashtable arguments=SceneManager.GetSceneArguments();
		
		if (arguments!=null && arguments.ContainsKey("serverMode"))
		{
			isServerMode=(bool)arguments["serverMode"];
			
			if (isServerMode)
			{
				StartServer();
			}
			else
			{
			}
		}
		else
		{
			Debug.LogError("Incorrect usage");
		}
	}
	
	private void StartServer()
	{
		serverInitializing=true;

		string roomName;

		roomName = Environment.UserName;

		Network.InitializeServer(2, port, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, roomName);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server initializied");

		serverInitializing=false;
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 80, 30), "Back"))
		{
			cancel();
		}

		if (isServerMode)
		{
			if (serverInitializing)
			{
				GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Initializing server", centerTextStyle);
			}
			else
			{
				GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Waiting for opponent", centerTextStyle);
			}
		}
	}

	void cancel()
	{
		SceneManager.LoadScene("GameMenu");
	}
}
