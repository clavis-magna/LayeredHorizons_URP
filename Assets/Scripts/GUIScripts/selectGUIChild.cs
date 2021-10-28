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

    //get the thumbsticks
    private InputAction getRightThumbstick;
    private InputAction getLeftThumbstick;

    private Vector2 ThumbPosition;

    private int activeToggle;

    private float buttonHitAgainTime = 0.5f;
    private float canHitAgain;


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
            case handSelect.Right:

                //get the thumbstick action
                getRightThumbstick = rightControllerMap.FindAction("Thumbstick");
                getRightThumbstick.performed += context => getRightControllerThumb(context);
                break;

            case handSelect.Left:

                getLeftThumbstick = leftControllerMap.FindAction("Thumbstick");
                getLeftThumbstick.performed += context => getLeftControllerThumb(context);
                break;
        }
    }

    void Update()
    {

        //loop through each of the toggles to tell it to turn off if it isn't the active toggle
        for (int i = 0; i < transform.childCount; i++)
        {
            //print("value of i : " + i);
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

        if (ThumbPosition.y > 0.8 && canHitAgain < Time.time)
        {
            canHitAgain = Time.time + buttonHitAgainTime;
            activeToggle -= 1;
        }

        if (ThumbPosition.y < -0.8 && canHitAgain < Time.time)
        {
            canHitAgain = Time.time + buttonHitAgainTime;
            activeToggle += 1;
        }


        //loop through so you can only select from the 4 toggles. Add more here if there are more toggles
        if (activeToggle > transform.childCount)
        {
            activeToggle = 0;
        }
        if (activeToggle < 0)
        {
            activeToggle = transform.childCount;
        }

    }

    private void onDestroy()
    {
        getRightThumbstick.performed -= context => getRightControllerThumb(context);
        getLeftThumbstick.performed -= context => getLeftControllerThumb(context);
    }

    private void getRightControllerThumb(InputAction.CallbackContext context)
    {
        ThumbPosition = context.ReadValue<Vector2>();
    }

    private void getLeftControllerThumb(InputAction.CallbackContext context)
    {
        ThumbPosition = context.ReadValue<Vector2>();
    }
}
