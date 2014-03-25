using UnityEngine;

public class KeyboardInput : CustomInput
{
    private KeyCode mKey;
    public KeyCode key
    {
        get
        {
            return mKey;
        }
    }

    public KeyboardInput(KeyCode aKey=KeyCode.None)
    {
        mKey=aKey;
    }

    public override float getInput()
    {
        return 0;
    }

    public override float getInputDown()
    {
        return 0;
    }

    public override float getInputUp()
    {
        return 0;
    }
}