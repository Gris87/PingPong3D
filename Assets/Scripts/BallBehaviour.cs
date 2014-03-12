using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour 
{
	public float acceleration = 0.1f;
	public float maxSpeed     = 50f;

	// Use this for initialization
	void Start()
	{
		resetPositionAndSpeed(true);
	}
	
	// Update is called once per frame
	void Update()
	{
		rigidbody.velocity=rigidbody.velocity*(1+acceleration*Time.deltaTime);

		if (rigidbody.velocity.magnitude>maxSpeed)
		{
			rigidbody.velocity=rigidbody.velocity.normalized*maxSpeed;
		}
	}

	private void resetPositionAndSpeed(bool moveToRight)
	{
		transform.position.Set(0, 0, 0);

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
