using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createAdjustmentGUI : MonoBehaviour
{
    public GameObject adjustmentLayer;
    public GameObject colourAdjustment;
    public GameObject opacityAdjustment;
    public GameObject textToggle;

    public void createAdjustmentLayer(GameObject meshParent, string layerName, bool createColGO, bool createOpaGO)
    {
        //Create the parent layer that will allow you to move through the menu items
        var newLayer = Instantiate(adjustmentLayer, transform);
        newLayer.name = layerName;

        if (createColGO)
        {
            createColourAdjustmentChild(newLayer, meshParent);
        }
        if (createOpaGO)
        {
            createOpacityAdjustmentChild(newLayer, meshParent);
        }

        //This toggle is created to turn on and off the text labels on a layer
        var newToggle = Instantiate(textToggle, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
        newToggle.transform.Translate(0.2f, 0, 0);
        newToggle.transform.parent = newLayer.transform;

        //Assigning things like the mesh to associate with and what the label says here
        assignMeshText toggleTextScript = newToggle.GetComponent<assignMeshText>();
        toggleTextScript.meshParent = meshParent;

        ToggleComponent toggleCompScript = newToggle.GetComponent<ToggleComponent>();
        toggleCompScript.toggleName = "Toggle Labels";

    }

    public void createColourAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //create a GUI element that allows you to change the colour of the mesh
        var newColourAdjust = Instantiate(colourAdjustment, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        newColourAdjust.transform.Translate(-0.06f, 0, 0);
        var colourScript = newColourAdjust.GetComponent<amendMeshColourFromSlider>();
        colourScript.meshParent = meshObject;
    }

    public void createOpacityAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        //create a GUI element that allows you to change the opacity of the mesh
        var newOpacityAdjust = Instantiate(opacityAdjustment, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, parentLayer.transform);
        var opacityScript = newOpacityAdjust.GetComponent<amendOpacityFromSlider>();
        opacityScript.meshParent = meshObject;
    }
}
