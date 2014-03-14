using UnityEngine;
using System;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private enum ServerState
	{
		ServerInit,
		ServerRegister,
		ServerStarted,
		ServerError
	}

	private const string typeName = "GrisPingPong3D";
	private const int    port     = 46115;

	private GUIStyle centerTextStyle;
	private Vector2  hostsScrollPosition = Vector2.zero;

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
		string roomName;
		roomName=Environment.UserName;

		serverState=ServerState.ServerInit;
		if (Network.InitializeServer(2, port, !Network.HavePublicAddress())==NetworkConnectionError.NoError)
		{
			serverState=ServerState.ServerRegister;
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
		Debug.Log("Joining to server: "+hostData.gameName+" | "+hostData.ip[0]+":"+hostData.port.ToString());
		Network.Connect(hostData);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server initializied");
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
				Debug.Log("Servers searching finished");
			break;
			case MasterServerEvent.RegistrationSucceeded:
				serverState=ServerState.ServerStarted;
			break;
			case MasterServerEvent.RegistrationFailedGameName:
			case MasterServerEvent.RegistrationFailedGameType:
			case MasterServerEvent.RegistrationFailedNoServer:
				serverState=ServerState.ServerError;
			break;
			default:
				Debug.LogError("Unknown event");
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
				case ServerState.ServerRegister:
					GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Registering server", centerTextStyle);
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
			HostData[] hostsList=MasterServer.PollHostList();

			if (hostsList!=null)
			{
				hostsScrollPosition=GUI.BeginScrollView(new Rect(20, 60, Screen.width-40, Screen.height-80), hostsScrollPosition, new Rect(0, 0, Screen.width-60, 40+(hostsList.Length-1)*50));

				for (int i=0; i<hostsList.Length; ++i)
				{
					if (GUI.Button(new Rect(0, 50*i, 300, 40), hostsList[i].gameName))
					{
						JoinServer(hostsList[i]);
					}
				}

				GUI.EndScrollView();
			}
		}
	}

	void cancel()
	{
		if (isServerMode)
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}

		SceneManager.LoadScene("GameMenu");
	}
}
