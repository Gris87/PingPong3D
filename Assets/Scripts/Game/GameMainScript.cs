﻿using UnityEngine;
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

	private int  difficulty;
	private int  playerScore;
	private int  enemyScore;
	private bool player2Mode;
	private bool enemyAIMode;

	// Use this for initialization
	void Start()
	{
		playerLogic  = player.GetComponent<PlayerLogic>();
		player2Logic = enemy.GetComponent<PlayerLogic>();
		enemyAI      = enemy.GetComponent<EnemyAI>();

		player2Mode=false;
		enemyAIMode=true;

		Hashtable arguments=SceneManager.GetSceneArguments();

		if (arguments!=null && arguments.ContainsKey("difficulty"))
		{
			difficulty=(int)arguments["difficulty"];
			
			if (difficulty>=0)
			{
				enemyAI.maxSpeed=10+difficulty*10;
			}
			else
			if (difficulty==-1)
			{
				playerLogic.playerMode=PlayerLogic.Mode.LeftPlayer;

				player2Mode=true;
				enemyAIMode=false;
			}
		}
		else
		{
			difficulty=0;
		}

		Init();
	}

	void Init()
	{
		Debug.Log("Game started (difficulty="+difficulty.ToString()+")");

		resetPositionAndSpeed(true);
		
		playerScore = 0;
		enemyScore  = 0;
		
		playerLogic.enabled  = true;
		player2Logic.enabled = player2Mode;
		enemyAI.enabled      = enemyAIMode;
	}
	
	// Update is called once per frame
	void Update()
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

	void OnGUI()
	{
		bool gameIsOver = (playerScore>=maxScore || enemyScore>=maxScore);

		GUI.Label(new Rect(20,              20, 10, 20), playerScore.ToString());
		GUI.Label(new Rect(Screen.width-30, 20, 10, 20), enemyScore.ToString());

		if (difficulty>=0 || gameIsOver)
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
			GUI.Label(new Rect(Screen.width/2-40, Screen.height/2-30, 80, 20), "GAME OVER");
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

		if (moveToRight)
		{
			rigidbody.velocity=new Vector3((float)(10+Random.value*10),  (float)((Random.value-0.5)*20), 0);
		}
		else
		{
			rigidbody.velocity=new Vector3((float)(-10-Random.value*10), (float)((Random.value-0.5)*20), 0);
		}
	}

	private void goToGameMenu()
	{
		Debug.Log("Go to game menu");

		SceneManager.LoadScene("GameMenu");
	}
}
