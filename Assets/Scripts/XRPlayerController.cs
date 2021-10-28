using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR;

public class XRPlayerController : MonoBehaviour
{

    [Header("Read from toggle to switch movement mode")]
    public ToggleComponent toggleInput;

    public bool printLogs = true;


    [Header("Add hands and controls")]

    //attach both hand GO's here
    public GameObject leftHand;
    public GameObject rightHand;

    public string moveForwardInput = "Activate";

    public InputActionAsset actionAsset;

    //using an actionmap to reduce the number of references on this page
    private InputActionMap rightControllerMap;
    private InputActionMap leftControllerMap;

    //Input actions for position and rotation
    private InputAction rightTrigger;
    private InputAction leftTrigger;

    private float rightTriggerValue;
    private float leftTriggerValue;


    public float normalMoveSpeed = 1;
    float speedMultiplier;
    float fallbackSpeedMultiplier;

    //public InputActionReference testReference = null;


    public bool desktopControls = false;

    PlayerInputActionsFallback playerInputActionsFallback;
    Vector2 rotation = Vector2.zero;
    public float speed = 0.1f;

    private void Awake()
    {
        playerInputActionsFallback = new PlayerInputActionsFallback();
        playerInputActionsFallback.Movement.Forward.performed += Forward;
        playerInputActionsFallback.Movement.Forward.canceled += Stop;
        playerInputActionsFallback.Movement.Rotate.performed += mouseLook;
    }

    private void Start()
    {
        //Find the action map so that we can reference each of the references inside
        //this one is for right controller only.
        rightControllerMap = actionAsset.FindActionMap("XRI RightHand");

        leftControllerMap = actionAsset.FindActionMap("XRI LeftHand");

        rightTrigger = rightControllerMap.FindAction(moveForwardInput);
        leftTrigger = leftControllerMap.FindAction(moveForwardInput);

        rightTrigger.performed += context => moveForwardRight(context);
        leftTrigger.performed += context => moveForwardLeft(context);

        rightTrigger.canceled += context => stopActionRight(context);
        leftTrigger.canceled += context => stopActionLeft(context);

    }

    public void Update()
    {
        // XR movement

        if (toggleInput.active)
        {
            // go in direction pointing
            transform.position += rightHand.transform.forward * normalMoveSpeed * Time.deltaTime * rightTriggerValue;
            transform.position += leftHand.transform.forward * normalMoveSpeed * Time.deltaTime * leftTriggerValue;
        }
        else
        {
            // go in direction looking
            transform.position += Camera.main.transform.forward * normalMoveSpeed * Time.deltaTime * rightTriggerValue;
            transform.position += Camera.main.transform.forward * normalMoveSpeed * Time.deltaTime * leftTriggerValue;

        }


        // Fallback movement
        if (desktopControls)
        {
            transform.position += Camera.main.transform.forward * normalMoveSpeed * Time.deltaTime * fallbackSpeedMultiplier;
        }
    }

    private void OnEnable()
    {
        rightControllerMap.Enable();
        leftControllerMap.Enable();

        playerInputActionsFallback.Enable();
    }

    private void OnDisable()
    {
        rightControllerMap.Disable();
        leftControllerMap.Disable();

        playerInputActionsFallback.Disable();
    }



    private void OnDestroy()
    {
        rightTrigger.performed -= context => moveForwardRight(context);
        leftTrigger.performed -= context => moveForwardLeft(context);

        rightTrigger.canceled -= context => stopActionRight(context);
        leftTrigger.canceled -= context => stopActionLeft(context);
    }

    private void moveForwardRight(InputAction.CallbackContext context)
    {
        rightTriggerValue = context.ReadValue<float>();
        if (printLogs)
        {
            print(rightTriggerValue);
        }
    }

    private void moveForwardLeft(InputAction.CallbackContext context)
    {
        leftTriggerValue = context.ReadValue<float>();
        if (printLogs)
        {
            print(leftTriggerValue);
        }
    }


    private void stopActionRight(InputAction.CallbackContext context)
    {
        if (printLogs)
            print("Released");
        rightTriggerValue = 0;
    }


    private void stopActionLeft(InputAction.CallbackContext context)
    {
        if (printLogs)
            print("Released");
        leftTriggerValue = 0;
    }


    //FALLBACK ACTIONS
    private void Forward(InputAction.CallbackContext context)
    {
        if (printLogs)
            print("Forward Pressed");
        fallbackSpeedMultiplier = 1;
    }

    private void Stop(InputAction.CallbackContext context)
    {
        if (printLogs)
            print("Forward Released");
        fallbackSpeedMultiplier = 0;
    }

    private void mouseLook(InputAction.CallbackContext context)
    {
        if (desktopControls)
        {
            Vector2 mouseIn = context.ReadValue<Vector2>();
            Camera.main.transform.parent.transform.Rotate(new Vector3(0f, mouseIn.x, 0f) * speed, Space.World);
            Camera.main.transform.parent.transform.Rotate(new Vector3(0f, 0f, mouseIn.y) * speed, Space.Self);
        }
    }
}
