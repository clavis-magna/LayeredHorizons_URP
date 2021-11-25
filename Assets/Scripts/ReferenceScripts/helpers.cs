using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class helpers
{

    //
    // getXYPos (should actually be get XZPos
    // todo: maybe refactor with a better method name at some stage
    // see: https://stackoverflow.com/questions/1369512/converting-longitude-latitude-to-x-y-on-a-map-with-calibration-points
    //
    public static float[] getXYPos(float lat, float lon, float scaleX, float scaleY)
    {
        float[] xy = new float[2];
        float x = (scaleX * lon / 180) - 180;
        float y = (scaleY * lat / 360);
        xy[0] = x;
        xy[1] = y;
        return xy;
    }


    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}


//extension for converting the vector3 List to vector2 array hacked together
//removes the y Axis
//see: https://answers.unity.com/questions/1034471/c-convert-vector3-to-vector2.html
public static class MyVector3Extension
{
    // public static Vector2[] toVector2 (this Vector3[] v3)
    // {
    //     return System.Array.ConvertAll<Vector3, Vector2> (v3, getV3fromV2);
    // }
    public static Vector2[] toVector2 (this List<Vector3> listV3)
    {
        Vector3[] v3 = listV3.ToArray();
        return System.Array.ConvertAll<Vector3, Vector2> (v3, getV3fromV2);
    }


    public static Vector2 getV3fromV2 (Vector3 v3)
    {
        return new Vector2 (v3.x, v3.z);
    }
}



//
// JsonHelper class for extending Unity's JsonUtility to work with json with multiple data objects (like we have)
// see: https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
//
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

