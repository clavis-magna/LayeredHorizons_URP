using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlighterCollision : MonoBehaviour
{
    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GameController")
        {
            //getColourFromParent.parentColor = 
            transform.GetComponent<getColourFromParent>().useHighlighterColour = !transform.GetComponent<getColourFromParent>().useHighlighterColour;

            Debug.Log("GameController Collided");

        }
    }
}
