using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class vibrationManager : MonoBehaviour
{

    public float _amplitude = 1.0f;
    public float _duration = 0.1f;


    //script pulled from here
    //https://forum.unity.com/threads/unity-support-for-openxr-in-preview.1023613/page-5#post-7046953
    /// <summary>
    /// Send a rumble command to a device
    /// </summary>
    /// <param name="device">Device to send rumble to</param>
    public void Rumble(InputDevice device)
    {
        // Setting channel to 1 will work in 1.1.1 but will be fixed in future versions such that 0 would be the correct channel.
        var channel = 1;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(channel, _amplitude, _duration);
        device.ExecuteCommand(ref command);
    }

}
