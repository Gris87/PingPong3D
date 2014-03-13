using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{	
	private CharacterController controller;
	public  int                 speed = 30;

	// Use this for initialization
	void Start()
	{
		controller=gameObject.GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		float verticalMovement=Input.GetAxis("Vertical");

		if (verticalMovement>0)
		{
			controller.Move(new Vector3(0, speed*Time.deltaTime, 0));
		}
		else
		if (verticalMovement<0)
		{
			controller.Move(new Vector3(0, -speed*Time.deltaTime, 0));
		}
	}
}
