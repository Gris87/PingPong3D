﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class InputControl
{
    // Set of keys
    private static List<KeyMapping>               mKeysList          = new List<KeyMapping>();
    private static Dictionary<string, KeyMapping> mKeysMap           = new Dictionary<string, KeyMapping>();

    // Set of axes
    private static List<Axis>                     mAxesList          = new List<Axis>();
    private static Dictionary<string, Axis>       mAxesMap           = new Dictionary<string, Axis>();

    // Smooth for GetAxis
    private static Dictionary<string, float>      mSmoothAxesValues  = new Dictionary<string, float>();
    private static float                          mSmoothCoefficient = 0.1f;

    // Joystick options
    private static float                          mJoystickThreshold = 0.2f;

    #region Properties
    
    #region Axis smooth
    public static float smoothCoefficient
    {
        get
        {
            return mSmoothCoefficient;
        }
        
        set
        {
            if (value<0.0001f)
            {
                mSmoothCoefficient=0.0001f;
            }
            else
                if (value>1f)
            {
                mSmoothCoefficient=1f;
            }
            else
            {
                mSmoothCoefficient=value;
            }
        }
    }
    #endregion
    
    #region Joystick threshold
    public static float joystickThreshold
    {
        get
        {
            return mJoystickThreshold;
        }
        
        set
        {
            if (value<0f)
            {
                mJoystickThreshold=0f;
            }
            else
                if (value>1f)
            {
                mJoystickThreshold=1f;
            }
            else
            {
                mJoystickThreshold=value;
            }
        }
    }
    #endregion
    
    #endregion

    #region Setup keys

    #region setKey with different arguments

    #region Level 1
    public static KeyMapping setKey(string aName, KeyCode primary)
    {
        return setKey(aName, argToInput(primary));
    }

    public static KeyMapping setKey(string aName, MouseAxis primary)
    {
        return setKey(aName, argToInput(primary));
    }

    public static KeyMapping setKey(string aName, MouseButton primary)
    {
        return setKey(aName, argToInput(primary));
    }
    #endregion

    // ============================================================================================================

    #region Level 2

    #region Level 2-0
    public static KeyMapping setKey(string aName, CustomInput primary, KeyCode secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }

    public static KeyMapping setKey(string aName, CustomInput primary, MouseAxis secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }

    public static KeyMapping setKey(string aName, CustomInput primary, MouseButton secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------

    #region Level 2-1
    public static KeyMapping setKey(string aName, KeyCode primary, CustomInput secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }

    public static KeyMapping setKey(string aName, KeyCode primary, KeyCode secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseAxis secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseButton secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------

    #region Level 2-2
    public static KeyMapping setKey(string aName, MouseAxis primary, CustomInput secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }

    public static KeyMapping setKey(string aName, MouseAxis primary, KeyCode secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseAxis secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseButton secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 2-3
    public static KeyMapping setKey(string aName, MouseButton primary, CustomInput secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }

    public static KeyMapping setKey(string aName, MouseButton primary, KeyCode secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseAxis secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseButton secondary)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary));
    }
    #endregion

    #endregion

    // ============================================================================================================

    #region Level 3

    #region Level 3-0

    #region Level 3-0-0
    public static KeyMapping setKey(string aName, CustomInput primary, CustomInput secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, CustomInput secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, CustomInput secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------

    #region Level 3-0-1
    public static KeyMapping setKey(string aName, CustomInput primary, KeyCode secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }

    public static KeyMapping setKey(string aName, CustomInput primary, KeyCode secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, KeyCode secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, KeyCode secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-0-2
    public static KeyMapping setKey(string aName, CustomInput primary, MouseAxis secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }

    public static KeyMapping setKey(string aName, CustomInput primary, MouseAxis secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, MouseAxis secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, MouseAxis secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-0-3
    public static KeyMapping setKey(string aName, CustomInput primary, MouseButton secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }

    public static KeyMapping setKey(string aName, CustomInput primary, MouseButton secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, MouseButton secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, CustomInput primary, MouseButton secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    #endregion

    // ************************************************************************************************************

    #region Level 3-1
    
    #region Level 3-1-0
    public static KeyMapping setKey(string aName, KeyCode primary, CustomInput secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }

    public static KeyMapping setKey(string aName, KeyCode primary, CustomInput secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, CustomInput secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, CustomInput secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-1-1
    public static KeyMapping setKey(string aName, KeyCode primary, KeyCode secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, KeyCode secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, KeyCode secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, KeyCode secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-1-2
    public static KeyMapping setKey(string aName, KeyCode primary, MouseAxis secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseAxis secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseAxis secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseAxis secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-1-3
    public static KeyMapping setKey(string aName, KeyCode primary, MouseButton secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseButton secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseButton secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, KeyCode primary, MouseButton secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    #endregion

    // ************************************************************************************************************
    
    #region Level 3-2
    
    #region Level 3-2-0
    public static KeyMapping setKey(string aName, MouseAxis primary, CustomInput secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, CustomInput secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, CustomInput secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, CustomInput secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-2-1
    public static KeyMapping setKey(string aName, MouseAxis primary, KeyCode secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, KeyCode secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, KeyCode secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, KeyCode secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-2-2
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseAxis secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseAxis secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseAxis secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseAxis secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-2-3
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseButton secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseButton secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseButton secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseAxis primary, MouseButton secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    #endregion

    // ************************************************************************************************************
    
    #region Level 3-3
    
    #region Level 3-3-0
    public static KeyMapping setKey(string aName, MouseButton primary, CustomInput secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, CustomInput secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, CustomInput secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, CustomInput secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-3-1
    public static KeyMapping setKey(string aName, MouseButton primary, KeyCode secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, KeyCode secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, KeyCode secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, KeyCode secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-3-2
    public static KeyMapping setKey(string aName, MouseButton primary, MouseAxis secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseAxis secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseAxis secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseAxis secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    // ------------------------------------------------------------------------------------------------------------
    
    #region Level 3-3-3
    public static KeyMapping setKey(string aName, MouseButton primary, MouseButton secondary, CustomInput third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseButton secondary, KeyCode third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseButton secondary, MouseAxis third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    
    public static KeyMapping setKey(string aName, MouseButton primary, MouseButton secondary, MouseButton third)
    {
        return setKey(aName, argToInput(primary), argToInput(secondary), argToInput(third));
    }
    #endregion
    
    #endregion

    #endregion

    // ============================================================================================================

    #region Argument to Input fuctions
    private static CustomInput argToInput(CustomInput arg)
    {
        return arg;
    }

    private static CustomInput argToInput(KeyCode arg)
    {
        return new KeyboardInput(arg);
    }

    private static CustomInput argToInput(MouseAxis arg)
    {
        return new MouseInput(arg);
    }

    private static CustomInput argToInput(MouseButton arg)
    {
        return new MouseInput(arg);
    }
    #endregion

    #endregion

    public static KeyMapping setKey(string aName, CustomInput primary=null, CustomInput secondary=null, CustomInput third=null)
    {
        KeyMapping outKey=null;

        if (mKeysMap.TryGetValue(aName, out outKey))
        {
            outKey.primaryInput   = primary;
            outKey.secondaryInput = secondary;
            outKey.thirdInput     = third;
        }
        else
        {
            outKey=new KeyMapping(aName, primary, secondary, third);

            mKeysList.Add(outKey);
            mKeysMap.Add (aName, outKey);
        }

        return outKey;
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
    public static Axis setAxis(string aName, string negative, string positive)
    {
        KeyMapping negativeKey=null;
        KeyMapping positiveKey=null;

        if (!mKeysMap.TryGetValue(negative, out negativeKey))
        {
            Debug.LogError("Negative key "+negative+" not found for axis "+aName);
            return null;
        }

        if (!mKeysMap.TryGetValue(positive, out positiveKey))
        {
            Debug.LogError("Positive key "+positive+" not found for axis "+aName);
            return null;
        }

        return setAxis(aName, negativeKey, positiveKey);
    }

    public static Axis setAxis(string aName, KeyMapping negative, KeyMapping positive)
    {
        Axis outAxis=null;
        
        if (mAxesMap.TryGetValue(aName, out outAxis))
        {
            outAxis.set(negative, positive);
        }
        else
        {
            outAxis=new Axis(aName, negative, positive);
            
            mAxesList.Add(outAxis);
            mAxesMap.Add (aName, outAxis);
        }

        return outAxis;
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

    public static CustomInput currentInput(bool ignoreMouseMovement=true)
    {
        #region Joystick
        // TODO: Get current input
        #endregion
        
        #region Mouse

        #region Axes

        #region Wheel
        float mouseAxis=Input.GetAxis("Mouse Wheel");
        
        if (mouseAxis<0)
        {
            return new MouseInput(MouseAxis.WheelDown);
        }
        
        if (mouseAxis>0)
        {
            return new MouseInput(MouseAxis.WheelUp);
        }
        #endregion

        #region X
        mouseAxis=Input.GetAxis("Mouse X");
        
        if (mouseAxis<0)
        {
            return new MouseInput(MouseAxis.MouseLeft);
        }
        
        if (mouseAxis>0)
        {
            return new MouseInput(MouseAxis.MouseRight);
        }
        #endregion

        #region Y
        mouseAxis=Input.GetAxis("Mouse Y");
        
        if (mouseAxis<0)
        {
            return new MouseInput(MouseAxis.MouseDown);
        }
        
        if (mouseAxis>0)
        {
            return new MouseInput(MouseAxis.MouseUp);
        }        
        #endregion

        #endregion

        for (int i=0; i<(int)MouseButton.None; ++i)
        {
            KeyCode key=(KeyCode)((int)KeyCode.Mouse0+i);
            
            if (Input.GetKey(key))
            {
                return new MouseInput((MouseButton)i);
            }
        }

        #endregion
        
        #region Keyboard
        foreach (var value in Enum.GetValues(typeof(KeyCode)))
        {
            KeyCode key=(KeyCode)value;
            
            if (Input.GetKey(key))
            {
                return new KeyboardInput(key);
            }
        }
        #endregion        

        return null;
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
        float previousValue;

        if (!mSmoothAxesValues.TryGetValue(axisName, out previousValue))
        {
            previousValue=0f;
        }

        float newValue = GetAxisRaw(axisName);
        float res      = previousValue+(newValue-previousValue)*mSmoothCoefficient;

        mSmoothAxesValues[axisName]=res;

        return res;
    }

    public static float GetAxisRaw(string axisName)
    {
        Axis outAxis=null;
        
        if (!mAxesMap.TryGetValue(axisName, out outAxis))
        {
            Debug.LogError("Axis "+axisName+" not found");
            return 0;
        }

        return outAxis.getValue();
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
        if (button>=0 && button<(int)MouseButton.None)
        {
            return GetMouseButton((MouseButton)button);
        }

        return false;
    }

    public static bool GetMouseButton(MouseButton button)
    {
        if (button!=MouseButton.None)
        {
            return Input.GetKey((KeyCode)((int)KeyCode.Mouse0 + (int)button));
        }

        return false;
    }

    public static bool GetMouseButtonDown(int button)
    {
        if (button>=0 && button<(int)MouseButton.None)
        {
            return GetMouseButtonDown((MouseButton)button);
        }
        
        return false;
    }
    
    public static bool GetMouseButtonDown(MouseButton button)
    {
        if (button!=MouseButton.None)
        {
            return Input.GetKeyDown((KeyCode)((int)KeyCode.Mouse0 + (int)button));
        }
        
        return false;
    }

    public static bool GetMouseButtonUp(int button)
    {
        if (button>=0 && button<(int)MouseButton.None)
        {
            return GetMouseButtonUp((MouseButton)button);
        }
        
        return false;
    }
    
    public static bool GetMouseButtonUp(MouseButton button)
    {
        if (button!=MouseButton.None)
        {
            return Input.GetKeyUp((KeyCode)((int)KeyCode.Mouse0 + (int)button));
        }
        
        return false;
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
