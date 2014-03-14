using UnityEngine;
using System;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private enum ServerState
	{
		ServerInit,
		ServerStarted,
		ServerError
	}

	private const string typeName = "GrisPingPong3D";
	private const int    port     = 46115;

	private GUIStyle   centerTextStyle;
	private HostData[] hostList = null;

	private bool        isServerMode;
	private ServerState serverState;

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
				RefreshHostList();
			}
		}
		else
		{
			Debug.LogError("Incorrect usage");
		}
	}
	
	private void StartServer()
	{
		serverState=ServerState.ServerInit;

		string roomName;
		roomName = Environment.UserName;

		if (Network.InitializeServer(2, port, !Network.HavePublicAddress())==NetworkConnectionError.NoError)
		{
			MasterServer.RegisterHost(typeName, roomName);
		}
		else
		{
			serverState=ServerState.ServerError;
		}
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server initializied");

		serverState=ServerState.ServerStarted;
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Joined to server");
	}
	
	void OnMasterServerEvent(MasterServerEvent aEvent)
	{	
		switch (aEvent)
		{
			case MasterServerEvent.HostListReceived:
				hostList=MasterServer.PollHostList();
			break;
			case MasterServerEvent.RegistrationFailedGameName:
			case MasterServerEvent.RegistrationFailedGameType:
			case MasterServerEvent.RegistrationFailedNoServer:
				serverState=ServerState.ServerError;
			break;
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 80, 30), "Back"))
		{
			cancel();
		}

		if (isServerMode)
		{
			switch (serverState)
			{
				case ServerState.ServerInit:
					GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Initializing server", centerTextStyle);
				break;
				case ServerState.ServerStarted:
					GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Waiting for opponent...", centerTextStyle);
				break;
				case ServerState.ServerError:
					GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Impossible to start server", centerTextStyle);
				break;
				default:
					Debug.LogError("Unknown state");
				break;
			}
		}
		else
		{

		}
	}

	void cancel()
	{
		SceneManager.LoadScene("GameMenu");
	}
}
