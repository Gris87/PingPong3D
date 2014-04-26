using UnityEngine;
using System.Collections;

public class PaypalDonate : MonoBehaviour
{
    public Texture2D buttonImage;
    public float     screenX = 0.5f;
    public float     screenY = 0.5f;
    public int       width   = 125;
    public int       height  = 71;
    public string    buttonID;

    private GUIStyle buttonStyle;

    // Use this for initialization
    void Start ()
    {
        if (buttonID.Equals(""))
        {
            Debug.LogError("It is required to set donate button ID to use Paypal button");
            enabled=false;

            return;
        }

        buttonStyle=new GUIStyle();

        buttonStyle.normal.background=buttonImage;
    }

    void OnGUI()
    {
        // It will open link https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=buttonID
        if (GUI.Button(new Rect(Screen.width*screenX - width*0.5f, Screen.height*screenY - height*0.5f, width, height), "", buttonStyle))
        {
            Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id="+buttonID);
        }
    }
}
