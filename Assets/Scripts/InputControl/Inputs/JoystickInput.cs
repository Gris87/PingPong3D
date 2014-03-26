using UnityEngine;

public class JoystickInput : CustomInput
{
    private JoystickAxis   mAxis;
    private JoystickButton mButton;
    private Joystick       mTarget;

    #region Properties
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

    public Joystick target
    {
        get
        {
            return mTarget;
        }
    }
    #endregion

    public JoystickInput(JoystickAxis aAxis, Joystick aTarget=Joystick.AllJoysticks)
    {
        if (aAxis==JoystickAxis.None)
        {
            Debug.LogError("aAxis can't be JoystickAxis.None");
        }

        mAxis   = aAxis;
        mButton = JoystickButton.None;
        mTarget = aTarget;
    }

    public JoystickInput(JoystickButton aButton, Joystick aTarget=Joystick.AllJoysticks)
    {
        if (aButton==JoystickButton.None)
        {
            Debug.LogError("aButton can't be JoystickButton.None");
        }

        mAxis   = JoystickAxis.None;
        mButton = aButton;
        mTarget = aTarget;
    }

    // TODO: Create from string

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