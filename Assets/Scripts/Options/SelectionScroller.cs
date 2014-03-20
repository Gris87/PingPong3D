using UnityEngine;
using System.Collections;

public class SelectionScroller
{
    private string[] mItems;
    private int      mCurrent;
    private GUIStyle mLeftArrowStyle;
    private GUIStyle mRightArrowStyle;
    private GUIStyle centerTextStyle;

    public SelectionScroller(string[] items, string selectedItem, Texture2D leftTexture, Texture2D rightTexture)
    {
        int index=0;

        for (int i=0; i<items.Length; ++i)
        {
            if (items[i]==selectedItem)
            {
                index=i;
                break;
            }
        }

        init(items, index, leftTexture, rightTexture);
    }

    public SelectionScroller(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture)
    {
        init(items, selectedItem, leftTexture, rightTexture);
    }

    public void init(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture)
    {
        mItems           = items;
        mCurrent         = selectedItem;
        mLeftArrowStyle  = new GUIStyle();
        mRightArrowStyle = new GUIStyle();
        centerTextStyle  = new GUIStyle();

        mLeftArrowStyle.normal.background  = leftTexture;
        mRightArrowStyle.normal.background = rightTexture;

        centerTextStyle.alignment=TextAnchor.MiddleCenter;
        centerTextStyle.clipping=TextClipping.Overflow;
        centerTextStyle.normal.textColor=Color.white;
    }

    public void draw(Rect rect)
    {
        if (mCurrent>0)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, rect.height, rect.height), "", mLeftArrowStyle))
            {
                --mCurrent;
            }
        }

        if (mCurrent>=0 && mCurrent<mItems.Length)
        {
            centerTextStyle.fontSize=24;

            Vector2 center=rect.center;
            GUI.Label(new Rect(center.x, center.y, 1, 1), mItems[mCurrent], centerTextStyle);
        }

        if (mCurrent<mItems.Length-1)
        {
            if (GUI.Button(new Rect(rect.xMax-rect.height, rect.y, rect.height, rect.height), "", mRightArrowStyle))
            {
                ++mCurrent;
            }
        }
    }

    public void control()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (mCurrent>0)
            {
                --mCurrent;
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mCurrent<mItems.Length-1)
            {
                ++mCurrent;
            }
        }
    }
}
