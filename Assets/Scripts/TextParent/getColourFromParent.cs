using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class getColourFromParent : MonoBehaviour
{

    public Color parentColor;

    //White Colour
    private Color whiteCol = new Color(1.0f, 1.0f, 1.0f);

    //Almost black colour
    private Color blackCol = new Color(0.1f, 0.1f, 0.1f);

    //Highlighter is used to touch the text labels and change the colour to highlight important data points.
    //I've turned off the use for this for now but this is a feature that can be added for future versions
    public bool useHighlighterColour = false;

    //line is the line next to the text label helps to guide the eyes to where the label sits.
    public GameObject line;

    //This is used to set the colour of the text. Currently used to just set the text black but can be used later on for highlighter feature.
    public TextMeshPro text;

    public TextMeshPro textBG;


    //This indicator helps to see which mesh the text label belongs to by setting the colour the same.
    public Image Indicator;



    void Start()
    {
        line.GetComponent<Renderer>().material.SetColor("_Color", blackCol);
    }

    void Update()
    {
        if (useHighlighterColour)
        {
            text.faceColor = whiteCol;

            textBG.text = $"<mark=#000000 padding='130,40,20,20'>{text.text}</mark>";
        }
        else
        {
            //darker colour for the text
            text.faceColor = blackCol;

            //NOTE: The textBG colour is set from the markup inside the text so this is set by a hex value inside the DeformableMesh script
            //This is the background that is behind the text labels
            //It uses a markup and then the same text behind it so that the length is dynamic with the length of the text
            textBG.text = $"<mark=#ffffff padding='130,40,20,20'>{text.text}</mark>";
        }

        parentColor = transform.parent.GetComponent<Renderer>().material.GetColor("_BaseColor");

        //Using the hue for the indicator because the opacity of the parentcolor could change.
        Color parentHue = new Color(parentColor.r, parentColor.g, parentColor.b);
        Indicator.color = parentHue;



    }
}
