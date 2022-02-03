using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignMeshText : MonoBehaviour
{

    [HideInInspector]
    public GameObject meshParent;

    ToggleComponent thisToggle;

    // Start is called before the first frame update
    void Start()
    {
        thisToggle = GetComponent<ToggleComponent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (thisToggle.active)
        {
            foreach (Transform child in meshParent.transform)
            {
                for (int i = 0; i < child.transform.childCount; i++)
                {
                    child.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

        }
        else
        {
            foreach (Transform child in meshParent.transform)
            {
                for (int i = 0; i < child.transform.childCount; i++)
                {
                    child.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
