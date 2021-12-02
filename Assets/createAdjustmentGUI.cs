using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createAdjustmentGUI : MonoBehaviour
{
    public GameObject adjustmentLayer;

    public GameObject colourAdjustment;
    public GameObject opacityAdjustment;


    public void createAdjustmentLayer(GameObject meshObject, string layerName, bool createColGO, bool createOpaGO)
    {
        print("create adjustment layer");
        var newLayer = Instantiate(adjustmentLayer, transform);
        newLayer.name = layerName;

        if (createColGO)
        {
            createColourAdjustmentChild(newLayer, meshObject);
        }
        if (createOpaGO)
        {
            createOpacityAdjustmentChild(newLayer, meshObject);
        }

    } 

    public void createColourAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //print("created colour adjustment");
        var newColourAdjust = Instantiate(colourAdjustment, new Vector3(transform.position.x + (float)0 / 10.0f, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        var colourScript = newColourAdjust.GetComponent<amendMeshColourFromSlider>();
        colourScript.mesh = meshObject;
        //counter++;

    }

    public void createOpacityAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //print("created opacity adjustment");
        var newOpacityAdjust = Instantiate(opacityAdjustment, new Vector3(transform.position.x + (float)1 / 10.0f, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        var opacityScript = newOpacityAdjust.GetComponent<amendOpacityFromSlider>();
        opacityScript.mesh = meshObject;
        //counter++;

    }
}
