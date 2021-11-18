using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum handSelect
{
    Left,
    Right
}


public class attachGUIToHands : MonoBehaviour
{
    //add the XR Default Input Action to this
    public InputActionAsset actionAsset;

    //get position of camera to have GUI face the player.
    //not headset because if the player moves it does not include that in headset position
    public GameObject m_CameraGameObject;

    //using an actionmap to reduce the number of references on this page
    private InputActionMap rightControllerMap;
    private InputActionMap leftControllerMap;

    //Input actions for position and rotation
    private InputAction getRightPosition;
    private InputAction getLeftPosition;

    Vector3 leftPositionXYZ;
    Vector3 rightPositionXYZ;


    private InputAction getRightRotation;
    private InputAction getLeftRotation;

    Quaternion leftRotation;
    Quaternion rightRotation;

    //[Header("Button to reveal GUI, leave blank for always on")]


    //Input actions for activating hand GUI
    //private InputAction rightGUIActivationInput;
    //private InputAction leftGUIActivationInput;


    //private bool LeftGUIActive;
    //private bool RightGUIActive;

    //Attach GUI objects to these
    [Header("Attach GUI Here")]

    public GameObject LeftHandGUI;
    public GameObject RightHandGUI;






    //adjusts y position of all the UI elements. 1.4f is on top of controller.
    [Header("Y Position of the GUI for both hands")]

    public float yPositionOffset = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        //Find the action map so that we can reference each of the references inside
        //this one is for right controller only.
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");
        rightControllerMap.Enable();

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");
        leftControllerMap.Enable();


        //Find the actions within the actionmaps

        //POSITION
        getRightPosition = rightControllerMap.FindAction("Position");
        getLeftPosition = leftControllerMap.FindAction("Position");

        getRightPosition.performed += context => getRightControllerPosition(context);
        getLeftPosition.performed += context => getLeftControllerPosition(context);

        //ROTATION
        getRightRotation = rightControllerMap.FindAction("Rotation");
        getLeftRotation = leftControllerMap.FindAction("Rotation");

        getRightRotation.performed += context => getRightControllerRotation(context);
        getLeftRotation.performed += context => getLeftControllerRotation(context);

        //activate the GUI for each button
        //rightGUIActivationInput = rightControllerMap.FindAction("Select");
        //leftGUIActivationInput = leftControllerMap.FindAction("Select");

        //leftGUIActivationInput.performed += context => LeftHandGripped(context);
        //leftGUIActivationInput.canceled += context => LeftHandReleased(context);

        //rightGUIActivationInput.performed += context => RightHandGripped(context);
        //rightGUIActivationInput.canceled += context => RightHandReleased(context);

    }

    // Update is called once per frame
    void Update()
    {
        //if (LeftGUIActive)
        //{
            LeftHandGUI.transform.position = new Vector3(leftPositionXYZ.x, leftPositionXYZ.y + yPositionOffset, leftPositionXYZ.z) + transform.position;
            LeftHandGUI.transform.LookAt(m_CameraGameObject.transform.position);
        //}
        //else
        //{
        //    //send this to some random place off the map
        //    LeftHandGUI.transform.position = new Vector3(0, 0, 0);
        //}



        //if (RightGUIActive)
        //{
            //print("rightHandPos : " + RightHandGUI.transform.position);
            RightHandGUI.transform.position = new Vector3(rightPositionXYZ.x, rightPositionXYZ.y + yPositionOffset, rightPositionXYZ.z) + transform.position;
            RightHandGUI.transform.LookAt(m_CameraGameObject.transform.position);
        //}
        //else
        //{
        //    //send this to some random place off the map
        //    RightHandGUI.transform.position = new Vector3(0, 0, 0);
        //}
        
    }

    private void onDestroy()
    {
        getRightPosition.performed -= context => getRightControllerPosition(context);
        getLeftPosition.performed -= context => getLeftControllerPosition(context);

        getRightRotation.performed -= context => getRightControllerRotation(context);
        getLeftRotation.performed -= context => getLeftControllerRotation(context);

        //rightGUIActivationInput.performed -= context => LeftHandGripped(context);
        //rightGUIActivationInput.canceled -= context => LeftHandReleased(context);

        //leftGUIActivationInput.performed -= context => RightHandGripped(context);
        //leftGUIActivationInput.canceled -= context => RightHandReleased(context);
    }

    private void getLeftControllerPosition(InputAction.CallbackContext context)
    {
        leftPositionXYZ = context.ReadValue<Vector3>();
    }

    private void getRightControllerPosition(InputAction.CallbackContext context)
    {
        rightPositionXYZ = context.ReadValue<Vector3>();
    }

    private void getLeftControllerRotation(InputAction.CallbackContext context)
    {
        leftRotation = context.ReadValue<Quaternion>();
    }

    private void getRightControllerRotation(InputAction.CallbackContext context)
    {
        rightRotation = context.ReadValue<Quaternion>();
    }

    //private void LeftHandGripped(InputAction.CallbackContext context)
    //{
    //    LeftGUIActive = true;
    //}

    //private void LeftHandReleased(InputAction.CallbackContext context)
    //{
    //    LeftGUIActive = false;
    //}

    //private void RightHandGripped(InputAction.CallbackContext context)
    //{
    //    RightGUIActive = true;
    //}

    //private void RightHandReleased(InputAction.CallbackContext context)
    //{
    //    RightGUIActive = false;
    //}
}
