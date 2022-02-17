using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class getColourFromParent : MonoBehaviour
{

    public Color parentColor;

    //For what the colour is when highlighted
    //White Colour
    private Color activeColor = new Color(1.0f, 1.0f, 1.0f);

    //Only for text when not highlighted at all
    //Almost black colour
    private Color inactiveColor = new Color(0.1f, 0.1f, 0.1f);

    public bool useHighlighterColour = false;

    private GameObject point;
    private GameObject line;

    private GameObject Can;


    private TextMeshPro text;

    //Graphic m_Graphic;

    public Image Indicator;



    void Start()
    {
        //Fetch the Graphic from the GameObject
        //m_Graphic = GetComponent<Graphic>();
        //Indicator = GetComponent<Image>();

        point = transform.Find("Point").gameObject;
        line = point.transform.Find("MarkerLine").gameObject;


        //adding the changing markerline length because I can't be bothered to make another script yet for a UI I don't know if I like or not.
        //This changed the line to be longer at the bottom and not increase at the top
        float lineLength = transform.position.y;
        line.transform.Translate(0, -lineLength, 0);
        line.transform.localScale = line.transform.localScale + new Vector3(0, lineLength, 0);

        text = transform.Find("textMesh").GetComponent<TextMeshPro>();
    }

    void Update()
    {
        //if (useHighlighterColour)
        //{
        //    parentColor = activeColor;
        //} else
        //{
        //    parentColor = transform.parent.GetComponent<Renderer>().material.GetColor("_BaseColor");
        //}
        //point.GetComponent<Renderer>().material.SetColor("_Color", parentColor);
        parentColor = transform.parent.GetComponent<Renderer>().material.GetColor("_BaseColor");
        //Indicator.GetComponent<Image>().color = parentColor;

        //m_Graphic.color = parentColor;
        Color parentHue = new Color(parentColor.r, parentColor.g, parentColor.b);

        Indicator.color = parentHue;

        line.GetComponent<Renderer>().material.SetColor("_Color", inactiveColor);


        //darker colour for the text
        text.faceColor = inactiveColor;

    }
}
