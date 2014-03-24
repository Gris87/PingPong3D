using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputControl
{
    #region KeyMapping
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
    }
    #endregion

    #region Axis
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
    }
    #endregion

    private static List<KeyMapping>               mKeysList = new List<KeyMapping>();
    private static Dictionary<string, KeyMapping> mKeysMap  = new Dictionary<string, KeyMapping>();

    private static List<Axis>                     mAxesList = new List<Axis>();
    private static Dictionary<string, Axis>       mAxesMap  = new Dictionary<string, Axis>();

    #region Setup keys
    public static void setKey(string aName, KeyCode aPrimaryCode, KeyCode aSecondaryCode = KeyCode.None)
    {
        KeyMapping outKey=null;

        if (mKeysMap.TryGetValue(aName, out outKey))
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
        KeyMapping outKey=null;

        if (mKeysMap.TryGetValue(aName, out outKey))
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
        KeyMapping aNegativeKey=null;
        KeyMapping aPositiveKey=null;

        if (!mKeysMap.TryGetValue(aNegative, out aNegativeKey))
        {
            Debug.LogError("Negative key "+aNegative+" not found for axis "+aName);
            return;
        }

        if (!mKeysMap.TryGetValue(aPositive, out aPositiveKey))
        {
            Debug.LogError("Positive key "+aPositive+" not found for axis "+aName);
            return;
        }

        setAxis(aName, aNegativeKey, aPositiveKey);
    }

    public static void setAxis(string aName, KeyMapping aNegative, KeyMapping aPositive)
    {
        Axis outAxis=null;
        
        if (mAxesMap.TryGetValue(aName, out outAxis))
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
        Axis outAxis=null;

        if (mAxesMap.TryGetValue(aName, out outAxis))
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

    public static bool anyKey
    {
        get
        {
            return Input.anyKey;
        }
    }

    public static bool anyKeyDown
    {
        get
        {
            return Input.anyKeyDown;
        }
    }

    public static Compass compass
    {
        get
        {
            return Input.compass;
        }
    }

    public static bool compensateSensors
    {
        get
        {
            return Input.compensateSensors;
        }
    }

    public static Vector2 compositionCursorPos
    {
        get
        {
            return Input.compositionCursorPos;
        }
    }
}
