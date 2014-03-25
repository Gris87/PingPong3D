using UnityEngine;
using System.Collections;

public class ControlSetter : ModifiableObject
{
    private static string           localizationPressAnyKey;

    private InputControl.KeyMapping mKeys;
    private int                     mKeyPressed;

    public ControlSetter() : base()
    {
        mKeys       = new InputControl.KeyMapping("", KeyCode.None, KeyCode.None);
        mKeyPressed = -1;
    }

    public bool draw(Rect rect)
    {
        if (GUI.Button(new Rect(rect.x,                   rect.y, rect.width*0.495f, rect.height), mKeyPressed==0 ? localizationPressAnyKey : mKeys.primaryCode.ToString()))
        {
            setKeyPressed(0);
        }

        if (GUI.Button(new Rect(rect.x+rect.width*0.505f, rect.y, rect.width*0.495f, rect.height), mKeyPressed==1 ? localizationPressAnyKey : mKeys.secondaryCode.ToString()))
        {
            setKeyPressed(1);
        }

        return mKeyPressed>=0;
    }

    public void setKeyPressed(int value)
    {
        mKeyPressed             = value;
        localizationPressAnyKey = LanguageManager.Instance.GetTextValue("OptionsScene.PressAnyKey");
    }

    public void setSelectedKey(KeyCode code)
    {
        if (mKeyPressed==0)
        {
            mKeys.set(code, mKeys.secondaryCode);
        }
        else
        if (mKeyPressed==1)
        {
            mKeys.set(mKeys.primaryCode, code);
        }
        else
        {
            Debug.LogError("Incorrect value of mKeyPressed");
        }

        mKeyPressed=-1;
    }

    public InputControl.KeyMapping getKeys()
    {
        return mKeys;
    }

    public void setKeys(InputControl.KeyMapping value)
    {
        mKeys.set(value);
    }
}
