using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public  Transform           ball;
	private CharacterController controller;
	public  int                 maxSpeed = 10;

	// Use this for initialization
	void Start()
	{
		controller=gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float offsetY   = ball.transform.position.y-transform.position.y;
		float maxOffset = maxSpeed*Time.deltaTime;

		if (offsetY>maxOffset)
		{
			offsetY=maxOffset;
		}
		else
		if (offsetY<-maxOffset)
		{
			offsetY=-maxOffset;
		}

		controller.Move(new Vector3(0, offsetY, 0));
	}
}
