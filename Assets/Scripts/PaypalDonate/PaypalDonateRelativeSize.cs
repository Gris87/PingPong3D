using UnityEngine;
using System.Collections;

public class PaypalDonateRelativeSize : MonoBehaviour
{
    public float width  = 0.1f;
    public float height = 0.1f;

    private PaypalDonate script;

    // Use this for initialization
    void Start()
    {
        script=GetComponent<PaypalDonate>();
    }

    // Update is called once per frame
    void Update()
    {
        script.width  = (int)(Screen.width  * width);
        script.height = (int)(Screen.height * height);
    }
}
