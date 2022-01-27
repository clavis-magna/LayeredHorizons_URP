using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createAdjustmentGUI : MonoBehaviour
{
    public GameObject adjustmentLayer;

    public GameObject colourAdjustment;
    public GameObject opacityAdjustment;
    public GameObject textToggle;


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

        var newToggle = Instantiate(textToggle, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
        newToggle.transform.Translate(0.2f, 0, 0);

        newToggle.transform.parent = newLayer.transform;


        assignMeshText toggleTextScript = newToggle.GetComponent<assignMeshText>();
        ToggleComponent toggleCompScript = newToggle.GetComponent<ToggleComponent>();
        //assign the mesh
        toggleTextScript.mesh = meshObject;
        ////change the name in the toggle
        toggleCompScript.toggleName = "Toggle Labels";
        ////change the GO name.
        //newToggle.name = meshName;


    }

    public void createColourAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //print("created colour adjustment");
        var newColourAdjust = Instantiate(colourAdjustment, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        newColourAdjust.transform.Translate(-0.06f, 0, 0);
        var colourScript = newColourAdjust.GetComponent<amendMeshColourFromSlider>();
        colourScript.mesh = meshObject;
        //counter++;

    }

    public void createOpacityAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //print("created opacity adjustment");
        var newOpacityAdjust = Instantiate(opacityAdjustment, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        var opacityScript = newOpacityAdjust.GetComponent<amendOpacityFromSlider>();
        opacityScript.mesh = meshObject;
        //counter++;

    }
}
