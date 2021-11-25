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
        //active toggle should always remain as the first one. Sibling order will change instead so that the position changes too
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
                //check if it has an amendToggleComp script inside first
                if ((child.GetComponent("amendToggleComp") as amendToggleComp) != null)
                {
                    child.GetComponent<amendToggleComp>().selectedToggle = true;
                }

                //check if it has an amendSliderComp script inside first
                if ((child.GetComponent("amendSliderComp") as amendSliderComp) != null)
                {
                    child.GetComponent<amendSliderComp>().selectedSlider = true;
                }
            }
            else
            {
                if ((child.GetComponent("amendToggleComp") as amendToggleComp) != null)
                {
                    child.GetComponent<amendToggleComp>().selectedToggle = false;
                }

                if ((child.GetComponent("amendSliderComp") as amendSliderComp) != null)
                {
                    child.GetComponent<amendSliderComp>().selectedSlider = false;
                }
            }
        }

        //loop through so you can only select from the toggles.
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

        ////get the first child and send it to the back.
        //GameObject child = transform.GetChild(0).gameObject;
        //child.transform.SetSiblingIndex(transform.childCount-1);

        ////reposition anytime that the order is changed
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    GameObject thisChild = transform.GetChild(i).gameObject;
        //    thisChild.GetComponent<positionFromChildCount>().RepositionChildren(i);
        //    print("child" + i + "position: " + thisChild.transform.position);

        //}
    }

    private void secondaryPressed(InputAction.CallbackContext context)
    {
        //print("Secondary pressed");

        activeToggle++;
        //GameObject child = transform.GetChild(transform.childCount-1).gameObject;
        //child.transform.SetSiblingIndex(0);

        ////reposition anytime that the order is changed
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    GameObject thisChild = transform.GetChild(i).gameObject;
        //    thisChild.GetComponent<positionFromChildCount>().RepositionChildren(i);
        //    print("child" + i + "position: " + thisChild.transform.position);

        //}
    }

}
