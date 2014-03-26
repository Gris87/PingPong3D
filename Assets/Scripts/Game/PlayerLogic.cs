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
            if (InputControl.GetMouseButton(MouseButton.Left))
            {
                Vector3 mousePos = getClickPosition(InputControl.mousePosition);
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
                verticalMovement=InputControl.GetAxis("Vertical");

                if (verticalMovement==0)
                {
                    verticalMovement=InputControl.GetAxis("Vertical 2");
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
                if (InputControl.touchCount>0)
                {
                    foreach (Touch touch in InputControl.touches)
                    {
                        Vector3 mousePos = getClickPosition(touch.position);

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

                            break;
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

                    verticalMovement=InputControl.GetAxis(axis);
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

    private Vector3 getClickPosition(Vector3 screenPosition)
    {
        Plane plane=new Plane(Vector3.forward, 0);
        float distance;
        Ray ray=Camera.main.ScreenPointToRay(screenPosition);

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
