using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class selectGUIChild : MonoBehaviour
{
    //the handSelect enum lives in attachGUIToHands FYI
    public handSelect activeHand;

    public InputActionAsset actionAsset;


    //using an actionmap to reduce the number of references on this page
    private InputActionMap rightControllerMap;
    private InputActionMap leftControllerMap;

    private InputAction getPrimaryButton;
    private InputAction getSecondaryButton;

    public int activeToggle;



    void Start()
    {
        //beginning with the first toggle
        activeToggle = 0;

        //Find the action map so that we can reference each of the references inside
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");
        rightControllerMap.Enable();

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");
        leftControllerMap.Enable();
        switch (activeHand)
        {

            case handSelect.Left:
                getPrimaryButton = leftControllerMap.FindAction("Dpad X");
                getPrimaryButton.performed += context => primaryPressed(context);

                getSecondaryButton = leftControllerMap.FindAction("Dpad Y");
                getSecondaryButton.performed += context => secondaryPressed(context);

                break;

            case handSelect.Right:

                getPrimaryButton = rightControllerMap.FindAction("Dpad A");
                getPrimaryButton.performed += context => primaryPressed(context);

                getSecondaryButton = rightControllerMap.FindAction("Dpad B");
                getSecondaryButton.performed += context => secondaryPressed(context);

                break;
        }
    }

    void Update()
    {
        //print("active Toggle: " + activeToggle);

        //loop through each of the toggles to tell it to turn off if it isn't the active toggle
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (activeToggle == i)
            {
                child.SetActive(true);
            }
            else
            {
                child.SetActive(false);
            }

        }

        //loop through so you can only select from the 4 toggles. Add more here if there are more toggles
        if (activeToggle > transform.childCount-1)
        {
            activeToggle = 0;
        }
        if (activeToggle < 0)
        {
            activeToggle = transform.childCount-1;
        }

    }

    private void onDestroy()
    {
        getPrimaryButton.performed -= context => primaryPressed(context);
        getSecondaryButton.performed -= context => secondaryPressed(context);
    }

    private void primaryPressed(InputAction.CallbackContext context)
    {
        //print("Primary pressed");

        activeToggle--;
    }

    private void secondaryPressed(InputAction.CallbackContext context)
    {
        //print("Secondary pressed");

        activeToggle++;
    }

}
