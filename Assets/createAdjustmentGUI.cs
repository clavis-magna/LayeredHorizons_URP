using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createAdjustmentGUI : MonoBehaviour
{
    public GameObject adjustmentLayer;

    public GameObject colourAdjustment;
    public GameObject opacityAdjustment;


    public void createAdjustmentLayer(GameObject meshObject, string layerName, bool createColGO, bool createOpaGO, bool createHighGO)
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
        print("created colour adjustment");
        var newColourAdjust = Instantiate(colourAdjustment, parentLayer.transform);
        var colourScript = newColourAdjust.GetComponent<amendMeshColourFromSlider>();
        colourScript.mesh = meshObject;
    }

    public void createOpacityAdjustmentChild(GameObject parentLayer, GameObject meshObject)
    {
        print("created opacity adjustment");
        var newOpacityAdjust = Instantiate(opacityAdjustment, parentLayer.transform);
        var opacityScript = newOpacityAdjust.GetComponent<amendOpacityFromSlider>();
        opacityScript.mesh = meshObject;
    }

    public void createHighlighter(GameObject parentLayer, GameObject meshObject)
    {
        print("created highlighter adjustment");
    }
}
