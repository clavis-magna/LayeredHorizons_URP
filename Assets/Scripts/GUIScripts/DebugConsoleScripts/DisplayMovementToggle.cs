using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMovementToggle : MonoBehaviour
{
    public ToggleComponent toggleInput;

    //[SerializeField]
    TextMeshProUGUI textDisplay;

    void Update()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();

        //for the output check toggle status
        if (toggleInput.active)
        {
            textDisplay.text = toggleInput.toggleOnString;
        }
        else
        {
            textDisplay.text = toggleInput.toggleOffString;

        }
    }
}
