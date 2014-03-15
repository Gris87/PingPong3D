using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{	
	public enum Mode
	{
		LeftPlayer,
		RightPlayer,
		BothPlayers
	}

	private CharacterController controller;
	public  int                 speed      = 30;
	public  Mode                playerMode = Mode.BothPlayers;

	// Use this for initialization
	void Start()
	{
		controller=gameObject.GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		float verticalMovement=0;

        #region Get vertical movement
		if (playerMode==Mode.BothPlayers)
		{
			verticalMovement=Input.GetAxis("Vertical");
			
			if (verticalMovement==0)
			{
				verticalMovement=Input.GetAxis("Vertical 2");
			}
		}
		else
		{
            if (!Network.isServer)
            {
                string axis;
                
                if (playerMode==Mode.LeftPlayer)
                {
                    axis="Vertical";
                }
                else
                    if (playerMode==Mode.RightPlayer)
                {
                    axis="Vertical 2";
                }
                else
                {
                    Debug.LogError("Unknown axis");
                    axis="Vertical";
                }
                
                verticalMovement=Input.GetAxis(axis);
            }
		}
        #endregion       

		if (verticalMovement!=0)
		{
			controller.Move(new Vector3(0, verticalMovement*speed*Time.deltaTime, 0));

            if (Network.isClient)
            {
                networkView.RPC("setPosition", RPCMode.Server, transform.position);
            }
		}
	}

    [RPC]
    private void setPosition(Vector3 position)
    {
        if (playerMode==Mode.RightPlayer)
        {
            transform.position=position;
        }
    }
}
