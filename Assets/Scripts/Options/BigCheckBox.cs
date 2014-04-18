using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="BigCheckBox"/> is a UI control that has 2 states (ON and OFF).
/// </summary>
public class BigCheckBox : ModifiableObject
{
    private bool     mChecked;
    private GUIStyle mOnStyle;
    private GUIStyle mOffStyle;

    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BigCheckBox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if checked ; otherwise, <c>false</c>.</value>
    public bool isChecked
    {
        get
        {
            return mChecked;
        }

        set
        {
            mChecked=value;
        }
    }
    #endregion

    /// <summary>
    /// Create a new instance of <see cref="BigCheckBox"/>.
    /// </summary>
    /// <param name="onTexture">Image for state ON.</param>
    /// <param name="offTexture">Image for state OFF.</param>
    public BigCheckBox(Texture2D onTexture, Texture2D offTexture) : base()
    {
        mChecked  = false;
        mOnStyle  = new GUIStyle();
        mOffStyle = new GUIStyle();

        mOnStyle.normal.background  = onTexture;
        mOffStyle.normal.background = offTexture;
    }

    /// <summary>
    /// Draw control in specified rectangle and handles user interactions.
    /// </summary>
    /// <param name="rect">Control location.</param>
    public void draw(Rect rect)
    {
        if (GUI.Button(new Rect(rect.x, rect.y, rect.height, rect.height), "", mChecked ? mOnStyle : mOffStyle))
        {
            mChecked=!mChecked;
            modificationMade();
        }
    }

    /// <summary>
    /// Handle keyboard events.
    /// </summary>
    public void control()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mChecked=!mChecked;
            modificationMade();
        }
    }
}
