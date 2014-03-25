using UnityEngine;

public class Axis
{
    private string     mName;
    private KeyMapping mNegative;
    private KeyMapping mPositive;

    #region Properties
    public string name
    {
        get
        {
            return mName;
        }
    }
    
    public KeyMapping negative
    {
        get
        {
            return mNegative;
        }
        
        set
        {
            if (value==null)
            {
                Debug.LogError("value can't be null");
            }

            mNegative=value;
        }
    }
    
    public KeyMapping positive
    {
        get
        {
            return mPositive;
        }

        set
        {
            if (value==null)
            {
                Debug.LogError("value can't be null");
            }

            mPositive=value;
        }
    }
    #endregion
    
    public Axis(string aName, KeyMapping aNegative, KeyMapping aPositive)
    {
        mName=aName;
        set(aNegative, aPositive);
    }

    public Axis(Axis another)
    {
        mName=another.mName;
        set(another);
    }

    public void set(Axis another)
    {
        mNegative = another.mNegative;
        mPositive = another.mPositive;
    }
    
    public void set(KeyMapping aNegative, KeyMapping aPositive)
    {
        mNegative = aNegative;
        mPositive = aPositive;
    }
    
    public float getValue()
    {
        return mPositive.getValue()-mNegative.getValue();
    }
}