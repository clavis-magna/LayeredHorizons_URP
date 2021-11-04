using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getColourFromParent : MonoBehaviour
{
    public Color parentColor;

    private GameObject point;
    private GameObject line;


    void Start()
    {
        point = transform.FindChild("MarkerPoint").gameObject;
        line = transform.FindChild("MarkerLine").gameObject;

    }

    void Update()
    {
        point.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        line.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
    }
}
