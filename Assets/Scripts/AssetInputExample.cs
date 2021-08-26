using UnityEngine;
using UnityEngine.InputSystem;

public class AssetInputExample : MonoBehaviour
{
    public bool printStuff = true;
    public GameObject hand;
    public float normalMoveSpeed = 1;
    public InputActionReference testReference = null;
    float speedMultiplier;

    private void Start()
    {
        testReference.action.started += DoPressedThing;
        testReference.action.performed += DoChangeThing;
        testReference.action.canceled += DoReleasedThing;
    }

    public void Update()
    {
        // go in direction looking
        //transform.position += Camera.main.transform.forward * normalMoveSpeed * Time.deltaTime * speedMultiplier;
        // go in direction pointing
        transform.position += hand.transform.forward * normalMoveSpeed * Time.deltaTime * speedMultiplier;
    }

    private void OnEnable()
    {
        testReference.asset.Enable();
    }

    private void OnDisable()
    {
        testReference.asset.Disable();
    }

    private void OnDestroy()
    {
        testReference.action.started -= DoPressedThing;
        testReference.action.performed -= DoChangeThing;
        testReference.action.canceled -= DoReleasedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Pressed");
    }

    private void DoChangeThing(InputAction.CallbackContext context)
    {
        speedMultiplier = context.ReadValue<float>(); 
        
        if (printStuff) {
            print(speedMultiplier);
        }
    }

    private void DoReleasedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Released");
        speedMultiplier = 0;
    }
}
