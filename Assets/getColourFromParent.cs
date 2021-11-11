using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class getColourFromParent : MonoBehaviour
{
    public Color parentColor;

    private GameObject point;
    private GameObject line;

    private TextMeshPro text;


    void Start()
    {
        point = transform.FindChild("MarkerPoint").gameObject;
        line = transform.FindChild("MarkerLine").gameObject;

        text = transform.FindChild("Text (TMP)").GetComponent<TextMeshPro>();


    }

    void Update()
    {
        point.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        line.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        //darker colour for the text
        text.faceColor = new Color(parentColor.r / 2, parentColor.g / 2, parentColor.b / 2, 255);

    }
}
