/* InputControl package required. Import from Assets Store: TODO */
#define PACKAGE_INPUT_CONTROL

#if PACKAGE_INPUT_CONTROL
using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="ControlSetter"/> is a UI control that display <see cref="KeyMapping"/> and allow to change controls.
/// </summary>
public class ControlSetter : ModifiableObject
{
    private KeyMapping mKeyMapping;
    private int        mKeyPressed;

    #region Properties
    /// <summary>
    /// Gets or sets handled <see cref="KeyMapping"/>.
    /// </summary>
    /// <value>Handled KeyMapping.</value>
    public KeyMapping keyMapping
    {
        get
        {
            return mKeyMapping;
        }

        set
        {
            mKeyMapping.set(value);
        }
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlSetter"/> class.
    /// </summary>
    public ControlSetter() : base()
    {
        mKeyMapping = new KeyMapping();
        mKeyPressed = -1;
    }

    /// <summary>
    /// Draw control in specified rectangle and handles user interactions.
    /// </summary>
    /// <param name="rect">Control location.</param>
    public bool draw(Rect rect)
    {
        GUIStyle buttonStyle=new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize=(int)(Screen.height*0.02);

        if (InputControl.GetJoystickCount()>0)
        {
            if (GUI.Button(new Rect(rect.x,                    rect.y, rect.width*0.325f, rect.height), mKeyPressed==0 ? "..." : mKeyMapping.primaryInput.ToString(),   buttonStyle))
            {
                if (
                    !(mKeyMapping.primaryInput is MouseInput)
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.None
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.WheelDown
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.WheelUp
                   )
                {
                    mKeyPressed=0;
                }
            }

            if (GUI.Button(new Rect(rect.x+rect.width*0.3375f, rect.y, rect.width*0.325f, rect.height), mKeyPressed==1 ? "..." : mKeyMapping.secondaryInput.ToString(), buttonStyle))
            {
                mKeyPressed=1;
            }

            if (GUI.Button(new Rect(rect.x+rect.width*0.675f,  rect.y, rect.width*0.325f, rect.height), mKeyPressed==2 ? "..." : mKeyMapping.thirdInput.ToString(),     buttonStyle))
            {
                mKeyPressed=2;
            }
        }
        else
        {
            if (mKeyPressed>1)
            {
                mKeyPressed=-1;
            }

            if (GUI.Button(new Rect(rect.x,                   rect.y, rect.width*0.49f, rect.height), mKeyPressed==0 ? "..." : mKeyMapping.primaryInput.ToString(),   buttonStyle))
            {
                if (
                    !(mKeyMapping.primaryInput is MouseInput)
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.None
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.WheelDown
                    ||
                    ((MouseInput)mKeyMapping.primaryInput).axis==MouseAxis.WheelUp
                   )
                {
                    mKeyPressed=0;
                }
            }

            if (GUI.Button(new Rect(rect.x+rect.width*0.495f, rect.y, rect.width*0.49f, rect.height), mKeyPressed==1 ? "..." : mKeyMapping.secondaryInput.ToString(), buttonStyle))
            {
                mKeyPressed=1;
            }
        }

        return mKeyPressed>=0;
    }

    /// <summary>
    /// Sets <see cref="CustomInput"/> for selected key.
    /// </summary>
    /// <param name="input">Some kind of input.</param>
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
}
#endif