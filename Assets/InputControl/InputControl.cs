using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputControl
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

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

    public static string compositionString
    {
        get
        {
            return Input.compositionString;
        }
    }

    public static DeviceOrientation deviceOrientation
    {
        get
        {
            return Input.deviceOrientation;
        }
    }

    public static AccelerationEvent GetAccelerationEvent(int index)
    {
        return Input.GetAccelerationEvent(index);
    }

    public static float GetAxis(string axisName)
    {
        // TODO: Smooth difference from previous value (0 - 0.5 - 0.75 - 0.875 - 1)
        return GetAxisRaw(axisName);
    }

    public static float GetAxisRaw(string axisName)
    {
        Axis outAxis=null;
        
        if (!mAxesMap.TryGetValue(axisName, out outAxis))
        {
            Debug.LogError("Axis "+axisName+" not found");
            return 0;
        }

        bool negativePressing=GetButton(outAxis.negative.name);
        bool positivePressing=GetButton(outAxis.positive.name);

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

    public static bool GetButton(string buttonName)
    {
        KeyMapping outKey=null;
        
        if (!mKeysMap.TryGetValue(buttonName, out outKey))
        {
            Debug.LogError("Key "+buttonName+" not found");
            return false;
        }

        return outKey.isPressed();
    }

    public static bool GetButtonDown(string buttonName)
    {
        KeyMapping outKey=null;
        
        if (!mKeysMap.TryGetValue(buttonName, out outKey))
        {
            Debug.LogError("Key "+buttonName+" not found");
            return false;
        }

        return outKey.isPressedDown();
    }

    public static bool GetButtonUp(string buttonName)
    {
        KeyMapping outKey=null;
        
        if (!mKeysMap.TryGetValue(buttonName, out outKey))
        {
            Debug.LogError("Key "+buttonName+" not found");
            return false;
        }
        
        return outKey.isPressedUp();
    }

    public static string[] GetJoystickNames()
    {
        return Input.GetJoystickNames();
    }

    public static bool GetKey(string name)
    {
        return Input.GetKey(name);
    }

    public static bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public static bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(name);
    }
    
    public static bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public static bool GetKeyUp(string name)
    {
        return Input.GetKeyUp(name);
    }
    
    public static bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public static bool GetMouseButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public static bool GetMouseButton(MouseButton button)
    {
        return Input.GetMouseButton((int)button);
    }

    public static bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }
    
    public static bool GetMouseButtonDown(MouseButton button)
    {
        return Input.GetMouseButtonDown((int)button);
    }

    public static bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }
    
    public static bool GetMouseButtonUp(MouseButton button)
    {
        return Input.GetMouseButtonUp((int)button);
    }

    public static Touch GetTouch(int button)
    {
        return Input.GetTouch(button);
    }

    public static Gyroscope gyro
    {
        get
        {
            return Input.gyro;
        }
    }

    public static IMECompositionMode imeCompositionMode
    {
        get
        {
            return Input.imeCompositionMode;
        }

        set
        {
            Input.imeCompositionMode=value;
        }
    }

    public static bool imeIsSelected
    {
        get
        {
            return Input.imeIsSelected;
        }
    }

    public static string inputString
    {
        get
        {
            return Input.inputString;
        }
    }

    public static LocationService location
    {
        get
        {
            return Input.location;
        }
    }

    public static Vector3 mousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }

    public static bool mousePresent
    {
        get
        {
            return Input.mousePresent;
        }
    }

    public static bool multiTouchEnabled
    {
        get
        {
            return Input.multiTouchEnabled;
        }

        set
        {
            Input.multiTouchEnabled=value;
        }
    }

    public static void ResetInputAxes()
    {
        Input.ResetInputAxes();
    }

    public static bool simulateMouseWithTouches
    {
        get
        {
            return Input.simulateMouseWithTouches;
        }
        
        set
        {
            Input.simulateMouseWithTouches=value;
        }
    }

    public static int touchCount
    {
        get
        {
            return Input.touchCount;
        }
    }

    public static Touch[] touches
    {
        get
        {
            return Input.touches;
        }
    }
}
