using UnityEngine;
using System.Collections;

public class SelectionScroller
{
    private string[] mItems;
    private int      mCurrent;
    private Texture  mLeftTexture;
    private Texture  mRightTexture;

    public SelectionScroller(string[] items, string selectedItem, Texture leftTexture, Texture rightTexture)
    {
        mItems   = items;
        mCurrent = 0;

        for (int i=0; i<mItems.Length; ++i)
        {
            if (mItems[i]==selectedItem)
            {
                mCurrent=i;
                break;
            }
        }

        mLeftTexture  = leftTexture;
        mRightTexture = rightTexture;
    }

    public SelectionScroller(string[] items, int selectedItem, Texture leftTexture, Texture rightTexture)
    {
        mItems        = items;
        mCurrent      = selectedItem;
        mLeftTexture  = leftTexture;
        mRightTexture = rightTexture;
    }

    public void draw(Rect rect)
    {
        if (mCurrent>0)
        {
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.height, rect.height), mLeftTexture);
        }

        if (mCurrent>=0 && mCurrent<mItems.Length)
        {
        }

        if (mCurrent<mItems.Length-1)
        {
            GUI.DrawTexture(new Rect(rect.xMax-rect.height, rect.y, rect.height, rect.height), mRightTexture);
        }
    }
}
