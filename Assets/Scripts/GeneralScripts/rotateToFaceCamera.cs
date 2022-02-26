using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToFaceCamera : MonoBehaviour
{

    //get position of camera to have GUI face the player.
    //not headset because if the player moves it does not include that in headset position
    public GameObject m_CameraGameObject;

    // Start is called before the first frame update
    void Start()
    {
        m_CameraGameObject = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_CameraGameObject.transform.position);
    }
}
