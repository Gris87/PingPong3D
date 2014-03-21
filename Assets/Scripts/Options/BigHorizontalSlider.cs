using UnityEngine;
using System.Collections;

public class BigHorizontalSlider : ModifiableObject
{
    private static Texture2D mValueTexture=null;

    private float mValue;
    private float mMinimum;
    private float mMaximum;

    public BigHorizontalSlider(float value, float minimum, float maximum) : base()
    {
        if (mValueTexture==null)
        {
            mValueTexture=new Texture2D(1, 1);
            mValueTexture.SetPixel(0, 0, Color.green);
        }

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
        GUI.Box(rect, "");

        Rect rangeRect=new Rect(rect.x+rect.width*0.02f, rect.y+rect.height*0.1f, rect.width*0.96f, rect.height*0.8f);
        GUI.DrawTexture(new Rect(rangeRect.x, rangeRect.y, rangeRect.width*(mValue-mMinimum)/(mMaximum-mMinimum), rangeRect.height), mValueTexture);



        ArrayList clickPositions=new ArrayList();

        if (Utils.isTouchDevice)
        {
            foreach (Touch touch in Input.touches)
            {
                clickPositions.Add(touch.position);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos=Input.mousePosition;

                clickPositions.Add(new Vector2(mousePos.x, Screen.height-mousePos.y));
            }
        }



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
                    setValue(mMinimum+((clickPos.x-rangeRect.x)/rangeRect.width)*(mMaximum-mMinimum));
                    break;
                }
            }
            
            if (mValue!=curValue)
            {
                modificationMade();
            }
        }
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
