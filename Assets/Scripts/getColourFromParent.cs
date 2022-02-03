using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class getColourFromParent : MonoBehaviour
{
    public Color parentColor;

    private Color activeColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    private Color inactiveColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);

    public bool useHighlighterColour = false;

    private GameObject point;
    private GameObject line;

    private TextMeshPro text;


    void Start()
    {
        point = transform.Find("MarkerPoint").gameObject;
        line = transform.Find("MarkerLine").gameObject;

        text = transform.Find("Text (TMP)").GetComponent<TextMeshPro>();


    }

    void Update()
    {
        if (useHighlighterColour)
        {
            parentColor = activeColor;
        } else
        {
            parentColor = transform.parent.GetComponent<Renderer>().material.GetColor("_BaseColor");
        }


        point.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        line.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        //darker colour for the text
        text.faceColor = inactiveColor;

    }
}
