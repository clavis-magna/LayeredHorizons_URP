using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//script pulled from here
//https://forum.unity.com/threads/unity-support-for-openxr-in-preview.1023613/page-5#post-7046953

public class Haptics : MonoBehaviour
{
    public InputActionReference action;
    public float _amplitude = 1.0f;
    public float _duration = 0.1f;
    
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            // If the action that was performed was on a XRController device then rumble
            if (ctx.control.device is XRController device)
                Rumble(device);
        };
    }

    /// <summary>
    /// Send a rumble command to a device
    /// </summary>
    /// <param name="device">Device to send rumble to</param>
    private void Rumble(InputDevice device)
    {
        // Setting channel to 1 will work in 1.1.1 but will be fixed in future versions such that 0 would be the correct channel.
        var channel = 0;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(channel, _amplitude, _duration);
        device.ExecuteCommand(ref command);
    }

    /// <summary>
    /// Function that demonstrates rumbling the right hand controller 
    /// </summary>
    private void RumbleRight()
    {
        UnityEngine.InputSystem.InputSystem.GetDevice<XRController>(CommonUsages.RightHand);
    }

    /// <summary>
    /// Function that demonstrates rumbling the left hand controller 
    /// </summary>
    private void RumbleLeft()
    {
        UnityEngine.InputSystem.InputSystem.GetDevice<XRController>(CommonUsages.LeftHand);
    }
}
