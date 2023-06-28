using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathFuncs 
{
    public static float WeibullFunc(float shape, float scale, float x) {
        if (shape < 0)
            return 0;
        else if (scale < 0)
            return 0;
        else if (x < 0)
            return 0;
        else
            return (shape / scale)*Mathf.Pow((x/scale),shape-1)*Mathf.Exp(-Mathf.Pow(x/scale,shape));
        
    }
    static public float NormalizedPoint(float ceiling, float floor,float actualPoint)
    {
        return Mathf.Clamp01((actualPoint - ceiling) / (-ceiling + floor));
    }
   static public float NormalizedInversePoint(float ceiling,float floor,float y)
    {
        return 1 - Mathf.Clamp01((y - ceiling) / (-ceiling + floor));
    }

    static public bool IsInThere(float leastOpenV, float greatClosedV, float theV)
    {
        if (theV > leastOpenV && theV <= greatClosedV)
            return true;
        else
            return false;
    }
    public static int AgnessiFunc(float topLevel, float maxPoints, int actualX)
    {
        float a = maxPoints / 2;
        return Mathf.RoundToInt(8 * Mathf.Pow(a, 3) / ((Mathf.Pow((actualX - topLevel), 2)) + 4 * Mathf.Pow(a, 2)));

    }

    public static float FloatEulerFunc(float numBase, float numTope, int x, int interval)
    {
        float k = Mathf.Log(numTope / numBase) / (interval);
        return numBase * Mathf.Exp(k * x);
    }
    public static float Part(float numBase, float numTope, int length, int x)
    {
        float b = (numTope - numBase) / length;
        // Debug.Log(numBase + (b * x));
        return numBase + (b * x);
    }
}
