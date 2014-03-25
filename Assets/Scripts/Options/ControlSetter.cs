using UnityEngine;
using System.Collections;

public class ControlSetter : ModifiableObject
{
    InputControl.KeyMapping mKeys;

    public ControlSetter() : base()
    {
        mKeys=new InputControl.KeyMapping("", KeyCode.None, KeyCode.None);
    }

    public void draw(Rect rect)
    {
        if (GUI.Button(new Rect(rect.x,                   rect.y, rect.width*0.495f, rect.height), mKeys.primaryCode.ToString()))
        {
        }

        if (GUI.Button(new Rect(rect.x+rect.width*0.505f, rect.y, rect.width*0.495f, rect.height), mKeys.secondaryCode.ToString()))
        {
        }
    }

    public void control()
    {

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
