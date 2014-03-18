using UnityEngine;
using System.Collections;

public class GameMainScript : MonoBehaviour
{
    public Transform player;
    public Transform enemy;

    public float acceleration = 0.1f;
    public float maxSpeed     = 50f;
    public float boardLimit   = 25f;
    public float gravity      = 100f;
    public float fallLimit    = 30f;
    public int   maxScore     = 3;

    private PlayerLogic playerLogic;
    private PlayerLogic player2Logic;
    private EnemyAI     enemyAI;
    private GUIStyle    topLeftTextStyle;
    private GUIStyle    topRightTextStyle;
    private GUIStyle    centerTextStyle;

    private int  difficulty;
    private int  playerScore;
    private int  enemyScore;
    private bool enemyAIMode;

    // Use this for initialization
    void Start()
    {
        #region Create text styles
        topLeftTextStyle  = new GUIStyle();
        topRightTextStyle = new GUIStyle();
        centerTextStyle   = new GUIStyle();

        topLeftTextStyle.alignment=TextAnchor.UpperLeft;
        topLeftTextStyle.clipping=TextClipping.Overflow;
        topLeftTextStyle.fontSize=(int)(Screen.height*0.075);
        topLeftTextStyle.normal.textColor=Color.white;

        topRightTextStyle.alignment=TextAnchor.UpperRight;
        topRightTextStyle.clipping=TextClipping.Overflow;
        topRightTextStyle.fontSize=(int)(Screen.height*0.075);
        topRightTextStyle.normal.textColor=Color.white;

        centerTextStyle.alignment=TextAnchor.MiddleCenter;
        centerTextStyle.clipping=TextClipping.Overflow;
        centerTextStyle.fontSize=(int)(Screen.height*0.075);
        centerTextStyle.normal.textColor=Color.white;
        #endregion

        // ---------------------------------------------------------------

        #region Get controllers
        playerLogic  = player.GetComponent<PlayerLogic>();
        player2Logic = enemy.GetComponent<PlayerLogic>();
        enemyAI      = enemy.GetComponent<EnemyAI>();
        #endregion

        #region Get difficulty from arguments
        Hashtable arguments=SceneManager.GetSceneArguments();

        if (arguments!=null && arguments.ContainsKey("difficulty"))
        {
            difficulty=(int)arguments["difficulty"];
        }
        else
        {
            difficulty=0;
        }
        #endregion

        #region Setup controllers
        if (difficulty>=0) // Single player
        {
            enemyAI.maxSpeed=10+difficulty*10;

            enemyAIMode = true;
        }
        else
        if (difficulty==-1) // Multiplayer
        {
            if (Network.isServer) // Server side
            {
                playerLogic.playerMode  = PlayerLogic.Mode.BothPlayers;
                player2Logic.playerMode = PlayerLogic.Mode.RightPlayer;
            }
            else
            if (Network.isClient) // Client side
            {
                playerLogic.playerMode  = PlayerLogic.Mode.LeftPlayer;
                player2Logic.playerMode = PlayerLogic.Mode.BothPlayers;
            }
            else // 2 players
            {
                playerLogic.playerMode  = PlayerLogic.Mode.LeftPlayer;
                player2Logic.playerMode = PlayerLogic.Mode.RightPlayer;
            }

            enemyAIMode = false;
        }
        #endregion

        init();
    }

    [RPC]
    private void init()
    {
        Debug.Log("Game started (difficulty="+difficulty.ToString()+")");

        playerScore = 0;
        enemyScore  = 0;

        player.position = new Vector3(player.position.x, 0, 0);
        enemy.position  = new Vector3(enemy.position.x,  0, 0);

        resetPositionAndSpeed(true);

        playerLogic.enabled  = true;
        player2Logic.enabled = !enemyAIMode;
        enemyAI.enabled      = enemyAIMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();
        }

        if (!Network.isClient)
        {
            float factor=(1+acceleration*Time.deltaTime);
            float velocityX=rigidbody.velocity.x*factor;
            float velocityY=rigidbody.velocity.y*factor;
            float velocityZ;

            if (velocityX!=0 || velocityY!=0)
            {
                if (Mathf.Abs(transform.position.x)<boardLimit)
                {
                    velocityZ=0;
                }
                else
                {
                    velocityZ=(rigidbody.velocity.z+gravity*Time.deltaTime)*factor;
                }

                if (Mathf.Abs(velocityX)<2f)
                {
                    if (velocityX<0)
                    {
                        velocityX=-2f;
                    }
                    else
                    {
                        velocityX=2f;
                    }
                }

                if (Mathf.Abs(velocityY)<2f)
                {
                    if (velocityY<0)
                    {
                        velocityY=-2f;
                    }
                    else
                    {
                        velocityY=2f;
                    }
                }

                rigidbody.velocity=new Vector3(velocityX, velocityY, velocityZ);

                if (rigidbody.velocity.magnitude>maxSpeed)
                {
                    rigidbody.velocity=rigidbody.velocity.normalized*maxSpeed;
                }


                if (transform.position.z>fallLimit)
                {
                    if (transform.position.x>0)
                    {
                        increasePlayerScore();

                        if (Network.isServer)
                        {
                            networkView.RPC("increasePlayerScore", RPCMode.Others);
                        }
                    }
                    else
                    {
                        increaseEnemyScore();

                        if (Network.isServer)
                        {
                            networkView.RPC("increaseEnemyScore", RPCMode.Others);
                        }
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        bool gameIsOver = (playerScore>=maxScore || enemyScore>=maxScore);

        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize=(int)(Screen.height*0.03);

        #region Draw score
        GUI.Label(new Rect(Screen.width*0.05f, Screen.height*0.05f, 1, 1), playerScore.ToString(), topLeftTextStyle);
        GUI.Label(new Rect(Screen.width*0.95f, Screen.height*0.05f, 1, 1), enemyScore.ToString(),  topRightTextStyle);
        #endregion

        #region Draw buttons
        if (difficulty>=0 || (gameIsOver && !Network.isClient))
        {
            if (GUI.Button(new Rect(Screen.width*0.15f, Screen.height*0.05f, Screen.width*0.3f, Screen.height*0.1f), "Restart", buttonStyle))
            {
                init();

                if (Network.isServer)
                {
                    networkView.RPC("init", RPCMode.Others);
                }
            }

            if (GUI.Button(new Rect(Screen.width*0.55f, Screen.height*0.05f, Screen.width*0.3f, Screen.height*0.1f), "Game menu", buttonStyle))
            {
                goBack();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width*0.35f, Screen.height*0.05f, Screen.width*0.3f, Screen.height*0.1f), "Game menu", buttonStyle))
            {
                goBack();
            }
        }
        #endregion

        #region Draw GAME OVER
        if (gameIsOver)
        {
            GUI.Label(new Rect(Screen.width*0.5f, Screen.height*0.5f, 1, 1), "GAME OVER", centerTextStyle);
        }
        #endregion
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;

        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);

            syncVelocity = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);

            rigidbody.position=syncPosition;
            rigidbody.velocity=syncVelocity;
        }
    }

    void OnPlayerDisconnected()
    {
        if (!Application.isLoadingLevel)
        {
            goBack();
        }
    }

    void OnDisconnectedFromServer()
    {
        if (!Application.isLoadingLevel)
        {
            goBack();
        }
    }

    [RPC]
    private void increasePlayerScore()
    {
        playerScore++;

        if (playerScore>=maxScore)
        {
            stop();
        }
        else
        {
            resetPositionAndSpeed(false);
        }
    }

    [RPC]
    private void increaseEnemyScore()
    {
        enemyScore++;

        if (enemyScore>=maxScore)
        {
            stop();
        }
        else
        {
            resetPositionAndSpeed(true);
        }
    }

    private void stop()
    {
        Debug.Log("Game over");

        player.position = new Vector3(player.position.x, 0, 0);
        enemy.position  = new Vector3(enemy.position.x,  0, 0);

        transform.position = new Vector3(0, 0, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);

        playerLogic.enabled  = false;
        player2Logic.enabled = false;
        enemyAI.enabled      = false;
    }

    private void resetPositionAndSpeed(bool moveToRight)
    {
        transform.position=new Vector3(0, 0, 0);

        if (!Network.isClient)
        {
            if (moveToRight)
            {
                rigidbody.velocity=new Vector3((float)(10+Random.value*10),  (float)((Random.value-0.5)*20), 0);
            }
            else
            {
                rigidbody.velocity=new Vector3((float)(-10-Random.value*10), (float)((Random.value-0.5)*20), 0);
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
}
