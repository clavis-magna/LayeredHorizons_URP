using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class amendSliderComp : MonoBehaviour
{
    public InputActionAsset actionAsset;
    //get the text that displays.
    //Would put this in another script but I think it can help indicate things for different interaction types.
    TextMeshProUGUI textDisplay;

    //using an actionmap to reduce the number of references on this page
    private InputActionMap rightControllerMap;
    private InputActionMap leftControllerMap;

    [Header("Select Hand and Action to Activate")]
    public handSelect activeHand;
    public InputActionReference activateDragInputRef;

    private InputAction getRightPosition;
    private InputAction getLeftPosition;

    private Vector3 controllerPositionXYZ;


    private float startControllerPosition;
    private float endControllerPosition;

    private float sliderValue;
    private float displayedValue;
    private float movementAmount;

    private bool clickDragActive;

    SliderComponent thisSlider;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = GetComponentInChildren<TextMeshProUGUI>();

        //Find the action map so that we can reference each of the references inside
        //this one is for right controller only.
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");
        rightControllerMap.Enable();

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");
        leftControllerMap.Enable();

        //this should read the toggle name instead of instructions
        textDisplay.text = "Grip and drag to change the slider";

        //tracking hand position depending on which hand you're on
        switch (activeHand)
        {
            case handSelect.Right:
                getRightPosition = rightControllerMap.FindAction("Position");
                getRightPosition.performed += context => getControllerPosition(context);
                break;

            case handSelect.Left:
                getLeftPosition = leftControllerMap.FindAction("Position");
                getLeftPosition.performed += context => getControllerPosition(context);
                break;
        }

        //checking if the grip has been pressed
        activateDragInputRef.action.performed += activateChangeValue;
        activateDragInputRef.action.canceled += deactivateChangeValue;

        thisSlider = GetComponent<SliderComponent>();

    }

    // Update is called once per frame
    void Update()
    {

        if (clickDragActive)
        {
            endControllerPosition = controllerPositionXYZ.y;

            //get the distance that controller moved from press to release.
            movementAmount = startControllerPosition - endControllerPosition;

            //this value gets placed on the text
            displayedValue = sliderValue - movementAmount;

            //make sure it can't get below or above 1
            if (displayedValue > 1)
            {
                displayedValue = 1;
            }
            if (displayedValue < 0)
            {
                displayedValue = 0;
            }

            //as the text changes, the changes are live.
            GetComponent<SliderComponent>().DefineValue(displayedValue);
        }

        //text is indicated by what is on displayedValue
        //convert it to a string and move to 2 decible places
        textDisplay.text = displayedValue.ToString("F2");
    }


    private void onDestroy()
    {
        activateDragInputRef.action.performed -= activateChangeValue;
        activateDragInputRef.action.canceled -= deactivateChangeValue;
    }




    //this is for the click drag
    private void activateChangeValue(InputAction.CallbackContext context)
    {
        //when pressed have this active which will keep recording controller positions.
        clickDragActive = true;

        //slider value is what you receive back from the slider Component
        //replaced each time pressed so that it can tell what is the starting value to change from.
        sliderValue = GetComponent<SliderComponent>().value;


        //get the conroller's position too from when first pressed.
        startControllerPosition = controllerPositionXYZ.y;

    }

    private void deactivateChangeValue(InputAction.CallbackContext context)
    {
        clickDragActive = false;

        //////define the slider by the movement when the controller position was moved.
        ////float newSliderValue = Mathf.Abs(startControllerPosition.y - endControllerPosition.y);
        GetComponent<SliderComponent>().DefineValue(displayedValue);

    }

    private void getControllerPosition(InputAction.CallbackContext context)
    {
        controllerPositionXYZ = context.ReadValue<Vector3>();
        //print("Controller: " + controllerPositionXYZ);
    }
}
