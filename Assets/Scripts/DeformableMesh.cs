using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{
    //moving this variable to be controlled by the deformer script
    //eventually to be controlled by the data script
    // public float maximumDepression;


    // public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    //converted vertices is just the XZ modified vertices, used to get distance without regards to the y pos.
    public List<Vector2> convertedVectors;

    public bool edgeSmoothing = true;


    private GeneratePlaneMesh plane;

    public GameObject textMarker;

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
    public void AddDepression(Vector3 depressionPoint, float radius, float maximumDepression)
    {
      //translate the depression relative to the worldspace and to the point of contact
      //creating a vector3 out of contact point that is relative to worldspace
      var worldPos3 = this.transform.InverseTransformPoint(depressionPoint);

      //removal of the y position of impact
      var worldPos2 = new Vector2(worldPos3.x, worldPos3.z);

      for (int i = 0; i < modifiedVertices.Count; ++i)
      {

        //distance is detecting which pixels in the x and z and y that need to be impacted
        var distance = (worldPos2 - (convertedVectors[i])).sqrMagnitude;

        //Detecting the impact radius in which the vertices of the mesh in that radius will be remade.
        if (distance < radius)
        {
          //newVert is by how much do these vertices change
          var newVert = modifiedVertices[i] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression;
          modifiedVertices.RemoveAt(i);
          modifiedVertices.Insert(i, newVert);

          //vertices to the side of the depression point are also shifted but less. The position number is +1 -1 +MeshSize+1 -MeshSize-1
          //helps to remove particularly spiky vertices to make smoother terrain.
          //edge smoothing creates shorter vertices around the collision points for "softer edges".
          //the result is smoother terrain
          //must adjust the depression height to be shorter if on.
          if (edgeSmoothing)
          {
            //remove the old and bring in the newVert coord
            //numbered in a clockwise direction 1 being the top

            var newVert1 = modifiedVertices[i-1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
            modifiedVertices.RemoveAt(i-1);
            modifiedVertices.Insert(i-1, newVert1);

            var newVert2 = modifiedVertices[i-plane.gridSize-2] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/3;
            modifiedVertices.RemoveAt(i-plane.gridSize-2);
            modifiedVertices.Insert(i-plane.gridSize-2, newVert2);

            var newVert3 = modifiedVertices[i-plane.gridSize-1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
            modifiedVertices.RemoveAt(i-plane.gridSize-1);
            modifiedVertices.Insert(i-plane.gridSize-1, newVert3);

            var newVert4 = modifiedVertices[i-plane.gridSize] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/3;
            modifiedVertices.RemoveAt(i-plane.gridSize);
            modifiedVertices.Insert(i-plane.gridSize, newVert4);

            var newVert5 = modifiedVertices[i+1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
            modifiedVertices.RemoveAt(i+1);
            modifiedVertices.Insert(i+1, newVert5);

            var newVert6 = modifiedVertices[i+plane.gridSize+2] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/3;
            modifiedVertices.RemoveAt(i+plane.gridSize+2);
            modifiedVertices.Insert(i+plane.gridSize+2, newVert6);

            var newVert7 = modifiedVertices[i+plane.gridSize+1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/2;
            modifiedVertices.RemoveAt(i+plane.gridSize+1);
            modifiedVertices.Insert(i+plane.gridSize+1, newVert7);

            var newVert8 = modifiedVertices[i+plane.gridSize] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression/3;
            modifiedVertices.RemoveAt(i+plane.gridSize);
            modifiedVertices.Insert(i+plane.gridSize, newVert8);
          }
        }
      }
      plane.mesh.SetVertices(modifiedVertices);
    }

    //Creates a text label at the point of impact above the mesh instead of a mesh deformation
    public void AddTextLabel(Vector3 pointOfContact, string text)
    {
      //translate the depression relative to the worldspace and to the point of contact
      //creating a vector3 out of contact point that is relative to worldspace
      var worldPos3 = this.transform.InverseTransformPoint(pointOfContact);
      //removal of the y position of impact
      var worldPos2 = new Vector2(worldPos3.x, worldPos3.z);

      var meshHeight = 5.0f;


      for (int i = 0; i < modifiedVertices.Count; ++i)
      {
        var distance = (worldPos2 - (convertedVectors[i])).sqrMagnitude;
        if (distance < 0.5)
        {
          meshHeight = modifiedVertices[i].y;
        }
      }

      GameObject thisMarker = Instantiate(textMarker, new Vector3(pointOfContact.x, meshHeight, pointOfContact.z), Quaternion.Euler(0, 0, 0));
      //Get the created textmeshpro and change the work it is displaying to match the data.
      TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
      nameText.text = text;

    }
}
