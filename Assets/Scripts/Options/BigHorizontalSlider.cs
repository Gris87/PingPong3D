#region Defines

#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
#define TOUCH_DEVICE
#endif

#endregion

using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="BigHorizontalSlider"/> is a UI control that allow to select value between minimum and maximum.
/// </summary>
public class BigHorizontalSlider : ModifiableObject
{
    private static Texture2D mValueTexture=new Texture2D(1, 1);

    private float mValue;
    private float mMinimum;
    private float mMaximum;

    #region Properties
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>Current value.</value>
    public float value
    {
        get
        {
            return mValue;
        }

        set
        {
            if (value<mMinimum)
            {
                mValue=mMinimum;
            }
            else
            if (value>mMaximum)
            {
                mValue=mMaximum;
            }
            else
            {
                mValue=value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the minimum.
    /// </summary>
    /// <value>Minimum value.</value>
    public float minimum
    {
        get
        {
            return mMinimum;
        }

        set
        {
            if (mMaximum<value)
            {
                mMaximum=value;
            }

            if (mValue<value)
            {
                mValue=value;
            }

            mMinimum=value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    /// <value>Maximum value.</value>
    public float maximum
    {
        get
        {
            return mMaximum;
        }

        set
        {
            if (mMinimum>value)
            {
                mMinimum=value;
            }

            if (mValue>value)
            {
                mValue=value;
            }

            mMaximum=value;
        }
    }
    #endregion

    /// <summary>
    /// Create a new instance of <see cref="BigHorizontalSlider"/>.
    /// </summary>
    /// <param name="value">Current value.</param>
    /// <param name="minimum">Minimum value.</param>
    /// <param name="maximum">Maximum value.</param>
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

        this.value=value;
    }

    /// <summary>
    /// Draw control in specified rectangle and handles user interactions.
    /// </summary>
    /// <param name="rect">Control location.</param>
    /// <param name="catchMouseEvents">Catch mouse events or not.</param>
    public void draw(Rect rect, bool catchMouseEvents)
    {
        GUI.Box(rect, "");

        Rect rangeRect=new Rect(rect.x+rect.width*0.02f, rect.y+rect.height*0.1f, rect.width*0.96f, rect.height*0.8f);

        Color tempColor=GUI.color;
        GUI.color=Color.green;
        GUI.DrawTexture(new Rect(rangeRect.x, rangeRect.y, rangeRect.width*(mValue-mMinimum)/(mMaximum-mMinimum), rangeRect.height), mValueTexture);
        GUI.color=tempColor;

        if (catchMouseEvents)
        {
            ArrayList clickPositions=new ArrayList();

#if TOUCH_DEVICE
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPos=touch.position;

                clickPositions.Add(new Vector2(touchPos.x, Screen.height-touchPos.y));
            }
#else
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos=Input.mousePosition;

                clickPositions.Add(new Vector2(mousePos.x, Screen.height-mousePos.y));
            }
#endif



            if (clickPositions.Count>0)
            {
                rect.center      = GUIUtility.GUIToScreenPoint(rect.center);
                rangeRect.center = GUIUtility.GUIToScreenPoint(rangeRect.center);



                float curValue=mValue;

                for (int i=0; i<clickPositions.Count; ++i)
                {
                    Vector2 clickPos=(Vector2)clickPositions[i];

                    if (rect.Contains(clickPos))
                    {
                        value = mMinimum + ((clickPos.x-rangeRect.x)/rangeRect.width) * (mMaximum-mMinimum);
                        break;
                    }
                }

                if (mValue!=curValue)
                {
                    modificationMade();
                }
            }
        }
    }

    /// <summary>
    /// Handle keyboard events.
    /// </summary>
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
}
