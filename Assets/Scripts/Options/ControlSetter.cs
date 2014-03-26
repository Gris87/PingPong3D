using UnityEngine;
using System.Collections;

public class ControlSetter : ModifiableObject
{
    private KeyMapping mKeyMapping;
    private int        mKeyPressed;

    public ControlSetter() : base()
    {
        mKeyMapping = new KeyMapping();
        mKeyPressed = -1;
    }

    public bool draw(Rect rect)
    {
        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize=(int)(Screen.height*0.02);

        if (GUI.Button(new Rect(rect.x,                    rect.y, rect.width*0.325f, rect.height), mKeyPressed==0 ? "..." : mKeyMapping.primaryInput.ToString(),   buttonStyle))
        {
            mKeyPressed=0;
        }

        if (GUI.Button(new Rect(rect.x+rect.width*0.3375f, rect.y, rect.width*0.325f, rect.height), mKeyPressed==1 ? "..." : mKeyMapping.secondaryInput.ToString(), buttonStyle))
        {
            mKeyPressed=1;
        }

        if (GUI.Button(new Rect(rect.x+rect.width*0.675f,  rect.y, rect.width*0.325f, rect.height), mKeyPressed==2 ? "..." : mKeyMapping.thirdInput.ToString(),     buttonStyle))
        {
            mKeyPressed=2;
        }

        return mKeyPressed>=0;
    }

    public void setKeyPressed(int value)
    {
        mKeyPressed=value;
    }

    public void setSelectedKey(CustomInput input)
    {
        if (mKeyPressed==0)
        {
            mKeyMapping.primaryInput=input;
        }
        else
        if (mKeyPressed==1)
        {
            mKeyMapping.secondaryInput=input;
        }
        else
        if (mKeyPressed==2)
        {
            mKeyMapping.thirdInput=input;
        }
        else
        {
            Debug.LogError("Incorrect value of mKeyPressed");
        }

        modificationMade();

        mKeyPressed=-1;
    }

    public KeyMapping getKeyMapping()
    {
        return mKeyMapping;
    }

    public void setKeyMapping(KeyMapping value)
    {
        mKeyMapping.set(value);
    }
}
