using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleComponent : MonoBehaviour
{
    public bool active = true;
    public string toggleName;

    //Leave current string blank, changes on toggle
    [HideInInspector] // Hides var below
    public string currentString;

    [Header("Messages for toggle on or off")]

    public string toggleOnString;
    public string toggleOffString;

    Toggle thisToggle;

    void Start()
    {
        thisToggle = transform.GetChild(0).GetComponent<Toggle>();
    }

    void Update()
    {
        if (active)
        {
            if (thisToggle != null)
            {
                thisToggle.isOn = true;
            }
            currentString = toggleOnString;

        } else
        {
            currentString = toggleOffString;
            if (thisToggle != null)
            {
                thisToggle.isOn = false;
            }
        }
    }

    public void ToggleOn() {
        active = true;
    }

    public void ToggleOff()
    {
        active = false;
    }

    public void ToggleAlternate()
    {
        active = !active;
    }
}
