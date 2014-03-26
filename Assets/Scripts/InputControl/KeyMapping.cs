public class KeyMapping
{
    private string      mName;
    private CustomInput mPrimaryInput;
    private CustomInput mSecondaryInput;
    private CustomInput mThirdInput;

    #region Properties
    public string name
    {
        get
        {
            return mName;
        }
    }
    
    public CustomInput primaryInput
    {
        get
        {
            return mPrimaryInput;
        }

        set
        {
            if (value==null)
            {
                mPrimaryInput=new KeyboardInput();
            }
            else
            {
                mPrimaryInput=value;
            }
        }
    }
    
    public CustomInput secondaryInput
    {
        get
        {
            return mSecondaryInput;
        }

        set
        {
            if (value==null)
            {
                mSecondaryInput=new KeyboardInput();
            }
            else
            {
                mSecondaryInput=value;
            }
        }
    }

    public CustomInput thirdInput
    {
        get
        {
            return mThirdInput;
        }

        set
        {
            if (value==null)
            {
                mThirdInput=new KeyboardInput();
            }
            else
            {
                mThirdInput=value;
            }
        }
    }
    #endregion
    
    public KeyMapping(string aName="", CustomInput aPrimaryInput=null, CustomInput aSecondaryInput=null, CustomInput aThirdInput=null)
    {
        mName=aName;
        primaryInput   = aPrimaryInput;
        secondaryInput = aSecondaryInput;
        thirdInput     = aThirdInput;
    }

    public KeyMapping(KeyMapping another)
    {
        mName=another.mName;
        set(another);
    }
    
    public void set(KeyMapping another)
    {
        mPrimaryInput   = another.mPrimaryInput;
        mSecondaryInput = another.mSecondaryInput;
        mThirdInput     = another.mThirdInput;
    }

    public float getValue()
    {
        float res;

        res=mPrimaryInput.getInput();

        if (res!=0)
        {
            return res;
        }

        res=mSecondaryInput.getInput();
        
        if (res!=0)
        {
            return res;
        }

        res=mThirdInput.getInput();

        return res;
    }

    public float getValueDown()
    {
        float res;
        
        res=mPrimaryInput.getInputDown();
        
        if (res!=0)
        {
            return res;
        }
        
        res=mSecondaryInput.getInputDown();
        
        if (res!=0)
        {
            return res;
        }
        
        res=mThirdInput.getInputDown();
        
        return res;
    }

    public float getValueUp()
    {
        float res;
        
        res=mPrimaryInput.getInputUp();
        
        if (res!=0)
        {
            return res;
        }
        
        res=mSecondaryInput.getInputUp();
        
        if (res!=0)
        {
            return res;
        }
        
        res=mThirdInput.getInputUp();
        
        return res;
    }
    
    public bool isPressed()
    {        
        return getValue()     != 0;
    }
    
    public bool isPressedDown()
    {
        return getValueDown() != 0;
    }
    
    public bool isPressedUp()
    {
        return getValueUp()   != 0;
    }
}