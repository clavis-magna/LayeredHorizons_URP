using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayToggle : MonoBehaviour
{
    //also get the gameobject so that we can check if it's active.
    public GameObject toggleObject;

    //this just receives the toggle part of the game object
    public ToggleComponent toggleInput;

    TextMeshProUGUI textDisplay;

    void Update()
    {
        bool toggleActive = toggleInput.active;

        textDisplay = GetComponent<TextMeshProUGUI>();


        //know if the gameobject is active or not and change the text colour so that we know which one we are changing.
        if (toggleObject.activeSelf)
        {
            textDisplay.color = new Color(1, 1, 1, 1.0f);
        }
        else
        {
            textDisplay.color = new Color(1, 1, 1, 0.1f);
        }

        //this is for checking if the toggle is active or not
        if (toggleActive)
        {
            textDisplay.text = toggleInput.toggleName + " : " + toggleInput.toggleOnString;
        }
        else
        {
            textDisplay.text = toggleInput.toggleName + " : " + toggleInput.toggleOffString;
        }


    }
}
