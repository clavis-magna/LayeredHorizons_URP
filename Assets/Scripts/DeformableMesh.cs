using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{

    public float maximumDepression;
    // public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    public List<Vector2> convertedVectors;

    private GeneratePlaneMesh plane;


    //called by GeneratePlaneMesh
    //anytime the mesh needs to be regenerated this is called.
    public void MeshRegenerated()
    {
      plane = GetComponent<GeneratePlaneMesh>();
      plane.mesh.MarkDynamic();
      // //references to the mesh, mark as dynamic so that they can change and affect all lists of vertices
      // originalVertices = plane.mesh.vertices.ToList();
      modifiedVertices = plane.mesh.vertices.ToList();

      //extension that converts the vec3 to a vec2 for use in the depression.
      //Gets the x and z of the modified mesh
      //sorry very hacked together
      Vector2[] modifiedVec2 = modifiedVertices.toVector2();
      for (int i = 0; i < modifiedVec2.Length; i++)
      {
        convertedVectors.Add(modifiedVec2[i]);
      }

      Debug.Log("Mesh Regenerated");
    }


    //This is called in the physics deformer which detects collision from the Collider
    //It then references this add depression which refreshes the values in the mesh.
    public void AddDepression(Vector3 depressionPoint, float radius)
    {
      Debug.Log("Depression Added");


      //translate the depression relative to the worldspace and to the point of contact
      //creating a vector3 out of vec4
      var worldPos4 = this.transform.InverseTransformPoint(depressionPoint);

      //not needed anymore
      //worldPos is where the depressionPoint is
      // var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);

      //removal of the y position of impact
      var worldPos2 = new Vector2(worldPos4.x, worldPos4.z);

      for (int i = 0; i < modifiedVertices.Count; ++i)
      {

        //distance is detecting which pixels in the x and z and y that need to be impacted
        var distance = (worldPos2 - (convertedVectors[i])).magnitude;

        //Detecting the impact radius in which the vertices of the mesh in that radius will be remade.
        if (distance < radius)
        {
          //newVert is by how much do these vertices change
          var newVert = modifiedVertices[i] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression;

          //vertices to the side of the depression point are also shifted but less. The position number is +1 -1 +MeshSize+1 -MeshSize-1
          var newVertLeft = modifiedVertices[i-1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
          var newVertRight = modifiedVertices[i+1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
          var newVertTop = modifiedVertices[i-plane.gridSize-1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
          var newVertBot = modifiedVertices[i+plane.gridSize+1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;

          //remove the old and bring in the newVert coord
          modifiedVertices.RemoveAt(i+1);
          modifiedVertices.Insert(i+1, newVertRight);

          modifiedVertices.RemoveAt(i-1);
          modifiedVertices.Insert(i-1, newVertLeft);

          modifiedVertices.RemoveAt(i-11);
          modifiedVertices.Insert(i-11, newVertTop);

          modifiedVertices.RemoveAt(i+plane.gridSize+1);
          modifiedVertices.Insert(i+plane.gridSize+1, newVertBot);

          modifiedVertices.RemoveAt(i);
          modifiedVertices.Insert(i, newVert);
        }



      }


      plane.mesh.SetVertices(modifiedVertices);

      // originalVertices = modifiedVertices;
    }

}
