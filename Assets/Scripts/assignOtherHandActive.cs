using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script takes the other hand and will make the same child object active.
public class assignOtherHandActive : MonoBehaviour
{
    public GameObject otherHandGUI;




    // Update is called once per frame
    void Update()
    {

        //this is just looping based on the assumption that the number of children under both GUI's are the same.
        //hope this holds up...
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (otherHandGUI.GetComponent<selectGUIChild>().activeToggle == i)
            {
                child.SetActive(true);
            }
            else
            {
                child.SetActive(false);
            }

        }
    }
}
