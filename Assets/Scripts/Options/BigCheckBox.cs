using UnityEngine;
using System.Collections;

public class BigCheckBox : ModifiableObject
{
    private bool     mChecked;
    private GUIStyle mOnStyle;
    private GUIStyle mOffStyle;

    public BigCheckBox(Texture2D onTexture, Texture2D offTexture) : base()
    {
        mChecked  = false;
        mOnStyle  = new GUIStyle();
        mOffStyle = new GUIStyle();

        mOnStyle.normal.background  = onTexture;
        mOffStyle.normal.background = offTexture;
    }

    public void draw(Rect rect)
    {
        if (GUI.Button(new Rect(rect.x, rect.y, rect.height, rect.height), "", mChecked ? mOnStyle : mOffStyle))
        {
            mChecked=!mChecked;
            modificationMade();
        }
    }

    public void control()
    {
        if (InputControl.GetKeyDown(KeyCode.Space))
        {
            mChecked=!mChecked;
            modificationMade();
        }
    }

    public bool isChecked()
    {
        return mChecked;
    }

    public void setChecked(bool value)
    {
        mChecked=value;
    }
}
