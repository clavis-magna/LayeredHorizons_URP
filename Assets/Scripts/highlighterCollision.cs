using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlighterCollision : MonoBehaviour
{
    //make sure that when changed you have a bit of time before it can be changed again.
    float m_LastPressTime;
    float m_PressDelay = 0.5f;

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GameController")
        {

            if (m_LastPressTime + m_PressDelay < Time.unscaledTime)
            {
                transform.GetComponent<getColourFromParent>().useHighlighterColour = !transform.GetComponent<getColourFromParent>().useHighlighterColour;
                m_LastPressTime = Time.unscaledTime;
            }


            Debug.Log("GameController Collided");

        }
    }
}
