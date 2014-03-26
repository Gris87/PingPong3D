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
        if (mButton!=MouseButton.None)
        {
            KeyCode mouseButton=(KeyCode) ((int)KeyCode.Mouse0 + (int)mButton);

            return Input.GetKey(mouseButton) ? 1 : 0;
        }

        return getInputByAxis();
    }

    public override float getInputDown()
    {
        if (mButton!=MouseButton.None)
        {
            KeyCode mouseButton=(KeyCode) ((int)KeyCode.Mouse0 + (int)mButton);
            
            return Input.GetKeyDown(mouseButton) ? 1 : 0;
        }
        
        return getInputByAxis();
    }
    
    public override float getInputUp()
    {
        if (mButton!=MouseButton.None)
        {
            KeyCode mouseButton=(KeyCode) ((int)KeyCode.Mouse0 + (int)mButton);
            
            return Input.GetKeyUp(mouseButton) ? 1 : 0;
        }
        
        return getInputByAxis();
    }

    private float getInputByAxis()
    {
        switch (mAxis)
        {
            case MouseAxis.MouseLeft:  return InputGetAxis("Mouse X",     false);
            case MouseAxis.MouseRight: return InputGetAxis("Mouse X",     true);
            case MouseAxis.MouseUp:    return InputGetAxis("Mouse Y",     true);
            case MouseAxis.MouseDown:  return InputGetAxis("Mouse Y",     false);
            case MouseAxis.WheelUp:    return InputGetAxis("Mouse Wheel", true);
            case MouseAxis.WheelDown:  return InputGetAxis("Mouse Wheel", false);
            default:
                Debug.LogError("Unknown axis");
            break;
        }

        return 0;
    }

    private float InputGetAxis(string axisName, bool positive)
    {
        float value=Input.GetAxis(axisName);

        if (positive)
        {
            return value>0 ? value : 0;
        }
        else
        {
            return value<0 ? -value : 0;
        }
    }
}