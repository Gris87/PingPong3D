using UnityEngine;

public class MouseInput : CustomInput
{
    private MouseAxis   mAxis;
    private MouseButton mButton;
    
    #region Properties    
    public MouseAxis axis
    {
        get
        {
            return mAxis;
        }
    }
    
    public MouseButton button
    {
        get
        {
            return mButton;
        }
    }
    #endregion

    public MouseInput(MouseAxis aAxis)
    {
        if (aAxis==MouseAxis.None)
        {
            Debug.LogError("aAxis can't be MouseAxis.None");
        }

        mAxis   = aAxis;
        mButton = MouseButton.None;
    }
    
    public MouseInput(MouseButton aButton)
    {
        if (aButton==MouseButton.None)
        {
            Debug.LogError("aButton can't be MouseButton.None");
        }

        mAxis   = MouseAxis.None;
        mButton = aButton;
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