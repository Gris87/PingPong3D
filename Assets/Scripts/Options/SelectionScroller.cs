using UnityEngine;
using System.Collections;

public class SelectionScroller
{
    public delegate void ModifiedFunction();

    private string[]         mItems;
    private int              mCurrent;
    private ModifiedFunction mModifiedFunction;
    private GUIStyle         mLeftArrowStyle;
    private GUIStyle         mRightArrowStyle;
    private GUIStyle         centerTextStyle;

    public SelectionScroller(string[] items, string selectedItem, ModifiedFunction modifiedFunction, Texture2D leftTexture, Texture2D rightTexture)
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

        init(items, index, modifiedFunction, leftTexture, rightTexture);
    }

    public SelectionScroller(string[] items, int selectedItem, ModifiedFunction modifiedFunction, Texture2D leftTexture, Texture2D rightTexture)
    {
        init(items, selectedItem, modifiedFunction, leftTexture, rightTexture);
    }

    public void init(string[] items, int selectedItem, ModifiedFunction modifiedFunction, Texture2D leftTexture, Texture2D rightTexture)
    {
        mItems            = items;
        mCurrent          = selectedItem;
        mModifiedFunction = modifiedFunction;
        mLeftArrowStyle   = new GUIStyle();
        mRightArrowStyle  = new GUIStyle();
        centerTextStyle   = new GUIStyle();

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
                modificationMade();
            }
        }

        if (mCurrent>=0 && mCurrent<mItems.Length)
        {
            centerTextStyle.fontSize=(int)(rect.height*0.5);

            Vector2 center=rect.center;
            GUI.Label(new Rect(center.x, center.y, 1, 1), mItems[mCurrent], centerTextStyle);
        }

        if (mCurrent<mItems.Length-1)
        {
            if (GUI.Button(new Rect(rect.xMax-rect.height, rect.y, rect.height, rect.height), "", mRightArrowStyle))
            {
                ++mCurrent;
                modificationMade();
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
                modificationMade();
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mCurrent<mItems.Length-1)
            {
                ++mCurrent;
                modificationMade();
            }
        }
    }

    private void modificationMade()
    {        
        if (mModifiedFunction!=null)
        {
            mModifiedFunction();
        }
    }

    public string getSelectedItem()
    {
        if (mCurrent>=0 && mCurrent<mItems.Length)
        {
            return mItems[mCurrent];
        }

        return "";
    }

    public void setCurrentItem(int index)
    {
        if (index<0)
        {
            index=0;
        }
        else
        if (index>mItems.Length-1)
        {
            index=mItems.Length-1;
        }

        mCurrent=index;
    }
}
