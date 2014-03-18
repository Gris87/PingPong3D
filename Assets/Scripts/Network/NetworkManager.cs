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

    private enum ClientState
    {
        ClientSearch,
        ClientJoin,
        ClientError
    }

    private const string typeName = "GrisPingPong3D";
    private const int    port     = 46115;

    private GUIStyle topRightTextStyle;
    private GUIStyle centerTextStyle;
    private GUIStyle hostTextStyle;
    private Vector2  hostsScrollPosition = Vector2.zero;

    private bool        isServerMode;
    private ServerState serverState;
    private ClientState clientState;
    private bool        refreshing;

    // Use this for initialization
    void Start()
    {
        #region Create text styles
        topRightTextStyle = new GUIStyle();
        centerTextStyle   = new GUIStyle();
        hostTextStyle     = new GUIStyle();

        topRightTextStyle.alignment=TextAnchor.UpperRight;
        topRightTextStyle.clipping=TextClipping.Overflow;
        topRightTextStyle.fontSize=24;
        topRightTextStyle.normal.textColor=Color.white;

        centerTextStyle.alignment=TextAnchor.MiddleCenter;
        centerTextStyle.clipping=TextClipping.Overflow;
        centerTextStyle.fontSize=24;
        centerTextStyle.normal.textColor=Color.white;

        hostTextStyle.alignment=TextAnchor.MiddleLeft;
        hostTextStyle.wordWrap=false;
        hostTextStyle.clipping=TextClipping.Clip;
        hostTextStyle.fontSize=14;
        hostTextStyle.normal.textColor=Color.white;
        #endregion

        // -----------------------------------------------------------

        #region Get mode from arguments
        Hashtable arguments=SceneManager.GetSceneArguments();

        if (arguments!=null && arguments.ContainsKey("serverMode"))
        {
            isServerMode=(bool)arguments["serverMode"];
        }
        else
        {
            Debug.LogError("Incorrect usage");
            isServerMode=true;
        }
        #endregion

        if (isServerMode)
        {
            StartServer();
        }
        else
        {
            clientState=ClientState.ClientSearch;
            RefreshHostList();
        }
    }

    private void StartServer()
    {
        string roomName;

        if (Application.platform==RuntimePlatform.Android)
        {
            roomName=SystemInfo.deviceModel+"_"+SystemInfo.deviceName+"_"+SystemInfo.deviceUniqueIdentifier;
        }
        else
        {
            roomName=Environment.UserName;
        }


        serverState=ServerState.ServerInit;
        if (Network.InitializeServer(1, port, !Network.HavePublicAddress())==NetworkConnectionError.NoError)
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
        Debug.Log("Requesting host list");

        refreshing=true;
        MasterServer.RequestHostList(typeName);
    }

    private void JoinServer(HostData hostData)
    {
        Debug.Log("Joining to server: "+hostData.gameName+" | "+hostData.ip[0]+":"+hostData.port.ToString());

        clientState=ClientState.ClientJoin;
        Network.Connect(hostData);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server initializied");
    }

    void OnFailedToConnect()
    {
        Debug.Log("Failed to join");

        clientState=ClientState.ClientError;
    }

    void OnConnectedToServer()
    {
        Debug.Log("Joined to server");

        startGame();
    }

    void OnPlayerConnected()
    {
        Debug.Log("Player connected");

        startGame();
    }

    void OnMasterServerEvent(MasterServerEvent aEvent)
    {
        switch (aEvent)
        {
            case MasterServerEvent.HostListReceived:
                Debug.Log("Servers searching finished");
                refreshing=false;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 120, 40), "Back"))
        {
            goBack();
        }

        if (isServerMode)
        {
            #region Draw server states
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
            #endregion
        }
        else
        {
            switch(clientState)
            {
                case ClientState.ClientSearch:
                {
                    #region Draw list of hosts
                    if (GUI.Button(new Rect(160, 20, 120, 40), "Refresh"))
                    {
                        RefreshHostList();
                    }

                    if (refreshing)
                    {
                        GUI.Label(new Rect(Screen.width-20, 20, 1, 1), "Refreshing...", topRightTextStyle);
                    }

                    int panelWidth  = Screen.width-40;
                    int panelHeight = Screen.height-100;

                    GUI.BeginGroup(new Rect(20, 80, panelWidth, panelHeight));
                    GUI.Box(new Rect(0, 0, panelWidth, panelHeight), "");

                    HostData[] hostsList=MasterServer.PollHostList();

                    if (hostsList!=null && hostsList.Length>0)
                    {
                        hostsScrollPosition=GUI.BeginScrollView(new Rect(4, 4, panelWidth-8, panelHeight-8), hostsScrollPosition, new Rect(0, 0, panelWidth-25, 40+(hostsList.Length-1)*50));

                        for (int i=0; i<hostsList.Length; ++i)
                        {
                            GUI.Label(new Rect(0, 50*i, panelWidth-190, 40), hostsList[i].gameName, hostTextStyle);
                            GUI.Label(new Rect(panelWidth-170, 50*i, 20, 40), hostsList[i].connectedPlayers.ToString()+"/"+hostsList[i].playerLimit.ToString(), hostTextStyle);

                            if (GUI.Button(new Rect(panelWidth-130, 50*i, 100, 40), "Connect"))
                            {
                                if (hostsList[i].connectedPlayers<hostsList[i].playerLimit)
                                {
                                    JoinServer(hostsList[i]);
                                }
                            }
                        }

                        GUI.EndScrollView();
                    }
                    else
                    {
                        if (!refreshing)
                        {
                            GUI.Label(new Rect(panelWidth/2, panelHeight/2, 1, 1), "Nothing found", centerTextStyle);
                        }
                    }

                    GUI.EndGroup();
                    #endregion
                }
                break;
                #region Draw client states
                case ClientState.ClientJoin:
                {
                    if (GUI.Button(new Rect(160, 20, 120, 40), "Cancel"))
                    {
                        cancelJoining();
                    }

                    GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Joining...", centerTextStyle);
                }
                break;
                case ClientState.ClientError:
                {
                    if (GUI.Button(new Rect(160, 20, 120, 40), "Cancel"))
                    {
                        cancelJoining();
                    }

                    GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "Failed to join", centerTextStyle);
                }
                break;
                default:
                    Debug.LogError("Unknown state");
                break;
                #endregion
            }
        }
    }

    private void goBack()
    {
        Debug.Log("Go to game menu");

        SceneManager.LoadScene("GameMenu");

        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    private void cancelJoining()
    {
        Debug.Log("Cancel joining");

        clientState=ClientState.ClientSearch;
        Network.Disconnect();
    }

    private void startGame()
    {
        Hashtable arguments=new Hashtable();

        arguments.Add("difficulty", -1);

        SceneManager.LoadScene("MainScene", arguments);
    }
}
