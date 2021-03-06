using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="FPSCounter"/> calculates frames/second and display it on the screen.
/// </summary>
public class FPSCounter : MonoBehaviour
{
    /// <summary>
    /// Global enabled flag.
    /// </summary>
    public static bool isOn = false;

    private static GUIStyle textStyle = null;

    private static float nextSecond = 0;
    private static int   fpsCurrent = 0;
    private static int   fpsTotal   = 0;

    // Use this for initialization
    void Start()
    {
        if (textStyle==null)
        {
            textStyle=new GUIStyle();

            textStyle.alignment=TextAnchor.LowerLeft;
            textStyle.clipping=TextClipping.Overflow;
            textStyle.normal.textColor=Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            ++fpsCurrent;

            float curTime=Time.realtimeSinceStartup;

            if (curTime>=nextSecond)
            {
                nextSecond = curTime+1;
                fpsTotal   = fpsCurrent;
                fpsCurrent = 0;
            }
        }
    }

    void OnGUI()
    {
        if (isOn)
        {
            textStyle.fontSize=(int)(Screen.height*0.025f);

            GUI.Label(new Rect(Screen.width*0.01f, Screen.height*0.99f, 1, 1), fpsTotal.ToString()+" FPS", textStyle);
        }
    }
}
