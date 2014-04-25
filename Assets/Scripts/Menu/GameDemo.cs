using UnityEngine;
using System.Collections;

public class GameDemo : MonoBehaviour
{
    public AudioClip leftBoardHitClip;
    public AudioClip rightBoardHitClip;
    public AudioClip wallBoardHitClip;

    public float acceleration = 0.1f;
    public float maxSpeed     = 20f;

    // Use this for initialization
    void Start()
    {
        rigidbody.velocity=new Vector3((float)(10+Random.value*10),  (float)((Random.value-0.5)*20), 0);
    }

    // Update is called once per frame
    void Update()
    {
        float factor=(1+acceleration*Time.deltaTime);
        float velocityX=rigidbody.velocity.x*factor;
        float velocityY=rigidbody.velocity.y*factor;

        if (velocityX!=0 || velocityY!=0)
        {
            if (Mathf.Abs(velocityX)<2f)
            {
                if (velocityX<0)
                {
                    velocityX=-2f;
                }
                else
                {
                    velocityX=2f;
                }
            }

            if (Mathf.Abs(velocityY)<2f)
            {
                if (velocityY<0)
                {
                    velocityY=-2f;
                }
                else
                {
                    velocityY=2f;
                }
            }

            rigidbody.velocity=new Vector3(velocityX, velocityY, 0);

            if (rigidbody.velocity.magnitude>maxSpeed)
            {
                rigidbody.velocity=rigidbody.velocity.normalized*maxSpeed;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        string otherTag=other.gameObject.tag;
        
        if (otherTag.Equals("LeftBoard"))
        {
            audio.PlayOneShot(leftBoardHitClip,  Options.effectsVolume);
        }
        else
        if (otherTag.Equals("RightBoard"))
        {
            audio.PlayOneShot(rightBoardHitClip, Options.effectsVolume);
        }
        else
        if (otherTag.Equals("Wall"))
        {
            audio.PlayOneShot(wallBoardHitClip,  Options.effectsVolume);
        }
    }
}
