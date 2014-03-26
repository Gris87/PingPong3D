using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    public static bool isOn = false;

    private GUIStyle textStyle = null;

    private static float nextSecond = 0;
    private static int fpsCurrent   = 0;
    private static int fpsTotal     = 0;

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

	void OnGUI()
    {
        if (isOn)
        {
            ++fpsCurrent;

            float curTime=Time.time;

            if (curTime>nextSecond)
            {
                nextSecond = curTime+1;
                fpsTotal   = fpsCurrent;
                fpsCurrent = 0;
            }

            textStyle.fontSize=(int)(Screen.height*0.025);

            GUI.Label(new Rect(Screen.width*0.01f, Screen.height*0.99f, 1, 1), fpsTotal.ToString()+" FPS", textStyle);
        }
	}
}
