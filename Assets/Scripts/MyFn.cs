using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFn
{

    public static float GetDeviceXYRotation(float tolerance = 0f)
    {
        Vector3 direction = Input.acceleration;
        if (Mathf.Abs(direction.z) > .85)
        {
            return 0;
        }
        direction = direction.WithZ(0).normalized;
        float angle = Mathf.Acos(Vector3.Dot(direction, Vector3.down)) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) <= tolerance)
            angle = 0;

        float sign = Mathf.Sign(Vector3.Cross(direction, Vector3.up).z);
        float val = angle * sign;
        return val;
    }
    public static float Remap(this float value, float FromLow, float FromHigh, float ToLow, float ToHigh)
    {
        return (value - FromLow) / (FromHigh - FromLow) * (ToHigh - ToLow) + ToLow;
    }
    public static Vector2 xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 WithZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

}
