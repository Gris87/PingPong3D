using UnityEngine;
using System.Collections;

public class BigCheckBox : ModifiableObject
{
    private bool mChecked;

    public BigCheckBox() : base()
    {
        mChecked=false;
    }

    public void draw(Rect rect)
    {
    }

    public void control()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
