using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;
using System.Threading.Tasks;



//get the number of mesh objects out there and create toggles for each of them
public class createToggleGUI : MonoBehaviour
{

    public GameObject toggleObject;

    public void createToggleObject(GameObject meshObject, string meshName)
    {
        Debug.Log("called to create Toggle");
        var newToggle = Instantiate(toggleObject, transform);
        assignMeshObject toggleMeshScript = newToggle.GetComponent<assignMeshObject>();
        ToggleComponent toggleCompScript = newToggle.GetComponent<ToggleComponent>();
        toggleMeshScript.mesh = meshObject;

        toggleCompScript.toggleName = meshName;
        newToggle.name = meshName;
    }
}
