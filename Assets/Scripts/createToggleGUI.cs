using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;
using System.Threading.Tasks;



//get the number of mesh objects out there and create toggles for each of them
public class createToggleGUI : MonoBehaviour
{

    public GameObject toggleObject;
    public Camera mainCam;

    int counter = 0;

    public void createToggleObject(GameObject meshParent, string meshName)
    {
        //Instantiate the GO and move it to your hand and not overlapping the last
        var newToggle = Instantiate(toggleObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
        newToggle.transform.Translate(0, (float)counter / 20.0f, 0);

        //Assign stuff to the right script
        assignMeshObject toggleMeshScript = newToggle.GetComponent<assignMeshObject>();
        toggleMeshScript.meshParent = meshParent;

        ToggleComponent toggleCompScript = newToggle.GetComponent<ToggleComponent>();
        toggleCompScript.toggleName = meshName;

        Canvas textCanvas = newToggle.GetComponent<Canvas>();
        textCanvas.renderMode = RenderMode.WorldSpace;
        textCanvas.worldCamera = mainCam;

        newToggle.name = meshName;

        counter++;
    }
}
