using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputControl
{
    #region KeyMapping
    public static class KeyMapping
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
    }
    #endregion

    #region Axis
    public static class Axis
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
    }
    #endregion

    private static List<KeyMapping>               mKeysList = new List();
    private static Dictionary<string, KeyMapping> mKeysMap  = new Dictionary();

    private static List<Axis>                     mAxesList = new List();
    private static Dictionary<string, Axis>       mAxesMap  = new Dictionary();

    #region Setup keys
    public static void setKey(string aName, KeyCode aPrimaryCode, KeyCode aSecondaryCode = KeyCode.None)
    {
        KeyMapping outKey;

        if (mKeysMap.TryGetValue(aName, outKey))
        {
            outKey.set(aPrimaryCode, aSecondaryCode);
        }
        else
        {
            KeyMapping newKey=new KeyMapping(aName, aPrimaryCode, aSecondaryCode);

            mKeysList.Add(newKey);
            mKeysMap.Add (aName, newKey);
        }
    }

    public static void removeKey(string aName)
    {
        KeyMapping outKey;

        if (mKeysMap.TryGetValue(aName, outKey))
        {
            mKeysList.Remove(outKey);
            mKeysMap.Remove (aName);
        }
    }

    public static List<KeyMapping> getKeys()
    {
        return mKeysList;
    }
    #endregion

    #region Setup axes
    public static void setAxis(string aName, string aNegative, string aPositive)
    {
        KeyMapping aNegativeKey;
        KeyMapping aPositiveKey;

        if (!mAxesMap.TryGetValue(aNegative, aNegativeKey))
        {
            Debug.LogError("Negative key "+aNegative+" not found for axis "+aName);
            return;
        }

        if (!mAxesMap.TryGetValue(aPositive, aPositiveKey))
        {
            Debug.LogError("Positive key "+aPositive+" not found for axis "+aName);
            return;
        }

        setAxis(aName, aNegativeKey, aPositiveKey);
    }

    public static void setAxis(string aName, KeyMapping aNegative, KeyMapping aPositive)
    {
        Axis outAxis;
        
        if (mAxesMap.TryGetValue(aName, outAxis))
        {
            outAxis.set(aNegative, aPositive);
        }
        else
        {
            Axis newAxis=new Axis(aName, aNegative, aPositive);
            
            mAxesList.Add(newAxis);
            mAxesMap.Add (aName, newAxis);
        }
    }
    
    public static void removeAxis(string aName)
    {
        Axis outAxis;

        if (mAxesMap.TryGetValue(aName, outAxis))
        {
            mAxesList.Remove(outAxis);
            mAxesMap.Remove (aName);
        }
    }
    
    public static List<Axis> getAxes()
    {
        return mAxesList;
    }
    #endregion

    // ----------------------------------------------------------------

    public static Vector3 acceleration
    {
        get
        {
            return Input.acceleration;
        }
    }

    public static int accelerationEventCount
    {
        get
        {
            return Input.accelerationEventCount;
        }
    }

    public static AccelerationEvent[] accelerationEvents
    {
        get
        {
            return Input.accelerationEvents;
        }
    }
}
