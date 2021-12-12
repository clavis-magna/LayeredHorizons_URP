using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignMeshText : MonoBehaviour
{

    [HideInInspector]
    public GameObject mesh;

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
            for (int i = 0; i < mesh.transform.childCount; i++)
            {
                mesh.transform.GetChild(i).gameObject.SetActive(true);
                //child.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < mesh.transform.childCount; i++)
            {
                mesh.transform.GetChild(i).gameObject.SetActive(false);
                //child.SetActive(true);
            }
        }
    }
}
