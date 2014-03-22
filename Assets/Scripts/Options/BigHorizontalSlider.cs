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
            
            if (Utils.isTouchDevice)
            {
                foreach (Touch touch in Input.touches)
                {
                    Vector2 touchPos=touch.position;
                    
                    clickPositions.Add(new Vector2(touchPos.x, Screen.height-touchPos.y));
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
