using UnityEngine;
using System.Collections;

public class GameMainScript : MonoBehaviour 
{
	public Transform player;
	public Transform enemy;

	public float acceleration = 0.1f;
	public float maxSpeed     = 50f;
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

		// ---------------------------------------------------------------

		playerLogic  = player.GetComponent<PlayerLogic>();
		player2Logic = enemy.GetComponent<PlayerLogic>();
		enemyAI      = enemy.GetComponent<EnemyAI>();

		Hashtable arguments=SceneManager.GetSceneArguments();

		if (arguments!=null && arguments.ContainsKey("difficulty"))
		{
			difficulty=(int)arguments["difficulty"];
		}
		else
		{
			difficulty=0;
		}

		if (difficulty>=0)
		{
			enemyAI.maxSpeed=10+difficulty*10;
			
			playerMode  = true;
			player2Mode = false;
			enemyAIMode = true;
		}
		else
		if (difficulty==-1)
		{
			if (Network.isServer)
			{
				playerMode  = true;
				player2Mode = false;
				enemyAIMode = false;
			}
			else
			if (Network.isClient)
			{
				player2Logic.playerMode=PlayerLogic.Mode.BothPlayers;
				
				playerMode  = false;
				player2Mode = true;
				enemyAIMode = false;
			}
			else
			{
				playerLogic.playerMode=PlayerLogic.Mode.LeftPlayer;
				
				playerMode  = true;
				player2Mode = true;
				enemyAIMode = false;
			}
		}
		
		Init();
	}
	
	void Init()
	{
		Debug.Log("Game started (difficulty="+difficulty.ToString()+")");

		resetPositionAndSpeed(true);
		
		playerScore = 0;
		enemyScore  = 0;
		
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
			
			rigidbody.velocity=new Vector3(rigidbody.velocity.x*factor,
			                               rigidbody.velocity.y*factor,
			                               (rigidbody.velocity.z+gravity*Time.deltaTime)*factor);
			
			if (rigidbody.velocity.magnitude>maxSpeed)
			{
				rigidbody.velocity=rigidbody.velocity.normalized*maxSpeed;
			}
			
			
			if (transform.position.z>fallLimit)
			{
				if (transform.position.x>0)
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
				else
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
			}
		}
	}

	void OnGUI()
	{
		bool gameIsOver = (playerScore>=maxScore || enemyScore>=maxScore);

		GUI.Label(new Rect(20,              20, 1, 1), playerScore.ToString(), topLeftTextStyle);
		GUI.Label(new Rect(Screen.width-20, 20, 1, 1), enemyScore.ToString(),  topRightTextStyle);

		if (difficulty>=0 || (gameIsOver && !Network.isClient))
		{
			if (GUI.Button(new Rect(Screen.width/2-100, 20, 200, 30), "Restart"))
			{
				Init();
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

		if (gameIsOver)
		{
			GUI.Label(new Rect(Screen.width/2, Screen.height/2, 1, 1), "GAME OVER", centerTextStyle);
		}
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
