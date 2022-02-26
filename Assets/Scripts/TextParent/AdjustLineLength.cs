using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustLineLength : MonoBehaviour
{

    //line is the line next to the text label helps to guide the eyes to where the label sits.
    public GameObject line;

    void Start()
    {
        //This changed the line to be longer at the bottom and not increase at the top
        float lineLength = transform.position.y;
        line.transform.Translate(0, -lineLength, 0);
        line.transform.localScale = line.transform.localScale + new Vector3(0, lineLength, 0);
    }


}
