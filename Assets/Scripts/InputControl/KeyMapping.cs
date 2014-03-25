using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyMapping
{
    private string  mName;
    private KeyCode mPrimaryCode;
    private KeyCode mSecondaryCode;
    
    public string name
    {
        get
        {
            return mName;
        }
    }
    
    public KeyCode primaryCode
    {
        get
        {
            return mPrimaryCode;
        }
    }
    
    public KeyCode secondaryCode
    {
        get
        {
            return mSecondaryCode;
        }
    }
    
    public KeyMapping(string aName, KeyCode aPrimaryCode, KeyCode aSecondaryCode = KeyCode.None)
    {
        mName=aName;
        set(aPrimaryCode, aSecondaryCode);
    }
    
    public void set(KeyCode aPrimaryCode, KeyCode aSecondaryCode = KeyCode.None)
    {
        mPrimaryCode   = aPrimaryCode;
        mSecondaryCode = aSecondaryCode;
    }
    
    public void set(KeyMapping another)
    {
        mPrimaryCode   = another.mPrimaryCode;
        mSecondaryCode = another.mSecondaryCode;
    }
    
    public bool isPressed()
    {
        if (Input.GetKey(mPrimaryCode))
        {
            return true;
        }
        
        if (Input.GetKey(mSecondaryCode))
        {
            return true;
        }
        
        return false;
    }
    
    public bool isPressedDown()
    {
        if (Input.GetKeyDown(mPrimaryCode))
        {
            return true;
        }
        
        if (Input.GetKeyDown(mSecondaryCode))
        {
            return true;
        }
        
        return false;
    }
    
    public bool isPressedUp()
    {
        if (Input.GetKeyUp(mPrimaryCode))
        {
            return true;
        }
        
        if (Input.GetKeyUp(mSecondaryCode))
        {
            return true;
        }
        
        return false;
    }
}