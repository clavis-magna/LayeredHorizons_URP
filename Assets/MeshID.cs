using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshID : MonoBehaviour
{

    public string meshName;


    [HideInInspector] // Hides var below
    public bool meshLoaded;

    // Start is called before the first frame update
    void Start()
    {
        name = meshName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
