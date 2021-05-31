using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateWorldScale : MonoBehaviour
{

    // _mapScale accessable and setable in the inspector
    [Header("Set overall world scale (in world units)")]
    public Vector2 _mapScale;

    // mapScale accessable to all other scripts 
    // as a static variable it can only have 1 value across the entire project)
    public static Vector2 mapScale;

    // Start is called before the first frame update
    void Awake()
    {
        mapScale = _mapScale;
    }
}
