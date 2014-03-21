using UnityEngine;
using System.Collections;

public class BigHorizontalSlider : ModifiableObject
{
    private float mValue;
    private float mMinimum;
    private float mMaximum;

    public BigHorizontalSlider(float value, float minimum, float maximum) : base()
    {
        if (minimum<maximum)
        {
            mMinimum = minimum;
            mMaximum = maximum;
        }
        else
        {
            mMinimum = maximum;
            mMaximum = minimum;
        }

        setValue(value);
    }

    public void draw(Rect rect)
    {
    }

    public void control()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (mValue>mMinimum)
            {
                mValue-=(mMaximum-mMinimum)*0.1f;

                if (mValue<mMinimum)
                {
                    mValue=mMinimum;
                }

                modificationMade();
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mValue<mMaximum)
            {
                mValue+=(mMaximum-mMinimum)*0.1f;

                if (mValue>mMaximum)
                {
                    mValue=mMaximum;
                }
                
                modificationMade();
            }
        }
    }

    public float getValue()
    {
        return mValue;
    }

    public void setValue(float value)
    {
        if (value<mMinimum)
        {
            value=mMinimum;
        }
        else
        if (value>mMaximum)
        {
            value=mMaximum;
        }

        mValue=value;
    }
}
