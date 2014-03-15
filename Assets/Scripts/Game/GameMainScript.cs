using UnityEngine;
using System.Collections;

public class GameMainScript : MonoBehaviour 
{
	public Transform player;
	public Transform enemy;

	public float acceleration = 0.1f;
	public float maxSpeed     = 50f;
    public float boardLimit   = 25f;
	public float gravity      = 20f;
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
	private bool playerMode;
	private bool player2Mode;
	private bool enemyAIMode;

	// Use this for initialization
	void Start()
	{
        #region Create text styles
		topLeftTextStyle=new GUIStyle();
		topRightTextStyle=new GUIStyle();
		centerTextStyle=new GUIStyle();
		
		topLeftTextStyle.alignment=TextAnchor.UpperLeft;
		topLeftTextStyle.clipping=TextClipping.Overflow;
		topLeftTextStyle.fontSize=24;
		topLeftTextStyle.normal.textColor=Color.white;
		
		topRightTextStyle.alignment=TextAnchor.UpperRight;
		topRightTextStyle.clipping=TextClipping.Overflow;
		topRightTextStyle.fontSize=24;
		topRightTextStyle.normal.textColor=Color.white;
		
		centerTextStyle.alignment=TextAnchor.MiddleCenter;
		centerTextStyle.clipping=TextClipping.Overflow;
		centerTextStyle.fontSize=24;
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
			
			playerMode  = true;
			player2Mode = false;
			enemyAIMode = true;
		}
		else
		if (difficulty==-1) // Multiplayer
		{
			if (Network.isServer) // Server side
			{
				playerMode  = true;
                player2Mode = true;
				enemyAIMode = false;
			}
			else
			if (Network.isClient) // Client side
			{
				player2Logic.playerMode=PlayerLogic.Mode.BothPlayers;
				
				playerMode  = false;
				player2Mode = true;
				enemyAIMode = false;
			}
			else // 2 players
			{
				playerLogic.playerMode=PlayerLogic.Mode.LeftPlayer;
				
				playerMode  = true;
				player2Mode = true;
				enemyAIMode = false;
			}
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
		
		playerLogic.enabled  = playerMode;
		player2Logic.enabled = player2Mode;
		enemyAI.enabled      = enemyAIMode;
	}
	
	// Update is called once per frame
	void Update()
	{
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

        #region Draw score
		GUI.Label(new Rect(20,              20, 1, 1), playerScore.ToString(), topLeftTextStyle);
		GUI.Label(new Rect(Screen.width-20, 20, 1, 1), enemyScore.ToString(),  topRightTextStyle);
        #endregion

        #region Draw buttons
		if (difficulty>=0 || (gameIsOver && !Network.isClient))
		{
			if (GUI.Button(new Rect(Screen.width/2-100, 20, 200, 30), "Restart"))
			{
				init();

                if (Network.isServer)
                {
                    networkView.RPC("init", RPCMode.Others);
                }
			}
			
			if (GUI.Button(new Rect(Screen.width/2-100, 60, 200, 30), "Game menu"))
			{
				goToGameMenu();
			}
		}
		else
		{			
			if (GUI.Button(new Rect(Screen.width/2-100, 20, 200, 30), "Game menu"))
			{
				goToGameMenu();
			}
		}
        #endregion

        #region Draw GAME OVER
		if (gameIsOver)
		{
			GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "GAME OVER", centerTextStyle);
		}
        #endregion
	}

	void OnPlayerDisconnected()
	{
		if (!Application.isLoadingLevel)
		{
			goToGameMenu();
		}
	}

	void OnDisconnectedFromServer()
	{
		if (!Application.isLoadingLevel)
		{
			goToGameMenu();
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

	private void goToGameMenu()
	{
		Debug.Log("Go to game menu");

		SceneManager.LoadScene("GameMenu");

		Network.Disconnect();
		MasterServer.UnregisterHost();
	}
}
