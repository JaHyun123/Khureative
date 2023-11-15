using UnityEngine;
using System;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua
{
    [Serializable]
    public class Injection
    {
        public GameObjectValue[]  gameObjectValues;
        public Vector3Value[]     vector3Values;
        public FloatValue[]       floatValue;
        public IntValue[]         intValue;
        public BoolValue[]        boolValue;
        public StringValue[]      stringValue;
        public ColorValue[]       colorValue;
        public VivenScriptValue[] vivenScriptValue;
    }

    [Serializable]
    public class GameObjectValue
    {
        public string     name;
        public GameObject value;
    }

    [Serializable]
    public class Vector3Value
    {
        public string  name;
        public Vector3 value;
    }

    [Serializable]
    public class FloatValue
    {
        public string name;
        public float  value;
    }

    [Serializable]
    public class IntValue
    {
        public string name;
        public int    value;
    }

    [Serializable]
    public class BoolValue
    {
        public string name;
        public bool   value;
    }

    [Serializable]
    public class StringValue
    {
        public string name;
        public string value;
    }

    [Serializable]
    public class ColorValue
    {
        public string name;
        public Color  value;
    }

    [Serializable]
    public class VivenScriptValue
    {
        public string            name;
        public VivenLuaBehaviour value;
    }


    public class VivenLuaBehaviour : MonoBehaviour
    {
        public VivenScript luaScript;
        public Injection   injection;
        
        //TODO 현재 사용하지 않으니 숨김
        // public bool        shouldSync  = true;
        // public bool        isGrabbable = false;
    }
}