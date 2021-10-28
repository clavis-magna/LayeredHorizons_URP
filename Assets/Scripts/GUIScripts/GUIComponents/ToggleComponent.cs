using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleComponent : MonoBehaviour
{
    public bool active;
    public string toggleName;

    //Leave current string blank, changes on toggle
    [HideInInspector] // Hides var below
    public string currentString;

    [Header("Messages for toggle on or off")]

    public string toggleOnString;
    public string toggleOffString;

    void Update()
    {
        if (active)
        {
            currentString = toggleOnString;
        } else
        {
            currentString = toggleOffString;
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
