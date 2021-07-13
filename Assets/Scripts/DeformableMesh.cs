using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{

    public float maximumDepression;
    public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    private GeneratePlaneMesh plane;



    //called by GeneratePlaneMesh
    //anytime the mesh needs to be regenerated this is called.
    public void MeshRegenerated()
    {
      plane = GetComponent<GeneratePlaneMesh>();
      plane.mesh.MarkDynamic();
      //references to the mesh, mark as dynamic so that they can change and affect all lists of vertices
      originalVertices = plane.mesh.vertices.ToList();
      modifiedVertices = plane.mesh.vertices.ToList();
      Debug.Log("Mesh Regenerated");


    }


    //This is called in the physics deformer which detects collision from the Collider
    //It then references this add depression which refreshes the values in the mesh.
    public void AddDepression(Vector3 depressionPoint, float radius)
    {
      //translate the depression relative to the worldspace and to the point of contact
      //creating a vector3 out of vec4
      var worldPos4 = this.transform.InverseTransformPoint(depressionPoint);

      var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);

      for (int i = 0; i < modifiedVertices.Count; ++i)
      {
        //distance is detecting which pixels in the x and z that need to be impacted
        var distance = (worldPos - (modifiedVertices[i] + new Vector3(0.0f, -1.0f, 0.0f))).magnitude;

        //Detecting the impact radius in which the vertices of the mesh in that radius will be remade.
        if (distance < radius)
        {
          //newVert is by how much do these vertices change
          var newVert = modifiedVertices[i] - new Vector3(0.0f, -1.0f, 0.0f) * maximumDepression;
          //remove the old and bring in the newVert coord
          modifiedVertices.RemoveAt(i);
          modifiedVertices.Insert(i, newVert);
        }



      }

      plane.mesh.SetVertices(modifiedVertices);
      // Debug.Log("Mesh Depressed");

    }

}
