using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour 
{
	public float acceleration = 0.1f;
	public float maxSpeed     = 50f;
	public float gravity      = 20f;
	public float fallLimit    = 30f;
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
		GUI.Label(new Rect(20,              20, 10, 20), playerScore.ToString());
		GUI.Label(new Rect(Screen.width-30, 20, 10, 20), enemyScore.ToString());

		if (playerScore>=maxScore || enemyScore>=maxScore)
		{
			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-100, 200, 60), "New game"))
			{
				Start();
			}
			
			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2+40, 200, 60), "Quit"))
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
