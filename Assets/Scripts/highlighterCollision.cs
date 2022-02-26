using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlighterCollision : MonoBehaviour
{
    //make sure that when changed you have a bit of time before it can be changed again.
    //float m_LastPressTime;
    //float m_PressDelay = 0.5f;

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "GameController")
        {
            transform.GetComponent<getColourFromParent>().useHighlighterColour = true;

            //if (m_LastPressTime + m_PressDelay < Time.unscaledTime)
            //{
            //    m_LastPressTime = Time.unscaledTime;
            //}


            //Debug.Log("GameController Collided");

        }
    }
}
