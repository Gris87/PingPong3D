using UnityEngine;

public class JoystickInput : CustomInput
{
    private Joystick       mTarget;
    private JoystickAxis   mAxis;
    private JoystickButton mButton;

    #region Properties
    public Joystick target
    {
        get
        {
            return mTarget;
        }
    }

    public JoystickAxis axis
    {
        get
        {
            return mAxis;
        }
    }

    public JoystickButton button
    {
        get
        {
            return mButton;
        }
    }
    #endregion

    public JoystickInput(Joystick aTarget, JoystickAxis aAxis)
    {
        if (aAxis==JoystickAxis.None)
        {
            Debug.LogError("aAxis can't be JoystickAxis.None");
        }

        mTarget = aTarget;
        mAxis   = aAxis;
        mButton = JoystickButton.None;
    }

    public JoystickInput(Joystick aTarget, JoystickButton aButton)
    {
        if (aButton==JoystickButton.None)
        {
            Debug.LogError("aButton can't be JoystickButton.None");
        }
        
        mTarget = aTarget;
        mAxis   = JoystickAxis.None;
        mButton = aButton;
    }

    public override string ToString()
    {
        string res;

        if (mTarget==Joystick.AllJoysticks)
        {
            res="Joystick ";
        }
        else
        {
            res="Joystick "+((int)mTarget).ToString()+" ";
        }

        if (mAxis!=JoystickAxis.None)
        {
            int axisId=(int)mAxis;
            bool positive;

            if (axisId % 2 == 0)
            {
                axisId=(axisId/2) + 1;
                positive=true;
            }
            else
            {
                axisId=((axisId-1)/2) + 1;
                positive=false;
            }

            res=res+"Axis "+axisId.ToString()+" "+(positive ? "(+)" : "(-)");
        }

        if (mButton!=JoystickButton.None)
        {
            res=res+"Button "+((int)mButton+1).ToString();
        }

        return res;
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