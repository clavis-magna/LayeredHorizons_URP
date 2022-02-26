using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlighterCollision : MonoBehaviour
{
    //make sure that when changed you have a bit of time before it can be changed again.
    float m_LastPressTime;
    float m_PressDelay = 0.5f;
    float n_LastPressTime;
    float n_PressDelay = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {

            if (m_LastPressTime + m_PressDelay < Time.unscaledTime)
            {
                transform.GetComponent<getColourFromParent>().useHighlighterColour = true;

                m_LastPressTime = Time.unscaledTime;
            }


            //Debug.Log("GameController IN");

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "GameController")
        {

            if (n_LastPressTime + n_PressDelay < Time.unscaledTime)
            {
                transform.GetComponent<getColourFromParent>().useHighlighterColour = false;

                n_LastPressTime = Time.unscaledTime;
            }


            //Debug.Log("GameController OUT");

        }
    }
}
