using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour 
{
	public float acceleration = 0.1f;
	public float maxSpeed     = 50f;
	public float boardLimit   = 26f;
	public int   maxScore     = 3;

	private int playerScore;
	private int enemyScore;

	// Use this for initialization
	void Start()
	{
		resetPositionAndSpeed(true);

		playerScore = 0;
		enemyScore  = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		rigidbody.velocity=rigidbody.velocity*(1+acceleration*Time.deltaTime);

		if (rigidbody.velocity.magnitude>maxSpeed)
		{
			rigidbody.velocity=rigidbody.velocity.normalized*maxSpeed;
		}

		if (transform.position.x>boardLimit)
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
		if (transform.position.x<-boardLimit)
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

	void OnGUI()
	{
		if (playerScore>=maxScore || enemyScore>=maxScore)
		{
			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-100, 200, 60), "New game"))
			{
				Start();
			}
			
			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2+100, 200, 60), "Quit"))
			{
				Application.Quit();
			}
		}
	}

	private void stop()
	{
		transform.position=new Vector3(0, 0, 0);
		rigidbody.velocity=new Vector3(0, 0, 0);
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
}
