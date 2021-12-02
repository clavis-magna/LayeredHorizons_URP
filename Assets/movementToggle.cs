using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//This script changes the toggle based on the movement of controller instead of tied to a game object.
public class movementToggle : MonoBehaviour
{

    //add the XR Default Input Action to this
    public InputActionAsset actionAsset;

    //using an actionmap to reduce the number of references on this page
    private InputActionMap rightControllerMap;
    private InputActionMap leftControllerMap;
    private InputActionMap HMDMap;

    //Input actions for position and rotation
    private InputAction getRightPosition;
    private InputAction getLeftPosition;
    private InputAction getHMDPosition;

    Vector3 leftPositionXYZ;
    Vector3 rightPositionXYZ;
    Vector3 headPositionXYZ;

    //get the XRController to apply the haptics these are to be sent to the vibration manager script
    //settings for amp and duration here too
    private InputDevice leftHand;
    private InputDevice rightHand;


    //a function to only call the vibration when the haptic boolean changes.
    //one is for switching head to hand the other is for hand to head
    //I know this isn't an efficient way of doing it could be improved.
    //https://answers.unity.com/questions/1354785/call-a-function-when-a-bool-changes-value.html
    private bool haptic1;
    public bool Haptic1
    {
        get { return haptic1; }
        set
        {
            if (value == haptic1)
                return;

            haptic1 = value;
            if (haptic1)
            {
                gameObject.GetComponent<vibrationManager>().Rumble(rightHand);
                gameObject.GetComponent<vibrationManager>().Rumble(leftHand);
                //Rumble(leftHand);
                //Rumble(rightHand);
            }
        }
    }
    private bool haptic2;
    public bool Haptic2
    {
        get { return haptic2; }
        set
        {
            if (value == haptic2)
                return;

            haptic2 = value;
            if (haptic2)
            {
                gameObject.GetComponent<vibrationManager>().Rumble(rightHand);
                gameObject.GetComponent<vibrationManager>().Rumble(leftHand);
                //Rumble(leftHand);
                //Rumble(rightHand);
            }
        }
    }

    void Start()
    {
        //Find the action map so that we can reference each of the references inside
        //this one is for right controller only.
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");
        rightControllerMap.Enable();

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");
        leftControllerMap.Enable();

        HMDMap = actionAsset.FindActionMap("XRI HMD");
        HMDMap.Enable();

        //POSITION
        getRightPosition = rightControllerMap.FindAction("Position");
        getLeftPosition = leftControllerMap.FindAction("Position");
        getHMDPosition = HMDMap.FindAction("Position");

        getRightPosition.performed += context => getRightControllerPosition(context);
        getLeftPosition.performed += context => getLeftControllerPosition(context);
        getHMDPosition.performed += context => getHeadsetPosition(context);
    }

    void Update()
    {

        //get the y distance of headset to controllers
        float rightHeadDistance = headPositionXYZ.y - rightPositionXYZ.y;
        float leftHeadDistance = headPositionXYZ.y - leftPositionXYZ.y;





        //currently based on distance between the hands and the headset
        if (leftHeadDistance < 0.8 && rightHeadDistance < 0.8)
        {
            GetComponent<ToggleComponent>().ToggleOn();
            Haptic1 = true;
            Haptic2 = false;


        }
        else
        {
            GetComponent<ToggleComponent>().ToggleOff();
            Haptic1 = false;
            Haptic2 = true;

        }
    }

    private void onDestroy()
    {
        getRightPosition.performed -= context => getRightControllerPosition(context);
        getLeftPosition.performed -= context => getLeftControllerPosition(context);
        getHMDPosition.performed -= context => getHeadsetPosition(context);
    }

    private void getLeftControllerPosition(InputAction.CallbackContext context)
    {
        leftPositionXYZ = context.ReadValue<Vector3>();
        //print("Left POS: " + leftPositionXYZ);

        //might be innefficient to put this here. It might be calling this each time position is called.
        leftHand = context.control.device;

    }

    private void getRightControllerPosition(InputAction.CallbackContext context)
    {
        rightPositionXYZ = context.ReadValue<Vector3>();
        //print("Right POS: " + rightPositionXYZ);

        //assign the controller to send haptics
        rightHand = context.control.device;
        //// If the action that was performed was on a XRController device then rumble
        //if (context.control.device is XRController device)
        //    Rumble(device);
    }

    private void getHeadsetPosition(InputAction.CallbackContext context)
    {
        headPositionXYZ = context.ReadValue<Vector3>();
        //print("Head POS: " + headPositionXYZ);
    }
}
