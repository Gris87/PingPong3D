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

    private Vector3             syncVelocity = Vector3.zero;
    private float               syncTime     = 0f;
    private float               syncDelay    = 0f;
    private float               lastSyncTime = 0f;

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
			if (Input.GetMouseButton(0))
			{
                Vector3 mousePos = getMousePos();
                verticalMovement = mousePos.y-transform.position.y;
                float maxOffset  = speed*Time.deltaTime;
                
                if (verticalMovement>maxOffset)
                {
                    verticalMovement=maxOffset;
                }
                else
                    if (verticalMovement<-maxOffset)
                {
                    verticalMovement=-maxOffset;
                }
                
                verticalMovement=verticalMovement/speed/Time.deltaTime;
			}
			else
			{
				verticalMovement=Input.GetAxis("Vertical");
				
				if (verticalMovement==0)
				{
					verticalMovement=Input.GetAxis("Vertical 2");
				}
			}
		}
		else
		{
            if (Network.isServer || Network.isClient)
            {
                if (syncTime<syncDelay)
                {
                    float deltaTime=Time.deltaTime;

                    if (syncTime+deltaTime>syncDelay)
                    {
                        deltaTime=syncDelay-syncTime;
                    }

                    syncTime+=Time.deltaTime;
                    
                    verticalMovement=(((syncVelocity.y*deltaTime)/syncDelay)/speed)/Time.deltaTime;
                }
            }
            else
            {
                if (
                    Application.platform==RuntimePlatform.Android
                    ||
                    Application.platform==RuntimePlatform.BB10Player
                    ||
                    Application.platform==RuntimePlatform.IPhonePlayer
                   )
                {
                    if (Input.GetMouseButton(0))
                    {
                        Vector3 mousePos = getMousePos();
                        
                        if (
                            (mousePos.x<0 && playerMode==Mode.LeftPlayer)
                            ||
                            (mousePos.x>=0 && playerMode==Mode.RightPlayer)
                            )
                        {
                            verticalMovement = mousePos.y-transform.position.y;
                            float maxOffset  = speed*Time.deltaTime;
                            
                            if (verticalMovement>maxOffset)
                            {
                                verticalMovement=maxOffset;
                            }
                            else
                                if (verticalMovement<-maxOffset)
                            {
                                verticalMovement=-maxOffset;
                            }
                            
                            verticalMovement=verticalMovement/speed/Time.deltaTime;
                        }
                    }
                }
                else
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

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            
            setPosition(syncPosition);
        }
    }

    [RPC]
    private void setPosition(Vector3 position)
    {
        if (playerMode!=Mode.BothPlayers)
        {
            float curTime = Time.time;
            float delay   = curTime-lastSyncTime;
            lastSyncTime  = curTime;

            if (delay!=0)
            {
                syncVelocity = position-transform.position;
                syncTime     = 0;
                syncDelay    = delay;
            }

            transform.position=position;
        }
    }

    private Vector3 getMousePos()
    {
        Plane plane=new Plane(Vector3.forward, 0);                
        float distance;
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
