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

    //count how many toggles get made 
    public int counter = 0;

    public void createToggleObject(GameObject meshObject, string meshName)
    {
        //Debug.Log("called to create Toggle");
        var newToggle = Instantiate(toggleObject, new Vector3(transform.position.x, transform.position.y + (float)counter/10.0f, transform.position.z), transform.rotation, transform);
        assignMeshObject toggleMeshScript = newToggle.GetComponent<assignMeshObject>();
        ToggleComponent toggleCompScript = newToggle.GetComponent<ToggleComponent>();
        Canvas textCanvas = newToggle.GetComponent<Canvas>();
        textCanvas.renderMode = RenderMode.WorldSpace;
        textCanvas.worldCamera = mainCam;

        toggleMeshScript.mesh = meshObject;
        toggleCompScript.toggleName = meshName;
        newToggle.name = meshName;

        counter++;
    }
}
