using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignMeshObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject meshParent;

    ToggleComponent thisToggle;

    void Start()
    {
        thisToggle = GetComponent<ToggleComponent>();
    }

    void Update()
    {

        if (thisToggle.active)
        {
            foreach (Transform child in meshParent.transform)
            {
                child.gameObject.SetActive(true);
            }
        } else
        {
            foreach (Transform child in meshParent.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
