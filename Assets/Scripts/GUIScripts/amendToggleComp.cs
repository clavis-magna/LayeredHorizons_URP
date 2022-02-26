using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class amendToggleComp : MonoBehaviour
{
    public bool selectedToggle;

    public InputActionAsset actionAsset;

    //get the text that displays.
    //Would put this in another script but I think it can help indicate things for different interaction types.
    TextMeshProUGUI toggleNameText;
    //TextMeshProUGUI toggleStatusText;


    public Color activeColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    public Color inactiveColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);

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

    private bool toggleText;

    ToggleComponent thisToggle;

    void Start()
    {
        toggleNameText = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();


        //Find the action map so that we can reference each of the references inside
        //this one is for right controller only.
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");
        rightControllerMap.Enable();

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");
        leftControllerMap.Enable();

        //this should read the toggle name instead of instructions
        toggleNameText.text = "Grip and drag to change the slider";

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

        thisToggle = GetComponent<ToggleComponent>();
    }

    void Update()
    {
        toggleNameText.text = (thisToggle.toggleName);

        //If selected toggle allow visually show and allow amendment to toggle too.
        if (selectedToggle)
        {
            toggleNameText.color = activeColor;
        }
        else
        {
            toggleNameText.color = inactiveColor;
        }
    }

    private void onDestroy()
    {
        activateDragInputRef.action.performed -= activateChangeValue;
        activateDragInputRef.action.canceled -= deactivateChangeValue;

    }




    //this is for the click drag
    private void activateChangeValue(InputAction.CallbackContext context)
    {
        if (selectedToggle)
        {
            thisToggle.ToggleAlternate();
        }

        //get the conroller's position too from when first pressed.
        startControllerPosition = controllerPositionXYZ.y;

    }

    private void deactivateChangeValue(InputAction.CallbackContext context)
    {
        //GetComponent<SliderComponent>().DefineValue(displayedValue);
    }

    private void getControllerPosition(InputAction.CallbackContext context)
    {
        controllerPositionXYZ = context.ReadValue<Vector3>();
        //print("Controller: " + controllerPositionXYZ);
    }
}
