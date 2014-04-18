using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="SelectionScroller"/> is a UI control that provide selection one entry from the list.
/// </summary>
public class SelectionScroller : ModifiableObject
{
    private string[] mItems;
    private int      mCurrentIndex;
    private GUIStyle mLeftArrowStyle;
    private GUIStyle mRightArrowStyle;
    private GUIStyle centerTextStyle;

    #region Properties
    /// <summary>
    /// Gets or sets the list of items.
    /// </summary>
    /// <value>List of items.</value>
    public string[] items
    {
        get
        {
            return mItems;
        }

        set
        {
            mItems=value;
            currentIndex=mCurrentIndex;
        }
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    /// <value>Selected item.</value>
    public string selectedItem
    {
        get
        {
            if (mCurrentIndex>=0 && mCurrentIndex<mItems.Length)
            {
                return mItems[mCurrentIndex];
            }

            return "";
        }

        set
        {
            int index=0;

            for (int i=0; i<mItems.Length; ++i)
            {
                if (mItems[i].Equals(value))
                {
                    mCurrentIndex=index;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the index of the current item.
    /// </summary>
    /// <value>The index of the current item.</value>
    public int currentIndex
    {
        get
        {
            return mCurrentIndex;
        }

        set
        {
            if (value<0)
            {
                mCurrentIndex=0;
            }
            else
            if (value>mItems.Length-1)
            {
                mCurrentIndex=mItems.Length-1;
            }
            else
            {
                mCurrentIndex=value;
            }
        }
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionScroller"/> class.
    /// </summary>
    /// <param name="items">List of items.</param>
    /// <param name="selectedItem">Selected item.</param>
    /// <param name="leftTexture">Image for left arrow.</param>
    /// <param name="rightTexture">Image for right arrow.</param>
    public SelectionScroller(string[] items, string selectedItem, Texture2D leftTexture, Texture2D rightTexture) : base()
    {
        init(items, 0, leftTexture, rightTexture);

        this.selectedItem=selectedItem;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionScroller"/> class.
    /// </summary>
    /// <param name="items">List of items.</param>
    /// <param name="selectedItem">Index of selected item.</param>
    /// <param name="leftTexture">Image for left arrow.</param>
    /// <param name="rightTexture">Image for right arrow.</param>
    public SelectionScroller(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture) : base()
    {
        init(items, selectedItem, leftTexture, rightTexture);
    }

    /// <summary>
    /// Initialization for created instance.
    /// </summary>
    /// <param name="items">List of items.</param>
    /// <param name="selectedItem">Index of selected item.</param>
    /// <param name="leftTexture">Image for left arrow.</param>
    /// <param name="rightTexture">Image for right arrow.</param>
    private void init(string[] items, int selectedItem, Texture2D leftTexture, Texture2D rightTexture)
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

    /// <summary>
    /// Draw control in specified rectangle and handles user interactions.
    /// </summary>
    /// <param name="rect">Control location.</param>
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

    /// <summary>
    /// Handle keyboard events.
    /// </summary>
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
}
