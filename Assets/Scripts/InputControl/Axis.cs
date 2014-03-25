public class Axis
{
    private string     mName;
    private KeyMapping mNegative;
    private KeyMapping mPositive;
    
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
    }
    
    public KeyMapping positive
    {
        get
        {
            return mPositive;
        }
    }
    
    public Axis(string aName, KeyMapping aNegative, KeyMapping aPositive)
    {
        mName=aName;
        set(aNegative, aPositive);
    }
    
    public void set(KeyMapping aNegative, KeyMapping aPositive)
    {
        mNegative = aNegative;
        mPositive = aPositive;
    }
    
    public float getValue()
    {
        bool negativePressing=mNegative.isPressed();
        bool positivePressing=mPositive.isPressed();
        
        if (negativePressing!=positivePressing)
        {
            if (negativePressing)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        
        return 0;
    }
}