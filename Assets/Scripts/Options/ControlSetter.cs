using UnityEngine;
using System.Collections;

public class ControlSetter : ModifiableObject
{
    InputControl.KeyMapping mKeys;

    public ControlSetter() : base()
    {
    }

    public void draw(Rect rect)
    {

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
        mKeys=value;
    }
}
