using UnityEngine;
using System.Collections;

public class SelectionScroller : ModifiableObject
{
    private string[]         mItems;
    private int              mCurrentIndex;
    private GUIStyle         mLeftArrowStyle;
    private GUIStyle         mRightArrowStyle;
    private GUIStyle         centerTextStyle;

    public SelectionScroller(string[] items, string selectedItem, Texture2D leftTexture, Texture2D rightTexture) : base()
    {
        init(items, 0, leftTexture, rightTexture);

        setSelectedItem(selectedItem);
    }

    public SelectionScroller(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture) : base()
    {
        init(items, selectedItem, leftTexture, rightTexture);
    }

    public void init(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture)
    {
        mItems           = items;
        mCurrentIndex    = selectedItem;
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
        if (mCurrentIndex>0)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, rect.height, rect.height), "", mLeftArrowStyle))
            {
                --mCurrentIndex;
                modificationMade();
            }
        }
        else
        {
            Color tempColor  = GUI.color;
            Color alphaColor = new Color(tempColor.r, tempColor.g, tempColor.b, 0.3f);

            GUI.color=alphaColor;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.height, rect.height), mLeftArrowStyle.normal.background);
            GUI.color=tempColor;
        }

        if (mCurrentIndex>=0 && mCurrentIndex<mItems.Length)
        {
            centerTextStyle.fontSize=(int)(rect.height*0.5);

            Vector2 center=rect.center;
            GUI.Label(new Rect(center.x, center.y, 1, 1), mItems[mCurrentIndex], centerTextStyle);
        }

        if (mCurrentIndex<mItems.Length-1)
        {
            if (GUI.Button(new Rect(rect.xMax-rect.height, rect.y, rect.height, rect.height), "", mRightArrowStyle))
            {
                ++mCurrentIndex;
                modificationMade();
            }
        }
        else
        {
            Color tempColor  = GUI.color;
            Color alphaColor = new Color(tempColor.r, tempColor.g, tempColor.b, 0.3f);

            GUI.color=alphaColor;
            GUI.DrawTexture(new Rect(rect.xMax-rect.height, rect.y, rect.height, rect.height), mRightArrowStyle.normal.background);
            GUI.color=tempColor;
        }
    }

    public void control()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (mCurrentIndex>0)
            {
                --mCurrentIndex;
                modificationMade();
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mCurrentIndex<mItems.Length-1)
            {
                ++mCurrentIndex;
                modificationMade();
            }
        }
    }

    public string[] getItems()
    {
        return mItems;
    }

    public void setItems(string[] items)
    {
        mItems=items;
        setCurrentIndex(mCurrentIndex);
    }

    public string getSelectedItem()
    {
        if (mCurrentIndex>=0 && mCurrentIndex<mItems.Length)
        {
            return mItems[mCurrentIndex];
        }

        return "";
    }

    public void setSelectedItem(string item)
    {
        int index=0;

        for (int i=0; i<mItems.Length; ++i)
        {
            if (mItems[i].Equals(item))
            {
                mCurrentIndex=index;
                return;
            }
        }
    }

    public int getCurrentIndex()
    {
        return mCurrentIndex;
    }

    public void setCurrentIndex(int index)
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

        mCurrentIndex=index;
    }
}
