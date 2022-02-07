using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class getColourFromParent : MonoBehaviour
{
    //this is what the colour actually will be
    public Color parentColor;

    //For what the colour is when highlighted
    private Color activeColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    //Only for text when not highlighted at all
    private Color inactiveColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);

    public bool useHighlighterColour = false;

    private GameObject point;
    private GameObject line;

    private TextMeshPro text;


    void Start()
    {
        point = transform.Find("MarkerPoint").gameObject;
        line = point.transform.Find("MarkerLine").gameObject;

        //adding the changing markerline length because I can't be bothered to make another script yet for a UI I don't know if I like or not.
        //This changed the line to be longer at the bottom and not increase at the top
        //for some reason translating the position it needs to be divided by 10
        line.transform.Translate(0, -100f/10f, 0);
        line.transform.localScale = line.transform.localScale + new Vector3(0, 100.0f, 0);



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
        //point.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        line.GetComponent<Renderer>().material.SetColor("_Color", parentColor);

        //darker colour for the text
        text.faceColor = inactiveColor;

    }
}
